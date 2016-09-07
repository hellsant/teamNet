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

namespace TeamNetDB.Repository.OrientImplementation
{
    public class CompetencyLevelRepository : IOrientTransactionable<CompetencyLevel>, IRepository<CompetencyLevel>
    {
        public CompetencyLevelRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        public IEnumerable<CompetencyLevel> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.CompetencyLevel, new List<ConditionValue>());
            var levels = this.CreateEntitiesFromDocuments(documents);
            return levels;
        }

        public CompetencyLevel GetSingle(string id)
        {
            var competencyLevel = this.GetAll().Where(v => v.Value.Equals(id)).Single();
            return competencyLevel;
        }

        public IEnumerable<CompetencyLevel> FindBy(Expression<Func<CompetencyLevel, bool>> predicate)
        {
            return (predicate != null) ? FindBy(predicate, this.GetAll()) : null;
        }

        public IEnumerable<CompetencyLevel> FindBy(Expression<Func<CompetencyLevel, bool>> predicate, IEnumerable<CompetencyLevel> competencyLevel)
        {
            if (competencyLevel != null)
            {
                var entitiesWithPredicate = competencyLevel.Where(predicate.Compile());
                return entitiesWithPredicate;
            }
            return null;
        }

        public bool Insert(CompetencyLevel entity, IList<string> connectors = null)
        {
            List<VertexConnector> vertexConnectors = new List<VertexConnector>();
            if (connectors != null)
            {

            }
            else vertexConnectors = new List<VertexConnector>();
            return this.Transaction.InsertVertex(entity, VertexName.CompetencyLevel, vertexConnectors);
        }

        public bool Delete(CompetencyLevel entity)
        {
            List<ConditionValue> conditions = new List<ConditionValue>(){
                new ConditionValue(){ConditionColumn="@orid",ValueCondition=entity.Id}
            };
            return this.Transaction.Delete(VertexName.CompetencyLevel, conditions);
        }

        public bool Update(CompetencyLevel entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CompetencyLevel> Query(string query)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To create the object extracted from the database.
        /// </summary>
        /// <param name="document">Document containing the data from the database.</param>
        /// <returns>The object CompetencyLevel.</returns>
        public override CompetencyLevel CreateEntityFromDocument(ODocument document)
        {
            CompetencyLevel competencyLevel = new CompetencyLevel()
            {
                Description = document.GetField<string>("description"),
                Value = document.GetField<int>("value"),
                CompetencyId = document.GetField<ORID>("in_CompetencyCompetencyLevel").ToString(),
                Id = document.ORID.ToString()
            };
            return competencyLevel;
        }
    }
}
