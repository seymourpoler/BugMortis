using System;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BugMortis.API.Tests.Controllers
{
    public class BaseControllerTests
    {
        protected List<string> _errorMessages;

        public BaseControllerTests()
        {
            _errorMessages = new List<string>();

        }

        protected void LoadRequestInController(ApiController controller, HttpMethod httpMethod, string url)
        {
            var request = new HttpRequestMessage(httpMethod, url);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = new HttpConfiguration(); ;
        }
    }
}
