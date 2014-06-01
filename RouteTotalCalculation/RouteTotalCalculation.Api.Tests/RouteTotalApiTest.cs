using System.Web.Http;
using System.Web.Http.Results;
using NUnit.Framework;
using RouteTotalCalculation.Api.Controllers;
using RouteTotalCalculation.Core.Contracts;
using SharpTestsEx;

namespace RouteTotalCalculation.Api.Tests
{
	[TestFixture]
	public class RouteTotalApiTest
	{
		[Test]
		public void GetRouteTotalValueTest()
		{
			var controller = new RouteTotalController();
			const string addressJson =
				"[{'street':'Avenida Paulista','houseNumber':'1000','city':'São Paulo','state':'SP'},{'street':'Av Pres Juscelino Kubitschek','houseNumber':'1000','city':'São Paulo','state':'SP'},{'street':'Av Nove de Julho','houseNumber':'1500','city':'São Paulo','state':'SP'}]";
			IHttpActionResult result = controller.GetRouteTotalValue(addressJson, 0);
			Assert.IsInstanceOf(typeof (OkNegotiatedContentResult<IRouteTotalValues>), result);

			var routeTotalValue = result as OkNegotiatedContentResult<IRouteTotalValues>;
			routeTotalValue.Should().Not.Be.Null();
			if (routeTotalValue != null)
			{
				routeTotalValue.Content.TotalCost.Should().Not.Be(0);
				routeTotalValue.Content.TotalDistance.Should().Not.Be(0);
				routeTotalValue.Content.TotalTime.Should().Not.Be.Empty();
				routeTotalValue.Content.TotalfuelCost.Should().Not.Be(0);
			}
		}

		[Test]
		public void InvalidAddressTest()
		{
			var controller = new RouteTotalController();
			const string addressJson =
				"[{'street':'a','houseNumber':'b','city':'c','state':'d'},{'street':'e','houseNumber':'f','city':'g','state':'h'}]";
			IHttpActionResult result = controller.GetRouteTotalValue(addressJson, 0);
			Assert.IsInstanceOf(typeof (ExceptionResult), result);

			var exception = result as ExceptionResult;
			if (exception != null)
				exception.Exception.Message.Should().Contain("getXY: Falha ao efetuar geocode");
		}

		[Test]
		public void InvalidRouteValue()
		{
			var controller = new RouteTotalController();
			const string addressJson =
				"[{  'street':'string', 'housenumber':'1', 'city':'São Paulo','state':'SP'}]";
			IHttpActionResult result = controller.GetRouteTotalValue(addressJson, 1);
			Assert.IsInstanceOf(typeof (ExceptionResult), result);
			var exception = result as ExceptionResult;
			if (exception != null)
				exception.Exception.Message.Should()
					.Contain("Deve enviar somente 0 para rota padrão rápida ou 23 para rota evitando o trânsito");
		}

		[Test]
		public void OnlyOneAddress()
		{
			var controller = new RouteTotalController();
			const string addressJson =
				"[{  'street':'string', 'housenumber':'1', 'city':'São Paulo','state':'SP'}]";
			IHttpActionResult result = controller.GetRouteTotalValue(addressJson, 0);
			Assert.IsInstanceOf(typeof (ExceptionResult), result);
			var exception = result as ExceptionResult;
			if (exception != null)
				exception.Exception.Message.Should()
					.Contain("Deve haver ao menos dois pontos de parada. Quantidade de pontos informados: 1");
		}

		[Test]
		public void MissingArgument()
		{
			var controller = new RouteTotalController();
			IHttpActionResult result = controller.GetRouteTotalValue(null, 0);
			Assert.IsInstanceOf(typeof(ExceptionResult), result);
			var exception = result as ExceptionResult;
			if (exception != null)
				exception.Exception.Message.Should()
					.Contain("O parâmetro jsonAddresses não pode ser null ou vazio!");
		}
	}
}