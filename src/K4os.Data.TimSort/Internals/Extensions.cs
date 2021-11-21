using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if NET5_0
using System.Runtime.InteropServices;
#endif

namespace K4os.Data.TimSort.Internals
{
	internal static class Extensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Gt<T, TComparer>(this TComparer comparer, in T a, in T b) 
			where TComparer: ITimComparer<T> =>
			comparer.Lt(b, a);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LtEq<T, TComparer>(this TComparer comparer, in T a, in T b)
			where TComparer: ITimComparer<T> =>
			!comparer.Lt(b, a);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GtEq<T, TComparer>(this TComparer comparer, in T a, in T b)
			where TComparer: ITimComparer<T> =>
			!comparer.Lt(a, b);

		#if NET5_0 || NET5_0_OR_GREATER

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Span<T> AsSpan<T>(this List<T> list) => CollectionsMarshal.AsSpan(list);

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
		public static Span<T> TryAsSpan<T>(this ICollection<T> collection) =>
			collection switch {
				T[] array => array.AsSpan(),
				List<T> list => list.AsSpan(),
				_ => null,
			};
	}
}
