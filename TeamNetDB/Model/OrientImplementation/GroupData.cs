using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Model.OrientImplementation
{
    public class GroupData : IGroupData, IEntity
    {
        
        [OProperty(Alias = "rid")]
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Manages the group name.
        /// </summary>
        [OProperty(Alias = "name")]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Manages the group description.
        /// </summary>
        [OProperty(Alias = "description")]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Manages access to group.
        /// </summary>
        [OProperty(Alias = "state")]
        public State State
        {
            get;
            set;
        }

        /// <summary>
        /// Manages the list of users belonging to the group.
        /// </summary>
        public List<User> Members
        {
            get;
            set;
        }

        /// <summary>
        /// It is the group administrator.
        /// </summary>
        public List<User> Administrators{ get; set; }

        /// <summary>
        /// List rids members
        /// </summary>
        public List<string> IdMembers { get; set; }

        /// <summary>
        /// List rids administrators
        /// </summary>
        public List<string> IdAdministrators { get; set; }

        /// <summary>
        /// Determines the permission that the requester has in the group.
        /// </summary>
        public bool HasPermission { get; set; }
    }
}
