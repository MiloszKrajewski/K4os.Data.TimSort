using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;

namespace K4os.Data.TimSort.Sorters
{
	public struct IntroSortAlgorithm: ISortAlgorithm
	{
		public static readonly IntroSortAlgorithm Default = new();
		
		public void Sort<T, TIndexer, TReference, TLessThan>(
			TIndexer array, TReference lo, TReference hi, TLessThan comparer)
			where TIndexer: IIndexer<T, TReference>
			where TReference: struct, IReference<TReference>
			where TLessThan: ILessThan<T> =>
			IntroSorter<T, TIndexer, TReference, TLessThan>.IntroSort(array, lo, hi, comparer);
	}
}
