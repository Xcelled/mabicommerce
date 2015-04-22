#if AUTODETECT

using MabiCommerce.Domain.Trading;
using MabiCommerce.Domain;
using MabiCommerce.Network;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows;
using System;

namespace MabiCommerce.UI
{
	/// <summary>
	/// Network logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged
	{
		private Window _hiddenWindow;
		private HwndSource _hwndSource;
		private bool _connected;

		public IntPtr AlissaHandle { get; private set; }

		public bool Connected
		{
			get { return _connected; }
			private set
			{
				_connected = value;
				RaisePropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged([CallerMemberName] string caller = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(caller));
			}
		}

		private void Window_Network_Closing(object sender, CancelEventArgs e)
		{
			Disconnect();
		}

		private void Window_Network_Loaded(object sender, RoutedEventArgs e)
		{
			_hiddenWindow = new Window()
			{
				Width = 0,
				Height = 0,
				WindowStyle = WindowStyle.None,
				ShowInTaskbar = false,
				ShowActivated = false,
				Background = Brushes.Transparent,
				AllowsTransparency = true,
				Title = "MabiCommerce",
				Owner = this,
			};

			_hiddenWindow.Show();

			_hwndSource = PresentationSource.FromVisual(_hiddenWindow) as HwndSource;
			_hwndSource.AddHook(WndProc);
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
		{
			if (msg == WinApi.WM_COPYDATA)
			{
				var cds = (WinApi.COPYDATASTRUCT)Marshal.PtrToStructure(lparam, typeof(WinApi.COPYDATASTRUCT));

				if (cds.cbData < 12)
					return IntPtr.Zero;

				var recv = (int)cds.dwData == Sig.Recv;

				if (!recv)
					return IntPtr.Zero;

				var data = new byte[cds.cbData];
				Marshal.Copy(cds.lpData, data, 0, cds.cbData);

				var packet = new Packet(data, 0);

				HandlePacket(packet);
			}

			return (IntPtr)1;
		}

		public void SelectPacketProvider(bool selectSingle)
		{
			var alissaWindows = WinApi.FindAllWindows("mod_Alissa");

			if (alissaWindows.Count == 0)
			{
				MessageBox.Show("No packet provider found.", Title, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			AlissaHandle = alissaWindows[0].HWnd;
		}

		public bool Connect()
		{
			if (!WinApi.IsWindow(AlissaHandle))
			{
				AlissaHandle = IntPtr.Zero;
				return false;
			}

			SendAlissa(AlissaHandle, Sig.Connect);

			Task.Factory.StartNew(async delegate
			{
				while (AlissaHandle != IntPtr.Zero)
				{
					if (!WinApi.IsWindow(AlissaHandle))
						Disconnect();
					else
						await Task.Delay(5000);
				}
			});

			Connected = true;

			return true;
		}

		public void Disconnect()
		{
			if (WinApi.IsWindow(AlissaHandle))
			{
				SendAlissa(AlissaHandle, Sig.Disconnect);
			}

			Connected = false;

			AlissaHandle = IntPtr.Zero;
		}

		/// <summary>
		/// Sends message to Alissa window.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="op"></param>
		private void SendAlissa(IntPtr hWnd, int op)
		{
			WinApi.COPYDATASTRUCT cds;
			cds.dwData = (IntPtr)op;
			cds.cbData = 0;
			cds.lpData = IntPtr.Zero;

			var cdsBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(cds));
			Marshal.StructureToPtr(cds, cdsBuffer, false);

			Dispatcher.Invoke(delegate
			{
				WinApi.SendMessage(hWnd, WinApi.WM_COPYDATA, _hwndSource.Handle, cdsBuffer);
			});
		}

		private void HandlePacket(Packet packet)
		{
			if (packet.Op == Properties.Settings.Default.CommerceInfoRequestOp)
				InfoRequest(packet);
			else if (packet.Op == Properties.Settings.Default.CommerceInfoUpdateOp)
				InfoUpdate(packet);
			else if (packet.Op == Properties.Settings.Default.ProductListRequestOp)
				ProductsRequest(packet);
		}

		private void InfoRequest(Packet packet)
		{
			if (!packet.GetBool()) // Valid
				return;

			Erinn.Ducats = packet.GetLong();
			var transportFlags = packet.GetLong(); // Transport type flags (Bitfield)
			packet.GetInt(); // Unknown

			var townCount = packet.GetShort();

			for (var i = 0; i < townCount; i++)
			{
				var townId = packet.GetInt();
				var tradingExp = packet.GetInt();

				var town = Erinn.Posts.FirstOrDefault(t => t.Id == townId);

				if (town != null)
					town.MerchantLevel = GetMerchantLevel(tradingExp);
			}

			if (Properties.Settings.Default.SniffTransports)
			{
				foreach (var transport in Erinn.Transports)
				{
					transport.Enabled = (transportFlags & 1 << transport.Id) != 0;
				}
			}
		}

		private void InfoUpdate(Packet packet)
		{
			Erinn.Ducats = packet.GetLong();
			var transportFlags = packet.GetLong(); // Transport type flags (Bitfield)
			packet.GetInt(); // Unknown

			var townCount = packet.GetShort();

			for (var i = 0; i < townCount; i++)
			{
				var townId = packet.GetInt();
				var tradingExp = packet.GetInt();

				var town = Erinn.Posts.FirstOrDefault(t => t.Id == townId);

				if (town != null)
					town.MerchantLevel = GetMerchantLevel(tradingExp);
			}

			if (Properties.Settings.Default.SniffTransports)
			{
				foreach (var transport in Erinn.Transports)
				{
					transport.Enabled = (transportFlags & 1 << transport.Id) != 0;
				}
			}
		}

		private void ProductsRequest(Packet packet)
		{
			if (packet.GetString() != "GetProductList" || !packet.GetBool()) // Is valid
				return;

			var currentPostId = packet.GetInt();
			var post = Erinn.Posts.FirstOrDefault(p => p.Id == currentPostId);

			if (post == null)
				return;

			packet.GetInt(); // Unk
			packet.GetInt(); // Unk

			var itemCount = packet.GetInt();
			for (var i = 0; i < itemCount; i++) // List of item ids (duplicated later in the packet)
				packet.GetInt();

			itemCount = packet.GetInt();
			for (var i = 0; i < itemCount; i++)
			{
				var itemId = packet.GetInt();

				var item = post.Items.FirstOrDefault(a => a.Id == itemId);

				var normalizedCost = packet.GetInt();
				var stock = packet.GetInt();
				packet.GetBool(); // Limited time

				if (item != null)
				{
					item.Stock = stock;
					item.Price = (int)Math.Round(normalizedCost * post.MerchantLevel.Discount, MidpointRounding.AwayFromZero);
				}

				var townCount = packet.GetInt();
				for (var j = 0; j < townCount; j++)
				{
					var destId = packet.GetInt();
					packet.GetInt(); // Stock at destination
					var normalizedSellPrice = packet.GetInt();

					var destPost = Erinn.Posts.FirstOrDefault(p => p.Id == destId);

					if (destPost == null || item == null)
						continue;

					if (!post.Weights.ContainsKey(destPost.Id))
						continue;

					var weight = post.Weights[destPost.Id];

					var profit = item.Profits.FirstOrDefault(p => p.Destination == destPost);

					if (profit != null)
					{
						var sellPrice = (int)Math.Round(normalizedSellPrice * weight * item.MultiFactor + item.AddFactor,
							MidpointRounding.AwayFromZero);

						profit.Amount = sellPrice - item.Price;
					}
				}
			}

			PostSelect.SelectedItem = post;
			//CalculateTrades();
		}

		private MerchantLevel GetMerchantLevel(int tradingExp)
		{
			return Erinn.MerchantLevels.TakeWhile(level => tradingExp >= level.Exp).Last();
		}
	}
}

#endif