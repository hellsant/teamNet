using RestService.Logs;
using RestService.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using TeamNetDB.Repository.OrientImplementation;
using System.Linq;
using System.Diagnostics;

namespace RestService.Controllers
{
    public class CompetencyController : ApiController
    {

        private static UnitOfWork data;

        public CompetencyController()
        {
            if (data == null)
            {
                data = UnitOfWork.GetInstance();
            }
        }

        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage GetAll()
        {
            try
            {
                IEnumerable<Competency> allCompetencies = data.CompetencyRepository.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, allCompetencies);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                Logger.Instance().Error(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        /// <summary>
        /// Get all competence with a level.
        /// </summary>
        /// <param name="userId">id to search</param>
        /// <returns>List Competencies</returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/competency/{userId}")]
        [Authorize]
        public HttpResponseMessage GetCompetencyById(string userId)
        {
            userId = "#13:" + userId;
            var allCompetency = data.CompetencyRepository.GetAll();
            var results360 = data.Result360Repository.GetAll();

            var resultRepository = data.Result360Repository as ResultRepository;
            foreach (var competency in allCompetency)
            {
                List<Result360> evaluatorResults = resultRepository.FindBy(result =>
                        result.UserId.Equals(userId) &&
                        result.CompetencyId.Equals(competency.Id),
                        results360
                        ).ToList();
                if (evaluatorResults.Count != 0)
                {
                    competency.Result = evaluatorResults.Average(result => result.Result);
                }
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, allCompetency);
            return response;
        }
    }
}