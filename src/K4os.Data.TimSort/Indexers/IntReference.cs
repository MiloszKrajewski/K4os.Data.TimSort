using System;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Indexers
{
	/// <summary>
	/// Implementation of <see cref="IReference{TReference}"/> using index (<see cref="int"/>).
	/// </summary>
	public readonly struct IntReference: IReference<IntReference>
	{
		private readonly int _index;

		/// <summary>Creates new reference.</summary>
		/// <param name="index">Index.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntReference(int index) => _index = index;

		/// <summary>Gets index value stored in reference.</summary>
		public int Index
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _index;
		}

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Eq(IntReference other) => _index == other.Index;

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(IntReference other) => _index < other.Index;

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntReference Ofs(int offset) => _index + offset;

		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Dif(IntReference other) => _index - other.Index;

		/// <summary>Implicit conversion from <see cref="int"/> to <see cref="IntReference"/>.</summary>
		/// <param name="index">Index.</param>
		/// <returns>New <see cref="IntReference"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator IntReference(int index) => new(index);

		/// <summary>Implicit conversion from <see cref="IntReference"/> to <see cref="int"/>.</summary>
		/// <param name="reference">Reference.</param>
		/// <returns>New <see cref="IntReference"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int(IntReference reference) => reference.Index;
	}
}
