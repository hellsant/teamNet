using Orient.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Repository.OrientImplementation
{
    public class RoleInGroupRepository : IOrientTransactionable<RoleInGroupData>, IRepository<RoleInGroupData>
    {

        /// <summary>
        /// Constructor that stores the transaccion received into a variable.
        /// </summary>
        /// <param name="transaction">Class that performs the queries</param>
        public RoleInGroupRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        /// <summary>
        /// Property that stores the transacton
        /// </summary>
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        public IEnumerable<RoleInGroupData> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.RoleInGroup, new List<ConditionValue>());
            List<RoleInGroupData> roleInGroups = this.CreateEntitiesFromDocuments(documents);
            return roleInGroups;
        }

        public RoleInGroupData GetSingle(string roleName)
        {
            RoleInGroupData roleInGroup = this.GetAll().FirstOrDefault(v => v.Name.Equals(roleName));
            return roleInGroup;
        }

        public IEnumerable<RoleInGroupData> FindBy(System.Linq.Expressions.Expression<Func<RoleInGroupData, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a new RoleInGroup into database with the entity an its connectors given
        /// </summary>
        /// <param name="entity">the object to be inserted</param>
        /// <param name="conectors">the connectors for the new object</param>
        /// <returns>A boolean value which returns true if the object has been inserted correctly
        /// otherwise returns false.</returns>
        public bool Insert(RoleInGroupData entity, IList<string> conectors = null)
        {
            bool created = true;
            try
            {
                List<string> rids = new List<string>();
                string groupId = GetRid(VertexName.Group, entity.GroupName);
                string roleId = GetRid(VertexName.Role, entity.RoleName);
                List<VertexConnector> connectors = new List<VertexConnector>
                {
                    new VertexConnector { EdgeName = EdgesName.HasGroup, To = groupId },
                    new VertexConnector { EdgeName = EdgesName.HasRole, To = roleId },
                    new VertexConnector { EdgeName = EdgesName.HasRoleInGroup, To = entity.UserId },
                };
                this.Transaction.InsertVertex(new RoleInGroup { Name = entity.Name },
                                               VertexName.RoleInGroup,
                                               connectors);
            }
            catch (Exception ex)
            {
                ex.ToString();
                created = false;
            }

            return created;
        }

        public IEnumerable<RoleInGroupData> Query(string query)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new Instance of RoleInGroupData object obtaining all necessary info from database.
        /// </summary>
        /// <param name="document">The document obtained from database.</param>
        /// <returns>A new Instance of RoleInGroupData.</returns>
        public override RoleInGroupData CreateEntityFromDocument(Orient.Client.ODocument document)
        {
            RoleInGroupData roleInGroup;
            try
            {
                string documentId = document.ORID.ToString();
                roleInGroup = new RoleInGroupData()
                {
                    Id = documentId,
                    Name = document.GetField<string>("name"),
                };
                roleInGroup.RoleName = ObtainPropertyFromRoleInGroup(documentId, VertexName.Role.ToString(), EdgesName.HasRole.ToString());
                string userName = ObtainPropertyFromRoleInGroup(documentId, VertexName.User.ToString(), EdgesName.HasRoleInGroup.ToString());
                roleInGroup.UserId = GetRid(VertexName.User, userName);
                roleInGroup.GroupName = ObtainPropertyFromRoleInGroup(documentId, VertexName.Group.ToString(), EdgesName.HasGroup.ToString());
                return roleInGroup;
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return null;
            }
        }

        public bool Delete(RoleInGroupData entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the RoleInGroupData given the entity which has the modifications for that vertex.
        /// </summary>
        /// <param name="entity">The entity which contains the changes for the vertex.</param>
        /// <returns>A boolean value which notifies wether the Vertex has been modified or not.</returns>
        public bool Update(RoleInGroupData entity)
        {
            RoleInGroupData oldRoleInGroup = GetSingle(entity.Name);
            return updateEntities(oldRoleInGroup, entity);
        }

        /// <summary>
        /// Gets the rid of entity given the name and vertex.
        /// </summary>
        /// <param name="vertexName">The vertex which the entity belongs to.</param>
        /// <param name="entityName">The name of the entity to get the rid.</param>
        /// <returns>An string value which represents the rid of the entity name given.</returns>
        private string GetRid(VertexName vertexName, string entityName)
        {
            List<ConditionValue> condition = new List<ConditionValue>();
            condition.Add(new ConditionValue() { ConditionColumn = "name", ValueCondition = entityName });
            return this.Transaction.GetRid(vertexName, condition);
        }

        /// <summary>
        /// Updates the Edges comparing the old RoleInGroup vertex with the new one.
        /// </summary>
        /// <param name="oldRoleInGroup">The old Vertex which is obtained from database.</param>
        /// <param name="entity">The new Entity which might contain the new Changes for the vertex.</param>
        /// <returns>A boolean value which notifies wether the RoleInGroup vertex has been updated or not.</returns>
        private bool updateEntities(RoleInGroupData oldRoleInGroup, RoleInGroupData entity)
        {
            bool updated = false;
            if (!oldRoleInGroup.GroupName.Equals(entity.GroupName) && entity.GroupName.Equals(""))
            {
                ModifyEdge("Delete", EdgesName.HasGroup.ToString(), entity.Id, GetRid(VertexName.Group, oldRoleInGroup.GroupName));
                ModifyEdge("Delete", EdgesName.HasRole.ToString(), oldRoleInGroup.Id, GetRid(VertexName.Role, oldRoleInGroup.RoleName));
                string newName = entity.UserId + entity.GroupName + entity.RoleName;
                updated = UpdateRoleInGroupName(entity.Id, newName.Trim());
            }
            else
            {
                if (!oldRoleInGroup.GroupName.Equals(entity.GroupName))
                {
                    ModifyEdge("Delete", EdgesName.HasGroup.ToString(), entity.Id, GetRid(VertexName.Group, oldRoleInGroup.GroupName));
                    ModifyEdge("Create", EdgesName.HasGroup.ToString(), entity.Id, GetRid(VertexName.Group, entity.GroupName));
                    ModifyEdge("Delete", EdgesName.HasRole.ToString(), entity.Id, GetRid(VertexName.Role, oldRoleInGroup.RoleName));
                    ModifyEdge("Create", EdgesName.HasRole.ToString(), entity.Id, GetRid(VertexName.Role, entity.RoleName));
                    string newName = entity.UserId + entity.GroupName + entity.RoleName;
                    return UpdateRoleInGroupName(entity.Id, newName.Trim());
                }
            }
            if (!oldRoleInGroup.RoleName.Equals(entity.RoleName))
            {
                ModifyEdge("Delete", EdgesName.HasRole.ToString(), entity.Id, GetRid(VertexName.Role, oldRoleInGroup.RoleName));
                ModifyEdge("Create", EdgesName.HasRole.ToString(), entity.Id, GetRid(VertexName.Role, entity.RoleName));
                string newName = entity.UserId + entity.GroupName + entity.RoleName;
                return UpdateRoleInGroupName(entity.Id, newName.Trim());
            }
            return updated;
        }

        /// <summary>
        /// Updates the name of the RoleInGroup vertex given its id and the newName.
        /// </summary>
        /// <param name="entityId">The rid of the RoleInGroup.</param>
        /// <param name="newName">The new name which contains if the vertex has its edges modified or not.</param>
        /// <returns></returns>
        private bool UpdateRoleInGroupName(string entityId, string newName)
        {
            List<ConditionValue> propertiesUpdated = new List<ConditionValue>();
            propertiesUpdated.Add(new ConditionValue() { ConditionColumn = "name", ValueCondition = newName });
            return this.Transaction.Update(VertexName.RoleInGroup, propertiesUpdated,
                        new List<ConditionValue> { new ConditionValue { ConditionColumn = "@rid", ValueCondition = entityId } });
        }

        /// <summary>
        /// Modifies an edge given the action to do with the edge, name, origin and destination.
        /// </summary>
        /// <param name="action">This represents if the modification needs to Create or Delete.</param>
        /// <param name="edgeName">The name of edge cluster.</param>
        /// <param name="origin">The origin where the edge is comming from</param>
        /// <param name="destination">The destination where the edge is going to.</param>
        private void ModifyEdge(string action, string edgeName, string origin, string destination)
        {
            string queryToModifyEdge = action + "Edge" + edgeName + " from " + origin +" to "+ destination +"";
            this.Transaction.QueryTraverse(queryToModifyEdge);
        }

        /// <summary>
        /// Obtains the names of the near vertes of the current RoleInGroup vertex.
        /// </summary>
        /// <param name="roleInGroupId">The id of the current RoleInGroup vertex.</param>
        /// <param name="property">The property to be fill it can be userid, groupname, rolename</param>
        /// <param name="edge">The edge to perform the traverse query.</param>
        /// <returns>an string which represents the name of the parameter obtained with the query.</returns>
        private string ObtainPropertyFromRoleInGroup(string roleInGroupId, string property, string edge)
        {
            string propertyObtained = "";
            string queryToGetPropertyName  = "select from (traverse out("+edge+") from (select from" 
                                                 +VertexName.RoleInGroup+" where @rid = "+roleInGroupId+")) where @class = \""
                                                 +property+"\"";
            ODocument resultOfQuery = Transaction.QueryTraverse(queryToGetPropertyName).First();
            if (resultOfQuery != null)
            {
                propertyObtained = resultOfQuery.GetField<string> ("name");

            }
            return propertyObtained;
        }
    }
}
