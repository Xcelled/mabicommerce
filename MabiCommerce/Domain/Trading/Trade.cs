using System;
using System.Linq;

namespace MabiCommerce.Domain.Trading
{
	public class Trade
	{
		public Transportation Transport { get; private set; }
		public Route Route { get; private set; }
		public Load Load { get; private set; }
		public TradingPost Destination { get; private set; }
		public TimeSpan Duration { get; private set; }

		public double Cost { get; private set; }
		public double Profit { get; private set; }
		public double ProfitPerSecond { get; private set; }
		public long Experience { get; private set; }

		public TradeFlags Flags { get; private set; }

		public Trade(Transportation transport, Route route, Load load, TradingPost source, TradingPost destination)
		{
			Destination = destination;
			Load = load;
			Route = route;
			Transport = transport;

			if (source.NoProfits.Contains(destination.Id))
				Flags |= TradeFlags.NoProfit;

			if (route.Path.Select(w => w.Target.Region).Any(r => r.ChokePoint))
				Flags |= TradeFlags.ChokePoint;

			Duration = TimeSpan.FromSeconds(route.Duration.TotalSeconds / Transport.SpeedFactor);

			Cost = load.Slots.Sum(i => i.Key.Price * i.Value);
			Profit = load.CalculateProfit(destination);
			ProfitPerSecond = Profit / Duration.TotalSeconds;
			Experience = load.CalculateExperience(destination);
		}

		public override string ToString()
		{
			return string.Format("Trading [{0}] to {1} via {2}", Load, Destination, Transport);
		}
	}

	[Flags]
	public enum TradeFlags
	{
		Normal,
		NoProfit = 1 << 0,
		ChokePoint = 1 << 1,
	}
}
