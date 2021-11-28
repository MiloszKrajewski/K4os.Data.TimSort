using System;

namespace K4os.Data.TimSort.Test.Utilities
{
	public readonly struct ReveredComparableWrapper<T>:
		IComparable<ReveredComparableWrapper<T>>
		where T: IComparable<T>
	{
		public readonly T Value;
		public ReveredComparableWrapper(T value) => Value = value;
		public int CompareTo(ReveredComparableWrapper<T> other) => -Value.CompareTo(other.Value);
	}
}
