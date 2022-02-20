using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StripeMaker.Views.Converters
{
	public class AngleConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if ((values is { Length: >= 2 }) &&
				(values[0] is double x) &&
				(values[1] is double y))
			{
				var angle = GetAngle(x, y);

				if ((values is { Length: 3 }) &&
					(values[2] is false))
				{
					return -angle;
				}
				return angle;
			}
			return DependencyProperty.UnsetValue;

			static double GetAngle(double x, double y) => Math.Atan2(y, x) * 180D / Math.PI;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}