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
	public class MerchantLevel
	{
		[JsonProperty(Required = Required.Always)]
		public int Level { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public int Exp { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public double Discount { get; private set; }

		public MerchantLevel(int level, int exp, double discount)
		{
			Level = level;
			Exp = exp;
			Discount = discount;
		}
	}
}
