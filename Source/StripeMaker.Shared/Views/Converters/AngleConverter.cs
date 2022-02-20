using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StripeMaker.Views.Converters
{
	public class AngleConverter : IMultiValueConverter
	{
		/// <summary>
		/// Converts x and y coodinates to the angle from horizontal line in degree.
		/// </summary>
		/// <param name="values">
		/// <para>1st: X coodinate</para>
		/// <para>2nd: Y coodinate</para>
		/// <para>3rd: Direction (If true, clockwise. If false, anticlockwise.)</para>
		/// </param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns>Angle in degree</returns>
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