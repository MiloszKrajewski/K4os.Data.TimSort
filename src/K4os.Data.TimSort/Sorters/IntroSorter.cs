using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;
using K4os.Data.TimSort.Internals;

namespace K4os.Data.TimSort.Sorters
{
	public class IntroSorter<T, TIndexer, TReference, TLessThan>:
		BasicSorter<T, TIndexer, TReference, TLessThan>
		where TIndexer: IIndexer<T, TReference>
		where TReference: IReference<TReference>
		where TLessThan: ILessThan<T>
	{
		private const int MinQuickSortWidth = 16;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort(
			TIndexer indexer,
			TReference lo, TReference hi, TLessThan comparer)
		{
			var length = hi.Dif(lo);
			var depth = (BitTwiddlingHacks.Log2((uint)length) + 1) << 1;
			IntroSort(indexer, lo, hi, comparer, depth);
		}

		#if NET5_0 || NET5_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		private static void IntroSort(
			TIndexer indexer, TReference lo, TReference hi,
			TLessThan comparer, int depth)
		{
			var length = hi.Dif(lo);

			switch (length)
			{
				case 0:
				case 1:
					// nothing to sort
					return;
				case 2:
					Sort2(indexer, lo, lo.Inc(), comparer);
					return;
				case 3:
					Sort3(indexer, lo, lo.Inc(), lo.Add(2), comparer);
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

			var mid = Partition(indexer, lo, hi, comparer);
			IntroSort(indexer, lo, mid, comparer, depth - 1);
			IntroSort(indexer, mid, hi, comparer, depth - 1);
		}

		#if NET5_0 || NET5_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		private static TReference Partition(
			TIndexer indexer, TReference lo, TReference hi,
			TLessThan comparer)
		{
			var mid = lo.Add(hi.Dif(lo) >> 1);
			Sort3(indexer, lo, mid, hi.Dec(), comparer);

			hi = hi.Sub(2);
			indexer.Swap(mid, hi);

			return PartitionLoop(lo, hi, indexer, comparer);
		}

		#if NET5_0 || NET5_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		private static TReference PartitionLoop(
			TReference lo, TReference hi, TIndexer indexer, TLessThan comparer)
		{
			var p = indexer[hi];

			for (var i = lo; i.Lt(hi); i = i.Inc())
			{
				if (!comparer.Lt(indexer[i], p)) continue;

				indexer.Swap(i, lo);
				lo = lo.Inc();
			}

			indexer.Swap(lo, hi);

			return lo;
		}
		
		#if NET5_0 || NET5_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		public static void HeapSort(
			TIndexer indexer, TReference lo, TReference hi,
			TLessThan comparer)
		{
			var length = hi.Dif(lo);

			for (var i = length >> 1; i > 0; i--)
			{
				DownHeap(indexer, lo, i, length, comparer);
			}

			for (var i = length; i > 1; i--)
			{
				indexer.Swap(lo, lo.Add(i - 1));
				DownHeap(indexer, lo, 1, i - 1, comparer);
			}
		}

		[SuppressMessage("ReSharper", "InconsistentNaming")]
		private static void DownHeap(
			TIndexer indexer, TReference offset,
			int i, int n,
			TLessThan comparer)
		{

			var iP = offset.Add(i);
			var d = indexer[iP.Dec()];

			while (i <= n >> 1)
			{
				var c = i << 1;
				var cP = offset.Add(c);

				if (c < n && comparer.Lt(indexer[cP.Dec()], indexer[cP]))
				{
					c++;
					cP = cP.Inc();
				}

				if (!comparer.Lt(d, indexer[cP.Dec()]))
					break;

				indexer[iP.Dec()] = indexer[cP.Dec()];

				i = c;
				iP = cP;
			}

			indexer[iP.Dec()] = d;
		}
	}
}
