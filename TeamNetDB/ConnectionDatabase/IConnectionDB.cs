using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.ConnectionDatabase
{
    public interface IConnectionDB
    {
        /// <summary>
        /// Create the connection to the database.
        /// </summary>
        /// <returns>Type data Connection</returns>
        IDisposable Connection();
    }
}
