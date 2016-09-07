using RestService.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using TeamNetDB.Repository.Interfaces;

namespace RestService.Controllers
{
    public class GroupController : ApiController
    {
        public static IRepository<TeamNetDB.Model.OrientImplementation.GroupData> groupRepository;
        public static IRepository<Publication> publicationRepository;

        /// <summary>
        /// Constructor that initialices a new group repository.
        /// </summary>
        public GroupController()
        {
            if (groupRepository == null || publicationRepository == null)
            {
                groupRepository = UnitOfWork.GetInstance().GroupRepository;
                publicationRepository = UnitOfWork.GetInstance().PublicationRepository;
            }
        }

        /// <summary>
        /// Gets a single group using the id given as a parameter to make the comparitions.
        /// </summary>
        /// <param name="groupId">the id of the group to be recovered.</param>
        /// <returns>An HttpResponseMessage value which is used in frontend</returns>
        [System.Web.Http.HttpGet]
        [Route("api/group/{groupId}")]
        [Authorize]
        public HttpResponseMessage GetGroupsById(string groupId)
        {
            string id = "#43:" + groupId;
            try
            {
                IEnumerable<GroupData> groupById = groupRepository.FindBy(group => group.Id.Equals(id));
                return Request.CreateResponse(HttpStatusCode.OK, groupById);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        /// <summary>
        /// Returns all Publications of a group.
        /// </summary>
        /// /// <returns> List Publications </returns>
        [System.Web.Http.HttpGet]
        [Route("api/group/publication/{groupId}")]
        [Authorize]
        public HttpResponseMessage GetAllPublicationsOfAGroup(string groupId)
        {
            string groupRid = "#43:" + groupId;
            try
            {
                IEnumerable<Publication> allPublications = publicationRepository.FindBy(
                    publication => publication.GroupsIds.Contains(groupRid));
                return Request.CreateResponse(HttpStatusCode.OK, allPublications);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        /// <summary>
        /// Gets all Groups in database
        /// </summary>
        /// <returns>an HttpResponseMessage which contains all groupRepository in database</returns>
        [System.Web.Http.HttpGet]
        [Authorize]
        public HttpResponseMessage GetAll()
        {
            try
            {
                IEnumerable<GroupData> allGroups = groupRepository.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, allGroups);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                Logger.Instance().Error(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        /// <summary>
        /// Inserts a new group into database
        /// </summary>
        /// <param name="group">The new group to be inserted</param>
        [System.Web.Http.HttpPost]
        [Authorize]
        public HttpResponseMessage Post(GroupData group)
        {
            bool created = groupRepository.Insert(group);
            //var collection = groupRepository.FindBy(e => e.Name.Equals(group.Name));
            //if (collection == null)
            //{
                if (created)
                {
                   return Request.CreateResponse(HttpStatusCode.Created, group);
                }
                else
                {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Error trying to insert a group");
                }   
            //}
            //else
            //{
              //  return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Error exists a group with that name");
            //}
            
        }

        /// <summary>
        /// Updates a group in database given the group with its parameters changed.
        /// </summary>
        /// <param name="group">The group with changed parameters.</param>
        /// <returns>An HttpResponseMessage which notifies if the group is updated or not.</returns>
        [System.Web.Http.HttpPut]
        [Authorize]
        public HttpResponseMessage Put(GroupData group)
        {
            bool updated = groupRepository.Update(group);
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
        /// Deletes a group given from database 
        /// </summary>
        /// <param name="group">the group to delete</param>
        /// <returns>An HttpResponseMessage which notifies if the group is deleted or not</returns>
        [System.Web.Http.HttpDelete]
        [Authorize]
        public HttpResponseMessage Delete(GroupData group)
        {
            bool isDeleted = groupRepository.Delete(group);
            if (isDeleted)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }
        }

        [System.Web.Http.HttpOptions]
        public HttpResponseMessage Options()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}