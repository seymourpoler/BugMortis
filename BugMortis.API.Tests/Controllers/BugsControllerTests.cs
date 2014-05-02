using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using BugMortis.API.Controllers;
using BugMortis.Domain;
using BugMortis.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;

namespace BugMortis.API.Tests.Controllers
{
    [TestClass]
    public class BugsControllerTests : BaseControllerTests
    {
        private string url = "http://localhost/api/bugs";
        private Mock<IService<Bug>> _bugService;
        private BugsController _bugsController;
        private Bug _bug;

        public BugsControllerTests()
        {
            _bugService = new Mock<IService<Bug>>();
            _bugsController = new BugsController(_bugService.Object);
            _bug = new Bug { IdBug = Guid.NewGuid(), Name = "Bug Three", IdPriority = Guid.NewGuid(), IdProject = Guid.NewGuid() };
        }

        [TestMethod]
        public void BugsController_Get()
        {
            var bugs = new List<Bug> { 
                new Bug{ IdBug = Guid.NewGuid(), Name = "Bug One", IdPriority = Guid.NewGuid(), IdProject = Guid.NewGuid()},
                new Bug{ IdBug = Guid.NewGuid(), Name = "Bug Two", IdPriority = Guid.NewGuid(), IdProject = Guid.NewGuid()},
                new Bug{ IdBug = Guid.NewGuid(), Name = "Bug Three", IdPriority = Guid.NewGuid(), IdProject = Guid.NewGuid()}
            };
            _bugService.Setup(service => service.GetAll()).Returns(bugs);

            LoadRequestInController(_bugsController, HttpMethod.Get, url);
            var response = _bugsController.Get();
            

            var values = ((ObjectContent)(response.Content)).Value;
            List<Bug> responseData = (List<Bug>)(values);

            Assert.IsTrue(responseData.Any());
            _bugService.Verify(service => service.GetAll(), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Get_Return_internal_server_error_When_there_is_an_exception()
        {
            _bugService.Setup(service => service.GetAll()).Throws(new Exception());
            LoadRequestInController(_bugsController, HttpMethod.Get, url);

            var response = _bugsController.Get();
            _bugService.Verify(service => service.GetAll(), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_GetById()
        {
            _bugService.Setup(service => service.GetById(It.IsAny<Guid>())).Returns(_bug);
            LoadRequestInController(_bugsController, HttpMethod.Get, url);

            var response = _bugsController.Get(Guid.NewGuid());
            var values = ((ObjectContent)(response.Content)).Value;
            Bug responseData = (Bug)(values);

            Assert.IsNotNull(responseData);
            Assert.IsInstanceOfType(responseData, typeof(Bug));
            _bugService.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_GetById__return_Not_found_when_the_bug_doesnot_found()
        {
            _bugService.Setup(service => service.GetById(It.IsAny<Guid>())).Returns(() => null);
            LoadRequestInController(_bugsController, HttpMethod.Get, url);

            var response = _bugsController.Get(Guid.NewGuid());

            _bugService.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_GetById_Return_internal_server_error_when_there_is_an_exception()
        {
            _bugService.Setup(service => service.GetById(It.IsAny<Guid>())).Returns(() => { throw new Exception(); });
            LoadRequestInController(_bugsController, HttpMethod.Get, url);

            var response = _bugsController.Get(Guid.NewGuid());

            _bugService.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_Post()
        {
            _bugService.Setup(service => service.IsValid(It.IsAny<Bug>(), out _errorMessages)).Returns(true);
            _bugService.Setup(service => service.Insert(It.IsAny<Bug>())).Returns(Guid.NewGuid());
            LoadRequestInController(_bugsController, HttpMethod.Post, url);

            var response = _bugsController.Post(_bug);

            _bugService.Verify(service => service.Insert(It.IsAny<Bug>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Post_Return_internal_server_error_when_there_is_an_exception()
        {
            _bugService.Setup(service => service.IsValid(It.IsAny<Bug>(), out _errorMessages)).Returns(true);
            _bugService.Setup(service => service.Insert(It.IsAny<Bug>())).Returns(() => { throw new Exception(); });
            LoadRequestInController(_bugsController, HttpMethod.Post, url);

            var response = _bugsController.Post(_bug);

            _bugService.Verify(service => service.IsValid(It.IsAny<Bug>(), out _errorMessages), Times.Once());
            _bugService.Verify(service => service.Insert(It.IsAny<Bug>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Post_Return_bad_request_when_a_model_is_null()
        {
            LoadRequestInController(_bugsController, HttpMethod.Post, url);

            var response = _bugsController.Post(null);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Post_Return_bad_request_when_the_name_of_the_model_is_empty()
        {
            LoadRequestInController(_bugsController, HttpMethod.Post, url);
            var invalidModel = new Bug { Name = string.Empty, IdPriority = Guid.NewGuid(), IdProject = Guid.NewGuid() };

            var response = _bugsController.Post(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Post_Return_bad_request_when_the_priority_of_the_model_is_empty()
        {
            LoadRequestInController(_bugsController, HttpMethod.Post, url);
            var invalidModel = new Bug { Name = "Bug", IdPriority = Guid.Empty, IdProject = Guid.NewGuid() };

            var response = _bugsController.Post(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Post_Return_bad_request_when_the_priority_of_the_model_is_null()
        {
            LoadRequestInController(_bugsController, HttpMethod.Post, url);
            var invalidModel = new Bug { Name = "Bug", IdPriority = null, IdProject = Guid.NewGuid() };

            var response = _bugsController.Post(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Post_Return_bad_request_when_the_project_of_the_model_is_null()
        {
            LoadRequestInController(_bugsController, HttpMethod.Post, url);
            var invalidModel = new Bug { Name = "Bug", IdPriority = Guid.NewGuid(), IdProject = null };

            var response = _bugsController.Post(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Post_Return_bad_request_when_the_project_of_the_model_is_empty()
        {
            LoadRequestInController(_bugsController, HttpMethod.Post, url);
            var invalidModel = new Bug { Name = "Bug", IdPriority = Guid.NewGuid(), IdProject = Guid.Empty };

            var response = _bugsController.Post(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Put()
        {
            _bugService.Setup(service => service.IsValid(It.IsAny<Bug>(), out _errorMessages)).Returns(true);
            _bugService.Setup(service => service.Update(It.IsAny<Bug>()));
            LoadRequestInController(_bugsController, HttpMethod.Put, url);

            var response = _bugsController.Put(_bug);

            _bugService.Verify(service => service.Update(It.IsAny<Bug>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Put_Return_internal_server_error_when_there_is_an_exception()
        {
            _bugService.Setup(service => service.IsValid(It.IsAny<Bug>(), out _errorMessages)).Returns(true);
            _bugService.Setup(service => service.Update(It.IsAny<Bug>())).Throws(new Exception());
            LoadRequestInController(_bugsController, HttpMethod.Put, url);

            var response = _bugsController.Put(_bug);

            _bugService.Verify(service => service.IsValid(It.IsAny<Bug>(), out _errorMessages), Times.Once());
            _bugService.Verify(service => service.Update(It.IsAny<Bug>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Put_Return_bad_request_when_a_model_is_null()
        {
            LoadRequestInController(_bugsController, HttpMethod.Put, url);

            var response = _bugsController.Put(null);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Put_Return_bad_request_when_the_name_of_the_model_is_empty()
        {
            LoadRequestInController(_bugsController, HttpMethod.Post, url);
            var invalidModel = new Bug { Name = string.Empty, IdPriority = Guid.NewGuid(), IdProject = Guid.NewGuid() };

            var response = _bugsController.Put(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Put_Return_bad_request_when_the_priority_of_the_model_is_empty()
        {
            LoadRequestInController(_bugsController, HttpMethod.Post, url);
            var invalidModel = new Bug { Name = "Bug", IdPriority = Guid.Empty, IdProject = Guid.NewGuid() };

            var response = _bugsController.Put(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Put_Return_bad_request_when_the_project_of_the_model_is_null()
        {
            LoadRequestInController(_bugsController, HttpMethod.Post, url);
            var invalidModel = new Bug { Name = "Bug", IdPriority = Guid.NewGuid(), IdProject = null };

            var response = _bugsController.Put(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Put_Return_bad_request_when_the_project_of_the_model_is_empty()
        {
            LoadRequestInController(_bugsController, HttpMethod.Post, url);
            var invalidModel = new Bug { Name = "Bug", IdPriority = Guid.NewGuid(), IdProject = Guid.Empty };

            var response = _bugsController.Put(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Put_Return_bad_request_when_the_priority_of_the_model_is_null()
        {
            LoadRequestInController(_bugsController, HttpMethod.Post, url);
            var invalidModel = new Bug { Name = "Bug", IdPriority = null, IdProject = Guid.NewGuid() };

            var response = _bugsController.Put(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Delete()
        {
            _bugService.Setup(service => service.Delete(It.IsAny<Guid>()));
            LoadRequestInController(_bugsController, HttpMethod.Delete, url);

            var response = _bugsController.Delete(Guid.NewGuid());

            _bugService.Verify(service => service.Delete(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void BugsController_Delete_Return_internal_server_error_when_there_is_an_exception()
        {
            _bugService.Setup(service => service.Delete(It.IsAny<Guid>())).Throws(new Exception());
            LoadRequestInController(_bugsController, HttpMethod.Delete, url);

            var response = _bugsController.Delete(Guid.NewGuid());

            _bugService.Verify(service => service.Delete(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
