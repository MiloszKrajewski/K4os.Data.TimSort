using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Comparers
{
	public readonly struct ComparableLessThan<T>: ILessThan<T>
		where T: IComparable<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static TOther As<TOther>(ref T a) => Unsafe.As<T, TOther>(ref a);

		// this works because when generic class in expanded only one branch survives
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SuppressMessage("ReSharper", "RedundantTernaryExpression")]
		public bool Lt(T a, T b) =>
			typeof(T) == typeof(bool) ? !As<bool>(ref a) && As<bool>(ref b) ? true : false :
			typeof(T) == typeof(byte) ? As<byte>(ref a) < As<byte>(ref b) ? true : false :
			typeof(T) == typeof(sbyte) ? As<sbyte>(ref a) < As<sbyte>(ref b) ? true : false :
			typeof(T) == typeof(short) ? As<short>(ref a) < As<short>(ref b) ? true : false :
			typeof(T) == typeof(ushort) ? As<ushort>(ref a) < As<ushort>(ref b) ? true : false :
			typeof(T) == typeof(int) ? As<int>(ref a) < As<int>(ref b) ? true : false :
			typeof(T) == typeof(uint) ? As<uint>(ref a) < As<uint>(ref b) ? true : false :
			typeof(T) == typeof(long) ? As<long>(ref a) < As<long>(ref b) ? true : false :
			typeof(T) == typeof(ulong) ? As<ulong>(ref a) < As<ulong>(ref b) ? true : false :
			typeof(T) == typeof(float) ? As<float>(ref a) < As<float>(ref b) ? true : false :
			typeof(T) == typeof(double) ? As<double>(ref a) < As<double>(ref b) ? true : false :
			typeof(T) == typeof(decimal) ? As<decimal>(ref a) < As<decimal>(ref b) ? true : false :
			// not sure if ones below are worth inlining (or "? true : false"-ing)...
			typeof(T) == typeof(DateTime) ? As<DateTime>(ref a) < As<DateTime>(ref b) :
			typeof(T) == typeof(TimeSpan) ? As<TimeSpan>(ref a) < As<TimeSpan>(ref b) :
			typeof(T) == typeof(DateTimeOffset) ? LtDateTimeOffset(a, b) :
			// ...and fallback
			a.CompareTo(b) < 0 ? true : false;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool LtDateTimeOffset(T a, T b) =>
			As<DateTimeOffset>(ref a) < As<DateTimeOffset>(ref b);
	}
}
