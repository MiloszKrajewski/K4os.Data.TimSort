using System;
using BenchmarkDotNet.Attributes;
using K4os.Data.TimSort;

namespace Benchmarks
{
	public class TimSortVsBuiltIn
	{
		private int[] _data;

		// [Params(10, 100, 1000)]
		[Params(1000)]
		public int Size { get; set; }

		[GlobalSetup]
		public void Setup()
		{
			_data = new int[Size];
			var r = new Random(0);
			for (var i = 0; i < _data.Length; i++) _data[i] = r.Next();
		}

		[Benchmark]
		public void TimSort_Inline()
		{
			TimSort.Sort(_data, TimComparer.Int32);
		}
		
		[Benchmark]
		public void TimSort_Default()
		{
			TimSort.Sort(_data, TimComparer.Default<int>());
		}
		
		[Benchmark]
		public void TimSort_Callback()
		{
			TimSort.Sort(_data, Compare);
		}
		
		private static int Compare(int a, int b) => a - b;
		
		[Benchmark]
		public void BuiltIn_Default()
		{
			Array.Sort(_data);
		}
		
		[Benchmark]
		public void BuiltIn_Callback()
		{
			Array.Sort(_data, Compare);
		}

	}
}
