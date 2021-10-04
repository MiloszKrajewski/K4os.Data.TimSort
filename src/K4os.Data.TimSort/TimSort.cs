using System;
using System.Collections.Generic;
using K4os.Data.TimSort.Adapters;
using K4os.Data.TimSort.Internals;

namespace K4os.Data.TimSort
{
	/// <summary>
	/// Entry point to TimSort algorithm. Provides multiple overload for different
	/// types of collections (ie: <see cref="IList{T}"/>, <see cref="IComparer{T}"/>,
	/// or <see cref="Comparison{T}"/>) and different types of comparers (ie: <see cref="Span{T}"/>,
	/// or <see cref="List{T}"/>).
	/// </summary>
	public class TimSort: AnySort<TimSortAdapter> { }
}
