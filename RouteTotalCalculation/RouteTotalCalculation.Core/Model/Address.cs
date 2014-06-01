namespace RouteTotalCalculation.Core.Model
{
	public class Address
	{
		public Address(string street, string houseNumber, string city, string state)
		{
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
