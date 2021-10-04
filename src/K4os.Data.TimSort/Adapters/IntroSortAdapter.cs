using System;
using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Internals;

namespace K4os.Data.TimSort.Adapters
{
	public struct IntroSortAdapter: IAnySortAdapter
	{
		public void Sort<T, TIndexer, TComparer>(TIndexer indexer, int capacity, TComparer comparer)
			where TIndexer: ITimIndexer<T> where TComparer: ITimComparer<T>
		{
			if (!AnySort.VerifyRange(capacity, 0, capacity)) 
				return;

			IntroSorter<T, TIndexer, TComparer>.IntroSort(
				indexer, 0, capacity, comparer);
		}

		public void Sort<T, TIndexer, TComparer>(
			TIndexer indexer, int capacity, int start, int length, TComparer comparer)
			where TIndexer: ITimIndexer<T> where TComparer: ITimComparer<T>
		{
			if (!AnySort.VerifyRange(capacity, start, length)) 
				return;

			IntroSorter<T, TIndexer, TComparer>.IntroSort(
				indexer, start, start + length, comparer);
		}
	}
}
