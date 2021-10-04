using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using K4os.Data.TimSort;

namespace Benchmarks
{
	public class VsBuiltInNative
	{
		private double[] _data;
		private double[] _copy;

		// [Params(10, 100, 1_000, 1_000_000)]
		[Params(10_000_000)]
		public int Size { get; set; }

		// [Params(DataOrder.Ascending, DataOrder.Random, DataOrder.Descending)]
		[Params(DataOrder.Random)]
		public DataOrder Order { get; set; }

		[GlobalSetup]
		public void Init()
		{
			_data = BuildArray(Size, Order);
			_copy = new double[_data.Length];
		}

		[IterationSetup]
		public void Setup()
		{
			_data.CopyTo(_copy, 0);
		}

		private static double[] BuildArray(int size, DataOrder order)
		{
			var a = new double[size];
			var r = new Random(0);

			for (var i = 0; i < size; i++) a[i] = r.NextDouble() * size;
			if (order != DataOrder.Random) Array.Sort(a);
			if (order == DataOrder.Descending) Array.Reverse(a);

			return a;
		}

		// [Benchmark]
		// public void UseTimSort()
		// {
		// 	_data.CopyTo(_copy, 0);
		// 	TimSort.Sort(_copy, TimComparer.Double);
		// }

		// [Benchmark]
		// public void Overhead() { _data.CopyTo(_copy, 0); }

		[Benchmark]
		public void UseBuiltIn()
		{
			Array.Sort(_copy);
		}
		
		[Benchmark]
		public void UseTimSort()
		{
			TimSort.Sort(_copy, TimComparer.Double);
		}

		[Benchmark]
		public void UseIntroSort()
		{
			IntroSort.Sort(_copy, TimComparer.Double);
		}
	}
}
