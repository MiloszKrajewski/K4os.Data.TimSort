using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Benchmarks.FiddleArea;

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
				var indexer = new SpanIndexer<double>(ptr0);
				var length = data.Length;
				IntroSortAlgorithm<
						double,
						SpanIndexer<double>,
						SpanReference<double>,
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
