using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Repository.OrientImplementation
{
    public class GroupManagementRepository : IOrientTransactionable<GroupManagement>, IRepository<GroupManagement>
    {
        /// <summary>
        /// Initializes global variables.
        /// </summary>
        /// <param name="transaction"></param>
        public GroupManagementRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        /// <summary>
        /// Manage the Transaction object.
        /// </summary>
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }
        
        public IEnumerable<GroupManagement> GetAll()
        {
            throw new NotImplementedException();
        }

        public GroupManagement GetSingle(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupManagement> FindBy(System.Linq.Expressions.Expression<Func<GroupManagement, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The method is responsible for sending the data necessary to RolInGroup to 
        /// create the relationship of new member and create.
        /// The method does not directly insert any data, it is only an intermediary.
        /// </summary>
        /// <param name="entity">Manages the group name and the new member to be added.</param>
        /// <param name="conectors">No connector is not handled.</param>
        /// <returns>returns a value of true if the data is well inserted.</returns>
        public bool Insert(GroupManagement entity, IList<string> conectors = null)
        {
            bool created = true;
            if (!entity.GroupName.Equals("") && (entity.RidMembers != null || entity.RidMembers.Count>0))
            {
                this.GenerateRolInGroupData(entity.RidMembers, entity.GroupName, RoleName.Member.ToString());
            }
            else
            {
                created = false;
            }
            return created;
        }

        public bool Delete(GroupManagement entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(GroupManagement entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupManagement> Query(string query)
        {
            throw new NotImplementedException();
        }

        public override GroupManagement CreateEntityFromDocument(Orient.Client.ODocument doc)
        {
            throw new NotImplementedException();
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
    }
}
