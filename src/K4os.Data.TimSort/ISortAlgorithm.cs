using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;
using K4os.Data.TimSort.Internals;
using K4os.Data.TimSort.Sorters;

namespace K4os.Data.TimSort
{
	/// <summary>Interface giving access to sorting algorithm.</summary>
	public interface ISortAlgorithm
	{
		/// <summary>Sorts given array.</summary>
		/// <param name="indexer">Indexer given access to underlying collection.</param>
		/// <param name="lo">Low index (inclusive).</param>
		/// <param name="hi">High index (exclusive).</param>
		/// <param name="comparer">Comparer.</param>
		/// <typeparam name="T">Type of items.</typeparam>
		/// <typeparam name="TIndexer">Indexer type.</typeparam>
		/// <typeparam name="TReference">Reference (within indexer) type.</typeparam>
		/// <typeparam name="TLessThan">Comparer type.</typeparam>
		void Sort<T, TIndexer, TReference, TLessThan>(
			TIndexer indexer, TReference lo, TReference hi, TLessThan comparer)
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
			where TReference: struct, IReference<TReference> =>
			ComparableSortHelper<T, TSorter, TIndexer, TReference>.Sort(
				sorter, indexer, lo, hi);

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

		/// <summary>Sorts <see cref="Span{T}"/> using given sorter and comparer.</summary>
		/// <param name="sorter">Sorter to be used.</param>
		/// <param name="span">Span to sort.</param>
		/// <param name="comparer">Comparer.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TSorter">Type of sorter.</typeparam>
		/// <typeparam name="TLessThan">Type of comparer.</typeparam>
		public static unsafe void Sort<T, TSorter, TLessThan>(
			this TSorter sorter, Span<T> span, TLessThan comparer)
			where TSorter: ISortAlgorithm
			where TLessThan: ILessThan<T>
		{
			if (!ValidateBounds(span))
				return;

			fixed (void* ptr0 = &span.Ref0())
			{
				var indexer = new PtrIndexer<T>(ptr0);
				sorter.Sort<T, PtrIndexer<T>, PtrReference<T>, TLessThan>(
					indexer, indexer.Ref0, indexer.Ref0.Add(span.Length), comparer);
			}
		}

		/// <summary>Sorts <see cref="Span{T}"/> using given sorter and default comparer.</summary>
		/// <param name="sorter">Sorter to be used.</param>
		/// <param name="span">Span to sort.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TSorter">Type of sorter.</typeparam>
		public static unsafe void Sort<T, TSorter>(
			this TSorter sorter, Span<T> span)
			where TSorter: ISortAlgorithm
		{
			if (!ValidateBounds(span))
				return;

			fixed (void* ptr0 = &span.Ref0())
			{
				var indexer = new PtrIndexer<T>(ptr0);
				sorter.SortDefaultOrder<T, TSorter, PtrIndexer<T>, PtrReference<T>>(
					indexer, indexer.Ref0, indexer.Ref0.Add(span.Length));
			}
		}

		/// <summary>Sorts <see cref="IList{T}"/> using given sorter and comparer.</summary>
		/// <param name="sorter">Sorter to be used.</param>
		/// <param name="list">List to sort.</param>
		/// <param name="offset">Offset within the list.</param>
		/// <param name="length">Number of elements to be sorted.</param>
		/// <param name="comparer">Comparer.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TSorter">Type of sorter.</typeparam>
		/// <typeparam name="TLessThan">Type of comparer.</typeparam>
		public static void Sort<T, TSorter, TLessThan>(
			this TSorter sorter, IList<T> list, int offset, int length, TLessThan comparer)
			where TSorter: ISortAlgorithm
			where TLessThan: ILessThan<T>
		{
			if (list.TryAsSpan(offset, length, out var span))
			{
				Sort(sorter, span, comparer);
				return;
			}

			if (!ValidateBounds(list, offset, length))
				return;

			var indexer = new ListIndexer<T>(list);
			sorter.Sort<T, ListIndexer<T>, IntReference, TLessThan>(
				indexer, offset, offset + length, comparer);
		}

		/// <summary>Sorts <see cref="IList{T}"/> using given sorter and default comparer.</summary>
		/// <param name="sorter">Sorter to be used.</param>
		/// <param name="list">List to sort.</param>
		/// <param name="offset">Offset within the list.</param>
		/// <param name="length">Number of elements to be sorted.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TSorter">Type of sorter.</typeparam>
		public static void Sort<T, TSorter>(
			this TSorter sorter, IList<T> list, int offset, int length)
			where TSorter: ISortAlgorithm
		{
			if (list.TryAsSpan(offset, length, out var span))
			{
				Sort(sorter, span);
				return;
			}

			if (!ValidateBounds(list, offset, length))
				return;

			var indexer = new ListIndexer<T>(list);
			sorter.SortDefaultOrder<T, TSorter, ListIndexer<T>, IntReference>(
				indexer, offset, offset + length);
		}

		#endregion
		
		/// <summary>Sorts <see cref="IList{T}"/> using given sorter and comparer.</summary>
		/// <param name="sorter">Sorter to be used.</param>
		/// <param name="list">List to sort.</param>
		/// <param name="comparer">Comparer.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TSorter">Type of sorter.</typeparam>
		/// <typeparam name="TLessThan">Type of comparer.</typeparam>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort<T, TSorter, TLessThan>(
			this TSorter sorter, IList<T> list, TLessThan comparer)
			where TSorter: ISortAlgorithm
			where TLessThan: ILessThan<T> =>
			Sort(sorter, list, 0, list.Count, comparer);

		/// <summary>Sorts <see cref="IList{T}"/> using given sorter and default comparer.</summary>
		/// <param name="sorter">Sorter to be used.</param>
		/// <param name="list">List to sort.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TSorter">Type of sorter.</typeparam>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort<T, TSorter>(
			this TSorter sorter, IList<T> list)
			where TSorter: ISortAlgorithm =>
			Sort(sorter, list, 0, list.Count);
	}
}
