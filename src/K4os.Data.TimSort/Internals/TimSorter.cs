#region Java implementation
/*
Author: Josh Bloch

A stable, adaptive, iterative mergesort that requires far fewer than
n lg(n) comparisons when running on partially sorted arrays, while
offering performance comparable to a traditional mergesort when run
on random arrays.  Like all proper merge-sorts, this sort is stable and
runs O(n log n) time (worst case).  In the worst case, this sort requires
temporary storage space for n/2 object references; in the best case,
it requires only a small constant amount of space.

This implementation was adapted from Tim Peters's list sort for
Python, which is described in detail here:
http://svn.python.org/projects/python/trunk/Objects/listsort.txt

Tim's C code may be found here:
http://svn.python.org/projects/python/trunk/Objects/listobject.c

The underlying techniques are described in this paper (and may have
even earlier origins):

"Optimistic Sorting and Information Theoretic Complexity"
Peter McIlroy
SODA (Fourth Annual ACM-SIAM Symposium on Discrete Algorithms),
pp 467-474, Austin, Texas, 25-27 January 1993.

While the API to this class consists solely of static methods, it is
(privately) instantiable; a TimSort instance holds the state of an ongoing
sort, assuming the input array is large enough to warrant the full-blown
TimSort. Small arrays are sorted in place, using a binary insertion sort.
*/
#endregion

