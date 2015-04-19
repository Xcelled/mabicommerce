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
		public TradeFlags Flags { get; private set; }
		public List<Modifiers> Modifiers { get; private set; }
		public double ProfitPerSecond { get; private set; }

		public TimeSpan Duration { get; private set; }
		public int Cost { get; private set; }
		public int Profit { get; private set; }
		public int Gold { get; private set; }
		public int MerchantRating { get; private set; }
		public int Experience { get; private set; }

		public TimeSpan BaseDuration { get; private set; }
		public int BaseCost { get; private set; }
		public int BaseProfit { get; private set; }
		public int BaseGold { get; private set; }
		public int BaseMerchantRating { get; private set; }
		public int BaseExperience { get; private set; }

		public TimeSpan AddedDuration { get; private set; }
		public int AddedCost { get; private set; }
		public int AddedProfit { get; private set; }
		public int AddedGold { get; private set; }
		public int AddedMerchantRating { get; private set; }
		public int AddedExperience { get; private set; }

		public Trade(Transportation transport, Route route, Load load, TradingPost source, TradingPost destination, List<Modifier> modifiers)
		{
			Destination = destination;
			Load = load;
			Route = route;
			Transport = transport;
			Modifiers = modifiers;

			BaseDuration = TimeSpan.FromSeconds(route.Duration.TotalSeconds / Transport.SpeedFactor);
			BaseCost = load.Slots.Sum(i => i.Key.Price * i.Value);
			BaseProfit = BaseGold = BaseMerchantRating = load.CalculateProfit(destination);
			BaseExperience = load.CalculateExperience(destination);

			Profit = BaseProfit * (1 + modifiers.Sum(m => m.ProfitBonus));
			Gold = BaseGold * (1 + modifiers.Sum(m => m.GoldBonus));
			MerchantRating = BaseMerchantRating * (1 + modifiers.Sum(m => m.MerchantRating));
			Experience = BaseExperience * (1 + modifiers.Sum(m => m.ExpBonus));
			Duration = BaseDuration / (1  + modifiers.Sum(m => m.SpeedBonus));

			Gold = Math.Max(0, Gold);
			MerchantRating = Math.Max(0, MerchantRating);
			Experience = Math.Max(0, Experience);

			AddedProfit = Profit - BaseProfit;
			AddedGold = Gold - BaseGold;
			AddedMerchantRating = MerchantRating - BaseMerchantRating;
			AddedExperience = Experience - BaseExperience;
			AddedDuration = Duration - BaseDuration;

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
