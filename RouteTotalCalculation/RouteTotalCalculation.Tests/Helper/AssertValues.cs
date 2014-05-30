using RouteTotalCalculation.Core.ServiceRoute;
using SharpTestsEx;

namespace RouteTotalCalculation.Tests.Helper
{
	public static class AssertValues
	{
		public static void CheckRouteTotal(RouteTotals routeTotal)
		{
			routeTotal.Should().Not.Be.Null();
			routeTotal.totalCost.Should().Not.Be(0);
			routeTotal.totalDistance.Should().Not.Be(0);
			routeTotal.totalFuelUsed.Should().Not.Be(0);
			routeTotal.totalTime.Should().Not.Be.Empty();
			routeTotal.totalfuelCost.Should().Not.Be(0);
			routeTotal.totaltollFeeCost.Should().Not.Be.GreaterThan(1);
		}
	}
}
