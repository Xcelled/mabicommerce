using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MabiCommerce.Domain;

namespace MabiCommerce.UI
{
	/// <summary>
	/// Interaction logic for Splash.xaml
	/// </summary>
	public partial class Splash : Window
	{
		private readonly ManualResetEvent _waitTime = new ManualResetEvent(false);

		public Splash()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Task.Factory.StartNew(async delegate
			{
				await Task.Delay(3000);
				_waitTime.Set();
			});
		}

		public void ReportProgress(double percent, string message)
		{
			Dispatcher.Invoke(() =>
			{
				Progress.Value = percent;
				Message.Text = message;
			}, System.Windows.Threading.DispatcherPriority.Background);
		}

		public void Shutdown()
		{
			Task.Factory.StartNew(delegate
			{
				_waitTime.WaitOne();
				Dispatcher.InvokeShutdown();
			});
		}
	}
}
