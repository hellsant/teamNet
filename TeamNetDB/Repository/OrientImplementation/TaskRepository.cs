using Orient.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.TransactionToDatabase;
using TeamNetDB.TransactionToDatabase.TransactionOrientDBBinary;

namespace TeamNetDB.Repository.OrientImplementation
{
    public class TaskRepository : IOrientTransactionable<Task>, IRepository<Task>
    {
        /// <summary>
        /// Constructor that receives a transaccion as parameter.
        /// </summary>
        /// <param name="transaction"> This parameter is in charge of queries</param>
        public TaskRepository(Transaction transaction)
        {
            this.Transaction = transaction;
        }

        /// <summary>
        /// The transaction received in the constructor is saved into this property.
        /// </summary>
        public TeamNetDB.Repository.Interfaces.ITransaction Transaction { get; set; }

        /// <summary>
        /// Returns an IEnumerable Collection with all tasks
        /// </summary>
        /// <returns>An IEnumerable result that contains all the tasks queried</returns>
        public IEnumerable<Task> GetAll()
        {
            var documents = this.Transaction.QueryVertex(VertexName.Task, new List<ConditionValue> ());
            List<Task> tasks = this.CreateEntitiesFromDocuments(documents);
            return tasks;
        }

        /// <summary>
        /// Returns a Single task given an id
        /// </summary>
        /// <param name="id">the id of the task to be recovered</param>
        /// <returns>A Task that with the given id</returns>
        public Task GetSingle(string id)
        {
            Task task;
            try
            {
                task = this.GetAll().Where(v => v.Id.Equals(id)).Single();
            }
            catch (Exception)
            {
                task = null;
            }
            return task;
        }

        /// <summary>
        /// Return an IEnumerable collection that contains all those tasks that
        /// matches with te predicate given as a lambda function.
        /// </summary>
        /// <param name="predicate">A lambda function that will work as a constraints
        /// for the query.</param>
        /// <returns>An IEnumerable collection</returns>
        public IEnumerable<Task> FindBy(Expression<Func<Task, bool>> predicate)
        {
            var allEntities = this.GetAll();
            var entitiesWithPredicate = allEntities.Where(predicate.Compile());
            return entitiesWithPredicate;
        }

        /// <summary>
        /// Inserts the task and its connectors given into the database
        /// </summary>
        /// <param name="entity">The task to be inserted</param>
        /// <param name="conectors">The edges for the task</param>
        /// <returns>A boolean value that points wether the task has been inserted
        /// or not</returns>
        public bool Insert(Task entity, IList<string> conectors = null)
        {
            bool canInsertTask = ValidateTaskFields(entity);
            if (canInsertTask)
            {
                List<VertexConnector> vertexConnectors = new List<VertexConnector>();
                vertexConnectors.Add(new VertexConnector() { EdgeName = EdgesName.UserTask, To = "#13:"+entity.UserId});
                vertexConnectors.Add(new VertexConnector() { EdgeName = EdgesName.CompetencyTask, To = "#12:"+entity.CompetencyId});
                vertexConnectors.Add(new VertexConnector() { EdgeName = EdgesName.ReviewerTask, To = "#13:"+entity.ReviewerId});
                //entity.UserId = null;
                //entity.CompetencyId = null;
                //entity.ReviewerId = null;
                //entity.IsApproved = false;
                SpecificsTask task = new SpecificsTask()
                {
                    CreatingDate = DateTime.Now.Date.ToString(),
                    Description = entity.Description,
                    EndDate = entity.EndDate,
                    FinalProduct = entity.FinalProduct,
                    InitialDate = entity.InitialDate,
                    IsApproved = entity.IsApproved,
                    IsSuggestion = entity.IsSuggestion,
                    Link = "",
                    Name = entity.Name,
                    Progress = entity.Progress,
                    Reviewer = entity.ReviewerId,
                    Score = entity.Score,
                    State = entity.State,
                };
                canInsertTask = this.Transaction.InsertVertex(task, VertexName.Task, vertexConnectors);
            }
            return canInsertTask;

        }

