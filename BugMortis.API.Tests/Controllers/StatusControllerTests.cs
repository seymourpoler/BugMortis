using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using BugMortis.Domain;
using BugMortis.Domain.Entity;
using BugMortis.API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net;

namespace BugMortis.API.Tests.Controllers
{
    [TestClass]
    public class StatusControllerTests : BaseControllerTests
    {
        private string url = "http://localhost/api/status";
        private Mock<IDataMasterService<Status>> _statusService;
        private StatusController _statusController;

        public StatusControllerTests()
        {
            _statusService = new Mock<IDataMasterService<Status>>();
            _statusController = new StatusController(_statusService.Object);
        }

        [TestMethod]
        public void StatusController_Get()
        {
            var status = new List<Status> { 
                new Status { IdStatus = new Guid("00000000-0000-0000-0000-000000000001"), Name = "Status One" },
                new Status { IdStatus = new Guid("00000000-0000-0000-0000-000000000002"), Name = "Status Two" },
                new Status { IdStatus = new Guid("00000000-0000-0000-0000-000000000003"), Name = "Status Three" }
            };

            _statusService.Setup(service => service.GetAll()).Returns(status);
            LoadRequestInController(_statusController, HttpMethod.Get, url);
            
            var response = _statusController.Get();

            var values = ((ObjectContent)(response.Content)).Value;
            List<Status> responseData = (List<Status>)(values);

            Assert.IsTrue(responseData.Any());
            _statusService.Verify(service => service.GetAll(), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void StatusController_Get_Return_internal_server_error_When_there_is_an_exception()
        {
            _statusService.Setup(service => service.GetAll()).Throws(new Exception());
            LoadRequestInController(_statusController, HttpMethod.Get, url);

            var response = _statusController.Get();

            _statusService.Verify(service => service.GetAll(), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
