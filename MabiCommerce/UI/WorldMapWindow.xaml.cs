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
	/// Interaction logic for WorldMapWindow.xaml
	/// </summary>
	public partial class WorldMapWindow : Window
	{
		public Trade Trade { get; private set; }
		public ReadOnlyObservableCollection<ColoredRegion> MappedRegions { get; private set; }

		public static readonly RoutedUICommand OpenMiniMap = new RoutedUICommand("Open minimap for region", "OpenMiniMap",
			typeof(WorldMapWindow));

		public WorldMapWindow(Trade trade)
		{
			InitializeComponent();
			Trade = trade;

			var regions = trade.Route.Path.Select(p => p.Target.Region).Distinct();

			var mappedRegions = new List<ColoredRegion>();
			foreach (var r in regions)
			{
				mappedRegions.Add(new ColoredRegion(r, trade.Route, Brushes.Lime));
			}

			MappedRegions =
				new ReadOnlyObservableCollection<ColoredRegion>(new ObservableCollection<ColoredRegion>(mappedRegions));

			DataContext = this;
		}

		private void OpenMiniMap_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			var r = (ColoredRegion)e.Parameter;

			new MiniMapWindow(r.Route, r.Region).Show();

		}
	}

	public class ColoredRegion
	{
		public Region Region { get; private set; }
		public Brush Color { get; private set; }
		public Route Route { get; private set; }

		public ColoredRegion(Region region, Route route, Brush color)
		{
			Route = route;
			Color = color;
			Region = region;
		}
	}
}
