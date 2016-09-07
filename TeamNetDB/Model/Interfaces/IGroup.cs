using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Model.Interfaces
{
    public interface IGroup
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
        string State { get; set; }
    }
}
