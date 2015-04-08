using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace MabiCommerce.Domain.Mapping
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Waypoint
	{
		[JsonProperty(Required=Required.Always)]
		public string Id { get; private set; }

		[JsonProperty(Required = Required.Always)]
		public Point Location { get; private set; }

		public Region Region { get; set; }

		[JsonConstructor]
		public Waypoint(string id, Point location)
		{
			Id = id;
			Location = location;
		}

		public override string ToString()
		{
			return string.Format("{0} - {1}", Id, Location);
		}
	}
}
