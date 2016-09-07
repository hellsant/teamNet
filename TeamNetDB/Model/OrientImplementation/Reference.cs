using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;

namespace TeamNetDB.Model.OrientImplementation
{
    public class Reference: IReference, IEntity
    {
       [OProperty(Alias = "rid")]
       public string Id
        {
            get;
            set;
        }

        [OProperty(Alias = "in_TaskReference", Serializable = false)]
       public string TaskId
       {
           get;
           set;
       }

        [OProperty(Alias = "name")]
        public string Name
        {
            get;
            set;
        }

        [OProperty(Alias = "url")]
        public string Url
        {
            get;
            set;
        }

    }
}
