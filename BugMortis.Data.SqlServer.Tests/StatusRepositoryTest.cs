using System;
using System.Linq;
using BugMortis.Data;
using BugMortis.Data.Entity;
using BugMortis.Data.SqlServer.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugMortis.Data.SqlServer.Tests
{
    [TestClass]
    public class StatusRepositoryTest : BaseRepositoryTest
    {
        [TestMethod]
        public void StatusRepository_get_all_status()
        {
            var statusRepository = new StatusRepository(_db);

            var expected = statusRepository.GetAll();

            Assert.IsNotNull(expected);
            Assert.IsTrue(expected.Any());
        }
    }
}
