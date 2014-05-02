using System;
using System.Linq;
using BugMortis.Data.Entity;
using BugMortis.Data.SqlServer.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugMortis.Data.SqlServer.Tests
{
    [TestClass]
    public class PriorityRepositoryTest : BaseRepositoryTest
    {
        [TestMethod]
        public void PriorityRepository_save_priority()
        {
            var priorityRepository = new PriorityRepository(_db);

            var expected = priorityRepository.GetAll();
    
            Assert.IsNotNull(expected);
            Assert.IsTrue(expected.Any());
        }
    }
}
