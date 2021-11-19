// using System;
// using BenchmarkDotNet.Attributes;
// using K4os.Data.TimSort;
// using K4os.Data.TimSort.Internals;
//
// namespace Benchmarks
// {
// 	public class IndexedVsLinear
// 	{
// 		private double[] _data;
// 		private double[] _copy;
//
// 		[Params(10_000_000)]
// 		public int Size { get; set; }
//
// 		[Params(DataOrder.Random)]
// 		public DataOrder Order { get; set; }
//
// 		[GlobalSetup]
// 		public void Init()
// 		{
// 			_data = BuildArray(Size, Order);
// 			_copy = new double[_data.Length];
// 		}
//
// 		private static double[] BuildArray(int size, DataOrder order)
// 		{
// 			var a = new double[size];
// 			var r = new Random(0);
//
// 			for (var i = 0; i < size; i++) a[i] = r.NextDouble() * size;
// 			if (order != DataOrder.Random) Array.Sort(a);
// 			if (order == DataOrder.Descending) Array.Reverse(a);
//
// 			return a;
// 		}
//
// 		[Benchmark(Baseline = true)]
// 		public void UseBuiltIn()
// 		{
// 			_data.CopyTo(_copy, 0);
// 			Array.Sort(_copy);
// 		}
//
// 		[Benchmark]
// 		public unsafe void UseIndexed()
// 		{
// 			_data.CopyTo(_copy, 0);
// 			var length = _copy.Length;
// 			fixed (void* ptr = &_copy[0])
// 			{
// 				IntroSorter<double, SpanIndexer<double>, DoubleComparer>.IntroSort(
// 					new SpanIndexer<double>(ptr), 0, length, new DoubleComparer());
// 			}
// 		}
//
// 		[Benchmark]
// 		public unsafe void UseLinear()
// 		{
// 			_data.CopyTo(_copy, 0);
// 			var length = _copy.Length;
// 			fixed (void* ptr = &_copy[0])
// 			{
// 				LinearIntroSorter<double, IntPtr, LinearSpanIndexer<double>, DoubleComparer>
// 					.IntroSort(
// 						new LinearSpanIndexer<double>(ptr), 0, length, new DoubleComparer());
// 			}
// 		}
// 		
// 		// [Benchmark]
// 		// public void UseIndexedIList()
// 		// {
// 		// 	_data.CopyTo(_copy, 0);
// 		// 	var length = _copy.Length;
// 		// 	IntroSorter<double, AnyIListIndexer<double>, DoubleComparer>.IntroSort(
// 		// 		new AnyIListIndexer<double>(_copy), 0, length, new DoubleComparer());
// 		// }
// 		//
// 		// [Benchmark]
// 		// public void UseLinearIList()
// 		// {
// 		// 	_data.CopyTo(_copy, 0);
// 		// 	var length = _copy.Length;
// 		// 	LinearIntroSorter<double, int, LinearListIndexer<double>, DoubleComparer>.IntroSort(
// 		// 		new LinearListIndexer<double>(_copy), 0, length, new DoubleComparer());
// 		// }
// 	}
// }
