using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Model.Interfaces
{
    public interface IRoleInGroupData
    {
        /// <summary>
        /// Manages code relationship between user, group and role.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Manages the name of the group to which the user belongs.
        /// </summary>
        string GroupName { get; set; }

        /// <summary>
        /// Manages the role name for the user in a group.
        /// </summary>
        string RoleName { get; set; }

        /// <summary>
        /// Manages user RID.
        /// </summary>
        string UserId { get; set; }
    }
}
