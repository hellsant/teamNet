using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;
using Orient.Client;

namespace TeamNetDB.Model.OrientImplementation
{
    public class Publication: IEntity, IPublication
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

        [OProperty(Alias = "name")]
        public string Publicator
        {
            get;
            set;
        }

        [OProperty(Alias = "link")]
        public string Task
        {
            get;
            set;
        }

        [OProperty(Alias = "userId")]
        public string UserId
        {
            get;
            set;
        }

        [OProperty(Alias = "groupId")]
        public List<string> GroupsIds
        {
            get;
            set;
        }

        [OProperty(Alias = "taskLinks")]
        public List<string> TaskLinks
        {
            get;
            set;
        }
    }
}
