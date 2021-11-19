using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using Benchmarks.FiddleArea;
using K4os.Data.TimSort;
using K4os.Data.TimSort.Internals;

namespace Benchmarks
{
	public class NewIndexerAndReference
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
			NewIndexerAndReferenceSorter.DoubleSorter1(_copy.AsSpan());
		}

		[Benchmark]
		public void UsingPointers()
		{
			_data.CopyTo(_copy, 0);
			NewIndexerAndReferenceSorter.DoubleSorter2(_copy.AsSpan());
		}

		[Benchmark]
		public void UsingReference()
		{
			_data.CopyTo(_copy, 0);
			NewIndexerAndReferenceSorter<double, LessThan<double>>
				.DoubleSorter3(_copy.AsSpan(), new LessThan<double>());
		}
		
		[Benchmark]
		public void UsingReferenceDedicated()
		{
			_data.CopyTo(_copy, 0);
			NewIndexerAndReferenceSorter<double, LessThanDouble>
				.DoubleSorter3(_copy.AsSpan(), new LessThanDouble());
		}

	}
}
