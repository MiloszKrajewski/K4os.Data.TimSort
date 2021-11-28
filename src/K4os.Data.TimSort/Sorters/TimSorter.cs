using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;
using K4os.Data.TimSort.Internals;

namespace K4os.Data.TimSort.Sorters
{
	internal class TimSorter<T, TIndexer, TReference, TLessThan>:
		BasicSorter<T, TIndexer, TReference, TLessThan>
		where TIndexer: IIndexer<T, TReference>
		where TReference: struct, IReference<TReference>
		where TLessThan: ILessThan<T>
	{
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
		/// ArrayOutOfBounds exception.  See listsort.txt for a discussion
		/// of the minimum stack length required as a function of the length
		/// of the array being sorted and the minimum merge sequence length.
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

		/// <summary>The array being sorted.</summary>
		private readonly TIndexer _array;

		/// <summary>Cached length of array, it won't change.</summary>
		private readonly int _arrayLength;
		
		/// <summary>The comparator for this sort.</summary>
		private readonly TLessThan _comparer;

		/// <summary>
		/// This controls when we get *into* galloping mode.  It is initialized
		/// to MIN_GALLOP.  The mergeLo and mergeHi methods nudge it higher for
		/// random data, and lower for highly structured data.
		/// </summary>
		private int _minGallop = MIN_GALLOP;

		/// <summary>
		/// Temp storage for merges.
		/// </summary>
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

		private readonly TReference[] _runBase;
		private readonly int[] _runLength;

		/// <summary>Initializes a new instance of the
		/// <see cref="TimSorter{T,TIndexer,TReference,TComparer}"/> class.</summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayLength">Length of the array.</param>
		/// <param name="comparer">Comparer.</param>
		public TimSorter(TIndexer array, int arrayLength, TLessThan comparer)
		{
			_array = array;
			_arrayLength = arrayLength;
			_comparer = comparer;

			// Allocate temp storage (which may be increased later if necessary)
			var mergeBufferLength =
				arrayLength < 2 * INITIAL_TMP_STORAGE_LENGTH
					? arrayLength >> 1
					: INITIAL_TMP_STORAGE_LENGTH;
			_mergeBuffer = new T[mergeBufferLength];

			// Allocate runs-to-be-merged stack (which cannot be expanded).  The
			// stack length requirements are described in listsort.txt.  The C
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
			_runBase = new TReference[stackLength];
			_runLength = new int[stackLength];
		}
		
		/// <summary>
		/// Returns the minimum acceptable run length for an array of the specified length.
		/// Natural runs shorter than this will be extended with BinarySort.
		/// Roughly speaking, the computation is:
		/// If <c>n &lt; MIN_MERGE</c>, return n (it's too small to bother with fancy stuff).
		/// Else if n is an exact power of 2, return <c>MIN_MERGE/2</c>.
		/// Else return an int k, <c>MIN_MERGE/2 &lt;= k &lt;= MIN_MERGE</c>, such that <c>n/k</c>
		/// is close to, but strictly less than, an exact power of 2. For the rationale,
		/// see listsort.txt.
		/// </summary>
		/// <param name="n">the length of the array to be sorted.</param>
		/// <returns>the length of the minimum run to be merged.</returns>
		private static int GetMinimumRunLength(int n)
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
		/// Examines the stack of runs waiting to be merged and merges adjacent runs until the
		/// stack invariants are reestablished: 
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
				var rl = _runLength;
				
				if (n > 0 && rl[n - 1] <= rl[n] + rl[n + 1])
				{
					if (rl[n - 1] < rl[n + 1]) n--;
					MergeAt(n);
				}
				else if (rl[n] <= rl[n + 1])
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
				var rl = _runLength;
				if (n > 0 && rl[n - 1] < rl[n + 1]) n--;
				MergeAt(n);
			}
		}

		/// <summary>
		/// Pushes the specified run onto the pending-run stack.
		/// </summary>
		/// <param name="runBase">index of the first element in the run.</param>
		/// <param name="runLength">the number of elements in the run.</param>
		private void PushRun(TReference runBase, int runLength)
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

			newSize = newSize < 0 
				? minCapacity 
				: Math.Min(newSize, _arrayLength >> 1);

			return _mergeBuffer = new T[newSize];
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
			T key, TIndexer array, TReference lo, int length, int hint, TLessThan comparer)
		{
			Debug.Assert(length > 0 && hint >= 0 && hint < length);
			
			var a = array;
			var lastOfs = 0;
			var ofs = 1;

			if (comparer.Gt(key, a[lo.Add(hint)])) // comparer(key, a[lo + hint]) > 0
			{
				// Gallop right until a[base+hint+lastOfs] < key <= a[base+hint+ofs]
				var maxOfs = length - hint;
				while (ofs < maxOfs && comparer.Gt(key, a[lo.Add(hint + ofs)])) // comparer(key, a[lo + hint + ofs]) > 0
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
				while (ofs < maxOfs && comparer.LtEq(key, a[lo.Add(hint - ofs)])) // comparer(key, a[lo + hint - ofs]) <= 0
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

				if (comparer.Gt(key, a[lo.Add(m)])) // comparer(key, a[lo + m]) > 0
					lastOfs = m + 1; // a[base + m] < key
				else
					ofs = m; // key <= a[base + m]
			}

			Debug.Assert(lastOfs == ofs); // so a[base + ofs - 1] < key <= a[base + ofs]
			return ofs;
		}
		
		private static unsafe int GallopLeft(
			T key, T[] array, int lo, int length, int hint, TLessThan comparer)
		{
			fixed (void* ptr0 = &array.Ref0())
			{
				var indexer = new PtrIndexer<T>(ptr0);
				return TimSorter<T, PtrIndexer<T>, PtrReference<T>, TLessThan>
					.GallopLeft(key, indexer, indexer.Ref0.Add(lo), length, hint, comparer);
			}
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
			T key, TIndexer array, TReference lo, int length, int hint, TLessThan comparer)
		{
			Debug.Assert(length > 0 && hint >= 0 && hint < length);

			var a = array;
			var ofs = 1;
			var lastOfs = 0;
			if (comparer.Lt(key, a[lo.Add(hint)])) // comparer(key, a[lo + hint]) < 0
			{
				// Gallop left until a[b+hint - ofs] <= key < a[b+hint - lastOfs]
				var maxOfs = hint + 1;
				while (ofs < maxOfs && comparer.Lt(key, a[lo.Add(hint - ofs)]))
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
				while (ofs < maxOfs && comparer.GtEq(key, a[lo.Add(hint + ofs)]))
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

				if (comparer.Lt(key, a[lo.Add(m)]))
					ofs = m; // key < a[b + m]
				else
					lastOfs = m + 1; // a[b + m] <= key
			}

			Debug.Assert(lastOfs == ofs); // so a[b + ofs - 1] <= key < a[b + ofs]
			return ofs;
		}

		private static unsafe int GallopRight(
			T key, T[] array, int lo, int length, int hint, TLessThan comparer)
		{
			fixed (void* ptr0 = &array.Ref0())
			{
				var indexer = new PtrIndexer<T>(ptr0);
				return TimSorter<T, PtrIndexer<T>, PtrReference<T>, TLessThan>
					.GallopRight(key, indexer, indexer.Ref0.Add(lo), length, hint, comparer);
			}
		}

		/// <summary>Sorts the specified array.</summary>
		/// <param name="array">Array to be sorted.</param>
		/// <param name="lo">the index of the first element in the range to be sorted.</param>
		/// <param name="hi">the index after the last element in the range to be sorted.</param>
		/// <param name="comparer">The comparator to determine the order of the sort.</param>
		public static void Sort(
			TIndexer array, TReference lo, TReference hi, TLessThan comparer)
		{
			var width = hi.Dif(lo);
			if (width < 2) 
				return; // Arrays of size 0 and 1 are always sorted

			// If array is small, do a "mini-TimSort" with no merges
			if (width < MIN_MERGE)
			{
				switch (width)
				{
					case 2: 
						Sort2(array, lo, comparer);
						return;
					case 3:
						Sort3(array, lo, comparer);
						return;
					default: {
						BinarySort(array, lo, hi, comparer);
						return;
					}
				}
			}

			// March over the array once, left to right, finding natural runs,
			// extending short natural runs to minRun elements, and merging runs
			// to maintain stack invariant.
			var sorter = new TimSorter<T, TIndexer, TReference, TLessThan>(
				array, hi.Dif(lo), comparer);
			var minRun = GetMinimumRunLength(width);
			
			do
			{
				// Identify next run
				var runLen = CountRunAndMakeAscending(array, lo, hi, comparer);

				// If run is short, extend to min(minRun, nRemaining)
				if (runLen < minRun)
				{
					var force = width <= minRun ? width : minRun;
					BinarySort(array, lo, lo.Add(force), lo.Add(runLen), comparer);
					runLen = force;
				}

				// Push run onto pending-run stack, and maybe merge
				sorter.PushRun(lo, runLen);
				sorter.MergeCollapse();

				// Advance to find next run
				lo = lo.Add(runLen);
				width -= runLen;
			}
			while (width != 0);

			// Merge all remaining runs to complete sort
			Debug.Assert(lo.Eq(hi));
			sorter.MergeForceCollapse();
			Debug.Assert(sorter._stackSize == 1);
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
		private void MergeLo(TReference base1, int len1, TReference base2, int len2)
		{
			Debug.Assert(len1 > 0 && len2 > 0 && base1.Add(len1).Eq(base2));

			// Copy first run into temp array
			var a = _array;
			var m = EnsureCapacity(len1);

			CopyRange(a, base1, m, 0, len1);

			var cursor1 = 0; // Indexes into tmp array
			var cursor2 = base2; // Indexes int a
			var dest = base1; // Indexes int a

			// Move first element of second run and deal with degenerate cases
			a[dest.PostInc()] = a[cursor2.PostInc()];
			if (--len2 == 0)
			{
				CopyRange(m, cursor1, a, dest, len1);
				return;
			}

			if (len1 == 1)
			{
				CopyRange(a, cursor2, dest, len2);
				a[dest.Add(len2)] = m[cursor1]; // Last elt of run 1 to end of merge
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
					if (comparer.Lt(a[cursor2], m[cursor1])) // c(a[cursor2], m[cursor1]) < 0
					{
						a[dest.PostInc()] = a[cursor2.PostInc()];
						count2++;
						count1 = 0;
						if (--len2 == 0)
							goto break_outer;
					}
					else
					{
						a[dest.PostInc()] = m[cursor1++];
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
					count1 = GallopRight(a[cursor2], EnsureCapacity(len1), cursor1, len1, 0, comparer);
					if (count1 != 0)
					{
						CopyRange(m, cursor1, a, dest, count1);
						dest = dest.Add(count1);
						cursor1 += count1;
						len1 -= count1;
						if (len1 <= 1) // len1 == 1 || len1 == 0
							goto break_outer;
					}

					a[dest.PostInc()] = a[cursor2.PostInc()];
					if (--len2 == 0)
						goto break_outer;

					count2 = GallopLeft(m[cursor1], a, cursor2, len2, 0, comparer);
					if (count2 != 0)
					{
						CopyRange(a, cursor2, dest, count2);
						dest = dest.Add(count2);
						cursor2 = cursor2.Add(count2);
						len2 -= count2;
						if (len2 == 0)
							goto break_outer;
					}

					a[dest.PostInc()] = m[cursor1++];
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
				CopyRange(a, cursor2, dest, len2);
				a[dest.Add(len2)] = m[cursor1]; //  Last elt of run 1 to end of merge
			}
			else if (len1 == 0)
			{
				throw new ArgumentException("Comparison method violates its general contract!");
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
		/// len1 &gt;= len2; mergeLo should be called if len1 &lt;= len2. (Either method may be called if len1 == len2.)
		/// </summary>
		/// <param name="base1">index of first element in first run to be merged.</param>
		/// <param name="len1">length of first run to be merged (must be &gt; 0).</param>
		/// <param name="base2">index of first element in second run to be merged (must be aBase + aLen).</param>
		/// <param name="len2">length of second run to be merged (must be &gt; 0).</param>
		private void MergeHi(TReference base1, int len1, TReference base2, int len2)
		{
			Debug.Assert(len1 > 0 && len2 > 0 && base1.Add(len1).Eq(base2));

			// Copy second run into temp array

			var a = _array;
			var m = EnsureCapacity(len2);

			CopyRange(a, base2, m, 0, len2);

			var cursor1 = base1.Add(len1 - 1); // Indexes into a
			var cursor2 = len2 - 1; // Indexes into mergeBuffer array
			var dest = base2.Add(len2 - 1); // Indexes into a

			// Move last element of first run and deal with degenerate cases
			a[dest.PostDec()] = a[cursor1.PostDec()];
			if (--len1 == 0)
			{
				CopyRange(m, 0, a, dest.Sub(len2 - 1), len2);
				return;
			}

			if (len2 == 1)
			{
				dest = dest.Sub(len1);
				cursor1 = cursor1.Sub(len1);
				CopyRange(a, cursor1.Inc(), dest.Inc(), len1);
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
					if (comparer.Lt(m[cursor2], a[cursor1])) // c(m[cursor2], a[cursor1]) < 0
					{
						a[dest.PostDec()] = a[cursor1.PostDec()];
						count1++;
						count2 = 0;
						if (--len1 == 0)
							goto break_outer;
					}
					else
					{
						a[dest.PostDec()] = m[cursor2--];
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
						m[cursor2], _array, base1, len1, len1 - 1, comparer);
					if (count1 != 0)
					{
						dest = dest.Sub(count1);
						cursor1 = cursor1.Sub(count1);
						len1 -= count1;
						CopyRange(a, cursor1.Inc(), dest.Inc(), count1);
						if (len1 == 0)
							goto break_outer;
					}

					a[dest.PostDec()] = m[cursor2--];
					if (--len2 == 1)
						goto break_outer;

					count2 = len2 - GallopLeft(
						a[cursor1], EnsureCapacity(len2), 0, len2, len2 - 1, comparer);
					if (count2 != 0)
					{
						dest = dest.Sub(count2);
						cursor2 -= count2;
						len2 -= count2;
						CopyRange(m, cursor2 + 1, a, dest.Inc(), count2);
						if (len2 <= 1) // len2 == 1 || len2 == 0
							goto break_outer;
					}

					a[dest.PostDec()] = a[cursor1.PostDec()];
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
				dest = dest.Sub(len1);
				cursor1 = cursor1.Sub(len1);
				CopyRange(a, cursor1.Inc(), dest.Inc(), len1);
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
				CopyRange(m, 0, a, dest.Sub(len2 - 1), len2);
			}
		}

		/// <summary>
		/// Merges the two runs at stack indices i and i+1. 
		/// Run i must be the penultimate or antepenultimate run on the stack. 
		/// In other words, i must be equal to stackSize-2 or stackSize-3.
		/// </summary>
		/// <param name="runIndex">stack index of the first of the two runs to merge.</param>
		protected void MergeAt(int runIndex)
		{
			Debug.Assert(_stackSize >= 2);
			Debug.Assert(runIndex >= 0);
			Debug.Assert(runIndex == _stackSize - 2 || runIndex == _stackSize - 3);

			var a = _array;
			var comparer = _comparer;
			var base1 = _runBase[runIndex];
			var len1 = _runLength[runIndex];
			var base2 = _runBase[runIndex + 1];
			var len2 = _runLength[runIndex + 1];
			Debug.Assert(len1 > 0 && len2 > 0);
			Debug.Assert(base1.Add(len1).Eq(base2));

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
			var k = GallopRight(a[base2], a, base1, len1, 0, comparer);
			Debug.Assert(k >= 0);
			base1 = base1.Add(k);
			len1 -= k;
			if (len1 == 0) return;

			// Find where the last element of run1 goes in run2. Subsequent elements
			// in run2 can be ignored (because they're already in place).
			len2 = GallopLeft(a[base1.Add(len1 - 1)], a, base2, len2, len2 - 1, comparer);
			Debug.Assert(len2 >= 0);
			if (len2 == 0) return;

			// Merge remaining runs, using tmp array with min(len1, len2) elements
			if (len1 <= len2)
				MergeLo(base1, len1, base2, len2);
			else
				MergeHi(base1, len1, base2, len2);
		}
	}
}
