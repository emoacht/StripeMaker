using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StripeMaker.Views.Converters
{
	[ValueConversion(typeof(double), typeof(double))]
	public class PathConverter : IValueConverter
	{
		public double BaseLength { get; set; } = 100D;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is not double sourceLength)
				return DependencyProperty.UnsetValue;

			return BaseLength - sourceLength;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}