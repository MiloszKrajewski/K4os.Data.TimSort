using System;
using System.Runtime.CompilerServices;

namespace TimSortRedo
{
	public readonly unsafe struct PtrIndexer<T>: IIndexer<T, PtrReference<T>>
	{
		private readonly PtrReference<T> _ref0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public PtrIndexer(void* ptr0) => _ref0 = new PtrReference<T>(ptr0);

		public PtrReference<T> Ref0
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _ref0;
		}

		public T this[PtrReference<T> reference]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Unsafe.Read<T>(reference.Ptr);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Unsafe.Write(reference.Ptr, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Swap(PtrReference<T> a, PtrReference<T> b)
		{
			// ReSharper disable once SwapViaDeconstruction
			var t = this[a];
			this[a] = this[b];
			this[b] = t;
		}

		public void Copy(PtrReference<T> source, PtrReference<T> target, int length) =>
			source.Span(length).CopyTo(target.Span(length));

		public void Reverse(PtrReference<T> lo, PtrReference<T> hi) =>
			lo.Span(hi.Dif(lo)).Reverse();

		public void Export(PtrReference<T> sourceOffset, Span<T> target, int length) => 
			sourceOffset.Span(length).CopyTo(target);

		public void Import(PtrReference<T> targetOffset, ReadOnlySpan<T> source, int length) =>
			source.CopyTo(targetOffset.Span(length));
	}
}
