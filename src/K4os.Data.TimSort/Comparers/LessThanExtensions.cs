using System;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Comparers
{
	/// <summary>
	/// Extensions for <see cref="ILessThan{T}"/>, providing other operations:
	/// "less than or equal", "greater than" and "greater than or equal".
	/// </summary>
	public static class LessThanExtensions
	{
		/// <summary>
		/// Check if item <paramref name="a"/> is greater than <paramref name="b"/>.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		/// <param name="a">First operand.</param>
		/// <param name="b">Second operand.</param>
		/// <returns><c>true</c> if <paramref name="a"/> is greater than <paramref name="b"/>,
		/// <c>false</c> otherwise</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Gt<TLessThan, T>(this TLessThan comparer, T a, T b)
			where TLessThan: ILessThan<T> =>
			comparer.Lt(b, a);

		/// <summary>
		/// Check if item <paramref name="a"/> is less than or equal to <paramref name="b"/>.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		/// <param name="a">First operand.</param>
		/// <param name="b">Second operand.</param>
		/// <returns><c>true</c> if <paramref name="a"/> is less than or equal to <paramref name="b"/>,
		/// <c>false</c> otherwise</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LtEq<TLessThan, T>(this TLessThan comparer, T a, T b)
			where TLessThan: ILessThan<T> =>
			!comparer.Lt(b, a);

		/// <summary>
		/// Check if item <paramref name="a"/> is greater than or equal to <paramref name="b"/>.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		/// <param name="a">First operand.</param>
		/// <param name="b">Second operand.</param>
		/// <returns><c>true</c> if <paramref name="a"/> is greater than or equal to <paramref name="b"/>,
		/// <c>false</c> otherwise</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GtEq<TLessThan, T>(this TLessThan comparer, T a, T b)
			where TLessThan: ILessThan<T> =>
			!comparer.Lt(a, b);
	}
}
