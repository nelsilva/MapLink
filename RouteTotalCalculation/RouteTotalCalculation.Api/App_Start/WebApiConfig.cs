using System.Web.Http;
using System.Web.Http.Cors;

namespace RouteTotalCalculation.Api
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			//Support for CORS
			var corsAttribute = new EnableCorsAttribute("*", "*", "GET,POST");
			config.EnableCors(corsAttribute);

			// Web API configuration and services

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
