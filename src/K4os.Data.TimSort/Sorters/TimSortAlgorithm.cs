using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;

namespace K4os.Data.TimSort.Sorters
{
	/// <summary>TimSort adapter for <see cref="ISortAlgorithm"/>.</summary>
	public readonly struct TimSortAlgorithm: ISortAlgorithm
	{
		/// <summary>Default instance.</summary>
		public static readonly TimSortAlgorithm Default = new();

		/// <inheritdoc />
		public void Sort<T, TIndexer, TReference, TLessThan>(
			TIndexer array, TReference lo, TReference hi, TLessThan comparer)
			where TIndexer: IIndexer<T, TReference>
			where TReference: struct, IReference<TReference>
			where TLessThan: ILessThan<T> =>
			TimSorter<T, TIndexer, TReference, TLessThan>.Sort(array, lo, hi, comparer);
	}
}
