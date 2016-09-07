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
    public class CompetencyRepository : IOrientTransactionable<Competency>,IRepository<Competency>
    {
        /// <summary>
        /// The transaction given will be stored in this property.
        /// </summary>
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        /// <summary>
        /// Constructor that receives a Transaction.
        /// </summary>
        /// <param name="transaction">The class responsible of execute queries</param>
        public CompetencyRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        /// <summary>
        /// Returns a collection with all the Competencies stored in database.
        /// </summary>
        /// <returns>Returns an IEnumerable collection that contains all the Competencias obtained</returns>
        public IEnumerable<Competency> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.Competency, new List<ConditionValue>());
            var competencies = this.CreateEntitiesFromDocuments(documents);
            return competencies;
        }
        
        /// <summary>
        /// Returns a single competency given the name of the competency to be obtained.
        /// </summary>
        /// <param name="name">The name of the competency to be obtained</param>
        /// <returns>A Competency which has the same name given</returns>
        public Competency GetSingle(string name)
        {
            var competency = this.GetAll().Where(v => v.Name.Equals(name)).Single();
            return competency;
        }

        /// <summary>
        /// Returns an IEnumerable collection that contains all the competencies that matches
        /// the predicate.
        /// </summary>
        /// <param name="predicate">The constrainst to get the competencies</param>
        /// <returns>An IEnumerable collection that contains all the competencies that matches the 
        /// constraints</returns>
        public IEnumerable<Competency> FindBy(Expression<Func<Competency, bool>> predicate)
        {
            var allEntities = this.GetAll();
            var entitiesWithPredicate = allEntities.Where(predicate.Compile());
            return entitiesWithPredicate;
        }

        /// <summary>
        /// Inserts a competency and its connectors given.
        /// </summary>
        /// <param name="entity">The entity to be inserted into database.</param>
        /// <param name="connectors">The connectors of the competency given.</param>
        /// <returns>A boolean value that points whether the competency has been inserted or not.</returns>
        public bool Insert(Competency entity, IList<string> connectors = null)
        {
            List<VertexConnector> vertexConnectors = new List<VertexConnector>();
            if (connectors != null)
            {
                vertexConnectors = new List<VertexConnector>()
                {
                    new VertexConnector(){EdgeName=EdgesName.CategoryCompetency},
                };
                for (int i = 0; i < connectors.Count(); i++)
                {
                    vertexConnectors[i].To = connectors[i];
                }
            }
            else vertexConnectors = new List<VertexConnector>();
            return this.Transaction.InsertVertex(entity, VertexName.Competency, vertexConnectors);
        }

        /// <summary>
        /// Deletes a competentcy given from the database.
        /// </summary>
        /// <param name="entity">The entity to be deleted from the database</param>
        /// <returns>A boolean value that points whether the competency has been deleted
        /// or not</returns>
        public bool Delete(Competency entity)
        {
            List<ConditionValue> conditions = new List<ConditionValue>(){
                new ConditionValue(){ConditionColumn="@orid",ValueCondition=entity.Id}
            };
            return this.Transaction.Delete(VertexName.Competency, conditions);
        }

        /// <summary>
        /// Updates a competency given in database
        /// </summary>
        /// <param name="entity">The entity that contains the modifications</param>
        /// <returns>A boolean value that points whether the competency has been updated or not</returns>
        public bool Update(Competency entity)
        {
            OVertex vertexModified = new OVertex();
            vertexModified.SetField("name", entity.Name);
            return this.Transaction.Update(vertexModified, entity.Id);
        }

        /// <summary>
        /// Executes the QueryTraverse method 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<Competency> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<Competency> competency = this.CreateEntitiesFromDocuments(documents);
            return competency;
        }

        /// <summary>
        /// Creates a Competency from an ODocument given
        /// </summary>
        /// <param name="document">The ODocument where the competency will be turned from </param>
        /// <returns>A competency created from an ODocument</returns>
        public override Competency CreateEntityFromDocument(ODocument document)
        {
            Competency compentecy = new Competency()
            {
                Name = document.GetField<string>("name"),
                Id = document.ORID.ToString()
            };
            return compentecy;
        }
    }
}
