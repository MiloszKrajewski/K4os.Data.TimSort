using System;
using System.Collections.Generic;
using System.Linq;
using K4os.Data.TimSort.Sorters;
using Xunit;

namespace K4os.Data.TimSort.Test
{
	public readonly struct DoubleLike: IComparable<DoubleLike>
	{
		public readonly double Value;
		public DoubleLike(double value) => Value = value;
		public int CompareTo(DoubleLike other) => Value.CompareTo(other.Value);
	}
	
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
			var list = Tools.BuildArray(0, 100_000).Select(v => new DoubleLike(v)).ToList();
			list.TimSort();
			Tools.VerifyArray(list.Select(v => v.Value).ToArray());
		}
	}
}
