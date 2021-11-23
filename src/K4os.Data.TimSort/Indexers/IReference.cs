namespace K4os.Data.TimSort.Indexers
{
	public interface IReference<TReference>
	{
		bool Eq(TReference other);
		bool Lt(TReference other);
		TReference Ofs(int offset);
		int Dif(TReference other);
	}
}
