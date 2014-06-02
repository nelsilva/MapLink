using System.Collections.Generic;
using RouteTotalCalculation.Core.Contracts;
using RouteTotalCalculation.Core.ServiceRoute;

namespace RouteTotalCalculation.Core.Services
{
	public class RouteService : IRouteService
	{
		public RouteTotals GetRouteTotalsResponse(IList<RouteStop> routes, RouteOptions routeOptions)
		{
			using (var routeSoapClient = new RouteSoapClient())
			{
				return routeSoapClient.getRouteTotals((List<RouteStop>) routes, routeOptions, Configuration.TokenValue);
			}
		}
	}
}