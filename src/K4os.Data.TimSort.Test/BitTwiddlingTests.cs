using K4os.Data.TimSort.Internals;
using Xunit;

namespace K4os.Data.TimSort.Test
{
	public class BitTwiddlingTests
	{
		[Fact]
		public void Log2ReturnsTheRightNumber()
		{
			for (var i = 0; i < 31; i++)
			{
				var min = 1uL << i;
				var max = (min << 1) - 1;
				Assert.Equal(i, BitTwiddlingHacks.Log2((uint)min));
				Assert.Equal(i, BitTwiddlingHacks.Log2((uint)max));
			}
		}
	}
}
