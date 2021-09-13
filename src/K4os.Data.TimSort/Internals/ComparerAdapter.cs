using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals
{
	/// <summary>TimSort adapter for <see cref="Comparer{T}"/></summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly struct ComparerAdapter<T>: ITimComparer<T>
	{
		private readonly Comparer<T> _comparer;

		/// <summary>Creates new instance of <see cref="Comparer{T}"/> adapter.</summary>
		/// <param name="comparer">Actual comparer.</param>
		public ComparerAdapter(Comparer<T> comparer) =>
			_comparer = comparer;

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in T a, in T b) => _comparer.Compare(a, b) < 0;
	}
}
