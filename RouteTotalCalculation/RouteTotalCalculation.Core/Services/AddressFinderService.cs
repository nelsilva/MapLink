using System.Collections.Generic;
using System.Linq;
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

		public static IList<AddressLocation> GetAddressLocationFromAddresses(IEnumerable<Address> addresses)
		{
			return addresses.Select(address => new AddressLocation
			{
				address = address,
				point = GetCoordinates(address)
			}).ToList();
		}
	}
}