using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugMortis.Data;

namespace BugMortis.Domain.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private Domain.Entity.User _user;
        private Mock<IRepository<Data.Entity.User>> _userRepository;
        private UserService _userService;

        public UserServiceTests()
        {
            _user = new Domain.Entity.User { Name = "Name User" };
            _userRepository = new Mock<IRepository<Data.Entity.User>>();
            _userService = new UserService(_userRepository.Object);
        }

        [TestMethod]
        public void UserService_IsValid_return_false_when_name_is_empty()
        {
            var user = new Domain.Entity.User();
            var errorMessages = new List<string>();
            var expected = _userService.IsValid(user, out errorMessages);

            Assert.IsFalse(expected);
            Assert.IsTrue(errorMessages.Any());
        }

        [TestMethod]
        public void UserService_IsValid_return_false_when_user_is_null()
        {
            var errorMessages = new List<string>();
            var expected = _userService.IsValid(null, out errorMessages);

            Assert.IsFalse(expected);
            Assert.IsTrue(errorMessages.Any());
        }

        [TestMethod]
        public void UserService_getbyid_simple_user()
        {
            var idUser = Guid.NewGuid();
            var dataUser = new Data.Entity.User { Name = "data User" };
            _userRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(dataUser);

            var result = _userService.GetById(idUser);

            _userRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once());
        }

        [TestMethod]
        public void UserService_get_all_users()
        {
            var users = new List<Data.Entity.User> { 
                new Data.Entity.User { IdUser = Guid.NewGuid(), Name = "user One" },
                new Data.Entity.User { IdUser = Guid.NewGuid(), Name = "user Two" },
                new Data.Entity.User { IdUser = Guid.NewGuid(), Name = "user Three" },
            };
            _userRepository.Setup(x => x.GetAll()).Returns(users);

            var result = _userService.GetAll();

            Assert.IsTrue(result.Any());
            _userRepository.Verify(x => x.GetAll(), Times.Once());
        }

        [TestMethod]
        public void UserService_insert_simple_user()
        {
            var idUser = Guid.NewGuid();
            
            _userRepository.Setup(x => x.Insert(It.IsAny<Data.Entity.User>())).Returns(idUser);
            var userService = new UserService(_userRepository.Object);
            
            var result = userService.Insert(_user);

            _userRepository.Verify(x => x.Insert(It.IsAny<Data.Entity.User>()), Times.Once());
            Assert.AreEqual(result, idUser);
        }

        [TestMethod]
        public void UserService_update_simple_user()
        {
            _userRepository.Setup(x => x.Update(It.IsAny<Data.Entity.User>()));
            var userService = new UserService(_userRepository.Object);
            
            userService.Update(_user);

            _userRepository.Verify(x => x.Update(It.IsAny<Data.Entity.User>()), Times.Once());
        }

        [TestMethod]
        public void UserService_delete_simple_user()
        {
            var idUser = Guid.NewGuid();
            _userRepository.Setup(x => x.Delete(It.IsAny<Guid>()));
            var userService = new UserService(_userRepository.Object);

            userService.Delete(idUser);

            _userRepository.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Once());
        }
    }
}
