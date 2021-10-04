using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals
{
	/// <summary>TimSort indexer for any <see cref="IList{T}"/>.</summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly struct AnyIListIndexer<T>: ITimIndexer<T>
	{
		private readonly IList<T> _list;

		/// <summary>Creates new indexer for given <paramref name="list"/>.</summary>
		/// <param name="list"><see cref="IList{T}"/> to be indexed.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public AnyIListIndexer(IList<T> list) => _list = list;

		/// <inheritdoc />
		public T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _list[index];
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => _list[index] = value;
		}

		/// <inheritdoc />
		public void Copy(int source, int target, int length)
		{
			if (source == target || length <= 0) return;

			if (source > target)
			{
				var limit = source + length;
				while (source < limit) this[target++] = this[source++];
			}
			else
			{
				var limit = source;
				source += length;
				target += length;
				while (source > limit) this[--target] = this[--source];
			}
		}

		/// <inheritdoc />
		public void Swap(int source, int target)
		{
			var list = _list;
			// ReSharper disable once SwapViaDeconstruction
			var swap = list[target];
			list[target] = list[source];
			list[source] = swap;
		}

		/// <inheritdoc />
		public void Reverse(int lo, int hi)
		{
			hi--;
			while (lo < hi)
			{
				// Swap with temp is still faster
				// ReSharper disable once SwapViaDeconstruction
				var swap = this[lo];
				this[lo] = this[hi];
				this[hi] = swap;
				lo++;
				hi--;
			}
		}

		/// <inheritdoc />
		public void Export(int source, T[] buffer, int target, int length)
		{
			if (length <= 0) return;

			var limit = source + length;
			while (source < limit) buffer[target++] = this[source++];
		}

		/// <inheritdoc />
		public void Import(int target, T[] buffer, int source, int length)
		{
			if (length <= 0) return;

			var limit = source + length;
			while (source < limit) this[target++] = buffer[source++];
		}
	}
}
