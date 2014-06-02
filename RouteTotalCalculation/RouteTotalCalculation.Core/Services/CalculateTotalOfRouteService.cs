using System.Collections.Generic;
using RouteTotalCalculation.Core.Contracts;
using RouteTotalCalculation.Core.Model;
using RouteTotalCalculation.Core.ServiceAddressFinder;
using RouteTotalCalculation.Core.ServiceRoute;

namespace RouteTotalCalculation.Core.Services
{
	public class CalculateTotalOfRouteService : ICalculateTotalOfRouteService
	{
		private readonly IAddressFinderService _addressFinderService;
		private readonly IRouteService _routeService;

		public CalculateTotalOfRouteService(IRouteService routeService, IAddressFinderService addressFinderService)
		{
			_routeService = routeService;
			_addressFinderService = addressFinderService;
		}

		public IRouteTotalValues GetTotalValuesOfRoute(IEnumerable<IAddress> addresses, int routeTypes)
		{
			var modelFactory = new ModelFactory(_addressFinderService);
			RouteOptions routeOptions = modelFactory.Create(routeTypes);
			IList<AddressLocation> locations = modelFactory.Create(addresses);
			IList<RouteStop> routes = modelFactory.Create(locations);
			RouteTotals routeTotal = _routeService.GetRouteTotalsResponse(routes, routeOptions);

			return modelFactory.Create(routeTotal);
		}
	}
}