        /// <summary>
        /// Deletes a task in database given the entity to be deleted.
        /// </summary>
        /// <param name="entity">The entity to be deleted</param>
        /// <returns>A boolean value that points wether the task has been deleted or not</returns>
        public bool Delete(Task entity)
        {
            List<ConditionValue> conditions = new List<ConditionValue>(){
               new ConditionValue(){ConditionColumn="@rid",ValueCondition=entity.Id}
           };
            return this.Transaction.Delete(VertexName.Task, conditions);
        }

        /// <summary>
        /// Updates the Task given in database, it recovers every task's field to 
        /// perform this operation.
        /// </summary>
        /// <param name="entity">The task modified to be updated</param>
        /// <returns>A boolean value that points wether the task has been updated or not</returns>
        public bool Update(Task entity)
        {
            OVertex vertexModified = new OVertex();
            vertexModified.SetField("name", entity.Name);
            vertexModified.SetField("progress", entity.Progress);
            vertexModified.SetField("description", entity.Description);
            vertexModified.SetField("initialDate", entity.InitialDate);
            vertexModified.SetField("endDate", entity.EndDate);
            vertexModified.SetField("finalProduct", entity.FinalProduct);
            vertexModified.SetField("score", entity.Score);
            vertexModified.SetField("isSuggestion", entity.IsSuggestion);
            vertexModified.SetField("state", entity.State);
            return this.Transaction.Update(vertexModified, entity.Id);
        }

        /// <summary>
        /// Executes Query method of Transaction of the query given as 
        /// string parameter wich represents the query itself
        /// </summary>
        /// <param name="query">The query to be executed by the transaction class</param>
        /// <returns>An IEnumerable collection that contains the result of the query 
        /// executed</returns>
        public IEnumerable<Task> Query(string query)
        {
            var documents = this.Transaction.QueryTraverse(query);
            List<Task> tasks = this.CreateEntitiesFromDocuments(documents);
            return tasks;
        }

        /// <summary>
        /// Turns an ODocument given into a Task.
        /// </summary>
        /// <param name="document">The ODocument to be converted into task</param>
        /// <returns>A Task that contains all the information taken from the ODocument</returns>
        public override Task CreateEntityFromDocument(ODocument document)
        {
            Task task = null;
            string initialDate = document.GetField<DateTime>("initialDate").ToString("yyyy-MM-dd");
            string endDate = document.GetField<DateTime>("endDate").ToString("yyyy-MM-dd");
            string competencyId = document.GetField<ORID>("out_CompetencyTask").RID;
            string userId = document.GetField<ORID>("out_UserTask").RID;
            string reviewerId = document.GetField<ORID>("out_ReviewerTask").RID;
            bool isApproved = document.GetField<bool>("isApproved");
            try
            {
                task = new Task()
                {
                    Name = document.GetField<string>("name"),
                    Description = document.GetField<string>("description"),
                    Progress = document.GetField<int>("progress"),
                    Id = document.ORID.ToString(),
                    ReviewerId = reviewerId,
                    FinalProduct = document.GetField<string>("finalProduct"),
                    IsSuggestion = document.GetField<bool>("isSuggestion"),
                    Score = document.GetField<double>("score"),
                    State = document.GetField<string>("state"),
                    InitialDate = initialDate,
                    EndDate = endDate,
                    CompetencyId = competencyId,
                    UserId = userId,
                    IsApproved = isApproved
                };
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
            return task;
        }

        private bool ValidateTaskFields(Task task)
        {
            bool validTask = false;
            if (task.UserId!=task.ReviewerId && task.ReviewerId!=null && task.InitialDate != null && task.EndDate != null && task.UserId != null && task.CompetencyId != null)
            {
                string currentDate = String.Format("{0}-{1}-{2}", DateTime.Now.Year, DateTime.Now.Month,
                                              DateTime.Now.Day);
                DateTime now = DateTime.Now;
                DateTime initialDate = Convert.ToDateTime(task.InitialDate);
                DateTime endDate = Convert.ToDateTime(task.EndDate);
                //validTask = endDate >= initialDate && initialDate  >= now;
                validTask = endDate >= initialDate;
            }
            return validTask;
        }
    }
}
