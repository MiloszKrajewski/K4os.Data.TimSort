using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using K4os.Data.TimSort;

namespace Benchmarks
{
	public class ComparerBenchmarks
	{
		private int[] _data;
		private int[] _copy;

		// [Params(10, 100, 1000)]
		[Params(1_000_000)]
		public int Size { get; set; }

		[GlobalSetup]
		public void Setup()
		{
			var l = Size;
			var a = new int[l];
			var r = new Random(0);
			for (var i = 0; i < l; i++) a[i] = r.Next();
			_data = a;
		}
		
		[IterationSetup]
		public void Clone()
		{
			_copy = _data.ToArray();
		}
		
		[Benchmark]
		public void TimSort_Native()
		{
			TimSort.Sort(_copy, TimComparer.Int32);
		}
		
		[Benchmark]
		public void TimSort_Default()
		{
			TimSort.Sort(_copy);
		}

		[Benchmark]
		public void TimSort_Comparer()
		{
			TimSort.Sort(_copy, Comparer<int>.Default);
		}
		
		[Benchmark]
		public void TimSort_Comparison()
		{
			TimSort.Sort(_copy, Comparer<int>.Default.Compare);
		}
		
		[Benchmark]
		public void TimSort_Lambda()
		{
			TimSort.Sort(_copy, (a, b) => a - b);
		}
		
		[Benchmark]
		public void TimSort_IComparer()
		{
			TimSort.Sort(_copy, (IComparer<int>)Comparer<int>.Default);
		}
	}
}
