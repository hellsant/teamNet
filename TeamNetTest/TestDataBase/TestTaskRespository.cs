using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Cleaner;
using System.Linq;
using System.Linq.Expressions;
using TeamNetDB.Repository;
using TeamNetDB.Repository.OrientImplementation;
using TeamNetDB.Model.OrientImplementation;

namespace TeamNetTest.TestDataBase
{
    [TestClass]
    public class TestTaskRespository
    {
        IRepository<TeamNetDB.Model.OrientImplementation.Task> taskRepository;
        OrientDataBaseCleaner initializer;

        [TestInitialize]
        public void SetUp()
        {
            this.initializer = new OrientDataBaseCleaner();
            this.initializer.CleanDataBase();
            Transaction t = new Transaction(UnitOfWorkTest.GetInstance().GetConnection());
            this.taskRepository = UnitOfWorkTest.GetInstance().TaskRepository;
        }

        [TestMethod]
        public void TestAfterCleaningThereShouldNotBeAnyTask()
        {
            var result = this.taskRepository.GetAll();
            int currentSize = result.Count();
            int emptySize = 0;
            Assert.AreEqual(emptySize, currentSize);
        }

        [TestMethod]
        public void TestInsertDefaultTasksShouldCreateNewEntitiesOnDataBase()
        {
            this.initializer.SetUpInitialData();
            string firtsUserId = this.initializer.DefaultUsers[0].Id;
            int tasksFromUserId = this.taskRepository.FindBy(e => e.UserId == firtsUserId).Count();
            int expected = this.initializer.DefaultTasks.Count;
            Assert.AreEqual(expected, tasksFromUserId);
        }

        [TestMethod]
        public void TestUpdateExistingTask()
        {
            this.initializer.SetUpInitialData();
            var userId = this.initializer.DefaultUsers[0].Id;
            var taskOrid = this.taskRepository.FindBy(e => e.UserId == userId).ToList()[0].Id;
            
            
            TeamNetDB.Model.OrientImplementation.Task task = new TeamNetDB.Model.OrientImplementation.Task();
            task.Progress = 100;
            task.Name = "Learn english";
            task.Description = "View pictures and series";
            task.ReviewerId = " team";
            task.IsSuggestion = false;
            task.State = "disabled";
            task.InitialDate = "2014-02-01";
            task.EndDate = "2014-03-02";
            task.Id = taskOrid;

            this.taskRepository.Update(task);

            var updatedTask = this.taskRepository.GetSingle(taskOrid);
            Assert.AreEqual(task.Name, updatedTask.Name);
            Assert.AreEqual(task.Description, updatedTask.Description);
            Assert.AreEqual(task.State, updatedTask.State);
        }

        [TestMethod]
        public void TestSelectAllTasksFromUser()
        {
            this.initializer.SetUpInitialData();
            var userId = this.initializer.DefaultUsers[0].Id;
            var allTasks = this.taskRepository.FindBy(t => t.UserId.Equals(userId));
            int expectedFromCleaner = this.initializer.DefaultTasks.Count;
            int currentTasks = allTasks.Count();
            Assert.AreEqual(expectedFromCleaner, currentTasks);
        }

        [TestMethod]
        public void TestSelectAllTaskFromUserAndCompetency()
        {
            this.initializer.SetUpInitialData();
            var userId = this.initializer.DefaultUsers[0].Id;
            var pointId = this.initializer.DefaultCompetencies[0].Id;
         
            
            var allTasks = this.taskRepository.FindBy(t => t.UserId.Equals(userId)&&t.CompetencyId.Equals(pointId));

            
            int expectedFromCleaner = this.initializer.DefaultTasks.Count;
            int currentTasks = allTasks.Count();
            Assert.AreEqual(expectedFromCleaner, currentTasks);
        }

        [TestMethod]
        public void TestSelectAllTasksFromUserAndInvalidCompetency()
        {
            this.initializer.SetUpInitialData();
            var userId = this.initializer.DefaultUsers[0].Id;
            var invalidCompetencytId = this.initializer.DefaultCompetencies[0].Id+"invalidText";
            var allTasks = this.taskRepository.FindBy(t => t.UserId.Equals(userId) && t.CompetencyId.Equals(invalidCompetencytId));
            int expected = 0;
            int currentTasks = allTasks.Count();
            Assert.AreEqual(expected, currentTasks);
        }

        [TestMethod]
        public void TestGetSingleWithInvalidId()
        {
            this.initializer.SetUpInitialData();
            string inventedId = "123";
            var task = this.taskRepository.GetSingle(inventedId);
            Assert.IsNull(task);
        }

