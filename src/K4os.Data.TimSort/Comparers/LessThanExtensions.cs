using System;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Comparers
{
	public static class LessThanExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Gt<TLessThan, T>(this TLessThan comparer, T a, T b)
			where TLessThan: ILessThan<T> =>
			comparer.Lt(b, a);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LtEq<TLessThan, T>(this TLessThan comparer, T a, T b)
			where TLessThan: ILessThan<T> =>
			!comparer.Lt(b, a);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GtEq<TLessThan, T>(this TLessThan comparer, T a, T b)
			where TLessThan: ILessThan<T> =>
			!comparer.Lt(a, b);
	}
}
