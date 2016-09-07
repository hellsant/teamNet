using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Repository;

namespace TeamNetDB.TransactionToDatabase
{
    public interface ITransaction
    {
        #region Insert
        bool InsertVertex(IEntityBase objectToBeInsert, VertexName name, List<VertexConnector> connector);
        bool CreateEdgeRelationship(EdgesName name, string RidFrom, string RidTo);
        #endregion Insert

        #region Query
        List<ODocument> QueryVertex(VertexName vertex, List<ConditionValue> condition);
        string GetRid(VertexName vertex, List<ConditionValue> condition);
        string GetRid(string query);
        #region QueryTraverse
        List<ODocument> QueryTraverse(string query);
        #endregion QueryTraverse
        #endregion Query
        #region Update
        bool Update(VertexName vertex, List<ConditionValue> setCondition, List<ConditionValue> whereCondition);
        #endregion Update
        #region Delete
        bool Delete(VertexName vertex, List<ConditionValue> condition);
        bool DeleteEdgeRelationship(string fromRid, string toRid);
        #endregion Delete
    }
}
