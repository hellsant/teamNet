using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Cleaner;
using TeamNetDB.Repository;
using System.Collections.Generic;
using System.Linq;

namespace TeamNetTest.TestDataBase
{
    [TestClass]
    public class TestTaskWithUserAndCompetency
    {
        IRepository<TeamNetDB.Model.OrientImplementation.Task> taskRepository;
        IRepository<TeamNetDB.Model.OrientImplementation.User> userRepository;
        IRepository<TeamNetDB.Model.OrientImplementation.Competency> competencyRepository;
        OrientDataBaseCleaner initializer;

        [TestInitialize]
        public void SetUp()
        {
            this.initializer = new OrientDataBaseCleaner();
            this.initializer.CleanDataBase();
            this.taskRepository = UnitOfWorkTest.GetInstance().TaskRepository;
            this.userRepository = UnitOfWorkTest.GetInstance().UserRepository;
            this.competencyRepository = UnitOfWorkTest.GetInstance().CompetencyRepository;
        }

        [TestMethod]
        public void TestGetAllTasksFromUser()
        {
            this.initializer.SetUpInitialData();
            
            //Getting user id from user repository
            string userEmail = this.initializer.DefaultUsers[0].Email;
            var obtainedUser = this.userRepository.FindBy(user => user.Email.Equals(userEmail)).Single();
            var idUser = obtainedUser.Id;

            //Getting all tasks from user id
            var obtainedTasks = this.taskRepository.FindBy(task => task.UserId.Equals(idUser));
            int obtainedTasksSize = obtainedTasks.Count();

            int expectedTasks = this.initializer.DefaultTasks.Count;

            Assert.AreEqual(expectedTasks, obtainedTasksSize);
        }

        [TestMethod]
        public void TestGetAllTasksFromCompetency()
        {
            this.initializer.SetUpInitialData();

            //Getting competency id from competency repository
            string competencyName = this.initializer.DefaultCompetencies[0].Name;
            var obtainedCompetency = this.competencyRepository.FindBy(competency => competency.Name.Equals(competencyName)).Single();
            var idCompetency = obtainedCompetency.Id;

            //Getting all tasks from competency id
            var obtainedTasks = this.taskRepository.FindBy(task => task.CompetencyId.Equals(idCompetency));
            int obtainedTasksSize = obtainedTasks.Count();

            int expectedTasks = this.initializer.DefaultTasks.Count;

            Assert.AreEqual(expectedTasks, obtainedTasksSize);
        }

        [TestMethod]
        public void TestGetAllTasksFromCompetencyAndUser()
        {
            this.initializer.SetUpInitialData();

            //Getting competency id from competency repository
            string competencyName = this.initializer.DefaultCompetencies[0].Name;
            var obtainedCompetency = this.competencyRepository.FindBy(c => c.Name.Equals(competencyName)).Single();
            var idCompetency = obtainedCompetency.Id;

            //Getting user id from user repository
            string emailUser = this.initializer.DefaultUsers[0].Email;
            var obtainedUser = this.userRepository.FindBy(u => u.Email.Equals(emailUser)).Single();
            var idUser = obtainedUser.Id;

            //Getting all tasks from competency id
            var obtainedTasks = this.taskRepository.FindBy(t => t.CompetencyId.Equals(idCompetency) && t.UserId.Equals(idUser));
            int obtainedTasksSize = obtainedTasks.Count();

            int expectedTasks = this.initializer.DefaultTasks.Count;

            Assert.AreEqual(expectedTasks, obtainedTasksSize);
        }
    }
}
