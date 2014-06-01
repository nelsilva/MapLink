using RouteTotalCalculation.Core.ServiceAddressFinder;
using RouteTotalCalculation.Core.ServiceRoute;
using City = RouteTotalCalculation.Core.ServiceAddressFinder.City;
using Point = RouteTotalCalculation.Core.ServiceRoute.Point;

namespace RouteTotalCalculation.Tests.Helper
{
	public static class ClassFactory
	{
		public static Address GetAddress(string streetValue, string houseNumber, string cityNameValue, string cityStateValue)
		{
			var address = new Address
			{
				street = streetValue,
				houseNumber = houseNumber,
				city = new City { name = cityNameValue, state = cityStateValue }
			};
			return address;
		}

		public static RouteOptions GetRouteOptions(string languageValue, RouteDetails routeDetails, Vehicle vehicle)
		{
			return new RouteOptions
			{
				language = languageValue,
				routeDetails = routeDetails,
				vehicle = vehicle
			};
		}

		public static RouteStop GetRouteStop(string descriptionValue, double xValue, double yValue)
		{
			return new RouteStop
			{
				description = descriptionValue,
				point = new Point {x = xValue, y = yValue}
			};
		}
	}
}
