// This file is automatically generated from template
// all changes will be lost
// T4 templates specification:
// https://docs.microsoft.com/en-us/visualstudio/modeling/writing-a-t4-text-template?view=vs-2022

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Internals;

namespace K4os.Data.TimSort.Sorters
{
	public static partial class SortAlgorithmExtensions
	{

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this Span<T> span) =>
			default(TimSortAlgorithm).Sort(span);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this Span<T> span, Comparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(span, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this Span<T> span, Comparison<T> comparer) =>
			default(TimSortAlgorithm).Sort(span, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this Span<T> span, IComparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(span, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this T[] array) =>
			default(TimSortAlgorithm).Sort(array.AsSpan());

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this T[] array, Comparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(array.AsSpan(), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this T[] array, Comparison<T> comparer) =>
			default(TimSortAlgorithm).Sort(array.AsSpan(), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this T[] array, IComparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(array.AsSpan(), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this List<T> list) =>
			default(TimSortAlgorithm).Sort(list.AsSpan());

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this List<T> list, Comparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(list.AsSpan(), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this List<T> list, Comparison<T> comparer) =>
			default(TimSortAlgorithm).Sort(list.AsSpan(), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this List<T> list, IComparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(list.AsSpan(), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this IList<T> list) =>
			default(TimSortAlgorithm).Sort(list);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this IList<T> list, Comparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(list, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this IList<T> list, Comparison<T> comparer) =>
			default(TimSortAlgorithm).Sort(list, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this IList<T> list, IComparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(list, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this Span<T> span, int offset, int length) =>
			default(TimSortAlgorithm).Sort(span.Slice(offset, length));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this Span<T> span, int offset, int length, Comparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(span.Slice(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this Span<T> span, int offset, int length, Comparison<T> comparer) =>
			default(TimSortAlgorithm).Sort(span.Slice(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this Span<T> span, int offset, int length, IComparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(span.Slice(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this T[] array, int offset, int length) =>
			default(TimSortAlgorithm).Sort(array.AsSpan(offset, length));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this T[] array, int offset, int length, Comparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(array.AsSpan(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this T[] array, int offset, int length, Comparison<T> comparer) =>
			default(TimSortAlgorithm).Sort(array.AsSpan(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this T[] array, int offset, int length, IComparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(array.AsSpan(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this List<T> list, int offset, int length) =>
			default(TimSortAlgorithm).Sort(list.AsSpan(offset, length));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this List<T> list, int offset, int length, Comparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(list.AsSpan(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this List<T> list, int offset, int length, Comparison<T> comparer) =>
			default(TimSortAlgorithm).Sort(list.AsSpan(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this List<T> list, int offset, int length, IComparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(list.AsSpan(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this IList<T> list, int offset, int length) =>
			default(TimSortAlgorithm).Sort(list, offset, offset + length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this IList<T> list, int offset, int length, Comparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(list, offset, offset + length, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this IList<T> list, int offset, int length, Comparison<T> comparer) =>
			default(TimSortAlgorithm).Sort(list, offset, offset + length, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void TimSort<T>(this IList<T> list, int offset, int length, IComparer<T> comparer) =>
			default(TimSortAlgorithm).Sort(list, offset, offset + length, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this Span<T> span) =>
			default(IntroSortAlgorithm).Sort(span);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this Span<T> span, Comparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(span, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this Span<T> span, Comparison<T> comparer) =>
			default(IntroSortAlgorithm).Sort(span, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this Span<T> span, IComparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(span, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this T[] array) =>
			default(IntroSortAlgorithm).Sort(array.AsSpan());

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this T[] array, Comparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(array.AsSpan(), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this T[] array, Comparison<T> comparer) =>
			default(IntroSortAlgorithm).Sort(array.AsSpan(), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this T[] array, IComparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(array.AsSpan(), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this List<T> list) =>
			default(IntroSortAlgorithm).Sort(list.AsSpan());

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this List<T> list, Comparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(list.AsSpan(), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this List<T> list, Comparison<T> comparer) =>
			default(IntroSortAlgorithm).Sort(list.AsSpan(), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this List<T> list, IComparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(list.AsSpan(), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this IList<T> list) =>
			default(IntroSortAlgorithm).Sort(list);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this IList<T> list, Comparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(list, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this IList<T> list, Comparison<T> comparer) =>
			default(IntroSortAlgorithm).Sort(list, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this IList<T> list, IComparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(list, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this Span<T> span, int offset, int length) =>
			default(IntroSortAlgorithm).Sort(span.Slice(offset, length));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this Span<T> span, int offset, int length, Comparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(span.Slice(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this Span<T> span, int offset, int length, Comparison<T> comparer) =>
			default(IntroSortAlgorithm).Sort(span.Slice(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this Span<T> span, int offset, int length, IComparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(span.Slice(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this T[] array, int offset, int length) =>
			default(IntroSortAlgorithm).Sort(array.AsSpan(offset, length));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this T[] array, int offset, int length, Comparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(array.AsSpan(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this T[] array, int offset, int length, Comparison<T> comparer) =>
			default(IntroSortAlgorithm).Sort(array.AsSpan(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this T[] array, int offset, int length, IComparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(array.AsSpan(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this List<T> list, int offset, int length) =>
			default(IntroSortAlgorithm).Sort(list.AsSpan(offset, length));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this List<T> list, int offset, int length, Comparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(list.AsSpan(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this List<T> list, int offset, int length, Comparison<T> comparer) =>
			default(IntroSortAlgorithm).Sort(list.AsSpan(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this List<T> list, int offset, int length, IComparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(list.AsSpan(offset, length), LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this IList<T> list, int offset, int length) =>
			default(IntroSortAlgorithm).Sort(list, offset, offset + length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this IList<T> list, int offset, int length, Comparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(list, offset, offset + length, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this IList<T> list, int offset, int length, Comparison<T> comparer) =>
			default(IntroSortAlgorithm).Sort(list, offset, offset + length, LessThan.Create(comparer));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IntroSort<T>(this IList<T> list, int offset, int length, IComparer<T> comparer) =>
			default(IntroSortAlgorithm).Sort(list, offset, offset + length, LessThan.Create(comparer));

	}
}