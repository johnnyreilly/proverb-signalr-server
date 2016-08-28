using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Proverb.Data.EntityFramework;
using Proverb.Web.Helpers;
using Proverb.Web.Logging;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using Proverb.Web.Hubs;
using Autofac.Core;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json;

//[assembly: OwinStartup(typeof(Startup))]

namespace Proverb.Web
{
   public class Startup
   {
      public void Configuration(IAppBuilder app)
      {
         var builder = Register();

         // STANDARD SIGNALR SETUP:

         // Get your HubConfiguration. In OWIN, you'll create one
         // rather than using GlobalHost.
         var hubConfig = new HubConfiguration();
         hubConfig.EnableDetailedErrors = true;

         // Set the dependency resolver to be Autofac.
         var container = builder.Build();
         hubConfig.Resolver = new AutofacDependencyResolver(container);
         var webApiResolver = new AutofacWebApiDependencyResolver(container);
         GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;

         // OWIN SIGNALR SETUP:

         // Register the Autofac middleware FIRST, then the standard SignalR middleware.
         app.UseAutofacMiddleware(container);
         app.UseCors(CorsOptions.AllowAll);
         //var crossOriginDomains = ConfigurationManager.AppSettings["Access-Control-Allow-Origin"];


         // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
         app.MapSignalR("/signalr", hubConfig);

         AddSignalRInjection(container, hubConfig.Resolver);
      }

      private void AddSignalRInjection(IContainer container, IDependencyResolver resolver)
      {
         var updater = new ContainerBuilder();

         updater.RegisterInstance(resolver.Resolve<IConnectionManager>());

         updater.Update(container);
      }

      private ContainerBuilder Register()
      {
         var builder = new ContainerBuilder();

         // DbContext
         builder.RegisterType<ProverbContext>().As<ProverbContext>().InstancePerDependency();

         // Queries / Commands
         builder.RegisterAssemblyTypes(Assembly.Load("Proverb.Data.CommandQuery"))
             .Where(t => t.Name.EndsWith("Query") || t.Name.EndsWith("Command"))
             .AsImplementedInterfaces()
             .InstancePerLifetimeScope();

         // Domain Services
         builder.RegisterAssemblyTypes(Assembly.Load("Proverb.Services"))
             .Where(t => t.Name.EndsWith("Service"))
             .AsImplementedInterfaces()
             .InstancePerLifetimeScope();

         builder.RegisterType<SayingFeed>().As<ISayingFeed>().SingleInstance();
         builder.RegisterType<SageFeed>().As<ISageFeed>().SingleInstance();

         var settings = new JsonSerializerSettings {ContractResolver = new SignalRContractResolver()};
         var serializer = JsonSerializer.Create(settings);
         builder.RegisterInstance(serializer).As<JsonSerializer>();

         // Web Project
         var assembly = Assembly.GetExecutingAssembly();
         builder.RegisterApiControllers(assembly).InstancePerLifetimeScope();
         builder.RegisterHubs(Assembly.GetExecutingAssembly());

         // Helpers
         builder.RegisterType<AppConfigHelper>().As<IAppConfigHelper>().InstancePerLifetimeScope();
         builder.RegisterType<AppCache>().As<IAppCache>().InstancePerLifetimeScope();
         builder.RegisterType<FileHelper>().As<IFileHelper>().InstancePerLifetimeScope();
         //builder.RegisterType<SessionHelper>().As<ISessionHelper>().InstancePerLifetimeScope();
         builder.RegisterType<UserHelper>().As<IUserHelper>().InstancePerLifetimeScope();

         // User
         builder.Register(c => HttpContext.Current.User).As<IPrincipal>().InstancePerLifetimeScope();

         // Logger
         //builder.Register(c => LoggerHelper.GetLogger()).As<ILog>().InstancePerLifetimeScope();
         builder.RegisterModule<LoggingModule>();

         return builder;
      }
   }
}
