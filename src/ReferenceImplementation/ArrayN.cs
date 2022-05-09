using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using K4os.Data.TimSort.Indexers;

namespace ReferenceImplementation
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct Array4<T>
	{
		public T i00;
		public T i01;
		public T i02;
		public T i03;

		public unsafe PtrIndexer<T> Indexer() => new(Unsafe.AsPointer(ref i00));
	}
	
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct Array8<T>
	{
		public T i00;
		public T i01;
		public T i02;
		public T i03;
		
		public T i04;
		public T i05;
		public T i06;
		public T i07;
		
		public unsafe PtrIndexer<T> Indexer() => new(Unsafe.AsPointer(ref i00));
	}
	
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct Array16<T>
	{
		public T i00;
		public T i01;
		public T i02;
		public T i03;
		
		public T i04;
		public T i05;
		public T i06;
		public T i07;
		
		public T i08;
		public T i09;
		public T i0A;
		public T i0B;
		
		public T i0C;
		public T i0D;
		public T i0E;
		public T i0F;
		
		public unsafe PtrIndexer<T> Indexer() => new(Unsafe.AsPointer(ref i00));
	}
	
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct Array32<T>
	{
		public T i00;
		public T i01;
		public T i02;
		public T i03;
		
		public T i04;
		public T i05;
		public T i06;
		public T i07;
		
		public T i08;
		public T i09;
		public T i0A;
		public T i0B;
		
		public T i0C;
		public T i0D;
		public T i0E;
		public T i0F;
		
		public T i10;
		public T i11;
		public T i12;
		public T i13;
		
		public T i14;
		public T i15;
		public T i16;
		public T i17;
		
		public T i18;
		public T i19;
		public T i1A;
		public T i1B;
		
		public T i1C;
		public T i1D;
		public T i1E;
		public T i1F;

		public unsafe PtrIndexer<T> Indexer() => new(Unsafe.AsPointer(ref i00));
	}
}
