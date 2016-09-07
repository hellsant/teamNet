using log4net;
using RestService.Logs;
using RestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace RestService.Controllers
{
    /// <summary>
    /// Controller to logs.
    /// </summary>
    public class LogController : ApiController
    {

        /// <summary>
        /// This method recieve the logs from the side client.
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [Authorize]
        public HttpResponseMessage Post(LogModel[] logs)
        {
            if (logs != null)
            {
                foreach (LogModel currentLog in logs)
                {
                    string logMessage = currentLog.ToString();
                    Logger.Instance().Error(logMessage);
                }

                return Request.CreateResponse(HttpStatusCode.Created, logs);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Error");
            }
        }

        /// <summary>
        /// this method send the ok response.
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpOptions]
        public HttpResponseMessage Options()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
