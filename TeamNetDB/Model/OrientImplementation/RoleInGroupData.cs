using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;

namespace TeamNetDB.Model.OrientImplementation
{
    public class RoleInGroupData : IRoleInGroupData, IEntity
    {
        public string Id { get; set; }

        /// <summary>
        /// Manages code relationship between user, group and role.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Manages the name of the group to which the user belongs.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Manages the role name for the user in a group.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Manages user RID.
        /// </summary>
        public string UserId { get; set; }
    }
}
