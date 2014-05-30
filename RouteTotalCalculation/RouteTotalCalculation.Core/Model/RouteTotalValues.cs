namespace RouteTotalCalculation.Core.Model
{
	public class RouteTotalValues
	{
		public RouteTotalValues(double totalDistance, string totalTime, double totalfuelCost, double totalCost)
		{
			TotalDistance = totalDistance;
			TotalTime = totalTime;
			TotalfuelCost = totalfuelCost;
			TotalCost = totalCost;
		}

		public double TotalDistance { get; private set; }

		public string TotalTime { get; private set; }

		public double TotalfuelCost { get; private set; }

		public double TotalCost { get; private set; }
	}
}
