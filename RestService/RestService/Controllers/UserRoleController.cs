using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Repository.OrientImplementation;

namespace RestService.Controllers
{
    public class UserRoleController : ApiController
    {
        public static IRepository<TeamNetDB.Model.OrientImplementation.RoleInGroupData> roleInGroupRepository;
        public static IRepository<TeamNetDB.Model.OrientImplementation.GroupManagement> groupManagementRepository;

        /// <summary>
        /// Constructor that initialices a new userRole controller.
        /// </summary>
        public UserRoleController()
        {
            if (roleInGroupRepository == null || groupManagementRepository == null)
            {
                roleInGroupRepository = UnitOfWork.GetInstance().RoleInGroupRepository;
                groupManagementRepository = UnitOfWork.GetInstance().GroupManagementRepository;
            }
        }

        public Transaction Transaction { get; set; }

        /// <summary>
        /// Updates a RoleInGroupVertex given a modified one.
        /// </summary>
        /// <param name="userRoleUpdated">The object which contains the modifications for UserRoleInGroup.</param>
        /// <returns>An HttpResponseMessage which notifies wheter the vertex has been updated or not.</returns>
        [System.Web.Http.HttpPut]
        [Authorize]
        public HttpResponseMessage Put(RoleInGroupData userRoleUpdated)
        {
            bool isUpdated = roleInGroupRepository.Update(userRoleUpdated);
            if (isUpdated)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotModified);
            }
        }

        /// <summary>
        /// Inserts a new group into database
        /// </summary>
        /// <param name="groupManagement">The new groupManagement to be inserted</param>
        [System.Web.Http.HttpPost]
        [Authorize]
        public HttpResponseMessage Post(GroupManagement groupManagement)
        {
            bool created = groupManagementRepository.Insert(groupManagement);
            if (created)
            {
                return Request.CreateResponse(HttpStatusCode.Created, groupManagement);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Error trying to insert members");
            }
        }

        [System.Web.Http.HttpOptions]
        public HttpResponseMessage Options()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}