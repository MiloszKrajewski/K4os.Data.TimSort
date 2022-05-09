using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Indexers
{
	/// <summary>
	/// Extension methods for <see cref="IIndexer{T,TReference}"/> derived classes.
	/// </summary>
	public static class IndexerExtensions
	{
		/// <summary>Exports items from collection to given <see cref="PtrReference{T}"/></summary>
		/// <param name="source"></param>
		/// <param name="sourceOffset">Reference to first exported item.</param>
		/// <param name="target">Target <see cref="PtrReference{T}"/></param>
		/// <param name="length">Number of elements to export.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TIndexer">Type of source indexer.</typeparam>
		/// <typeparam name="TReference">Type of source reference.</typeparam>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Export<T, TIndexer, TReference>(
			this TIndexer source, TReference sourceOffset, PtrReference<T> target, int length)
			where TIndexer: IIndexer<T, TReference>
			where TReference: IReference<TReference> =>
			source.Export(sourceOffset, target.Span(length), length);

		/// <summary>Imports items into collection from given <see cref="PtrReference{T}"/>.</summary>
		/// <param name="target">Target indexer..</param>
		/// <param name="targetOffset">Reference to first imported item.</param>
		/// <param name="source">Source <see cref="PtrReference{T}"/></param>
		/// <param name="length">Number of elements to import.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <typeparam name="TIndexer">Type of target indexer.</typeparam>
		/// <typeparam name="TReference">Type of target reference.</typeparam>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Import<T, TIndexer, TReference>(
			this TIndexer target, TReference targetOffset, PtrReference<T> source, int length)
			where TIndexer: IIndexer<T, TReference>
			where TReference: IReference<TReference> =>
			target.Import(targetOffset, source.Span(length), length);
	}
}
