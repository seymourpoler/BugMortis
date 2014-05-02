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
    public class ProjectsController : ApiController
    {
        private IService<Project> _projectService;

        public ProjectsController(IService<Project> projectService)
        {
            _projectService = projectService;
        }

        // GET api/projects
        public HttpResponseMessage Get()
        {
            try
            {
                var projects = _projectService.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, projects);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // GET api/projects/5
        public HttpResponseMessage Get(Guid id)
        {
            try
            {
                var project = _projectService.GetById(id);
                if (project == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK, project);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // POST api/projects
        public HttpResponseMessage  Post(Project project)
        {
            try
            {
                List<string> errorMessages;
                if (_projectService.IsValid(project, out errorMessages))
                {
                    _projectService.Insert(project);
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessages);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // PUT api/projects/5
        public HttpResponseMessage  Put(Project project)
        {
            try
            {
                List<string> errorMessages ;
                if (_projectService.IsValid(project, out errorMessages))
                {
                    _projectService.Update(project);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorMessages);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // DELETE api/projects/5
        public HttpResponseMessage Delete(Guid id)
        {
            try
            {
                _projectService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
