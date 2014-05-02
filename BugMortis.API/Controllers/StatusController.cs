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
    public class StatusController : ApiController
    {
        private IDataMasterService<Status> _statusService;

        public StatusController(IDataMasterService<Status> statusService)
        {
            _statusService = statusService;
        }

        // GET api/status
        public HttpResponseMessage Get()
        {
            try
            {
                var data = _statusService.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
