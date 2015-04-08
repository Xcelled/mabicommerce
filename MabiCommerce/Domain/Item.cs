using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MabiCommerce.Domain
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Item : INotifyPropertyChanged
	{
		[JsonProperty(Required = Required.Always)]
		public int Id { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string Name { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string Image { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public int Weight { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public int QuantityPerSlot { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public int MerchantRating { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public double MultiFactor { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public double AddFactor { get; private set; }

		private int _price;
		public int Price
		{
			get { return _price; }
			set
			{
				_price = value;
				RaisePropertyChanged();
			}
		}

		private int _stock = int.MaxValue;
		public int Stock
		{
			get { return _stock; }
			set
			{
				_stock = value;
				RaisePropertyChanged();
				UpdateStatus();
			}
		}

		private bool _isRatingMet;
		public bool IsRatingMet
		{
			get { return _isRatingMet; }
			set
			{
				_isRatingMet = value;
				RaisePropertyChanged();
				UpdateStatus();
			}
		}

		private ItemStatus _status;
		public ItemStatus Status
		{
			get { return _status; }
			private set
			{
				_status = value;
				RaisePropertyChanged();
			}
		}

		public ObservableCollection<Profit> Profits { get; protected set; }

		[JsonConstructor]
		public Item(int id, string name, string image, int weight, int quantityPerSlot, int merchantRating, double multiFactor, double addFactor)
		{
			AddFactor = addFactor;
			MultiFactor = multiFactor;
			Id = id;
			Name = name;
			Image = image;
			MerchantRating = merchantRating;
			QuantityPerSlot = quantityPerSlot;
			Weight = weight;

			Profits = new ObservableCollection<Profit>();
			Price = 1;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void RaisePropertyChanged([CallerMemberName] string caller = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(caller));
			}
		}

		private void UpdateStatus()
		{
			if (!IsRatingMet)
				Status = ItemStatus.Locked;
			else if (Stock == 0)
				Status = ItemStatus.SoldOut;
			else
				Status = ItemStatus.Available;
		}

		public override string ToString()
		{
			return Name;
		}
	}

	public class Profit : INotifyPropertyChanged
	{
		public TradingPost Destination { get; private set; }

		private int _amount;
		public int Amount { get { return _amount; } set { _amount = value; RaisePropertyChanged(); } }

		public event PropertyChangedEventHandler PropertyChanged;
		private void RaisePropertyChanged([CallerMemberName] string caller = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(caller));
			}
		}

		public Profit(TradingPost destination, int profit)
		{
			Destination = destination;
			Amount = profit;
		}

		public override string ToString()
		{
			return Amount + " at " + Destination.Name;
		}
	}

	public enum ItemStatus
	{
		Locked,
		SoldOut,
		Available
	}
}
