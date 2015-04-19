using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MabiCommerce.Domain.Trading
{
	public class Load
	{
		private readonly Dictionary<Item, int> _slots;
		public ReadOnlyDictionary<Item, int> Slots { get; private set; }

		public int SlotCapacity { get; private set; }
		public int SlotsUsed { get { return _slots.Sum(kvp => (int)Math.Ceiling(kvp.Value / (double)kvp.Key.QuantityPerSlot)); } }
		public int RemainingSlots { get { return SlotCapacity - SlotsUsed; } }

		public int WeightCapacity { get; private set; }
		public int Weight { get { return _slots.Sum(kvp => kvp.Value * kvp.Key.Weight); } }
		public int RemainingWeight { get { return WeightCapacity - Weight; } }

		public Load(int maxSlots, int weightCapacity)
		{
			SlotCapacity = maxSlots;
			WeightCapacity = weightCapacity;

			_slots = new Dictionary<Item, int>();
			Slots = new ReadOnlyDictionary<Item, int>(_slots);
		}

		public int CalculateProfit(TradingPost destination)
		{
			return _slots.Sum(kvp => kvp.Key.Profits.First(x => x.Destination == destination).Amount * kvp.Value);
		}

		public int CalculateExperience(TradingPost destination)
		{
			// http://wiki.mabinogiworld.com/view/Commerce
			// floor(sqrt(Single Item Profit * Single Item Weight)) * Item Quantity * 30

			return _slots.Sum(kvp =>
				(int)(Math.Sqrt(kvp.Key.Profits.First(x => x.Destination == destination).Amount * kvp.Key.Weight)) * kvp.Value * 30);
		}

		public override string ToString()
		{
			return string.Format("Load of: {0}", string.Join(",", _slots.Keys.Select(i => i.Name)));
		}

		/// <summary>
		/// Adds the item to the load.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="amount">The requested amount.</param>
		/// <returns>The amount of the item that was actually added to the load.</returns>
		public int AddItem(Item item, int amount)
		{
			amount = Math.Min(amount, RemainingWeight / item.Weight);
			amount = Math.Min(amount, RemainingSlots * item.QuantityPerSlot);

			if (amount != 0)
			{
				if (!_slots.ContainsKey(item))
					_slots[item] = amount;
				else
					_slots[item] += amount;
			}

			return amount;
		}

		public Load Copy()
		{
			var newLoad = new Load(SlotCapacity, WeightCapacity);

			foreach (var kvp in _slots)
				newLoad._slots.Add(kvp.Key, kvp.Value);

			return newLoad;
		}
	}
}
