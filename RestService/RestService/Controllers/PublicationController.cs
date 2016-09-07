using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using TeamNetDB.Repository.Interfaces;

namespace RestService.Controllers
{
    public class PublicationController : ApiController
    {
        /// <summary>
        /// Manages the repository publications.
        /// </summary>
        private IRepository<Publication> publicationRepository;

        /// <summary>
        /// Initialize repository.
        /// </summary>
        public PublicationController()
        {
            if (publicationRepository == null)
            {
                publicationRepository = UnitOfWork.GetInstance().PublicationRepository;
            }
        }

        /// <summary>
        /// Returns all Publications.
        /// </summary>
        /// /// <returns> List Publications </returns>
        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetAllPublications()
        {
            try
            {
                IEnumerable<Publication> allPublications = publicationRepository.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, allPublications);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        /// <summary>
        /// Insert a new Publication.
        /// </summary>
        /// <param name="publication"> Object to Insert </param>
        /// <returns> If the publication has been inserted </returns>
        [System.Web.Http.HttpPost]
        [Authorize]
        public HttpResponseMessage Post(Publication publication)
        {
            bool created = publicationRepository.Insert(publication);
            if (created)
            {
                return Request.CreateResponse(HttpStatusCode.Created, publication);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Can't insert this Publication");
            }
        }

        [System.Web.Http.HttpOptions]
        public HttpResponseMessage Options()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}