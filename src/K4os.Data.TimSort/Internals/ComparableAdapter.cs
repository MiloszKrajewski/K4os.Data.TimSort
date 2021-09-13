using System;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals
{
	/// <summary>
	/// Comparer which can be used with <see cref="IComparable{T}"/> classes.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly struct ComparableAdapter<T>: ITimComparer<T> 
		where T: IComparable<T>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in T a, in T b) => a.CompareTo(b) < 0;
	}
}
