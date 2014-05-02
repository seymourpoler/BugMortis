using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using BugMortis.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugMortis.Data;
using BugMortis.Data.Entity;

namespace BugMortis.Domain.Tests
{
    [TestClass]
    public class BugServiceTests
    {
        private Domain.Entity.Bug _bug;
        private Mock<IRepository<Data.Entity.Bug>>  _bugRepository;
        private BugService _bugService;

        public BugServiceTests()
        {
            _bug = new Domain.Entity.Bug { Name = "Simple Bug" };
            _bugRepository = new Mock<IRepository<Data.Entity.Bug>>();
            _bugService = new BugService(_bugRepository.Object);
        }

        [TestMethod]
        public void BugService_IsValid_return_false_when_name_is_empty()
        {
            var bug = new Domain.Entity.Bug();
            var errorMessages = new List<string>();
            var expected = _bugService.IsValid(bug, out errorMessages);

            Assert.IsFalse(expected);
            Assert.IsTrue(errorMessages.Any());
            Assert.AreEqual("the name of the bug cannot be empty", errorMessages[0]);
        }

        [TestMethod]
        public void BugService_IsValid_return_false_when_bug_is_null()
        {
            var errorMessages = new List<string>();
            var expected = _bugService.IsValid(null, out errorMessages);

            Assert.IsFalse(expected);
            Assert.IsTrue(errorMessages.Any());
            Assert.AreEqual("the bug cannot be null", errorMessages[0]);
        }

        [TestMethod]
        public void BugService_IsValid_return_false_when_IdPriority_is_null()
        {
            var bug = new Domain.Entity.Bug { Name = "name", IdPriority = null};
            var errorMessages = new List<string>();
            var expected = _bugService.IsValid(bug, out errorMessages);

            Assert.IsFalse(expected);
            Assert.IsTrue(errorMessages.Any());
            Assert.AreEqual("the priority of the bug cannot be null", errorMessages[0]);
        }

        [TestMethod]
        public void BugService_IsValid_return_false_when_IdPriority_is_guidEmpty()
        {
            var bug = new Domain.Entity.Bug { Name = "name", IdPriority = Guid.Empty };
            var errorMessages = new List<string>();
            var expected = _bugService.IsValid(bug, out errorMessages);

            Assert.IsFalse(expected);
            Assert.IsTrue(errorMessages.Any());
            Assert.AreEqual("the priority of the bug cannot be a guid empty", errorMessages[0]);
        }

        [TestMethod]
        public void BugService_IsValid_return_false_when_IdProject_is_null()
        {
            var bug = new Domain.Entity.Bug { Name = "name", IdPriority = Guid.NewGuid(), IdProject = null };
            var errorMessages = new List<string>();
            var expected = _bugService.IsValid(bug, out errorMessages);

            Assert.IsFalse(expected);
            Assert.IsTrue(errorMessages.Any());
            Assert.AreEqual("the id of the project of the bug cannot be null", errorMessages[0]);
        }

        [TestMethod]
        public void BugService_IsValid_return_false_when_IdProject_is_guidEmpty()
        {
            var bug = new Domain.Entity.Bug { Name = "name", IdPriority = Guid.NewGuid(), IdProject = Guid.Empty };
            var errorMessages = new List<string>();
            var expected = _bugService.IsValid(bug, out errorMessages);

            Assert.IsFalse(expected);
            Assert.IsTrue(errorMessages.Any());
            Assert.AreEqual("the id pf the project of the bug cannot be a guid empty", errorMessages[0]);
        }

        [TestMethod]
        public void BugService_getbyid_simple_bug()
        {
            var idBug = Guid.NewGuid();
            var dataBug = new Data.Entity.Bug {Name = "Data Bug" };
            _bugRepository.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(dataBug);

            var resut = _bugService.GetById(idBug);

            _bugRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once());
        }

        [TestMethod]
        public void BugService_get_all_bugs()
        {
            var bugs = new List<Data.Entity.Bug> {
                new Data.Entity.Bug{ IdBug = Guid.NewGuid(), Name = "Bug One"},
                new Data.Entity.Bug{ IdBug = Guid.NewGuid(), Name = "Bug Two"},
            };
            _bugRepository.Setup(x => x.GetAll()).Returns(bugs);

            var resut = _bugService.GetAll();

            Assert.IsTrue(resut.Any());
            _bugRepository.Verify(x => x.GetAll(), Times.Once());
        }

        [TestMethod]
        public void BugService_save_simple_bug()
        {
            var idBug = Guid.NewGuid();
            _bugRepository.Setup(x => x.Insert(It.IsAny<Data.Entity.Bug>())).Returns(idBug);

            var resut = _bugService.Insert(_bug);

            _bugRepository.Verify(x => x.Insert(It.IsAny<Data.Entity.Bug>()), Times.Once());
            Assert.AreEqual(resut, idBug);
        }

        [TestMethod]
        public void BugService_update_simple_bug()
        {
            _bugRepository.Setup(x => x.Update(It.IsAny<Data.Entity.Bug>()));

            _bugService.Update(_bug);

            _bugRepository.Verify(x => x.Update(It.IsAny<Data.Entity.Bug>()), Times.Once());
        }

        [TestMethod]
        public void BugService_delete_simple_bug()
        {
            var idBug = Guid.NewGuid();
            _bugRepository.Setup(x => x.Delete(It.IsAny<Guid>()));

            _bugService.Delete(idBug);

            _bugRepository.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Once());
        }
    }
}
