using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json;
using RouteTotalCalculation.Core.Model;
using RouteTotalCalculation.Core.Services;

namespace RouteTotalCalculation.Api.Controllers
{
	[RoutePrefix("api/v1/public")]
	public class RouteTotalController : ApiController
	{
		[Route("GetRouteTotalValue")]
		public IHttpActionResult GetRouteTotalValue(string jsonAddresses, int routeTypes)
		{
			try
			{
				if (String.IsNullOrEmpty(jsonAddresses))
					throw new Exception("O parâmetro jsonAddresses não pode ser null ou vazio!");

				IList<Address> addresses = JsonConvert.DeserializeObject<List<Address>>(jsonAddresses);

				return Ok(CalculateTotalOfRouteService.GetTotalValuesOfRoute(addresses, routeTypes));
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}
	}
}