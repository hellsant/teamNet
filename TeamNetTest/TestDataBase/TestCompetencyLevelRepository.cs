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
    public class TestCompetencyLevelRepository
    {
        /// <summary>
        /// Manage repository Suggestion.
        /// </summary>
        private IRepository<TeamNetDB.Model.OrientImplementation.CompetencyLevel> competencyLevelRepository;
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
            this.competencyLevelRepository = UnitOfWorkTest.GetInstance().CompetencyLevelRepository;
        }
        
        /// <summary>
        /// Check if the query data to the database are correct.
        /// </summary>
        [TestMethod]
        public void TestGetAllComptencyLevel()
        {
            this.initializer.SetUpInitialData();
            var collection = this.competencyLevelRepository.GetAll();
            int actual = collection.Count();
            int expected = 3;
            Assert.AreEqual(expected, actual);
        }
    }
}
