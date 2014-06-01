using System;
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
			if (routeTypes != 0 && routeTypes != 23)
				throw new Exception("Deve enviar somente 0 para rota padrão rápida ou 23 para rota evitando o trânsito");

			IEnumerable<AddressLocation> locations = ModelFactory.Create(addresses);
			IList<RouteStop> routes = ModelFactory.Create(locations);
			RouteOptions routeOptions = ModelFactory.Create(routeTypes);

			RouteTotals routeTotal = RouteService.GetRouteTotalsResponse(routes, routeOptions);

			return ModelFactory.Create(routeTotal);
		}
	}
}