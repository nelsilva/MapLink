using System;
using System.Collections.Generic;
using NUnit.Framework;
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
		[ExpectedException(typeof(System.ServiceModel.FaultException), UserMessage = "Deve haver ao menos dois pontos de parada. Quantidade de pontos informados: 1")]
		public void OnlyOneAddress()
		{
			IList<Address> addresses = new List<Address>()
			{
				ClassFactory.GetAddress("Avenida Paulista", "1000", "São Paulo", "SP")
			};

			var routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, RouterTypes.DefaultQuickestRoute);
		}

		[Test]
		public void OriginAndDestinationWithSameAddress()
		{
			IList<Address> addresses = new List<Address>()
			{
				ClassFactory.GetAddress("Avenida Paulista", "1000", "São Paulo", "SP"),
				ClassFactory.GetAddress("Avenida Paulista", "1000", "São Paulo", "SP"),
			};

			var routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, RouterTypes.DefaultQuickestRoute);
			routeTotal.Should().Not.Be.Null();
			routeTotal.totalCost.Should().Be(0);
			routeTotal.totalDistance.Should().Be(0);
			routeTotal.totalFuelUsed.Should().Be(0);
			routeTotal.totalTime.Should().Not.Be.Empty();
			routeTotal.totalfuelCost.Should().Be(0);
			routeTotal.totaltollFeeCost.Should().Be(0);
		}

		[Test]
		public void RouteTotalCalculationWith2Addresses()
		{
			IList<Address> addresses = new List<Address>()
			{
				ClassFactory.GetAddress("Avenida Paulista", "1000", "São Paulo", "SP"),
				ClassFactory.GetAddress("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			var routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, RouterTypes.DefaultQuickestRoute);
			AssertValues.CheckRouteTotal(routeTotal);
		}

		[Test]
		public void RouteTotalCalculationWithMultipleAddressesAndDefaultQuickestRouteTest()
		{
			IList<Address> addresses = new List<Address>()
			{
				ClassFactory.GetAddress("Avenida Paulista", "1000", "São Paulo", "SP"),
				ClassFactory.GetAddress("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP"),
				ClassFactory.GetAddress("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			var routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, RouterTypes.DefaultQuickestRoute);
			AssertValues.CheckRouteTotal(routeTotal);
		}

		[Test]
		public void RouteTotalCalculationWithMultipleAddressesAndRouteAvoidingTrafficTest()
		{
			IList<Address> addresses = new List<Address>()
			{
				ClassFactory.GetAddress("Avenida Paulista", "1000", "São Paulo", "SP"),
				ClassFactory.GetAddress("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP"),
				ClassFactory.GetAddress("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			var routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, RouterTypes.RouteAvoidingTraffic);
			AssertValues.CheckRouteTotal(routeTotal);
		}

		[Test]
		public void GetFindAddressTest()
		{
			var address = ClassFactory.GetAddress("Avenida Paulista", "1000", "São Paulo", "SP");
			var addressOptions = ClassFactory.GetAddressOptions(usePhoneticValue: true, searchTypeValue: 2, pageIndexValue: 1,
				recordsPerPageValue: 10);
			var findAddressResponse = AddressFinderService.GetFindAddressResponse(address, addressOptions);

			findAddressResponse.Should().Not.Be.Null();
			findAddressResponse.pageCount.Should().Not.Be(0);
			findAddressResponse.recordCount.Should().Not.Be(0);
		}

		[Test]
		public void GetPointsFromAddressTest()
		{
			var address = ClassFactory.GetAddress("Avenida Paulista", "1000", "São Paulo", "SP");

			var points = AddressFinderService.GetXY(address);
			points.Should().Not.Be.Null();
			points.x.Should().Not.Be(0);
			points.y.Should().Not.Be(0);
		}

		[Test]
		public void GetAddressLocationFromAddressesTest()
		{
			IList<Address> addresses = new List<Address>()
			{
				ClassFactory.GetAddress("Avenida Paulista", "1000", "São Paulo", "SP"),
				ClassFactory.GetAddress("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP"),
				ClassFactory.GetAddress("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			var addressLocations = AddressFinderService.GetAddressLocationFromAddresses(addresses);
			addressLocations.Should().Not.Be.Null();
			addressLocations.Count.Should().Be(3);
			addressLocations[0].point.Should().Not.Be.Null();
			addressLocations[1].point.Should().Not.Be.Null();
			addressLocations[2].point.Should().Not.Be.Null();
		}

		[Test]
		public void GetRouteStopFromAddressLocationTest()
		{
			IList<AddressLocation> locations = new List<AddressLocation>()
			{
				new AddressLocation { 
					address = ClassFactory.GetAddress("Avenida Paulista", "1000", "São Paulo", "SP"), 
					point = new Point { x = -46.6520066, y = -23.5650127}
				},
				new AddressLocation { 
					address = ClassFactory.GetAddress("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP"), 
					point = new Point { x = -46.6520066, y = -23.5650127}
				},
				new AddressLocation { 
					address = ClassFactory.GetAddress("Av Nove de Julho", "1500", "São Paulo", "SP"), 
					point = new Point { x = -46.6520066, y = -23.5650127}
				},
			};

			var routeStops = RouteService.GetRouteStopsFromAddressesLocation(locations);
			routeStops.Should().Not.Be.Null();
			routeStops.Count.Should().Be(3);
			routeStops[0].Should().Not.Be.Null();
			routeStops[1].Should().Not.Be.Null();
			routeStops[2].Should().Not.Be.Null();
		}

		[Test]
		public void GetRouteTotalsTest()
		{
			IList<RouteStop> routes = new List<RouteStop>()
			{
				ClassFactory.GetRouteStop("Avenida Paulista, 1000", -46.6520066, -23.5650127),
				ClassFactory.GetRouteStop("Av Pres Juscelino Kubitschek, 1000", -46.679055, -23.589735),
				ClassFactory.GetRouteStop("Av Nove de Julho, 1500", -46.6513602, -23.5564401)
			};

			var routeOptions = ClassFactory.GetRouteOptions(
				"portuguese",
				new RouteDetails {descriptionType = 0, routeType = 1, optimizeRoute = true},
				new Vehicle
				{
					tankCapacity = 20,
					averageConsumption = 9,
					fuelPrice = 3,
					averageSpeed = 60,
					tollFeeCat = 2
				});

			var routeTotal = RouteService.GetRouteTotalsResponse(routes, routeOptions);
			AssertValues.CheckRouteTotal(routeTotal);
		}
	}
}
