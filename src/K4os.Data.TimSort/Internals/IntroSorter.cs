// ReSharper disable SwapViaDeconstruction

using System;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals
{
	public class IntroSorter<T, TIndexer, TComparer>
		where TIndexer: ITimIndexer<T>
		where TComparer: ITimComparer<T>
	{
		private const int MinQuickSortWidth = 16;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort(
			TIndexer indexer, int lo, int hi, TComparer comparer) =>
			IntroSort(
				indexer, lo, hi, comparer, 
				(BitTwiddlingHacks.Log2((uint)(hi - lo)) + 1) << 1);

		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		public static void IntroSort(
			TIndexer indexer, int lo, int hi, TComparer comparer, int depth)
		{
			var length = hi - lo;

			switch (length)
			{
				case 0:
				case 1:
					// nothing to sort
					return;
				case 2:
					Sort2(indexer, lo, lo + 1, comparer);
					return;
				case 3:
					Sort3(indexer, lo, lo + 1, lo + 2, comparer);
					return;
				case < MinQuickSortWidth:
					InsertionSort(indexer, lo, hi, comparer);
					return;
			}

			if (depth <= 0)
			{
				HeapSort(indexer, lo, hi, comparer);
				return;
			}

			var m = Partition(indexer, lo, hi, comparer);
			IntroSort(indexer, lo, m, comparer, depth - 1);
			IntroSort(indexer, m + 1, hi, comparer, depth - 1);
		}

		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		private static int Partition(TIndexer indexer, int lo, int hi, TComparer comparer)
		{
			var mid = (int)((uint)(hi + lo) >> 1);
			Sort3(indexer, lo, mid, hi - 1, comparer);

			hi -= 2;
			Swap(indexer, mid, hi);

			var p = indexer[hi];

			var j = lo;

			for (var i = lo; i < hi; i++)
			{
				if (comparer.Lt(indexer[i], p))
					Swap(indexer, i, j++);
			}

			Swap(indexer, j, hi);

			return j;
		}

		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		public static void InsertionSort(
			TIndexer keys, int lo, int hi, TComparer comparer)
		{
			for (var i = lo + 1; i < hi; i++)
			for (var j = i; j > lo; j--)
			{
				if (comparer.LtEq(keys[j - 1], keys[j]))
					break;

				Swap(keys, j - 1, j);
			}
		}

		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		public static void HeapSort(
			TIndexer keys, int lo, int hi, TComparer comparer)
		{
			var length = hi - lo;

			for (var i = length >> 1; i > 0; i--)
			{
				DownHeap(keys, lo, i, length, comparer);
			}

			for (var i = length; i > 1; i--)
			{
				Swap(keys, lo, lo + i - 1);
				DownHeap(keys, lo, 1, i - 1, comparer);
			}
		}

		private static void DownHeap(
			TIndexer indexer, int offset, int i, int n, TComparer comparer)
		{
			var d = indexer[offset + i - 1];
			while (i <= n >> 1)
			{
				var child = i << 1;
				if (child < n && comparer.Lt(indexer[offset + child - 1], indexer[offset + child]))
					child++;

				if (!comparer.Lt(d, indexer[offset + child - 1]))
					break;

				indexer[offset + i - 1] = indexer[offset + child - 1];
				i = child;
			}

			indexer[offset + i - 1] = d;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Swap(TIndexer array, int a, int b)
		{
			var swap = array[a];
			array[a] = array[b];
			array[b] = swap;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Sort2(TIndexer array, int a, int b, TComparer comparer)
		{
			if (comparer.Gt(array[a], array[b])) Swap(array, a, b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Sort3(TIndexer array, int a, int b, int c, TComparer comparer)
		{
			Sort2(array, a, b, comparer);
			Sort2(array, a, c, comparer);
			Sort2(array, b, c, comparer);
		}
	}
}
