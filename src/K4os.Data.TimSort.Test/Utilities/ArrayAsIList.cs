using System.Collections;
using System.Collections.Generic;

namespace K4os.Data.TimSort.Test.Utilities
{
	public class ArrayAsIList<T>: IList<T>
	{
		private readonly T[] _array;
		private IList<T> AsList => _array;

		public ArrayAsIList(T[] array) => _array = array;
		public IEnumerator<T> GetEnumerator() => AsList.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => AsList.GetEnumerator();
		public void Add(T item) => AsList.Add(item);
		public void Clear() => AsList.Clear();
		public bool Contains(T item) => AsList.Contains(item);
		public void CopyTo(T[] array, int arrayIndex) => AsList.CopyTo(array, arrayIndex);
		public bool Remove(T item) => AsList.Remove(item);
		public int Count => AsList.Count;
		public bool IsReadOnly => AsList.IsReadOnly;
		public int IndexOf(T item) => AsList.IndexOf(item);
		public void Insert(int index, T item) => AsList.Insert(index, item);
		public void RemoveAt(int index) => AsList.RemoveAt(index);

		public T this[int index]
		{
			get => AsList[index];
			set => AsList[index] = value;
		}
	}
}
