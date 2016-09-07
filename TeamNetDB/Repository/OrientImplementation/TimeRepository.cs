using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Repository.OrientImplementation;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Repository.OrientImplementation
{
    class TimeRepository : IOrientTransactionable<Time>, IRepository<Time>
    {

        public TimeRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        public IEnumerable<Time> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.Time, new List<ConditionValue>());
            var times = this.CreateEntitiesFromDocuments(documents);
            return times;
        }

        public Time GetSingle(string id)
        {
            Time time;
            try
            {
                time = this.GetAll().Where(v => v.Id.Equals(id)).Single();
            }
            catch (Exception ex)
            {
                time = null;
            }
            return time;
        }

        public IEnumerable<Time> FindBy(Expression<Func<Time, bool>> predicate)
        {
            var allEntities = this.GetAll();
            var entitiesWithPredicate = allEntities.Where(predicate.Compile());
            return entitiesWithPredicate;
        }

        public bool Insert(Time entity, IList<string> conectors = null)
        {
            List<VertexConnector> vertexConnectors = new List<VertexConnector>();
            return this.Transaction.InsertVertex(entity, VertexName.Time, vertexConnectors);
        }

        public bool Delete(Time entity)
        {
            List<ConditionValue> conditions = new List<ConditionValue>(){
               new ConditionValue(){ConditionColumn="@orid",ValueCondition=entity.Id}
           };
            return this.Transaction.Delete(VertexName.Time, conditions);
        }

        public bool Update(Time entity)
        {
            OVertex vertexModified = new OVertex();
            vertexModified.SetField("name", entity.Name);
            return this.Transaction.Update(vertexModified, entity.Id);
        }

        public IEnumerable<Time> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<Time> times = this.CreateEntitiesFromDocuments(documents);
            return times;
        }


        public override Time CreateEntityFromDocument(ODocument document)
        {
            Time time = new Time()
            {
                Name = document.GetField<string>("name"),
                Id = document.ORID.ToString()
            };
            return time;
        }
    }
}
