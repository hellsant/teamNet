using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Repository.OrientImplementation
{
    public class ReferenceRepository: IOrientTransactionable<Reference>, IRepository<Reference>
    {
        /// <summary>
        /// this method contructor
        /// </summary>
        /// <param name="transaction">This parameter is in charge of queries</param>
         public ReferenceRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        /// <summary>
        /// The transaction received in the constructor is saved into this property.
        /// </summary>
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        /// <summary>
        /// Returns an IEnumerable Collection with all reference from a task
        /// </summary>
        /// <returns>An IEnumerable result that contains all the reference queried</returns>
       public IEnumerable<Reference> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.Reference, new List<ConditionValue>());
            List<Reference> requests = this.CreateEntitiesFromDocuments(documents);
            return requests;
        }

        public Reference GetSingle(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return an IEnumerable collection that contains all those reference that
        /// matches with te predicate given as a lambda function.
        /// </summary>
        /// <param name="predicate">A lambda function that will work as a constraint
        /// for the query.</param>
        /// <returns>An IEnumerable collection</returns>
        public IEnumerable<Reference> FindBy(System.Linq.Expressions.Expression<Func<Reference, bool>> predicate)
        {
            var allEntities = this.GetAll();
            var entitiesWithPredicate = allEntities.Where(predicate.Compile());
            return entitiesWithPredicate;
        }

        /// <summary>
        /// Inserts the reference and its connectors given into the database
        /// </summary>
        /// <param name="entity">The reference to be inserted</param>
        /// <param name="conectors">The edges for the reference</param>
        /// <returns>A boolean value that points wether the reference has been inserted
        /// or not</returns>
        public bool Insert(Reference entity, IList<string> conectors = null)
        {
            bool canInsertReference = ValidateRequest(entity);
            if (canInsertReference)
            {
                List<VertexConnector> vertexConnectors = new List<VertexConnector>();
                vertexConnectors.Add(new VertexConnector() { EdgeName = EdgesName.TaskReference, To = entity.TaskId, IsInEdge = true });
                canInsertReference = this.Transaction.InsertVertex(entity, VertexName.Reference, vertexConnectors);
            }
            return canInsertReference;
        }

        public bool Delete(Reference entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(Reference entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes Query method of Transaction of the query given as 
        /// string parameter wich represents the query itself
        /// </summary>
        /// <param name="query">The query to be executed by the transaction class</param>
        /// <returns>An IEnumerable collection that contains the result of the query 
        /// executed</returns>
        public IEnumerable<Reference> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<Reference> requests = this.CreateEntitiesFromDocuments(documents);
            return requests;
        }

        /// <summary>
        /// Turns an ODocument given into a Reference.
        /// </summary>
        /// <param name="document">The ODocument to be converted into reference</param>
        /// <returns>A Reference that contains all the information taken from the ODocument</returns>
        public override Reference CreateEntityFromDocument(Orient.Client.ODocument doc)
        {
            Reference reference = new Reference()
        {
                Id = doc.ORID.RID,
                Name = doc.GetField<string>("name"),
                Url = doc.GetField<string>("url"),              
                TaskId = doc.GetField<ORID>("in_TaskReference").RID,
            };
            return reference;
        }

        /// <summary>
        /// Method that validate params from reference
        /// </summary>
        /// <param name="reference"> The reference to be validated</param>
        /// <returns>a bool if is validated</returns>
        private bool ValidateRequest(Reference reference)
        {
            bool validReference = false;
            if (reference.Url != null && reference.TaskId != null && reference.Name !=null)
            {
                validReference = IsUrlValid(reference.Url);
            }
            return validReference;
        }

        /// <summary>
        /// Method of help for URL validate.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool IsUrlValid(string url)
        {
            string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }
    }
}
