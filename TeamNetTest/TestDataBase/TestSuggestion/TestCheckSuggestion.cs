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
namespace TeamNetDBTest.TestDataBase.TestSuggestion
{
    [TestClass]
    public class TestCheckSuggestion
    {
        /// <summary>
        /// Manage repository Suggestion.
        /// </summary>
        private IRepository<TeamNetDB.Model.OrientImplementation.SuggestionData> suggestionRepository;
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
            this.suggestionRepository = UnitOfWorkTest.GetInstance().SugestionRepository;
        }
        
        /// <summary>
        /// The method checks whether the correct amount of data.
        /// </summary>
        [TestMethod]
        public void TestInsertAndGetAll()
        {
            this.initializer.SetUpInitialData();
            var collection = this.suggestionRepository.GetAll();
            int actual = collection.Count();
            int expected = 3;
            Assert.AreEqual(expected, actual);
        }

    }
}
