using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.ConnectionDatabase
{
    class ConnectionData
    {
        #region Fields
        private string hostname;
        private int port;
        private string username;
        private string password;
        private string rootUserName;
        private string rootUserParssword;
        private string nameDatabase;
        private string nameDatabaseAlias;
        #endregion

        #region Constructor
        public ConnectionData(string nameDatabase)
        {
            this.DataInformationDatabase(nameDatabase);
        }
        #endregion

        #region Properties
        public string HostName
        {
            get { return this.hostname; }
            private set { }
        }

        public int Port
        {
            get { return this.port; }
            private set { }
        }

        public string Username
        {
            get { return this.username; }
            private set { }
        }

        public string Password
        {
            get { return this.password; }
            private set { }
        }

        public string RootUserName
        {
            get { return this.rootUserName; }
            private set { }
        }

        public string RootUserPassword
        {
            get { return this.rootUserParssword; }
            private set { }
        }
        public string NameDatabase
        {
            get { return this.nameDatabase; }
            private set { }
        }

        public string NameDatabaseAlias
        {
            get { return this.nameDatabaseAlias; }
            private set { }
        }
        #endregion

        #region Methods
        private void DataInformationDatabase(string nameDatabase)
        {
            this.hostname = "localhost";
            //this.hostname = "teamnetvm.cloudapp.net";
            //this.hostname = "172.20.80.20";
            this.port = 2424;
            this.username = "admin";
            this.password = "admin";

            this.rootUserName = "team";
            this.rootUserParssword = "team123";

            this.nameDatabase = nameDatabase;
            this.nameDatabaseAlias = nameDatabase+"Alias";
        }
        #endregion
    }
}
