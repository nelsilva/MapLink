using System;
using System.Collections.Generic;
using System.Linq;
using RouteTotalCalculation.Core.Contracts;
using RouteTotalCalculation.Core.ServiceAddressFinder;
using RouteTotalCalculation.Core.ServiceRoute;
using City = RouteTotalCalculation.Core.ServiceAddressFinder.City;
using Point = RouteTotalCalculation.Core.ServiceRoute.Point;

namespace RouteTotalCalculation.Core.Model
{
	public class ModelFactory
	{
		private readonly IAddressFinderService _addressFinderService;

		public ModelFactory(IAddressFinderService addressFinderService)
		{
			_addressFinderService = addressFinderService;
		}

		public ServiceAddressFinder.Address Create(IAddress address)
		{
			if (address == null) throw new ArgumentNullException("address");
			var serviceAddress = new ServiceAddressFinder.Address
			{
				street = address.Street,
				houseNumber = address.HouseNumber,
				city = new City { name = address.City, state = address.State }
			};
			return serviceAddress;
		}

		public IList<AddressLocation> Create(IEnumerable<IAddress> addresses)
		{
			if (addresses == null) throw new ArgumentNullException("addresses");
			return addresses.Select(Create).Select(serviceAddress => new AddressLocation
			{
				address = serviceAddress,
				point = _addressFinderService.GetCoordinates(serviceAddress)
			}).ToList();
		}

		public IList<RouteStop> Create(IEnumerable<AddressLocation> addressesLocation)
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

		public IAddress Create(string streetValue, string houseNumber, string cityNameValue, string cityStateValue)
		{
			return new Address(streetValue, houseNumber, cityNameValue, cityStateValue);
		}

		public RouteStop Create(string descriptionValue, double xValue, double yValue)
		{
			return new RouteStop
			{
				description = descriptionValue,
				point = new Point { x = xValue, y = yValue }
			};
		}

		public IRouteTotalValues Create(RouteTotals routeTotals)
		{
			return new RouteTotalValues(routeTotals.totalDistance, routeTotals.totalTime, routeTotals.totalfuelCost, routeTotals.totalCost);
		}

		public RouteOptions Create(int routeTypes)
		{
			if (routeTypes != 0 && routeTypes != 23)
				throw new Exception("Deve enviar somente 0 para rota padrão rápida ou 23 para rota evitando o trânsito");

			//RouteOption com configuração padrão
			return new RouteOptions
			{
				language = "portuguese",
				routeDetails = new RouteDetails {descriptionType = 0, routeType = routeTypes, optimizeRoute = true},
				vehicle = new Vehicle
				{
					tankCapacity = 20,
					averageConsumption = 9,
					fuelPrice = 3,
					averageSpeed = 60,
					tollFeeCat = 2
				}
			};
		}
	}
}