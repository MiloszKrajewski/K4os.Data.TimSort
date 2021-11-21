using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Benchmarks.FiddleArea
{
	public interface ILessThan<in T>
	{
		bool Lt(T a, T b);
	}

	public interface IReference<TReference>
	{
		bool Eq(TReference other);
		bool Lt(TReference other);
		TReference Ofs(int offset);
		int Dif(TReference other);
	}

	public interface IIndexer<T, TReference>
		where TReference: IReference<TReference>
	{
		TReference Ref0 { get; }

		T this[TReference reference] { get; set; }

		void Swap(TReference a, TReference b);
		void Copy(TReference source, TReference target, int length);
	}

	public static class ReferenceExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TRef Inc<TRef>(this TRef reference) where TRef: IReference<TRef> =>
			reference.Ofs(1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TRef Dec<TRef>(this TRef reference) where TRef: IReference<TRef> =>
			reference.Ofs(-1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TRef Add<TRef>(this TRef reference, int offset)
			where TRef: IReference<TRef> =>
			reference.Ofs(offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TRef Sub<TRef>(this TRef reference, int offset)
			where TRef: IReference<TRef> =>
			reference.Ofs(-offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Gt<T>(this T subject, T other) where T: IReference<T> =>
			other.Lt(subject);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LtEq<T>(this T subject, T other) where T: IReference<T> =>
			!other.Lt(subject);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GtEq<T>(this T subject, T other) where T: IReference<T> =>
			!subject.Lt(other);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool NEq<T>(this T subject, T other) where T: IReference<T> =>
			!subject.Eq(other);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Mid<T>(this T lo, T hi) where T: IReference<T> =>
			lo.Add(hi.Dif(lo) >> 1);
	}

	public static class LessThanExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Gt<TLessThan, T>(this TLessThan comparer, T a, T b)
			where TLessThan: ILessThan<T> =>
			comparer.Lt(b, a);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LtEq<TLessThan, T>(this TLessThan comparer, T a, T b)
			where TLessThan: ILessThan<T> =>
			!comparer.Lt(b, a);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GtEq<TLessThan, T>(this TLessThan comparer, T a, T b)
			where TLessThan: ILessThan<T> =>
			!comparer.Lt(a, b);
	}

	public readonly unsafe struct SpanReference<T>: IReference<SpanReference<T>>
	{
		private readonly byte* _ptr;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SpanReference(void* ptr) => _ptr = (byte*)ptr;

		private static uint SizeOfT
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (uint)Unsafe.SizeOf<T>();
		}

		public byte* Ptr
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _ptr;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SpanReference<T> Ofs(int offset) =>
			new(_ptr + offset * SizeOfT);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int Dif(SpanReference<T> other) =>
			(int)((_ptr - other.Ptr) / SizeOfT);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Eq(SpanReference<T> other) => _ptr == other.Ptr;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(SpanReference<T> other) => _ptr < other.Ptr;
	}

	public readonly unsafe struct SpanIndexer<T>: IIndexer<T, SpanReference<T>>
	{
		private readonly SpanReference<T> _ref0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SpanIndexer(void* ptr0) => _ref0 = new SpanReference<T>(ptr0);

		public SpanReference<T> Ref0
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _ref0;
		}

		public T this[SpanReference<T> reference]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Unsafe.Read<T>(reference.Ptr);
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Unsafe.Write(reference.Ptr, value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Swap(SpanReference<T> a, SpanReference<T> b)
		{
			// ReSharper disable once SwapViaDeconstruction
			var t = this[a];
			this[a] = this[b];
			this[b] = t;
		}
	}

	public readonly struct ComparableLessThan<T>: ILessThan<T>
		where T: IComparable<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static TOther As<TOther>(ref T a) => Unsafe.As<T, TOther>(ref a);

		// this works because when generic class in expanded only one branch survives
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SuppressMessage("ReSharper", "RedundantTernaryExpression")]
		public bool Lt(T a, T b) =>
			typeof(T) == typeof(bool) ? !As<bool>(ref a) && As<bool>(ref b) ? true : false :
			typeof(T) == typeof(byte) ? As<byte>(ref a) < As<byte>(ref b) ? true : false :
			typeof(T) == typeof(sbyte) ? As<sbyte>(ref a) < As<sbyte>(ref b) ? true : false :
			typeof(T) == typeof(short) ? As<short>(ref a) < As<short>(ref b) ? true : false :
			typeof(T) == typeof(ushort) ? As<ushort>(ref a) < As<ushort>(ref b) ? true : false :
			typeof(T) == typeof(int) ? As<int>(ref a) < As<int>(ref b) ? true : false :
			typeof(T) == typeof(uint) ? As<uint>(ref a) < As<uint>(ref b) ? true : false :
			typeof(T) == typeof(long) ? As<long>(ref a) < As<long>(ref b) ? true : false :
			typeof(T) == typeof(ulong) ? As<ulong>(ref a) < As<ulong>(ref b) ? true : false :
			typeof(T) == typeof(float) ? As<float>(ref a) < As<float>(ref b) ? true : false :
			typeof(T) == typeof(double) ? As<double>(ref a) < As<double>(ref b) ? true : false :
			typeof(T) == typeof(decimal) ? As<decimal>(ref a) < As<decimal>(ref b) ? true : false :
			// not sure if ones below are worth inlining (or "? true : false"-ing)...
			typeof(T) == typeof(DateTime) ? As<DateTime>(ref a) < As<DateTime>(ref b) :
			typeof(T) == typeof(TimeSpan) ? As<TimeSpan>(ref a) < As<TimeSpan>(ref b) :
			typeof(T) == typeof(DateTimeOffset) ? LtDateTimeOffset(a, b) :
			// ...and fallback
			a.CompareTo(b) < 0 ? true : false;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool LtDateTimeOffset(T a, T b) =>
			As<DateTimeOffset>(ref a) < As<DateTimeOffset>(ref b);
	}
	
	public readonly struct DefaultLessThan<T>: ILessThan<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static TOther As<TOther>(ref T a) => Unsafe.As<T, TOther>(ref a);

		// this works because when generic class in expanded only one branch survives
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[SuppressMessage("ReSharper", "RedundantTernaryExpression")]
		public bool Lt(T a, T b) =>
			typeof(T) == typeof(bool) ? !As<bool>(ref a) && As<bool>(ref b) ? true : false :
			typeof(T) == typeof(byte) ? As<byte>(ref a) < As<byte>(ref b) ? true : false :
			typeof(T) == typeof(sbyte) ? As<sbyte>(ref a) < As<sbyte>(ref b) ? true : false :
			typeof(T) == typeof(short) ? As<short>(ref a) < As<short>(ref b) ? true : false :
			typeof(T) == typeof(ushort) ? As<ushort>(ref a) < As<ushort>(ref b) ? true : false :
			typeof(T) == typeof(int) ? As<int>(ref a) < As<int>(ref b) ? true : false :
			typeof(T) == typeof(uint) ? As<uint>(ref a) < As<uint>(ref b) ? true : false :
			typeof(T) == typeof(long) ? As<long>(ref a) < As<long>(ref b) ? true : false :
			typeof(T) == typeof(ulong) ? As<ulong>(ref a) < As<ulong>(ref b) ? true : false :
			typeof(T) == typeof(float) ? As<float>(ref a) < As<float>(ref b) ? true : false :
			typeof(T) == typeof(double) ? As<double>(ref a) < As<double>(ref b) ? true : false :
			typeof(T) == typeof(decimal) ? As<decimal>(ref a) < As<decimal>(ref b) ? true : false :
			// not sure if ones below are worth inlining (or "? true : false"-ing)...
			typeof(T) == typeof(DateTime) ? As<DateTime>(ref a) < As<DateTime>(ref b) :
			typeof(T) == typeof(TimeSpan) ? As<TimeSpan>(ref a) < As<TimeSpan>(ref b) :
			typeof(T) == typeof(DateTimeOffset) ? LtDateTimeOffset(a, b) :
			// ...and fallback
			Comparer<T>.Default.Compare(a, b) < 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool LtDateTimeOffset(T a, T b) =>
			As<DateTimeOffset>(ref a) < As<DateTimeOffset>(ref b);
	}


	public readonly struct LessThanDouble: ILessThan<double>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(double a, double b) => a < b;
	}
}
