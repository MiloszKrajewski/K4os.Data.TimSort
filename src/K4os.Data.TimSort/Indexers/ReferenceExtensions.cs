using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Indexers
{
	/// <summary>
	/// Extension methods for <see cref="IReference{TReference}"/> derived classes.
	/// </summary>
	public static class ReferenceExtensions
	{
		/// <summary>Increment by one.</summary>
		/// <param name="reference">Reference.</param>
		/// <typeparam name="TReference">Type of reference.</typeparam>
		/// <returns>New reference incremented by 1.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference Inc<TReference>(this TReference reference)
			where TReference: IReference<TReference> =>
			reference.Ofs(1);

		/// <summary>Decrement by one.</summary>
		/// <param name="reference">Reference.</param>
		/// <typeparam name="TReference">Type of reference.</typeparam>
		/// <returns>New reference decremented by 1.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference Dec<TReference>(this TReference reference)
			where TReference: IReference<TReference> =>
			reference.Ofs(-1);

		/// <summary>Add to reference.</summary>
		/// <param name="reference">Reference.</param>
		/// <param name="offset">Offset.</param>
		/// <typeparam name="TReference">Type of reference.</typeparam>
		/// <returns>New increased reference.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference Add<TReference>(this TReference reference, int offset)
			where TReference: IReference<TReference> =>
			reference.Ofs(offset);

		/// <summary>Subtract to reference.</summary>
		/// <param name="reference">Reference.</param>
		/// <param name="offset">Offset.</param>
		/// <typeparam name="TReference">Type of reference.</typeparam>
		/// <returns>New decreased reference.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference Sub<TReference>(this TReference reference, int offset)
			where TReference: IReference<TReference> =>
			reference.Ofs(-offset);

		/// <summary>Check if reference is greater than.</summary>
		/// <param name="subject">This reference.</param>
		/// <param name="other">Other reference.</param>
		/// <typeparam name="TReference">Type of reference.</typeparam>
		/// <returns><c>true</c> if this reference is greater than other.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Gt<TReference>(this TReference subject, TReference other)
			where TReference: IReference<TReference> =>
			other.Lt(subject);

		/// <summary>Check if reference is less than or equal to.</summary>
		/// <param name="subject">This reference.</param>
		/// <param name="other">Other reference.</param>
		/// <typeparam name="TReference">Type of reference.</typeparam>
		/// <returns><c>true</c> if this reference is less than or equal to other.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LtEq<TReference>(this TReference subject, TReference other)
			where TReference: IReference<TReference> =>
			!other.Lt(subject);

		/// <summary>Check if reference is greater than or equal to.</summary>
		/// <param name="subject">This reference.</param>
		/// <param name="other">Other reference.</param>
		/// <typeparam name="TReference">Type of reference.</typeparam>
		/// <returns><c>true</c> if this reference is greater than or equal to other.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GtEq<TReference>(this TReference subject, TReference other)
			where TReference: IReference<TReference> =>
			!subject.Lt(other);

		/// <summary>Checks if reference is not equal.</summary>
		/// <param name="subject">This reference.</param>
		/// <param name="other">Other reference.</param>
		/// <typeparam name="TReference">Type of reference.</typeparam>
		/// <returns><c>true</c> if this reference is not equal to other.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool NEq<TReference>(this TReference subject, TReference other)
			where TReference: IReference<TReference> =>
			!subject.Eq(other);

		/// <summary>Calculates reference in the middle of this and other.</summary>
		/// <param name="lo">Lower bound.</param>
		/// <param name="hi">Higher bound.</param>
		/// <typeparam name="TReference">Type of reference.</typeparam>
		/// <returns>Reference in the middle.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference Mid<TReference>(this TReference lo, TReference hi)
			where TReference: IReference<TReference> =>
			lo.Add(hi.Dif(lo) >> 1);
		
		/// <summary>Post increment reference. Equivalent of "this++".
		/// Please note, there is no "PreInc" ("++this") as it can be implemented easily
		/// as "this = this.Inc()" while it is not so trivial with "this++".</summary>
		/// <param name="reference">Reference to be increased.</param>
		/// <typeparam name="TReference">Type of reference.</typeparam>
		/// <returns>Previous value of reference.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TReference PostInc<TReference>(this ref TReference reference)
			where TReference: struct, IReference<TReference>
		{
			var result = reference;
			reference = reference.Inc();
			return result;
		}

		/// <summary>Post decrement reference. Equivalent of "this--".
		/// Please note, there is no "PreDec" ("--this") as it can be implemented easily
		/// as "this = this.Dec()" while it is not so trivial with "this--".</summary>
		/// <param name="reference">Reference to be increased.</param>
		/// <typeparam name="TReference">Type of reference.</typeparam>
		/// <returns>Previous value of reference.</returns>
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
