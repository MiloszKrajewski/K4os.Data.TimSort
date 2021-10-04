using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using K4os.Data.TimSort.Internals;

namespace K4os.Data.TimSort.Adapters
{
	public class AnySort
	{
		#region Comparer

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static ComparisonAdapter<T> Comparer<T>(Comparison<T> comparer) =>
			TimComparer.From(comparer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static AnyComparerAdapter<T> Comparer<T>(IComparer<T> comparer) =>
			TimComparer.From(comparer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static ComparerAdapter<T> Comparer<T>(Comparer<T> comparer) =>
			TimComparer.From(comparer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static DefaultComparerAdapter<T> Comparer<T>() =>
			TimComparer.Default<T>();

		#endregion

		#region Range validation

		internal static bool VerifyRange(int capacity, int start, int length)
		{
			if (start < 0)
				ThrowArgumentRangeError(
					nameof(start), $"Start ('{start}') cannot be negative");

			if (length < 0)
				ThrowArgumentRangeError(
					nameof(length), $"Length ('{length}') cannot be negative");

			if (start + length > capacity)
				ThrowArgumentRangeError(
					nameof(capacity),
					$"Given bounds ({start},{length}) are outside buffer of size ({capacity})");

			return length > 1;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void ThrowArgumentRangeError(string paramName, string message) => 
			throw new ArgumentOutOfRangeException(paramName, message);

		
		#endregion
	}
	
	public class AnySort<TSorter>: AnySort where TSorter: struct, IAnySortAdapter
	{
		#region Span<T>

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TComparer">Type of comparer.</typeparam>
		public static unsafe void Sort<T, TComparer>(Span<T> data, TComparer comparer)
			where TComparer: ITimComparer<T>
		{
			ref var data0 = ref MemoryMarshal.GetReference(data);
			fixed (void* pinned = &Unsafe.As<T, byte>(ref data0))
			{
				default(TSorter).Sort<T, SpanIndexer<T>, TComparer>(
					new SpanIndexer<T>(pinned), data.Length, 0, data.Length, comparer);
			}
		}

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(Span<T> data, Comparison<T> comparer) =>
			Sort(data, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(Span<T> data, IComparer<T> comparer) =>
			Sort(data, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(Span<T> data, Comparer<T> comparer) =>
			Sort(data, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using default comparer.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(Span<T> data) =>
			Sort(data, Comparer<T>());

		#endregion

		#region Array<T>

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TComparer">Type of comparer.</typeparam>
		public static void Sort<T, TComparer>(T[] data, TComparer comparer)
			where TComparer: ITimComparer<T> =>
			Sort(data.AsSpan(), comparer);

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(T[] data, Comparison<T> comparer) =>
			Sort(data, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(T[] data, IComparer<T> comparer) =>
			Sort(data, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(T[] data, Comparer<T> comparer) =>
			Sort(data, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using default comparer.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(T[] data) =>
			Sort(data, Comparer<T>());

		#endregion

		#region Array<T> partial

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TComparer">Type of comparer.</typeparam>
		public static void Sort<T, TComparer>(T[] data, int start, int length, TComparer comparer)
			where TComparer: ITimComparer<T> =>
			Sort(data.AsSpan(start, length), comparer);

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(T[] data, int start, int length, Comparison<T> comparer) =>
			Sort(data, start, length, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(T[] data, int start, int length, IComparer<T> comparer) =>
			Sort(data, start, length, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(T[] data, int start, int length, Comparer<T> comparer) =>
			Sort(data, start, length, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using default comparer.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(T[] data, int start, int length) =>
			Sort(data, start, length, Comparer<T>());

		#endregion

		#region List<T>

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TComparer">Type of comparer.</typeparam>
		public static void Sort<T, TComparer>(List<T> data, TComparer comparer)
			where TComparer: ITimComparer<T> =>
			Sort(data.AsSpan(), comparer);

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(List<T> data, Comparison<T> comparer) =>
			Sort(data, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(List<T> data, IComparer<T> comparer) =>
			Sort(data, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(List<T> data, Comparer<T> comparer) =>
			Sort(data, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using default comparer.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(List<T> data) =>
			Sort(data, Comparer<T>());

		#endregion

		#region List<T> partial

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TComparer">Type of comparer.</typeparam>
		public static void Sort<T, TComparer>(
			List<T> data, int start, int length, TComparer comparer)
			where TComparer: ITimComparer<T> =>
			Sort(data.AsSpan(start, length), comparer);

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(
			List<T> data, int start, int length, Comparison<T> comparer) =>
			Sort(data, start, length, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(
			List<T> data, int start, int length, IComparer<T> comparer) =>
			Sort(data, start, length, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(
			List<T> data, int start, int length, Comparer<T> comparer) =>
			Sort(data, start, length, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using default comparer.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(
			List<T> data, int start, int length) =>
			Sort(data, start, length, Comparer<T>());

		#endregion

		#region IList<T>

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TComparer">Type of comparer.</typeparam>
		public static void Sort<T, TComparer>(IList<T> data, TComparer comparer)
			where TComparer: ITimComparer<T> =>
			Sort(data, 0, data.Count, comparer);

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(IList<T> data, Comparison<T> comparer) =>
			Sort(data, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(IList<T> data, IComparer<T> comparer) =>
			Sort(data, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(IList<T> data, Comparer<T> comparer) =>
			Sort(data, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using default comparer.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(IList<T> data) =>
			Sort(data, Comparer<T>());

		#endregion

		#region IList<T> partial

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TComparer">Type of comparer.</typeparam>
		public static void Sort<T, TComparer>(
			IList<T> data, int start, int length, TComparer comparer)
			where TComparer: ITimComparer<T>
		{
			var span = data.TryAsSpan();
			if (span != null)
			{
				Sort(span.Slice(start, length), comparer);
				return;
			}

			var indexer = new AnyIListIndexer<T>(data);
			var capacity = data.Count;
			default(TSorter).Sort<T, AnyIListIndexer<T>, TComparer>(
				indexer, capacity, start, length, comparer);
		}

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(
			IList<T> data, int start, int length, Comparison<T> comparer) =>
			Sort(data, 0, length, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(
			IList<T> data, int start, int length, IComparer<T> comparer) =>
			Sort(data, 0, length, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(
			IList<T> data, int start, int length, Comparer<T> comparer) =>
			Sort(data, 0, length, Comparer(comparer));

		/// <summary>Sorts <paramref name="data"/> using default comparer.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		public static void Sort<T>(
			IList<T> data, int start, int length) =>
			Sort(data, 0, length, Comparer<T>());

		#endregion
	}
}
