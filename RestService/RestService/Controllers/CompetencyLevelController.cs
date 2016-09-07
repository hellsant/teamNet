using RestService.Logs;
using System;
using System.Collections.Generic;
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
    public class CompetencyLevelController : ApiController
    {
        public static IRepository<CompetencyLevel> competencyLevel;

        public CompetencyLevelController()
        {
            competencyLevel = UnitOfWork.GetInstance().CompetencyLevelRepository;
        }

        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage GetAll()
        {
            try
            {
                IEnumerable<CompetencyLevel> myCompetencyLevel = competencyLevel.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, myCompetencyLevel);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                Logger.Instance().Error(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }
    }
}