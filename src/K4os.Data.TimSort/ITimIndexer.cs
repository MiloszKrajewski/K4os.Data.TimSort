using System;

namespace K4os.Data.TimSort
{
	/// <summary>
	/// Collection indexer allowing TimSort to access underlying collection
	/// without specifying concrete type.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public interface ITimIndexer<T>
	{
		/// <summary>Allows access to item at given index.</summary>
		/// <param name="index">Element index.</param>
		T this[int index] { get; set; }

		/// <summary>
		/// Copies <paramref name="length"/> elements from index <paramref name="source"/>
		/// to index <paramref name="target"/>. Handles overlapping blocks.
		/// </summary>
		/// <param name="source">Source index.</param>
		/// <param name="target">Target index.</param>
		/// <param name="length">Length.</param>
		void Copy(int source, int target, int length);

		/// <summary>
		/// Reverse order of items between index <paramref name="lo"/> (inclusive) and
		/// <paramref name="hi"/> (exclusive). 
		/// </summary>
		/// <param name="lo">Lower index.</param>
		/// <param name="hi">Higher index (exclusive).</param>
		void Reverse(int lo, int hi);

		/// <summary>
		/// Exports items from collection starting at index <paramref name="source"/> into
		/// <paramref name="buffer"/> starting at index <paramref name="target"/>.
		/// </summary>
		/// <param name="source">Source index.</param>
		/// <param name="buffer">Buffer.</param>
		/// <param name="target">Target index (within <paramref name="buffer"/>)</param>
		/// <param name="length">Number of elements.</param>
		void Export(int source, T[] buffer, int target, int length);

		/// <summary>
		/// Imports items into collection starting at index <paramref name="target"/> from
		/// <paramref name="buffer"/> starting at index <paramref name="source"/>.
		/// </summary>
		/// <param name="target">Target index.</param>
		/// <param name="buffer">Buffer.</param>
		/// <param name="source">Source index (within <paramref name="buffer"/>.</param>
		/// <param name="length">Number of elements.</param>
		void Import(int target, T[] buffer, int source, int length);
	}
}
