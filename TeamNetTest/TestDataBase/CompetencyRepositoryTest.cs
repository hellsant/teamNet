using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamNetDB.Cleaner;
using TeamNetDB.Repository;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Model.OrientImplementation;
using System.Collections.Generic;

namespace TeamNetTest.TestDataBase
{
    [TestClass]
    public class CompetencyRepositoryTest
    {
        IRepository<Competency> competencyRepository;
        OrientDataBaseCleaner cleaner;

        [TestInitialize]
        public void SetUp()
        {
            this.cleaner = new OrientDataBaseCleaner();
            this.cleaner.CleanDataBase();
            this.competencyRepository = UnitOfWorkTest.GetInstance().CompetencyRepository;
        }

        [TestMethod]
        public void TestInsertDefaultCompetencies()
        {
            this.cleaner.SetUpInitialData();
            int defaultCompetencies = cleaner.DefaultCompetencies.Count;
            int insertedCompetencies = 2;
            Assert.AreEqual(defaultCompetencies, insertedCompetencies);
        }

        [TestMethod]
        public void TestGetAllCompetencies()
        {
            this.cleaner.SetUpInitialData();
            IList<Competency> obtainedCompetencies = competencyRepository.GetAll() as IList<Competency>;
            int competenciesSizeExpected = 2;
            Assert.AreEqual(competenciesSizeExpected, obtainedCompetencies.Count);
        }

        [TestMethod]
        public void TestDeleteCompetency()
        {
            this.cleaner.SetUpInitialData();
            IList<Competency> obtainedCompetencies = competencyRepository.GetAll() as IList<Competency>;
            Assert.IsTrue(competencyRepository.Delete(obtainedCompetencies[0]));
        }

        [TestMethod]
        public void TestUpdateCompetency()
        {
            this.cleaner.SetUpInitialData();
            IList<Competency> obtainedCompetencies = competencyRepository.GetAll() as IList<Competency>;
            Competency obtainedCompetency = obtainedCompetencies[0];
            obtainedCompetency.Name = "Integration";
            Assert.IsTrue(competencyRepository.Update(obtainedCompetency));
        }
    }
}
