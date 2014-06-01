using System.Collections.Generic;
using RouteTotalCalculation.Core.ServiceRoute;

namespace RouteTotalCalculation.Core.Services
{
	public static class RouteService
	{
		public static RouteTotals GetRouteTotalsResponse(IList<RouteStop> routes, RouteOptions routeOptions)
		{
			using (var routeSoapClient = new RouteSoapClient())
			{
				return routeSoapClient.getRouteTotals((List<RouteStop>) routes, routeOptions, Configuration.TokenValue);
			}
		}
	}
}