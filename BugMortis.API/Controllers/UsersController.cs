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
    public class UsersController : ApiController
    {
        private IService<User> _userService;

        public UsersController(IService<User> userService)
        {
            _userService = userService;
        }

        // GET api/users
        public HttpResponseMessage Get()
        {
            try
            {
                var data = _userService.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // GET api/users/5
        public HttpResponseMessage Get(Guid id)
        {
            try
            {
                var data = _userService.GetById(id);
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

        // POST api/users
        public HttpResponseMessage Post(User user)
        {
            try
            {
                List<string> errorMessages;
                if (_userService.IsValid(user, out errorMessages))
                {
                    _userService.Insert(user);
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessages);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // PUT api/users/5
        public HttpResponseMessage Put(User user)
        {
            try
            {
                List<string> errorMessages;
                if (_userService.IsValid(user, out errorMessages))
                {
                    _userService.Update(user);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessages);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // DELETE api/users/5
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                _userService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
