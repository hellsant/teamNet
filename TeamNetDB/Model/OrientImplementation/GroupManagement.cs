using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;

namespace TeamNetDB.Model.OrientImplementation
{
    public class GroupManagement : IGroupManagement, IEntity
    {
        /// <summary>
        /// Contains group id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Manages the group name
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Contains Ids new members are added to the group.
        /// </summary>
        public List<string> RidMembers { get; set; }

        /// <summary>
        /// It contains the id of the group member to be deleted.
        /// </summary>
        public string Member{ get; set; }
    }
}
