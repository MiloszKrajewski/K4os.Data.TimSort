using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Comparers
{
	/// <summary>
	/// Factory of <see cref="ILessThan{T}"/> adapters.
	/// </summary>
	public static class LessThan
	{
		/// <summary>
		/// Create <see cref="ILessThan{T}"/> comparer around <see cref="Comparer{T}"/>.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns><see cref="Comparer{T}"/> wrapped with <see cref="ILessThan{T}"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static LessThanComparer<T> Create<T>(Comparer<T> comparer) => new(comparer);

		/// <summary>
		/// Create <see cref="ILessThan{T}"/> comparer around <see cref="IComparer{T}"/>.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns><see cref="IComparer{T}"/> wrapped with <see cref="ILessThan{T}"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static LessThanIComparer<T> Create<T>(IComparer<T> comparer) => new(comparer);

		/// <summary>
		/// Create <see cref="ILessThan{T}"/> comparer around <see cref="Comparison{T}"/>.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns><see cref="Comparison{T}"/> wrapped with <see cref="ILessThan{T}"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static LessThanComparison<T> Create<T>(Comparison<T> comparer) => new(comparer);
	}

	/// <summary><see cref="ILessThan{T}"/> wrapper around <see cref="Comparer{T}"/></summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly struct LessThanComparer<T>: ILessThan<T>
	{
		private readonly Comparer<T> _comparer;

		/// <summary>Create new instance of <see cref="LessThanComparer{T}"/></summary>
		/// <param name="comparer">Actual comparer.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LessThanComparer(Comparer<T> comparer) => _comparer = comparer;

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(T a, T b) => _comparer.Compare(a, b) < 0;
	}

	/// <summary><see cref="ILessThan{T}"/> wrapper around <see cref="IComparer{T}"/></summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly struct LessThanIComparer<T>: ILessThan<T>
	{
		private readonly IComparer<T> _comparer;

		/// <summary>Create new instance of <see cref="LessThanIComparer{T}"/></summary>
		/// <param name="comparer">Actual comparer.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LessThanIComparer(IComparer<T> comparer) => _comparer = comparer;

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(T a, T b) => _comparer.Compare(a, b) < 0;
	}

	/// <summary><see cref="ILessThan{T}"/> wrapper around <see cref="Comparison{T}"/></summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public readonly struct LessThanComparison<T>: ILessThan<T>
	{
		private readonly Comparison<T> _comparer;

		/// <summary>Create new instance of <see cref="LessThanComparison{T}"/></summary>
		/// <param name="comparer">Actual comparer.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public LessThanComparison(Comparison<T> comparer) => _comparer = comparer;

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(T a, T b) => _comparer(a, b) < 0;
	}
}
