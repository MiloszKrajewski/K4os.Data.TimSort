using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals
{
	internal class TimSorter
	{
		#region consts

		/// <summary>
		/// This is the minimum sized sequence that will be merged.  Shorter
		/// sequences will be lengthened by calling BinarySort.  If the entire
		/// array is less than this length, no merges will be performed.
		/// This constant should be a power of two.  It was 64 in Tim Peter's C
		/// implementation, but 32 was empirically determined to work better in
		/// this implementation.  In the unlikely event that you set this constant
		/// to be a number that's not a power of two, you'll need to change the
		/// <c>minRunLength</c> computation.
		/// If you decrease this constant, you must change the stackLen
		/// computation in the TimSort constructor, or you risk an
		/// ArrayOutOfBounds exception.
		/// See ALGORITHM.txt for a discussion of the minimum stack length required
		/// as a function of the length of the array being sorted and the minimum merge
		/// sequence length.
		/// </summary>
		protected const int MIN_MERGE = 32;

		/// <summary>
		/// When we get into galloping mode, we stay there until both runs win less
		/// often than MIN_GALLOP consecutive times.
		/// </summary>
		protected const int MIN_GALLOP = 7;

		/// <summary>
		/// Maximum initial size of tmp array, which is used for merging. 
		/// The array can grow to accommodate demand.
		/// Unlike Tim's original C version, we do not allocate this much storage
		/// when sorting smaller arrays. This change was required for performance.
		/// </summary>
		protected const int INITIAL_TMP_STORAGE_LENGTH = 256;

		#endregion

		/// <summary>
		/// Returns the minimum acceptable run length for an array of the specified length.
		/// Natural runs shorter than this will be extended with BinarySort.
		/// Roughly speaking, the computation is:
		/// If <c>n &lt; MIN_MERGE</c>, return n (it's too small to bother with fancy stuff).
		/// Else if n is an exact power of 2, return <c>MIN_MERGE/2</c>.
		/// Else return an int k, <c>MIN_MERGE/2 &lt;= k &lt;= MIN_MERGE</c>,
		/// such that <c>n/k</c> is close to, but strictly less than, an exact power of 2.
		/// For the rationale, see ALGORITHM.txt.
		/// </summary>
		/// <param name="n">the length of the array to be sorted.</param>
		/// <returns>the length of the minimum run to be merged.</returns>
		protected static int GetMinimumRunLength(int n)
		{
			Debug.Assert(n >= 0);
			var r = 0; // Becomes 1 if any 1 bits are shifted off
			while (n >= MIN_MERGE)
			{
				r |= n & 1;
				n >>= 1;
			}

			return n + r;
		}

		/// <summary>
		/// Checks that fromIndex and toIndex are in range, and throws an
		/// appropriate exception if they aren't.
		/// </summary>
		/// <param name="arrayLen">the length of the array.</param>
		/// <param name="fromIndex">the index of the first element of the range.</param>
		/// <param name="toIndex">the index after the last element of the range.</param>
		protected static void CheckRange(int arrayLen, int fromIndex, int toIndex)
		{
			if (fromIndex > toIndex)
				throw new ArgumentException(
					$"fromIndex({fromIndex}) > toIndex({toIndex})");
			if (fromIndex < 0)
				throw new IndexOutOfRangeException(
					$"fromIndex ({fromIndex}) is out of bounds");
			if (toIndex > arrayLen)
				throw new IndexOutOfRangeException(
					$"toIndex ({toIndex}) is out of bounds");
		}

		/// <summary>Generates exception when comparer is not well implemented.</summary>
		/// <returns>New <c>InvalidOperationException</c> exception to be thrown.</returns>
		protected static InvalidOperationException CorruptedComparer() =>
			new("Comparison method violates its general contract!");
	}

	internal class TimSorter<T>: TimSorter
	{
		/// <summary>
		/// Locates the position at which to insert the specified key into the
		/// specified sorted range; if the range contains an element equal to key,
		/// returns the index of the leftmost equal element.
		/// </summary>
		/// <param name="key">the key whose insertion point to search for.</param>
		/// <param name="array">the array in which to search.</param>
		/// <param name="lo">the index of the first element in the range.</param>
		/// <param name="length">the length of the range; must be &gt; 0.</param>
		/// <param name="hint">the index at which to begin the search, 0 &lt;= hint &lt; n. 
		/// The closer hint is to the result, the faster this method will run.</param>
		/// <param name="comparer">the comparator used to order the range, and to search.</param>
		/// <returns>the int k,  0 &lt;= k &lt;= n such that a[b + k - 1] &lt; key &lt;= a[b + k], pretending that a[b - 1] 
		/// is minus infinity and a[b + n] is infinity. In other words, key belongs at index b + k; or in other words, the 
		/// first k elements of a should precede key, and the last n - k should follow it.</returns>
		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		protected static int GallopLeft<TIndexer, TComparer>(
			T key, TIndexer array, int lo, int length, int hint, TComparer comparer)
			where TIndexer: ITimIndexer<T>
			where TComparer: ITimComparer<T>
		{
			Debug.Assert(length > 0 && hint >= 0 && hint < length);

			var a = array;
			var lastOfs = 0;
			var ofs = 1;

			if (comparer.Gt(key, a[lo + hint]))
			{
				// Gallop right until a[base+hint+lastOfs] < key <= a[base+hint+ofs]
				var maxOfs = length - hint;
				while (ofs < maxOfs && comparer.Gt(key, a[lo + hint + ofs]))
				{
					lastOfs = ofs;
					ofs = (ofs << 1) + 1;
					if (ofs <= 0) ofs = maxOfs;
				}

				if (ofs > maxOfs) ofs = maxOfs;

				// Make offsets relative to base
				lastOfs += hint;
				ofs += hint;
			}
			else // if (key <= a[base + hint])
			{
				// Gallop left until a[base+hint-ofs] < key <= a[base+hint-lastOfs]
				var maxOfs = hint + 1;
				while (ofs < maxOfs && comparer.LtEq(key, a[lo + hint - ofs]))
				{
					lastOfs = ofs;
					ofs = (ofs << 1) + 1;
					if (ofs <= 0) ofs = maxOfs;
				}

				if (ofs > maxOfs) ofs = maxOfs;

				// Make offsets relative to base
				var tmp = lastOfs;
				lastOfs = hint - ofs;
				ofs = hint - tmp;
			}

			Debug.Assert(-1 <= lastOfs && lastOfs < ofs && ofs <= length);

			// Now a[base+lastOfs] < key <= a[base+ofs], so key belongs somewhere
			// to the right of lastOfs but no farther right than ofs.  Do a binary
			// search, with invariant a[base + lastOfs - 1] < key <= a[base + ofs].
			lastOfs++;
			while (lastOfs < ofs)
			{
				var m = lastOfs + ((ofs - lastOfs) >> 1);

				if (comparer.Gt(key, a[lo + m]))
					lastOfs = m + 1; // a[base + m] < key
				else
					ofs = m; // key <= a[base + m]
			}

			Debug.Assert(lastOfs == ofs); // so a[base + ofs - 1] < key <= a[base + ofs]
			return ofs;
		}

		/// <summary>
		/// Like GallopLeft, except that if the range contains an element equal to
		/// key, GallopRight returns the index after the rightmost equal element.
		/// </summary>
		/// <param name="key">the key whose insertion point to search for.</param>
		/// <param name="array">the array in which to search.</param>
		/// <param name="lo">the index of the first element in the range.</param>
		/// <param name="length">the length of the range; must be &gt; 0.</param>
		/// <param name="hint">the index at which to begin the search, 0 &lt;= hint &lt; n. The closer hint is to the result, 
		/// the faster this method will run.</param>
		/// <param name="comparer">the comparator used to order the range, and to search.</param>
		/// <returns>int k, that 0 &lt;= k &lt;= n such that a[b + k - 1] &lt;= key &lt; a[b + k]</returns>
		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		protected static int GallopRight<TIndexer, TComparer>(
			T key, TIndexer array, int lo, int length, int hint, TComparer comparer)
			where TIndexer: ITimIndexer<T>
			where TComparer: ITimComparer<T>
		{
			Debug.Assert(length > 0 && hint >= 0 && hint < length);

			var a = array;
			var ofs = 1;
			var lastOfs = 0;
			if (comparer.Lt(key, a[lo + hint]))
			{
				// Gallop left until a[b+hint - ofs] <= key < a[b+hint - lastOfs]
				var maxOfs = hint + 1;
				while (ofs < maxOfs && comparer.Lt(key, a[lo + hint - ofs]))
				{
					lastOfs = ofs;
					ofs = (ofs << 1) + 1;
					if (ofs <= 0) ofs = maxOfs;
				}

				if (ofs > maxOfs) ofs = maxOfs;

				// Make offsets relative to b
				var tmp = lastOfs;
				lastOfs = hint - ofs;
				ofs = hint - tmp;
			}
			else
			{
				// a[b + hint] <= key
				// Gallop right until a[b+hint + lastOfs] <= key < a[b+hint + ofs]
				var maxOfs = length - hint;
				while (ofs < maxOfs && comparer.GtEq(key, a[lo + hint + ofs]))
				{
					lastOfs = ofs;
					ofs = (ofs << 1) + 1;
					if (ofs <= 0) ofs = maxOfs;
				}

				if (ofs > maxOfs) ofs = maxOfs;

				// Make offsets relative to b
				lastOfs += hint;
				ofs += hint;
			}

			Debug.Assert(-1 <= lastOfs && lastOfs < ofs && ofs <= length);

			// Now a[b + lastOfs] <= key < a[b + ofs], so key belongs somewhere to
			// the right of lastOfs but no farther right than ofs.  Do a binary
			// search, with invariant a[b + lastOfs - 1] <= key < a[b + ofs].
			lastOfs++;
			while (lastOfs < ofs)
			{
				var m = lastOfs + ((ofs - lastOfs) >> 1);

				if (comparer.Lt(key, a[lo + m]))
					ofs = m; // key < a[b + m]
				else
					lastOfs = m + 1; // a[b + m] <= key
			}

			Debug.Assert(lastOfs == ofs); // so a[b + ofs - 1] <= key < a[b + ofs]
			return ofs;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ref byte Ref0(T[] buffer) =>
			ref Unsafe.As<T, byte>(ref buffer[0]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static unsafe int GallopLeft<TComparer>(
			T key, T[] buffer, int lo, int length, int hint, TComparer comparer)
			where TComparer: ITimComparer<T>
		{
			fixed (void* pinned = &Ref0(buffer))
				return GallopLeft(key, new SpanIndexer<T>(pinned), lo, length, hint, comparer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static unsafe int GallopRight<TComparer>(
			T key, T[] buffer, int lo, int length, int hint, TComparer comparer)
			where TComparer: ITimComparer<T>
		{
			fixed (void* pinned = &Ref0(buffer))
				return GallopRight(key, new SpanIndexer<T>(pinned), lo, length, hint, comparer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void ReverseRange<TIndexer>(
			in TIndexer indexer, int lo, int hi)
			where TIndexer: ITimIndexer<T> =>
			indexer.Reverse(lo, hi);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void CopyRange<TIndexer>(
			in TIndexer indexer, int sourceOffset, int targetOffset, int length)
			where TIndexer: ITimIndexer<T> =>
			indexer.Copy(sourceOffset, targetOffset, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void CopyRange<TIndexer>(
			in TIndexer source, int sourceOffset, T[] target, int targetOffset, int length)
			where TIndexer: ITimIndexer<T> =>
			source.Export(sourceOffset, target, targetOffset, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void CopyRange<TIndexer>(
			T[] source, int sourceOffset, in TIndexer target, int targetOffset, int length)
			where TIndexer: ITimIndexer<T> =>
			target.Import(targetOffset, source, sourceOffset, length);

		/// <summary>
		/// Sorts the specified portion of the specified array using a binary insertion sort.
		/// This is the best method for sorting small numbers of elements.
		/// It requires O(n log n) compares, but O(n^2) data movement (worst case).
		/// If the initial part of the specified range is already sorted, this method can take
		/// advantage of it: the method assumes that the elements from index <c>lo</c>, inclusive,
		/// to <c>start</c>, exclusive are already sorted.
		/// </summary>
		/// <param name="array">the array in which a range is to be sorted.</param>
		/// <param name="lo">the index of the first element in the range to be sorted.</param>
		/// <param name="hi">the index after the last element in the range to be sorted.</param>
		/// <param name="start">start the index of the first element in the range that is not
		/// already known to be sorted (<c><![CDATA[lo <= start <= hi]]></c>)</param>
		/// <param name="comparer">The comparator to used for the sort.</param>
		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		protected static void BinarySort<TIndexer, TComparer>(
			TIndexer array, int lo, int hi, int start, TComparer comparer)
			where TIndexer: ITimIndexer<T>
			where TComparer: ITimComparer<T>
		{
			Debug.Assert(lo <= start && start <= hi);

			var a = array;

			if (start == lo) start++;

			for ( /* nothing */; start < hi; start++)
			{
				var pivot = a[start];

				// Set left (and right) to the index where a[start] (pivot) belongs
				var left = lo;
				var right = start;
				Debug.Assert(left <= right);

				// Invariants:
				// * pivot >= all in [lo, left).
				// * pivot < all in [right, start).
				while (left < right)
				{
					var mid = (int)((uint)(left + right) >> 1); // no overflow
					if (comparer.Lt(pivot, a[mid]))
					{
						right = mid;
					}
					else
					{
						left = mid + 1;
					}
				}

				Debug.Assert(left == right);

				// The invariants still hold: pivot >= all in [lo, left) and
				// pivot < all in [left, start), so pivot belongs at left.  Note
				// that if there are elements equal to pivot, left points to the
				// first slot after them -- that's why this sort is stable.
				// Slide elements over to make room to make room for pivot.

				var n = start - left; // The number of elements to move

				// switch is just an optimization for copyRange in default case
				switch (n)
				{
					case 2:
						a[left + 2] = a[left + 1];
						a[left + 1] = a[left];
						break;
					case 1:
						a[left + 1] = a[left];
						break;
					default:
						CopyRange(a, left, left + 1, n);
						break;
				}

				a[left] = pivot;
			}
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
		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		protected static int CountRunAndMakeAscending<TIndexer, TComparer>(
			TIndexer array, int lo, int hi, TComparer comparer)
			where TIndexer: ITimIndexer<T>
			where TComparer: ITimComparer<T>
		{
			Debug.Assert(lo < hi);

			var a = array;
			var runHi = lo + 1;
			if (runHi == hi) return 1;

			// Find end of run, and reverse range if descending
			if (comparer.Lt(a[runHi++], a[lo]))
			{
				// Descending
				while (runHi < hi && comparer.Lt(a[runHi], a[runHi - 1])) runHi++;
				ReverseRange(a, lo, runHi);
			}
			else
			{
				// Ascending
				while (runHi < hi && comparer.GtEq(a[runHi], a[runHi - 1])) runHi++;
			}

			return runHi - lo;
		}

		/// <summary>Sorts the specified array.</summary>
		/// <param name="array">Array to be sorted.</param>
		/// <param name="arrayLength">Total length of array.</param>
		/// <param name="lo">the index of the first element in the range to be sorted.</param>
		/// <param name="hi">the index after the last element in the range to be sorted.</param>
		/// <param name="comparer">The comparator to determine the order of the sort.</param>
		public static void Sort<TIndexer, TComparer>(
			TIndexer array, int arrayLength, int lo, int hi, TComparer comparer)
			where TIndexer: ITimIndexer<T>
			where TComparer: ITimComparer<T>
		{
			CheckRange(arrayLength, lo, hi);

			var width = hi - lo;
			if (width < 2) return; // Arrays of size 0 and 1 are always sorted

			// If array is small, do a "mini-TimSort" with no merges
			if (width < MIN_MERGE)
			{
				var initRunLength = CountRunAndMakeAscending(array, lo, hi, comparer);
				BinarySort(array, lo, hi, lo + initRunLength, comparer);
				return;
			}

			// March over the array once, left to right, finding natural runs,
			// extending short natural runs to minRun elements, and merging runs
			// to maintain stack invariant.
			var sorter = new TimSorter<T, TIndexer, TComparer>(array, comparer, arrayLength);
			sorter.MergeSort(array, lo, hi, comparer);
		}
	}
}
