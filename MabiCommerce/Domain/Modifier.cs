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
		[JsonProperty(Required = Required.Default)]
		public int ExtraSlots { get; private set; }
		[JsonProperty(Required = Required.Default)]
		public int ExtraWeight { get; private set; }
		[JsonProperty(Required = Required.Default)]
		public double SpeedBonus { get; private set; }
		[JsonProperty(Required = Required.Default)]
		public double ExpBonus { get; private set; }
		[JsonProperty(Required = Required.Default)]
		public double GoldBonus { get; private set; }
		[JsonProperty(Required = Required.Default)]
		public double ProfitBonus { get; private set; }
		[JsonProperty(Required = Required.Default)]
		public List<int> TransportationBlacklist { get; private set; }
		[JsonProperty(Required = Required.Default)]
		public List<int> ConflictsWith { get; private set; }

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

		[JsonIgnore]
		public string EffectDescription { get; private set; }

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

		public Modifier(int id, string name, int extraSlots, int extraWeight,
			double speedBonus, double expBonus, double goldBonus, double profitBonus,
			List<int> transportationBlacklist, List<int> conflictsWith)
		{
			Id = id;
			Name = name;

			ExtraSlots = extraSlots;
			ExtraWeight = extraWeight;
			SpeedBonus = speedBonus;
			ExpBonus = expBonus;
			GoldBonus = goldBonus;
			ProfitBonus = profitBonus;

			ConflictsWith = conflictsWith;
			TransportationBlacklist = transportationBlacklist;

			const string intFormat = "+#,###;-#,###";
			const string percentFormat = "+#,##0.##%;-#,##0.##%";

			var effects = new StringBuilder();
			if (extraSlots != 0)
				effects.AppendLine(string.Format("{0} Slots", extraSlots.ToString(intFormat)));
			if (extraWeight != 0)
				effects.AppendLine(string.Format("{0} Weight", extraWeight.ToString(intFormat)));
			if (speedBonus != 0.0)
				effects.AppendLine(string.Format("{0} Speed", speedBonus.ToString(percentFormat)));
			if (profitBonus != 0.0)
				effects.AppendLine(string.Format("{0} Profit", profitBonus.ToString(percentFormat)));
			if (goldBonus != 0.0)
				effects.AppendLine(string.Format("{0} Gold", goldBonus.ToString(percentFormat)));
			if (expBonus != 0.0)
				effects.AppendLine(string.Format("{0} Exp", expBonus.ToString(percentFormat)));

			EffectDescription = effects.ToString().Trim();
		}
	}
}
