using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Repository.OrientImplementation;
using TeamNetDB.Repository;
using TeamNetDB.Model.OrientImplementation;

namespace TeamNetTest.TestDataBase
{
    /// <summary>
    /// Summary description for TestPublicationRepository
    /// </summary>
    [TestClass]
    public class TestPublicationRepository
    {
        private IRepository<TeamNetDB.Model.OrientImplementation.Publication> publicationRepository;

        [TestInitialize]
        public void SetUp()
        {
            Transaction t = new Transaction(UnitOfWorkTest.GetInstance().GetConnection());
            this.publicationRepository = UnitOfWorkTest.GetInstance().PublicationRepository;
        }

        [TestMethod]
        public void TestGetAllPublications()
        {
            List<Publication> publications = this.publicationRepository.GetAll() as List<Publication>;
            int result = publications.Count;
            int expected = 3;
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInsertPublication()
        {
            Publication publication = new Publication() { Publicator = "Ivet",
                                                          Comment = "Finished my Research about Design Patterns",
                                                          Date = "2015-01-15",
                                                          Task = "github.DesignPatterns.com"
                                                        };
            bool result = this.publicationRepository.Insert(publication);
            Assert.IsTrue(result);
        }
    }
}
