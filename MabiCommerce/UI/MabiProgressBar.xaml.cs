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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MabiCommerce.UI
{
	/// <summary>
	/// Interaction logic for MabiProgressBar.xaml
	/// </summary>
	public partial class MabiProgressBar : UserControl
	{
		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(double), typeof(MabiProgressBar), new PropertyMetadata(0.0));
		

		public double Maximum
		{
			get { return (double)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(double), typeof(MabiProgressBar), new PropertyMetadata(100.0));

		
		public Brush BarBorderStroke
		{
			get { return (Brush)GetValue(BarBorderStrokeProperty); }
			set { SetValue(BarBorderStrokeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for BarBorderStroke.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BarBorderStrokeProperty =
			DependencyProperty.Register("BarBorderStroke", typeof(Brush), typeof(MabiProgressBar), new PropertyMetadata(null));

		
		public Brush BarForeground
		{
			get { return (Brush)GetValue(BarForegroundProperty); }
			set { SetValue(BarForegroundProperty, value); }
		}

		// Using a DependencyProperty as the backing store for BarForeground.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BarForegroundProperty =
			DependencyProperty.Register("BarForeground", typeof(Brush), typeof(MabiProgressBar), new PropertyMetadata(null));

		
		public Brush BarBackground
		{
			get { return (Brush)GetValue(BarBackgroundProperty); }
			set { SetValue(BarBackgroundProperty, value); }
		}

		// Using a DependencyProperty as the backing store for BarBackground.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BarBackgroundProperty =
			DependencyProperty.Register("BarBackground", typeof(Brush), typeof(MabiProgressBar), new PropertyMetadata(null));

		
		public MabiProgressBar()
		{
			InitializeComponent();
		}
	}
}
