using System;

namespace K4os.Data.TimSort.Indexers
{
	public interface IIndexer<T, TReference>
		where TReference: IReference<TReference>
	{
		TReference Ref0 { get; }

		T this[TReference reference] { get; set; }

		void Swap(TReference a, TReference b);
		void Copy(TReference source, TReference target, int length);
		void Reverse(TReference lo, TReference hi);
		void Export(TReference sourceOffset, Span<T> target, int length);
		void Import(TReference targetOffset, ReadOnlySpan<T> source, int length);
	}
}
