using System.Collections.Generic;

namespace RouteTotalCalculation.Core.Contracts
{
	public interface ICalculateTotalOfRouteService
	{
		IRouteTotalValues GetTotalValuesOfRoute(IEnumerable<IAddress> addresses, int routeTypes);
	}
}
