using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
	[MemoryDiagnoser]
	public class AllocVsArrayPool
	{
		private static readonly object SomeObject = new();
			
		[Benchmark]
		public void AllocArray()
		{
			var o = SomeObject;
			var a = new object[32];
			for (var i = 0; i < 32; i++) a[i] = o;
		}

		[Benchmark]
		public void RentArray()
		{
			var p = ArrayPool<object>.Shared;
			var o = SomeObject;
			var a = p.Rent(32);
			for (var i = 0; i < 32; i++) a[i] = o;
			p.Return(a);
		}

		[Benchmark]
		public void UseStruct()
		{
			var o = SomeObject;
			var a = new Array32<object>();
			for (var i = 0; i < 32; i++) a[i] = o;
		}
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

		public unsafe ref T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => ref Unsafe.AsRef<T>(Unsafe.Add<T>(Unsafe.AsPointer(ref i00), index));
		}
	}
}
