using K4os.Data.TimSort;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;

namespace ReferenceImplementation.Quad;

/// <summary>QuadSort adapter for <see cref="ISortAlgorithm"/>.</summary>
public readonly struct QuadSortAlgorithm: ISortAlgorithm
{
	/// <summary>Default instance.</summary>
	public static readonly QuadSortAlgorithm Default = new();

	/// <inheritdoc />
	public void Sort<T, TIndexer, TReference, TLessThan>(
		TIndexer array, TReference lo, TReference hi, TLessThan comparer)
		where TIndexer: IIndexer<T, TReference>
		where TReference: struct, IReference<TReference>
		where TLessThan: ILessThan<T> =>
		QuadSorter<T, TIndexer, TReference, TLessThan>.QuadSort(array, lo, hi, comparer);
}