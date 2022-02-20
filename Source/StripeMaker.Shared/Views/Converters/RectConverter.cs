using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StripeMaker.Views.Converters
{
	public class RectConverter : IMultiValueConverter
	{
		/// <summary>
		/// Converts width and height to Rect.
		/// </summary>
		/// <param name="values">
		/// <para>1st: Width</para>
		/// <para>2nd: Height</para>
		/// </param> 
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns>Rect</returns>
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