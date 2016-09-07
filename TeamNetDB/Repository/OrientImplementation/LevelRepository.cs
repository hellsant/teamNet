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
    public class LevelRepository : IOrientTransactionable<Level>,IRepository<Level>
    {
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        public LevelRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        public IEnumerable<Level> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.Level, new List<ConditionValue>());
            List<Level> levels = this.CreateEntitiesFromDocuments(documents);
            return levels;
        }

        public Level GetSingle(string id)
        {
            var level = this.GetAll().Where(v => v.Id.Equals(id)).Single();
            return level;
        }

        public IEnumerable<Level> FindBy(Expression<Func<Level, bool>> predicate)
        {
            var allEntities = this.GetAll();
            var entitiesWithPredicate = allEntities.Where(predicate.Compile());
            return entitiesWithPredicate;
        }
        public bool Insert(Level entity, IList<string> connectors)
        {
            List<VertexConnector> vertexConnectors = new List<VertexConnector>();
            return this.Transaction.InsertVertex(entity, VertexName.Level, vertexConnectors);
        }
        public bool Delete(Level entity)
        {
           List<ConditionValue> conditions = new List<ConditionValue>(){
               new ConditionValue(){ConditionColumn="@orid",ValueCondition=entity.Id}
           };
           return this.Transaction.Delete(VertexName.Level, conditions);
        }
        public bool Update(Level entity)
        {
            OVertex vertexModified = new OVertex();
            vertexModified.SetField("name", entity.Name);
            return this.Transaction.Update(vertexModified, entity.Id);
        }
        public IEnumerable<Level> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<Level> level = this.CreateEntitiesFromDocuments(documents);
            return level;
        }
        public override Level CreateEntityFromDocument(ODocument document)
        {
            Level level = new Level()
            {
                Name = document.GetField<string>("name"),
                Id = document.ORID.ToString(),
            };
            return level;
        }

    }
}
