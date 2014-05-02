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
    public class BugsController : ApiController
    {
        private IService<Bug> _bugService;
        public BugsController(IService<Bug> bugService)
        {
            _bugService = bugService;
        }

        // GET api/bugs
        public HttpResponseMessage Get()
        {
            try
            {
                var bugs = _bugService.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, bugs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // GET api/bugs/5
        public HttpResponseMessage Get(Guid id)
        {
            try
            {
                var data = _bugService.GetById(id);
                if (data == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // POST api/bugs
        public HttpResponseMessage Post(Bug bug)
        {
            try
            {
                List<string> errorMessage;
                if (_bugService.IsValid(bug, out errorMessage))
                {
                    _bugService.Insert(bug);
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // PUT api/bugs/5
        public HttpResponseMessage Put(Bug bug)
        {
            try
            {
                List<string> errorMessages;
                if ( _bugService.IsValid(bug, out errorMessages))
                {
                    _bugService.Update(bug);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessages);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // DELETE api/bugs/5
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                _bugService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
