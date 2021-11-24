using System;
using K4os.Data.TimSort.Comparers;
using Xunit;

namespace K4os.Data.TimSort.Test
{
	public class LessThanTests
	{
		private readonly ILessThan<int> _comparer = new DefaultLessThan<int>();

		[Theory]
		[InlineData(-1, 0, true)]
		[InlineData(0, 0, false)]
		[InlineData(1, 0, false)]
		public void Lt(int a, int b, bool result) => Assert.Equal(_comparer.Lt(a, b), result);

		[Theory]
		[InlineData(-1, 0, true)]
		[InlineData(0, 0, true)]
		[InlineData(1, 0, false)]
		public void LtEq(int a, int b, bool result) => Assert.Equal(_comparer.LtEq(a, b), result);

		[Theory]
		[InlineData(-1, 0, false)]
		[InlineData(0, 0, false)]
		[InlineData(1, 0, true)]
		public void Gt(int a, int b, bool result) => Assert.Equal(_comparer.Gt(a, b), result);

		[Theory]
		[InlineData(-1, 0, false)]
		[InlineData(0, 0, true)]
		[InlineData(1, 0, true)]
		public void GtEq(int a, int b, bool result) => Assert.Equal(_comparer.GtEq(a, b), result);
	}
}
