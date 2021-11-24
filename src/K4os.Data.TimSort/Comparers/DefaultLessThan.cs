using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Comparers
{
	/// <summary>
	/// Default comparer using native %lt; operator for known types and
	/// falling back to <see cref="Comparer{T}.Default"/> for other types.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly struct DefaultLessThan<T>: ILessThan<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static TOther As<TOther>(ref T a) => Unsafe.As<T, TOther>(ref a);

		// this works because when generic class in expanded only one branch survives
		/// <inheritdoc />
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
			typeof(T) == typeof(string) ? LtString(a, b) :
			typeof(T) == typeof(Guid) ? LtGuid(a, b) :
			// ...and fallback
			Comparer<T>.Default.Compare(a, b) < 0;

		// this works because when generic class in expanded only one branch survives
		// keep it in sync with list above
		/// <summary>Determines if item type is natively handled.</summary>
		/// <returns><c>true</c> it item type is natively handled; <c>false</c> otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsNative() =>
			typeof(T) == typeof(bool) ||
			typeof(T) == typeof(byte) ||
			typeof(T) == typeof(sbyte) ||
			typeof(T) == typeof(short) ||
			typeof(T) == typeof(ushort) ||
			typeof(T) == typeof(int) ||
			typeof(T) == typeof(uint) ||
			typeof(T) == typeof(long) ||
			typeof(T) == typeof(ulong) ||
			typeof(T) == typeof(float) ||
			typeof(T) == typeof(double) ||
			typeof(T) == typeof(decimal) ||
			typeof(T) == typeof(DateTime) ||
			typeof(T) == typeof(TimeSpan) ||
			typeof(T) == typeof(DateTimeOffset) ||
			typeof(T) == typeof(string) ||
			typeof(T) == typeof(Guid);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool LtDateTimeOffset(T a, T b) =>
			As<DateTimeOffset>(ref a) < As<DateTimeOffset>(ref b);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool LtString(T a, T b) => 
			string.CompareOrdinal(As<string>(ref a), As<string>(ref b)) < 0;
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool LtGuid(T a, T b) =>
			As<Guid>(ref a).CompareTo(As<Guid>(ref b)) < 0;
	}
}
