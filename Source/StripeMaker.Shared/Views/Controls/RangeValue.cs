using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace StripeMaker.Views.Controls
{
	public static class RangeValue
	{
		public static bool GetIsWheelEnabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsWheelEnabledProperty);
		}
		public static void SetIsWheelEnabled(DependencyObject obj, bool value)
		{
			obj.SetValue(IsWheelEnabledProperty, value);
		}
		public static readonly DependencyProperty IsWheelEnabledProperty =
			DependencyProperty.RegisterAttached(
				"IsWheelEnabled",
				typeof(bool),
				typeof(RangeValue),
				new PropertyMetadata(
					false,
					(d, e) =>
					{
						if (d is not RangeBase range)
							return;

						if ((bool)e.NewValue)
							range.MouseWheel += OnMouseWheel;
						else
							range.MouseWheel -= OnMouseWheel;
					}));

		public static float WheelFactor { get; set; } = 0.8F;

		private static void OnMouseWheel(object sender, MouseWheelEventArgs e)
		{
			var range = (RangeBase)sender;
			range.Value -= (e.Delta / Mouse.MouseWheelDeltaForOneLine * WheelFactor);
		}
	}
}