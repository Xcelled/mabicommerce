using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MabiCommerce.Domain.Mapping;
using Newtonsoft.Json;

namespace MabiCommerce.Domain
{
	[JsonObject(MemberSerialization.OptIn)]
	public class TradingPost : INotifyPropertyChanged
	{
		[JsonProperty(Required=Required.Always)]
		public int Id { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string Name { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string Image { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public ObservableCollection<Item> Items { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string WaypointRegion { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string WaypointId { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public List<int> NoProfits { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public Dictionary<int, double> Weights { get; private set; }

		public Waypoint Waypoint { get; set; }

		private MerchantRating _merchantRating;
		public MerchantRating MerchantRating
		{
			get { return _merchantRating; }
			set
			{
				_merchantRating = value;
				RaisePropertyChanged();

				foreach (var item in Items)
				{
					item.IsRatingMet = value >= item.MerchantRating;
				}
			}
		}

		[JsonConstructor]
		public TradingPost(int id, string name, string image, ObservableCollection<Item> items, string waypointRegion, string waypointId, List<int> noProfits, Dictionary<int, double> weights)
		{
			Weights = weights;
			NoProfits = noProfits;
			WaypointId = waypointId;
			WaypointRegion = waypointRegion;
			Id = id;
			Items = items;
			Image = image;
			Name = name;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void RaisePropertyChanged([CallerMemberName] string caller = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(caller));
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
