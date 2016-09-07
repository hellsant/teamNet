using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;

namespace TeamNetDB.Model.OrientImplementation
{
    public class PublicationData : IEntity
    {
        [OProperty(Alias = "rid")]
        public string Id
        {
            get;
            set;
        }

        [OProperty(Alias = "comment")]
        public string Comment
        {
            get;
            set;
        }

        [OProperty(Alias = "date")]
        public string Date
        {
            get;
            set;
        }
    }
}
