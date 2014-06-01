using System;
using System.Collections.Generic;
using System.Linq;
using RouteTotalCalculation.Core.ServiceAddressFinder;
using RouteTotalCalculation.Core.ServiceRoute;
using Point = RouteTotalCalculation.Core.ServiceRoute.Point;

namespace RouteTotalCalculation.Core.Services
{
	public static class RouteService
	{
		public static RouteTotals GetRouteTotalsResponse(IList<RouteStop> routes, RouteOptions routeOptions)
		{
			using (var routeSoapClient = new RouteSoapClient())
			{
				return routeSoapClient.getRouteTotals((List<RouteStop>)routes, routeOptions, Configuration.TokenValue);
			}
		}

		public static IList<RouteStop> GetRouteStopsFromAddressesLocation(IEnumerable<AddressLocation> addressesLocation)
		{
			return addressesLocation.Select(addressLocation => new RouteStop
			{
				description = String.Format("{0}, {1}, {2}, {3}",
					addressLocation.address.street, addressLocation.address.houseNumber, addressLocation.address.city.name,
					addressLocation.address.city.state),
				point = new Point 
				{ 
					x = addressLocation.point.x, 
					y = addressLocation.point.y 
				}
			}).ToList();
		}		
	}
}