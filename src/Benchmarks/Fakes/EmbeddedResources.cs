#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Benchmarks.Fakes
{
	public class EmbeddedResources
	{
		public static T Load<T>(
			Type hookType, string resourceName, Func<Stream, T> loader)
		{
			using var stream =
				hookType.Assembly.GetManifestResourceStream(hookType, resourceName);
			if (stream is null)
				throw new IOException($"Embedded stream {resourceName} could not be opened");

			return loader(stream);
		}

		public static Stream LoadStream(Type hookType, string resourceName) =>
			Load(hookType, resourceName, LoadStream);

		public static XDocument LoadXml(Type hookType, string resourceName) =>
			Load(hookType, resourceName, LoadXml);

		public static string[] LoadStrings(Type hookType, string resourceName) =>
			Load(hookType, resourceName, LoadStrings);

		public string[] LoadStrings(string resourceName) =>
			LoadStrings(GetType(), resourceName);

		public Stream LoadStream(string resourceName) =>
			LoadStream(GetType(), resourceName);

		public XDocument LoadXml(string resourceName) =>
			LoadXml(GetType(), resourceName);
	
		private static Stream LoadStream(Stream stream)
		{
			var result = new MemoryStream();
			stream.CopyTo(result);
			result.Position = 0;
			return result;
		}

		private static string[] LoadStrings(Stream stream)
		{
			using var reader = new StreamReader(stream);
			var buffer = new List<string>();
			string? line;
			while ((line = reader.ReadLine()) != null) buffer.Add(line);
			return buffer.ToArray();
		}

		private static XDocument LoadXml(Stream stream) => 
			XDocument.Load(stream);
	}
}
