using System;
using System.Collections.Generic;
using System.ServiceModel;
using NUnit.Framework;
using RouteTotalCalculation.Core.Model;
using RouteTotalCalculation.Core.ServiceAddressFinder;
using RouteTotalCalculation.Core.ServiceRoute;
using RouteTotalCalculation.Core.Services;
using RouteTotalCalculation.Tests.Helper;
using SharpTestsEx;
using Address = RouteTotalCalculation.Core.Model.Address;
using Point = RouteTotalCalculation.Core.ServiceAddressFinder.Point;

namespace RouteTotalCalculation.Tests
{
	[TestFixture]
	public class RouteTotalCalculationTest
	{
		[Test]
		public void GetAddressLocationFromAddressesTest()
		{
			IList<Address> addresses = new List<Address>
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
			Address address = ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP");
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

			RouteOptions routeOptions = ModelFactory.Create(
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

			RouteTotals routeTotal = RouteService.GetRouteTotalsResponse(routes, routeOptions);
			routeTotal.Should().Not.Be.Null();
			routeTotal.totalCost.Should().Not.Be(0);
			routeTotal.totalDistance.Should().Not.Be(0);
			routeTotal.totalfuelCost.Should().Not.Be(0);
			routeTotal.totalTime.Should().Not.Be.Empty();
		}

		[Test]
		[ExpectedException(typeof (FaultException),
			UserMessage = "Deve haver ao menos dois pontos de parada. Quantidade de pontos informados: 1")]
		public void OnlyOneAddress()
		{
			IList<Address> addresses = new List<Address>
			{
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP")
			};

			CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}


		[Test]
		public void OriginAndDestinationWithSameAddress()
		{
			IList<Address> addresses = new List<Address>
			{
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
			};

			RouteTotalValues routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
			routeTotal.Should().Not.Be.Null();
			routeTotal.TotalCost.Should().Be(0);
			routeTotal.TotalDistance.Should().Be(0);
			routeTotal.TotalfuelCost.Should().Be(0);
			routeTotal.TotalTime.Should().Not.Be.Empty();
		}

		[Test]
		public void RouteTotalCalculationWith2Addresses()
		{
			IList<Address> addresses = new List<Address>
			{
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			RouteTotalValues routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
			AssertValues.CheckRouteTotal(routeTotal);
		}

		[Test]
		public void RouteTotalCalculationWithMultipleAddressesAndDefaultQuickestRouteTest()
		{
			IList<Address> addresses = new List<Address>
			{
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			RouteTotalValues routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
			AssertValues.CheckRouteTotal(routeTotal);
		}

		[Test]
		public void RouteTotalCalculationWithMultipleAddressesAndRouteAvoidingTrafficTest()
		{
			IList<Address> addresses = new List<Address>
			{
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Av Pres Juscelino Kubitschek", "1000", "São Paulo", "SP"),
				ModelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			RouteTotalValues routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 23);
			AssertValues.CheckRouteTotal(routeTotal);
		}

		[Test]
		[ExpectedException(typeof (Exception),
			UserMessage = "Deve enviar somente 0 para rota padrão rápida ou 23 para rota evitando o trânsito")]
		public void SendDifferentRouteType()
		{
			IList<Address> addresses = new List<Address>
			{
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP")
			};

			CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 1);
		}
	}
}