using RestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestService.Controllers
{
    public class SecurityController : ApiController
    {
        //[AllowAnonymous]
        [Authorize]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get()
        {
            var responseObject = "ok";
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, responseObject);
            return response;
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
