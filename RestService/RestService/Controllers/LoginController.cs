using RestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Repository.OrientImplementation;
using TeamNetDB.Security;
namespace RestService.Controllers
{
    public class LoginController : ApiController
    {
        private  IRepository<User> users;
        private LoginManager loginManager;

        public LoginController()
        {
            this.loginManager = new LoginManager();
            if (users == null)
            {
                users = UnitOfWork.GetInstance().UserRepository;
            }
        }

        /// <summary>
        /// Create a POST request with this structure: 
        /// [domain]/api/login
        /// Content-Type : application/json
        /// Body : 
        /// {
        ///     email : "teammember1@fundacion-jala.org",
        ///     password: "safepassword"
        /// }
        /// </summary>
        /// <param name="user">Login data of user (email and password)</param>
        /// <returns></returns>

        [System.Web.Http.HttpPost]
        [Authorize]
        public HttpResponseMessage Post(UserData user)
        {
           var autentication = this.loginManager.AuthenticateUser(user.Email, user.Password);
            return autentication==null? Request.CreateResponse(HttpStatusCode.Conflict, "{}") :  
                                        Request.CreateResponse(HttpStatusCode.OK, autentication);
        }

        [System.Web.Http.HttpOptions]
        public HttpResponseMessage Options()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
