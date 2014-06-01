using System.Web.Http;
using NUnit.Framework;
using RouteTotalCalculation.Api.Controllers;

namespace RouteTotalCalculation.Api.Tests
{
	[TestFixture]
	public class RouteTotal
	{
		[Test]
		public void GetRouteTotalValueTest()
		{
			var controller = new RouteTotalController();
			const string addressJson =
				"[{\"street\":\"Avenida Paulista\",\"houseNumber\":\"1000\",\"city\":{\"name\":\"São Paulo\",\"state\":\"SP\"}},{\"street\":\"Av Pres Juscelino Kubitschek\",\"houseNumber\":\"1000\",\"city\":{\"name\":\"São Paulo\",\"state\":\"SP\"}},{\"street\":\"Av Nove de Julho\",\"houseNumber\":\"1500\",\"city\":{\"name\":\"São Paulo\",\"state\":\"SP\"}}]";
			IHttpActionResult result = controller.GetRouteTotalValue(addressJson, 0);
		}
	}
}