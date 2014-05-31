using System;
using System.Collections.Generic;
using System.Linq;
using RouteTotalCalculation.Core.ServiceAddressFinder;

namespace RouteTotalCalculation.Core.Services
{
	public static class AddressFinderService
	{
		public static AddressInfo GetFindAddressResponse(Address address, AddressOptions addressOptions)
		{
			using (var addressFinderSoapClient = new AddressFinderSoapClient())
			{
				try
				{
					return addressFinderSoapClient.findAddress(address, addressOptions, Configuration.TokenValue);
				}
				catch (Exception)
				{
					
					throw;
				}
			}
		}

		public static Point GetXY(Address address)
		{
			using (var addressFinderSoapClient = new AddressFinderSoapClient())
			{
				try
				{
					return addressFinderSoapClient.getXY(address, Configuration.TokenValue);
				}
				catch (Exception)
				{
					
					throw;
				}
			}
		}

		public static IList<AddressLocation> GetAddressLocationFromAddresses(IEnumerable<Address> addresses)
		{
			try
			{
				return addresses.Select(address => new AddressLocation
				{
					address = address,
					point = GetXY(address)
				}).ToList();
			}
			catch (Exception)
			{
				
				throw;
			}
		}
	}
}