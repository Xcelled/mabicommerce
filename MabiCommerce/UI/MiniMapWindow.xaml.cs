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
using MabiCommerce.Domain.Mapping;
using MabiCommerce.Domain.Trading;

namespace MabiCommerce.UI
{
	/// <summary>
	/// Interaction logic for MiniMapWindow.xaml
	/// </summary>
	public partial class MiniMapWindow : Window
	{
		public Region Region { get; private set; }
		public ReadOnlyObservableCollection<Waypoint> Waypoints { get; private set; } 
		public ReadOnlyObservableCollection<Connection> Connections { get; private set; }

		public MiniMapWindow(Route route, Region region)
		{
			InitializeComponent();
			Region = region;

			var waypoints = new List<Waypoint>();
			var connections = new List<Connection>();

			foreach (var conn in route.Path)
			{
				if (conn.Source.Region == region)
					waypoints.Add(conn.Source);

				if (conn.Target.Region == region)
					waypoints.Add(conn.Target);

				if (conn.Source.Region == region && conn.Target.Region == region)
					connections.Add(conn);
			}

			Waypoints = new ReadOnlyObservableCollection<Waypoint>(new ObservableCollection<Waypoint>(waypoints));
			Connections = new ReadOnlyObservableCollection<Connection>(new ObservableCollection<Connection>(connections));

			DataContext = this;
		}
	}

	public class ConnectionToGeometryConverter : ConverterBase
	{
		public ConnectionToGeometryConverter()
		{
			
		}

		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var conn = (Connection)value;

			var endpoint1 = new Point(conn.Source.Location.X, (conn.Source.Region.Size.Height - conn.Source.Location.Y));
			var endpoint2 = new Point(conn.Target.Location.X, (conn.Target.Region.Size.Height - conn.Target.Location.Y));

			endpoint1.Offset(-conn.Source.Region.MiniMapOffset.X, -conn.Source.Region.MiniMapOffset.Y);
			endpoint2.Offset(-conn.Target.Region.MiniMapOffset.X, -conn.Target.Region.MiniMapOffset.Y);

			return new LineGeometry(endpoint1, endpoint2);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class TranslateAndScaleWidthConverter : MultiConverterBase
	{
		public TranslateAndScaleWidthConverter()
		{
			
		}

		public override object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var sourceX = (double)values[0];
			var sourceOffset = (double)values[1];
			var imageWidth = (double)values[2];
			var regionWidth = (double)values[3];

			return (sourceX - sourceOffset) * (imageWidth / regionWidth);
		}

		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class TranslateAndScaleHeightConverter : MultiConverterBase
	{
		public TranslateAndScaleHeightConverter()
		{
			
		}

		public override object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var sourceY = (double)values[0];
			var sourceOffset = (double)values[1];
			var imageHeight = (double)values[2];
			var regionHeight = (double)values[3];

			return (regionHeight - (sourceY - sourceOffset)) * (imageHeight / regionHeight);
		}

		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class PercentToStringConverter : ConverterBase
	{
		public PercentToStringConverter()
		{
			
		}

		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ((int)(((double)value) * 100)).ToString();
		}

		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return double.Parse((string)value) / 100;
		}
	}
}
