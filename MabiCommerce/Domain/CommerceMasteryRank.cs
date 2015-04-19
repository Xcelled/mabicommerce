using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MabiCommerce.Domain
{
	public class CommerceMasteryRank : INotifyPropertyChanged
	{
		[JsonProperty(Required = Required.Always)]
		public int Id { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string Rank { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public double Bonus { get; private set; }

		private bool _enabled;
		[JsonIgnore]
		public bool Enabled
		{
			get { return _enabled; }
			set
			{
				_enabled = value;
				RaisePropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void RaisePropertyChanged([CallerMemberName] string caller = "")
		{
			RaisePropertyChangedExplicit(caller);
		}

		private void RaisePropertyChangedExplicit(string name)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public CommerceMasteryRank(int id, string rank, double bonus)
		{
			Id = id;
			Rank = rank;
			Bonus = bonus;
		}
	}
}
