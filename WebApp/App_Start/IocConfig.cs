using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Data;
using WebApp.Models;
using WebApp.Providers;
using WebApp.Services;

namespace WebApp
{
    public class IocConfig
    {

        public static IContainer Container { get; private set; }


        public static void RegisterDependencies(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(IocConfig).Assembly);

            // Register your Web API controllers.
            builder.RegisterApiControllers(typeof(IocConfig).Assembly);

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // Register application dependencies.
            builder
                .RegisterType<ApplicationDbContext>()
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(EfRepository<>))
                .As(typeof(IAsyncRepository<>));

            //builder.RegisterAssemblyTypes(typeof(IAsyncRepository<>).Assembly)
            //    .AsClosedTypesOf(typeof(IAsyncRepository<>))
            //    .InstancePerRequest();

            //builder.RegisterAssemblyTypes(ServiceAssembly).Where(type => typeof(ServiceBase).IsAssignableFrom(type) && !type.IsAbstract).AsImplementedInterfaces().InstancePerRequest();
            //builder.RegisterAssemblyTypes(MapperAssembly).Where(type => typeof(MapperBase).IsAssignableFrom(type) && !type.IsAbstract).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterType<ApplicationUserStore>().As<IUserStore<ApplicationUser>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            //builder.RegisterType<RoleStore<IdentityRole>>().As<IRoleStore<IdentityRole, string>>().InstancePerRequest();
            //builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager>
            {
                DataProtectionProvider = app.GetDataProtectionProvider()
            });

            builder.RegisterType<SecureDataFormat<AuthenticationTicket>>()
                .As<ISecureDataFormat<AuthenticationTicket>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<Base64UrlTextEncoder>()
                .As<ITextEncoder>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TicketSerializer>()
                .As<IDataSerializer<AuthenticationTicket>>()
                .InstancePerLifetimeScope();

            //builder.RegisterModule(new LoggingModule());
            builder.RegisterType<TenantRepository>().As<ITenantRepository>();
            builder.RegisterType<TenantService>().As<ITenantService>();

            Container = builder.Build();

            // Set the dependency resolver to be Autofac.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container); //Set the WebApi DependencyResolver

            app.UseAutofacMiddleware(Container);
            app.UseAutofacMvc();
        }

        //private static void NewMethod(IAppBuilder app)
        //{
        //    var builder = new ContainerBuilder();

        //    // Register your MVC controllers. (MvcApplication is the name of
        //    // the class in Global.asax.)
        //    builder.RegisterControllers(typeof(IocConfig).Assembly);

        //    // Register your Web API controllers.
        //    builder.RegisterApiControllers(typeof(IocConfig).Assembly);

        //    // OPTIONAL: Register web abstractions like HttpContextBase.
        //    builder.RegisterModule<AutofacWebTypesModule>();

        //    // Register application dependencies.
        //    builder.RegisterType<ApplicationDbContext>().InstancePerRequest();

        //    builder.RegisterType<EfRepository<Speaker>>().As<IAsyncRepository<Speaker>>();
        //    builder.RegisterType<TenantRepository>().As<ITenantRepository>();
        //    builder.RegisterType<TenantService>().As<ITenantService>();

        //    // Set the dependency resolver to be Autofac.
        //    var container = builder.Build();
        //    DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        //    GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container); //Set the WebApi DependencyResolver

        //    app.UseAutofacMiddleware(container);
        //}
    }
}