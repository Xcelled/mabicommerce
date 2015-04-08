using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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
