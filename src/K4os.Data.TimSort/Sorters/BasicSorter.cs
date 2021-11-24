using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;

namespace K4os.Data.TimSort.Sorters
{
	/// <summary>
	/// Base class sort sorters providing some utilities.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	/// <typeparam name="TIndexer">Type of indexer.</typeparam>
	/// <typeparam name="TReference">Type of reference.</typeparam>
	/// <typeparam name="TLessThan">Type of comparer.</typeparam>
	public class BasicSorter<T, TIndexer, TReference, TLessThan>
		where TIndexer: IIndexer<T, TReference>
		where TReference: struct, IReference<TReference>
		where TLessThan: ILessThan<T>
	{
		/// <summary>
		/// Performs binary insertion sort on given array.
		/// https://ducmanhphan.github.io/2019-05-24-Binary-Insertion-sort/
		/// </summary>
		/// <param name="array">Array to be sorted.</param>
		/// <param name="lo">Lower bound (inclusive).</param>
		/// <param name="hi">Upper bound (exclusive).</param>
		/// <param name="comparer">Comparer.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void BinarySort(
			TIndexer array, TReference lo, TReference hi, TLessThan comparer)
		{
			if (hi.Dif(lo) < 2) return;
			var ascendingRun = CountRunAndMakeAscending(array, lo, hi, comparer);
			BinarySort(array, lo, hi, lo.Add(ascendingRun), comparer);
		}
		
		/// <summary>
		/// Returns the length of the run beginning at the specified position in
		/// the specified array and reverses the run if it is descending (ensuring
		/// that the run will always be ascending when the method returns).
		/// A run is the longest ascending sequence with: <c><![CDATA[a[lo] <= a[lo + 1] <= a[lo + 2] <= ...]]></c>
		/// or the longest descending sequence with: <c><![CDATA[a[lo] >  a[lo + 1] >  a[lo + 2] >  ...]]></c>
		/// For its intended use in a stable mergesort, the strictness of the
		/// definition of "descending" is needed so that the call can safely
		/// reverse a descending sequence without violating stability.
		/// </summary>
		/// <param name="array">the array in which a run is to be counted and possibly reversed.</param>
		/// <param name="lo">index of the first element in the run.</param>
		/// <param name="hi">index after the last element that may be contained in the run. It is required 
		/// that <c><![CDATA[lo < hi]]></c>.</param>
		/// <param name="comparer">the comparator to used for the sort.</param>
		/// <returns>the length of the run beginning at the specified position in the specified array</returns>
		#if NET5_0 || NET5_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		protected static int CountRunAndMakeAscending(
			TIndexer array, TReference lo, TReference hi, TLessThan comparer)
		{
			Debug.Assert(lo.Lt(hi));
			
			var a = array;
			var runHi = lo.Inc();
			if (runHi.Eq(hi)) return 1;

			// Find end of run, and reverse range if descending
			if (comparer.Lt(a[runHi.PostInc()], a[lo])) // c(a[runHi++], a[lo]) < 0
			{
				// Descending
				while (runHi.Lt(hi) && comparer.Lt(a[runHi], a[runHi.Dec()]))
					runHi = runHi.Inc();
				ReverseRange(a, lo, runHi);
			}
			else
			{
				// Ascending
				while (runHi.Lt(hi) && comparer.GtEq(a[runHi], a[runHi.Dec()])) // c(a[runHi], a[runHi - 1]) >= 0
					runHi = runHi.Inc();
			}

			return runHi.Dif(lo);
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

		/// <summary>Insertion sort implementation.</summary>
		/// <param name="array">Array to be sorted.</param>
		/// <param name="lo">Lower bound (inclusive).</param>
		/// <param name="hi">Upper bound (exclusive).</param>
		/// <param name="comparer">Comparer.</param>
		#if NET5_0 || NET5_0_OR_GREATER
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		public static void InsertionSort(
			TIndexer array, TReference lo, TReference hi,
			TLessThan comparer)
		{
			for (var i = lo.Inc(); i.Lt(hi); i = i.Inc())
			{
				var t = array[i];

				var j = i.Dec();
				while (j.GtEq(lo) && comparer.Lt(t, array[j]))
				{
					array[j.Inc()] = array[j];
					j = j.Dec();
				}

				array[j.Inc()] = t;
			}
		}

		/// <summary>Sorts two elements in array.</summary>
		/// <param name="indexer">Array.</param>
		/// <param name="lo">Lower bound.</param>
		/// <param name="comparer">Comparer.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort2(TIndexer indexer, TReference lo, TLessThan comparer) => 
			Sort2(indexer, lo, lo.Inc(), comparer);

		/// <summary>Sorts two elements in array.</summary>
		/// <param name="indexer">Array.</param>
		/// <param name="a">First element.</param>
		/// <param name="b">Second element.</param>
		/// <param name="comparer">Comparer.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort2(
			TIndexer indexer, TReference a, TReference b,
			TLessThan comparer)
		{
			if (comparer.Gt(indexer[a], indexer[b])) indexer.Swap(a, b);
		}

		/// <summary>Sorts three elements in array.</summary>
		/// <param name="indexer">Array.</param>
		/// <param name="lo">Lower bound.</param>
		/// <param name="comparer">Comparer.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort3(TIndexer indexer, TReference lo, TLessThan comparer) => 
			Sort3(indexer, lo, lo.Inc(), lo.Add(2), comparer);

		/// <summary>Sorts two elements in array.</summary>
		/// <param name="indexer">Array.</param>
		/// <param name="a">First element.</param>
		/// <param name="b">Second element.</param>
		/// <param name="c">Third element.</param>
		/// <param name="comparer">Comparer.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort3(
			TIndexer indexer, TReference a, TReference b, TReference c,
			TLessThan comparer)
		{
			Sort2(indexer, a, b, comparer);
			Sort2(indexer, a, c, comparer);
			Sort2(indexer, b, c, comparer);
		}
		
		/// <summary>Reverses range of array.</summary>
		/// <param name="array">Array.</param>
		/// <param name="lo">Lower bound (inclusive).</param>
		/// <param name="hi">Upper bound (exclusive).</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void ReverseRange(TIndexer array, TReference lo, TReference hi) => 
			array.Reverse(lo, hi);

		/// <summary>Copies range of array.</summary>
		/// <param name="array">Array.</param>
		/// <param name="sourceIndex">Source reference.</param>
		/// <param name="targetIndex">Target reference.</param>
		/// <param name="length">Length of range.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void CopyRange(
			TIndexer array, TReference sourceIndex, TReference targetIndex, int length) =>
			array.Copy(sourceIndex, targetIndex, length);

		/// <summary>Copies range of array.</summary>
		/// <param name="source">Source array.</param>
		/// <param name="sourceIndex">Source reference.</param>
		/// <param name="target">Target array.</param>
		/// <param name="targetIndex">Target reference.</param>
		/// <param name="length">Length of range.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void CopyRange(
			TIndexer source, TReference sourceIndex, T[] target, int targetIndex, int length) =>
			source.Export(sourceIndex, target.AsSpan(targetIndex, length), length);

		/// <summary>Copies range of array.</summary>
		/// <param name="source">Source array.</param>
		/// <param name="sourceIndex">Source reference.</param>
		/// <param name="target">Target array.</param>
		/// <param name="targetIndex">Target reference.</param>
		/// <param name="length">Length of range.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void CopyRange(
			T[] source, int sourceIndex, TIndexer target, TReference targetIndex, int length) =>
			target.Import(targetIndex, source.AsSpan(sourceIndex, length), length);
	}
}
