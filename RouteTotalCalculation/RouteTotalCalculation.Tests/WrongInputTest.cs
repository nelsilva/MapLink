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
		[ExpectedException(typeof(FaultException),
			UserMessage = "Deve haver ao menos dois pontos de parada. Quantidade de pontos informados: 1")]
		public void OnlyOneAddress()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP")
			};

			_calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(Exception),
			UserMessage = "Deve enviar somente 0 para rota padrão rápida ou 23 para rota evitando o trânsito")]
		public void SendDifferentRouteType()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create("Avenida Paulista", "1000", "São Paulo", "SP")
			};

			_calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 1);
		}

		[Test]
		[ExpectedException(typeof (ArgumentNullException))]
		public void AddressNullTest()
		{
			_calculateTotalOfRouteService.GetTotalValuesOfRoute(null, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void StreetNullTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create(null, "1000", "São Paulo", "SP")
			};

			_calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void HouseNumberNullTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create("Avenida Paulista", null, "São Paulo", "SP")
			};

			_calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void CityNullTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create("Avenida Paulista", "1000", null, "SP"),
			};

			_calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void StateNullTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create("Avenida Paulista", "1000", "São Paulo", null)
			};

			_calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ArgumentEmptyTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create(String.Empty, "1000", "São Paulo", "SP")
			};

			_calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(FaultException), UserMessage = "getXY: Falha ao efetuar geocode", MatchType=MessageMatch.Contains)]
		public void InvalidAddressTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				_modelFactory.Create("5", "6", "7", "8"),
				_modelFactory.Create("1", "2", "3", "4")
			};

			_calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddressWithOneOfValuesIsNUllTest()
		{
			IEnumerable<IAddress> addresses = new List<IAddress>
			{
				null,
				_modelFactory.Create("Av Nove de Julho", "1500", "São Paulo", "SP")
			};

			var routeTotal = _calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
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

			var routeTotal = _calculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, 0);
			routeTotal.Should().Not.Be.Null();
		}
	}
}
