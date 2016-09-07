using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Repository.OrientImplementation
{
    public class ExpectedRepository : IOrientTransactionable<Expected>, IRepository<Expected>
    {
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        public ExpectedRepository(OrientImplementation.Transaction transaction)
        {
            this.Transaction = transaction;
        }

        public IEnumerable<Expected> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.Expected, new List<ConditionValue>());
            List<Expected> expecteds = this.CreateEntitiesFromDocuments(documents);
            return expecteds;
        }
        public Expected GetSingle(string id)
        {
            var expected = this.GetAll().Where(v => v.Id.Equals(id)).Single();
            return expected;
        }
        public IEnumerable<Expected> FindBy(Expression<Func<Expected, bool>> predicate)
        {
            return this.FindBy(predicate, this.GetAll());
        }
        public IEnumerable<Expected> FindBy(Expression<Func<Expected, bool>> predicate, IEnumerable<Expected> expecteds)
        {
            var entitiesWithPredicate = expecteds.Where(predicate.Compile());
            int i = entitiesWithPredicate.Count();
            return entitiesWithPredicate;
        }
        public bool Insert(Expected entity, IList<string> connectors)
        {
            List<VertexConnector> vertexConnectors = new List<VertexConnector>();
            if (connectors != null)
            {
                vertexConnectors = new List<VertexConnector>()
                {
                    new VertexConnector(){EdgeName=EdgesName.LevelExpected},
                    new VertexConnector(){EdgeName=EdgesName.CompetencyExpected}
                };
                for (int i = 0; i < connectors.Count(); i++)
                {
                    vertexConnectors[i].To = connectors[i];
                }
            }
            else vertexConnectors = new List<VertexConnector>();
            return this.Transaction.InsertVertex(entity, VertexName.Expected, vertexConnectors);
        }
        public bool Delete(Expected entity)
        {
            List<ConditionValue> conditions = new List<ConditionValue>(){
                new ConditionValue(){ConditionColumn="@orid",ValueCondition=entity.Id}
            };
            return this.Transaction.Delete(VertexName.Expected, conditions);
        }
        public bool Update(Expected entity)
        {
            OVertex vertexModified = new OVertex();
            vertexModified.SetField("priority", entity.Priority);
            vertexModified.SetField("valueExpected", entity.ValueExpected);
            return this.Transaction.Update(vertexModified, entity.Id);
        }
        public IEnumerable<Expected> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<Expected> expected = this.CreateEntitiesFromDocuments(documents);
            return expected;
        }
        public override Expected CreateEntityFromDocument(ODocument document)
        {
            Expected expected = new Expected()
            {
                ValueExpected = document.GetField<int>("valueExpected"),
                Priority = document.GetField<int>("priority"),
                CompetencyId = document.GetField<ORID>("in_CompetencyExpected").ToString(),
                LevelId = document.GetField<ORID>("in_LevelExpected").ToString(),
                Id = document.ORID.ToString(),
            };
            return expected;
        }
    }
}
