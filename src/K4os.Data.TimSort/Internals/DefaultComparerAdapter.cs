using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals
{
	/// <summary>TimSort adapter for default comparer
	/// (uses <see cref="Comparer{T}.Default"/> under the hood).</summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly struct DefaultComparerAdapter<T>: ITimComparer<T>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in T a, in T b) => Comparer<T>.Default.Compare(a, b) < 0;
	}
}
