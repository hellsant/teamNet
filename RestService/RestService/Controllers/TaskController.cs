using RestService.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamNetDB.Model;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using TeamNetDB.Repository.Interfaces;

namespace RestService.Controllers
{
    public class TaskController : ApiController
    {
        public static IRepository<TeamNetDB.Model.OrientImplementation.Task> tasks;
    
        public TaskController()
        {
            if (tasks == null)
            {
                tasks = UnitOfWork.GetInstance().TaskRepository;
            }
        }

        /// <summary>
        /// Return the list of task with userId as parameter 
        /// </summary>
        /// <param name="userId">@rid of a user  example : '#13:4'</param>
        /// <returns>IEnumerable Task</returns>
        [System.Web.Http.HttpGet]
        [Route("api/task/{userId}")]
        [Authorize]
        public HttpResponseMessage GetTasksById(string userId)
        {
            string id = "#13:" + userId;
            try
            {
                    IEnumerable<Task> tasksByUserId = tasks.FindBy(e=>e.UserId.Equals(id));
                return Request.CreateResponse(HttpStatusCode.OK, tasksByUserId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                Logger.Instance().Error(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        [System.Web.Http.HttpGet]
        [Route("api/task/competency/{competencyId}/{userId}")]
        [Authorize]
        public HttpResponseMessage GetByCompetency(string competencyId, string userId)
        {
            string rid = "#12:" + competencyId;
            string id = "#13:" + userId;
            try
            {
                IEnumerable<Task> tasksByUserId = tasks.FindBy(e => e.CompetencyId.Equals(rid) && e.UserId.Equals(id));
                return Request.CreateResponse(HttpStatusCode.OK, tasksByUserId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                Logger.Instance().Error(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage GetAll()
        {
            try
            {
                IEnumerable<Task> tasksByUserId = tasks.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, tasksByUserId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                Logger.Instance().Error(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        [System.Web.Http.HttpPost]
        [Authorize]
        public HttpResponseMessage Post(Task task)
        {
            bool created = tasks.Insert(task); 
            if (created)
            {
                return Request.CreateResponse(HttpStatusCode.Created, task);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Error");
            }
        }

        [System.Web.Http.HttpDelete]
        [Route("api/task/{id}")]
        [Authorize]
        public HttpResponseMessage Delete(string id)
        {
            string taskDelete = "#14:" + id;
            Task task = tasks.GetSingle(taskDelete);
            bool removed = DeleteManager.Delete<Task>(tasks, task);

            if (removed)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
        [System.Web.Http.HttpPut]
        [Authorize]
        public HttpResponseMessage Put(Task task)
        {
            bool updated = tasks.Update(task);
            if (updated)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }
        }
        [System.Web.Http.HttpOptions]
        public HttpResponseMessage Options()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
