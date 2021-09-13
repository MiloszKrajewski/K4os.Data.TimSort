using System;
using Xunit;

namespace K4os.Data.TimSort.Test
{
	public class StressTests
	{
		[Theory]
		[InlineData(0, 0)]
		[InlineData(0, 1)]
		[InlineData(0, 2)]
		[InlineData(0, 10)]
		[InlineData(0, 100)]
		[InlineData(0, 1_000)]
		[InlineData(0, 10_000)]
		[InlineData(0, 100_000)]
		#if !DEBUG
		[InlineData(0, 1_000_000)]
		[InlineData(0, 10_000_000)]
		#endif
		public void RandomData(int seed, int length)
		{
			var array = Tools.BuildArray(seed, length);
			TimSort.Sort(array, TimComparer.Double);
			Tools.VerifyArray(array);
		}
		
		[Theory]
		[InlineData(0, 0)]
		[InlineData(0, 1)]
		[InlineData(0, 2)]
		[InlineData(0, 10)]
		[InlineData(0, 100)]
		[InlineData(0, 1_000)]
		[InlineData(0, 10_000)]
		[InlineData(0, 100_000)]
		#if !DEBUG
		[InlineData(0, 1_000_000)]
		[InlineData(0, 10_000_000)]
		#endif
		public void SortedData(int seed, int length)
		{
			var array = Tools.BuildSortedArray(seed, length);
			TimSort.Sort(array, TimComparer.Double);
			Tools.VerifyArray(array);
		}
		
		[Theory]
		[InlineData(0, 0)]
		[InlineData(0, 1)]
		[InlineData(0, 2)]
		[InlineData(0, 10)]
		[InlineData(0, 100)]
		[InlineData(0, 1_000)]
		[InlineData(0, 10_000)]
		[InlineData(0, 100_000)]
		#if !DEBUG
		[InlineData(0, 1_000_000)]
		[InlineData(0, 10_000_000)]
		#endif
		public void ReversedData(int seed, int length)
		{
			var array = Tools.BuildSortedArray(seed, length);
			Array.Reverse(array);
			TimSort.Sort(array, TimComparer.Double);
			Tools.VerifyArray(array);
		}
	}
}