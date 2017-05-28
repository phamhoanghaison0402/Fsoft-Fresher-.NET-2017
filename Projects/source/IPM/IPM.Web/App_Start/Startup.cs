using Autofac;
using Autofac.Integration.Mvc;
using IPM.Business;
using IPM.Data;
using IPM.Data.Infrastructure;
using IPM.Data.Repositories;
using IPM.Service;
using IPM.Web;
using Microsoft.Owin;
using Owin;
using System.Reflection;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Startup))]

namespace IPM.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            ConfigAutofac(app); 
        }
        private void ConfigAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
           

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();

            builder.RegisterType<IPMDbContext>().AsSelf().InstancePerRequest();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(SkillRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Business
            builder.RegisterAssemblyTypes(typeof(SkillBusiness).Assembly)
                .Where(t => t.Name.EndsWith("Business"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Services
            builder.RegisterAssemblyTypes(typeof(SkillService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
