using System.Numerics;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals
{
	/// <summary>
	/// All the bit hacks needed for quick integer math.
	/// See: https://graphics.stanford.edu/~seander/bithacks.html#IntegerLogDeBruijn
	/// </summary>
	public static class BitTwiddlingHacks
	{
		#if NET5_0
		/// <summary>Calculates log2(n) of given integer value.</summary>
		/// <param name="n">Value of n.</param>
		/// <returns>Log2(n).</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Log2(uint n) => BitOperations.Log2(n);
		#else
		private static readonly int[] MultiplyDeBruijnBitPosition = {
			0, 9, 1, 10, 13, 21, 2, 29, 11, 14, 16, 18, 22, 25, 3, 30,
			8, 12, 20, 28, 15, 17, 24, 7, 19, 27, 23, 6, 26, 5, 4, 31,
		};
		
		/// <summary>Calculates log2(n) of given integer value.</summary>
		/// <param name="n">Value of n.</param>
		/// <returns>Log2(n).</returns>
		public static int Log2(uint n)
		{
			n |= n >> 1; 
			n |= n >> 2;
			n |= n >> 4;
			n |= n >> 8;
			n |= n >> 16;

			return MultiplyDeBruijnBitPosition[n * 0x07C4ACDDu >> 27];
		}
		#endif
	}
}
