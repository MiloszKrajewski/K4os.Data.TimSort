namespace K4os.Data.TimSort.Comparers
{
	/// <summary>Comparer abstraction for sorting algorithms.</summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public interface ILessThan<in T>
	{
		/// <summary>
		/// Check if item <paramref name="a"/> is less than <paramref name="b"/>.
		/// </summary>
		/// <param name="a">First operand.</param>
		/// <param name="b">Second operand.</param>
		/// <returns><c>true</c> if <paramref name="a"/> is less than <paramref name="b"/>,
		/// <c>false</c> otherwise</returns>
		bool Lt(T a, T b);
	}
}
