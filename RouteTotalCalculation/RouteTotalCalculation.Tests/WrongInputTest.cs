using System;
using System.Collections.Generic;
using System.ServiceModel;
using NUnit.Framework;
using RouteTotalCalculation.Core.Contracts;
using RouteTotalCalculation.Core.Model;
using RouteTotalCalculation.Core.Services;
using SharpTestsEx;

namespace RouteTotalCalculation.Tests
{
	[TestFixture]
	public class WrongInputTest
	{
		[Test]
		[ExpectedException(typeof(FaultException),
			UserMessage = "Deve haver ao menos dois pontos de parada. Quantidade de pontos informados: 1")]
		public void OnlyOneAddress()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP")
			};

			CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(Exception),
			UserMessage = "Deve enviar somente 0 para rota padrão rápida ou 23 para rota evitando o trânsito")]
		public void SendDifferentRouteType()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP")
			};

			CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 1);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void AddressNullTest()
		{
			CalculateTotalOfRouteService.GetTotalValuesOfRoute(null, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void StreetNullTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				ModelFactory.Create(null, "1000", "São Paulo", "SP")
			};

			CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void HouseNumberNullTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				ModelFactory.Create("Avenida Paulista", null, "São Paulo", "SP")
			};

			CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CityNullTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				ModelFactory.Create("Avenida Paulista", "1000", null, "SP"),
			};

			CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void StateNullTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				ModelFactory.Create("Avenida Paulista", "1000", "São Paulo", null)
			};

			CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ArgumentEmptyTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				ModelFactory.Create(String.Empty, "1000", "São Paulo", "SP")
			};

			CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(FaultException), UserMessage = "getXY: Falha ao efetuar geocode", MatchType=MessageMatch.Contains)]
		public void InvalidAddressTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				ModelFactory.Create("5", "6", "7", "8"),
				ModelFactory.Create("1", "2", "3", "4")
			};

			CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddressWithOneOfValuesIsNUllTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				null,
				ModelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			var routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
			routeTotal.Should().Not.Be.Null();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddressWithBothValuesNullTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				null,
				null
			};

			var routeTotal = CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
			routeTotal.Should().Not.Be.Null();
		}
	}
}
