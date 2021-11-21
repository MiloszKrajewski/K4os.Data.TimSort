using System;
using System.Runtime.CompilerServices;

namespace TimSortRedo
{
	public interface ILessThan<in T>
	{
		bool Lt(T a, T b);
	}

	public interface IReference<TReference>
	{
		bool Eq(TReference other);
		bool Lt(TReference other);
		TReference Ofs(int offset);
		int Dif(TReference other);
	}

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

	public static class ReferenceExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference Inc<TReference>(this TReference reference)
			where TReference: IReference<TReference> =>
			reference.Ofs(1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference Dec<TReference>(this TReference reference)
			where TReference: IReference<TReference> =>
			reference.Ofs(-1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference Add<TReference>(this TReference reference, int offset)
			where TReference: IReference<TReference> =>
			reference.Ofs(offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference Sub<TReference>(this TReference reference, int offset)
			where TReference: IReference<TReference> =>
			reference.Ofs(-offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Gt<TReference>(this TReference subject, TReference other)
			where TReference: IReference<TReference> =>
			other.Lt(subject);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LtEq<TReference>(this TReference subject, TReference other)
			where TReference: IReference<TReference> =>
			!other.Lt(subject);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GtEq<TReference>(this TReference subject, TReference other)
			where TReference: IReference<TReference> =>
			!subject.Lt(other);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool NEq<TReference>(this TReference subject, TReference other)
			where TReference: IReference<TReference> =>
			!subject.Eq(other);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference Mid<TReference>(this TReference lo, TReference hi)
			where TReference: IReference<TReference> =>
			lo.Add(hi.Dif(lo) >> 1);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference PostInc<TReference>(this ref TReference reference)
			where TReference: struct, IReference<TReference>
		{
			var result = reference;
			reference = reference.Inc();
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference PostDec<TReference>(this ref TReference reference)
			where TReference: struct, IReference<TReference>
		{
			var result = reference;
			reference = reference.Dec();
			return result;
		}
	}

	public static class LessThanExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Gt<TLessThan, T>(this TLessThan comparer, T a, T b)
			where TLessThan: ILessThan<T> =>
			comparer.Lt(b, a);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LtEq<TLessThan, T>(this TLessThan comparer, T a, T b)
			where TLessThan: ILessThan<T> =>
			!comparer.Lt(b, a);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GtEq<TLessThan, T>(this TLessThan comparer, T a, T b)
			where TLessThan: ILessThan<T> =>
			!comparer.Lt(a, b);
	}
}
