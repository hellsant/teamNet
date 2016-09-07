using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.ConnectionDatabase
{
    class CreateConnection
    {
        #region Fields
        private OServer server;
        private ConnectionData connectionData;
        #endregion

        #region Properties
        public int GlobalDatabasePoolSize { get { return 3; } }
        public string GlobalDatabaseName { get; private set; }
        public ODatabaseType GlobalDatabaseType { get; private set; }
        public string GlobalDatabaseAlias { get; private set; }
        #endregion

        #region Constructor
        public CreateConnection(string nameDatabase)
        {
            this.connectionData = new ConnectionData(nameDatabase);
            server = new OServer(this.connectionData.HostName, this.connectionData.Port, this.connectionData.RootUserName,
                this.connectionData.RootUserPassword);

            GlobalDatabaseName = this.connectionData.NameDatabase;
            GlobalDatabaseType = ODatabaseType.Graph;
            GlobalDatabaseAlias = this.connectionData.NameDatabaseAlias;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Manages database server temporarily.
        /// </summary>
        public void CreatePool()
        {
            OClient.CreateDatabasePool(
                this.connectionData.HostName,
                this.connectionData.Port,
                GlobalDatabaseName,
                GlobalDatabaseType,
                this.connectionData.Username,
                this.connectionData.Password,
                GlobalDatabasePoolSize,
                GlobalDatabaseAlias
            );
        }

        /// <summary>
        /// Removes database server temporarily.
        /// </summary>
        public void DropPool()
        {
            OClient.DropDatabasePool(GlobalDatabaseAlias);
        }

        /// <summary>
        /// Returns the data handler server.
        /// </summary>
        /// <returns></returns>
        public OServer GetServer()
        {
            return server;
        }
        #endregion
    }
}
