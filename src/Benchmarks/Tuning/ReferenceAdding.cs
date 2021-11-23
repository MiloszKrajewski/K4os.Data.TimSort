using System;
using BenchmarkDotNet.Attributes;
using K4os.Data.TimSort.Indexers;

namespace Benchmarks.Tuning
{
	public unsafe class ReferenceAdding
	{
		public readonly PtrReference<double> P =
			new(new IntPtr(0x12345678).ToPointer());

		public readonly int A = 345;
		public readonly int B = 22;
		public readonly int BNeg = -22;
		public readonly int C = 14;
		public PtrReference<double> D;

		[Benchmark]
		public void AddOffsetsFirst() { D = P.Add(A - B + C); }

		[Benchmark]
		public void AddOffsetsToPointer() { D = P.Add(A).Sub(B).Add(C); }

		[Benchmark]
		public void AddOffsetsToPointerNoSub() { D = P.Add(A).Add(BNeg).Add(C); }
	}
}
