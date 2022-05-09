using System;
using System.Linq;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;
using ReferenceImplementation.Quad;

namespace ReferenceImplementation
{
	unsafe class Program
	{
		static void Main(string[] args)
		{
			var array = Enumerable.Range(1, 153).Select(i => -Math.Round(11.0 * i)).ToArray();
			var length = array.Length;

			fixed (double* ptr0 = &array[0])
			{
				var indexer = new PtrIndexer<double>(ptr0);
				QuadSort2<
					double, PtrIndexer<double>, PtrReference<double>, DefaultLessThan<double>
				>.quad_sort(indexer, length, default);
			}

			for (var i = 0; i < array.Length; i++)
				Console.WriteLine(array[i]);
		}
	}
}
