using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StripeMaker.Views.Converters
{
	public class RectConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if ((values is { Length: 2 }) &&
				(values[0] is double actualWidth and >= 0) &&
				(values[1] is double actualHeight and >= 0))
			{
				return new Rect(0, 0, actualWidth, actualHeight);
			}
			return DependencyProperty.UnsetValue;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}