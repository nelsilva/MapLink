using RouteTotalCalculation.Core.Contracts;
using SharpTestsEx;

namespace RouteTotalCalculation.Tests.Helper
{
	public static class AssertValues
	{
		public static void CheckRouteTotal(IRouteTotalValues routeTotal)
		{
			routeTotal.Should().Not.Be.Null();
			routeTotal.TotalCost.Should().Not.Be(0);
			routeTotal.TotalDistance.Should().Not.Be(0);
			routeTotal.TotalfuelCost.Should().Not.Be(0);
			routeTotal.TotalTime.Should().Not.Be.Empty();
		}
	}
}