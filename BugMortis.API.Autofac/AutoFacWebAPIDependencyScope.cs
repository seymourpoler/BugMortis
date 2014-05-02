using Autofac;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace BugMortis.API.Autofac
{
    public class AutoFacWebAPIDependencyScope : IDependencyScope
    {
        private readonly ILifetimeScope _lifetimeScope;

        public AutoFacWebAPIDependencyScope(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public void Dispose()
        {
            _lifetimeScope.Dispose();
        }

        public object GetService(Type serviceType)
        {
            object instance = null;
            _lifetimeScope.TryResolve(serviceType, out instance);
            return instance;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            object instance = null;
            var ienumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            _lifetimeScope.TryResolve(ienumerableType, out instance);
            return (IEnumerable<object>)instance;
        }
    }
}
