using System;
using System.Linq;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using K4os.Data.TimSort.Comparers;

namespace Benchmarks
{
	public class DedicatedVsGenericLessThan
	{
		private double[] _data;
		private double _threshold;

		[Params(100_000)]
		public int Size { get; set; }

		public int Result { get; set; }

		[Params(DataOrder.Random)]
		public DataOrder Order { get; set; }

		[GlobalSetup]
		public void Init()
		{
			_data = BuildArray(Size, Order);
			_threshold = Size * 0.5;

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
		public void Manual()
		{
			var counter = 0;
			var length = _data.Length;
			for (var i = 0; i < length; i++)
				if (_data[i] < _threshold)
					counter++;
			Result = counter;
		}
		
		[Benchmark]
		public void Dedicated()
		{
			var counter = 0;
			var length = _data.Length;
			var comparer = new LessThanDouble();
			for (var i = 0; i < length; i++)
				if (comparer.Lt(_data[i], _threshold))
					counter++;
			Result = counter;
		}

		[Benchmark]
		public void Generic()
		{
			var counter = 0;
			var length = _data.Length;
			var comparer = new ComparableLessThan<double>();
			for (var i = 0; i < length; i++)
				if (comparer.Lt(_data[i], _threshold))
					counter++;
			Result = counter;
		}
	}

	public struct LessThanDouble: ILessThan<double>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(double a, double b) => a < b;
	}
}
