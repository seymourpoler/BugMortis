using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using BugMortis.Domain;
using BugMortis.Domain.Entity;
using BugMortis.API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;

namespace BugMortis.API.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests : BaseControllerTests
    {
        private string url = "http://localhost/api/users";
        private Mock<IService<User>> _userService;
        private UsersController _usersController;
        private User _user;

        public UsersControllerTests()
        {
            _userService = new Mock<IService<User>>();
            _usersController = new UsersController(_userService.Object);
            _user = new User { IdUser = Guid.NewGuid(), Name = "Name User" };
        }

        [TestMethod]
        public void UsersController_get()
        {
            var users = new List<User> { 
                new User{ IdUser = Guid.NewGuid(), Name = "Name User One"},
                new User{ IdUser = Guid.NewGuid(), Name = "Name User Two"},
                new User{ IdUser = Guid.NewGuid(), Name = "Name User Three"},
                new User{ IdUser = Guid.NewGuid(), Name = "Name User Four"}
            };
            _userService.Setup(obj => obj.GetAll()).Returns(users);

            LoadRequestInController(_usersController, HttpMethod.Get, url);

            var response = _usersController.Get();

            var values = ((ObjectContent)(response.Content)).Value;
            List<User> responseData = (List<User>)(values);

            Assert.IsTrue(responseData.Any());
            _userService.Verify(service => service.GetAll(), Times.Once());

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_Get_Return_internal_server_error_When_there_is_an_exception()
        {
            _userService.Setup(service => service.GetAll()).Throws(new Exception());
            LoadRequestInController(_usersController, HttpMethod.Get, url);

            var response = _usersController.Get();

            _userService.Verify(service => service.GetAll(), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_GetById()
        {
            _userService.Setup(service => service.GetById(It.IsAny<Guid>())).Returns(_user);
            LoadRequestInController(_usersController, HttpMethod.Get, url);

            var response = _usersController.Get(Guid.NewGuid());
            var values = ((ObjectContent)(response.Content)).Value;
            User responseData = (User)(values);

            Assert.IsNotNull(responseData);
            Assert.IsInstanceOfType(responseData, typeof(User));
            _userService.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_GetById__return_Not_found_when_the_bug_doesnot_found()
        {
            _userService.Setup(service => service.GetById(It.IsAny<Guid>())).Returns(() => null);
            LoadRequestInController(_usersController, HttpMethod.Get, url);

            var response = _usersController.Get(Guid.NewGuid());

            _userService.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_GetById_Return_internal_server_error_when_there_is_an_exception()
        {
            _userService.Setup(service => service.GetById(It.IsAny<Guid>())).Returns(() => { throw new Exception(); });
            LoadRequestInController(_usersController, HttpMethod.Get, url);

            var response = _usersController.Get(Guid.NewGuid());

            _userService.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_Post()
        {
            _userService.Setup(service => service.IsValid(It.IsAny<User>(), out _errorMessages)).Returns(true);
            _userService.Setup(service => service.Insert(It.IsAny<User>())).Returns(Guid.NewGuid());
            LoadRequestInController(_usersController, HttpMethod.Post, url);

            var response = _usersController.Post(_user);

            _userService.Verify(service => service.Insert(It.IsAny<User>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_Post_Return_internal_server_error_when_there_is_an_exception()
        {
            _userService.Setup(service => service.IsValid(It.IsAny<User>(), out _errorMessages)).Returns(true);
            _userService.Setup(service => service.Insert(It.IsAny<User>())).Returns(() => { throw new Exception(); });
            LoadRequestInController(_usersController, HttpMethod.Post, url);

            var response = _usersController.Post(_user);

            _userService.Verify(service => service.IsValid(It.IsAny<User>(), out _errorMessages), Times.Once());
            _userService.Verify(service => service.Insert(It.IsAny<User>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_Post_Return_bad_request_when_the_name_of_the_model_is_empty()
        {
            LoadRequestInController(_usersController, HttpMethod.Post, url);
            var invalidModel = new User { Name = string.Empty };

            var response = _usersController.Post(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_Post_Return_bad_request_when_a_model_is_null()
        {
            LoadRequestInController(_usersController, HttpMethod.Post, url);
            var response = _usersController.Post(null);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_Put()
        {
            _userService.Setup(service => service.IsValid(It.IsAny<User>(), out _errorMessages)).Returns(true);
            _userService.Setup(service => service.Update(It.IsAny<User>()));
            LoadRequestInController(_usersController, HttpMethod.Put, url);

            var response = _usersController.Put(_user);

            _userService.Verify(service => service.Update(It.IsAny<User>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_Put_Return_internal_server_error_when_there_is_an_exception()
        {
            _userService.Setup(service => service.IsValid(It.IsAny<User>(), out _errorMessages)).Returns(true);
            _userService.Setup(service => service.Update(It.IsAny<User>())).Throws(new Exception());
            LoadRequestInController(_usersController, HttpMethod.Put, url);

            var response = _usersController.Put(_user);

            _userService.Verify(service => service.IsValid(It.IsAny<User>(), out _errorMessages), Times.Once());
            _userService.Verify(service => service.Update(It.IsAny<User>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_Put_Return_bad_request_when_the_name_of_the_model_is_empty()
        {
            LoadRequestInController(_usersController, HttpMethod.Put, url);
            var invalidModel = new User { Name = string.Empty };

            var response = _usersController.Put(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_Put_Return_bad_request_when_a_model_is_null()
        {
            LoadRequestInController(_usersController, HttpMethod.Put, url);
            var response = _usersController.Put(null);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_Delete()
        {
            _userService.Setup(service => service.Delete(It.IsAny<Guid>()));
            LoadRequestInController(_usersController, HttpMethod.Delete, url);

            var response = _usersController.Delete(Guid.NewGuid());

            _userService.Verify(service => service.Delete(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void UsersController_Delete_Return_internal_server_error_when_there_is_an_exception()
        {
            _userService.Setup(service => service.Delete(It.IsAny<Guid>())).Throws(new Exception());
            LoadRequestInController(_usersController, HttpMethod.Delete, url);

            var response = _usersController.Delete(Guid.NewGuid());

            _userService.Verify(service => service.Delete(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
