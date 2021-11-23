using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Comparers
{
	public static class LessThan
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static LessThanComparer<T> Create<T>(Comparer<T> comparer) => new(comparer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static LessThanIComparer<T> Create<T>(IComparer<T> comparer) => new(comparer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static LessThanComparison<T> Create<T>(Comparison<T> comparer) => new(comparer);
	}

	public readonly struct LessThanComparer<T>: ILessThan<T>
	{
		private readonly Comparer<T> _comparer;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LessThanComparer(Comparer<T> comparer) => _comparer = comparer;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(T a, T b) => _comparer.Compare(a, b) < 0;
	}

	public readonly struct LessThanIComparer<T>: ILessThan<T>
	{
		private readonly IComparer<T> _comparer;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LessThanIComparer(IComparer<T> comparer) => _comparer = comparer;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(T a, T b) => _comparer.Compare(a, b) < 0;
	}

	public readonly struct LessThanComparison<T>: ILessThan<T>
	{
		private readonly Comparison<T> _comparer;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LessThanComparison(Comparison<T> comparer) => _comparer = comparer;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(T a, T b) => _comparer(a, b) < 0;
	}
}
