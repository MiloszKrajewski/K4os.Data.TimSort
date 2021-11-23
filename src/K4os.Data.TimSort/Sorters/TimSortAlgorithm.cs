using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;

namespace K4os.Data.TimSort.Sorters
{
	public struct TimSortAlgorithm: ISortAlgorithm
	{
		public static readonly TimSortAlgorithm Default = new();

		public void Sort<T, TIndexer, TReference, TLessThan>(
			TIndexer array, TReference lo, TReference hi, TLessThan comparer)
			where TIndexer: IIndexer<T, TReference>
			where TReference: struct, IReference<TReference>
			where TLessThan: ILessThan<T> =>
			TimSorter<T, TIndexer, TReference, TLessThan>.Sort(array, lo, hi, comparer);
	}
}
