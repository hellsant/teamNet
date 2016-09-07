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
    public class Group : IGroup, IEntity
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
        public string State
        {
            get;
            set;
        }
    }
}
