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
		public int BaseSlots { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public int BaseWeight { get; private set; }
		[JsonProperty(Required = Required.Default)]
		public bool IsRequired { get; private set; }
		[JsonProperty(Required = Required.Default)]
		public int Id { get; private set; }

		private ObservableCollection<Modifier> _modifiers = new ObservableCollection<Modifier>();
		public ReadOnlyObservableCollection<Modifier> Modifiers { get; private set; }

		public int Slots { get { return BaseSlots + Modifiers.Where(m => m.Enabled).Sum(m => m.ExtraSlots); } }
		public int Weight { get { return BaseWeight + Modifiers.Where(m => m.Enabled).Sum(m => m.ExtraWeight); } }

		public string FullName
		{
			get
			{
				if (Modifiers.Any(m => m.Enabled))
				{
					return string.Format("{0} ({1})", Name, string.Join(", ", Modifiers.Where(m => m.Enabled).Select(m => m.Name)));
				}

				return Name;
			}
		}

		private bool _enabled;
		public bool Enabled
		{
			get { return _enabled; }
			set
			{
				if (IsRequired)
					value = true;
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
		public Transportation(string name, string icon, float speedFactor, int baseSlots, int baseWeight, bool isRequired, int id)
		{
			Id = id;
			IsRequired = isRequired;
			BaseWeight = baseWeight;
			BaseSlots = baseSlots;
			SpeedFactor = speedFactor;
			Icon = icon;
			Name = name;

			if (IsRequired)
				Enabled = true;

			Modifiers = new ReadOnlyObservableCollection<Modifier>(_modifiers);

			System.Diagnostics.Debug.Assert(BaseWeight != 0, "Base weight is 0 for " + name);
			System.Diagnostics.Debug.Assert(BaseSlots != 0, "Base slots is 0 for " + name);
		}

		public void AddModifier(Modifier mod)
		{
			_modifiers.Add(mod);
			mod.PropertyChanged += mod_PropertyChanged;
			RaisePropertyChangedExplicit("FullName");
		}

		void mod_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			RaisePropertyChangedExplicit("FullName");
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
