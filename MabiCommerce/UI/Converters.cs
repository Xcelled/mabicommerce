using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using MabiCommerce.Domain.Trading;

namespace MabiCommerce.UI
{
	public abstract class ConverterBase : MarkupExtension, IValueConverter
	{
		public sealed override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}

		public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

		public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
	}

	public abstract class MultiConverterBase : MarkupExtension, IMultiValueConverter
	{
		public sealed override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}

		public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

		public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
	}

	public class IntToHexStringConverter : ConverterBase
	{
		public IntToHexStringConverter()
		{

		}

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return string.Format("0x{0:X}", (int)value);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var str = value as string;

			if (string.IsNullOrEmpty(str))
				throw new ArgumentException();

			if (str.StartsWith("0x"))
				str = str.Substring(2);

			return int.Parse(str, NumberStyles.HexNumber);
		}
	}

	public class NotConverter : ConverterBase
	{
		public NotConverter()
		{
			
		}

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return !(bool)value;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class HandleIsZeroConverter : ConverterBase
	{
		public HandleIsZeroConverter()
		{
			
		}

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((IntPtr)value) == IntPtr.Zero;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class RelativeToAbsolutePathConverter : ConverterBase
	{
		public RelativeToAbsolutePathConverter()
		{

		}

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var relative = value as string;
			return relative == null ? null : System.IO.Path.GetFullPath(relative);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class IntToPolarIntConverter : ConverterBase
	{
		public IntToPolarIntConverter()
		{
			
		}

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var i = (int)value;

			if (i < 0)
				return -1;
			if (i > 0)
				return 1;

			return 0;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class Tautology : ConverterBase
	{
		public Tautology()
		{
			
		}

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return true;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class HasNoProfitFlagConverter : ConverterBase
	{
		public HasNoProfitFlagConverter()
		{
			
		}

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((TradeFlags)value).HasFlag(TradeFlags.NoProfit);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class HasChokePointFlagConverter : ConverterBase
	{
		public HasChokePointFlagConverter()
		{
			
		}

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((TradeFlags)value).HasFlag(TradeFlags.ChokePoint);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class PercentageOfConverter : MultiConverterBase
	{
		public PercentageOfConverter()
		{
			
		}

		public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var percent = (double)values[0];
			var of = (double)values[1];

			return percent * of;
		}

		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class DivisionConverter : MultiConverterBase
	{
		public DivisionConverter()
		{
			
		}

		public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var num = (double)values[0];
			var den = (double)values[1];

			return num / den;
		}

		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class RatioToPercentOfConverter : MultiConverterBase
	{
		public RatioToPercentOfConverter()
		{
			
		}

		public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (!values.All(o => o is double))
				return 0.0;

			var num = (double)values[0];
			var den = (double)values[1];
			var of = (double)values[2];

			return (num / den) * of;
		}

		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class ErrorsToStringConverter : ConverterBase
	{
		public ErrorsToStringConverter()
		{

		}

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var errors = value as ReadOnlyCollection<ValidationError>;

			return errors == null ? string.Empty : string.Join(Environment.NewLine, errors.Where(e => e.ErrorContent != null).Select(e => e.ErrorContent.ToString()));
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
