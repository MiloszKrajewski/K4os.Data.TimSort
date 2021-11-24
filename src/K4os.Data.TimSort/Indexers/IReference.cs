using System;

namespace K4os.Data.TimSort.Indexers
{
	/// <summary>
	/// Interface used to point to specific elements of collection.
	/// It is a little bit over-engineered <see cref="int"/> or <see cref="IntPtr"/>.
	/// </summary>
	/// <typeparam name="TReference"></typeparam>
	public interface IReference<TReference>
	{
		/// <summary>Checks if two references are equal.</summary>
		/// <param name="other">Other reference.</param>
		/// <returns><c>true</c> if references are equal; <c>false</c> otherwise.</returns>
		bool Eq(TReference other);
		
		/// <summary>Checks if this references is less than the other.</summary>
		/// <param name="other">Other reference.</param>
		/// <returns><c>true</c> if this reference is less than the other; <c>false</c> otherwise.</returns>
		bool Lt(TReference other);
		
		/// <summary>Offsets reference by number of items.
		/// Equivalent of <c>return this.index + offset</c>.</summary>
		/// <param name="offset">Offset.</param>
		/// <returns>New reference.</returns>
		TReference Ofs(int offset);
		
		/// <summary>Distance between references.
		/// Equivalent of <c>return this.index - other.index;</c></summary>
		/// <param name="other">Other reference.</param>
		/// <returns>Distance.</returns>
		int Dif(TReference other);
	}
}
