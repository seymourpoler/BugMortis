using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugMortis.Data.Entity;
using BugMortis.Data.SqlServer.Repositories;

namespace BugMortis.Data.SqlServer.Tests
{
    [TestClass]
    public class BugRepositoryTest : BaseRepositoryTest
    {
        private CompanyRepository _companyRepository;
        private ProjectRepository _projectRepository;
        private BugRepository _bugRepository;
        private Company _company;
        private Project _project;
        private Bug _bug;

        public BugRepositoryTest():base()
        {
            _company = new Company { Name = "New Co." };
            _companyRepository = new CompanyRepository(_db);
            _companyRepository.Insert(_company);
            _projectRepository = new ProjectRepository(_db);
            _project = new Project { Name = "new Project", IdCompany = _company.IdCompany };
            _projectRepository.Insert(_project);
            _bugRepository = new BugRepository(_db);
            _bug = new Bug{ 
                Name = "SimpleBug", 
                IdPriority = new Guid("00000000-0000-0000-0000-000000000003"), 
                IdStatus = new Guid("00000000-0000-0000-0000-000000000001"), 
                IdProject = _project.IdProject,
                Days = 23 };
        }

        [TestInitialize]
        public void SetUp()
        {
            _bug.IdBug = _bugRepository.Insert(_bug);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bugRepository.Delete(_bug.IdBug);
        }

        [TestMethod]
        public void BugRepository_should_save_simple_bug()
        {
            _bugRepository.Insert(_bug);
            var expectedBug = _bugRepository.GetById(_bug.IdBug);

            Assert.AreEqual<Bug>(expectedBug, _bug);
        }

        [TestMethod]
        public void BugRepository_should_update_simple_bug()
        {
            var expectedBug = _bugRepository.GetById(_bug.IdBug);
            _bug.Days = 125;
            _bug.Name = "Simple Custom Task";
            _bug.IdPriority = new Guid("00000000-0000-0000-0000-000000000001");
            _bug.IdStatus = new Guid("00000000-0000-0000-0000-000000000002");

            _bugRepository.Update(expectedBug);
            var modifiedTask = _bugRepository.GetById(expectedBug.IdBug);

            Assert.AreEqual<Bug>(expectedBug, modifiedTask);
        }

        [TestMethod]
        public void BugRepository_save_bug_with_one_attach()
        {
            var bug = new Bug() { Name = "simple bug", 
                                 IdPriority = new Guid("00000000-0000-0000-0000-000000000003"), 
                                 IdStatus = new Guid("00000000-0000-0000-0000-000000000002"),
                                 IdProject = _project.IdProject,
                                 Days = 23 };
            var attach = new Attachment() { ContentType = "contetType", Name = "name", Content = new byte[] { 123, 12, 3 } };

            bug.Attachments.Add(attach);
            bug.IdBug = _bugRepository.Insert(bug);
            
            var expected = _bugRepository.GetById(bug.IdBug);
            Assert.AreEqual(bug, expected);
            _bugRepository.Delete(bug.IdBug);
        }

        [TestMethod]
        public void BugRepository_save_update_bug_with_attachments()
        {
            var bug = new Bug()
            {
                Name = "simple bug",
                IdPriority = new Guid("00000000-0000-0000-0000-000000000003"),
                IdStatus = new Guid("00000000-0000-0000-0000-000000000002"),
                IdProject = _project.IdProject,
                Days = 23
            };
            var attachOne = new Attachment() { ContentType = "contetType", Name = "name", Content = new byte[] { 123, 12, 3 } };
            var attachTwo = new Attachment() { ContentType = "contetType2", Name = "name2", Content = new byte[] { 158, 123, 233 } };
            var attachThree = new Attachment() { ContentType = "contetType3", Name = "name3", Content = new byte[] { 111, 123, 222 } };

            bug.Attachments.Add(attachOne);
            bug.Attachments.Add(attachTwo);
            _bugRepository.Insert(bug);

            bug.Attachments.Remove(attachTwo);
            _bugRepository.Update(bug);
            
            bug.Attachments.Add(attachThree);
            _bugRepository.Update(bug);
            
            var expected = _bugRepository.GetById(bug.IdBug);

            Assert.AreEqual(bug, expected);
        }

        [TestMethod]
        public void BugRepository_update_bug_with_one_attach()
        {
            var bug = new Bug()
            {
                Name = "simple bug",
                IdProject = _project.IdProject,
                IdPriority = new Guid("00000000-0000-0000-0000-000000000003"),
                IdStatus = new Guid("00000000-0000-0000-0000-000000000002"),
                Days = 23
            };
            var attach = new Attachment() { ContentType = "contetType", Name = "name", Content = new byte[] { 123, 12, 3 } };

            bug.IdBug = _bugRepository.Insert(bug);

            var modified = _bugRepository.GetById(bug.IdBug);
            modified.IdStatus = new Guid("00000000-0000-0000-0000-000000000002");
            modified.Description = "Description Custom";

            _bugRepository.Update(modified);
            var expected = _bugRepository.GetById(bug.IdBug);

            Assert.AreEqual(modified, expected);
            _bugRepository.Delete(bug.IdBug);
        }
    }
}
