using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BugMortis.Domain;
using BugMortis.Domain.Entity;

namespace BugMortis.API.Controllers
{
    public class CompaniesController : ApiController
    {
        private IService<Company> _companyService;

        public CompaniesController(IService<Company> companyService)
        {
            _companyService = companyService;
        }

        // GET api/companies
        public HttpResponseMessage Get()
        {
            try
            {
                var companies = _companyService.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, companies);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // GET api/companies/5
        public HttpResponseMessage Get(Guid id)
        {
            try
            {
                var company = _companyService.GetById(id);
                if (company == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK, company);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // POST api/companies
        public HttpResponseMessage Post(Company company)
        {
            try
            {
                var errorMessages = new List<string>();
                if (_companyService.IsValid(company, out errorMessages))
                {
                    _companyService.Insert(company);
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessages);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // PUT api/companies/5
        public HttpResponseMessage Put(Company company)
        {
            try
            {
                var errorMessages = new List<string>();
                if (_companyService.IsValid(company, out errorMessages))
                {
                    _companyService.Update(company);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessages);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // DELETE api/companies/5
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                _companyService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
