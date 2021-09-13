// ReSharper disable BuiltInTypeReferenceStyle

using System;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort
{
	public partial class TimComparer
	{
        ///<summary>Native comparer for <see cref="Byte"/>.</summary>
        public static readonly ByteComparer Byte = new();
        ///<summary>Native comparer for <see cref="SByte"/>.</summary>
        public static readonly SByteComparer SByte = new();
        ///<summary>Native comparer for <see cref="Int16"/>.</summary>
        public static readonly Int16Comparer Int16 = new();
        ///<summary>Native comparer for <see cref="UInt16"/>.</summary>
        public static readonly UInt16Comparer UInt16 = new();
        ///<summary>Native comparer for <see cref="Int32"/>.</summary>
        public static readonly Int32Comparer Int32 = new();
        ///<summary>Native comparer for <see cref="UInt32"/>.</summary>
        public static readonly UInt32Comparer UInt32 = new();
        ///<summary>Native comparer for <see cref="Int64"/>.</summary>
        public static readonly Int64Comparer Int64 = new();
        ///<summary>Native comparer for <see cref="UInt64"/>.</summary>
        public static readonly UInt64Comparer UInt64 = new();
        ///<summary>Native comparer for <see cref="Single"/>.</summary>
        public static readonly SingleComparer Single = new();
        ///<summary>Native comparer for <see cref="Double"/>.</summary>
        public static readonly DoubleComparer Double = new();
        ///<summary>Native comparer for <see cref="Decimal"/>.</summary>
        public static readonly DecimalComparer Decimal = new();
        ///<summary>Native comparer for <see cref="DateTime"/>.</summary>
        public static readonly DateTimeComparer DateTime = new();
        ///<summary>Native comparer for <see cref="DateTimeOffset"/>.</summary>
        public static readonly DateTimeOffsetComparer DateTimeOffset = new();
	}

    ///<summary>Native comparer for <see cref="Byte"/>.</summary>
	public struct ByteComparer: ITimComparer<Byte>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in Byte a, in Byte b) => a < b;
	}

    ///<summary>Native comparer for <see cref="SByte"/>.</summary>
	public struct SByteComparer: ITimComparer<SByte>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in SByte a, in SByte b) => a < b;
	}

    ///<summary>Native comparer for <see cref="Int16"/>.</summary>
	public struct Int16Comparer: ITimComparer<Int16>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in Int16 a, in Int16 b) => a < b;
	}

    ///<summary>Native comparer for <see cref="UInt16"/>.</summary>
	public struct UInt16Comparer: ITimComparer<UInt16>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in UInt16 a, in UInt16 b) => a < b;
	}

    ///<summary>Native comparer for <see cref="Int32"/>.</summary>
	public struct Int32Comparer: ITimComparer<Int32>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in Int32 a, in Int32 b) => a < b;
	}

    ///<summary>Native comparer for <see cref="UInt32"/>.</summary>
	public struct UInt32Comparer: ITimComparer<UInt32>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in UInt32 a, in UInt32 b) => a < b;
	}

    ///<summary>Native comparer for <see cref="Int64"/>.</summary>
	public struct Int64Comparer: ITimComparer<Int64>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in Int64 a, in Int64 b) => a < b;
	}

    ///<summary>Native comparer for <see cref="UInt64"/>.</summary>
	public struct UInt64Comparer: ITimComparer<UInt64>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in UInt64 a, in UInt64 b) => a < b;
	}

    ///<summary>Native comparer for <see cref="Single"/>.</summary>
	public struct SingleComparer: ITimComparer<Single>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in Single a, in Single b) => a < b;
	}

    ///<summary>Native comparer for <see cref="Double"/>.</summary>
	public struct DoubleComparer: ITimComparer<Double>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in Double a, in Double b) => a < b;
	}

    ///<summary>Native comparer for <see cref="Decimal"/>.</summary>
	public struct DecimalComparer: ITimComparer<Decimal>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in Decimal a, in Decimal b) => a < b;
	}

    ///<summary>Native comparer for <see cref="DateTime"/>.</summary>
	public struct DateTimeComparer: ITimComparer<DateTime>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in DateTime a, in DateTime b) => a < b;
	}

    ///<summary>Native comparer for <see cref="DateTimeOffset"/>.</summary>
	public struct DateTimeOffsetComparer: ITimComparer<DateTimeOffset>
	{
		/// <inheritdoc />
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Lt(in DateTimeOffset a, in DateTimeOffset b) => a < b;
	}

}

