using System;
using System.Collections.Generic;
using System.Linq;
using K4os.Data.TimSort.Test.Utilities;
using Xunit;

namespace K4os.Data.TimSort.Test
{
	public class AdapterTests
	{
		[Fact]
		public void ListIndexer()
		{
			var array = Tools.BuildArray(0, 100_000);
			var list = array.ToList();
			list.TimSort();
			Tools.VerifyArray(list.ToArray());
		}

		[Fact]
		public void AnyListIndexer()
		{
			var array = Tools.BuildArray(0, 100_000);
			var list = new ArrayAsIList<double>(array);
			list.TimSort();
			Tools.VerifyArray(array);
		}

		[Fact]
		public void ArrayAsIList()
		{
			var array = Tools.BuildArray(0, 100_000);
			IList<double> list = array;
			list.TimSort();
			Tools.VerifyArray(array);
		}

		[Fact]
		public void ListAsIList()
		{
			var array = Tools.BuildArray(0, 100_000).ToList();
			IList<double> list = array;
			list.TimSort();
			Tools.VerifyArray(array.ToArray());
		}

		[Fact]
		public void ComparableComparerDoesNotCrash()
		{
			var list = Tools
				.BuildArray(0, 100_000)
				.Select(v => new ComparableStructWrapper<double>(v))
				.ToList();
			list.TimSort();
			Tools.VerifyArray(list.Select(v => v.Value).ToArray());
		}

		[Fact]
		public void ComparableComparerIsAutomaticallySelected()
		{
			var list = Tools
				.BuildArray(0, 100_000)
				.Select(v => new ReveredComparableWrapper<double>(v))
				.ToList();
			list.TimSort();
			Tools.VerifyArray(list.Select(v => v.Value).Reverse().ToArray());
		}
	}
}
