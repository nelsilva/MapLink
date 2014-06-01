using System.Collections.Generic;
using RouteTotalCalculation.Core.Contracts;
using RouteTotalCalculation.Core.Model;
using RouteTotalCalculation.Core.ServiceAddressFinder;
using RouteTotalCalculation.Core.ServiceRoute;

namespace RouteTotalCalculation.Core.Services
{
	public static class CalculateTotalOfRouteService
	{
		public static IRouteTotalValues GetTotalValuesOfRoute(IEnumerable<IAddress> addresses, int routeTypes)
		{
			RouteOptions routeOptions = ModelFactory.Create(routeTypes);
			IEnumerable<AddressLocation> locations = ModelFactory.Create(addresses);
			IList<RouteStop> routes = ModelFactory.Create(locations);

			RouteTotals routeTotal = RouteService.GetRouteTotalsResponse(routes, routeOptions);

			return ModelFactory.Create(routeTotal);
		}
	}
}