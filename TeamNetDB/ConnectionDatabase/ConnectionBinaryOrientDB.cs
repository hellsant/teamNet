using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orient.Client.Protocol;
using Orient.Client;

namespace TeamNetDB.ConnectionDatabase
{
    public class ConnectionBinaryOrientDB : IConnectionDB
    {
        /// <summary>
        /// Manages the connection to the database.
        /// </summary>
        private CreateConnection createConnection;

        /// <summary>
        /// Initialize variables for the server.
        /// </summary>
        public ConnectionBinaryOrientDB(string nameDatabase)
        {
            createConnection = new CreateConnection(nameDatabase);
            createConnection.CreatePool();
        }

        /// <summary>
        /// Create the connection database.
        /// </summary>
        /// <returns>Connection</returns>
        public IDisposable Connection()
        {
            return new ODatabase(this.createConnection.GlobalDatabaseAlias);
        }
    }
}
