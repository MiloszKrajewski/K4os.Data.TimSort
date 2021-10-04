using System;
using K4os.Data.TimSort.Adapters;

namespace K4os.Data.TimSort
{
	public class IntroSort: AnySort<IntroSortAdapter> { }

	public class HeapSort: AnySort<HeapSortAdapter> { }
}
