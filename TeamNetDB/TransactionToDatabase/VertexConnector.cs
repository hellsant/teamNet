using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.TransactionToDatabase
{
    public class VertexConnector
    {
        public EdgesName EdgeName{get; set;}
        
        public string To { get; set; }

        public bool IsInEdge { get; set; }
    }
}
