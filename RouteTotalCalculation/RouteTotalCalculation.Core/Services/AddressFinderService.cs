using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using RouteTotalCalculation.Core.ServiceAddressFinder;

namespace RouteTotalCalculation.Core.Services
{
	public static class AddressFinderService
	{
		public static AddressInfo GetFindAddressResponse(Address address, AddressOptions addressOptions)
		{
			using (var addressFinderSoapClient = new AddressFinderSoapClient())
			{
				return addressFinderSoapClient.findAddress(address, addressOptions, Configuration.TokenValue);
			}
		}

		public static Point GetXY(Address address)
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
				point = GetXY(address)
			}).ToList();
		}		
	}
}
