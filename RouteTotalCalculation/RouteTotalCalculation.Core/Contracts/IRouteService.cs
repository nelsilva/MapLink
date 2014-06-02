using System.Collections.Generic;
using RouteTotalCalculation.Core.ServiceRoute;

namespace RouteTotalCalculation.Core.Contracts
{
	public interface IRouteService
	{
		RouteTotals GetRouteTotalsResponse(IList<RouteStop> routes, RouteOptions routeOptions);
	}
}
