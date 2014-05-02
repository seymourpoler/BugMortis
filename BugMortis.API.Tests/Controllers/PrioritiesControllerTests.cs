using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using BugMortis.Domain;
using BugMortis.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net;
using BugMortis.API.Controllers;

namespace BugMortis.API.Tests.Controllers
{
    [TestClass]
    public class PrioritiesControllerTests : BaseControllerTests
    {
        private string url = "http://localhost/api/priorities";
        private Mock<IDataMasterService<Priority>> _priorityService;
        private PrioritiesController _prioritiesController;

        public PrioritiesControllerTests()
        {
            _priorityService = new Mock<IDataMasterService<Priority>>();
            _prioritiesController = new PrioritiesController(_priorityService.Object);
        }

        [TestMethod]
        public void PrioritiesController_Get()
        {
            var priorities = new List<Priority> { 
                new Priority{ IdPriority = new Guid("00000000-0000-0000-0000-000000000001"), Name = "High" },
                new Priority { IdPriority = new Guid("00000000-0000-0000-0000-000000000002"), Name = "Medium" },
                new Priority { IdPriority = new Guid("00000000-0000-0000-0000-000000000003"), Name = "Low" }
            };

            _priorityService.Setup(service => service.GetAll()).Returns(priorities);
            LoadRequestInController(_prioritiesController, HttpMethod.Get, url);
            
            var response = _prioritiesController.Get();

            var values = ((ObjectContent)(response.Content)).Value;
            List<Priority> responseData = (List<Priority>)(values);

            Assert.IsTrue(responseData.Any());
            _priorityService.Verify(service => service.GetAll(), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void PrioritiesController_Get_Return_internal_server_error_When_there_is_an_exception()
        {
            _priorityService.Setup(service => service.GetAll()).Throws(new Exception());
            LoadRequestInController(_prioritiesController, HttpMethod.Get, url);

            var response = _prioritiesController.Get();

            _priorityService.Verify(service => service.GetAll(), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
