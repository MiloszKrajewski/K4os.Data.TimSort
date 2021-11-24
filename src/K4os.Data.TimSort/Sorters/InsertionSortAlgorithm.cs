using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;

namespace K4os.Data.TimSort.Sorters
{
	/// <summary>InsertionSort adapter for <see cref="ISortAlgorithm"/>.</summary>
	public readonly struct InsertionSortAlgorithm: ISortAlgorithm
	{
		/// <summary>Default instance.</summary>
		public static readonly InsertionSortAlgorithm Default = new();

		/// <inheritdoc />
		public void Sort<T, TIndexer, TReference, TLessThan>(
			TIndexer array, TReference lo, TReference hi, TLessThan comparer)
			where TIndexer: IIndexer<T, TReference>
			where TReference: struct, IReference<TReference>
			where TLessThan: ILessThan<T> =>
			IntroSorter<T, TIndexer, TReference, TLessThan>.InsertionSort(array, lo, hi, comparer);
	}
}
