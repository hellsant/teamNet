using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;

namespace TeamNetDB.Model.OrientImplementation
{
    public class Notifies : INotifies, IEntity
    {
        [OProperty(Alias = "date")]
        public string Date
        {
            get;
            set;
        }

        [OProperty(Alias = "rid")]
        public string Id
        {
            get;
            set;
        }
    }
}
