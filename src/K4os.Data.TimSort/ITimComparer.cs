namespace K4os.Data.TimSort
{
	/// <summary>Interface to be implemented by TimSort comparer.</summary>
	/// <typeparam name="T">Item type.</typeparam>
	public interface ITimComparer<T>
	{
		/// <summary>When implemented return <c>true</c> if <paramref name="a"/>
		/// is smaller than <paramref name="b"/> and <c>false</c> otherwise.</summary>
		/// <param name="a">First argument.</param>
		/// <param name="b">Second argument.</param>
		/// <returns><c>true</c> if <paramref name="a"/>
		/// is smaller than <paramref name="b"/> and <c>false</c> otherwise.</returns>
		bool Lt(in T a, in T b);
	}
}
