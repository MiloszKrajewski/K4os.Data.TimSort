// ReSharper disable SwapViaDeconstruction

using System;
using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Internals;

namespace Benchmarks.FiddleArea
{
	public class DoubleIntroSorter1
	{
		private const int MinQuickSortWidth = 16;
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort(
			SpanIndexer<double> indexer, int lo, int hi) =>
			IntroSort(indexer, lo, hi, (BitTwiddlingHacks.Log2((uint)(hi - lo)) + 1) << 1);

		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		public static void IntroSort(
			SpanIndexer<double> indexer, int lo, int hi, int depth)
		{
			var length = hi - lo;

			switch (length)
			{
				case 0:
				case 1:
					// nothing to sort
					return;
				case 2:
					Sort2(indexer, lo, lo + 1);
					return;
				case 3:
					Sort3(indexer, lo, lo + 1, lo + 2);
					return;
				case < MinQuickSortWidth:
					InsertionSort(indexer, lo, hi);
					return;
			}

			var m = Partition(indexer, lo, hi);
			IntroSort(indexer, lo, m, depth - 1);
			IntroSort(indexer, m + 1, hi, depth - 1);
		}

		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		private static int Partition(SpanIndexer<double> indexer, int lo, int hi)
		{
			var mid = (int)((uint)(hi + lo) >> 1);
			Sort3(indexer, lo, mid, hi - 1);

			hi -= 2;
			Swap(indexer, mid, hi);

			var p = indexer[hi];

			var j = lo;

			for (var i = lo; i < hi; i++)
			{
				if (indexer[i] < p)
					Swap(indexer, i, j++);
			}

			Swap(indexer, j, hi);

			return j;
		}

		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		public static void InsertionSort(
			SpanIndexer<double> keys, int lo, int hi)
		{
			for (var i = lo + 1; i < hi; i++)
			for (var j = i; j > lo; j--)
			{
				if (keys[j - 1] <= keys[j])
					break;

				Swap(keys, j - 1, j);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Swap(SpanIndexer<double> array, int a, int b)
		{
			var swap = array[a];
			array[a] = array[b];
			array[b] = swap;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Sort2(SpanIndexer<double> array, int a, int b)
		{
			if (array[a] > array[b]) Swap(array, a, b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Sort3(SpanIndexer<double> array, int a, int b, int c)
		{
			Sort2(array, a, b);
			Sort2(array, a, c);
			Sort2(array, b, c);
		}
	}
}
