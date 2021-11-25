using System;
using System.Collections.Generic;
using K4os.Data.TimSort.Internals;
using Xunit;

namespace K4os.Data.TimSort.Test
{
	public class ArrayExtractorTests
	{
		[Fact]
		public void CanExtractArrayFromList()
		{
			var list = new List<double> { 1, 1337, Math.PI };
			var array = ArrayExtractor<double>.GetArray(list);

			Assert.NotNull(array);
			Assert.True(array.Length >= 3);
			Assert.Equal(1, array[0]);
			Assert.Equal(1337, array[1]);
			Assert.Equal(Math.PI, array[2]);
		}

		[Fact]
		public void CanExtractSpanFromIList()
		{
			var actual = new List<double> { 1, 1337, Math.PI };
			var list = (ICollection<double>)actual;

			var success = SpanExtractor.TryGetSpan(list, out var span);

			Assert.True(success);
			Assert.Equal(3, span.Length);
		}
		
		[Fact]
		public void CanExtractSpanFromArrayAsIList()
		{
			var list = new[] { 1, 1337, Math.PI };
			var success = SpanExtractor.TryGetSpan(list, out var span);
			Assert.True(success);
			Assert.Equal(3, span.Length);
		}

		[Fact]
		public void WhenSpanCannotGetExtractedItIsReportedNicely()
		{
			var actual = new HashSet<double> { 1, 1337, Math.PI };
			var list = (ICollection<double>)actual;

			var success = SpanExtractor.TryGetSpan(list, out _);

			Assert.False(success);
		}
	}
}
