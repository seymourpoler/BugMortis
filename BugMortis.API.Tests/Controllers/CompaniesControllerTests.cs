using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using BugMortis.Domain;
using BugMortis.Domain.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugMortis.API.Controllers;
using System.Net.Http;
using System.Net;

namespace BugMortis.API.Tests.Controllers
{
    [TestClass]
    public class CompaniesControllerTests : BaseControllerTests
    {
        private Mock<IService<Company>> _companyService;
        private CompaniesController _companiesController;
        private readonly string url = "http://localhost/api/companies";
        private Company _company;

        public CompaniesControllerTests()
        {
            _companyService = new Mock<IService<Company>>();
            _company = new Company { IdCompany = Guid.NewGuid(), Name = "Compamy One" };
        }

        private void SetUpController(HttpMethod httpMethod)
        {
            _companiesController = new CompaniesController(_companyService.Object);
            LoadRequestInController(_companiesController, httpMethod, url);
        }

        [TestMethod]
        public void CompaniesController_Get()
        {
            var companies = new List<Company> { 
                new Company{ IdCompany = Guid.NewGuid(), Name = "Compamy One"},
                new Company{ IdCompany = Guid.NewGuid(), Name = "Compamy Two"},
                new Company{ IdCompany = Guid.NewGuid(), Name = "Compamy Three"}
            };
            _companyService.Setup(service => service.GetAll()).Returns(companies);
            SetUpController(HttpMethod.Get);
            
            var response = _companiesController.Get();
            var  values = ((System.Net.Http.ObjectContent)(response.Content)).Value;
            List<Company> responseCompanies = (System.Collections.Generic.List<BugMortis.Domain.Entity.Company>)(values);

            Assert.IsTrue(responseCompanies.Any());
            _companyService.Verify(service => service.GetAll() , Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_Get_Return_internal_server_error_When_there_is_an_exception()
        {
            _companyService.Setup(service => service.GetAll()).Returns(() => {throw new Exception();});
            SetUpController(HttpMethod.Get);

            var response = _companiesController.Get();
            _companyService.Verify(service => service.GetAll(), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_GetById()
        {
            _companyService.Setup(service => service.GetById(It.IsAny<Guid>())).Returns(_company);
            SetUpController(HttpMethod.Get);

            var response = _companiesController.Get(Guid.NewGuid());
            var values = ((System.Net.Http.ObjectContent)(response.Content)).Value;
            Company responseCompany = (BugMortis.Domain.Entity.Company)(values);

            Assert.IsNotNull(responseCompany);
            Assert.IsInstanceOfType(responseCompany, typeof(Company));
            _companyService.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_GetById__return_Not_found_when_the_company_doesnot_found()
        {
            _companyService.Setup(service => service.GetById(It.IsAny<Guid>())).Returns(() => null);
            SetUpController(HttpMethod.Get);

            var response = _companiesController.Get(Guid.NewGuid());

            _companyService.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_GetById_Return_internal_server_error_when_there_is_an_exception()
        {
            _companyService.Setup(service => service.GetById(It.IsAny<Guid>())).Returns(() => { throw new Exception(); });
            SetUpController(HttpMethod.Get);

            var response = _companiesController.Get(Guid.NewGuid());

            _companyService.Verify(service => service.GetById(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_Post()
        {
            _companyService.Setup(service => service.IsValid(It.IsAny<Company>(), out _errorMessages)).Returns(true);
            _companyService.Setup(service => service.Insert(It.IsAny<Company>())).Returns(Guid.NewGuid());
            SetUpController(HttpMethod.Post);

            var response = _companiesController.Post(_company);

            _companyService.Verify(service => service.IsValid(It.IsAny<Company>(), out _errorMessages), Times.Once());
            _companyService.Verify(service => service.Insert(It.IsAny<Company>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_Post_Return_internal_server_error_when_there_is_an_exception()
        {
            _companyService.Setup(service => service.IsValid(It.IsAny<Company>(), out _errorMessages)).Returns(true);
            _companyService.Setup(service => service.Insert(It.IsAny<Company>())).Returns(() => {throw new Exception();});
            SetUpController(HttpMethod.Post);

            var response = _companiesController.Post(_company);

            _companyService.Verify(service => service.IsValid(It.IsAny<Company>(), out _errorMessages), Times.Once());
            _companyService.Verify(service => service.Insert(It.IsAny<Company>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_Post_Return_bad_request_when_the_name_of_the_model_is_empty()
        {
            _companyService.Setup(service => service.IsValid(It.IsAny<Company>(), out _errorMessages)).Returns(false);
            SetUpController(HttpMethod.Post);
            var invalidCompany = new Company { Name = string.Empty };

            var response = _companiesController.Post(invalidCompany);

            _companyService.Verify(service => service.IsValid(It.IsAny<Company>(), out _errorMessages), Times.Once());
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_Post_Return_bad_request_when_a_model_is_null()
        {
            _companyService.Setup(service => service.IsValid(It.IsAny<Company>(), out _errorMessages)).Returns(false);
            SetUpController(HttpMethod.Post);

            var response = _companiesController.Post(null);

            _companyService.Verify(service => service.IsValid(It.IsAny<Company>(), out _errorMessages), Times.Once());
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_Put()
        {
            _companyService.Setup(service => service.IsValid(It.IsAny<Company>(), out _errorMessages)).Returns(true);
            _companyService.Setup(service => service.Update(It.IsAny<Company>()));
            SetUpController(HttpMethod.Put);

            var response = _companiesController.Put(_company);

            _companyService.Verify(service => service.Update(It.IsAny<Company>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_Put_Return_internal_server_error_when_there_is_an_exception()
        {
            _companyService.Setup(service => service.IsValid(It.IsAny<Company>(), out _errorMessages)).Returns(true);
            _companyService.Setup(service => service.Update(It.IsAny<Company>())).Throws(new Exception());
            SetUpController(HttpMethod.Put);

            var response = _companiesController.Put(_company);

            _companyService.Verify(service => service.IsValid(It.IsAny<Company>(), out _errorMessages), Times.Once());
            _companyService.Verify(service => service.Update(It.IsAny<Company>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_Put_Return_bad_request_when_the_name_of_the_model_is_empty()
        {
            _companyService.Setup(service => service.IsValid(It.IsAny<Company>(), out  _errorMessages)).Returns(false);
            SetUpController(HttpMethod.Put);
            var invalidCompany = new Company { Name = string.Empty };

            var response = _companiesController.Put(invalidCompany);

            _companyService.Verify(service => service.IsValid(It.IsAny<Company>(), out _errorMessages), Times.Once());
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_Put_Return_bad_request_when_a_model_is_null()
        {
            _companyService.Setup(service => service.IsValid(It.IsAny<Company>(), out _errorMessages)).Returns(false);
            SetUpController(HttpMethod.Put);
            var response = _companiesController.Put(null);

            _companyService.Verify(service => service.IsValid(It.IsAny<Company>(), out _errorMessages), Times.Once());
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_Delete()
        {
            _companyService.Setup(service => service.Delete(It.IsAny<Guid>()));
            SetUpController(HttpMethod.Delete);

            var response = _companiesController.Delete(Guid.NewGuid());

            _companyService.Verify(service => service.Delete(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void CompaniesController_Delete_Return_internal_server_error_when_there_is_an_exception()
        {
            _companyService.Setup(service => service.Delete(It.IsAny<Guid>())).Throws(new Exception());
            SetUpController(HttpMethod.Delete);

            var response = _companiesController.Delete(Guid.NewGuid());

            _companyService.Verify(service => service.Delete(It.IsAny<Guid>()), Times.Once());
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
