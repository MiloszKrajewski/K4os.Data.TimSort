using System;

namespace K4os.Data.TimSort.Indexers
{
	/// <summary>Interface to access array-like object.</summary>
	/// <typeparam name="T">Type of collection item.</typeparam>
	/// <typeparam name="TReference">Type used for referencing items within collection.</typeparam>
	public interface IIndexer<T, TReference>
		where TReference: IReference<TReference>
	{
		/// <summary>Reference to 0th element.</summary>
		TReference Ref0 { get; }

		/// <summary>Get or sets item in collection.</summary>
		/// <param name="reference">Item's reference.</param>
		T this[TReference reference] { get; set; }

		/// <summary>Swaps two items in collection.</summary>
		/// <param name="a">Reference to first item.</param>
		/// <param name="b">Reference to second item.</param>
		void Swap(TReference a, TReference b);
		
		/// <summary>Copies items within collection. Handles overlapping block.</summary>
		/// <param name="source">Source reference.</param>
		/// <param name="target">Target reference.</param>
		/// <param name="length">Number of elements to copy.</param>
		void Copy(TReference source, TReference target, int length);
		
		/// <summary>Reverses order of elements in collection.</summary>
		/// <param name="lo">Reference of first item (inclusive).</param>
		/// <param name="hi">Reference of last item (exclusive).</param>
		void Reverse(TReference lo, TReference hi);
		
		/// <summary>Exports items from collection to given <see cref="Span{T}"/></summary>
		/// <param name="sourceOffset">Reference to first exported item.</param>
		/// <param name="target">Target <see cref="Span{T}"/></param>
		/// <param name="length">Number of elements to export.</param>
		void Export(TReference sourceOffset, Span<T> target, int length);
		
		/// <summary>Imports items into collection from given <see cref="Span{T}"/>.</summary>
		/// <param name="targetOffset">Reference to first imported item.</param>
		/// <param name="source">Source <see cref="Span{T}"/></param>
		/// <param name="length">Number of elements to import.</param>
		void Import(TReference targetOffset, ReadOnlySpan<T> source, int length);
	}
}
