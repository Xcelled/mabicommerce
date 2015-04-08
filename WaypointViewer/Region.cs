using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WaypointViewer
{
	public class Region
	{
		public BitmapImage Image { get; private set; }
		public ObservableCollection<Waypoint> Waypoints { get; private set; }
		public ObservableCollection<Connection> Connections { get; private set; }

		public Region(List<Waypoint> waypoints, List<Connection> connections, BitmapImage img)
		{
			Waypoints = new ObservableCollection<Waypoint>(waypoints);
			Connections = new ObservableCollection<Connection>(connections);

			Image = img;
		}
	}
}
