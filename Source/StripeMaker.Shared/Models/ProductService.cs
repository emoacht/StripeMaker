using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace StripeMaker.Models
{
	internal static class ProductService
	{
		private static readonly Assembly _assembly = Assembly.GetEntryAssembly();

		public static Version Version { get; } = _assembly.GetName().Version;

		public static string Title
		{
			get => _title ??= _assembly.GetAttribute<AssemblyTitleAttribute>().Title;
			set => _title = value;
		}
		private static string? _title;

		private static TAttribute GetAttribute<TAttribute>(this Assembly assembly) where TAttribute : Attribute =>
			(TAttribute)Attribute.GetCustomAttribute(assembly, typeof(TAttribute));

		public static Dictionary<string, Action> MenuActions { get; } = new()
		{
			{ "Site", () => Process.Start("https://github.com/emoacht/StripeMaker") },
			{ "License", () => Process.Start("https://github.com/emoacht/StripeMaker/blob/master/LICENSE.txt") },
		};
	}
}