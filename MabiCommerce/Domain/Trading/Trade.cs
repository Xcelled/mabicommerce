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

		public int Cost { get; private set; }
		public int Profit { get; private set; }
		public int Gold { get; private set; }
		public int MerchantRating { get; private set; }
		public int ProfitPerSecond { get; private set; }
		public long Experience { get; private set; }

		public TradeFlags Flags { get; private set; }

		public List<Modifiers> Modifiers { get; private set; }

		public Trade(Transportation transport, Route route, Load load, TradingPost source, TradingPost destination, List<Modifier> modifiers)
		{
			Destination = destination;
			Load = load;
			Route = route;
			Transport = transport;
			Modifiers = modifiers;

			Duration = TimeSpan.FromSeconds(route.Duration.TotalSeconds / 
				(Transport.SpeedFactor + modifiers.Sum(m => m.SpeedBonus)));

			Cost = load.Slots.Sum(i => i.Key.Price * i.Value);
			Profit = Gold = MerchantRating = load.CalculateProfit(destination);
			Experience = load.CalculateExperience(destination);

			Profit *= (1 + modifiers.Sum(m => m.ProfitBonus));
			Gold *= (1 + modifiers.Sum(m => m.GoldBonus));
			//MerchantRating *= (1 + modifiers.Sum(m => m.MerchantRating));
			Experience *= (1 + modifiers.Sum(m => m.ExpBonus));

			ProfitPerSecond = Profit / Duration.TotalSeconds;

			if (source.NoProfits.Contains(destination.Id))
				Flags |= TradeFlags.NoProfit;

			if (route.Path.Select(w => w.Target.Region).Any(r => r.ChokePoint))
				Flags |= TradeFlags.ChokePoint;
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
