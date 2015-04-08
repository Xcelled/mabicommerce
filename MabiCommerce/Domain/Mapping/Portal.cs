using System;
using Newtonsoft.Json;

namespace MabiCommerce.Domain.Mapping
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Portal
	{
		[JsonProperty(Required=Required.Always)]
		public string StartRegionId { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string EndRegionId { get; private set; }

		[JsonProperty(Required = Required.Always)]
		public string StartWaypointId { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string EndWaypointId { get; private set; }

		[JsonProperty(Required = Required.Always)]
		public TimeSpan Time { get; private set; }

		[JsonConstructor]
		public Portal(string startRegionId, string startWaypointId, string endRegionId, string endWaypointId, TimeSpan time)
		{
			Time = time;
			EndWaypointId = endWaypointId;
			EndRegionId = endRegionId;
			StartWaypointId = startWaypointId;
			StartRegionId = startRegionId;
		}
	}
}
