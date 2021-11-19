using System;
using System.Runtime.CompilerServices;

namespace Benchmarks.FiddleArea
{
	public class NewIndexerAndReferenceSorter
	{
		public static void DoubleSorter1(Span<double> array)
		{
			var l = array.Length;
			for (var i = 1; i < l; i++)
			for (var j = i; j > 0; j--)
			{
				if (array[j - 1] <= array[j]) break;

				Swap(array, j - 1, j);
			}
		}

		public static unsafe void DoubleSorter2(Span<double> array)
		{
			fixed (double* ptr0 = &array[0])
			{
				var l = ptr0 + array.Length;
				for (var i = ptr0 + 1; i < l; i++)
				for (var j = i; j > ptr0; j--)
				{
					if (*(j - 1) <= *j) break;

					Swap(j - 1, j);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Swap(Span<double> array, int i, int j)
		{
			// ReSharper disable once SwapViaDeconstruction
			var t = array[i];
			array[i] = array[j];
			array[j] = t;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe void Swap(double* i, double* j)
		{
			// ReSharper disable once SwapViaDeconstruction
			var t = *i;
			*i = *j;
			*j = t;
		}
	}

	public class NewIndexerAndReferenceSorter<T, TLessThan>
		where TLessThan: ILessThan<T>
	{
		public static unsafe void DoubleSorter3(Span<T> array, TLessThan comparer)
		{
			fixed (void* ptr0 = &Unsafe.As<T, byte>(ref array[0]))
			{
				var indexer = new SpanIndexer<T>(ptr0);
				var length = array.Length;
				DoTheJob<SpanIndexer<T>, SpanReference<T>>(indexer, length, comparer);
			}
		}

		private static void DoTheJob<TIndexer, TReference>(
			TIndexer indexer, int length, TLessThan comparer)
			where TIndexer: IIndexer<T, TReference>
			where TReference: IReference<TReference>
		{
			var ref0 = indexer.Ref0;
			var refL = ref0.Add(length);
			for (var i = ref0.Inc(); i.Lt(refL); i = i.Inc())
			for (var j = i; j.Gt(ref0); j = j.Dec())
			{
				if (comparer.LtEq(indexer[j.Dec()], indexer[j])) break;

				indexer.Swap(j.Dec(), j);
			}
		}
	}
}
