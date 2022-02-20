using System;
using System.Windows;
using System.Windows.Media;

namespace StripeMaker.Views.Converters
{
	public static class PathPoint
	{
		private static double Round(object value) => Math.Round((double)value * 100D, MidpointRounding.AwayFromZero) / 100D;

		public static double GetX(DependencyObject obj)
		{
			return (double)obj.GetValue(XProperty);
		}
		public static void SetX(DependencyObject obj, double value)
		{
			obj.SetValue(XProperty, value);
		}
		public static readonly DependencyProperty XProperty =
			DependencyProperty.RegisterAttached(
				"X",
				typeof(double),
				typeof(PathPoint),
				new PropertyMetadata(
					0D,
					(d, e) =>
					{
						switch (d)
						{
							case PathFigure figure:
								figure.StartPoint = new Point(x: Round(e.NewValue), y: figure.StartPoint.Y);
								break;

							case LineSegment segment:
								segment.Point = new Point(x: Round(e.NewValue), y: segment.Point.Y);
								break;
						}
					}));

		public static double GetY(DependencyObject obj)
		{
			return (double)obj.GetValue(YProperty);
		}
		public static void SetY(DependencyObject obj, double value)
		{
			obj.SetValue(YProperty, value);
		}
		public static readonly DependencyProperty YProperty =
			DependencyProperty.RegisterAttached(
				"Y",
				typeof(double),
				typeof(PathPoint),
				new PropertyMetadata(
					0D,
					(d, e) =>
					{
						switch (d)
						{
							case PathFigure figure:
								figure.StartPoint = new Point(x: figure.StartPoint.X, y: Round(e.NewValue));
								break;

							case LineSegment segment:
								segment.Point = new Point(x: segment.Point.X, y: Round(e.NewValue));
								break;
						}
					}));
	}
}