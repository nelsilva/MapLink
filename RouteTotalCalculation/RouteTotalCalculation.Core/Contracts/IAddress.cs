namespace RouteTotalCalculation.Core.Contracts
{
	public interface IAddress
	{
		string Street { get; }
		string HouseNumber { get; }
		string City { get; }
		string State { get; }
	}
}
