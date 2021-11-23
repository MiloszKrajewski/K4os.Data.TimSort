using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;

namespace K4os.Data.TimSort.Sorters
{
	public struct HeapSortAlgorithm: ISortAlgorithm
	{
		public static readonly HeapSortAlgorithm Default = new();
		
		public void Sort<T, TIndexer, TReference, TLessThan>(
			TIndexer array, TReference lo, TReference hi, TLessThan comparer)
			where TIndexer: IIndexer<T, TReference>
			where TReference: struct, IReference<TReference>
			where TLessThan: ILessThan<T> =>
			IntroSorter<T, TIndexer, TReference, TLessThan>.HeapSort(array, lo, hi, comparer);
	}
}
