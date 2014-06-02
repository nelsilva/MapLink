using RouteTotalCalculation.Core.ServiceAddressFinder;

namespace RouteTotalCalculation.Core.Contracts
{
	public interface IAddressFinderService
	{
		Point GetCoordinates(Address address);
	}
}
