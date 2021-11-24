using System;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Indexers
{
	/// <summary>
	/// <see cref="IIndexer{T,TReference}"/> implementation for any contiguous block of
	/// memory, for example <see cref="Span{T}"/>
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly unsafe struct PtrIndexer<T>: IIndexer<T, PtrReference<T>>
	{
		private readonly PtrReference<T> _ref0;

		/// <summary>
		/// Creates <see cref="PtrIndexer{T}"/> for given <see cref="Span{T}"/>.
		/// Please note, <see cref="Span{T}"/> needs to be already pinned (bad
		/// things may happen if it is not).
		/// Also note, that <see cref="PtrIndexer{T}"/> does not perform any
		/// bounds check, so it can be very fast but also unsafe.
		/// </summary>
		/// <param name="ptr0">Pointer to first item.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public PtrIndexer(void* ptr0) => _ref0 = new PtrReference<T>(ptr0);

		/// <inheritdoc />
		public PtrReference<T> Ref0
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _ref0;
		}

		/// <inheritdoc />
		public T this[PtrReference<T> reference]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Unsafe.Read<T>(reference.Ptr);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Unsafe.Write(reference.Ptr, value);
		}

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Swap(PtrReference<T> a, PtrReference<T> b)
		{
			// ReSharper disable once SwapViaDeconstruction
			var t = this[a];
			this[a] = this[b];
			this[b] = t;
		}

		/// <inheritdoc />
		public void Copy(PtrReference<T> source, PtrReference<T> target, int length) =>
			source.Span(length).CopyTo(target.Span(length));

		/// <inheritdoc />
		public void Reverse(PtrReference<T> lo, PtrReference<T> hi) =>
			lo.Span(hi.Dif(lo)).Reverse();

		/// <inheritdoc />
		public void Export(PtrReference<T> sourceOffset, Span<T> target, int length) => 
			sourceOffset.Span(length).CopyTo(target);

		/// <inheritdoc />
		public void Import(PtrReference<T> targetOffset, ReadOnlySpan<T> source, int length) =>
			source.CopyTo(targetOffset.Span(length));
	}
}
