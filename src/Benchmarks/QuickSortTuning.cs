using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using Benchmarks.FiddleArea;

namespace Benchmarks
{
	public class QuickSortTuning
	{
		private double[] _data;
		private double[] _copy;

		[Params(100_000)]
		public int Size { get; set; }

		[Params(DataOrder.Random)]
		public DataOrder Order { get; set; }

		[GlobalSetup]
		public void Init()
		{
			_data = BuildArray(Size, Order);
			_copy = new double[_data.Length];
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

		[Benchmark(Baseline = true)]
		public void Default()
		{
			_data.CopyTo(_copy, 0);
			Array.Sort(_copy);
		}

		[Benchmark]
		public unsafe void Mine()
		{
			_data.CopyTo(_copy, 0);
			fixed (double* ptr0 = &_copy[0])
			{
				var indexer = new SpanIndexer<double>(ptr0);
				var length = _copy.Length;
				IntroSortAlgorithm<double, SpanIndexer<double>, SpanReference<double>, LessThan<double>>
					.IntroSort(
						indexer, 
						new LessThan<double>(), indexer.Ref0, indexer.Ref0.Add(length));
			}
		}
	}
}
