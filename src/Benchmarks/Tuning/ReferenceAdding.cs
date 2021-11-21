using System;
using BenchmarkDotNet.Attributes;
using Benchmarks.FiddleArea;

namespace Benchmarks.Tuning
{
	public unsafe class ReferenceAdding
	{
		public readonly SpanReference<double> P =
			new(new IntPtr(0x12345678).ToPointer());

		public readonly int A = 345;
		public readonly int B = 22;
		public readonly int BNeg = -22;
		public readonly int C = 14;
		public SpanReference<double> D;

		[Benchmark]
		public void AddOffsetsFirst() { D = P.Add(A - B + C); }

		[Benchmark]
		public void AddOffsetsToPointer() { D = P.Add(A).Sub(B).Add(C); }

		[Benchmark]
		public void AddOffsetsToPointerNoSub() { D = P.Add(A).Add(BNeg).Add(C); }
	}
}
