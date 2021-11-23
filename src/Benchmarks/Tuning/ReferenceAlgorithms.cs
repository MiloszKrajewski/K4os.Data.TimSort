using System;
using System.Collections;
using System.Collections.Generic;
using ReferenceImplementation;

namespace Benchmarks.Tuning
{
	public class ReferenceAlgorithms
	{
		public static void IntroSort(Span<double> data) { data.Sort(); }

		public static void HeapSort(Span<double> data)
		{
			var n = data.Length;
			for (var i = n >> 1; i >= 1; i--)
			{
				DownHeap(data, i, n);
			}

			for (var i = n; i > 1; i--)
			{
				Swap(data, 0, i - 1);
				DownHeap(data, 1, i - 1);
			}
		}

		private static void DownHeap(Span<double> data, int i, int n)
		{
			var d = data[i - 1];
			while (i <= n >> 1)
			{
				var child = 2 * i;

				if (child < n && data[child - 1] < data[child]) child++;
				if (!(d < data[child - 1])) break;

				data[i - 1] = data[child - 1];
				i = child;
			}

			data[i - 1] = d;
		}

		public static void InsertionSort(Span<double> keys)
		{
			for (var i = 0; i < keys.Length - 1; i++)
			{
				var t = keys[i + 1];

				var j = i;
				while (j >= 0 && t < keys[j])
				{
					keys[j + 1] = keys[j];
					j--;
				}

				keys[j + 1] = t;
			}
		}

		private static void Swap(Span<double> data, int a, int b)
		{
			// ReSharper disable once SwapViaDeconstruction
			var t = data[a];
			data[a] = data[b];
			data[b] = t;
		}

		public static void LegacyTimSort(double[] keys)
		{
			ArrayTimSort<double>.Sort(keys, 0, keys.Length, Comparer<double>.Default.Compare);
		}
	}
}
