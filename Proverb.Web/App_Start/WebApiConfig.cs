using System.Configuration;
using Newtonsoft.Json.Serialization;
using Proverb.Web.Logging;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;

namespace Proverb.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // http://www.asp.net/web-api/overview/security/enabling-cross-origin-requests-in-web-api#enable-cors
            var crossOriginDomains = ConfigurationManager.AppSettings["Access-Control-Allow-Origin"];
            var cors = new EnableCorsAttribute(crossOriginDomains, "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Services.Add(typeof(IExceptionLogger), new UnhandledExceptionLogger());

            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
    }
}
