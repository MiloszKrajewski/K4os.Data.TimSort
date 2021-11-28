using System;
using K4os.Data.TimSort.Indexers;
using Xunit;

namespace K4os.Data.TimSort.Test
{
	public abstract class ReferenceTests<TReference> 
		where TReference: struct, IReference<TReference>
	{
		public abstract TReference Ref0 { get; }

		[Fact]
		public void IncDec()
		{
			var ref0 = Ref0;
			
			Assert.True(ref0.Inc().Gt(ref0));
			Assert.True(ref0.Inc().Dec().Eq(ref0));
			Assert.True(ref0.Dec().Lt(ref0));
			Assert.True(ref0.Dec().Inc().Eq(ref0));
			Assert.True(ref0.Add(1).Eq(ref0.Inc()));
			Assert.True(ref0.Add(2).Eq(ref0.Inc().Inc()));
			Assert.True(ref0.Add(3).Eq(ref0.Inc().Inc().Inc()));
			Assert.True(ref0.Add(-3).Eq(ref0.Dec().Dec().Dec()));
			Assert.True(ref0.Sub(3).Eq(ref0.Add(-3)));
		}

		[Fact]
		public void DifMid()
		{
			var ref0 = Ref0;
			Assert.Equal(100, ref0.Add(100).Dif(ref0));
			Assert.True(ref0.Ofs(3).Eq(ref0.Add(3)));
			Assert.Equal(50, ref0.Mid(ref0.Add(100)).Dif(ref0));
			Assert.Equal(0, ref0.Mid(ref0.Inc()).Dif(ref0));
			Assert.Equal(1, ref0.Mid(ref0.Add(2)).Dif(ref0));
		}
		
		[Fact]
		public void ReferenceComparison()
		{
			var lo = Ref0;
			var hi = Ref0.Add(1);
			
			Assert.True(lo.Lt(hi));
			Assert.True(lo.LtEq(hi));
			Assert.True(lo.LtEq(lo));
			
			Assert.True(lo.Eq(lo));
			Assert.True(lo.NEq(hi));
			
			Assert.True(hi.Gt(lo));
			Assert.True(hi.GtEq(lo));
			Assert.True(hi.GtEq(hi));
		}

		[Fact]
		public void PostInc()
		{
			var a = Ref0;
			var b = Ref0;
			
			Assert.Equal(1, a.Inc().Dif(a));
			Assert.True(a.PostInc().Eq(b));
			Assert.True(a.Eq(b.Inc()));
		}
		
		[Fact]
		public void PostDec()
		{
			var a = Ref0;
			var b = Ref0;
			
			Assert.Equal(-1, a.Dec().Dif(a));
			Assert.True(a.PostDec().Eq(b));
			Assert.True(a.Eq(b.Dec()));
		}
	}

	public class IntReferenceTests: ReferenceTests<IntReference>
	{
		public override IntReference Ref0 => 0x12345678;
	}
	
	public class PtrReferenceTests: ReferenceTests<PtrReference<double>>
	{
		public override unsafe PtrReference<double> Ref0 =>
			new(new IntPtr(0x12345678).ToPointer());
	}

}
