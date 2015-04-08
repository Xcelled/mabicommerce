using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using QuickGraph;

namespace MabiCommerce.Domain.Mapping
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Region
	{
		/// <summary>
		/// This is the speed, in cm/s, of a human using a handcart.
		/// </summary>
		private const double BaseSpeed = 373.8506;

		[JsonProperty(Required = Required.Always)]
		public string Id { get; private set; }

		[JsonProperty(Required = Required.Always)]
		public List<Waypoint> WaypointList { get; private set; }

		[JsonProperty(Required = Required.Always)]
		public List<Tuple<string, string>> Connections { get; private set; }

		[JsonProperty(Required = Required.Always)]
		public Size Size { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string MiniMap { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public Point MiniMapOffset { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public string WorldMap { get; private set; }
		[JsonProperty(Required = Required.Always)]
		public Point WorldMapOffset { get; private set; }

		[JsonProperty(Required=Required.Default)]
		public bool ChokePoint { get; private set; }

		public Dictionary<string, Waypoint> Waypoints { get; private set; }
		public AdjacencyGraph<Waypoint, Connection> RegionGraph { get; private set; }

		[JsonConstructor]
		public Region(string id, List<Waypoint> waypointList, List<Tuple<string, string>> connections,
			Size size, string miniMap, Point miniMapOffset, string worldMap, Point worldMapOffset, bool chokePoint)
		{
			ChokePoint = chokePoint;
			WorldMapOffset = worldMapOffset;
			WorldMap = worldMap;
			MiniMapOffset = miniMapOffset;
			MiniMap = miniMap;
			Size = size;
			Id = id;
			WaypointList = waypointList;
			Connections = connections;

			foreach (var wp in WaypointList)
			{
				wp.Region = this;
			}

			Waypoints = WaypointList.ToDictionary(x => x.Id.ToLowerInvariant());

			RegionGraph = new AdjacencyGraph<Waypoint, Connection>(true);
			RegionGraph.AddVertexRange(waypointList);

			foreach (var c in Connections)
			{
				var wp1 = Waypoints[c.Item1];
				var wp2 = Waypoints[c.Item2];

				var dist = Math.Sqrt(Math.Pow(wp1.Location.X - wp2.Location.X, 2) + Math.Pow(wp1.Location.Y - wp2.Location.Y, 2));

				var time = TimeSpan.FromSeconds(dist / BaseSpeed);

				RegionGraph.AddEdge(new Connection(wp1, wp2, time));
				RegionGraph.AddEdge(new Connection(wp2, wp1, time));
			}
		}

		public override string ToString()
		{
			return Id;
		}
	}
}
