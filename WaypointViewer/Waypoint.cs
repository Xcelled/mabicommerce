using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WaypointViewer
{
	public class Waypoint
	{
		public string Id { get; set; }
		public Point Location { get; set; }

		public override string ToString()
		{
			return string.Format("{0} - {1}", Id, Location);
		}
	}
}
