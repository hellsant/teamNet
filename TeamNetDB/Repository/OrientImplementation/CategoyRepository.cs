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
    public class CategoyRepository : IOrientTransactionable<Area>,IRepository<Area>
    {
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        public CategoyRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        public IEnumerable<Area> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.Category, new List<ConditionValue>());
            List<Area> areas = this.CreateEntitiesFromDocuments(documents);
            return areas;
        }

        public Area GetSingle(string id)
        {
            var area = this.GetAll().Where(v => v.Id.Equals(id)).Single();
            return area;
        }

        public IEnumerable<Area> FindBy(Expression<Func<Area, bool>> predicate)
        {
            var allEntities = this.GetAll();
            var entitiesWithPredicate = allEntities.Where(predicate.Compile());
            return entitiesWithPredicate;
        }

        public bool Insert(Area entity,IList<string> connectors = null)
        {
            List<VertexConnector> vertexConnectors = new List<VertexConnector>();
            return this.Transaction.InsertVertex(entity, VertexName.Category, vertexConnectors);
        }

        public bool Delete(Area entity)
        {
          List<ConditionValue> conditions = new List<ConditionValue>(){
              new ConditionValue(){ConditionColumn="@orid",ValueCondition=entity.Id}
          };
          return this.Transaction.Delete(VertexName.Category, conditions);    
        }

        public bool Update(Area entity)
        {
            OVertex vertexModified = new OVertex();
            vertexModified.SetField("name", entity.Name);
            return this.Transaction.Update(vertexModified, entity.Id);
        }

        public IEnumerable<Area> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<Area> areas = this.CreateEntitiesFromDocuments(documents);
            return areas;
        }

        public override Area CreateEntityFromDocument(ODocument document)
        {
            Area area = new Area()
            {
                Name = document.GetField<string>("name"),
                 Id = document.ORID.ToString(),
            };
            return area;
        }

    }
}
