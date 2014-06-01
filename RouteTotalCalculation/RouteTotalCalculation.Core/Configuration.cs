using System.Configuration;

namespace RouteTotalCalculation.Core
{
	public static class Configuration
	{
		public static string TokenValue
		{
			get { return ConfigurationManager.AppSettings["token"] ?? "TokenNotFound"; }
		}
	}
}