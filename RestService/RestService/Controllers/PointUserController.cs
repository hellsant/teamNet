using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamNetDB.Model;
using TeamNetDB.Repository;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using RestService.Logs;
namespace RestService.Controllers
{
    public class PointUserController : ApiController
    {
        public static IRepository<Competency> pointOfAreaRepository;

        public PointUserController()
        {
            if (pointOfAreaRepository == null)
            {
                pointOfAreaRepository = UnitOfWork.GetInstance().CompetencyRepository;
            }
        }

        /**
         * http://localhost:5859/api/pointuser?groupId=%2313:0
         * **/

        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage Get(string userId)
        {
            try
            {
                IEnumerable<Competency> pointUser = pointOfAreaRepository.FindBy(e=>e.Id.Equals(userId));
                return Request.CreateResponse(HttpStatusCode.OK, pointUser);
            }
            catch (Exception ex)
            {
                Logger.Instance().Error(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound,String.Format("{'error':'{0}'}",ex.Message));
            }
        }

        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage Get()
        {
            try
            {
                IEnumerable<Competency> pointByAreaId = pointOfAreaRepository.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, pointByAreaId);
            }
            catch (Exception ex)
            {
                Logger.Instance().Error(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, String.Format("{'error':'{0}'}", ex.Message));
            }
        }

        [System.Web.Http.HttpPost]
        [Authorize]
        public HttpResponseMessage Post(Competency point)
        {
            bool created = pointOfAreaRepository.Insert(point);
            if (created)
            {
                return Request.CreateResponse(HttpStatusCode.Created, point);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Error");
            }
        }
	}
}