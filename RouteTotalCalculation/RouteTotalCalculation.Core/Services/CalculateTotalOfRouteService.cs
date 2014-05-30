using System.Collections.Generic;
using RouteTotalCalculation.Core.Model;
using RouteTotalCalculation.Core.ServiceAddressFinder;
using RouteTotalCalculation.Core.ServiceRoute;

namespace RouteTotalCalculation.Core.Services
{
	public static class CalculateTotalOfRouteService
	{
		public static RouteTotals GetTotalValuesOfRoute(IEnumerable<Address> addresses, RouterTypes routerTypes)
		{
			var locations = AddressFinderService.GetAddressLocationFromAddresses(addresses);
			var routes = RouteService.GetRouteStopsFromAddressesLocation(locations);

			var routeOptions = new RouteOptions
			{
				language = "portuguese",
				routeDetails = new RouteDetails { descriptionType = 0, routeType = (int)routerTypes, optimizeRoute = true },
				vehicle = new Vehicle
				{
					tankCapacity = 20,
					averageConsumption = 9,
					fuelPrice = 3,
					averageSpeed = 60,
					tollFeeCat = 2
				}
			};

			return RouteService.GetRouteTotalsResponse(routes, routeOptions);
		}
	}
}
