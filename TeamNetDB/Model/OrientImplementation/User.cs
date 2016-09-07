using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.Interfaces;

namespace TeamNetDB.Model.OrientImplementation
{
    public class User : IUser, IEntity
    {
        [OProperty(Alias = "rid")]
        public string Id
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

        [OProperty(Alias = "lastName")]
        public string LastName
        {
            get;
            set;
        }

        [OProperty(Alias = "email")]
        public string Email
        {
            get;
            set;
        }

        [OProperty(Alias = "password")]
        public string Password
        {
            get;
            set;
        }
    }
}
