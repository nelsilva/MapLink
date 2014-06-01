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
		[Test]
		public void GetAddressLocationFromAddressesTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			IList<AddressLocation> addressLocations = ModelFactory.Create(addresses);
			addressLocations.Should().Not.Be.Null();
			addressLocations.Count.Should().Be(3);
			addressLocations[0].point.Should().Not.Be.Null();
			addressLocations[1].point.Should().Not.Be.Null();
			addressLocations[2].point.Should().Not.Be.Null();
		}

		[Test]
		public void GetPointsFromAddressTest()
		{
			IAddress address = ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP");
			Core.ServiceAddressFinder.Address serviceAddress = ModelFactory.Create(address);

			Point points = AddressFinderService.GetCoordinates(serviceAddress);
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
					address = ModelFactory.Create(ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP")),
					point = new Point {x = -46.6520066, y = -23.5650127}
				},
				new AddressLocation
				{
					address = ModelFactory.Create(ModelFactory.Create("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP")),
					point = new Point {x = -46.6520066, y = -23.5650127}
				},
				new AddressLocation
				{
					address = ModelFactory.Create(ModelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")),
					point = new Point {x = -46.6520066, y = -23.5650127}
				},
			};

			IList<RouteStop> routeStops = ModelFactory.Create(locations);
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
				ModelFactory.Create("Avenida Paulista, 1000", -46.6520066, -23.5650127),
				ModelFactory.Create("Av Pres Juscelino Kubitschek, 1000", -46.679055, -23.589735),
				ModelFactory.Create("Av Nove de Julho, 1500", -46.6513602, -23.5564401)
			};

			RouteOptions routeOptions = ModelFactory.Create(routeTypes: 23);

			RouteTotals routeTotal = RouteService.GetRouteTotalsResponse(routes, routeOptions);
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
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
			};

			var routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
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
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			var routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
			AssertValues.CheckRouteTotal(routeTotal);
		}

		[Test]
		public void RouteTotalCalculationWithMultipleAddressesAndDefaultQuickestRouteTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			var routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
			AssertValues.CheckRouteTotal(routeTotal);
		}

		[Test]
		public void RouteTotalCalculationWithMultipleAddressesAndRouteAvoidingTrafficTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			var routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 23);
			AssertValues.CheckRouteTotal(routeTotal);
		}
	}
}