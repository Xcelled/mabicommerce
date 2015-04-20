using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MabiCommerce.Domain;
using MabiCommerce.Domain.Trading;
using MabiCommerce.Network;

namespace MabiCommerce.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public Erinn Erinn { get; private set; }

		public MainWindow()
		{
			InitializeComponent();

			Environment.CurrentDirectory = Path.GetDirectoryName(GetType().Assembly.Location);

			DataContext = Erinn = Erinn.Load(@"Data", App.Splash.ReportProgress);

			App.Splash.ReportProgress(1.0, "Loading main window...");

			App.Splash.Shutdown();
		}

		private void WindowBar_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				DragMove();
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void MinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		private void CalculateTrades_Click(object sender, RoutedEventArgs e)
		{
			CalculateTrades();
		}

		private void MapItButton_Click(object sender, RoutedEventArgs e)
		{
			if (TradeSelect.SelectedItem == null)
				return;

			new WorldMapWindow(TradeSelect.SelectedItem as Trade).Show();
		}

		public void CalculateTrades()
		{
			if (!CalculateTradesBtn.IsEnabled)
				return;

			CalculateTradesBtn.IsEnabled = false;

			var post = PostSelect.SelectedItem as TradingPost;

			Task.Factory.StartNew(delegate
			{
				var trades = Erinn.CalculateTrades(post);

				Dispatcher.Invoke(delegate
				{
					Erinn.Trades.Clear();

					foreach (var t in trades)
						Erinn.Trades.Add(t);

					CalculateTradesBtn.IsEnabled = true;
					TradeSelect.ScrollIntoView(TradeSelect.Items[0]);
				});
			});
		}

		private void ConnectBtn_Click(object sender, RoutedEventArgs e)
		{
			if (AlissaHandle == IntPtr.Zero)
				SelectPacketProvider(true);

			Connect();
		}

		private void DisonnectBtn_Click(object sender, RoutedEventArgs e)
		{
			Disconnect();
		}

		private void SettingsBtn_Click(object sender, RoutedEventArgs e)
		{
			new Settings().ShowDialog();
		}

		private void ItemSelect_TargetUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
		{
			ItemSelect.SelectedIndex = 0;
		}
	}
}
