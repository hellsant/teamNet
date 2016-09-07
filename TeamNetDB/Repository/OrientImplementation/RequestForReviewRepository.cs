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
    public class RequestForReviewRepository : IOrientTransactionable<RequestForReview>, IRepository<RequestForReview>
    {

        public RequestForReviewRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        /// <summary>
        /// The transaction received in the constructor is saved into this property.
        /// </summary>
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        public IEnumerable<RequestForReview> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.RequestForReview, new List<ConditionValue>());
            List<RequestForReview> requests = this.CreateEntitiesFromDocuments(documents);
            return requests;
        }

        /// <summary>
        /// Returns a Single request for review given an id
        /// </summary>
        /// <param name="id">the id of the request to be recovered</param>
        /// TODO - Add cluster identifier to id field 
        ///      "#45:" + "id"
        /// <returns>A request that with the given id</returns>
        public RequestForReview GetSingle(string id)
        {
            var request = this.GetAll().Where(v => v.Id.Equals(id)).Single();
            return request;
        }

        /// <summary>
        /// Return an IEnumerable collection that contains all those request for review that
        /// matches with te predicate given as a lambda function.
        /// </summary>
        /// <param name="predicate">A lambda function that will work as a constraint
        /// for the query.</param>
        /// <returns>An IEnumerable collection</returns>
        public IEnumerable<RequestForReview> FindBy(Expression<Func<RequestForReview, bool>> predicate)
        {
            var allEntities = this.GetAll();
            var entitiesWithPredicate = allEntities.Where(predicate.Compile());
            int count = entitiesWithPredicate.Count();
            return entitiesWithPredicate;
        }

        /// <summary>
        /// Inserts the request for review and its connectors given into the database
        /// </summary>
        /// <param name="entity">The request for review to be inserted</param>
        /// <param name="conectors">The edges for the task</param>
        /// <returns>A boolean value that points wether the request for review has been inserted
        /// or not</returns>
        public bool Insert(RequestForReview entity, IList<string> conectors = null)
        {
            entity.Approved = false;
            entity.Reviewed = false;
            bool canInsertTask = ValidateRequest(entity);
            if (canInsertTask)
            {
                List<VertexConnector> vertexConnectors = new List<VertexConnector>();
                vertexConnectors.Add(new VertexConnector() { EdgeName = EdgesName.Reviewer, To = entity.ReviewerId, IsInEdge = true });
                vertexConnectors.Add(new VertexConnector() { EdgeName = EdgesName.TaskForReview, To = entity.TaskId, IsInEdge = true });
                entity.ReviewerId = null;
                entity.TaskId = null;
                canInsertTask = this.Transaction.InsertVertex(entity, VertexName.RequestForReview, vertexConnectors);
            }
            return canInsertTask;
        }

        public bool Delete(RequestForReview entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(RequestForReview entity)
        {
            bool updated = false;
            try
            {
                OVertex vertexModified = new OVertex();
                vertexModified.SetField("commentsFromReviewer", entity.CommentsFromReviewer);
                //vertexModified.SetField("commentsFromRequester", entity.CommentsFromRequester);
                //vertexModified.SetField("requestDate", entity.RequestDate);
                vertexModified.SetField("revisionDate", entity.RevisionDate);
                vertexModified.SetField("approved", entity.Approved);
                vertexModified.SetField("reviewed", entity.Reviewed);
                UnitOfWork.GetInstance().GetDatabase().Update(vertexModified)
                             .Where("@rid").Equals(entity.Id)
                             .Limit(1)
                             .Run();
                updated = true;

                if (entity.Approved == true)
                {
                    RequestForReview request = GetSingle(entity.Id);
                    List<ConditionValue> set = new List<ConditionValue>(){new ConditionValue(){ConditionColumn="isApproved",ValueCondition=true.ToString()}};
                    List<ConditionValue> where = new List<ConditionValue>(){new ConditionValue(){ConditionColumn="@rid",ValueCondition=request.TaskId}};
                    new TransactionBinary(UnitOfWork.GetInstance().GetDatabase()).Update(VertexName.Task, set, where);
                    //UnitOfWork.GetInstance().GetDatabase().Update("Task").Set("isApproved",true).Where("@rid").Equals(entity.TaskId).Run();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                updated = false;
            }
            return updated;
        }

        /// <summary>
        /// Executes Query method of Transaction of the query given as 
        /// string parameter wich represents the query itself
        /// </summary>
        /// <param name="query">The query to be executed by the transaction class</param>
        /// <returns>An IEnumerable collection that contains the result of the query 
        /// executed</returns>
        public IEnumerable<RequestForReview> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<RequestForReview> requests = this.CreateEntitiesFromDocuments(documents);
            return requests;
        }

        /// <summary>
        /// Turns an ODocument given into a reference for review.
        /// </summary>
        /// <param name="document">The ODocument to be converted into refernce for review</param>
        /// <returns>A request for review that contains all the information taken from the ODocument</returns>
        public override RequestForReview CreateEntityFromDocument(Orient.Client.ODocument doc)
        {
            RequestForReview request = new RequestForReview()
        {
                Id = doc.ORID.RID,
                Approved = doc.GetField<bool>("approved"),
                Reviewed = doc.GetField<bool>("reviewed"),
                CommentsFromRequester = doc.GetField<string>("commentsFromRequester"),
                CommentsFromReviewer = doc.GetField<string>("commentsFromReviewer"),
                RequestDate = doc.GetField<string>("requestDate"),
                RevisionDate = doc.GetField<string>("revisionDate"),
                ReviewerId = doc.GetField<ORID>("in_Reviewer").RID,
                TaskId = doc.GetField<ORID>("in_TaskForReview").RID,
            };
            return request;
        }

        /// <summary>
        /// Method that validate params from Request for review
        /// </summary>
        /// <param name="reference"> The request for review to be validated</param>
        /// <returns>a bool if is validated</returns>
        private bool ValidateRequest(RequestForReview request)
        {
            bool validRequest = false;
            if (request.ReviewerId != null && request.TaskId != null)
            {
                validRequest = true;
            }
            return validRequest;
        }
    }
}
