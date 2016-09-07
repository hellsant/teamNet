using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.Security
{
    public static class CredentialsRetriever
    {
        /// <summary>
        /// Property that allows us to know the user's id. It will be set during the login.
        /// </summary>
        public static string Orid { get; set; }
    }
}
