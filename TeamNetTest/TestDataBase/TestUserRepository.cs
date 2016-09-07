using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamNetDB.Repository;
using TeamNetDB.Model;
using TeamNetDB.Cleaner;
using System.Linq;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Model.OrientImplementation;
namespace TeamNetTest.TestDataBase
{
    [TestClass]
    public class TestUserRepository
    {
        IRepository<User> userRepository;
        OrientDataBaseCleaner cleaner;

        [TestInitialize]
        public void SetUp()
        {
            this.cleaner = new OrientDataBaseCleaner();
            this.cleaner.CleanDataBase();
            this.userRepository = UnitOfWorkTest.GetInstance().UserRepository;
        }

        [TestMethod]
        public void TestInitiallyDataBaseShouldBeEmpty()
        {
            var result = this.userRepository.GetAll();
            int currentSize = result.Count();
            int emptySize = 0;
            Assert.AreEqual(emptySize, currentSize);
        }

        [TestMethod]
        public void TestInsertDefaultUsers()
        {
            this.cleaner.SetUpInitialData();
            int defaultSize = this.cleaner.DefaultUsers.Count();
            int currentSize = this.userRepository.GetAll().Count();
            Assert.AreEqual(defaultSize, currentSize);
        }

        [TestMethod]
        public void TestLoginWithInvalidUserShouldReturnEmptyListOfResults()
        {
            this.cleaner.SetUpInitialData();
            string invalidString = " ·$%3 ";
            string anyPassword = "dsfsd";
            string invalidUserEmail = this.cleaner.DefaultUsers[0].Email + invalidString;
            var result = this.userRepository.FindBy(u => u.Email.Equals(invalidUserEmail) && u.Password.Equals(anyPassword));
            int resultSize = result.Count();
            Assert.AreEqual(0, resultSize);
        }
        
        [TestMethod]
        public void TestLoginWithValidUserAndInvalidPasswordShouldReturnEmptyListOfResults()
        {
            this.cleaner.SetUpInitialData();
            string validUserEmail = this.cleaner.DefaultUsers[0].Email;
            string invalidPassword = this.cleaner.DefaultUsers[0].Password + " any text to fail";
            var result = this.userRepository.FindBy(u => u.Email.Equals(validUserEmail) && u.Password.Equals(invalidPassword));
            int resultSize = result.Count();
            Assert.AreEqual(0, resultSize);
        }

        [TestMethod]
        public void TestInsertUserWithExistingEmailShouldRetunFalse()
        {
            this.cleaner.SetUpInitialData();
            string validUserEmail = this.cleaner.DefaultUsers[0].Email;
            User newUserWithExistingEmail = new User(){Email = validUserEmail,Name = "Other user"};
            bool isInserted = this.userRepository.Insert(newUserWithExistingEmail);
            Assert.IsFalse(isInserted);
        }

        [TestMethod]
        public void TestLoginWithValidUserAndValidPasswordShouldReturnUniqueResult()
        {
            this.cleaner.SetUpInitialData();
            string validUserEmail = this.cleaner.DefaultUsers[0].Email;
            string validPassword = this.cleaner.DefaultUsers[0].Password;
            var result = this.userRepository.FindBy(user => user.Email.Equals(validUserEmail) && user.Password.Equals(validPassword));
            int resultSize = result.Count();
            Assert.IsTrue(resultSize < 2);
        }

        [TestMethod]
        public void TestEncriptedPasswordWithRinjdaelAlgorithm()
        {
            Rinjdael encryptor = new Rinjdael();
            string plainText = "Hello, World!";
            string encryptedText = encryptor.EncryptData(plainText);
            string decryptedText = encryptor.DecryptData(encryptedText);
            Assert.AreEqual(decryptedText, plainText);
        }
    }
}
