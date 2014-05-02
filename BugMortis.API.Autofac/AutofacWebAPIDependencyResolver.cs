using Autofac;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace BugMortis.API.Autofac
{
    internal class AutofacWebAPIDependencyResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public AutofacWebAPIDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            object instance = null;
            _container.TryResolve(serviceType, out instance);
            return instance;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            object instance = null;
            var ienumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            _container.TryResolve(ienumerableType, out instance);
            return (IEnumerable<object>)instance;
        }

        public IDependencyScope BeginScope()
        {
            return new AutoFacWebAPIDependencyScope(_container.BeginLifetimeScope());
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
