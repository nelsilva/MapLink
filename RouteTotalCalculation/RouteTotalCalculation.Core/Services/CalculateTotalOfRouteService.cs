using System.Collections.Generic;
using RouteTotalCalculation.Core.Model;
using RouteTotalCalculation.Core.ServiceAddressFinder;
using RouteTotalCalculation.Core.ServiceRoute;

namespace RouteTotalCalculation.Core.Services
{
	public static class CalculateTotalOfRouteService
	{
		public static RouteTotalValues GetTotalValuesOfRoute(IEnumerable<Address> addresses, RouteTypes routerTypes)
		{
			var locations = AddressFinderService.GetAddressLocationFromAddresses(addresses);
			var routes = RouteService.GetRouteStopsFromAddressesLocation(locations);

			//RouterOption com configuração padrão
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

			var routeTotal = RouteService.GetRouteTotalsResponse(routes, routeOptions);
			//Criando um objeto novo somente com as informações relevantes
			var routeTotalValues = new RouteTotalValues(routeTotal.totalDistance, routeTotal.totalTime,
				routeTotal.totalfuelCost, routeTotal.totalCost);

			return routeTotalValues;
		}
	}
}
