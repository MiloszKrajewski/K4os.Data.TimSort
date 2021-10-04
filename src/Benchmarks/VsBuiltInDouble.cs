using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using K4os.Data.TimSort;

namespace Benchmarks
{
	public enum DataOrder
	{
		Random,
		Ascending,
		Descending,
	}

	public class VsBuiltIn
	{
		private double[] _data;
		private double[] _copy;
		private readonly Comparison<double> _comparison = Comparer<double>.Default.Compare;

		// [Params(10, 100, 1_000, 1_000_000)]
		[Params(1_000_000)]
		public int Size { get; set; }

		[Params(DataOrder.Ascending, DataOrder.Random, DataOrder.Descending)]
		public DataOrder Order { get; set; }

		[GlobalSetup]
		public void Init()
		{
			_data = BuildArray(Size, Order);
			_copy = new double[_data.Length];
		}

		// [IterationSetup]
		// public void Setup() { _data.CopyTo(_copy, 0); }

		private static double[] BuildArray(int size, DataOrder order)
		{
			var a = new double[size];
			var r = new Random(0);

			for (var i = 0; i < size; i++) a[i] = r.NextDouble() * size;
			if (order != DataOrder.Random) Array.Sort(a);
			if (order == DataOrder.Descending) Array.Reverse(a);

			return a;
		}

		[Benchmark]
		public void TimSort_Default()
		{
			_data.CopyTo(_copy, 0);
			TimSort.Sort(_copy, _comparison);
		}

		[Benchmark]
		public void Intro_Default()
		{
			_data.CopyTo(_copy, 0);
			IntroSort.Sort(_copy, _comparison);
		}

		[Benchmark]
		public void BuiltIn_Default()
		{
			_data.CopyTo(_copy, 0);
			Array.Sort(_copy, _comparison);
		}
	}
}
