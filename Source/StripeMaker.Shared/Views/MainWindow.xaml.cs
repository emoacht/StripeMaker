using System;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using StripeMaker.Models;

namespace StripeMaker.Views
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private readonly SystemMenuExtender _extender = new();

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			_extender.Register(this, $"{ProductService.Title} {ProductService.Version}",
				ProductService.MenuActions.Keys);

			_extender.MenuSelected += (_, key) =>
			{
				if (ProductService.MenuActions.TryGetValue(key, out var action))
					action.Invoke();
			};
		}

		/// <summary>
		/// Whether direction of stripes is backward (or forward)
		/// </summary>
		public bool IsBackward
		{
			get { return (bool)GetValue(IsBackwardProperty); }
			set { SetValue(IsBackwardProperty, value); }
		}
		public static readonly DependencyProperty IsBackwardProperty =
			DependencyProperty.Register(
				"IsBackward",
				typeof(bool),
				typeof(MainWindow),
				new PropertyMetadata(true));

		private async void CopyButton_Click(object sender, RoutedEventArgs e)
		{
			await CopyAsync();
		}

		internal async Task CopyAsync()
		{
			var pathDataString = this.StripePath.Data.ToString();
			if (string.IsNullOrEmpty(pathDataString))
				return;

			try
			{
				Clipboard.Clear();

				await Task.Delay(TimeSpan.FromSeconds(0.1));

				Clipboard.SetText(InsertSpaces(pathDataString));

				SystemSounds.Asterisk.Play();
			}
			catch
			{
				SystemSounds.Exclamation.Play();
			}

			static string InsertSpaces(string source)
			{
				var buffer = new StringBuilder();

				for (int i = 0; i < source.Length; i++)
				{
					buffer.Append(source[i]);

					if (i < source.Length - 1)
					{
						if ((char.IsLetter(source[i]) && char.IsDigit(source[i + 1])) ||
							(char.IsDigit(source[i]) && char.IsLetter(source[i + 1])))
						{
							buffer.Append(' ');
						}
					}
				}
				return buffer.ToString();
			}
		}
	}
}