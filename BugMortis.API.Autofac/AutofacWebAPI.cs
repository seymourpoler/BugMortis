using Autofac;
using System.Web.Http;
using Autofac.Integration.WebApi;
using BugMortis.Domain;
using BugMortis.Domain.Entity;

namespace BugMortis.API.Autofac
{
    public class AutofacWebAPI
    {
        public static void Initialize(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();
            configuration.DependencyResolver = new AutofacWebAPIDependencyResolver(RegisterServices(builder));
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {

            builder
                .RegisterType<BugService>()
                .As<IService<Bug>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<CompanyService>()
                .As<IService<Company>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<PriorityService>()
                .As<IDataMasterService<Priority>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<ProjectService>()
                .As<IService<Project>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<StatusService>()
                .As<IDataMasterService<Bug>>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<UserService>()
                .As<IService<User>>()
                .InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
