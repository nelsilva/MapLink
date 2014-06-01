using RouteTotalCalculation.Core.ServiceAddressFinder;

namespace RouteTotalCalculation.Core.Services
{
	public static class AddressFinderService
	{
		public static Point GetCoordinates(Address address)
		{
			using (var addressFinderSoapClient = new AddressFinderSoapClient())
			{
				return addressFinderSoapClient.getXY(address, Configuration.TokenValue);
			}
		}
	}
}