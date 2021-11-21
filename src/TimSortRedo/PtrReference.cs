using System;
using System.Runtime.CompilerServices;

namespace TimSortRedo
{
	public readonly unsafe struct PtrReference<T>: IReference<PtrReference<T>>
	{
		private readonly byte* _ptr;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public PtrReference(void* ptr) => _ptr = (byte*)ptr;

		private static uint SizeOfT
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (uint)Unsafe.SizeOf<T>();
		}

		public byte* Ptr
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _ptr;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public PtrReference<T> Ofs(int offset) =>
			new(_ptr + offset * SizeOfT);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Dif(PtrReference<T> other) =>
			(int)((_ptr - other.Ptr) / SizeOfT);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Eq(PtrReference<T> other) => _ptr == other.Ptr;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(PtrReference<T> other) => _ptr < other.Ptr;
	}

	public static class PtrReferenceExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe Span<T> Span<T>(this PtrReference<T> reference, int length) => 
			new(reference.Ptr, length); 

	}
}
