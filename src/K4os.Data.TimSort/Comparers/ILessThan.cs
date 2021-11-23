namespace K4os.Data.TimSort.Comparers
{
	public interface ILessThan<in T>
	{
		bool Lt(T a, T b);
	}
}
