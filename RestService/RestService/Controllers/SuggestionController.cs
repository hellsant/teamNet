using TeamNetDB.Repository;
using TeamNetDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Repository.OrientImplementation;
namespace RestService.Controllers
{
    public class SuggestionController : ApiController
    {
        /// <summary>
        /// Manages the repository suggestions.
        /// </summary>
        public static IRepository<SuggestionData> suggestion;
        
        /// <summary>
        /// Initializes variables.
        /// </summary>
        public SuggestionController()
        {
            suggestion = UnitOfWork.GetInstance().SugestionRepository;
        }

        /// <summary>
        /// Get all suggestions.
        /// </summary>
        /// <returns>List suggestions.</returns>
        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage Get()
        {
            var collection = suggestion.GetAll();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, collection);
            return response;
        }

        /// <summary>
        /// Get all suggestions CompetencyLevel and comptency.
        /// </summary>
        /// <param name="competencyLevel">competencyLevel to search</param>
        /// <param name="competency">Competency to search</param>
        /// <returns>List Suggestions</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/suggestion/{competencyLevel}/{competency}")]
        [Authorize]
        public HttpResponseMessage GetSuggestionByCompetencyLevelAndCompetency(int competencyLevel, string competency)
        {
            var collection = suggestion.FindBy(e => e.Competency.Equals(competency) && e.ValueCompetecyLevel==competencyLevel);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, collection);
            return response;
        }

        /// <summary>
        /// Returns all suggestions of a specific user.
        /// </summary>
        /// <param name="user">user to search</param>
        /// <returns>List suggestions</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/suggestion/{user}")]
        [Authorize]
        public HttpResponseMessage GetSuggestionsOfUser(string user)
        {
            var collection = suggestion.FindBy(e => e.CreatedTo.Equals(user));
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, collection);
            return response;
        }

        /// <summary>
        /// We send suggestions to be stored in the database.
        /// </summary>
        /// <param name="newSuggestion">sugestion to save.</param>
        /// <returns>bool</returns>
        [System.Web.Http.HttpPost]
        [Authorize]
        public HttpResponseMessage Post(SuggestionData newSuggestion)
        {
            bool created = suggestion.Insert(newSuggestion);
            if (created)
            {
                return Request.CreateResponse(HttpStatusCode.Created, created);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Error");
            }
        }

        [System.Web.Http.HttpOptions]
        public HttpResponseMessage Options()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [System.Web.Http.HttpDelete]
        [Authorize]
        public HttpResponseMessage Delete(string id)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
	}
}