#region C# implementation
/*
Author: Milosz Krajewski

This implementation was adapted from Josh Bloch array sort for Java which can be found here:
http://gee.cs.oswego.edu/cgi-bin/viewcvs.cgi/jsr166/src/main/java/util/TimSort.java?revision=1.5&view=markup

All modifications are licensed using Apache License (same as original Java implementation)
*/
#endregion

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals
{
	internal class TimSorter<T, TIndexer, TComparer>: TimSorter<T>
		where TIndexer: ITimIndexer<T>
		where TComparer: ITimComparer<T>
	{
		#region fields

		/// <summary>The array being sorted.</summary>
		private readonly TIndexer _array;

		/// <summary>Comparer used for sorting.</summary>
		private readonly TComparer _comparer;

		/// <summary>Cached length of array, it won't change.</summary>
		private readonly int _arrayLength;

		/// <summary>
		/// This controls when we get *into* galloping mode.  It is initialized
		/// to MIN_GALLOP.  The mergeLo and mergeHi methods nudge it higher for
		/// random data, and lower for highly structured data.
		/// </summary>
		private int _minGallop = MIN_GALLOP;

		/// <summary>Temp storage for merges.</summary>
		private T[] _mergeBuffer;

		/// <summary>
		/// A stack of pending runs yet to be merged.  Run i starts at
		/// address base[i] and extends for len[i] elements.  It's always
		/// true (so long as the indices are in bounds) that:
		/// <c>runBase[i] + runLen[i] == runBase[i + 1]</c>
		/// so we could cut the storage for this, but it's a minor amount,
		/// and keeping all the info explicit simplifies the code.
		/// </summary>
		private int _stackSize; // = 0; // Number of pending runs on stack

		/// <summary>Run stack, see <see cref="_stackSize"/></summary>
		private readonly int[] _runBase;
		
		/// <summary>Run stack, see <see cref="_stackSize"/></summary>
		private readonly int[] _runLength;
		
		#endregion

		#region constructor

		/// <summary>Initializes a new instance of the
		/// <see cref="TimSorter{T,TIndexer,TComparer}"/> class.</summary>
		/// <param name="array">The array.</param>
		/// <param name="comparer">The comparer.</param>
		/// <param name="arrayLength">Length of the array.</param>
		internal TimSorter(TIndexer array, TComparer comparer, int arrayLength)
		{
			_array = array;
			_comparer = comparer;
			_arrayLength = arrayLength;

			// Allocate temp storage (which may be increased later if necessary)
			var mergeBufferLength =
				arrayLength < 2 * INITIAL_TMP_STORAGE_LENGTH
					? arrayLength >> 1
					: INITIAL_TMP_STORAGE_LENGTH;
			_mergeBuffer = new T[mergeBufferLength];

			// Allocate runs-to-be-merged stack (which cannot be expanded).  The
			// stack length requirements are described in ALGORITHM.txt.  The C
			// version always uses the same stack length (85), but this was
			// measured to be too expensive when sorting "mid-sized" arrays (e.g.,
			// 100 elements) in Java.  Therefore, we use smaller (but sufficiently
			// large) stack lengths for smaller arrays.  The "magic numbers" in the
			// computation below must be changed if MIN_MERGE is decreased.  See
			// the MIN_MERGE declaration above for more information.
			var stackLength =
				arrayLength < 120 ? 5 :
				arrayLength < 1542 ? 10 :
				arrayLength < 119151 ? 19 :
				40;
			_runBase = new int[stackLength];
			_runLength = new int[stackLength];
		}

		#endregion
		
		#region implementation

		internal void MergeSort(TIndexer array, int lo, int hi, TComparer comparer)
		{
			var width = hi - lo;
			var minRun = GetMinimumRunLength(width);
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
				PushRun(lo, runLen);
				MergeCollapse();

				// Advance to find next run
				lo += runLen;
				width -= runLen;
			}
			while (width != 0);

			// Merge all remaining runs to complete sort
			Debug.Assert(lo == hi);
			MergeForceCollapse();
			Debug.Assert(_stackSize == 1);
		}

		/// <summary>
		/// Merges the two runs at stack indices i and i+1. 
		/// Run i must be the penultimate or antepenultimate run on the stack. 
		/// In other words, i must be equal to stackSize-2 or stackSize-3.
		/// </summary>
		/// <param name="runIndex">stack index of the first of the two runs to merge.</param>
		private void MergeAt(int runIndex)
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
		/// Examines the stack of runs waiting to be merged and merges adjacent runs until the stack invariants are
		/// reestablished: 
		/// <c><![CDATA[1. runLen[i - 3] > runLen[i - 2] + runLen[i - 1] ]]></c> and 
		/// <c><![CDATA[2. runLen[i - 2] > runLen[i - 1] ]]></c>
		/// This method is called each time a new run is pushed onto the stack,
		/// so the invariants are guaranteed to hold for i &lt; stackSize upon
		/// entry to the method.
		/// </summary>
		private void MergeCollapse()
		{
			while (_stackSize > 1)
			{
				var n = _stackSize - 2;

				if (n > 0 && _runLength[n - 1] <= _runLength[n] + _runLength[n + 1])
				{
					if (_runLength[n - 1] < _runLength[n + 1]) n--;
					MergeAt(n);
				}
				else if (_runLength[n] <= _runLength[n + 1])
				{
					MergeAt(n);
				}
				else
				{
					break; // Invariant is established
				}
			}
		}

		/// <summary>
		/// Merges all runs on the stack until only one remains.  This method is called once, to complete the sort.
		/// </summary>
		private void MergeForceCollapse()
		{
			while (_stackSize > 1)
			{
				var n = _stackSize - 2;
				if (n > 0 && _runLength[n - 1] < _runLength[n + 1]) n--;
				MergeAt(n);
			}
		}

		/// <summary>
		/// Pushes the specified run onto the pending-run stack.
		/// </summary>
		/// <param name="runBase">index of the first element in the run.</param>
		/// <param name="runLength">the number of elements in the run.</param>
		private void PushRun(int runBase, int runLength)
		{
			_runBase[_stackSize] = runBase;
			_runLength[_stackSize] = runLength;
			_stackSize++;
		}

		/// <summary>
		/// Ensures that the external array tmp has at least the specified
		/// number of elements, increasing its size if necessary.  The size
		/// increases exponentially to ensure amortized linear time complexity.
		/// </summary>
		/// <param name="minCapacity">the minimum required capacity of the tmp array.</param>
		/// <returns>tmp, whether or not it grew</returns>
		private T[] EnsureCapacity(int minCapacity)
		{
			if (_mergeBuffer.Length >= minCapacity)
				return _mergeBuffer;

			// Compute smallest power of 2 > minCapacity
			var newSize = minCapacity;
			newSize |= newSize >> 1;
			newSize |= newSize >> 2;
			newSize |= newSize >> 4;
			newSize |= newSize >> 8;
			newSize |= newSize >> 16;
			newSize++;

			newSize = newSize < 0 ? minCapacity : Math.Min(newSize, _arrayLength >> 1);

			_mergeBuffer = new T[newSize];

			return _mergeBuffer;
		}

		/// <summary>
		/// Merges two adjacent runs in place, in a stable fashion.
		/// The first element of the first run must be greater than the first element of the
		/// second run (a[base1] &gt; a[base2]), and the last element of the first run 
		/// (a[base1 + len1-1]) must be greater than all elements of the second run.
		/// For performance, this method should be called only when len1 &lt;= len2; its twin,
		/// mergeHi should be called if len1 &gt;= len2.
		/// (Either method may be called if len1 == len2.)
		/// </summary>
		/// <param name="base1">index of first element in first run to be merged.</param>
		/// <param name="len1">length of first run to be merged (must be &gt; 0).</param>
		/// <param name="base2">index of first element in second run to be merged (must be aBase + aLen).</param>
		/// <param name="len2">length of second run to be merged (must be &gt; 0).</param>
		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		private void MergeLo(int base1, int len1, int base2, int len2)
		{
			Debug.Assert(len1 > 0 && len2 > 0 && base1 + len1 == base2);

			// Copy first run into temp array
			var a = _array;
			var m = EnsureCapacity(len1);

			CopyRange(a, base1, m, 0, len1);

			var cursor1 = 0; // Indexes into tmp array
			var cursor2 = base2; // Indexes int a
			var dest = base1; // Indexes int a

			// Move first element of second run and deal with degenerate cases
			a[dest++] = a[cursor2++];
			if (--len2 == 0)
			{
				CopyRange(m, cursor1, a, dest, len1);
				return;
			}

			if (len1 == 1)
			{
				CopyRange(a, cursor2, dest, len2);
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
					if (comparer.Lt(a[cursor2], m[cursor1]))
					{
						a[dest++] = a[cursor2++];
						count2++;
						count1 = 0;
						if (--len2 == 0) goto break_outer;
					}
					else
					{
						a[dest++] = m[cursor1++];
						count1++;
						count2 = 0;
						if (--len1 == 1) goto break_outer;
					}
				}
				while ((count1 | count2) < minGallop);

				// One run is winning so consistently that galloping may be a
				// huge win. So try that, and continue galloping until (if ever)
				// neither run appears to be winning consistently anymore.
				do
				{
					Debug.Assert(len1 > 1 && len2 > 0);
					count1 = GallopRight(a[cursor2], m, cursor1, len1, 0, comparer);
					if (count1 != 0)
					{
						CopyRange(m, cursor1, a, dest, count1);
						dest += count1;
						cursor1 += count1;
						len1 -= count1;
						if (len1 <= 1) // len1 == 1 || len1 == 0
							goto break_outer;
					}

					a[dest++] = a[cursor2++];
					if (--len2 == 0) goto break_outer;

					count2 = GallopLeft(m[cursor1], a, cursor2, len2, 0, comparer);
					if (count2 != 0)
					{
						CopyRange(a, cursor2, dest, count2);
						dest += count2;
						cursor2 += count2;
						len2 -= count2;
						if (len2 == 0) goto break_outer;
					}

					a[dest++] = m[cursor1++];
					if (--len1 == 1) goto break_outer;

					minGallop--;
				}
				while (count1 >= MIN_GALLOP | count2 >= MIN_GALLOP);

				if (minGallop < 0) minGallop = 0;
				minGallop += 2; // Penalize for leaving gallop mode
			} // End of "outer" loop

			break_outer: // goto me! ;)

			_minGallop = minGallop < 1 ? 1 : minGallop; // Write back to field

			if (len1 == 1)
			{
				Debug.Assert(len2 > 0);
				CopyRange(a, cursor2, dest, len2);
				a[dest + len2] = m[cursor1]; //  Last elt of run 1 to end of merge
			}
			else if (len1 == 0)
			{
				throw CorruptedComparer();
			}
			else
			{
				Debug.Assert(len2 == 0);
				Debug.Assert(len1 > 1);
				CopyRange(m, cursor1, a, dest, len1);
			}
		}

		/// <summary>
		/// Like mergeLo, except that this method should be called only if
		/// len1 &gt;= len2; mergeLo should be called if len1 &lt;= len2.
		/// (Either method may be called if len1 == len2.)
		/// </summary>
		/// <param name="base1">index of first element in first run to be merged.</param>
		/// <param name="len1">length of first run to be merged (must be &gt; 0).</param>
		/// <param name="base2">index of first element in second run to be merged (must be aBase + aLen).</param>
		/// <param name="len2">length of second run to be merged (must be &gt; 0).</param>
		#if NET5_0
		[MethodImpl(MethodImplOptions.AggressiveOptimization)]
		#endif
		private void MergeHi(int base1, int len1, int base2, int len2)
		{
			Debug.Assert(len1 > 0 && len2 > 0 && base1 + len1 == base2);

			var a = _array;
			var m = EnsureCapacity(len2);

			CopyRange(a, base2, m, 0, len2);

			var cursor1 = base1 + len1 - 1; // Indexes into a
			var cursor2 = len2 - 1; // Indexes into mergeBuffer array
			var dest = base2 + len2 - 1; // Indexes into a

			// Move last element of first run and deal with degenerate cases
			a[dest--] = a[cursor1--];
			if (--len1 == 0)
			{
				CopyRange(m, 0, a, dest - (len2 - 1), len2);
				return;
			}

			if (len2 == 1)
			{
				dest -= len1;
				cursor1 -= len1;
				CopyRange(a, cursor1 + 1, dest + 1, len1);
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
					if (comparer.Lt(m[cursor2], a[cursor1]))
					{
						a[dest--] = a[cursor1--];
						count1++;
						count2 = 0;
						if (--len1 == 0) goto break_outer;
					}
					else
					{
						a[dest--] = m[cursor2--];
						count2++;
						count1 = 0;
						if (--len2 == 1) goto break_outer;
					}
				}
				while ((count1 | count2) < minGallop);

				// One run is winning so consistently that galloping may be a
				// huge win. So try that, and continue galloping until (if ever)
				// neither run appears to be winning consistently anymore.
				do
				{
					Debug.Assert(len1 > 0 && len2 > 1);
					count1 = len1 - GallopRight(m[cursor2], a, base1, len1, len1 - 1, comparer);
					if (count1 != 0)
					{
						dest -= count1;
						cursor1 -= count1;
						len1 -= count1;
						CopyRange(a, cursor1 + 1, dest + 1, count1);
						if (len1 == 0) goto break_outer;
					}

					a[dest--] = m[cursor2--];
					if (--len2 == 1)
						goto break_outer;

					count2 = len2 - GallopLeft(a[cursor1], EnsureCapacity(len2), 0, len2, len2 - 1, comparer);
					if (count2 != 0)
					{
						dest -= count2;
						cursor2 -= count2;
						len2 -= count2;
						CopyRange(m, cursor2 + 1, a, dest + 1, count2);
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
				CopyRange(a, cursor1 + 1, dest + 1, len1);
				a[dest] = m[cursor2]; // Move first elt of run2 to front of merge
			}
			else if (len2 == 0)
			{
				throw CorruptedComparer();
			}
			else
			{
				Debug.Assert(len1 == 0);
				Debug.Assert(len2 > 0);
				CopyRange(m, 0, a, dest - (len2 - 1), len2);
			}
		}
		
		#endregion
	}
}
