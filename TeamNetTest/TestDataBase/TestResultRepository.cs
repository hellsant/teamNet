using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using Orient.Client;

namespace TeamNetTest.TestDataBase
{
    [TestClass]
    public class TestResultRepository
    {
        IRepository<Result360> resultRepository;

        [TestInitialize]
        public void SetUp()
        {
            this.resultRepository = UnitOfWorkTest.GetInstance().Result360Repository;
        }


        [TestMethod]
        public void TestGetSingleNoID()
        {
            string resultId = "#99:99";
            Assert.IsNull(resultRepository.GetSingle(resultId));
        }

        [TestMethod]
        public void TestGetSingle()
        {
            Result360 result = new Result360();
        }
    }
}
