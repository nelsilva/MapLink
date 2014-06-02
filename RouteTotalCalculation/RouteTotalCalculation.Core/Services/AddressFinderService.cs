using RouteTotalCalculation.Core.Contracts;
using RouteTotalCalculation.Core.ServiceAddressFinder;

namespace RouteTotalCalculation.Core.Services
{
	public class AddressFinderService : IAddressFinderService
	{
		public Point GetCoordinates(Address address)
		{
			using (var addressFinderSoapClient = new AddressFinderSoapClient())
			{
				return addressFinderSoapClient.getXY(address, Configuration.TokenValue);
			}
		}
	}
}