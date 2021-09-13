using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ReferenceImplementation
{
	/// <summary>TimSort implementation for AnyIList.</summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public class AnyIListTimSort<T>: TimSortBase<IList<T>, T>
	{
		/// <summary>The comparator for this sort.</summary>
		private readonly Comparison<T> _comparer;

		/// <summary>
		/// Prevents a default instance of the AnyIListTimSort&lt;T&gt; class from being created.
		/// Creates a TimSort instance to maintain the state of an ongoing sort.
		/// </summary>
		/// <param name="array">The array to be sorted.</param>
		/// <param name="comparer">The comparator to determine the order of the sort.</param>
		private AnyIListTimSort(IList<T> array, Comparison<T> comparer)
			: base(array, array.Count)
		{
			_comparer = comparer;
		}

		/// <summary>Sorts the specified array.</summary>
		/// <param name="array">Array to be sorted.</param>
		/// <param name="comparer">Comparer.</param>
		public static void Sort(IList<T> array, Comparison<T> comparer)
		{
			Sort(array, 0, array.Count, comparer);
		}

		/// <summary>Sorts the specified array.</summary>
		/// <param name="array">Array to be sorted.</param>
		/// <param name="lo">the index of the first element in the range to be sorted.</param>
		/// <param name="hi">the index after the last element in the range to be sorted.</param>
		/// <param name="comparer">The comparator to determine the order of the sort.</param>
		public static void Sort(IList<T> array, int lo, int hi, Comparison<T> comparer)
		{
			CheckRange(array.Count, lo, hi);

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
			var sorter = new AnyIListTimSort<T>(array, comparer);
			int minRun = GetMinimumRunLength(width);
			do
			{
				// Identify next run
				var runLen = CountRunAndMakeAscending(array, lo, hi, comparer);

				// If run is short, extend to min(minRun, nRemaining)
				if (runLen < minRun)
				{
					var force = width <= minRun ? width : minRun;
					BinarySort(array, lo, lo + force, lo + runLen, comparer);
					runLen = force;
				}

				// Push run onto pending-run stack, and maybe merge
				sorter.PushRun(lo, runLen);
				sorter.MergeCollapse();

				// Advance to find next run
				lo += runLen;
				width -= runLen;
			}
			while (width != 0);

			// Merge all remaining runs to complete sort
			Debug.Assert(lo == hi);
			sorter.MergeForceCollapse();
			Debug.Assert(sorter._stackSize == 1);
		}

		/// <summary>
		/// Sorts the specified portion of the specified array using a binary insertion sort. This is the best method for 
		/// sorting small numbers of elements. It requires O(n log n) compares, but O(n^2) data movement (worst case).
		/// If the initial part of the specified range is already sorted, this method can take advantage of it: the method 
		/// assumes that the elements from index <c>lo</c>, inclusive, to <c>start</c>, exclusive are already sorted.
		/// </summary>
		/// <param name="array">the array in which a range is to be sorted.</param>
		/// <param name="lo">the index of the first element in the range to be sorted.</param>
		/// <param name="hi">the index after the last element in the range to be sorted.</param>
		/// <param name="start">start the index of the first element in the range that is not already known to be sorted 
		/// (<c><![CDATA[lo <= start <= hi]]></c>)</param>
		/// <param name="comparer">The comparator to used for the sort.</param>
		private static void BinarySort(
			IList<T> array, int lo, int hi, int start, Comparison<T> comparer)
		{
			var a = array;
			{
				// fixed (...)
				Debug.Assert(lo <= start && start <= hi);

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
						var mid = (left + right) >> 1;
						if (comparer(pivot, a[mid]) < 0) // c(pivot, a[mid]) < 0
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
							IndexedCopyRange(a, left, left + 1, n);
							break;
					}

					a[left] = pivot;
				}
			} // fixed (...)
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
		private static int CountRunAndMakeAscending(
			IList<T> array, int lo, int hi, Comparison<T> comparer)
		{
			var a = array;
			{
				// fixed (...)
				Debug.Assert(lo < hi);
				var runHi = lo + 1;
				if (runHi == hi) return 1;

				// Find end of run, and reverse range if descending
				if (comparer(a[runHi++], a[lo]) < 0) // c(a[runHi++], a[lo]) < 0
				{
					// Descending
					while (runHi < hi && comparer(a[runHi], a[runHi - 1]) < 0) runHi++;
					IndexedReverseRange(a, lo, runHi);
				}
				else
				{
					// Ascending
					while (runHi < hi && comparer(a[runHi], a[runHi - 1]) >= 0)
						runHi++; // c(a[runHi], a[runHi - 1]) >= 0
				}

				return runHi - lo;
			} // fixed (...)
		}

		/// <summary>Copies the range from one array to another.</summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="srcIndex">Source index in array.</param>
		/// <param name="dstIndex">Target index in array.</param>
		/// <param name="length">Number of elements to be copied.</param>
		private static void IndexedCopyRange(
			IList<T> buffer, int srcIndex, int dstIndex, int length)
		{
			if (dstIndex > srcIndex) // copy forward to the same array => use reverse order
			{
				srcIndex += length - 1;
				dstIndex += length - 1;
				while (length-- > 0) buffer[dstIndex--] = buffer[srcIndex--];
			}
			else // copy to different array or backwards => use natural order
			{
				while (length-- > 0) buffer[dstIndex++] = buffer[srcIndex++];
			}
		}

		/// <summary>Copies the range from one array to another.</summary>
		/// <param name="src">The source array.</param>
		/// <param name="srcIndex">Starting index in source array.</param>
		/// <param name="dst">The destination array.</param>
		/// <param name="dstIndex">Starting index in destination array.</param>
		/// <param name="length">Number of elements to be copied.</param>
		private static void IndexedCopyRange(
			T[] src, int srcIndex, IList<T> dst, int dstIndex, int length)
		{
			var s = src;
			{
				// fixed (...)
				while (length-- > 0) dst[dstIndex++] = s[srcIndex++];
			} // fixed (...)
		}

		/// <summary>Copies the range from one array to another.</summary>
		/// <param name="src">The source array.</param>
		/// <param name="srcIndex">Starting index in source array.</param>
		/// <param name="dst">The destination array.</param>
		/// <param name="dstIndex">Starting index in destination array.</param>
		/// <param name="length">Number of elements to be copied.</param>
		private static void IndexedCopyRange(
			IList<T> src, int srcIndex, T[] dst, int dstIndex, int length)
		{
			var d = dst;
			{
				// fixed (...)
				while (length-- > 0) d[dstIndex++] = src[srcIndex++];
			} // fixed (...)
		}

		/// <summary>Reverse the specified range of the specified array.</summary>
		/// <param name="array">the array in which a range is to be reversed.</param>
		/// <param name="lo">the index of the first element in the range to be reversed.</param>
		/// <param name="hi">the index after the last element in the range to be reversed.</param>
		private static void IndexedReverseRange(IList<T> array, int lo, int hi)
		{
			hi--;
			while (lo < hi)
			{
				var t = array[lo];
				array[lo++] = array[hi];
				array[hi--] = t;
			}
		}

		/// <summary>
		/// Merges the two runs at stack indices i and i+1.  Run i must be the penultimate 
		/// or antepenultimate run on the stack. In other words, i must be equal to 
		/// stackSize-2 or stackSize-3.
		/// </summary>
		/// <param name="runIndex">Stack index of the first of the two runs to merge.</param>
		protected override void MergeAt(int runIndex)
		{
			Debug.Assert(_stackSize >= 2);
			Debug.Assert(runIndex >= 0);
			Debug.Assert(runIndex == _stackSize - 2 || runIndex == _stackSize - 3);

			var comparer = _comparer;
			var base1 = _runBase[runIndex];
			var len1 = _runLength[runIndex];
			var base2 = _runBase[runIndex + 1];
			var len2 = _runLength[runIndex + 1];
			Debug.Assert(len1 > 0 && len2 > 0);
			Debug.Assert(base1 + len1 == base2);

			// Record the length of the combined runs; if i is the 3rd-last
			// run now, also slide over the last run (which isn't involved
			// in this merge). The current run (i+1) goes away in any case.
			_runLength[runIndex] = len1 + len2;
			if (runIndex == _stackSize - 3)
			{
				_runBase[runIndex + 1] = _runBase[runIndex + 2];
				_runLength[runIndex + 1] = _runLength[runIndex + 2];
			}

			_stackSize--;

			// Find where the first element of run2 goes in run1. Prior elements
			// in run1 can be ignored (because they're already in place).
			var k = GallopRight(_array[base2], _array, base1, len1, 0, comparer);
			Debug.Assert(k >= 0);
			base1 += k;
			len1 -= k;
			if (len1 == 0) return;

			// Find where the last element of run1 goes in run2. Subsequent elements
			// in run2 can be ignored (because they're already in place).
			len2 = GallopLeft(_array[base1 + len1 - 1], _array, base2, len2, len2 - 1, comparer);
			Debug.Assert(len2 >= 0);
			if (len2 == 0) return;

			// Merge remaining runs, using tmp array with min(len1, len2) elements
			if (len1 <= len2)
				MergeLo(base1, len1, base2, len2);
			else
				MergeHi(base1, len1, base2, len2);
		}

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
		internal static int GallopLeft(
			T key, IList<T> array, int lo, int length, int hint, Comparison<T> comparer)
		{
			var a = array;
			{
				// fixed (...)
				Debug.Assert(length > 0 && hint >= 0 && hint < length);
				var lastOfs = 0;
				var ofs = 1;

				if (comparer(key, a[lo + hint]) > 0) // comparer(key, a[lo + hint]) > 0
				{
					// Gallop right until a[base+hint+lastOfs] < key <= a[base+hint+ofs]
					var maxOfs = length - hint;
					while (ofs < maxOfs && comparer(
						key, a[lo + hint + ofs]) > 0) // comparer(key, a[lo + hint + ofs]) > 0
					{
						lastOfs = ofs;
						ofs = (ofs << 1) + 1;
						if (ofs <= 0) // int overflow
							ofs = maxOfs;
					}

					if (ofs > maxOfs)
						ofs = maxOfs;

					// Make offsets relative to base
					lastOfs += hint;
					ofs += hint;
				}
				else // if (key <= a[base + hint])
				{
					// Gallop left until a[base+hint-ofs] < key <= a[base+hint-lastOfs]
					var maxOfs = hint + 1;
					while (ofs < maxOfs && comparer(
						key, a[lo + hint - ofs]) <= 0) // comparer(key, a[lo + hint - ofs]) <= 0
					{
						lastOfs = ofs;
						ofs = (ofs << 1) + 1;
						if (ofs <= 0) // int overflow
							ofs = maxOfs;
					}

					if (ofs > maxOfs)
						ofs = maxOfs;

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

					if (comparer(key, a[lo + m]) > 0) // comparer(key, a[lo + m]) > 0
						lastOfs = m + 1; // a[base + m] < key
					else
						ofs = m; // key <= a[base + m]
				}

				Debug.Assert(lastOfs == ofs); // so a[base + ofs - 1] < key <= a[base + ofs]
				return ofs;
			} // fixed (...)
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
		internal static int GallopRight(
			T key, IList<T> array, int lo, int length, int hint, Comparison<T> comparer)
		{
			var a = array;
			{
				Debug.Assert(length > 0 && hint >= 0 && hint < length);

				var ofs = 1;
				var lastOfs = 0;
				if (comparer(key, a[lo + hint]) < 0) // comparer(key, a[lo + hint]) < 0
				{
					// Gallop left until a[b+hint - ofs] <= key < a[b+hint - lastOfs]
					var maxOfs = hint + 1;
					while (ofs < maxOfs && comparer(key, a[lo + hint - ofs]) < 0)
					{
						lastOfs = ofs;
						ofs = (ofs << 1) + 1;
						if (ofs <= 0) // int overflow
							ofs = maxOfs;
					}

					if (ofs > maxOfs)
						ofs = maxOfs;

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
					while (ofs < maxOfs && comparer(key, a[lo + hint + ofs]) >= 0)
					{
						lastOfs = ofs;
						ofs = (ofs << 1) + 1;
						if (ofs <= 0) // int overflow
							ofs = maxOfs;
					}

					if (ofs > maxOfs)
						ofs = maxOfs;

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

					if (comparer(key, a[lo + m]) < 0)
						ofs = m; // key < a[b + m]
					else
						lastOfs = m + 1; // a[b + m] <= key
				}

				Debug.Assert(lastOfs == ofs); // so a[b + ofs - 1] <= key < a[b + ofs]
				return ofs;
			} // fixed (...)
		}

		/// <summary>
		/// Merges two adjacent runs in place, in a stable fashion. The first element of the first run must be greater than 
		/// the first element of the second run (a[base1] &gt; a[base2]), and the last element of the first run 
		/// (a[base1 + len1-1]) must be greater than all elements of the second run.
		/// For performance, this method should be called only when len1 &lt;= len2; its twin, mergeHi should be called if 
		/// len1 &gt;= len2. (Either method may be called if len1 == len2.)
		/// </summary>
		/// <param name="base1">index of first element in first run to be merged.</param>
		/// <param name="len1">length of first run to be merged (must be &gt; 0).</param>
		/// <param name="base2">index of first element in second run to be merged (must be aBase + aLen).</param>
		/// <param name="len2">length of second run to be merged (must be &gt; 0).</param>
		private void MergeLo(int base1, int len1, int base2, int len2)
		{
			Debug.Assert(len1 > 0 && len2 > 0 && base1 + len1 == base2);

			// Copy first run into temp array
			var array = _array;
			var mergeBuffer = EnsureCapacity(len1);

			var m = mergeBuffer;
			var a = array;
			{
				// fixed (...)
				IndexedCopyRange(a, base1, m, 0, len1);

				var cursor1 = 0; // Indexes into tmp array
				var cursor2 = base2; // Indexes int a
				var dest = base1; // Indexes int a

				// Move first element of second run and deal with degenerate cases
				a[dest++] = a[cursor2++];
				if (--len2 == 0)
				{
					IndexedCopyRange(m, cursor1, a, dest, len1);
					return;
				}

				if (len1 == 1)
				{
					IndexedCopyRange(a, cursor2, dest, len2);
					a[dest + len2] = m[cursor1]; // Last elt of run 1 to end of merge
					return;
				}

				var comparer = _comparer; // Use local variables for performance
				var minGallop = _minGallop;

				while (true)
				{
					var count1 = 0; // Number of times in a row that first run won
					var count2 = 0; // Number of times in a row that second run won

					// Do the straightforward thing until (if ever) one run starts
					// winning consistently.
					do
					{
						Debug.Assert(len1 > 1 && len2 > 0);
						if (comparer(a[cursor2], m[cursor1]) < 0) // c(a[cursor2], m[cursor1]) < 0
						{
							a[dest++] = a[cursor2++];
							count2++;
							count1 = 0;
							if (--len2 == 0)
								goto break_outer;
						}
						else
						{
							a[dest++] = m[cursor1++];
							count1++;
							count2 = 0;
							if (--len1 == 1)
								goto break_outer;
						}
					}
					while ((count1 | count2) < minGallop);

					// One run is winning so consistently that galloping may be a
					// huge win. So try that, and continue galloping until (if ever)
					// neither run appears to be winning consistently anymore.
					do
					{
						Debug.Assert(len1 > 1 && len2 > 0);
						count1 = AnyArrayTimSort<T>.GallopRight(
							a[cursor2], mergeBuffer, cursor1, len1, 0, comparer);
						if (count1 != 0)
						{
							IndexedCopyRange(m, cursor1, a, dest, count1);
							dest += count1;
							cursor1 += count1;
							len1 -= count1;
							if (len1 <= 1) // len1 == 1 || len1 == 0
								goto break_outer;
						}

						a[dest++] = a[cursor2++];
						if (--len2 == 0)
							goto break_outer;

						count2 = GallopLeft(m[cursor1], array, cursor2, len2, 0, comparer);
						if (count2 != 0)
						{
							IndexedCopyRange(a, cursor2, dest, count2);
							dest += count2;
							cursor2 += count2;
							len2 -= count2;
							if (len2 == 0)
								goto break_outer;
						}

						a[dest++] = m[cursor1++];
						if (--len1 == 1)
							goto break_outer;

						minGallop--;
					}
					while (count1 >= MIN_GALLOP | count2 >= MIN_GALLOP);

					if (minGallop < 0)
						minGallop = 0;
					minGallop += 2; // Penalize for leaving gallop mode
				} // End of "outer" loop

				break_outer: // goto me! ;)

				_minGallop = minGallop < 1 ? 1 : minGallop; // Write back to field

				if (len1 == 1)
				{
					Debug.Assert(len2 > 0);
					IndexedCopyRange(a, cursor2, dest, len2);
					a[dest + len2] = m[cursor1]; //  Last elt of run 1 to end of merge
				}
				else if (len1 == 0)
				{
					throw new ArgumentException("Comparison method violates its general contract!");
				}
				else
				{
					Debug.Assert(len2 == 0);
					Debug.Assert(len1 > 1);
					IndexedCopyRange(m, cursor1, a, dest, len1);
				}
			} // fixed (...)
		}

		/// <summary>
		/// Like mergeLo, except that this method should be called only if
		/// len1 &gt;= len2; mergeLo should be called if len1 &lt;= len2. (Either method may be called if len1 == len2.)
		/// </summary>
		/// <param name="base1">index of first element in first run to be merged.</param>
		/// <param name="len1">length of first run to be merged (must be &gt; 0).</param>
		/// <param name="base2">index of first element in second run to be merged (must be aBase + aLen).</param>
		/// <param name="len2">length of second run to be merged (must be &gt; 0).</param>
		private void MergeHi(int base1, int len1, int base2, int len2)
		{
			Debug.Assert(len1 > 0 && len2 > 0 && base1 + len1 == base2);

			// Copy second run into temp array
			var array = _array; // For performance
			var mergeBuffer = EnsureCapacity(len2);

			var m = mergeBuffer;
			var a = array;
			{
				// fixed (...)
				IndexedCopyRange(a, base2, m, 0, len2);

				var cursor1 = base1 + len1 - 1; // Indexes into a
				var cursor2 = len2 - 1; // Indexes into mergeBuffer array
				var dest = base2 + len2 - 1; // Indexes into a

				// Move last element of first run and deal with degenerate cases
				a[dest--] = a[cursor1--];
				if (--len1 == 0)
				{
					IndexedCopyRange(m, 0, a, dest - (len2 - 1), len2);
					return;
				}

				if (len2 == 1)
				{
					dest -= len1;
					cursor1 -= len1;
					IndexedCopyRange(a, cursor1 + 1, dest + 1, len1);
					a[dest] = m[cursor2];
					return;
				}

				var comparer = _comparer; // Use local variables for performance
				var minGallop = _minGallop;

				while (true)
				{
					var count1 = 0; // Number of times in a row that first run won
					var count2 = 0; // Number of times in a row that second run won

					// Do the straightforward thing until (if ever) one run appears to win consistently.
					do
					{
						Debug.Assert(len1 > 0 && len2 > 1);
						if (comparer(m[cursor2], a[cursor1]) < 0) // c(m[cursor2], a[cursor1]) < 0
						{
							a[dest--] = a[cursor1--];
							count1++;
							count2 = 0;
							if (--len1 == 0)
								goto break_outer;
						}
						else
						{
							a[dest--] = m[cursor2--];
							count2++;
							count1 = 0;
							if (--len2 == 1)
								goto break_outer;
						}
					}
					while ((count1 | count2) < minGallop);

					// One run is winning so consistently that galloping may be a
					// huge win. So try that, and continue galloping until (if ever)
					// neither run appears to be winning consistently anymore.
					do
					{
						Debug.Assert(len1 > 0 && len2 > 1);
						count1 = len1 - GallopRight(
							m[cursor2], array, base1, len1, len1 - 1, comparer);
						if (count1 != 0)
						{
							dest -= count1;
							cursor1 -= count1;
							len1 -= count1;
							IndexedCopyRange(a, cursor1 + 1, dest + 1, count1);
							if (len1 == 0)
								goto break_outer;
						}

						a[dest--] = m[cursor2--];
						if (--len2 == 1)
							goto break_outer;

						count2 = len2 - AnyArrayTimSort<T>.GallopLeft(
							a[cursor1], mergeBuffer, 0, len2, len2 - 1, comparer);
						if (count2 != 0)
						{
							dest -= count2;
							cursor2 -= count2;
							len2 -= count2;
							IndexedCopyRange(m, cursor2 + 1, a, dest + 1, count2);
							if (len2 <= 1) // len2 == 1 || len2 == 0
								goto break_outer;
						}

						a[dest--] = a[cursor1--];
						if (--len1 == 0)
							goto break_outer;

						minGallop--;
					}
					while (count1 >= MIN_GALLOP | count2 >= MIN_GALLOP);

					if (minGallop < 0)
						minGallop = 0;
					minGallop += 2; // Penalize for leaving gallop mode
				} // End of "outer" loop

				break_outer: // goto me! ;)

				_minGallop = minGallop < 1 ? 1 : minGallop; // Write back to field

				if (len2 == 1)
				{
					Debug.Assert(len1 > 0);
					dest -= len1;
					cursor1 -= len1;
					IndexedCopyRange(a, cursor1 + 1, dest + 1, len1);
					a[dest] = m[cursor2]; // Move first elt of run2 to front of merge
				}
				else if (len2 == 0)
				{
					throw new ArgumentException("Comparison method violates its general contract!");
				}
				else
				{
					Debug.Assert(len1 == 0);
					Debug.Assert(len2 > 0);
					IndexedCopyRange(m, 0, a, dest - (len2 - 1), len2);
				}
			} // fixed (...)
		}
	}
}