        [TestMethod]
        public void TestFindByWithInvalidUserId()
        {
            this.initializer.SetUpInitialData();
            string inventedUserId = "123";
            var tasks = this.taskRepository.FindBy(t => t.UserId.Equals(inventedUserId));
            Assert.IsFalse(tasks.Count() > 0);
        }

        [TestMethod]
        public void TestDelete()
        {
            this.initializer.SetUpInitialData();
            var userId = this.initializer.DefaultUsers[0].Id;
            var task = this.taskRepository.FindBy(e => e.UserId == userId).ToList()[0];
            Assert.IsTrue(taskRepository.Delete(task));
            var taskId = task.Id;

            var newTask = this.taskRepository.GetSingle(taskId);
            Assert.IsNull(newTask);
        }

        [TestMethod]
        public void TestUpdateInsertedTask()
        {
            this.initializer.SetUpInitialData();
            var userId = this.initializer.DefaultUsers[0].Id;
            var taskOrid = this.taskRepository.FindBy(u => u.UserId.Equals(userId)).ToList()[0].Id;
            Task task = new Task();
            task.Progress = 100;
            task.Name = "Learn english";
            task.Description = "View pictures and series";
            task.ReviewerId = " team";
            task.IsSuggestion = false;
            task.State = "disabled";
            task.InitialDate = "2014-10-17";
            task.EndDate = "2014-12-31";
            task.Id = taskOrid;
            Assert.IsTrue(this.taskRepository.Update(task));
        }

        [TestMethod]
        public void TestInsertTaskWithUserIdNullShouldNotInsertIt()
        {
            Task taskWithoutUserId = new Task();
            taskWithoutUserId.Name = "Learn english";
            taskWithoutUserId.Description = "View pictures and series";
            Assert.IsFalse(this.taskRepository.Insert(taskWithoutUserId));
        }

        [TestMethod]
        public void TestInsertTaskWithCompetencyIdNullShouldNotInsertIt()
        {
            Task taskWithoutCompetencyId = new Task();
            taskWithoutCompetencyId.Name = "Learn english";
            taskWithoutCompetencyId.Description = "View pictures and series";
         
            Assert.IsFalse(this.taskRepository.Insert(taskWithoutCompetencyId));
        }

        [TestMethod]
        public void TestInsertTaskWithEndDateNullShouldNotInsertIt()
        {
            Task taskWithEndDateNull = new Task();
            taskWithEndDateNull.Name = "Learn english";
            taskWithEndDateNull.Description = "View pictures and series";         
            Assert.IsFalse(this.taskRepository.Insert(taskWithEndDateNull));
        }

        [TestMethod]
        public void TestGivenTaskWithEndDateEqualsToInitialDateShouldInsertIt()
        {
            this.initializer.SetUpInitialData();
            string userId = this.initializer.DefaultUsers[0].Id;
            string competencyId = this.initializer.DefaultCompetencies[0].Id;
            Task taskWithEndDateEqualsInitialDate = new Task();
            taskWithEndDateEqualsInitialDate.UserId = userId;
            taskWithEndDateEqualsInitialDate.CompetencyId = competencyId;
            taskWithEndDateEqualsInitialDate.Name = "Learn english";
            taskWithEndDateEqualsInitialDate.Description = "View pictures and series";
            string now = "2015-01-12";
            taskWithEndDateEqualsInitialDate.InitialDate = now;
            taskWithEndDateEqualsInitialDate.EndDate = now;
            Assert.IsTrue(this.taskRepository.Insert(taskWithEndDateEqualsInitialDate));
        }

        [TestMethod]
        public void TestGivenTaskWithEndDateGraterThanInitialDateShouldInsertIt()
        {
            this.initializer.SetUpInitialData();
            string userId = this.initializer.DefaultUsers[0].Id;
            string competencyId = this.initializer.DefaultCompetencies[0].Id;
            Task taskWithEndDateEqualsInitialDate = new Task();
            taskWithEndDateEqualsInitialDate.UserId = userId;
            taskWithEndDateEqualsInitialDate.CompetencyId = competencyId;
            taskWithEndDateEqualsInitialDate.Name = "Learn english";
            taskWithEndDateEqualsInitialDate.Description = "View pictures and series";
            string initialDate = "2015-01-12";
            string endDate = "2015-02-12";
            taskWithEndDateEqualsInitialDate.InitialDate = initialDate;
            taskWithEndDateEqualsInitialDate.EndDate = endDate;
            Assert.IsTrue(this.taskRepository.Insert(taskWithEndDateEqualsInitialDate));
        }
    }
}
