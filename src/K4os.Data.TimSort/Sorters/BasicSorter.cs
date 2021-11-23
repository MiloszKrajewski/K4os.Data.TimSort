using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;

namespace K4os.Data.TimSort.Sorters
{
	public class BasicSorter<T, TIndexer, TReference, TLessThan>
		where TIndexer: IIndexer<T, TReference>
		where TReference: IReference<TReference>
		where TLessThan: ILessThan<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BinarySort(
			TIndexer array, TReference lo, TReference hi, TLessThan comparer)
		{
			BinarySort(array, lo, hi, lo, comparer);
		}

		/// <summary>
		/// Sorts the specified portion of the specified array using a binary insertion sort.
		/// This is the best method for sorting small numbers of elements.
		/// It requires O(n log n) compares, but O(n^2) data movement (worst case). If the initial
		/// part of the specified range is already sorted, this method can take advantage of it:
		/// the method assumes that the elements from index <c>lo</c>, inclusive, to <c>start</c>,
		/// exclusive are already sorted.
		/// </summary>
		/// <param name="array">the array in which a range is to be sorted.</param>
		/// <param name="lo">the index of the first element in the range to be sorted.</param>
		/// <param name="hi">the index after the last element in the range to be sorted.</param>
		/// <param name="start">start the index of the first element in the range that is not
		/// already known to be sorted (<c><![CDATA[lo <= start <= hi]]></c>)</param>
		/// <param name="comparer">The comparator to used for the sort.</param>
		#if NET5_0 || NET5_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		protected static void BinarySort(
			TIndexer array, TReference lo, TReference hi, TReference start, TLessThan comparer)
		{
			Debug.Assert(lo.LtEq(start) && start.LtEq(hi));

			var a = array;

			if (start.Eq(lo)) start = start.Inc();

			for ( /* nothing */; start.Lt(hi); start = start.Inc())
			{
				var pivot = a[start];

				// Set left (and right) to the index where a[start] (pivot) belongs
				var left = lo;
				var right = start;
				Debug.Assert(left.LtEq(right));

				// Invariants:
				// * pivot >= all in [lo, left).
				// * pivot < all in [right, start).
				while (left.Lt(right))
				{
					var mid = left.Mid(right);
					if (comparer.Lt(pivot, a[mid])) // c(pivot, a[mid]) < 0
					{
						right = mid;
					}
					else
					{
						left = mid.Inc();
					}
				}

				Debug.Assert(left.Eq(right));

				// The invariants still hold: pivot >= all in [lo, left) and
				// pivot < all in [left, start), so pivot belongs at left.  Note
				// that if there are elements equal to pivot, left points to the
				// first slot after them -- that's why this sort is stable.
				// Slide elements over to make room to make room for pivot.

				var n = start.Dif(left); // The number of elements to move

				// switch is just an optimization for copyRange in default case
				switch (n)
				{
					case 2:
						a[left.Add(2)] = a[left.Inc()];
						a[left.Inc()] = a[left];
						break;
					case 1:
						a[left.Inc()] = a[left];
						break;
					default:
						a.Copy(left, left.Inc(), n);
						break;
				}

				a[left] = pivot;
			}
		}

		#if NET5_0 || NET5_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		public static void InsertionSort(
			TIndexer indexer, TReference lo, TReference hi,
			TLessThan comparer)
		{
			for (var i = lo.Inc(); i.Lt(hi); i = i.Inc())
			{
				var t = indexer[i];

				var j = i.Dec();
				while (j.GtEq(lo) && comparer.Lt(t, indexer[j]))
				{
					indexer[j.Inc()] = indexer[j];
					j = j.Dec();
				}

				indexer[j.Inc()] = t;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort2(TIndexer indexer, TReference lo, TLessThan comparer)
		{
			Sort2(indexer, lo, lo.Inc(), comparer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort2(
			TIndexer indexer, TReference a, TReference b,
			TLessThan comparer)
		{
			if (comparer.Gt(indexer[a], indexer[b])) indexer.Swap(a, b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort3(TIndexer indexer, TReference a, TLessThan comparer)
		{
			Sort3(indexer, a, a.Inc(), a.Add(2), comparer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort3(
			TIndexer indexer, TReference a, TReference b, TReference c,
			TLessThan comparer)
		{
			Sort2(indexer, a, b, comparer);
			Sort2(indexer, a, c, comparer);
			Sort2(indexer, b, c, comparer);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void ReverseRange(TIndexer array, TReference lo, TReference hi) => 
			array.Reverse(lo, hi);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void CopyRange(
			TIndexer array, TReference sourceIndex, TReference targetIndex, int length) =>
			array.Copy(sourceIndex, targetIndex, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void CopyRange(
			TIndexer source, TReference sourceIndex, T[] target, int targetIndex, int length) =>
			source.Export(sourceIndex, target.AsSpan(targetIndex, length), length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void CopyRange(
			T[] source, int sourceIndex, TIndexer target, TReference targetIndex, int length) =>
			target.Import(targetIndex, source.AsSpan(sourceIndex, length), length);
	}
}
