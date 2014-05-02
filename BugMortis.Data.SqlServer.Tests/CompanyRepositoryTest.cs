using System;
using System.Linq;
using BugMortis.Data;
using BugMortis.Data.Entity;
using BugMortis.Data.SqlServer.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugMortis.Data.SqlServer.Tests
{
    [TestClass]
    public class CompanyRepositoryTest : BaseRepositoryTest
    {
        private IRepository<Company> _companyRepository;
        private Company _simpleCompany, _companyTwo;
        private IRepository<Project> _projectRepository;
        private Project _project;

        [TestInitialize]
        public void SetUp()
        {
            _companyRepository = new CompanyRepository(_db);
            _simpleCompany = new Company { Name = "New Co" };
            _companyTwo = new Company { Name = "New Co II" };
            _project = new Project { Name = "New Project ..." };
            _projectRepository = new ProjectRepository(_db);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _companyRepository.Delete(_simpleCompany.IdCompany);
            _companyRepository.Delete(_companyTwo.IdCompany);
        }

        [TestMethod]
        public void CompanyRepository_save_simple_Company()
        {
            var idCompany = _companyRepository.Insert(_simpleCompany);
            var expected = _companyRepository.GetById(idCompany);
            Assert.AreEqual(_simpleCompany, expected);
        }

        [TestMethod]
        public void CompanyRepository_get_all_companies()
        {
            _companyRepository.Insert(_simpleCompany);
            _companyRepository.Insert(_companyTwo);
            var companies = _companyRepository.GetAll();

            Assert.IsTrue(companies.Count() > 1);
        }

        [TestMethod]
        public void CompanyRepository_update_simple_Company()
        {
            var idCompany = _companyRepository.Insert(_simpleCompany);
            var modified = _companyRepository.GetById(idCompany);

            modified.Name = "New Co, custom";
            _companyRepository.Update(modified);
            var expected = _companyRepository.GetById(modified.IdCompany);

            Assert.AreEqual(modified, expected);
        }

        [TestMethod]
        public void CompanyRepository_delete_simple_Company()
        {
            _companyRepository.Delete(_simpleCompany.IdCompany);
            var expected = _companyRepository.GetById(_simpleCompany.IdCompany);

            Assert.IsNull(expected);
        }

        [TestMethod]
        public void CompanyRepository_delete_Company_with_a_project()
        {
            _companyRepository.Insert(_simpleCompany);
            _project.IdCompany = _simpleCompany.IdCompany;
            _projectRepository.Insert(_project);
            _companyRepository.Delete(_simpleCompany.IdCompany);
            var expectedCompany = _companyRepository.GetById(_simpleCompany.IdCompany);
            var expectedProject = _projectRepository.GetById(_project.IdProject);

            Assert.IsNull(expectedCompany);
            Assert.IsNull(expectedProject);
        }
    }
}
