using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamNetDB;
using TeamNetDB.Model.OrientImplementation;

namespace RestService.Controllers
{
    public class GroupTasksController : ApiController
    {
        public static GroupTasksBrowser browser = new GroupTasksBrowser();

        /// <summary>
        /// Gets the tasks of users in the group given its name.
        /// </summary>
        /// <param name="groupName">The name of the group from which the task is being obtained</param>
        /// <returns>An HttpResponseMessage that notifies if the task has been obtained</returns>
        [System.Web.Http.HttpGet]
        [Route("api/groupTasks/{groupId}")]
        [Authorize]
        public HttpResponseMessage Get(string groupId)
        {
            string id = "#43:" + groupId;
            IList<Task> taskRecovered = browser.BrowseGroupTasks(id).ToList();
            if (taskRecovered.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.Created, taskRecovered);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, "Error while getting tasks");
            }
        }
    }
}