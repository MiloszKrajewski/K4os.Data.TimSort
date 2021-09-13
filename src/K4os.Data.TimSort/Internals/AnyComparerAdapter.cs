using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals
{
	/// <summary>TimSort adapter for <see cref="IComparer{T}"/></summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly struct AnyComparerAdapter<T>: ITimComparer<T>
	{
		private readonly IComparer<T> _comparer;

		/// <summary>Creates new instance of <see cref="IComparer{T}"/> adapter.</summary>
		/// <param name="comparer">Actual comparer.</param>
		public AnyComparerAdapter(IComparer<T> comparer) =>
			_comparer = comparer;

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in T a, in T b) => _comparer.Compare(a, b) < 0;
	}
}
