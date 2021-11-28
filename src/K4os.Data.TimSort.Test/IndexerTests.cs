using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Indexers;
using K4os.Data.TimSort.Test.Utilities;
using Xunit;

namespace K4os.Data.TimSort.Test
{
	public abstract unsafe class GenericIndexerTests<T, TIndexer, TReference>
		where TIndexer: IIndexer<T, TReference>
		where TReference: IReference<TReference>
	{
		protected T[] Values;

		public abstract T Box(double value);
		public abstract double Unbox(T box);

		protected ref byte Byte0 => ref Unsafe.As<T, byte>(ref Values[0]);
		protected abstract TIndexer Indexer(byte* byte0);

		protected GenericIndexerTests() { Setup(); }

		private void Setup()
		{
			Values = new T[1000];
			for (var i = 0; i < Values.Length; i++)
				Values[i] = Box(i * Math.PI);
		}

		private static void AssertEqual(IReadOnlyList<T> array, TIndexer indexer)
		{
			var length = array.Count;
			var ref0 = indexer.Ref0;
			for (var i = 0; i < length; i++)
				Assert.Equal(array[i], indexer[ref0.Add(i)]);
		}

		private static void Swap(IList<T> array, int source, int target) =>
			(array[source], array[target]) = (array[target], array[source]);

		[Fact]
		public void ValuesCanBeRead()
		{
			var array = Values;

			fixed (byte* byte0 = &Byte0)
			{
				var indexer = Indexer(byte0);
				AssertEqual(array, indexer);
			}
		}

		[Theory]
		[InlineData(0, 50, 10)]
		[InlineData(33, 55, 10)]
		[InlineData(33, 55, 40)]
		[InlineData(77, 70, 20)]
		[InlineData(3, 3, 20)]
		[InlineData(34, 39, 0)]
		public void ValuesCanBeCopied(int source, int target, int length)
		{
			var clone = Values.ToArray();

			fixed (byte* byte0 = &Byte0)
			{
				var indexer = Indexer(byte0);
				var ref0 = indexer.Ref0;

				clone.AsSpan(source, length).CopyTo(clone.AsSpan(target, length));
				indexer.Copy(ref0.Add(source), ref0.Add(target), length);

				AssertEqual(clone, indexer);
			}
		}

		[Theory]
		[InlineData(0, 50)]
		[InlineData(33, 55)]
		[InlineData(77, 70)]
		[InlineData(3, 3)]
		[InlineData(34, 39)]
		public void ValuesCanBeSwapped(int source, int target)
		{
			var clone = Values.ToArray();

			fixed (byte* byte0 = &Byte0)
			{
				var indexer = Indexer(byte0);
				var ref0 = indexer.Ref0;

				Swap(clone, source, target);
				indexer.Swap(ref0.Add(source), ref0.Add(target));

				AssertEqual(clone, indexer);
			}
		}

		[Theory]
		[InlineData(0, 50)]
		[InlineData(33, 55)]
		[InlineData(3, 3)]
		[InlineData(34, 39)]
		public void RangeCanBeReversed(int lo, int hi)
		{
			var clone = Values.ToArray();

			fixed (byte* byte0 = &Byte0)
			{
				var indexer = Indexer(byte0);
				var ref0 = indexer.Ref0;

				Array.Reverse(clone, lo, hi - lo);
				indexer.Reverse(ref0.Add(lo), ref0.Add(hi));

				AssertEqual(clone, indexer);
			}
		}

		[Theory]
		[InlineData(0, 50)]
		[InlineData(33, 55)]
		[InlineData(3, 3)]
		[InlineData(34, 39)]
		public void ValuesCanBeExported(int source, int length)
		{
			fixed (byte* byte0 = &Byte0)
			{
				var indexer = Indexer(byte0);
				var ref0 = indexer.Ref0;
				var cloned = new T[length];

				indexer.Export(ref0.Add(source), cloned.AsSpan(), length);

				for (var i = 0; i < length; i++)
					Assert.Equal(indexer[ref0.Add(source + i)], cloned[i]);
			}
		}
		
		[Theory]
		[InlineData(0, 50)]
		[InlineData(33, 55)]
		[InlineData(3, 3)]
		[InlineData(34, 39)]
		public void ValuesCanBeImported(int source, int length)
		{
			fixed (byte* byte0 = &Byte0)
			{
				var indexer = Indexer(byte0);
				var ref0 = indexer.Ref0;
				var cloned = new T[length];
				for (var i = 0; i < length; i++) 
					cloned[i] = Box(i * i);

				indexer.Import(ref0.Add(source), cloned.AsSpan(), length);

				for (var i = 0; i < length; i++)
					Assert.Equal(indexer[ref0.Add(source + i)], cloned[i]);
			}
		}
	}

	public unsafe class ValueTypePtrReference:
		GenericIndexerTests<double, PtrIndexer<double>, PtrReference<double>>
	{
		public override double Box(double value) => value;
		public override double Unbox(double box) => box;
		protected override PtrIndexer<double> Indexer(byte* byte0) => new(byte0);
	}

	public unsafe class ValueTypeIntReference:
		GenericIndexerTests<double, ListIndexer<double>, IntReference>
	{
		public override double Box(double value) => value;
		public override double Unbox(double box) => box;
		protected override ListIndexer<double> Indexer(byte* _) => new(Values);
	}

	public unsafe class ReferenceTypePtrReference:
		GenericIndexerTests<
			ClassWrapper<double>,
			PtrIndexer<ClassWrapper<double>>,
			PtrReference<ClassWrapper<double>>
		>
	{
		public override ClassWrapper<double> Box(double value) => new() { Value = value };
		public override double Unbox(ClassWrapper<double> box) => box.Value;
		protected override PtrIndexer<ClassWrapper<double>> Indexer(byte* byte0) => new(byte0);
	}

	public unsafe class ReferenceTypeIntReference:
		GenericIndexerTests<ClassWrapper<double>, ListIndexer<ClassWrapper<double>>, IntReference>
	{
		public override ClassWrapper<double> Box(double value) => new() { Value = value };
		public override double Unbox(ClassWrapper<double> box) => box.Value;
		protected override ListIndexer<ClassWrapper<double>> Indexer(byte* _) => new(Values);
	}
}
