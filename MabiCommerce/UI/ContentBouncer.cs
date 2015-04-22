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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MabiCommerce.UI
{
	/// <summary>
	/// Interaction logic for ContentBouncer.xaml
	/// </summary>
	public partial class ContentBouncer : ContentControl
	{
		public double ContentWidth
		{
			get { return (double)GetValue(ContentWidthProperty); }
			set { SetValue(ContentWidthProperty, value);
				System.Diagnostics.Debug.WriteLine(value);
			}
		}

		// Using a DependencyProperty as the backing store for ContentWidth.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ContentWidthProperty =
			DependencyProperty.Register("ContentWidth", typeof(double), typeof(ContentBouncer), new PropertyMetadata(double.NaN));



		public double PixelsPerSecond
		{
			get { return (double)GetValue(PixelsPerSecondProperty); }
			set { SetValue(PixelsPerSecondProperty, value); }
		}

		// Using a DependencyProperty as the backing store for PixelsPerSecond.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty PixelsPerSecondProperty =
			DependencyProperty.Register("PixelsPerSecond", typeof(double), typeof(ContentBouncer), new PropertyMetadata(15.0));

		private readonly ThicknessAnimation _aniSlider;

		public ContentBouncer()
		{
			_aniSlider = new ThicknessAnimation
			{
				AutoReverse = true,
				FillBehavior = FillBehavior.Stop,
				RepeatBehavior = RepeatBehavior.Forever,
				BeginTime = TimeSpan.FromSeconds(.5),
			};

			Loaded += ContentBouncer_Loaded;
		}

		void ContentBouncer_Loaded(object sender, RoutedEventArgs e)
		{
			var contentPresenter = (Template.FindName("DisplayContent", this) as ContentPresenter);

			contentPresenter.MouseEnter += StartAnim;
			contentPresenter.MouseLeave += StopAnim;
		}

		private void StartAnim(object sender, RoutedEventArgs e)
		{
			var contentPresenter = (Template.FindName("DisplayContent", this) as ContentPresenter);

			var overflow = (double.IsNaN(ContentWidth) ? (Template.FindName("Bounds", this) as FrameworkElement).ActualWidth : ContentWidth) -
				(contentPresenter.Content as FrameworkElement).ActualWidth;

			if (overflow < 0)
			{
				_aniSlider.Duration = TimeSpan.FromSeconds(Math.Abs(overflow / PixelsPerSecond));
				_aniSlider.From = contentPresenter.Margin;
				_aniSlider.To = new Thickness(overflow, contentPresenter.Margin.Top, contentPresenter.Margin.Right,
					contentPresenter.Margin.Bottom);
				contentPresenter.BeginAnimation(ContentPresenter.MarginProperty, _aniSlider);
			}
		}

		private void StopAnim(object sender, MouseEventArgs e)
		{
			(Template.FindName("DisplayContent", this) as ContentPresenter).BeginAnimation(ContentPresenter.MarginProperty, null);
		}
	}
}
