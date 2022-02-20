using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace StripeMaker.Views
{
	internal class SystemMenuExtender
	{
		#region Win32

		[DllImport("User32.dll")]
		private static extern IntPtr GetSystemMenu(
			IntPtr hWnd,
			[MarshalAs(UnmanagedType.Bool)] bool bRevert);

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool InsertMenu(
			IntPtr hMenu,
			uint wPosition,
			uint wFlags,
			uint wIDNewItem,
			string lpNewItem);

		private const int MF_BYPOSITION = 0x00000400;
		private const uint MF_SEPARATOR = 0x00000800;
		private const int MF_DISABLED = 0x00000002;
		private const int MF_GRAYED = 0x00000001;

		#endregion

		public event EventHandler<string>? MenuSelected;

		private string[]? _menus;
		private HwndSource? _source;

		public void Register(Window window, string title, IReadOnlyCollection<string> menus)
		{
			var windowHandle = new WindowInteropHelper(window).Handle;
			if (windowHandle == IntPtr.Zero)
				return;

			_menus = menus?.ToArray() ?? throw new ArgumentNullException(nameof(menus));

			var menuHandle = GetSystemMenu(windowHandle, false);

			InsertMenu(menuHandle, 0, MF_BYPOSITION | MF_DISABLED, 0, title);

			for (int i = 0; i < _menus.Length; i++)
				InsertMenu(menuHandle, (uint)(i + 1), MF_BYPOSITION, (uint)i, _menus[i]);

			InsertMenu(menuHandle, (uint)(_menus.Length + 1), MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty);

			_source = PresentationSource.FromVisual(window) as HwndSource;
			_source?.AddHook(WndProc);
		}

		public void Unregister()
		{
			_source?.RemoveHook(WndProc);
		}

		private const int WM_SYSCOMMAND = 0x0112;

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			switch (msg)
			{
				case WM_SYSCOMMAND:
					int i = (Environment.Is64BitProcess ? (int)wParam.ToInt64() : wParam.ToInt32()) & 0xFFFF;

					if ((_menus is { Length: > 0 }) &&
						(0 <= i && i < _menus.Length))
					{
						MenuSelected?.Invoke(this, _menus[i]);
					}
					break;
			}
			return IntPtr.Zero;
		}
	}
}