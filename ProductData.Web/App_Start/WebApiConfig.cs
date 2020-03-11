using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace ProductData.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Remove stock XML and JSON serializer
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Remove(config.Formatters.JsonFormatter);

            // Add custom JSON formatter.
            var jsonFormatter = new JsonMediaTypeFormatter
            {
                UseDataContractJsonSerializer = false,
                SerializerSettings = { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            };
            config.Formatters.Add(jsonFormatter); 
        }
    }
}
