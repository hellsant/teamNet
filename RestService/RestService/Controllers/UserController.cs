using RestService.Models;
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
using TeamNetDB.Security;
using TeamNetDB.Repository.OrientImplementation;
namespace RestService.Controllers
{
    public class UserController : ApiController
    {
        private IRepository<User> users;
        private LoginManager loginManager;
        private IRepository<Publication> publicationRepository;

        public UserController()
        {
            this.loginManager = new LoginManager();
            if (users == null && publicationRepository == null)
            {
                users = UnitOfWork.GetInstance().UserRepository;
                publicationRepository = UnitOfWork.GetInstance().PublicationRepository;
            }
        }


        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage Get()
        {
           var allUsers = users.GetAll();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, allUsers);
            return response;
        }

        [Route("api/user/basic/{userId}")]
        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage GetBasicInformationForUser(string userId)
        {
            var id = "#13:" + userId;
            var allUsers = users.FindBy(u=>!u.Id.Equals(id));
            List<UserBasicInformation> usersInformation = new List<UserBasicInformation>();
            foreach (var user in allUsers)
            {
                UserBasicInformation information = new UserBasicInformation()
                {
                    Name = user.Name+" "+user.LastName ,
                    Email = user.Email,
                    Id = user.Id
                };
                usersInformation.Add(information);
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, usersInformation);
            return response;
        }

        [System.Web.Http.HttpPost]
        public HttpResponseMessage Post(User newUser)
        {
            var email = newUser.Email;
            var password = newUser.Password;
            bool created = users.Insert(newUser);
            if (created)
            {
                var userAutenticated = this.loginManager.AuthenticateUser(email, password);
                return Request.CreateResponse(HttpStatusCode.Created, userAutenticated);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Error");
            }
        }

        [System.Web.Http.HttpGet]
        [Route("api/user/{userName}")]
        public HttpResponseMessage Get(string userName)
        {
            var allUsers = users.FindBy(user => user.Name.Contains(userName) || user.LastName.Contains(userName));
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, allUsers);
            return response;
        }

        [System.Web.Http.HttpGet]
        [Route("api/user/user/{email}")]
        public HttpResponseMessage GetUser(string email)
        {
            var userResult = users.FindBy(user => user.Email.Equals(email));
            var result = new User { };
            if (userResult.Count() == 0)
            {
                result.Name = string.Empty;
            }
            else
            {
                result.Name = userResult.ElementAt(0).Name;
            }
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        /// <summary>
        /// This method help to get the name from user by id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [Route("api/user/id/{userId}")]
        [Authorize]
        public HttpResponseMessage GetNameById(string userId)
        {
            var id = "#13:" + userId;
            HttpResponseMessage response;
            try
            {
                var userResult = users.FindBy(user => user.Id.Equals(id)).Single();
                var userName = userResult.Name + " " + userResult.LastName;
                response = Request.CreateResponse(HttpStatusCode.OK, userName);
            }catch(Exception expection)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "No name");
            }
            return response;
        }

        [System.Web.Http.HttpGet]
        [Route("api/user/userInfo/{userId}")]
        [Authorize]
        public HttpResponseMessage GetUserInfo(string userId)
        {
            var id = "#13:" + userId;
            var userResult = users.FindBy(user => user.Id.Equals(id)).Single();
            var userInfo = new {
                Name = userResult.Name,
                LastName = userResult.LastName,
                Email = userResult.Email

            };

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, userInfo);
            return response;
        }

        /// <summary>
        /// Returns all Publications of a user.
        /// </summary>
        /// /// <returns> List Publications </returns>
        [System.Web.Http.HttpGet]
        [Route("api/user/publication/{userId}")]
        [Authorize]
        public HttpResponseMessage GetAllPublicationsOfAUser(string userId)
        {
            string userRid = "#13:" + userId;
            try
            {
                IEnumerable<Publication> allPublications = publicationRepository.FindBy(
                    publication => publication.UserId.Equals(userRid));
                return Request.CreateResponse(HttpStatusCode.OK, allPublications);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        [System.Web.Http.HttpOptions]
        public HttpResponseMessage Options()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
