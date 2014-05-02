using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using BugMortis.Data;
using BugMortis.Data.Entity;
using BugMortis.Domain.Entity;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugMortis.Domain.Tests
{
    [TestClass]
    public class PriorityServiceTests
    {
        [TestMethod]
        public void PriorityService_get_all_priorities()
        {
            var listPriorities = new List<Data.Entity.Priority>{
                new Data.Entity.Priority{ IdPriority = Guid.NewGuid(), Name = "Priority One"},
                new Data.Entity.Priority{ IdPriority = Guid.NewGuid(), Name = "Priority two"},
                new Data.Entity.Priority{ IdPriority = Guid.NewGuid(), Name = "Priority three"},
            };
            var repository = new Mock<IDataMasterRepository<Data.Entity.Priority>>();
            repository.Setup(x => x.GetAll()).Returns(listPriorities);
            var priorityService = new PriorityService(repository.Object);

            var expected = priorityService.GetAll();

            Assert.IsTrue(expected.Any());
            repository.Verify(x => x.GetAll(), Times.Once());
        }
    }
}
