using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;

namespace WebApp
{
    public static class Ioc
    {
        /// <summary>
        /// Setups for Autofac dependancy injection
        /// </summary>
        public static IContainer RegisterGlobalDependencies()
        {
            // Build up your application container and register your dependencies.
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register your Web API controllers.
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            //// OPTIONAL: Register model binders that require DI.
            //builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            //builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            //// OPTIONAL: Enable property injection in view pages.
            //builder.RegisterSource(new ViewRegistrationSource());

            //// OPTIONAL: Enable property injection into action filters.
            //builder.RegisterFilterProvider();

            // Register application dependencies.
            builder
                .RegisterType<ApplicationDbContext>()
                .InstancePerRequest();

            // Default is .InstancePerDependency() // a unique instance will be returned from each request for a service.
            builder.RegisterType<ApplicationSignInManager>();
            builder.RegisterType<ApplicationUserManager>();
            builder.RegisterType<EmailService>();

            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication)
                .As<IAuthenticationManager>();

            //builder.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
            //    new InjectionConstructor(typeof(ApplicationDbContext)));

            builder.RegisterType<UserStore<ApplicationUser>>()
                .As<IUserStore<ApplicationUser>>();

            builder.RegisterType<EfRepository<Speaker>>().As<IAsyncRepository<Speaker>>();
            builder.RegisterType<TenantRepository>().As<ITenantRepository>();
            builder.RegisterType<TenantService>().As<ITenantService>();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();

            return container;
        }
    }
}