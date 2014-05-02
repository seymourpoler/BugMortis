using System;
using System.Linq;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugMortis.Data.SqlServer.Repositories;
using BugMortis.Data.Entity;

namespace BugMortis.Data.SqlServer.Tests
{
    [TestClass]
    public class UserRepositoryTest : BaseRepositoryTest
    {
        private UserRepository _userRepository;
        private User _simpleUser, _userTwo;

        public UserRepositoryTest() : base(){}

        [TestInitialize]
        public void SetUp()
        {
            _userRepository = new UserRepository(_db);
            _simpleUser = new User() { Name = "Name" };
            _userTwo = new User() { Name = "Name II" };
            _simpleUser.IdUser = _userRepository.Insert(_simpleUser);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _userRepository.Delete(_simpleUser.IdUser);
            _userRepository.Delete(_userTwo.IdUser);
        }

        [TestMethod]
        public void UserRepository_should_save_simple_user()
        {
            var userExpected = _userRepository.GetById(_simpleUser.IdUser);
            
            Assert.AreEqual<User>(_simpleUser, userExpected);
        }

        [TestMethod]
        public void UserRepository_get_all_users()
        {
            _userRepository.Insert(_userTwo);
            var userExpected = _userRepository.GetAll();

            Assert.IsTrue(userExpected.Count() > 1);
        }

        [TestMethod]
        public void UserRepository_should_update_simple_user()
        {
            var userModified = _userRepository.GetById(_simpleUser.IdUser);
            userModified.Name = "CustomName";
            _userRepository.Update(userModified);
            var userExpected = _userRepository.GetById(_simpleUser.IdUser);
            
            Assert.AreEqual<User>(userModified, userExpected);
        }

        [TestMethod]
        public void UserRepository_delete_user()
        {
            _userRepository.Delete(_simpleUser.IdUser);
            var expected = _userRepository.GetById(_simpleUser.IdUser);

            Assert.IsNull(expected);
        }
    }
}
