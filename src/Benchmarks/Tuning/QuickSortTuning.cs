using System;
using BenchmarkDotNet.Attributes;
using Benchmarks.FiddleArea;

namespace Benchmarks.Tuning
{
	public class QuickSortTuning: TuningBase
	{
		[Benchmark(Baseline = true)]
		public void Theirs()
		{
			var data = GetData();
			ReferenceAlgorithms.IntroSort(data.AsSpan());
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
					.IntroSort(
						indexer, 
						indexer.Ref0, 
						indexer.Ref0.Add(length), 
						new DefaultLessThan<double>());
			}
		}
	}
}
