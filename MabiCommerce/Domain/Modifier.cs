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
	public class Modifier : INotifyPropertyChanged
	{
		[JsonProperty(Required = Required.Always)]
		public int Id { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string Name { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public int ExtraSlots { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public int ExtraWeight { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public List<int> AppliesTo { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public List<int> ConflictsWith { get; private set; }

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

		public Modifier(int id, string name, int extraSlots, int extraWeight, List<int> appliesTo, List<int> conflictsWith)
		{
			ConflictsWith = conflictsWith;
			AppliesTo = appliesTo;
			ExtraWeight = extraWeight;
			ExtraSlots = extraSlots;
			Name = name;
			Id = id;
		}
	}
}
