using Orient.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Security;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Repository.OrientImplementation
{
    public class GroupRepository : IOrientTransactionable<GroupData>, IRepository<GroupData>
    {
        /// <summary>
        /// Constructor that stores the transaccion received into a variable.
        /// </summary>
        /// <param name="transaction">Class that performs the queries</param>
        public GroupRepository(Transaction transaction)
        {
            this.Transaction = transaction;
            this.MyTransaction = transaction;
        }

        /// <summary>
        /// Property that stores the transacton
        /// </summary>
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }
        private TeamNetDB.Repository.Interfaces.ITransaction MyTransaction { get; set; }

        /// <summary>
        /// Gets all groups from database
        /// </summary>
        /// <returns>An IEnumerable collection which contains all grooups stored in database</returns>
        public IEnumerable<GroupData> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.Group, new List<ConditionValue>());
            List<GroupData> groups = this.CreateEntitiesFromDocuments(documents);
            return groups;
        }

        /// <summary>
        /// Creates a new group using the document received from database
        /// </summary>
        /// <param name="document">Document from database which has all necessary data to create a new group</param>
        /// <returns>A new Group object with the information of the document.</returns>
        public override GroupData CreateEntityFromDocument(Orient.Client.ODocument document)
        {
            GroupData group;
            string documentId = document.ORID.ToString();
            group = new GroupData()
            {
                Name = document.GetField<string>("name"),
                Description = document.GetField<string>("description"),
                State = (State)Enum.Parse(typeof(State), document.GetField<string>("state"), false),
                Id = documentId
            };
            List<User> UsersInTheGroup = ObtainUsersFromThisGroup(documentId, "member");
            List<User> Administrators = ObtainUsersFromThisGroup(documentId, "administrator");
            group.Members = UsersInTheGroup;
            group.Administrators = Administrators;
            group.IdMembers = ObtainUsersIds(UsersInTheGroup);
            group.IdAdministrators = ObtainUsersIds(Administrators);
            group.HasPermission = CheckPermission(group);

            return group;
        }

        /// <summary>
        /// Gets a single group given the id of the group.
        /// </summary>
        /// <param name="groupId">the id of the group to be recovered.</param>
        /// <returns>A group class which has the same id given</returns>
        public GroupData GetSingle(string groupId)
        {
            List<ODocument> documents = this.Transaction.QueryVertex(VertexName.Group, new List<ConditionValue> { new ConditionValue { ConditionColumn = "@rid", ValueCondition = groupId } });
            return CreateEntitiesFromDocuments(documents).Single();
        }

        /// <summary>
        /// Gets a Group given some parameters as a predicate.
        /// </summary>
        /// <param name="predicate">The parameters to filter the groups.</param>
        /// <returns>An IEnumerable collection which contains all groups that matched the filters.</returns>
        public IEnumerable<GroupData> FindBy(System.Linq.Expressions.Expression<Func<GroupData, bool>> predicate)
        {
            var allEntities = this.GetAll();
            var entitiesWithPredicate = allEntities.Where(predicate.Compile());
            return entitiesWithPredicate;
        }

        /// <summary>
        /// Inserts a new group into database with the entity an its connectors given
        /// </summary>
        /// <param name="entity">the object to be inserted</param>
        /// <param name="conectors">the connectors for the new object</param>
        /// <returns>A boolean value which returns true if the object has been inserted correctly
        /// otherwise returns false.</returns>
        public bool Insert(GroupData entity, IList<string> conectors = null)
        {
            bool created = true;
            string name = entity.Name;
            if (true)
            {
                try
                {
                    this.Transaction.InsertVertex(this.CreateDataGroup(entity),
                        VertexName.Group, this.GenerateEdges(this.CreateRid(entity)));

                    this.CreatedRolInGroup(entity);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    created = false;
                }
            }
            else
            {
                created = false;
            }
            return created;
        }

        /// <summary>
        /// Deletes a group from database with the entity.
        /// </summary>
        /// <param name="groupToBeDeleted">the object to be deleted.</param>
        /// <returns>A boolean value which returns true if the object has been deleted correctly
        /// otherwise returns false.</returns>
        public bool Delete(GroupData groupToBeDeleted)
        {
            bool isDeleted = false;
            try
            {
                List<ConditionValue> constraints = FillVertexConstraints(groupToBeDeleted.Id);
                isDeleted = this.Transaction.Delete(VertexName.Group, constraints);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                isDeleted = false;
            }
            return isDeleted;
        }

        /// <summary>
        /// Updates the entity given.
        /// </summary>
        /// <param name="entity">the object to be updated.</param>
        /// <returns>A boolean value which returns true if the object has been Updated correctly
        /// otherwise returns false.</returns>
        public bool Update(GroupData entity)
        {
            List<ConditionValue> propertiesModified = FillChangedProperties(entity);
            List<ConditionValue> classConstraints = FillVertexConstraints(entity.Id);
            return this.Transaction.Update(VertexName.Group, propertiesModified, classConstraints);
        }

        public IEnumerable<GroupData> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<GroupData> groups = this.CreateEntitiesFromDocuments(documents);
            return groups;
        }

        /// <summary>
        /// Obtains the Users for the id group given.
        /// </summary>
        /// <param name="groupId">This is the group's id to get the users for.</param>
        /// <param name="userRol">This parameter is the rol of the user to be get from database.</param>
        /// <returns>Returns a List of users that are in the group.</returns>
        private List<User> ObtainUsersFromThisGroup(string groupId, string userRol)
        {
            string traverseUsersWithRole = "select from (traverse out(" + EdgesName.HasRoleInGroup.ToString() + ") from (select from (traverse in("
                                            + EdgesName.HasGroup.ToString() + ") from "
                                            + groupId + ") where @class = '" + VertexName.RoleInGroup.ToString() + "' AND out_"
                                            + EdgesName.HasRole.ToString() + ".name.toLowerCase() = '" + userRol + "')) where @class = '"
                                            + VertexName.User.ToString() + "'";
            List<ODocument> usersFromQuery = this.Transaction.QueryTraverse(traverseUsersWithRole);
            return CreateUsersFromDocument(usersFromQuery);
        }

        /// <summary>
        /// Creates the users from the list of document given
        /// </summary>
        /// <param name="usersFromQuery">The result of the mega query</param>
        /// <returns>A list of Users</returns>
        private List<User> CreateUsersFromDocument(IList<ODocument> usersFromQuery)
        {
            List<User> usersInGroup = new List<User>();
            foreach (ODocument user in usersFromQuery)
            {
                User userRecovered = new User()
                {
                    Id = user.ORID.ToString(),
                    Name = user.GetField<string>("name"),
                    LastName = user.GetField<string>("lastName")
                };
                usersInGroup.Add(userRecovered);
            }
            return usersInGroup;
        }

        /// <summary>
        /// Fills the Where condition for Update method.
        /// </summary>
        /// <param name="classId">The id of the vertex which is being updated.</param>
        /// <returns>A List of condition value which is used for the Transaction.</returns>
        private List<ConditionValue> FillVertexConstraints(string classId)
        {
            return new List<ConditionValue> {
            new ConditionValue { ConditionColumn = "@rid", ValueCondition = classId}
            };
        }

        /// <summary>
        /// The method is responsible for creating the object Group.
        /// </summary>
        /// <param name="entity">is object DataGroup</param>
        /// <returns>Object group</returns>
        private Group CreateDataGroup(GroupData entity)
        {
            return new Group
            {
                Name = entity.Name,
                Description = entity.Description,
                State = entity.State.ToString()
            };
        }

        /// <summary>
        /// The method is responsible for creating the relationship of edges with vertex.
        /// </summary>
        /// <param name="rids">Ordi of the vertex</param>
        /// <returns>list edges</returns>
        private List<VertexConnector> GenerateEdges(List<string> rids)
        {
            List<VertexConnector> connectors = new List<VertexConnector>();
            if (rids.Count > 0)
            {
                List<EdgesName> edges = this.ListGroupEdgesName();
                VertexConnector connector = new VertexConnector();
                connector.EdgeName = edges[0];
                connector.To = rids[0];
                connectors.Add(connector);
                connector.EdgeName = edges[0];
                connector.To = rids[1];
                connectors.Add(connector);
            }
            return connectors;
        }

        /// <summary>
        /// The method creates the edges that have relation with the vertex group.
        /// </summary>
        /// <returns>List edges</returns>
        private List<EdgesName> ListGroupEdgesName()
        {
            List<EdgesName> edges = new List<EdgesName>();
            edges.Add(EdgesName.CanHave);
            return edges;
        }

        /// <summary>
        /// he method aims to get rids of vertices that relate.
        /// </summary>
        /// <param name="entity">Object GroupData</param>
        /// <returns>List by rids</returns>
        private List<string> CreateRid(GroupData entity)
        {
            List<string> rids = new List<string>();
            List<ConditionValue> condition = new List<ConditionValue>();
            if (entity.IdAdministrators != null)
            {
                condition.Add(this.GetRidUser("name", RoleName.Administrator.ToString()));
                rids.Add(this.Transaction.GetRid(VertexName.Role, condition));
                condition.Add(this.GetRidUser("name", RoleName.Member.ToString()));
                rids.Add(this.Transaction.GetRid(VertexName.Role, condition));

            }
            return rids;
        }

        /// <summary>
        /// Gets Rid User.
        /// </summary>
        /// <param name="condition">The field you will search in the database.</param>
        /// <param name="value">The value that will search the database.</param>
        /// <returns>The condition created for the relationship between vertices.</returns>
        private ConditionValue GetRidUser(string condition, string value)
        {
            return new ConditionValue() { ConditionColumn = condition, ValueCondition = value };
        }

        /// <summary>
        /// Returns a List of Properties to be changed given the class name an the entity which contains the 
        /// changes for the vertex.
        /// </summary>
        /// <param name="className">the name of the class where its method's name are gonna be taken.</param>
        /// <param name="entity">The entity which contains the changes.</param>
        /// <returns></returns>
        private List<ConditionValue> FillChangedProperties(GroupData entity)
        {
            List<ConditionValue> conditions = new List<ConditionValue> 
            {
                new ConditionValue(){ ConditionColumn = "name", ValueCondition = entity.Name},
                new ConditionValue(){ ConditionColumn = "description", ValueCondition = entity.Description},
                new ConditionValue(){ ConditionColumn = "state", ValueCondition = entity.State.ToString()},
            };
            return conditions;
        }

        /// <summary>
        /// It is responsible for creating the RolInGroup object and insert the object and create their vertices.
        /// </summary>
        /// <param name="entity">Object Group Data</param>
        private void CreatedRolInGroup(GroupData entity)
        {
            this.GenerateRolInGroupData(entity.IdAdministrators, entity.Name, RoleName.Administrator.ToString());
            if (entity.IdMembers != null && entity.IdMembers.Count > 0)
            {
                this.GenerateRolInGroupData(entity.IdMembers, entity.Name, RoleName.Member.ToString());
            }
        }

        /// <summary>
        /// Creates the object RolInGroup.
        /// </summary>
        /// <param name="users">ids users group</param>
        /// <param name="groupName">is the name group.</param>
        /// <param name="roleName">is the role user</param>
        private void GenerateRolInGroupData(List<string> users, string groupName, string roleName)
        {
            foreach (string user in users)
            {
                string name = user + groupName + roleName;
                this.InsertRolInGroup(new RoleInGroupData()
                {
                    GroupName = groupName,
                    RoleName = roleName,
                    UserId = user,
                    Name = name
                });
            }
        }

        /// <summary>
        /// insert the object and make its edges.
        /// </summary>
        /// <param name="roleInGroupData">The insert object.</param>
        /// <returns>value bool.</returns>
        private bool InsertRolInGroup(RoleInGroupData roleInGroupData)
        {
            IRepository<RoleInGroupData> roleInGroup = UnitOfWork.GetInstance().RoleInGroupRepository;
            return roleInGroup.Insert(roleInGroupData);
        }

        /// <summary>
        /// Obtains and stores the ids of all users in the list given
        /// </summary>
        /// <param name="users">The list of users given where the ids will be taken from.</param>
        /// <returns>A list which contains all the ids of the users in the list.</returns>
        private List<string> ObtainUsersIds(List<User> users)
        {
            List<string> ids = new List<string>();
            foreach (User singleUser in users)
            {
                ids.Add(singleUser.Id);
            }
            return ids;
        }

        /// <summary>
        /// Checks wether the user performing the request is an administrator of the group.
        /// </summary>
        private bool CheckPermission(GroupData group)
        {
            foreach (var adminId in group.IdAdministrators)
            {
                if (adminId == CredentialsRetriever.Orid)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
