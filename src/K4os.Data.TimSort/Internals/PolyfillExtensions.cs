using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals;

internal static class PolyfillExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref byte Ref0<T>(this Span<T> span) =>
		ref Unsafe.As<T, byte>(ref span.GetPinnableReference());

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref byte Ref0<T>(this T[] array) =>
		ref Unsafe.As<T, byte>(ref array[0]);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<T> AsSpan<T>(this List<T> list) =>
		SpanExtractor.GetSpan(list);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<T> AsSpan<T>(this List<T> list, int start, int length) =>
		list.AsSpan().Slice(start, length);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryAsSpan<T>(this ICollection<T> collection, out Span<T> span) =>
		SpanExtractor.TryGetSpan(collection, out span);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryAsSpan<T>(
		this ICollection<T> collection, int start, int length, out Span<T> span)
	{
		if (!collection.TryAsSpan(out span))
			return false;

		span = span.Slice(start, length);
		return true;
	}

	#if !NET5_0_OR_GREATER

	public static bool IsAssignableTo(this Type derived, Type @base) =>
		@base.IsAssignableFrom(derived);

	#endif
}
