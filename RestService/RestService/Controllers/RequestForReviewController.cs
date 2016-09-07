using RestService.Logs;
using RestService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using TeamNetDB.Repository.Interfaces;

namespace RestService.Controllers
{
    public class RequestForReviewController : ApiController
    {
        public static IRepository<RequestForReview> requests;
        public static IRepository<User> users;
        public static IRepository<Task> tasks;
        public static IRepository<Reference> references;

        public RequestForReviewController()
        {
            if (requests == null)
            {
                requests = UnitOfWork.GetInstance().RequestForReviewRepository;
                users = UnitOfWork.GetInstance().UserRepository;
                tasks = UnitOfWork.GetInstance().TaskRepository;
                references = UnitOfWork.GetInstance().ReferenceRepository;
            }
        }

        /// <summary>
        ///GetAll method returns all requests for review
        /// </summary>
        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage GetAll()
        {
            try
            {
                IEnumerable<RequestForReview> allRequests = requests.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, allRequests);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                Logger.Instance().Error(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        /// <summary>
        ///Post method inserts a requests for review .
        ///Neccesary RequestForReview's fields : ReviewerId , TaskId
        /// </summary>
        [System.Web.Http.HttpPost]
        [Authorize]
        public HttpResponseMessage Post(RequestForReview request)
        {
            bool created = requests.Insert(request);
            if (created)
            {
                return Request.CreateResponse(HttpStatusCode.Created, request);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Error");
            }
        }


        /// <summary>
        ///GetByReviewer method returns all pending requests given the reviewer Id
        /// </summary>
        [System.Web.Http.HttpGet]
        [Route("api/requestforreview/reviewer/{reviewerId}")]
        [Authorize]
        public HttpResponseMessage GetByReviewer(string reviewerId)
        {
            string rid = "#13:" + reviewerId;
            try
            {
                List<RequestModel> requestForSend = new List<RequestModel>();
                IEnumerable<RequestForReview> requestsFromReviewer = requests.FindBy(r => (r.ReviewerId.Equals(rid) && r.Reviewed==false));
                foreach (var request in requestsFromReviewer)
                {
                    Task task = tasks.GetSingle(request.TaskId);
                    string userId = task.UserId;
                    User owner = users.FindBy(u=>u.Id == userId).Single();
                    IEnumerable<Reference> relatedReferences = references.FindBy(f => f.TaskId == task.Id);
                    RequestModel model = new RequestModel()
                    {
                        RequestId = request.Id,
                        RequestDate = request.RequestDate,
                        Name = owner.Name + " " + owner.LastName,
                        TaskTitle = task.Name,
                        TaskDescription = task.Description,
                        CommentsFromRequester = request.CommentsFromRequester,
                        CommentsFromReviewer = "",
                        References = relatedReferences.ToList()
                    };
                    requestForSend.Add(model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, requestForSend);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                Logger.Instance().Error(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        /// <summary>
        ///GetByOwner method returns all request given the owner Id
        /// </summary>
        [System.Web.Http.HttpGet]
        [Route("api/requestforreview/owner/{ownerId}")]
        [Authorize]
        public HttpResponseMessage GetByOwner(string ownerId)
        {
            try
            {
                List<ResponseModel> requestsByOwner = new List<ResponseModel>();
                string userId = "#13:" + ownerId;
                var allTaskByUser = UnitOfWork.GetInstance().TaskRepository.FindBy(t => t.UserId == userId);
                foreach (var task in allTaskByUser)
                {
                    var taskId = task.Id;
                    var allRequestsByTaskId = requests.FindBy(r => r.TaskId.Equals(taskId));
                    IEnumerable<Reference> relatedReferences = references.FindBy(f => f.TaskId == task.Id);
                    
                    foreach (var request in allRequestsByTaskId)
                    {
                        var reviewer = users.FindBy(u => u.Id.Equals(request.ReviewerId)).Single();
                        var reviewerName = reviewer.Name +" "+ reviewer.LastName;


                        ResponseModel response = new ResponseModel()
                        {
                             CommentsFromReviewer = request.CommentsFromReviewer,
                             CommentsFromRequester  = request.CommentsFromRequester,
                             RequestDate  = request.RequestDate,
                             RevisionDate  = request.RevisionDate,
                             Approved  = request.Approved,
                             Reviewed  = request.Reviewed,
                             ReviewerName  = reviewerName,
                             TaskTitle  = task.Name,
                             TaskDescription = task.Description,
                             References = relatedReferences.ToList()
                        };
                        requestsByOwner.Add(response);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, requestsByOwner);

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                Logger.Instance().Error(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        /// <summary>
        ///GetById method returns a request given its Id
        /// </summary>
        [System.Web.Http.HttpGet]
        [Route("api/requestforreview/{requestId}")]
        [Authorize]
        public HttpResponseMessage GetById(string requestId)
        {
            string rid = "#45:" + requestId;
            try
            {
                var request = requests.GetSingle(rid);
                return Request.CreateResponse(HttpStatusCode.OK, request);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                Logger.Instance().Error(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        [System.Web.Http.HttpPut]
        [Authorize]
        public HttpResponseMessage Put(RequestForReview request)
        {
            DateTime date = DateTime.Now;
            string revitionDate = date.ToString("yyyy-MM-dd");
            request.RevisionDate = revitionDate;
            request.Reviewed = true;
            bool updated = requests.Update(request);
            if (updated)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }
        }

        public HttpResponseMessage Options()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}