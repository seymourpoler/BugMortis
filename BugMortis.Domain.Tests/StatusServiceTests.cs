using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugMortis.Data;
using BugMortis.Data.Entity;

namespace BugMortis.Domain.Tests
{
    [TestClass]
    public class StatusServiceTests
    {

        [TestMethod]
        public void StatusService_get_all()
        {
            var listStatus = new List<Status>{
                new Status { IdStatus = Guid.NewGuid(), Name = "Status One" },
                new Status { IdStatus = Guid.NewGuid(), Name = "Status Two" },
                new Status { IdStatus = Guid.NewGuid(), Name = "Status Three" },
            };

            var repository = new Mock<IDataMasterRepository<Status>>();
            repository.Setup(x => x.GetAll()).Returns(listStatus);
            var statusService = new StatusService(repository.Object);

            var expected = statusService.GetAll();

            Assert.IsTrue(expected.Any());
            repository.Verify(x => x.GetAll(), Times.Once());
        }
    }
}
