using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals;

internal class SmartPool
{
	private const int MIN_POOLED_BYTES = 1024;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool UsePool<T>(int length) => length * Unsafe.SizeOf<T>() >= MIN_POOLED_BYTES;
	
	// ReSharper disable once SuggestBaseTypeForParameter
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool PoolUsed<T>(T[] array) => array.Length > 0 && UsePool<T>(array.Length);

	public static T[] AllocateArray<T>(int size) =>
		size <= 0 ? Array.Empty<T>() :
		!UsePool<T>(size) ? new T[size] :
		ArrayPool<T>.Shared.Rent(size);

	public static void ReallocateArray<T>(ref T[]? array, int size)
	{
		if (array != null && array.Length >= size) return;

		ReleaseArray(ref array);
		array = AllocateArray<T>(size);
	}

	public static void ReleaseArray<T>(ref T[]? array)
	{
		if (array is null || !PoolUsed(array)) return;

		// it is safer to clear it as we don't know what's in it
		ArrayPool<T>.Shared.Return(array, true);
		array = null;
	}

}
