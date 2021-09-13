using System;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals
{
	/// <summary>TimSort indexer for any type which can be converted to <see cref="Span{T}"/>.</summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly unsafe struct SpanIndexer<T>: ITimIndexer<T>
	{
		private readonly void* _ptr;

		/// <summary>Creates indexer for previously pinned <see cref="Span{T}"/>.</summary>
		/// <param name="pinned">Reference of to 0'th item.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SpanIndexer(void* pinned) => _ptr = pinned;

		/// <summary>Creates indexer for previously pinned <see cref="Span{T}"/>.</summary>
		/// <param name="pinned">Reference of to 0'th item.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SpanIndexer(IntPtr pinned) => _ptr = pinned.ToPointer();

		/// <inheritdoc />
		public T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Unsafe.Add(ref Unsafe.AsRef<T>(_ptr), index);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Unsafe.Add(ref Unsafe.AsRef<T>(_ptr), index) = value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Span<T> Span(int start, int length) =>
			new Span<T>(_ptr, start + length).Slice(start);

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Copy(int source, int target, int length) =>
			Span(source, length).CopyTo(Span(target, length));

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Reverse(int lo, int hi) =>
			Span(lo, hi - lo).Reverse();

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Export(int source, T[] buffer, int target, int length) =>
			Span(source, length).CopyTo(buffer.AsSpan(target, length));

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Import(int target, T[] buffer, int source, int length) =>
			buffer.AsSpan(source, length).CopyTo(Span(target, length));
	}
}
