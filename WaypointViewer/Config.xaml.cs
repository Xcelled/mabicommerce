using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace WaypointViewer
{
	/// <summary>
	/// Interaction logic for Config.xaml
	/// </summary>
	public partial class Config : Window
	{
		public string WaypointJson { get; set; }
		public string ConnectionJson { get; set; }
		public string ImagePath { get; set; }
		public double MapWidth { get; set; }
		public double MapHeight { get; set; }
		public double MapOffsetX { get; set; }
		public double MapOffsetY { get; set; }

		public Config()
		{
			InitializeComponent();

			DataContext = this;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{

			var img = new BitmapImage(new Uri(System.IO.Path.GetFullPath(ImagePath), UriKind.Absolute));

			var xScale = img.Width / MapWidth;
			var yScale = img.Height / MapHeight;
			
			var waypoints = JsonConvert.DeserializeObject<List<Waypoint>>(WaypointJson);

			var connections = new List<Connection>();

			foreach (var w in JsonConvert.DeserializeObject<List<Tuple<string, string>>>(ConnectionJson))
			{
				var w1 = waypoints.FirstOrDefault(x => x.Id.Equals(w.Item1, StringComparison.OrdinalIgnoreCase));
				var w2 = waypoints.FirstOrDefault(x => x.Id.Equals(w.Item2, StringComparison.OrdinalIgnoreCase));

				connections.Add(new Connection { Wp1 = w1, Wp2 = w2 });
			}

			foreach (var w in waypoints)
			{
				w.Location = new Point((w.Location.X - MapOffsetX ) * xScale, (w.Location.Y  - MapOffsetY) * yScale);
			}

			var mappedRegion = new Region(waypoints, connections, img);

			new MainWindow(mappedRegion).ShowDialog();
		}
	}
}
