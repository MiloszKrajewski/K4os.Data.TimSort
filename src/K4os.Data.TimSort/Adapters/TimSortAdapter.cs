using K4os.Data.TimSort.Internals;

namespace K4os.Data.TimSort.Adapters
{
	public struct TimSortAdapter: IAnySortAdapter
	{
		public void Sort<T, TIndexer, TComparer>(
			TIndexer indexer, int capacity, TComparer comparer)
			where TIndexer: ITimIndexer<T> where TComparer: ITimComparer<T> =>
			TimSorter<T>.Sort(indexer, capacity, 0, capacity, comparer);

		public void Sort<T, TIndexer, TComparer>(
			TIndexer indexer,
			int capacity, int start, int length, TComparer comparer)
			where TIndexer: ITimIndexer<T>
			where TComparer: ITimComparer<T> =>
			TimSorter<T>.Sort(indexer, capacity, start, start + length, comparer);
	}
}
