namespace K4os.Data.TimSort.Adapters
{
	public interface IAnySortAdapter
	{
		void Sort<T, TIndexer, TComparer>(
			TIndexer indexer, int capacity, TComparer comparer)
			where TIndexer: ITimIndexer<T>
			where TComparer: ITimComparer<T>;

		void Sort<T, TIndexer, TComparer>(
			TIndexer indexer, int capacity, int start, int length, TComparer comparer)
			where TIndexer: ITimIndexer<T>
			where TComparer: ITimComparer<T>;
	}
}
