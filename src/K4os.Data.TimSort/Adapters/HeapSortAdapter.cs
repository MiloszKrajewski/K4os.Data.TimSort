using K4os.Data.TimSort.Internals;

namespace K4os.Data.TimSort.Adapters
{
	public struct HeapSortAdapter: IAnySortAdapter
	{
		public void Sort<T, TIndexer, TComparer>(TIndexer indexer, int capacity, TComparer comparer)
			where TIndexer: ITimIndexer<T> where TComparer: ITimComparer<T>
		{
			#warning not implemented
//			AnySort.VerifyRange(capacity, 0, capacity);
//			IntroSorter<T, TIndexer, TComparer>.HeapSort(
//				indexer, 0, capacity, comparer);
		}

		public void Sort<T, TIndexer, TComparer>(
			TIndexer indexer, int capacity, int start, int length, TComparer comparer)
			where TIndexer: ITimIndexer<T> where TComparer: ITimComparer<T>
		{
			#warning not implemented
//			AnySort.VerifyRange(capacity, start, length);
//			IntroSorter<T, TIndexer, TComparer>.HeapSort(
//				indexer, start, start + length, comparer);
		}
	}
}
