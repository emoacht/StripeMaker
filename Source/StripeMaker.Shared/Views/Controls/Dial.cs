using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StripeMaker.Views.Controls
{
	[TemplatePart(Name = "PART_Dial", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_Rotation", Type = typeof(RotateTransform))]
	public class Dial : ContentControl
	{
		#region Property

		public double Angle
		{
			get { return (double)GetValue(AngleProperty); }
			set { SetValue(AngleProperty, value); }
		}
		public static readonly DependencyProperty AngleProperty =
			DependencyProperty.Register(
				"Angle",
				typeof(double),
				typeof(Dial),
				new PropertyMetadata(
					0D,
					(d, e) =>
					{
						var instance = (Dial)d;
						if (instance._rotation is RotateTransform rotation)
							rotation.Angle = (double)e.NewValue;

						instance.OnAngleChanged((double)e.OldValue, (double)e.NewValue);
					},
					(d, baseValue) =>
					{
						var instance = (Dial)d;
						var (minimum, maximum) = instance.IsBackward ? (0D, 90D) : (-90D, 0D);
						return Math.Min(maximum, Math.Max(minimum, (double)baseValue));
					}));

		public double TargetWidth
		{
			get { return (double)GetValue(TargetWidthProperty); }
			set { SetValue(TargetWidthProperty, value); }
		}
		public static readonly DependencyProperty TargetWidthProperty =
			DependencyProperty.Register(
				"TargetWidth",
				typeof(double),
				typeof(Dial),
				new PropertyMetadata(0D, (d, _) => ((Dial)d).OnTargetChanged()));

		public double TargetHeight
		{
			get { return (double)GetValue(TargetHeightProperty); }
			set { SetValue(TargetHeightProperty, value); }
		}
		public static readonly DependencyProperty TargetHeightProperty =
			DependencyProperty.Register(
				"TargetHeight",
				typeof(double),
				typeof(Dial),
				new PropertyMetadata(0D, (d, _) => ((Dial)d).OnTargetChanged()));

		public bool IsBackward
		{
			get { return (bool)GetValue(IsBackwardProperty); }
			set { SetValue(IsBackwardProperty, value); }
		}
		public static readonly DependencyProperty IsBackwardProperty =
			DependencyProperty.Register(
				"IsBackward",
				typeof(bool),
				typeof(Dial),
				new PropertyMetadata(true, (d, _) => ((Dial)d).OnTargetChanged()));

		private bool _isAngleChanging;
		private bool _isTargetChanging;

		private void OnAngleChanged(double oldAngle, double newAngle)
		{
			if (_isTargetChanging)
				return;

			try
			{
				_isAngleChanging = true;

				const double unit = 0.1;
				var isWider = Math.Abs(oldAngle) > Math.Abs(newAngle);
				var gap0 = double.MaxValue;

				var (width0, height0) = (TargetWidth, TargetHeight);
				var (width1, height1) = (width0, height0);

				while (true)
				{
					var gap1 = Math.Abs(Math.Abs(Angle) - GetAngleFromRectangle(width1, height1));
					if (gap0 <= gap1)
						break;

					gap0 = gap1;

					(width0, height0) = (width1, height1);
					(width1, height1) = isWider
						? (width1 + unit, height1 - unit)
						: (width1 - unit, height1 + unit);
				}

				(TargetWidth, TargetHeight) = (width0, height0);
			}
			finally
			{
				_isAngleChanging = false;
			}
		}

		private void OnTargetChanged()
		{
			if (_isAngleChanging)
				return;

			try
			{
				_isTargetChanging = true;

				Angle = GetAngleFromRectangle(TargetWidth, TargetHeight) * (IsBackward ? 1 : -1);
			}
			finally
			{
				_isTargetChanging = false;
			}
		}

		private static double GetAngleFromRectangle(double x, double y) => Math.Atan2(y, x) * 180D / Math.PI;

		#endregion

		private FrameworkElement? _dial;
		private RotateTransform? _rotation;

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_dial = this.GetTemplateChild("PART_Dial") as FrameworkElement;
			_rotation = this.GetTemplateChild("PART_Rotation") as RotateTransform;

			if (_dial is not null)
			{
				_dial.MouseDown += OnDialMouseDown;
				_dial.MouseMove += OnDialMouseMove;
				_dial.MouseUp += OnDialMouseLeave;
				_dial.MouseLeave += OnDialMouseLeave;
				_dial.MouseWheel += OnDialMouseWheel;
			}

			if (_rotation is not null)
				_rotation.Angle = Angle;
		}

		private bool _isMoving;

		private void OnDialMouseDown(object sender, MouseButtonEventArgs e)
		{
			Angle = GetAngleFromMouseEvent((FrameworkElement)sender, e);

			_isMoving = true;
		}

		private void OnDialMouseMove(object sender, MouseEventArgs e)
		{
			if (!_isMoving)
				return;

			Angle = GetAngleFromMouseEvent((FrameworkElement)sender, e);
		}

		private void OnDialMouseLeave(object sender, MouseEventArgs e)
		{
			_isMoving = false;
		}

		private void OnDialMouseWheel(object sender, MouseWheelEventArgs e)
		{
			Angle -= e.Delta / 120D;
		}

		private static double GetAngleFromMouseEvent(FrameworkElement element, MouseEventArgs e)
		{
			var v1 = new Vector(0, -1);
			var v2 = GetVector(element, e);
			return Vector.AngleBetween(v1, v2);

			static Vector GetVector(FrameworkElement element, MouseEventArgs e)
			{
				var center = new Point(element.ActualWidth / 2D, element.ActualHeight / 2D);
				var position = e.GetPosition(element);
				return new Vector(position.X - center.X, position.Y - center.Y);
			}
		}
	}
}