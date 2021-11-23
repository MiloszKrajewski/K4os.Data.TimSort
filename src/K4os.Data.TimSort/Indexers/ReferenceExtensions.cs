using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Indexers
{
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
}
