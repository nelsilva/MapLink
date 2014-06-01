using System;
using System.Collections.Generic;
using System.Linq;
using RouteTotalCalculation.Core.ServiceAddressFinder;
using RouteTotalCalculation.Core.ServiceRoute;
using RouteTotalCalculation.Core.Services;
using City = RouteTotalCalculation.Core.ServiceAddressFinder.City;
using Point = RouteTotalCalculation.Core.ServiceRoute.Point;

namespace RouteTotalCalculation.Core.Model
{
	public static class ModelFactory
	{
		public static ServiceAddressFinder.Address Create(Address address)
		{
			var serviceAddress = new ServiceAddressFinder.Address
			{
				street = address.Street,
				houseNumber = address.HouseNumber,
				city = new City { name = address.City, state = address.State }
			};
			return serviceAddress;
		}

		public static IList<AddressLocation> Create(IEnumerable<Address> addresses)
		{
			return addresses.Select(Create).Select(serviceAddress => new AddressLocation
			{
				address = serviceAddress, point = AddressFinderService.GetCoordinates(serviceAddress)
			}).ToList();
		}

		public static IList<RouteStop> Create(IEnumerable<AddressLocation> addressesLocation)
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

		public static Address Create(string streetValue, string houseNumber, string cityNameValue, string cityStateValue)
		{
			return new Address(streetValue, houseNumber, cityNameValue, cityStateValue);
		}

		public static RouteOptions Create(string languageValue, RouteDetails routeDetails, Vehicle vehicle)
		{
			return new RouteOptions
			{
				language = languageValue,
				routeDetails = routeDetails,
				vehicle = vehicle
			};
		}

		public static RouteStop Create(string descriptionValue, double xValue, double yValue)
		{
			return new RouteStop
			{
				description = descriptionValue,
				point = new Point { x = xValue, y = yValue }
			};
		}
	}
}