using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;
using K4os.Data.TimSort.Sorters;

namespace Benchmarks.Tuning
{
	public unsafe class BasicSortTuning: TuningBase
	{
		[Benchmark(Baseline = true)]
		public void Insertion()
		{
			var data = GetData();
			fixed (double* ptr0 = &data[0])
			{
				var indexer = new PtrIndexer<double>(ptr0);
				var length = data.Length;
				BasicSorter<
						double,
						PtrIndexer<double>,
						PtrReference<double>,
						DefaultLessThan<double>>
					.InsertionSort(
						indexer,
						indexer.Ref0,
						indexer.Ref0.Add(length),
						new DefaultLessThan<double>());
			}
		}

		[Benchmark]
		public void Binary()
		{
			var data = GetData();
			fixed (double* ptr0 = &data[0])
			{
				var indexer = new PtrIndexer<double>(ptr0);
				var length = data.Length;
				BasicSorter<
						double,
						PtrIndexer<double>,
						PtrReference<double>,
						DefaultLessThan<double>>
					.BinarySort(
						indexer,
						indexer.Ref0,
						indexer.Ref0.Add(length),
						new DefaultLessThan<double>());
			}
		}
	}
}
