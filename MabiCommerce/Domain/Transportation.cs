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
	public class Transportation : INotifyPropertyChanged
	{
		[JsonProperty(Required=Required.Always)]
		public string Name { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string Icon { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public float SpeedFactor { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public int Slots { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public int Weight { get; private set; }
		[JsonProperty(Required = Required.Default)]
		public bool IsRequired { get; private set; }
		[JsonProperty(Required = Required.Default)]
		public int Id { get; private set; }

		private bool _enabled;
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

		[JsonConstructor]
		public Transportation(string name, string icon, float speedFactor, int slots, int weight, bool isRequired, int id)
		{
			Id = id;
			IsRequired = isRequired;
			Weight = weight;
			Slots = slots;
			SpeedFactor = speedFactor;
			Icon = icon;
			Name = name;

			if (IsRequired)
				Enabled = true;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
