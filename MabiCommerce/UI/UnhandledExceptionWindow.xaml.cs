using System;
using System.Collections.Generic;
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
	/// Interaction logic for UnhandledExceptionWindow.xaml
	/// </summary>
	public partial class UnhandledExceptionWindow : Window
	{
		public UnhandledExceptionWindow(Exception ex)
		{
			InitializeComponent();
			Details.Text = ex.ToString();
		}
	}
}
