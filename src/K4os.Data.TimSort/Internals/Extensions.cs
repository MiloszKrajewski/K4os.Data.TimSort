using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if NET5_0 || NET5_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace K4os.Data.TimSort.Internals
{
	internal static class Extensions
	{
		#if NET5_0 || NET5_0_OR_GREATER
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Span<T> AsSpan<T>(this List<T> list) => 
			CollectionsMarshal.AsSpan(list);

		#else

		public static Span<T> AsSpan<T>(this List<T> list)
		{
			var array = ArrayExtractor<T>.GetArray(list);
			var length = list.Count;
			return array.AsSpan(0, length);
		}

		#endif

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Span<T> AsSpan<T>(this List<T> list, int start, int length) =>
			list.AsSpan().Slice(start, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryAsSpan<T>(this ICollection<T> collection, out Span<T> span)
		{
			switch (collection)
			{
				case T[] array:
					span = array.AsSpan();
					return true;
				case List<T> list:
					span = list.AsSpan();
					return true;
				default:
					span = Span<T>.Empty;
					return false;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryAsSpan<T>(
			this ICollection<T> collection, int start, int length, out Span<T> span)
		{
			if (!collection.TryAsSpan(out span))
				return false;

			span = span.Slice(start, length);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe bool IsNull<T>(this Span<T> span) =>
			Unsafe.AsPointer(ref Unsafe.As<T, byte>(ref span.GetPinnableReference())) == null;

		#if !NET5_0_OR_GREATER

		public static bool IsAssignableTo(this Type derived, Type @base) =>
			@base.IsAssignableFrom(derived);

		#endif
	}
}
