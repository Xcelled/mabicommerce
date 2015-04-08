using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WaypointViewer
{
	public abstract class ConverterBase : MarkupExtension, IValueConverter
	{
		public sealed override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}

		public abstract object Convert(object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture);

		public abstract object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture);
	}

	public class WaypointToGeometryConverter : ConverterBase
	{
		public WaypointToGeometryConverter()
		{

		}

		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var wp = (Waypoint)value;

			var geoGroup = new GeometryGroup();

			geoGroup.Children.Add(new EllipseGeometry(wp.Location, 5, 5));

			var textLocation = new Point(wp.Location.X, wp.Location.Y + 7);

			var text = new FormattedText(wp.Id, culture, FlowDirection.LeftToRight,
				new Typeface("Tahoma"), 12, Brushes.Blue);

			var textGeo = text.BuildGeometry(textLocation);

			geoGroup.Children.Add(textGeo);

			return geoGroup;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
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

			return new LineGeometry(conn.Wp1.Location, conn.Wp2.Location);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class StringPercentToDoubleConverter : ConverterBase
	{
		public StringPercentToDoubleConverter()
		{
			
		}

		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return double.Parse((string)value) / 100;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return ((double)value * 100).ToString();
		}
	}
}
