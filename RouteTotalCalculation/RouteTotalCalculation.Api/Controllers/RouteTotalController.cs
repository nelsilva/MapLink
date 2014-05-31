using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RouteTotalCalculation.Core.ServiceAddressFinder;
using RouteTotalCalculation.Core.Services;

namespace RouteTotalCalculation.Api.Controllers
{
	[RoutePrefix("api/v1/public")]
	public class RouteTotalController : ApiController
	{
		[Route("GetRouteTotalValue")]
		public IHttpActionResult GetRouteTotalValue(string addressesJson, int routeTypes)
		{
			try
			{
				IList<Address> addresses = JsonConvert.DeserializeObject<List<Address>>(addressesJson);

				return Ok(CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, routeTypes));					
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}
	}
}