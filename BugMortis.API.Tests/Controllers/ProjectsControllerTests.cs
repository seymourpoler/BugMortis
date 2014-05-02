using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using BugMortis.Domain.Entity;
using BugMortis.Domain;
using BugMortis.API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net;

namespace BugMortis.API.Tests.Controllers
{
    [TestClass]
    public class ProjectsControllerTests : BaseControllerTests
    {
        private readonly string url = "http://localhost/api/projects";
        private Mock<IService<Project>> _projectService;
        private ProjectsController _projectController;
        private Project _project = new Project { IdProject = Guid.NewGuid(), Name = "Project Name" };

        public ProjectsControllerTests()
        {
            _projectService = new Mock<IService<Project>>();
            _projectController = new ProjectsController(_projectService.Object);
        }

        [TestMethod]
        public void ProjectsController_Get()
        {
            var projects = new List<Project> { 
                new Project{ IdProject = Guid.NewGuid(), Name = "Project One"},
                new Project{ IdCompany = Guid.NewGuid(), Name = "Project Two"},
                new Project{ IdCompany = Guid.NewGuid(), Name = "Project Three"}
            };
            _projectService.Setup(service => service.GetAll()).Returns(projects);
            LoadRequestInController(_projectController,HttpMethod.Get, url);
            var response = _projectController.Get();

            var values = ((ObjectContent)(response.Content)).Value;
            List<Project> responseData = (List<Project>)(values);

            Assert.IsTrue(responseData.Any());
            _projectService.Verify(service => service.GetAll(), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_Get_Return_internal_server_error_When_there_is_an_exception()
        {
            _projectService.Setup(service => service.GetAll()).Throws(new Exception());
            LoadRequestInController(_projectController, HttpMethod.Get, url);

            var response = _projectController.Get();
            _projectService.Verify(service => service.GetAll(), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_GetById()
        {
            _projectService.Setup(service => service.GetById(It.IsAny<Guid>())).Returns(_project);
            LoadRequestInController(_projectController, HttpMethod.Get, url);

            var response = _projectController.Get(Guid.NewGuid());
            var values = ((ObjectContent)(response.Content)).Value;
            Project responseData = (Project)(values);

            Assert.IsNotNull(responseData);
            Assert.IsInstanceOfType(responseData, typeof(Project));
            _projectService.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_GetById__return_Not_found_when_the_project_doesnot_found()
        {
            _projectService.Setup(service => service.GetById(It.IsAny<Guid>())).Returns(() => null);
            LoadRequestInController(_projectController, HttpMethod.Get, url);

            var response = _projectController.Get(Guid.NewGuid());

            _projectService.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_GetById_Return_internal_server_error_when_there_is_an_exception()
        {
            _projectService.Setup(service => service.GetById(It.IsAny<Guid>())).Returns(() => { throw new Exception(); });
            LoadRequestInController(_projectController, HttpMethod.Get, url);

            var response = _projectController.Get(Guid.NewGuid());

            _projectService.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_Post()
        {
            _projectService.Setup(service => service.IsValid(It.IsAny<Project>(), out _errorMessages)).Returns(true);
            _projectService.Setup(service => service.Insert(It.IsAny<Project>())).Returns(Guid.NewGuid());
            LoadRequestInController(_projectController, HttpMethod.Post, url);

            var response = _projectController.Post(_project);

            _projectService.Verify(service => service.Insert(It.IsAny<Project>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_Post_Return_internal_server_error_when_there_is_an_exception()
        {
            _projectService.Setup(service => service.IsValid(It.IsAny<Project>(), out _errorMessages)).Returns(true);
            _projectService.Setup(service => service.Insert(It.IsAny<Project>())).Returns(() => { throw new Exception(); });
            LoadRequestInController(_projectController, HttpMethod.Post, url);

            var response = _projectController.Post(_project);

            _projectService.Verify(service => service.IsValid(It.IsAny<Project>(), out _errorMessages), Times.Once());
            _projectService.Verify(service => service.Insert(It.IsAny<Project>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_Post_Return_bad_request_when_the_name_of_the_model_is_empty()
        {
            LoadRequestInController(_projectController, HttpMethod.Post, url);
            var invalidModel = new Project { Name = string.Empty };

            var response = _projectController.Post(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_Post_Return_bad_request_when_a_model_is_null()
        {
            LoadRequestInController(_projectController, HttpMethod.Post, url);
            var response = _projectController.Post(null);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_Put()
        {
            _projectService.Setup(service => service.IsValid(It.IsAny<Project>(), out _errorMessages)).Returns(true);
            _projectService.Setup(service => service.Update(It.IsAny<Project>()));
            LoadRequestInController(_projectController, HttpMethod.Put, url);

            var response = _projectController.Put(_project);

            _projectService.Verify(service => service.Update(It.IsAny<Project>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_Put_Return_internal_server_error_when_there_is_an_exception()
        {
            _projectService.Setup(service => service.IsValid(It.IsAny<Project>(), out _errorMessages)).Returns(true);
            _projectService.Setup(service => service.Update(It.IsAny<Project>())).Throws(new Exception());
            LoadRequestInController(_projectController, HttpMethod.Put, url);

            var response = _projectController.Put(_project);

            _projectService.Verify(service => service.IsValid(It.IsAny<Project>(), out _errorMessages), Times.Once());
            _projectService.Verify(service => service.Update(It.IsAny<Project>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_Put_Return_bad_request_when_the_name_of_the_model_is_empty()
        {
            LoadRequestInController(_projectController, HttpMethod.Put, url);
            var invalidModel = new Project { Name = string.Empty };

            var response = _projectController.Put(invalidModel);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_Put_Return_bad_request_when_a_model_is_null()
        {
            LoadRequestInController(_projectController, HttpMethod.Put, url);
            var response = _projectController.Put(null);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_Delete()
        {
            _projectService.Setup(service => service.Delete(It.IsAny<Guid>()));
            LoadRequestInController(_projectController, HttpMethod.Delete, url);

            var response = _projectController.Delete(Guid.NewGuid());

            _projectService.Verify(service => service.Delete(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void ProjectsController_Delete_Return_internal_server_error_when_there_is_an_exception()
        {
            _projectService.Setup(service => service.Delete(It.IsAny<Guid>())).Throws(new Exception());
            LoadRequestInController(_projectController, HttpMethod.Delete, url);

            var response = _projectController.Delete(Guid.NewGuid());

            _projectService.Verify(service => service.Delete(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
