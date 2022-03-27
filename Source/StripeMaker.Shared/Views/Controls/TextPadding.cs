using System.Windows;
using System.Windows.Controls;

namespace StripeMaker.Views.Controls
{
	public static class TextPadding
	{
		public static double GetValue(DependencyObject obj)
		{
			return (double)obj.GetValue(ValueProperty);
		}
		public static void SetValue(DependencyObject obj, double value)
		{
			obj.SetValue(ValueProperty, value);
		}
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.RegisterAttached(
				"Value",
				typeof(double),
				typeof(TextPadding),
				new PropertyMetadata(
					double.MinValue,
					(d, e) =>
					{
						var padding = (double)e.NewValue switch
						{
							>= 0 => new Thickness(4, 0, 0, 0),
							_ => new Thickness(0),
						};

						if (d is TextBlock textBlock)
							textBlock.Padding = padding;

						if (d is TextBox textBox)
							textBox.Padding = padding;
					}));
	}
}