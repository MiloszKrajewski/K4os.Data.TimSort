using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;

namespace Benchmarks
{
	public class StructInliningBenchmarks
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
			DoubleSorter1(_copy.AsSpan());
		}

		[Benchmark]
		public void UsingPointers()
		{
			_data.CopyTo(_copy, 0);
			DoubleSorter2(_copy.AsSpan());
		}

		[Benchmark]
		public void UsingReference()
		{
			_data.CopyTo(_copy, 0);
			DoubleSorter3(_copy.AsSpan());
		}

		public static void DoubleSorter1(Span<double> array)
		{
			var l = array.Length;
			for (var i = 1; i < l; i++)
			for (var j = i; j > 0; j--)
			{
				if (array[j - 1] <= array[j]) break;

				Swap(array, j - 1, j);
			}
		}

		public static unsafe void DoubleSorter2(Span<double> array)
		{
			fixed (double* ptr0 = &array[0])
			{
				var l = ptr0 + array.Length;
				for (var i = ptr0 + 1; i < l; i++)
				for (var j = i; j > ptr0; j--)
				{
					if (*(j - 1) <= *j) break;

					Swap(j - 1, j);
				}
			}
		}

		public static unsafe void DoubleSorter3(Span<double> array)
		{
			fixed (double* ptr0 = &array[0])
			{
				var indexer = new PtrIndexer<double>(ptr0);
				var length = array.Length;
				GenericDoubleSorter3<
						double,
						PtrIndexer<double>,
						PtrReference<double>,
						DefaultLessThan<double>>
					.Sort(indexer, length, default);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Swap(Span<double> array, int i, int j)
		{
			// ReSharper disable once SwapViaDeconstruction
			var t = array[i];
			array[i] = array[j];
			array[j] = t;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void Swap(double* i, double* j)
		{
			// ReSharper disable once SwapViaDeconstruction
			var t = *i;
			*i = *j;
			*j = t;
		}

		public class GenericDoubleSorter3<T, TIndexer, TReference, TLessThan>
			where TIndexer: IIndexer<T, TReference>
			where TReference: IReference<TReference>
			where TLessThan: ILessThan<T>
		{
			public static void Sort(TIndexer indexer, int length, TLessThan comparer)
			{
				var ref0 = indexer.Ref0;
				var refL = ref0.Add(length);
				for (var i = ref0.Inc(); i.Lt(refL); i = i.Inc())
				for (var j = i; j.Gt(ref0); j = j.Dec())
				{
					if (comparer.LtEq(indexer[j.Dec()], indexer[j])) break;

					indexer.Swap(j.Dec(), j);
				}
			}
		}
	}
}
