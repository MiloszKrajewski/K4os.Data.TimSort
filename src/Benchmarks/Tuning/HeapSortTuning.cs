using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;
using K4os.Data.TimSort.Sorters;

namespace Benchmarks.Tuning
{
	public class HeapSortTuning: TuningBase
	{
		[Benchmark(Baseline = true)]
		public void Theirs()
		{
			var data = GetData();
			ReferenceAlgorithms.HeapSort(data.AsSpan());
		}

		[Benchmark]
		public unsafe void Mine()
		{
			var data = GetData();

			fixed (double* ptr0 = &data[0])
			{
				var indexer = new PtrIndexer<double>(ptr0);
				var length = data.Length;
				IntroSorter<
						double,
						PtrIndexer<double>,
						PtrReference<double>,
						DefaultLessThan<double>>
					.HeapSort(
						indexer,
						indexer.Ref0,
						indexer.Ref0.Add(length),
						new DefaultLessThan<double>());
			}
		}
	}
}
