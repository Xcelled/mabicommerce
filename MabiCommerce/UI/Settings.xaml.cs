using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

namespace MabiCommerce.UI
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : Window
	{
		Properties.Settings AppSettings { get { return Properties.Settings.Default; } }

		public Version Version { get { return typeof(Settings).Assembly.GetName().Version; } }

		public int CommerceInfoRequestOp { get { return AppSettings.CommerceInfoRequestOp; } set { AppSettings.CommerceInfoRequestOp = value;} }
		public int CommerceInfoUpdateOp { get { return AppSettings.CommerceInfoUpdateOp; } set { AppSettings.CommerceInfoUpdateOp = value;} }
		public int ProductListRequestOp { get { return AppSettings.ProductListRequestOp; } set { AppSettings.ProductListRequestOp = value;} }
		public bool SniffTransports { get { return AppSettings.SniffTransports; } set { AppSettings.SniffTransports = value; } }
		public bool UpdateCheck { get { return AppSettings.UpdateCheck; } set { AppSettings.UpdateCheck = value; } }

		public Settings()
		{
			InitializeComponent();
		}

		private void SaveBtn_Click(object sender, RoutedEventArgs e)
		{
			AppSettings.Save();
			Close();
		}

		private void CancelBtn_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			AppSettings.Reload();
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}
	}

	public class OpCodeValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			var str = value as string;

			if (string.IsNullOrWhiteSpace(str))
				return new ValidationResult(false, "Please enter a number.");

			if (str.StartsWith("0x"))
				str = str.Substring(2);

			try
			{
				int.Parse(str, NumberStyles.HexNumber, cultureInfo);
			}
			catch (Exception ex)
			{
				return new ValidationResult(false, ex.Message);
			}

			return new ValidationResult(true, null);
		}
	}

}
