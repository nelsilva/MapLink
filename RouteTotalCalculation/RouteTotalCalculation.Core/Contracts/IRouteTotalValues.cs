namespace RouteTotalCalculation.Core.Contracts
{
	public interface IRouteTotalValues
	{
		double TotalDistance { get; }
		string TotalTime { get; }
		double TotalfuelCost { get; }
		double TotalCost { get; }
	}
}