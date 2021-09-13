using System;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals
{
	/// <summary>TimSort adapter for <see cref="Comparison{T}"/></summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly struct ComparisonAdapter<T>: ITimComparer<T>
	{
		private readonly Comparison<T> _comparer;

		/// <summary>Creates new instance of <see cref="Comparison{T}"/> adapter.</summary>
		/// <param name="comparer">Actual comparer.</param>
		public ComparisonAdapter(Comparison<T> comparer) =>
			_comparer = comparer;

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in T a, in T b) => _comparer(a, b) < 0;
	}
}
