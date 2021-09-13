using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Internals;

namespace K4os.Data.TimSort
{
	/// <summary>
	/// Helper class to create TimSort comparers.
	/// </summary>
	public partial class TimComparer
	{
		/// <summary>Default comparer for given type (uses <see cref="Comparer{T}.Default"/>
		/// under the hood).</summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns>TimSort comparer adapter.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DefaultComparerAdapter<T> Default<T>() =>
			new();

		/// <summary>
		/// Comparer for <see cref="IComparable{T}"/> classes.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns>TimSort comparer adapter.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ComparableAdapter<T> Comparable<T>()
			where T: IComparable<T> => new();

		/// <summary>Comparer using any <see cref="IComparer{T}"/>.</summary>
		/// <param name="comparer">Actual comparer.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns>TimSort comparer adapter.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AnyComparerAdapter<T> From<T>(IComparer<T> comparer) =>
			new(comparer);

		/// <summary>Comparer using any <see cref="Comparer{T}"/>.</summary>
		/// <param name="comparer">Actual comparer.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns>TimSort comparer adapter.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ComparerAdapter<T> From<T>(Comparer<T> comparer) =>
			new(comparer);

		/// <summary>Comparer using any <see cref="Comparison{T}"/>.</summary>
		/// <param name="comparer">Actual comparer.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns>TimSort comparer adapter.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ComparisonAdapter<T> From<T>(Comparison<T> comparer) =>
			new(comparer);
	}
}
