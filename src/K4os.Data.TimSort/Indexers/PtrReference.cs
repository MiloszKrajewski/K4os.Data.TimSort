using System;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Indexers
{
	/// <summary>
	/// <see cref="IReference{TReference}"/> for <see cref="IntPtr"/>.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly unsafe struct PtrReference<T>: IReference<PtrReference<T>>
	{
		private readonly byte* _ptr;

		/// <summary>Creates new reference.</summary>
		/// <param name="ptr">Pointer.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public PtrReference(void* ptr) => _ptr = (byte*)ptr;

		private static uint SizeOfT
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (uint)Unsafe.SizeOf<T>();
		}

		/// <summary>Gets referenced memory location.</summary>
		public byte* Ptr
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _ptr;
		}

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public PtrReference<T> Ofs(int offset) => new(_ptr + offset * SizeOfT);

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Dif(PtrReference<T> other) => (int)((_ptr - other.Ptr) / SizeOfT);

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Eq(PtrReference<T> other) => _ptr == other.Ptr;

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(PtrReference<T> other) => _ptr < other.Ptr;
	}

	/// <summary>Extensions for <see cref="PtrReference{T}"/>.</summary>
	public static class PtrReferenceExtensions
	{
		/// <summary>Creates a <see cref="Span{T}"/> at given memory location.</summary>
		/// <param name="reference">Reference.</param>
		/// <param name="length">Length of the span.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns><see cref="Span{T}"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe Span<T> Span<T>(this PtrReference<T> reference, int length) =>
			new(reference.Ptr, length);
	}
}
