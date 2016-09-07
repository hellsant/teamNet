using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamNetDB.Model;
using TeamNetDB.Repository;
using System.Collections.Generic;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using System.Linq;
using System.Linq.Expressions;
using TeamNetDB.Model.Interfaces;
using TeamNetDB.Cleaner;
using TeamNetDB.Repository.OrientImplementation;

namespace TeamNetTest.TestDataBase
{
    [TestClass]
    public class TestTimeRepository
    {
        /// <summary>
        /// Manage repository Suggestion.
        /// </summary>
        private IRepository<TeamNetDB.Model.OrientImplementation.Time> timeRepository;
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
            this.timeRepository = UnitOfWorkTest.GetInstance().TimeRepository;
        }
        
        /// <summary>
        /// Check if a return and is correct from the database.
        /// </summary>
        [TestMethod]
        public void TestGetAllTime()
        {
            this.initializer.SetUpInitialData();
            var collection = this.timeRepository.GetAll();
            int actual = collection.Count();
            int expected = 4;
            Assert.AreEqual(expected, actual);
        }

    }
}
