using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
	public class BoolToInt
	{
		public double[] Array;
		public int Result;

		[GlobalSetup]
		public void Setup()
		{
			Array = new double[1_000_000];
			var r = new Random(0);
			for (var i = 0; i < Array.Length; i++)
				Array[i] = r.NextDouble();
		}

		[Benchmark(Baseline = true)]
		public void ConvertToInt32()
		{
			var array = Array;
			var length = Array.Length;
			var result = 0;
			for (var i = 0; i < length; i++)
				result += Convert.ToInt32(array[i] < 0.5);
			Result = result;
		}
		
		[Benchmark]
		public void UnsafeAsByte_InNonZero()
		{
			var array = Array;
			var length = Array.Length;
			var result = 0;
			for (var i = 0; i < length; i++)
				result += CastAndNormalizeToInt32(array[i] < 0.5);
			Result = result;
		}
		
		[Benchmark]
		public void UnsafeAsByte()
		{
			var array = Array;
			var length = Array.Length;
			var result = 0;
			for (var i = 0; i < length; i++)
				result += CastToInt32(array[i] < 0.5);
			Result = result;
		}
	
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int IsNonZero(int x) => 
			(int)((uint)~(~x & (x - 1)) >> 31);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int CastToInt32(bool b) => 
			Unsafe.As<bool, int>(ref b);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int CastAndNormalizeToInt32(bool b) => 
			IsNonZero(Unsafe.As<bool, int>(ref b));
	}
}
