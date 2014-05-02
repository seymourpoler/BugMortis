using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using BugMortis.Data;
using BugMortis.Data.Entity;
using BugMortis.Domain;
using BugMortis.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugMortis.Domain.Tests
{
    [TestClass]
    public class CompanyServiceTests
    {
        private Mock<IRepository<Data.Entity.Company>> _companyRepository;
        private CompanyService _companyService;
        private Domain.Entity.Company _company;

        public CompanyServiceTests()
        {
            _companyRepository = new Mock<IRepository<Data.Entity.Company>>();
            _companyService = new CompanyService(_companyRepository.Object);
            _company = new Domain.Entity.Company { Name = "New Company" };
        }

        [TestMethod]
        public void CompanyService_isvalid_return_false_when_name_is_empty()
        {
            var company = new Domain.Entity.Company();
            var errorMesage = new List<string>();
            var expected= _companyService.IsValid(company, out errorMesage);

            Assert.IsFalse(expected);
            Assert.IsTrue(errorMesage.Any());
        }

        [TestMethod]
        public void CompanyService_isvalid_return_false_when_company_is_null()
        {
            var errorMesage = new List<string>();
            var expected = _companyService.IsValid(null, out errorMesage);

            Assert.IsFalse(expected);
            Assert.IsTrue(errorMesage.Any());
        }

        [TestMethod]
        public void CompanyService_save_Company()
        {
            var newIdCompany = Guid.NewGuid();
            _companyRepository.Setup(repo => repo.Insert(It.IsAny<Data.Entity.Company>())).Returns(newIdCompany);

            var expectedIdCompany = _companyService.Insert(_company);

            Assert.IsNotNull(expectedIdCompany);
            Assert.IsInstanceOfType(expectedIdCompany, typeof(Guid));
            Assert.AreEqual(expectedIdCompany, newIdCompany);
        }

        [TestMethod]
        public void CompanyService_get_all_Companies()
        {
            var companies = new List<Data.Entity.Company>{
                new Data.Entity.Company{ IdCompany = Guid.NewGuid(), Name = "Companie One"},
                new Data.Entity.Company{ IdCompany = Guid.NewGuid(), Name = "Companie Two"},
                new Data.Entity.Company{ IdCompany = Guid.NewGuid(), Name = "Companie Three"}
            };
            _companyRepository.Setup(repo => repo.GetAll()).Returns(companies);

            var expectedCompanies = _companyService.GetAll();

            Assert.IsNotNull(expectedCompanies);
            Assert.IsTrue(expectedCompanies.Any());
            _companyRepository.Verify(x => x.GetAll(), Times.Once());
        }

        [TestMethod]
        public void CompanyService_getbyid_Company()
        {
            var idCompany = Guid.NewGuid();
            var dataCompany = new Data.Entity.Company { Name = "New Company" };
            _companyRepository.Setup(repo => repo.GetById(It.IsAny<Guid>())).Returns(dataCompany);

            var expectedCompany = _companyService.GetById(idCompany);

            Assert.IsNotNull(expectedCompany);
            Assert.IsInstanceOfType(expectedCompany, typeof(Domain.Entity.Company));
            Assert.AreEqual(expectedCompany.Name, _company.Name);
            _companyRepository.Verify(x => x.GetById(idCompany), Times.Once());
        }

        [TestMethod]
        public void CompanyService_update_Company()
        {
            _companyRepository.Setup(repo => repo.Update(It.IsAny<Data.Entity.Company>()));

            _companyService.Update(_company);

            _companyRepository.Verify(x => x.Update(It.IsAny<Data.Entity.Company>()), Times.Once());
        }

        [TestMethod]
        public void CompanyService_delete_Company()
        {
            _companyRepository.Setup(repo => repo.Delete(It.IsAny<Guid>()));

            _companyService.Delete(Guid.NewGuid());

            _companyRepository.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Once());
        }
    }
}
