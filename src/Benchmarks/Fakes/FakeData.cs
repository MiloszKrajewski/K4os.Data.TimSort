using System;
using System.Linq;

namespace Benchmarks.Fakes
{
	public class FakeData
	{
		private static readonly object Mutex = new();

		private static bool _loaded;
		private static string[] _firstNames;
		private static string[] _lastNames;

		public static void Load(bool force = false)
		{
			lock (Mutex)
			{
				if (_loaded && !force) return;

				_firstNames = LoadStrings("firstnames.txt");
				_lastNames = LoadStrings("lastnames.txt");

				_loaded = true;
			}
		}

		public static string FirstName(int seed) => _firstNames[seed % _firstNames.Length];
		public static string LastName(int seed) => _lastNames[seed % _lastNames.Length];
		public static DateTime DateOfBirth(Random random) => 
			new DateTime(2000 - random.Next(20), 1, 1).AddDays(random.Next(365));

		private static string[] LoadStrings(string fileName) =>
			EmbeddedResources
				.LoadStrings(typeof(FakeData), fileName)
				.Where(s => !string.IsNullOrWhiteSpace(s))
				.ToArray();
	}
}
