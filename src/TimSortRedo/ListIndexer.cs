using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TimSortRedo
{
	public readonly struct ListIndexer<T>: IIndexer<T, IntReference>
	{
		private readonly IList<T> _list;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ListIndexer(IList<T> list) => _list = list;

		public IntReference Ref0
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => new(0);
		}

		public T this[IntReference reference]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _list[reference.Index];
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => _list[reference.Index] = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Swap(IntReference a, IntReference b) =>
			Swap(_list, a.Index, b.Index);

		public void Copy(IntReference source, IntReference target, int length) =>
			Copy(_list, source.Index, target.Index, length);

		public void Reverse(IntReference lo, IntReference hi) =>
			Reverse(_list, lo.Index, hi.Index);

		public void Export(IntReference sourceOffset, Span<T> target, int length) =>
			Export(_list, sourceOffset.Index, target, length);

		public void Import(IntReference targetOffset, ReadOnlySpan<T> source, int length) =>
			Import(_list, targetOffset.Index, source, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Swap(IList<T> list, int a, int b)
		{
			// ReSharper disable once SwapViaDeconstruction
			var t = list[a];
			list[a] = list[b];
			list[b] = t;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Copy(IList<T> list, int source, int target, int length)
		{
			if (source == target || length <= 0) return;

			if (source > target)
			{
				var limit = source + length;
				while (source < limit) list[target++] = list[source++];
			}
			else
			{
				var limit = source;
				source += length;
				target += length;
				while (source > limit) list[--target] = list[--source];
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Reverse(IList<T> list, int lo, int hi)
		{
			while (lo < --hi)
				Swap(list, lo++, hi);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Export(
			IList<T> source, int sourceIndex, Span<T> target, int length)
		{
			var targetIndex = 0;
			while (targetIndex < length) 
				target[targetIndex++] = source[sourceIndex++];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Import(
			IList<T> target, int targetIndex, ReadOnlySpan<T> source, int length)
		{
			var sourceIndex = 0;
			while (sourceIndex < length)
				target[targetIndex++] = source[sourceIndex++];
		}
	}
}
