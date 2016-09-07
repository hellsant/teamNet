using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using TeamNetDB.Repository.Interfaces;

namespace RestService.Controllers
{
    public class RoleController : ApiController
    {
        public static IRepository<TeamNetDB.Model.OrientImplementation.Role> roleRepository;

        public RoleController()
        {
            if (roleRepository == null)
            {
                roleRepository = UnitOfWork.GetInstance().RoleRepository;
            }
        }

        /// <summary>
        /// Gets all roles available for groups
        /// </summary>
        /// <returns>an HttpResponseMessage wich contains all roles available.</returns>
        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage GetAll()
        {
            try
            {
                IEnumerable<Role> roles = roleRepository.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, roles);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        /// <summary>
        /// Gets a Role given its name.
        /// </summary>
        /// <param name="roleName">The name of the role to be obtained.</param>
        /// <returns>An HttpResponseMessage wich notifies if the role exists or not.</returns>
        [System.Web.Http.HttpGet]
        [Route("api/role/{roleName}")]
        [Authorize]
        public HttpResponseMessage GetByName(string roleName)
        {
            try
            {
                Role roles = roleRepository.GetSingle(roleName);
                return Request.CreateResponse(HttpStatusCode.OK, roles);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "None role matches the name");
            }
        }

        /// <summary>
        /// Inserts a new Role into database
        /// </summary>
        /// <param name="newRole">The role to be inserted.</param>
        /// <returns>An HttpResponseMessage which notifies wether the role has benn inserted or not.</returns>
        [System.Web.Http.HttpPost]
        [Authorize]
        public HttpResponseMessage Post(Role newRole)
        {
            bool created = roleRepository.Insert(newRole);
            if (created)
            {
                return Request.CreateResponse(HttpStatusCode.Created, newRole);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Error trying to insert a role");
            }   
        }

        /// <summary>
        /// Updates a role given which contains the its self changes.
        /// </summary>
        /// <param name="role">the role which contains the changes.</param>
        /// <returns>An HttpResponseMessage which notifies wether the role has been updated or not.</returns>
        [System.Web.Http.HttpPut]
        [Authorize]
        public HttpResponseMessage Put(Role role)
        {
            bool updated = roleRepository.Update(role);
            if (updated)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }
        }

        /// <summary>
        /// Deletes a Role from database given the role to be eliminated for.
        /// </summary>
        /// <param name="role">The role to be eliinated.</param>
        /// <returns>An HttpResponseMessage which notifies wether the Role has been deleted or not.</returns>
        [System.Web.Http.HttpDelete]
        [Authorize]
        public HttpResponseMessage Delete(Role role)
        {
            bool isDeleted = roleRepository.Delete(role);
            if (isDeleted)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }
        }
    }
}