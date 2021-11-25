using System;

namespace K4os.Data.TimSort.Test.Utilities
{
	public readonly struct ComparableStructWrapper<T>:
		IComparable<ComparableStructWrapper<T>>
		where T: IComparable<T>
	{
		public readonly T Value;
		public ComparableStructWrapper(T value) => Value = value;
		public int CompareTo(ComparableStructWrapper<T> other) => Value.CompareTo(other.Value);
	}
}
