using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PortalApi.App_Start;
using PortalApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PortalApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Web API configuration and servicesz

            var formatter = GlobalConfiguration.Configuration.Formatters;
            var jsonformtat = formatter.JsonFormatter;
            var setting = jsonformtat.SerializerSettings;

            jsonformtat.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;            
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            setting.Formatting = Formatting.Indented;
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
                      

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
