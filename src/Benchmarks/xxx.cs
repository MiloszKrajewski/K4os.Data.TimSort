using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using K4os.Data.TimSort;

namespace Benchmarks
{
	public class ComparerVsIComparer
	{
		private int[] _data;

		// [Params(10, 100, 1000)]
		[Params(100_000)]
		public int Size { get; set; }

		[GlobalSetup]
		public void Setup()
		{
			_data = new int[Size];
			var r = new Random(0);
			for (var i = 0; i < _data.Length; i++) _data[i] = r.Next();
		}
		
		[Benchmark]
		public void TimSort_Native()
		{
			TimSort.Sort(_data, TimComparer.Int32);
		}
		
		[Benchmark]
		public void TimSort_Default()
		{
			TimSort.Sort(_data);
		}

		[Benchmark]
		public void TimSort_Comparer()
		{
			TimSort.Sort(_data, Comparer<int>.Default);
		}
		
		[Benchmark]
		public void TimSort_Comparison()
		{
			TimSort.Sort(_data, Comparer<int>.Default.Compare);
		}
		
		[Benchmark]
		public void TimSort_Lambda()
		{
			TimSort.Sort(_data, (a, b) => a - b);
		}
		
		[Benchmark]
		public void TimSort_IComparer()
		{
			TimSort.Sort(_data, (IComparer<int>)Comparer<int>.Default);
		}
	}
}
