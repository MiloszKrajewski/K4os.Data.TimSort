using System;
using System.Runtime.CompilerServices;

namespace TimSortRedo
{
	public readonly struct IntReference: IReference<IntReference>
	{
		private readonly int _index;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntReference(int index) => _index = index;

		public int Index
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _index;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Eq(IntReference other) => _index == other.Index;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(IntReference other) => _index < other.Index;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntReference Ofs(int offset) => new(_index + offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Dif(IntReference other) => _index - other.Index;
	}
}
