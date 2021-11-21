using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.Tuning
{
	public class TuningBase
	{
		private double[] Data;
		private double[] Copy;
		private double[] Sorted;

		[Params(100_000)]
		public int Size { get; set; }

		[Params(DataOrder.Random)]
		public DataOrder Order { get; set; }

		[GlobalSetup]
		public void Init()
		{
			Data = BuildArray(Size, Order);
			Sorted = Data.ToArray();
			Array.Sort(Sorted);
			Copy = new double[Data.Length];
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

		public double[] GetData()
		{
			Data.CopyTo(Copy, 0);
			return Copy;
		}
		
	}
}
