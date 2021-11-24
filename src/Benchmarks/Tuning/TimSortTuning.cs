using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using K4os.Data.TimSort;
using K4os.Data.TimSort.Sorters;

namespace Benchmarks.Tuning
{
	public class TimSortTuning: TuningBase
	{
		[Benchmark(Baseline = true)]
		public void Legacy()
		{
			var data = GetData();
			ReferenceAlgorithms.LegacyTimSort(data);
		}

		[Benchmark]
		public void Mine()
		{
			var data = GetData();
			data.TimSort();
		}
		
		[Benchmark]
		public void MineWithComparer()
		{
			var data = GetData();
			data.TimSort(Comparer<double>.Default.Compare);
		}

	}
}
