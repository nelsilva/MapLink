using System;
using RouteTotalCalculation.Core.Contracts;

namespace RouteTotalCalculation.Core.Model
{
	public class Address : IAddress
	{
		public Address(string street, string houseNumber, string city, string state)
		{
			if (String.IsNullOrEmpty(city)) throw new ArgumentException("city");
			if (String.IsNullOrEmpty(houseNumber)) throw new ArgumentException("houseNumber");
			if (String.IsNullOrEmpty(state)) throw new ArgumentException("state");
			if (String.IsNullOrEmpty(street)) throw new ArgumentException("street");

			Street = street;
			HouseNumber = houseNumber;
			City = city;
			State = state;
		}

		public string Street { get; private set; }
		public string HouseNumber { get; private set; }
		public string City { get; private set; }
		public string State { get; private set; }
	}
}
