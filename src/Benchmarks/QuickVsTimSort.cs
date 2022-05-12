using System;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using Benchmarks.Fakes;
using K4os.Data.TimSort;

namespace Benchmarks
{
	public abstract class SortingBenchmark<T>
	{
		public T[] Data;
		public T[] Copy;

		public abstract T New(Random r);
		public abstract int Cmp(T a, T b);
		
		[Params(10, 100, 1_000, 10_000, 100_000)]
		public int Size { get; set; }
		
		[Params(DataOrder.Random, DataOrder.Ascending, DataOrder.Descending)]
		public DataOrder Order { get; set; }
		
		public object Result { get; set; }

		[GlobalSetup]
		public void Setup()
		{
			FakeData.Load();
			
			var a = new T[Size];
			var r = new Random(0);
			for (var i = 0; i < a.Length; i++) a[i] = New(r);
			if (Order != DataOrder.Random) Array.Sort(a);
			if (Order == DataOrder.Descending) Array.Reverse(a);

			Data = a;
			Copy = new T[a.Length];
		}

		public T[] GetData()
		{
			Data.CopyTo(Copy, 0);
			return Copy;
		}
		
		[Benchmark]
		public void Background()
		{
			Result = GetData();
		}

		[Benchmark]
		public void TimSort_Native()
		{
			var a = GetData();
			a.TimSort();
		}
		
		[Benchmark]
		public void IntroSort_Native()
		{
			var a = GetData();
			a.IntroSort();
		}
		
		[Benchmark]
		public void TimSort_Virtual()
		{
			var a = GetData();
			a.TimSort(Cmp);
		}
		
		[Benchmark]
		public void IntroSort_Virtual()
		{
			var a = GetData();
			a.IntroSort(Cmp);
		}
		
//		[Benchmark]
//		public void QuadSort_Native()
//		{
//			var a = GetData();
//			a.QuadSort();
//		}
	}
	
	public class QuickVsTimSortInt32: SortingBenchmark<int>
	{
		public override int New(Random r) => r.Next();
		public override int Cmp(int a, int b) => a - b;
	}
	
	public class QuickVsTimSortDouble: SortingBenchmark<double>
	{
		public override double New(Random r) => r.NextDouble() * Size;
		public override int Cmp(double a, double b) => a.CompareTo(b);
	}
	
	public class QuickVsTimSortGuid: SortingBenchmark<Guid>
	{
		public override Guid New(Random _) => Guid.NewGuid();
		public override int Cmp(Guid a, Guid b) => a.CompareTo(b);
	}
	
	public class QuickVsTimSortString: SortingBenchmark<string>
	{
		public override string New(Random _) => Guid.NewGuid().ToString();
		public override int Cmp(string a, string b) => string.CompareOrdinal(a, b);
	}
	
	public class QuickVsTimSortUserDetails: SortingBenchmark<UserDetails>
	{
		public override UserDetails New(Random r) => new() {
			FirstName = FakeData.FirstName(r.Next()),
			LastName = FakeData.LastName(r.Next()),
			DateOfBirth = FakeData.DateOfBirth(r),
		};

		public override int Cmp(UserDetails a, UserDetails b) => a.CompareTo(b);
	}


	public class UserDetails: IComparable<UserDetails>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }

		[SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
		[SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
		public int CompareTo(UserDetails other)
		{
			if (ReferenceEquals(this, other)) return 0;
			if (ReferenceEquals(null, other)) return 1;

			int c;

			c = string.Compare(LastName, other.LastName, StringComparison.Ordinal);
			if (c != 0) return c;
			
			c = string.Compare(FirstName, other.FirstName, StringComparison.Ordinal);
			if (c != 0) return c;

			return DateOfBirth.CompareTo(other.DateOfBirth);
		}
	}
}
