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
    public class PrioritiesController : ApiController
    {
        private IDataMasterService<Priority> _priorityService;

        public PrioritiesController(IDataMasterService<Priority> priorityService)
        {
            _priorityService = priorityService;
        }

        // GET api/priorities
        public HttpResponseMessage Get()
        {
            try
            {
                var data = _priorityService.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
