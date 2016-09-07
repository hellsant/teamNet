using Orient.Client;
using System.Collections.Generic;
using TeamNetDB.Model.Interfaces;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Repository.Interfaces
{
    public interface ITransaction
    {
        bool InsertVertex(IEntity objectToBeInsert, VertexName name, List<VertexConnector> connector);

        List<ODocument> QueryVertex(VertexName vertex, List<ConditionValue> condition);

        string GetRid(VertexName vertex, List<ConditionValue> condition);

        List<ODocument> QueryTraverse(string query);

        bool Update(VertexName vertex, List<ConditionValue> setCondition, List<ConditionValue> whereCondition);

        bool Update(OVertex vertex, string id);

        bool Delete(VertexName vertex, List<ConditionValue> condition);
    }
}
