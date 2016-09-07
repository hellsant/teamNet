using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Model.Interfaces
{
    public interface IGroupData
    {
        /// <summary>
        /// Manages the group name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Manages the group description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Manages access to group.
        /// </summary>
        State State { get; set; }

        /// <summary>
        /// Manages the list of users belonging to the group.
        /// </summary>
        List<User> Members { get; set; }

        /// <summary>
        /// It is the group administrator.
        /// </summary>
        List<User> Administrators { get; set; }

        /// <summary>
        /// List rids members
        /// </summary>
        List<string> IdMembers { get; set; }

        /// <summary>
        /// List rids administrators
        /// </summary>
        List<string> IdAdministrators { get; set; }

        /// <summary>
        /// Determines the permission that the requester has in the group.
        /// </summary>
        bool HasPermission { get; set; }
    }
}
