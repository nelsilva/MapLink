using System.Collections.Generic;
using NUnit.Framework;
using RouteTotalCalculation.Core.Contracts;
using RouteTotalCalculation.Core.Model;
using RouteTotalCalculation.Core.ServiceAddressFinder;
using RouteTotalCalculation.Core.ServiceRoute;
using RouteTotalCalculation.Core.Services;
using RouteTotalCalculation.Tests.Helper;
using SharpTestsEx;
using Point = RouteTotalCalculation.Core.ServiceAddressFinder.Point;

namespace RouteTotalCalculation.Tests
{
	[TestFixture]
	public class RouteTotalCalculationTest
	{
		private IRouteService _routeService;
		private IAddressFinderService _addressFinderService;
		private ICalculateTotalOfRouteService _calculateTotalOfRouteService;
		private ModelFactory _modelFactory;

		[SetUp]
		public void SetUp()
		{
			_routeService = new RouteService();
			_addressFinderService = new AddressFinderService();
			_calculateTotalOfRouteService = new CalculateTotalOfRouteService(_routeService, _addressFinderService);
			_modelFactory = new ModelFactory(_addressFinderService);
		}

		[Test]
		public void GetAddressLocationFromAddressesTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				_modelFactory.Create("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP"),
				_modelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			IList<AddressLocation> addressLocations = _modelFactory.Create(addresses);
			addressLocations.Should().Not.Be.Null();
			addressLocations.Count.Should().Be(3);
			addressLocations[0].point.Should().Not.Be.Null();
			addressLocations[1].point.Should().Not.Be.Null();
			addressLocations[2].point.Should().Not.Be.Null();
		}

		[Test]
		public void GetPointsFromAddressTest()
		{
			IAddress address = _modelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP");
			Core.ServiceAddressFinder.Address serviceAddress = _modelFactory.Create(address);

			Point points = _addressFinderService.GetCoordinates(serviceAddress);
			points.Should().Not.Be.Null();
			points.x.Should().Not.Be(0);
			points.y.Should().Not.Be(0);
		}

		[Test]
		public void GetRouteStopFromAddressLocationTest()
		{
			IList<AddressLocation> locations = new List<AddressLocation>
			{
				new AddressLocation
				{
					address = _modelFactory.Create(_modelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP")),
					point = new Point {x = -46.6520066, y = -23.5650127}
				},
				new AddressLocation
				{
					address = _modelFactory.Create(_modelFactory.Create("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP")),
					point = new Point {x = -46.6520066, y = -23.5650127}
				},
				new AddressLocation
				{
					address = _modelFactory.Create(_modelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")),
					point = new Point {x = -46.6520066, y = -23.5650127}
				},
			};

			IList<RouteStop> routeStops = _modelFactory.Create(locations);
			routeStops.Should().Not.Be.Null();
			routeStops.Count.Should().Be(3);
			routeStops[0].Should().Not.Be.Null();
			routeStops[1].Should().Not.Be.Null();
			routeStops[2].Should().Not.Be.Null();
		}

		[Test]
		public void GetRouteTotalsTest()
		{
			IList<RouteStop> routes = new List<RouteStop>
			{
				_modelFactory.Create("Avenida Paulista, 1000", -46.6520066, -23.5650127),
				_modelFactory.Create("Av Pres Juscelino Kubitschek, 1000", -46.679055, -23.589735),
				_modelFactory.Create("Av Nove de Julho, 1500", -46.6513602, -23.5564401)
			};

			RouteOptions routeOptions = _modelFactory.Create(routeTypes: 23);

			RouteTotals routeTotal = _routeService.GetRouteTotalsResponse(routes, routeOptions);
			routeTotal.Should().Not.Be.Null();
			routeTotal.totalCost.Should().Not.Be(0);
			routeTotal.totalDistance.Should().Not.Be(0);
			routeTotal.totalfuelCost.Should().Not.Be(0);
			routeTotal.totalTime.Should().Not.Be.Empty();
		}

		[Test]
		public void OriginAndDestinationWithSameAddress()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				_modelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
			};

			var routeTotal = _calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
			routeTotal.Should().Not.Be.Null();
			routeTotal.TotalCost.Should().Be(0);
			routeTotal.TotalDistance.Should().Be(0);
			routeTotal.TotalfuelCost.Should().Be(0);
			routeTotal.TotalTime.Should().Not.Be.Empty();
		}

		[Test]
		public void RouteTotalCalculationWith2Addresses()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				_modelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			var routeTotal = _calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
			AssertValues.CheckRouteTotal(routeTotal);
		}

		[Test]
		public void RouteTotalCalculationWithMultipleAddressesAndDefaultQuickestRouteTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				_modelFactory.Create("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP"),
				_modelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			var routeTotal = _calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
			AssertValues.CheckRouteTotal(routeTotal);
		}

		[Test]
		public void RouteTotalCalculationWithMultipleAddressesAndRouteAvoidingTrafficTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				_modelFactory.Create("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP"),
				_modelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			var routeTotal = _calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 23);
			AssertValues.CheckRouteTotal(routeTotal);
		}
	}
}