using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StripeMaker.Views.Converters
{
	public class LengthConverter : IMultiValueConverter
	{
		public double BaseLength { get; set; } = 100D;

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if ((values is { Length: >= 2 }) &&
				(values[0] is double offset) &&
				(values[1] is double actualLength))
			{
				if ((values is { Length: 3 }) &&
					(values[2] is true))
				{
					offset = BaseLength - offset;
				}

				return offset * actualLength / BaseLength;
			}
			return DependencyProperty.UnsetValue;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}