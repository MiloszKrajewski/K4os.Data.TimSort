using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Indexers;
using K4os.Data.TimSort.Test.Utilities;
using Xunit;

namespace K4os.Data.TimSort.Test
{
	public class IndexerTests
	{
		[Fact]
		public unsafe void ValueTypesCanBeSafelyReadByPtrIndexer()
		{
			var array = new double[1000];
			for (var i = 0; i < array.Length; i++)
				array[i] = i * 1.72;

			fixed (double* ptr0 = &array[0])
			{
				var indexer = new PtrIndexer<double>(ptr0);
				var ref0 = indexer.Ref0;
				for (var i = 0; i < array.Length; i++)
					Assert.Equal(array[i], indexer[ref0.Add(i)]);
			}
		}

		[Fact]
		public unsafe void ReferenceTypesCanBeSafelyReadByPtrIndexer()
		{
			var array = new ClassWrapper<double>[1000];
			for (var i = 0; i < array.Length; i++)
				array[i] = new ClassWrapper<double> { Value = i * 1.72 };

			fixed (void* ptr0 = &Unsafe.As<ClassWrapper<double>, byte>(ref array[0]))
			{
				var indexer = new PtrIndexer<ClassWrapper<double>>(ptr0);
				var ref0 = indexer.Ref0;
				for (var i = 0; i < array.Length; i++)
					Assert.Equal(array[i], indexer[ref0.Add(i)]);
			}
		}
	}
}
