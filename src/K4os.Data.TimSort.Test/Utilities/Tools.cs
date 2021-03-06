using System;

namespace K4os.Data.TimSort.Test.Utilities
{
	public class Tools
	{
		public static double[] BuildArray(int seed, int length)
		{
			var random = new Random(seed);
			var array = new double[length];
			for (var i = 0; i < length; i++) array[i] = random.NextDouble();
			return array;
		}

		public static void VerifyArray(double[] array)
		{
			for (var i = 1; i < array.Length; i++)
			{
				if (array[i] >= array[i - 1]) continue;

				throw new ArgumentException(@"Array in not sorted @ {i}");
			}
		}
	}
}
