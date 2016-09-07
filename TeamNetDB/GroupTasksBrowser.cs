using System.Collections.Generic;
using System.Linq;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using TeamNetDB.Repository.Interfaces;

namespace TeamNetDB
{
    public class GroupTasksBrowser
    {
        private IRepository<GroupData> groupRepository;
        private IRepository<Task> taskRepository;

        public GroupTasksBrowser()
        {
            groupRepository = UnitOfWork.GetInstance().GroupRepository;
            taskRepository = UnitOfWork.GetInstance().TaskRepository;
        }

        /// <summary>
        /// Browses the tasks from users in the groupid given
        /// </summary>
        /// <param name="groupId">The id of the group where the tasks will be obtained from.</param>
        /// <returns>An IEnumerable collection which contains "All" the tasks from users in the group id given.</returns>
        public IEnumerable<Task> BrowseGroupTasks(string groupId)
        {
            IEnumerable<Task> tasksRecovered = new List<Task>();
            List<User> usersGroup = groupRepository.GetSingle(groupId).Members;
            foreach (User singleUser in usersGroup)
            {
                IEnumerable<Task> tasks = taskRepository.FindBy(user => user.Id.Equals(singleUser.Id));
                tasksRecovered.Concat(tasks);
            }
            return tasksRecovered;
        }
    }
}
