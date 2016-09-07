using RestService.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TeamNetDB.Repository;
using TeamNetDB.Repository.Interfaces;

namespace RestService.Controllers
{
    public class TimeController : ApiController
    {

        public static IRepository<TeamNetDB.Model.OrientImplementation.Time> tasks;

        public TimeController()
        {
            if (tasks == null)
            {
                tasks = UnitOfWork.GetInstance().TimeRepository;
            }
        }

        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage GetAll()
        {
            try
            {
                IEnumerable<TeamNetDB.Model.OrientImplementation.Time> tasksByUserId = tasks.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, tasksByUserId);
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