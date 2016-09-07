using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Cleaner;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.TransactionToDatabase;
using TeamNetDB.Repository.OrientImplementation;
using TeamNetDB.Repository;
using System.Collections.Generic;

namespace TeamNetTest.TestDataBase
{
    [TestClass]
    public class TestGroup
    {
        /// <summary>
        /// Manage repository Suggestion.
        /// </summary>
        private IRepository<GroupData> groupRepository;
        /// <summary>
        /// Manages information database.
        /// </summary>
        private OrientDataBaseCleaner initializer;

        /// <summary>
        /// Initializes global variables.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.initializer = new OrientDataBaseCleaner();
            this.initializer.CleanDataBase();
            Transaction t = new Transaction(UnitOfWorkTest.GetInstance().GetConnection());
            this.groupRepository = UnitOfWorkTest.GetInstance().GroupRepository;
        }

        /// <summary>
        /// The method checks the insertion of a new group to the database administrator without members.
        /// </summary>
        [TestMethod]
        public void InsertNewGroupAloneAdministrator()
        {
            //this.initializer.SetUpInitialData();
            bool actual = this.groupRepository.Insert(this.InformationGroup(), null);
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// The method checks the insertion of a new group to the database administrator and members.
        /// </summary>
        [TestMethod]
        public void InsertNewGroupAndMembers()
        {
            //this.initializer.SetUpInitialData();
            bool actual = this.groupRepository.Insert(this.InformationGroupAndMembers(), null);
            Assert.IsTrue(actual);
        }

        private GroupData InformationGroup()
        {
            string name = "Team Suggestion";
            string description = "This group works to create new Suggestion";
            State state = State.Private;
            List<User> administrators = new List<User>();
                administrators.Add(new User(){Name = "Farid",Email="farid@jalasoft.com",Password = "F20142011",
                           LastName="Sanches"});
            GroupData group = new GroupData() {  Name=name, Description=description, State=state, Administrators= administrators};
            return group;
        }

        private GroupData InformationGroupAndMembers()
        {
            string name = "Team Suggestion";
            string description = "This group works to create new Suggestion";
            State state = State.Private;
            List<User> administrator = new List<User>();
            administrator.Add(new User
            {
                Name = "Farid",
                Email = "farid@jalasoft.com",
                Password = "F20142011",
                LastName = "Sanches"
            });

            List<User> users = new List<User>()
            {
                new User(){Name = "Aida",Email="aida@jalasoft.com",Password = "A20142011",
                           LastName = "Medrano"},
                new User(){Name = "Ivet",Email="ivet@jalasoft.com",Password = "I20142011",
                           LastName = "Rojas"},
            };
            GroupData group = new GroupData() { Name = name, Description = description, State = state,
                Administrators = administrator , Members = users};
            return group;
        }
    }
}
