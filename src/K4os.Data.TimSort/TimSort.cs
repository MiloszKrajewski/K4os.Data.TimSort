using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using K4os.Data.TimSort.Internals;

namespace K4os.Data.TimSort
{
	/// <summary>
	/// Low level entry point for TimSort algorithm.
	/// Accepts any indexer and any comparer, but is designed for advanced usage.
	/// If not sure, use <see cref="TimSort"/> instead.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class TimSort<T>
	{
		/// <summary>Invokes TimSort on given indexer using provided comparer.</summary>
		/// <param name="indexer">Collection indexer.</param>
		/// <param name="capacity">Collection capacity (length).</param>
		/// <param name="comparer">Comparer.</param>
		/// <typeparam name="TIndexer">Type of indexer.</typeparam>
		/// <typeparam name="TComparer">Type of comparer.</typeparam>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort<TIndexer, TComparer>(
			TIndexer indexer, int capacity, TComparer comparer)
			where TIndexer: ITimIndexer<T>
			where TComparer: ITimComparer<T> =>
			TimSorter<T>.Sort(indexer, capacity, 0, capacity, comparer);

		/// <summary>Invokes TimSort on given indexer using provided comparer.</summary>
		/// <param name="indexer">Collection indexer.</param>
		/// <param name="capacity">Collection capacity (length).</param>
		/// <param name="start">Starting index within collection.</param>
		/// <param name="length">Length of sorted range (not <paramref name="capacity"/>)</param>
		/// <param name="comparer">Comparer.</param>
		/// <typeparam name="TIndexer">Type of indexer.</typeparam>
		/// <typeparam name="TComparer">Type of comparer.</typeparam>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Sort<TIndexer, TComparer>(
			TIndexer indexer, int capacity, int start, int length, TComparer comparer)
			where TIndexer: ITimIndexer<T>
			where TComparer: ITimComparer<T> =>
			TimSorter<T>.Sort(indexer, capacity, start, start + length, comparer);

		internal static unsafe void Sort<TComparer>(Span<T> data, TComparer comparer)
			where TComparer: ITimComparer<T>
		{
			ref var data0 = ref MemoryMarshal.GetReference(data);
			fixed (void* pinned = &Unsafe.As<T, byte>(ref data0))
			{
				var indexer = new SpanIndexer<T>(pinned);
				var length = data.Length;
				Sort(indexer, length, comparer);
			}
		}

		internal static void Sort<TComparer>(
			IList<T> data, int start, int length, TComparer comparer)
			where TComparer: ITimComparer<T>
		{
			var span = data.TryAsSpan();
			if (span == null)
			{
				var indexer = new AnyIListIndexer<T>(data);
				var capacity = data.Count;
				Sort(indexer, capacity, start, length, comparer);
				return;
			}

			Sort(span.Slice(start, length), comparer);
		}
		
		internal static void Sort<TComparer>(
			IList<T> data, TComparer comparer)
			where TComparer: ITimComparer<T> =>
			Sort(data, 0, data.Count, comparer);
	}

	/// <summary>
	/// Entry point to TimSort algorithm. Provides multiple overload for different
	/// types of collections (ie: <see cref="Span{T}"/>, <see cref="List{T}"/>,
	/// or <see cref="IList{T}"/>) and different types of comparers (ie: <see cref="IComparer{T}"/>,
	/// or <see cref="Comparison{T}"/>).
	/// </summary>
	public static class TimSort
	{
		#region Comparer
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ComparisonAdapter<T> Comparer<T>(Comparison<T> comparer) => 
			TimComparer.From(comparer);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static AnyComparerAdapter<T> Comparer<T>(IComparer<T> comparer) => 
			TimComparer.From(comparer);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ComparerAdapter<T> Comparer<T>(Comparer<T> comparer) => 
			TimComparer.From(comparer);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static DefaultComparerAdapter<T> Comparer<T>() => 
			TimComparer.Default<T>();
		
		#endregion

		#region Span<T>

		/// <summary>Sorts <paramref name="data"/> using provided <paramref name="comparer"/>.</summary>
		/// <param name="data">Collection to be sorted.</param>
		/// <param name="comparer">Comparer to be used.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TComparer">Type of comparer.</typeparam>
		public static void Sort<T, TComparer>(Span<T> data, TComparer comparer)
			where TComparer: ITimComparer<T> =>
			TimSort<T>.Sort(data, comparer);

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
			TimSort<T>.Sort(data.AsSpan(), comparer);

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
			TimSort<T>.Sort(data.AsSpan(start, length), comparer);

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
			TimSort<T>.Sort(data.AsSpan(), comparer);

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
			TimSort<T>.Sort(data.AsSpan(start, length), comparer);

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
			TimSort<T>.Sort(data, comparer);

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
			where TComparer: ITimComparer<T> =>
			TimSort<T>.Sort(data, 0, length, comparer);

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
