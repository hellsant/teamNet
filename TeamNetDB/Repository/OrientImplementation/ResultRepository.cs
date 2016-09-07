using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.TransactionToDatabase;
using TeamNetDB.TransactionToDatabase.TransactionOrientDBBinary;

namespace TeamNetDB.Repository.OrientImplementation
{
    public class ResultRepository : IOrientTransactionable<Result360>, IRepository<Result360>
    {
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        public ResultRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        public IEnumerable<Result360> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.Result360, new List<ConditionValue>());
            List<Result360> results = this.CreateEntitiesFromDocuments(documents);
            return results;
        }
        public Result360 GetSingle(string id)
        {
            try
            {
                var result = this.GetAll().Where(v => v.Id.Equals(id)).Single();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        public IEnumerable<Result360> FindBy(Expression<Func<Result360, bool>> predicate)
        {
            return this.FindBy(predicate, this.GetAll());
        }

        public IEnumerable<Result360> FindBy(Expression<Func<Result360, bool>> predicate, IEnumerable<Result360> result360)
        {
            if (result360 != null)
            {
                var entitiesWithPredicate = result360.Where(predicate.Compile());
                return entitiesWithPredicate;
            }
            return null;
        }

        public bool Insert(Result360 entity, IList<string> connectors)
        {
            List<VertexConnector> vertexConnectors = new List<VertexConnector>();
            if (connectors != null)
            {
                vertexConnectors = new List<VertexConnector>()
                {
                    new VertexConnector(){EdgeName=EdgesName.UserResult360},
                    new VertexConnector(){EdgeName=EdgesName.EvaluatorResult360},
                    new VertexConnector(){EdgeName=EdgesName.CompetencyResult360}
                };
                for (int i = 0; i < connectors.Count(); i++)
                {
                    vertexConnectors[i].To = connectors[i];
                }
            }
            else vertexConnectors = new List<VertexConnector>();
            return this.Transaction.InsertVertex(entity, VertexName.Result360, vertexConnectors);

        }
        public bool Delete(Result360 entity)
        {
            List<ConditionValue> conditions = new List<ConditionValue>(){
                new ConditionValue(){ConditionColumn="@orid",ValueCondition=entity.Id}
            };
            return this.Transaction.Delete(VertexName.Result360, conditions);
        }
        public bool Update(Result360 entity)
        {
            OVertex vertexModified = new OVertex();
            vertexModified.SetField("result", entity.Result);
            vertexModified.SetField("management", entity.Management);
            return this.Transaction.Update(vertexModified, entity.Id);
        }
        public IEnumerable<Result360> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<Result360> results = this.CreateEntitiesFromDocuments(documents);
            return results;
        }
        public override Result360 CreateEntityFromDocument(ODocument document)
        {
            Result360 result = new Result360()
            {
                Result = document.GetField<float>("result"),
                Management = document.GetField<string>("management"),
                UserId = document.GetField<ORID>("in_UserResult360").ToString(),
                CompetencyId = document.GetField<ORID>("in_CompetencyResult360").ToString(),
                EvaluatorId = document.GetField<ORID>("in_EvaluatorResult360").ToString(),
                Id = document.ORID.ToString()
            };
            return result;
        }

    }
}
