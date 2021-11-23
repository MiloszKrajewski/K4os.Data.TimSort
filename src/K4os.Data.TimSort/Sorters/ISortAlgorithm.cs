using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;
using K4os.Data.TimSort.Internals;

namespace K4os.Data.TimSort.Sorters
{
	public interface ISortAlgorithm
	{
		void Sort<T, TIndexer, TReference, TLessThan>(
			TIndexer array, TReference lo, TReference hi, TLessThan comparer)
			where TIndexer: IIndexer<T, TReference>
			where TReference: struct, IReference<TReference>
			where TLessThan: ILessThan<T>;
	}

	public static partial class SortAlgorithmExtensions
	{
		#region comparable

		// this method is used through reflection
		internal static void ComparableSort<T, TSorter, TIndexer, TReference>(
			this TSorter sorter, TIndexer indexer, TReference lo, TReference hi)
			where T: IComparable<T>
			where TSorter: ISortAlgorithm
			where TIndexer: IIndexer<T, TReference>
			where TReference: struct, IReference<TReference> =>
			sorter.Sort<T, TIndexer, TReference, ComparableLessThan<T>>(indexer, lo, hi, default);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void ComparableSortProxy<T, TSorter, TIndexer, TReference>(
			TSorter sorter, TIndexer indexer, TReference lo, TReference hi)
			where TSorter: ISortAlgorithm where TIndexer: IIndexer<T, TReference>
			where TReference: struct, IReference<TReference>
		{
			ComparableSortHelper<T, TSorter, TIndexer, TReference>.Sort(
				sorter, indexer, lo, hi);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SortDefaultOrder<T, TSorter, TIndexer, TReference>(
			this TSorter sorter, TIndexer indexer, TReference lo, TReference hi)
			where TSorter: ISortAlgorithm
			where TIndexer: IIndexer<T, TReference>
			where TReference: struct, IReference<TReference>
		{
			if (DefaultsToComparable<T>())
				ComparableSortProxy<T, TSorter, TIndexer, TReference>(
					sorter, indexer, lo, hi);
			else
				sorter.Sort<T, TIndexer, TReference, DefaultLessThan<T>>(
					indexer, lo, hi, default);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool DefaultsToComparable<T>() =>
			!default(DefaultLessThan<T>).IsNative() &&
			typeof(T).IsAssignableTo(typeof(IComparable<T>));

		#endregion

		#region baseline

		private static bool ValidateBounds<T>(Span<T> array)
		{
			if (array == null)
				throw new ArgumentNullException(
					nameof(array), "Given Span<T> is null");

			return array.Length > 1;
		}

		[SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
		private static bool ValidateBounds<T>(IList<T> array, int offset, int length)
		{
			if (offset < 0)
				throw new ArgumentOutOfRangeException(
					nameof(offset), "Given Offset is less than 0");
			if (length < 0)
				throw new ArgumentOutOfRangeException(
					nameof(length), "Given Length is less than 0");
			if (offset + length > array.Count)
				throw new ArgumentException(
					"Offset and/or Length are invalid");

			return length > 1;
		}

		public static unsafe void Sort<T, TSorter, TLessThan>(
			this TSorter sorter, Span<T> array, TLessThan comparer)
			where TSorter: ISortAlgorithm
			where TLessThan: ILessThan<T>
		{
			if (!ValidateBounds(array))
				return;

			fixed (void* ptr0 = &Unsafe.As<T, byte>(ref array[0]))
			{
				var indexer = new PtrIndexer<T>(ptr0);
				sorter.Sort<T, PtrIndexer<T>, PtrReference<T>, TLessThan>(
					indexer, indexer.Ref0, indexer.Ref0.Add(array.Length), comparer);
			}
		}

		public static unsafe void Sort<T, TSorter>(
			this TSorter sorter, Span<T> array)
			where TSorter: ISortAlgorithm
		{
			if (!ValidateBounds(array))
				return;

			fixed (void* ptr0 = &Unsafe.As<T, byte>(ref array[0]))
			{
				var indexer = new PtrIndexer<T>(ptr0);
				sorter.SortDefaultOrder<T, TSorter, PtrIndexer<T>, PtrReference<T>>(
					indexer, indexer.Ref0, indexer.Ref0.Add(array.Length));
			}
		}

		public static void Sort<T, TSorter, TLessThan>(
			this TSorter sorter, IList<T> array, int offset, int length, TLessThan comparer)
			where TSorter: ISortAlgorithm
			where TLessThan: ILessThan<T>
		{
			if (array.TryAsSpan(offset, length, out var span))
			{
				Sort(sorter, span, comparer);
				return;
			}

			if (!ValidateBounds(array, offset, length))
				return;

			var indexer = new ListIndexer<T>(array);
			sorter.Sort<T, ListIndexer<T>, IntReference, TLessThan>(
				indexer, offset, offset + length, comparer);
		}

		public static void Sort<T, TSorter>(
			this TSorter sorter, IList<T> array, int offset, int length)
			where TSorter: ISortAlgorithm
		{
			if (array.TryAsSpan(offset, length, out var span))
			{
				Sort(sorter, span);
				return;
			}

			if (!ValidateBounds(array, offset, length))
				return;

			var indexer = new ListIndexer<T>(array);
			sorter.SortDefaultOrder<T, TSorter, ListIndexer<T>, IntReference>(
				indexer, offset, offset + length);
		}

		#endregion

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort<T, TSorter>(
			this TSorter sorter, IList<T> array)
			where TSorter: ISortAlgorithm =>
			Sort(sorter, array, 0, array.Count);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort<T, TSorter, TLessThan>(
			this TSorter sorter, IList<T> array, TLessThan comparer)
			where TSorter: ISortAlgorithm
			where TLessThan: ILessThan<T> =>
		Sort(sorter, array, 0, array.Count, comparer);
	}
}
