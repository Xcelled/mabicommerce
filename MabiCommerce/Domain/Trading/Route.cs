using System;
using System.Collections.Generic;
using System.Linq;
using MabiCommerce.Domain.Mapping;

namespace MabiCommerce.Domain.Trading
{
	public class Route
	{
		public Waypoint Origin { get; private set; }
		public Waypoint Destination { get; private set; }
		public TimeSpan Duration { get; private set; }

		public IReadOnlyList<Connection> Path { get; private set; }

		public Route(List<Connection> path)
		{
			Path = path.AsReadOnly();

			Origin = path.First().Source;
			Destination = path.Last().Target;

			Duration = TimeSpan.FromTicks(path.Sum(c => c.Time.Ticks));
		}

		public override string ToString()
		{
			return string.Format("{0} to {1} in {2}", Origin, Destination, Duration);
		}
	}
}
