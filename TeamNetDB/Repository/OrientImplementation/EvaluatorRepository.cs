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
    public class EvaluatorRepository : IOrientTransactionable<Evaluator>, IRepository<Evaluator>
    {
        //private OrientImplementation.Transaction transaction;

        public EvaluatorRepository(OrientImplementation.Transaction transaction)
        {
            this.Transaction = transaction;
        }
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        public IEnumerable<Evaluator> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.Evaluator, new List<ConditionValue>());
            List<Evaluator> evaluator = this.CreateEntitiesFromDocuments(documents);
            return evaluator;
        }

        public Evaluator GetSingle(string id)
        {
            var evaluator = this.GetAll().Where(v => v.Id.Equals(id)).Single();
            return evaluator;
        }

        public IEnumerable<Evaluator> FindBy(Expression<Func<Evaluator, bool>> predicate)
        {
            var allEntities = this.GetAll();
            var entitiesWithPredicate = allEntities.Where(predicate.Compile());
            return entitiesWithPredicate;
        }

        public bool Insert(Evaluator entity, IList<string> connectors)
        {
            List<VertexConnector> vertexConnectors = new List<VertexConnector>();
            return this.Transaction.InsertVertex(entity, VertexName.Evaluator, vertexConnectors);
        }

        public bool Delete(Evaluator entity)
        {
           List<ConditionValue> conditions = new List<ConditionValue>(){
               new ConditionValue(){ConditionColumn="@orid",ValueCondition=entity.Id}
           };
           return this.Transaction.Delete(VertexName.Evaluator, conditions);
        }

        public bool Update(Evaluator entity)
        {
            OVertex vertexModified = new OVertex();
            vertexModified.SetField("name", entity.Name);
            return this.Transaction.Update(vertexModified, entity.Id);
        }

        public IEnumerable<Evaluator> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<Evaluator> evaluator = this.CreateEntitiesFromDocuments(documents);
            return evaluator;
        }

        public override Evaluator CreateEntityFromDocument(ODocument document)
        {
            Evaluator evaluator = new Evaluator()
            {
                Name = document.GetField<string>("name"),
                Id = document.ORID.ToString(),
            };
            return evaluator;
        }
    }
}
