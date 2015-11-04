using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Schema;
using MabiCommerce.Domain;
using MabiCommerce.UI;
using Newtonsoft.Json;

namespace MabiCommerce
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static  Splash Splash;

		private ManualResetEvent _resetSplashCreated;
		private Thread _splashThread;
		protected override void OnStartup(StartupEventArgs e)
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			// ManualResetEvent acts as a block.  It waits for a signal to be set.
			_resetSplashCreated = new ManualResetEvent(false);

			// Create a new thread for the splash screen to run on
			_splashThread = new Thread(ShowSplash);
			_splashThread.SetApartmentState(ApartmentState.STA);
			_splashThread.IsBackground = true;
			_splashThread.Start();

			// Wait for the blocker to be signaled before continuing. This is essentially the same as: while(ResetSplashCreated.NotSet) {}
			_resetSplashCreated.WaitOne();

			base.OnStartup(e);

			if (MabiCommerce.Properties.Settings.Default.UpdateCheck)
			{
				Task.Factory.StartNew(CheckForUpdates);
			}

			Environment.CurrentDirectory = Path.GetDirectoryName(typeof(MainWindow).Assembly.Location);

			Erinn erinn;

			try
			{
				erinn = Erinn.Load(@"Data", Splash.ReportProgress);
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to load MabiCommerce's data.", ex);
			}

			Splash.ReportProgress(1.0, "Loading main window...");
			var mw = new MainWindow(erinn);
			mw.Show();
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var w = new UnhandledExceptionWindow(e.ExceptionObject as Exception);
			w.ShowDialog();
			Shutdown();
			Environment.Exit(1);
		}

		private static async void CheckForUpdates()
		{
			await Task.Delay(5000);

			try
			{
				using (var wc = new WebClient())
				{
					var latest =
						Version.Parse(wc.DownloadString("https://raw.githubusercontent.com/Xcelled/mabicommerce/master/latest"));

					var current = typeof(Settings).Assembly.GetName().Version;
					if (current < latest)
					{
						if (MessageBox.Show(string.Format(@"There is a new version of MabiCommerce available!

You're running: {0}
Latest: {1}

Would you like to download the new version?", current, latest), "Update Available", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
						{
							Process.Start(@"https://github.com/Xcelled/mabicommerce");
						}
					}
				}
			}
			catch
			{
				
			}
		}

		private void ShowSplash()
		{
			// Create the window
			Splash = new Splash();

			// Show it
			Splash.Show();

			// Now that the window is created, allow the rest of the startup to run
			_resetSplashCreated.Set();
			System.Windows.Threading.Dispatcher.Run();
		}
	}
}
