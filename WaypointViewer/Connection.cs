using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaypointViewer
{
	public class Connection
	{
		public Waypoint Wp1 { get; set; }
		public Waypoint Wp2 { get; set; }

		public override string ToString()
		{
			return string.Format("{0} to {1}", Wp1.Id, Wp2.Id);
		}
	}
}
