using RestService.Logs;
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
    public class ReferenceController : ApiController
    {
        public static IRepository<Reference> references;

        /// <summary>
        /// Constructor method
        /// </summary>
        public ReferenceController()
        {
            if (references == null)
            {
                references = UnitOfWork.GetInstance().ReferenceRepository;
            }
        }

        /// <summary>
        ///GetAll method returns all references from a task
        /// </summary>
        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage GetAll()
        {
            try
            {
                IEnumerable<Reference> allRequests = references.GetAll();
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
        ///Post method inserts a reference.
        ///Neccesary Reference's fields : taskId
        /// </summary>
        [System.Web.Http.HttpPost]
        [Authorize]
        public HttpResponseMessage Post(List<Reference> referencesTask)
        {
            bool created = true;
            foreach (Reference reference in referencesTask)
            {
                created = references.Insert(reference) && created;
            }
            if (created)
            {
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Error in save references");
            }
        }

        public HttpResponseMessage Options()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}