using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using K4os.Data.TimSort.Comparers;
using K4os.Data.TimSort.Indexers;

namespace ReferenceImplementation.Quad;

[SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
[SuppressMessage("ReSharper", "TooWideLocalVariableScope")]
public class QuadSort2Tools
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int I(bool v) => Unsafe.As<bool, byte>(ref v);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int N(int v) => v ^ 1;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void parity_merge_two<
		T, TIndexerA, TIndexerS, TReferenceA, TReferenceS, TLessThan
	>(
		TIndexerA arr, TReferenceA arr0,
		TIndexerS swp, TReferenceS swp0,
		TLessThan cmp
	)
		where TIndexerA: IIndexer<T, TReferenceA>
		where TIndexerS: IIndexer<T, TReferenceS>
		where TReferenceA: IReference<TReferenceA>
		where TReferenceS: IReference<TReferenceS>
		where TLessThan: ILessThan<T>
	{
		TReferenceA ptl, ptr;
		TReferenceS pts;
		int x, y;

		// ptl = array + 0; ptr = array + 2; pts = swap + 0;
		ptl = arr0;
		ptr = arr0.Add(2);
		pts = swp0;

		// x = cmp(ptl, ptr) <= 0; y = !x; pts[x] = *ptr; ptr += y; pts[y] = *ptl; ptl += x; pts++;
		y = N(x = I(cmp.LtEq(arr[ptl], arr[ptr])));
		swp[pts.Add(x)] = arr[ptr];
		ptr = ptr.Add(y);
		swp[pts.Add(y)] = arr[ptl];
		ptl = ptl.Add(x);
		pts = pts.Inc();

		// *pts = cmp(ptl, ptr) <= 0 ? *ptl : *ptr;
		swp[pts] = arr[cmp.LtEq(arr[ptl], arr[ptr]) ? ptl : ptr];

		// ptl = array + 1; ptr = array + 3; pts = swap + 3;
		ptl = arr0.Add(1);
		ptr = arr0.Add(3);
		pts = swp0.Add(3);

		// x = cmp(ptl, ptr) <= 0; y = !x; pts--; pts[x] = *ptr; ptr -= x; pts[y] = *ptl; ptl -= y; 
		y = N(x = I(cmp.LtEq(arr[ptl], arr[ptr])));
		pts = pts.Dec();
		swp[pts.Add(x)] = arr[ptr];
		ptr = ptr.Sub(x);
		swp[pts.Add(y)] = arr[ptl];
		ptl = ptl.Sub(y);

		// *pts = cmp(ptl, ptr)  > 0 ? *ptl : *ptr;
		swp[pts] = arr[cmp.Gt(arr[ptl], arr[ptr]) ? ptl : ptr];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void parity_merge_four<
		T, TIndexerA, TIndexerS, TReferenceA, TReferenceS, TLessThan
	>(
		TIndexerA arr, TReferenceA arr0,
		TIndexerS swp, TReferenceS swp0,
		TLessThan cmp
	)
		where TIndexerA: IIndexer<T, TReferenceA>
		where TIndexerS: IIndexer<T, TReferenceS>
		where TReferenceA: IReference<TReferenceA>
		where TReferenceS: IReference<TReferenceS>
		where TLessThan: ILessThan<T>
	{
		TReferenceA ptl, ptr;
		TReferenceS pts;
		int x, y;

		// ptl = array + 0; ptr = array + 4; pts = swap;
		ptl = arr0;
		ptr = arr0.Add(4);
		pts = swp0;

		// x = cmp(ptl, ptr) <= 0; y = !x; pts[x] = *ptr; ptr += y; pts[y] = *ptl; ptl += x; pts++;
		y = N(x = I(cmp.LtEq(arr[ptl], arr[ptr])));
		swp[pts.Add(x)] = arr[ptr];
		ptr = ptr.Add(y);
		swp[pts.Add(y)] = arr[ptl];
		ptl = ptl.Add(x);
		pts = pts.Inc();

		// x = cmp(ptl, ptr) <= 0; y = !x; pts[x] = *ptr; ptr += y; pts[y] = *ptl; ptl += x; pts++;
		y = N(x = I(cmp.LtEq(arr[ptl], arr[ptr])));
		swp[pts.Add(x)] = arr[ptr];
		ptr = ptr.Add(y);
		swp[pts.Add(y)] = arr[ptl];
		ptl = ptl.Add(x);
		pts = pts.Inc();

		// x = cmp(ptl, ptr) <= 0; y = !x; pts[x] = *ptr; ptr += y; pts[y] = *ptl; ptl += x; pts++;
		y = N(x = I(cmp.LtEq(arr[ptl], arr[ptr])));
		swp[pts.Add(x)] = arr[ptr];
		ptr = ptr.Add(y);
		swp[pts.Add(y)] = arr[ptl];
		ptl = ptl.Add(x);
		pts = pts.Inc();

		// *pts = cmp(ptl, ptr) <= 0 ? *ptl : *ptr;
		swp[pts] = arr[cmp.LtEq(arr[ptl], arr[ptr]) ? ptl : ptr];

		// ----

		// ptl = array + 3; ptr = array + 7; pts = swap + 7;
		ptl = arr0.Add(3);
		ptr = arr0.Add(7);
		pts = swp0.Add(7);

		// x = cmp(ptl, ptr) <= 0; y = !x; pts--; pts[x] = *ptr; ptr -= x; pts[y] = *ptl; ptl -= y;
		y = N(x = I(cmp.LtEq(arr[ptl], arr[ptr])));
		pts = pts.Dec();
		swp[pts.Add(x)] = arr[ptr];
		ptr = ptr.Sub(x);
		swp[pts.Add(y)] = arr[ptl];
		ptl = ptl.Sub(y);

		// x = cmp(ptl, ptr) <= 0; y = !x; pts--; pts[x] = *ptr; ptr -= x; pts[y] = *ptl; ptl -= y;
		y = N(x = I(cmp.LtEq(arr[ptl], arr[ptr])));
		pts = pts.Dec();
		swp[pts.Add(x)] = arr[ptr];
		ptr = ptr.Sub(x);
		swp[pts.Add(y)] = arr[ptl];
		ptl = ptl.Sub(y);

		// x = cmp(ptl, ptr) <= 0; y = !x; pts--; pts[x] = *ptr; ptr -= x; pts[y] = *ptl; ptl -= y;
		y = N(x = I(cmp.LtEq(arr[ptl], arr[ptr])));
		pts = pts.Dec();
		swp[pts.Add(x)] = arr[ptr];
		ptr = ptr.Sub(x);
		swp[pts.Add(y)] = arr[ptl];
		ptl = ptl.Sub(y);

		// *pts = cmp(ptl, ptr)  > 0 ? *ptl : *ptr;
		swp[pts] = arr[cmp.Gt(arr[ptl], arr[ptr]) ? ptl : ptr];
	}

	public static void parity_merge<
		T, TIndexerA, TIndexerB, TReferenceA, TReferenceB, TLessThan
	>(
		TIndexerA dst, TReferenceA dst0,
		TIndexerB src, TReferenceB src0,
		int block, int limit,
		TLessThan cmp
	)
		where TIndexerA: IIndexer<T, TReferenceA>
		where TIndexerB: IIndexer<T, TReferenceB>
		where TReferenceA: IReference<TReferenceA>
		where TReferenceB: IReference<TReferenceB>
		where TLessThan: ILessThan<T>
	{
		// VAR *ptl, *ptr, *tpl, *tpr, *tpd, *ptd;
		// unsigned char x, y;
		TReferenceB ptl, ptr, tpl, tpr;
		TReferenceA tpd, ptd;
		int x, y;

		// ptl = from;
		// ptr = from + block;
		// ptd = dest;
		ptl = src0;
		ptr = src0.Add(block);
		ptd = dst0;

		// tpl = from + block - 1;
		// tpr = from + nmemb - 1;
		// tpd = dest + nmemb - 1;
		tpl = src0.Add(block - 1);
		tpr = src0.Add(limit - 1);
		tpd = dst0.Add(limit - 1);

		for (block--; block > 0; block--)
		{
			// x = cmp(ptl, ptr) <= 0; y = !x; ptd[x] = *ptr; ptr += y; ptd[y] = *ptl; ptl += x; ptd++;
			y = N(x = I(cmp.LtEq(src[ptl], src[ptr])));
			dst[ptd.Add(x)] = src[ptr];
			ptr = ptr.Add(y);
			dst[ptd.Add(y)] = src[ptl];
			ptl = ptl.Add(x);
			ptd = ptd.Inc();

			// x = cmp(tpl, tpr) <= 0; y = !x; tpd--; tpd[x] = *tpr; tpr -= x; tpd[y] = *tpl; tpl -= y;
			y = N(x = I(cmp.LtEq(src[tpl], src[tpr])));
			tpd = tpd.Dec();
			dst[tpd.Add(x)] = src[tpr];
			tpr = tpr.Sub(x);
			dst[tpd.Add(y)] = src[tpl];
			tpl = tpl.Sub(y);
		}

		// *ptd = cmp(ptl, ptr) <= 0 ? *ptl : *ptr;
		// *tpd = cmp(tpl, tpr)  > 0 ? *tpl : *tpr;
		dst[ptd] = src[cmp.LtEq(src[ptl], src[ptr]) ? ptl : ptr];
		dst[tpd] = src[cmp.Gt(src[tpl], src[tpr]) ? tpl : tpr];
	}

	public static void forward_merge<
		T, TIndexerA, TIndexerB, TReferenceA, TReferenceB, TLessThan
	>(
		TIndexerA dst, TReferenceA dst0,
		TIndexerB src, TReferenceB src0,
		int block,
		TLessThan cmp
	)
		where TIndexerA: IIndexer<T, TReferenceA>
		where TIndexerB: IIndexer<T, TReferenceB>
		where TReferenceA: IReference<TReferenceA>
		where TReferenceB: IReference<TReferenceB>
		where TLessThan: ILessThan<T>
	{
		// VAR *ptl, *ptr, *m, *e; // left, right, middle, end
		TReferenceB ptl, ptr, m, e; // left, right, middle, end

		ptl = src0;
		ptr = src0.Add(block);
		m = ptr;
		e = ptr.Add(block);

		// if (cmp(m - 1, e - block / 4) <= 0)
		if (cmp.LtEq(src[m.Add(-1)], src[e.Sub(block / 4)]))
		{
			do
			{
				// if (cmp(ptl, ptr) <= 0)
				if (cmp.LtEq(src[ptl], src[ptr]))
				{
					// *dest++ = *ptl++;
					dst[dst0] = src[ptl];
					dst0 = dst0.Inc();
					ptl = ptl.Inc();

					continue;
				}

				// *dest++ = *ptr++;
				dst[dst0] = src[ptr];
				dst0 = dst0.Inc();
				ptr = ptr.Inc();

				// if (cmp(ptl, ptr) <= 0)
				if (cmp.LtEq(src[ptl], src[ptr]))
				{
					// *dest++ = *ptl++;
					dst[dst0] = src[ptl];
					dst0 = dst0.Inc();
					ptl = ptl.Inc();

					continue;
				}

				// *dest++ = *ptr++;
				dst[dst0] = src[ptr];
				dst0 = dst0.Inc();
				ptr = ptr.Inc();

				// if (cmp(ptl, ptr) <= 0)
				if (cmp.LtEq(src[ptl], src[ptr]))
				{
					// *dest++ = *ptl++;
					dst[dst0] = src[ptl];
					dst0 = dst0.Inc();
					ptl = ptl.Inc();

					continue;
				}

				// *dest++ = *ptr++;
				dst[dst0] = src[ptr];
				dst0 = dst0.Inc();
				ptr = ptr.Inc();
			}
			while (ptl.Lt(m));

			// do *dest++ = *ptr++; while (ptr < e);
			do
			{
				// *dest++ = *ptr++;
				dst[dst0] = src[ptr];
				dst0 = dst0.Inc();
				ptr = ptr.Inc();
			}
			while (ptr.Lt(e));
		}
		// else if (cmp(m - block / 4, e - 1) > 0)
		else if (cmp.Gt(src[m.Sub(block / 4)], src[e.Sub(1)]))
		{
			do
			{
				// if (cmp(ptl, ptr) > 0)
				if (cmp.Gt(src[ptl], src[ptr]))
				{
					// *dest++ = *ptr++;
					dst[dst0] = src[ptr];
					dst0 = dst0.Inc();
					ptr = ptr.Inc();

					continue;
				}

				// *dest++ = *ptl++;
				dst[dst0] = src[ptl];
				dst0 = dst0.Inc();
				ptl = ptl.Inc();

				// if (cmp(ptl, ptr) > 0)
				if (cmp.Gt(src[ptl], src[ptr]))
				{
					// *dest++ = *ptr++;
					dst[dst0] = src[ptr];
					dst0 = dst0.Inc();
					ptr = ptr.Inc();
					continue;
				}

				// *dest++ = *ptl++;
				dst[dst0] = src[ptl];
				dst0 = dst0.Inc();
				ptl = ptl.Inc();

				// if (cmp(ptl, ptr) > 0)
				if (cmp.Gt(src[ptl], src[ptr]))
				{
					// *dest++ = *ptr++;
					dst[dst0] = src[ptr];
					dst0 = dst0.Inc();
					ptr = ptr.Inc();

					continue;
				}

				// *dest++ = *ptl++;
				dst[dst0] = src[ptl];
				dst0 = dst0.Inc();
				ptl = ptl.Inc();
			}
			while (ptr.Lt(e));

			// do *dest++ = *ptl++; while (ptl < m);
			do
			{
				// *dest++ = *ptl++;
				dst[dst0] = src[ptl];
				dst0 = dst0.Inc();
				ptl = ptl.Inc();
			}
			while (ptl.Lt(m));
		}
		else
		{
			// FUNC(parity_merge)(dest, from, block, block * 2, cmp);
			parity_merge<
				T, TIndexerA, TIndexerB, TReferenceA, TReferenceB, TLessThan
			>(dst, dst0, src, src0, block, block * 2, cmp);
		}
	}
}

[SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
[SuppressMessage("ReSharper", "TooWideLocalVariableScope")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class QuadSort2<T, TIndexer, TReference, TLessThan>
	where TIndexer: IIndexer<T, TReference>
	where TReference: struct, IReference<TReference>
	where TLessThan: ILessThan<T>
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int I(bool v) => Unsafe.As<bool, byte>(ref v);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int N(int v) => v ^ 1;

	public static void unguarded_insert(
		TIndexer arr, TReference arr0, int offset, int limit, TLessThan cmp)
	{
		T key;
		TReference pta, end;
		int top;

		for (var i = offset; i < limit; i++)
		{
			pta = end = arr0.Add(i);

			if (cmp.LtEq(arr[pta = pta.Dec()], arr[end]))
			{
				continue;
			}

			key = arr[end];

			if (cmp.Gt(arr[arr0], key))
			{
				top = i;

				do
				{
					arr[end] = arr[pta];
					end = end.Dec();
					pta = pta.Dec();
				}
				while (--top > 0);

				arr[end] = key;
			}
			else
			{
				do
				{
					arr[end] = arr[pta];
					end = end.Dec();
					pta = pta.Dec();
				}
				while (cmp.Gt(arr[pta], key));

				arr[end] = key;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void swap_two_if(
		TIndexer arr, TReference idx, PtrIndexer<T> swp, PtrReference<T> swpN, TLessThan cmp)
	{
		if (cmp.LtEq(arr[idx], arr[idx.Inc()]))
			return;

		swp[swpN] = arr[idx];
		arr[idx] = arr[idx.Inc()];
		arr[idx.Inc()] = swp[swpN];
	}

	public static void parity_swap_four(TIndexer arr, TReference arr0, TLessThan cmp)
	{
		// VAR swap[4], *ptl, *ptr, *pts;
		// unsigned char x, y;
		var swap4 = new Array4<T>();
		var swp = swap4.Indexer();
		var swp0 = swp.Ref0;
		int x, y;

		// x = cmp(array + 0, array + 1) <= 0; y = !x; swap[0 + y] = array[0]; swap[0 + x] = array[1];
		y = N(x = I(cmp.LtEq(arr[arr0], arr[arr0.Inc()])));
		swp[swp0.Add(y)] = arr[arr0];
		swp[swp0.Add(x)] = arr[arr0.Inc()];

		// x = cmp(array + 2, array + 3) <= 0; y = !x; swap[2 + y] = array[2]; swap[2 + x] = array[3];
		y = N(x = I(cmp.LtEq(arr[arr0.Add(2)], arr[arr0.Add(3)])));
		swp[swp0.Add(2 + y)] = arr[arr0.Add(2)];
		swp[swp0.Add(2 + x)] = arr[arr0.Add(3)];

		// parity_merge_two(swap, array, x, y, ptl, ptr, pts, cmp);
		QuadSort2Tools.parity_merge_two<
			T, PtrIndexer<T>, TIndexer, PtrReference<T>, TReference, TLessThan
		>(swp, swp0, arr, arr0, cmp);
	}

	public static void parity_swap_eight(TIndexer arr, TReference arr0, TLessThan cmp)
	{
		// VAR swap[8], *ptl, *ptr, *pts;
		var swap8 = new Array8<T>();
		var swp = swap8.Indexer();
		var swp0 = swp.Ref0;

		// if (cmp(array + 0, array + 1) > 0) { swap[0] = array[0]; array[0] = array[1]; array[1] = swap[0]; }
		// if (cmp(array + 2, array + 3) > 0) { swap[0] = array[2]; array[2] = array[3]; array[3] = swap[0]; }
		// if (cmp(array + 4, array + 5) > 0) { swap[0] = array[4]; array[4] = array[5]; array[5] = swap[0]; }
		// if (cmp(array + 6, array + 7) > 0) { swap[0] = array[6]; array[6] = array[7]; array[7] = swap[0]; } else
		swap_two_if(arr, arr0, swp, swp0, cmp);
		swap_two_if(arr, arr0.Add(2), swp, swp0, cmp);
		swap_two_if(arr, arr0.Add(4), swp, swp0, cmp);
		swap_two_if(arr, arr0.Add(6), swp, swp0, cmp);

		// if (cmp(array + 1, array + 2) <= 0 && cmp(array + 3, array + 4) <= 0 && cmp(array + 5, array + 6) <= 0)
		if (
			cmp.LtEq(arr[arr0.Add(1)], arr[arr0.Add(2)]) &&
			cmp.LtEq(arr[arr0.Add(3)], arr[arr0.Add(4)]) &&
			cmp.LtEq(arr[arr0.Add(5)], arr[arr0.Add(6)])
		) return;

		// parity_merge_two(array + 0, swap + 0, x, y, ptl, ptr, pts, cmp);
		QuadSort2Tools.parity_merge_two<
			T, TIndexer, PtrIndexer<T>, TReference, PtrReference<T>, TLessThan
		>(arr, arr0, swp, swp0, cmp);

		// parity_merge_two(array + 4, swap + 4, x, y, ptl, ptr, pts, cmp);
		QuadSort2Tools.parity_merge_two<
			T, TIndexer, PtrIndexer<T>, TReference, PtrReference<T>, TLessThan
		>(arr, arr0.Add(4), swp, swp0.Add(4), cmp);

		// parity_merge_four(swap, array, x, y, ptl, ptr, pts, cmp);
		QuadSort2Tools.parity_merge_four<
			T, PtrIndexer<T>, TIndexer, PtrReference<T>, TReference, TLessThan
		>(swp, swp0, arr, arr0, cmp);
	}

//		public static void parity_merge(
//			TIndexer arr, TReference arr0, PtrIndexer<T> swp, PtrReference<T> swp0, 
//			int block, int limit, TLessThan cmp)
//		{
//			// VAR *ptl, *ptr, *tpl, *tpr, *tpd, *ptd;
//			// unsigned char x, y;
//			PtrReference<T> ptl, ptr, tpl, tpr;
//			TReference tpd, ptd;
//			int x, y;
//			
//			// ptl = from;
//			// ptr = from + block;
//			// ptd = dest;
//			ptl = swp0;
//			ptr = swp0.Add(block);
//			ptd = arr0;
//
//			// tpl = from + block - 1;
//			// tpr = from + nmemb - 1;
//			// tpd = dest + nmemb - 1;
//			tpl = swp0.Add(block - 1);
//			tpr = swp0.Add(limit - 1);
//			tpd = arr0.Add(limit - 1);
//
//			for (block--; block > 0; block--)
//			{
//				// x = cmp(ptl, ptr) <= 0; y = !x; ptd[x] = *ptr; ptr += y; ptd[y] = *ptl; ptl += x; ptd++;
//				y = N(x = I(cmp.LtEq(swp[ptl], swp[ptr])));
//				arr[ptd.Add(x)] = swp[ptr];
//				ptr = ptr.Add(y);
//				arr[ptd.Add(y)] = swp[ptl];
//				ptl = ptl.Add(x);
//				ptd = ptd.Inc();
//
//				// x = cmp(tpl, tpr) <= 0; y = !x; tpd--; tpd[x] = *tpr; tpr -= x; tpd[y] = *tpl; tpl -= y;
//				y = N(x = I(cmp.LtEq(swp[tpl], swp[tpr])));
//				tpd = tpd.Dec();
//				arr[tpd.Add(x)] = swp[tpr];
//				tpr = tpr.Sub(x);
//				arr[tpd.Add(y)] = swp[tpl];
//				tpl = tpl.Sub(y);
//			}
//
//			// *ptd = cmp(ptl, ptr) <= 0 ? *ptl : *ptr;
//			// *tpd = cmp(tpl, tpr)  > 0 ? *tpl : *tpr;
//			arr[ptd] = swp[cmp.LtEq(swp[ptl], swp[ptr]) ? ptl : ptr];
//			arr[tpd] = swp[cmp.Gt(swp[tpl], swp[tpr]) ? tpl : tpr];
//		}

	public static void parity_swap_sixteen(TIndexer arr, TReference arr0, TLessThan cmp)
	{
		// VAR swap[16], *ptl, *ptr, *pts;
		var swap16 = new Array16<T>();
		var swp = swap16.Indexer();
		var swp0 = swp.Ref0;

		// FUNC(parity_swap_four)(array +  0, cmp);
		// FUNC(parity_swap_four)(array +  4, cmp);
		// FUNC(parity_swap_four)(array +  8, cmp);
		// FUNC(parity_swap_four)(array + 12, cmp);
		parity_swap_four(arr, arr0, cmp);
		parity_swap_four(arr, arr0.Add(4), cmp);
		parity_swap_four(arr, arr0.Add(8), cmp);
		parity_swap_four(arr, arr0.Add(12), cmp);

		// if (cmp(array + 3, array + 4) <= 0 && cmp(array + 7, array + 8) <= 0 && cmp(array + 11, array + 12) <= 0)
		if (
			cmp.LtEq(arr[arr0.Add(3)], arr[arr0.Add(4)]) &&
			cmp.LtEq(arr[arr0.Add(7)], arr[arr0.Add(8)]) &&
			cmp.LtEq(arr[arr0.Add(11)], arr[arr0.Add(12)])
		) return;

		// parity_merge_four(array + 0, swap + 0, x, y, ptl, ptr, pts, cmp);
		QuadSort2Tools.parity_merge_four<
			T, TIndexer, PtrIndexer<T>, TReference, PtrReference<T>, TLessThan
		>(arr, arr0, swp, swp0, cmp);

		// parity_merge_four(array + 8, swap + 8, x, y, ptl, ptr, pts, cmp);
		QuadSort2Tools.parity_merge_four<
			T, TIndexer, PtrIndexer<T>, TReference, PtrReference<T>, TLessThan
		>(arr, arr0.Add(8), swp, swp0.Add(8), cmp);

		// FUNC(parity_merge)(array, swap, 8, 16, cmp);
		QuadSort2Tools.parity_merge<
			T, TIndexer, PtrIndexer<T>, TReference, PtrReference<T>, TLessThan
		>(arr, arr0, swp, swp0, 8, 16, cmp);
	}

	public static void tail_swap(TIndexer array, TReference arr0, int nmemb, TLessThan cmp)
	{
		switch (nmemb)
		{
			case < 4:
				// FUNC(unguarded_insert)(array, 1, nmemb, cmp);
				unguarded_insert(array, arr0, 1, nmemb, cmp);
				return;
			case < 8:
				// FUNC(parity_swap_four)(array, cmp);
				// FUNC(unguarded_insert)(array, 4, nmemb, cmp);
				parity_swap_four(array, arr0, cmp);
				unguarded_insert(array, arr0, 4, nmemb, cmp);
				return;
			case < 16:
				// FUNC(parity_swap_eight)(array, cmp);
				// FUNC(unguarded_insert)(array, 8, nmemb, cmp);
				parity_swap_eight(array, arr0, cmp);
				unguarded_insert(array, arr0, 8, nmemb, cmp);
				return;
			default:
				// FUNC(parity_swap_sixteen)(array, cmp);
				// FUNC(unguarded_insert)(array, 16, nmemb, cmp);
				parity_swap_sixteen(array, arr0, cmp);
				unguarded_insert(array, arr0, 16, nmemb, cmp);
				break;
		}
	}

	public static void parity_tail_swap_eight(TIndexer arr, TReference arr0, TLessThan cmp)
	{
		// VAR swap[8], *ptl, *ptr, *pts;
		var swap8 = new Array8<T>();
		var swp = swap8.Indexer();
		var swp0 = swp.Ref0;

		// if (cmp(array + 4, array + 5) > 0) { swap[5] = array[4]; array[4] = array[5]; array[5] = swap[5]; }
		swap_two_if(arr, arr0.Add(4), swp, swp0.Add(5), cmp);

		// if (cmp(array + 6, array + 7) > 0) { swap[7] = array[6]; array[6] = array[7]; array[7] = swap[7]; } else
		swap_two_if(arr, arr0.Add(6), swp, swp0.Add(7), cmp);

		// else?
		// if (cmp(array + 3, array + 4) <= 0 && cmp(array + 5, array + 6) <= 0)
		if (
			cmp.LtEq(arr[arr0.Add(3)], arr[arr0.Add(4)]) &&
			cmp.LtEq(arr[arr0.Add(5)], arr[arr0.Add(6)])
		) return;

		// swap[0] = array[0]; swap[1] = array[1]; swap[2] = array[2]; swap[3] = array[3];
		swp[swp0] = arr[arr0];
		swp[swp0.Add(1)] = arr[arr0.Add(1)];
		swp[swp0.Add(2)] = arr[arr0.Add(2)];
		swp[swp0.Add(3)] = arr[arr0.Add(3)];

		// parity_merge_two(array + 4, swap + 4, x, y, ptl, ptr, pts, cmp);
		QuadSort2Tools.parity_merge_two<
			T, TIndexer, PtrIndexer<T>, TReference, PtrReference<T>, TLessThan
		>(arr, arr0.Add(4), swp, swp0.Add(4), cmp);

		// parity_merge_four(swap, array, x, y, ptl, ptr, pts, cmp);
		QuadSort2Tools.parity_merge_four<
			T, PtrIndexer<T>, TIndexer, PtrReference<T>, TReference, TLessThan
		>(swp, swp0, arr, arr0, cmp);
	}

	public static void parity_tail_flip_eight(TIndexer arr, TReference arr0, TLessThan cmp)
	{
		// VAR swap[8], *ptl, *ptr, *pts;
		var swap8 = new Array8<T>();
		var swp = swap8.Indexer();
		var swp0 = swp.Ref0;

		// if (cmp(array + 3, array + 4) <= 0)
		if (cmp.LtEq(arr[arr0.Add(3)], arr[arr0.Add(4)]))
			return;

		// swap[0] = array[0]; swap[1] = array[1]; swap[2] = array[2]; swap[3] = array[3];
		swp[swp0] = arr[arr0];
		swp[swp0.Add(1)] = arr[arr0.Add(1)];
		swp[swp0.Add(2)] = arr[arr0.Add(2)];
		swp[swp0.Add(3)] = arr[arr0.Add(3)];

		// swap[4] = array[4]; swap[5] = array[5]; swap[6] = array[6]; swap[7] = array[7];
		swp[swp0.Add(4)] = arr[arr0.Add(4)];
		swp[swp0.Add(5)] = arr[arr0.Add(5)];
		swp[swp0.Add(6)] = arr[arr0.Add(6)];
		swp[swp0.Add(7)] = arr[arr0.Add(7)];

		// parity_merge_four(swap, array, x, y, ptl, ptr, pts, cmp);
		QuadSort2Tools.parity_merge_four<
			T, PtrIndexer<T>, TIndexer, PtrReference<T>, TReference, TLessThan
		>(swp, swp0, arr, arr0, cmp);
	}

	public static int quad_swap(TIndexer arr, TReference arr0, int nmemb, TLessThan cmp)
	{
		// VAR swap[32];
		var swap32 = new Array32<T>();
		var swp = swap32.Indexer();
		var swp0 = swp.Ref0;

		// size_t count, reverse;
		int count, reverse;

		// VAR *pta, *pts, *pte, tmp;
		TReference pta, pts, pte;
		T tmp;

		pta = arr0;

		count = nmemb / 8 * 2;

		while (count-- > 0)
		{
			// if (cmp(&pta[0], &pta[1]) > 0)
			if (cmp.Gt(arr[pta], arr[pta.Inc()]))
			{
				// if (cmp(&pta[2], &pta[3]) > 0)
				if (cmp.Gt(arr[pta.Add(2)], arr[pta.Add(3)]))
				{
					// if (cmp(&pta[1], &pta[2]) > 0)
					if (cmp.Gt(arr[pta.Inc()], arr[pta.Add(2)]))
					{
						// pts = pta; pta += 4; goto swapper;
						pts = pta;
						pta = pta.Add(4);
						goto swapper;
					}

					// tmp = pta[2]; pta[2] = pta[3]; pta[3] = tmp;
					tmp = arr[pta.Add(2)];
					arr[pta.Add(2)] = arr[pta.Add(3)];
					arr[pta.Add(3)] = tmp;
				}

				// tmp = pta[0]; pta[0] = pta[1]; pta[1] = tmp;
				tmp = arr[pta];
				arr[pta] = arr[pta.Inc()];
				arr[pta.Inc()] = tmp;
			}
			// else if (cmp(&pta[2], &pta[3]) > 0)
			else if (cmp.Gt(arr[pta.Add(2)], arr[pta.Add(3)]))
			{
				// tmp = pta[2]; pta[2] = pta[3]; pta[3] = tmp;
				tmp = arr[pta.Add(2)];
				arr[pta.Add(2)] = arr[pta.Add(3)];
				arr[pta.Add(3)] = tmp;
			}

			// if (cmp(&pta[1], &pta[2]) > 0)
			if (cmp.Gt(arr[pta.Add(1)], arr[pta.Add(2)]))
			{
				// if (cmp(&pta[0], &pta[2]) <= 0)
				if (cmp.LtEq(arr[pta], arr[pta.Add(2)]))
				{
					// if (cmp(&pta[1], &pta[3]) <= 0)
					if (cmp.LtEq(arr[pta.Add(1)], arr[pta.Add(3)]))
					{
						// tmp = pta[1]; pta[1] = pta[2]; pta[2] = tmp;
						tmp = arr[pta.Add(1)];
						arr[pta.Add(1)] = arr[pta.Add(2)];
						arr[pta.Add(2)] = tmp;
					}
					else
					{
						// tmp = pta[1]; pta[1] = pta[2]; pta[2] = pta[3]; pta[3] = tmp;
						tmp = arr[pta.Add(1)];
						arr[pta.Add(1)] = arr[pta.Add(2)];
						arr[pta.Add(2)] = arr[pta.Add(3)];
						arr[pta.Add(3)] = tmp;
					}
				}
				// else if (cmp(&pta[0], &pta[3]) > 0)
				else if (cmp.Gt(arr[pta], arr[pta.Add(3)]))
				{
					// tmp = pta[1]; pta[1] = pta[3]; pta[3] = tmp;
					tmp = arr[pta.Add(1)];
					arr[pta.Add(1)] = arr[pta.Add(3)];
					arr[pta.Add(3)] = tmp;

					// tmp = pta[0]; pta[0] = pta[2]; pta[2] = tmp;
					tmp = arr[pta];
					arr[pta] = arr[pta.Add(2)];
					arr[pta.Add(2)] = tmp;
				}
				// else if (cmp(&pta[1], &pta[3]) <= 0)
				else if (cmp.LtEq(arr[pta.Add(1)], arr[pta.Add(3)]))
				{
					// tmp = pta[1]; pta[1] = pta[0]; pta[0] = pta[2]; pta[2] = tmp;
					tmp = arr[pta.Add(1)];
					arr[pta.Add(1)] = arr[pta];
					arr[pta] = arr[pta.Add(2)];
					arr[pta.Add(2)] = tmp;
				}
				else
				{
					// tmp = pta[1]; pta[1] = pta[0]; pta[0] = pta[2]; pta[2] = pta[3]; pta[3] = tmp;
					tmp = arr[pta.Add(1)];
					arr[pta.Add(1)] = arr[pta];
					arr[pta] = arr[pta.Add(2)];
					arr[pta.Add(2)] = arr[pta.Add(3)];
					arr[pta.Add(3)] = tmp;
				}
			}

			count--;

			parity_tail_swap_eight(arr, pta, cmp);

			pta = pta.Add(8);

			continue;

			swapper:

			if (count-- > 0)
			{
				// if (cmp(&pta[0], &pta[1]) > 0)
				if (cmp.Gt(arr[pta], arr[pta.Add(1)]))
				{
					// if (cmp(&pta[2], &pta[3]) > 0)
					if (cmp.Gt(arr[pta.Add(2)], arr[pta.Add(3)]))
					{
						// if (cmp(&pta[1], &pta[2]) > 0)
						if (cmp.Gt(arr[pta.Add(1)], arr[pta.Add(2)]))
						{
							// if (cmp(&pta[-1], &pta[0]) > 0)
							if (cmp.Gt(arr[pta.Add(-1)], arr[pta]))
							{
								pta = pta.Add(4);
								goto swapper;
							}
						}

						// tmp = pta[2]; pta[2] = pta[3]; pta[3] = tmp;
						tmp = arr[pta.Add(2)];
						arr[pta.Add(2)] = arr[pta.Add(3)];
						arr[pta.Add(3)] = tmp;
					}

					// tmp = pta[0]; pta[0] = pta[1]; pta[1] = tmp;
					tmp = arr[pta];
					arr[pta] = arr[pta.Add(1)];
					arr[pta.Add(1)] = tmp;
				}
				// else if (cmp(&pta[2], &pta[3]) > 0)
				else if (cmp.Gt(arr[pta.Add(2)], arr[pta.Add(3)]))
				{
					// tmp = pta[2]; pta[2] = pta[3]; pta[3] = tmp;
					tmp = arr[pta.Add(2)];
					arr[pta.Add(2)] = arr[pta.Add(3)];
					arr[pta.Add(3)] = tmp;
				}

				// if (cmp(&pta[1], &pta[2]) > 0)
				if (cmp.Gt(arr[pta.Add(1)], arr[pta.Add(2)]))
				{
					// if (cmp(&pta[0], &pta[2]) <= 0)
					if (cmp.LtEq(arr[pta], arr[pta.Add(2)]))
					{
						// if (cmp(&pta[1], &pta[3]) <= 0)
						if (cmp.LtEq(arr[pta.Add(1)], arr[pta.Add(3)]))
						{
							// tmp = pta[1]; pta[1] = pta[2]; pta[2] = tmp;
							tmp = arr[pta.Add(1)];
							arr[pta.Add(1)] = arr[pta.Add(2)];
							arr[pta.Add(2)] = tmp;
						}
						else
						{
							// tmp = pta[1]; pta[1] = pta[2]; pta[2] = pta[3]; pta[3] = tmp;
							tmp = arr[pta.Add(1)];
							arr[pta.Add(1)] = arr[pta.Add(2)];
							arr[pta.Add(2)] = arr[pta.Add(3)];
							arr[pta.Add(3)] = tmp;
						}
					}
					// else if (cmp(&pta[0], &pta[3]) > 0)
					else if (cmp.Gt(arr[pta], arr[pta.Add(3)]))
					{
						// tmp = pta[0]; pta[0] = pta[2]; pta[2] = tmp;
						tmp = arr[pta];
						arr[pta] = arr[pta.Add(2)];
						arr[pta.Add(2)] = tmp;

						// tmp = pta[1]; pta[1] = pta[3]; pta[3] = tmp;
						tmp = arr[pta.Add(1)];
						arr[pta.Add(1)] = arr[pta.Add(3)];
						arr[pta.Add(3)] = tmp;
					}
					// else if (cmp(&pta[1], &pta[3]) <= 0)
					else if (cmp.LtEq(arr[pta.Add(1)], arr[pta.Add(3)]))
					{
						// tmp = pta[0]; pta[0] = pta[2]; pta[2] = pta[1]; pta[1] = tmp;
						tmp = arr[pta];
						arr[pta] = arr[pta.Add(2)];
						arr[pta.Add(2)] = arr[pta.Add(1)];
						arr[pta.Add(1)] = tmp;
					}
					else
					{
						// tmp = pta[0]; pta[0] = pta[2]; pta[2] = pta[3]; pta[3] = pta[1]; pta[1] = tmp;
						tmp = arr[pta];
						arr[pta] = arr[pta.Add(2)];
						arr[pta.Add(2)] = arr[pta.Add(3)];
						arr[pta.Add(3)] = arr[pta.Add(1)];
						arr[pta.Add(1)] = tmp;
					}
				}

				pte = pta.Dec();

				// note: this is "swapper" and pts is set before "goto swapper"
				reverse = pte.Dif(pts) / 2;

				do
				{
					// tmp = *pts; *pts++ = *pte; *pte-- = tmp;
					tmp = arr[pts];
					arr[pts] = arr[pte];
					arr[pte] = tmp;
					pts = pts.Inc();
					pte = pte.Dec();
				}
				while (reverse-- > 0);

				if (count % 2 == 0)
				{
					pta = pta.Sub(4);

					parity_tail_flip_eight(arr, pta, cmp);
				}
				else
				{
					count--;

					parity_tail_swap_eight(arr, pta, cmp);
				}

				pta = pta.Add(8);

				continue;
			}

			if (pts.Eq(arr0))
			{
				// AAA! fall-though cases!
				switch (nmemb % 8)
				{
					// case 7: if (cmp(&pta[5], &pta[6]) <= 0) break;
					case 7:
						if (cmp.LtEq(arr[pta.Add(5)], arr[pta.Add(6)])) break;

						goto case 6;
					// case 6: if (cmp(&pta[4], &pta[5]) <= 0) break;
					case 6:
						if (cmp.LtEq(arr[pta.Add(4)], arr[pta.Add(5)])) break;

						goto case 5;
					// case 5: if (cmp(&pta[3], &pta[4]) <= 0) break;
					case 5:
						if (cmp.LtEq(arr[pta.Add(3)], arr[pta.Add(4)])) break;

						goto case 4;
					// case 4: if (cmp(&pta[2], &pta[3]) <= 0) break;
					case 4:
						if (cmp.LtEq(arr[pta.Add(2)], arr[pta.Add(3)])) break;

						goto case 3;
					// case 3: if (cmp(&pta[1], &pta[2]) <= 0) break;
					case 3:
						if (cmp.LtEq(arr[pta.Add(1)], arr[pta.Add(2)])) break;

						goto case 2;
					// case 2: if (cmp(&pta[0], &pta[1]) <= 0) break;
					case 2:
						if (cmp.LtEq(arr[pta], arr[pta.Add(1)])) break;

						goto case 1;
					// case 1: if (cmp(&pta[-1], &pta[0]) <= 0) break;
					case 1:
						if (cmp.LtEq(arr[pta.Add(-1)], arr[pta])) break;

						goto case 0;
					case 0:
						// pte = pts + nmemb - 1;
						pte = pts.Add(nmemb - 1);
						// reverse = (pte - pts) / 2;
						reverse = pte.Dif(pts) / 2;

						do
						{
							// tmp = *pts; *pts++ = *pte; *pte-- = tmp;

							tmp = arr[pts];
							arr[pts] = arr[pte];
							arr[pte] = tmp;
							pts = pts.Inc();
							pte = pte.Dec();
						}
						while (reverse-- > 0);

						return 1;
				}
			}

			// pte = pta - 1;
			pte = pta.Dec();
			// reverse = (pte - pts) / 2;
			reverse = pte.Dif(pts) / 2;

			do
			{
				// tmp = *pts; *pts++ = *pte; *pte-- = tmp;
				tmp = arr[pts];
				arr[pts] = arr[pte];
				arr[pte] = tmp;
				pts = pts.Inc();
				pte = pte.Dec();
			}
			while (reverse-- > 0);

			break;
		}

		tail_swap(arr, pta, nmemb % 8, cmp);

		pta = arr0;

		// count = nmemb / 32;

		// for (count = nmemb / 32 ; count-- ; pta += 32)
		for (count = nmemb / 32; count-- > 0; pta = pta.Add(32))
		{
			// if (cmp(pta + 7, pta + 8) <= 0 && cmp(pta + 15, pta + 16) <= 0 && cmp(pta + 23, pta + 24) <= 0)
			if (
				cmp.LtEq(arr[pta.Add(7)], arr[pta.Add(8)]) &&
				cmp.LtEq(arr[pta.Add(15)], arr[pta.Add(16)]) &&
				cmp.LtEq(arr[pta.Add(23)], arr[pta.Add(24)])
			) continue;

			// FUNC(parity_merge)(swap, pta, 8, 16, cmp);
			QuadSort2Tools.parity_merge<
				T, PtrIndexer<T>, TIndexer, PtrReference<T>, TReference, TLessThan
			>(swp, swp0, arr, pta, 8, 16, cmp);

			// FUNC(parity_merge)(swap + 16, pta + 16, 8, 16, cmp);
			QuadSort2Tools.parity_merge<
				T, PtrIndexer<T>, TIndexer, PtrReference<T>, TReference, TLessThan
			>(swp, swp0.Add(16), arr, pta.Add(16), 8, 16, cmp);

			// FUNC(parity_merge)(pta, swap, 16, 32, cmp);
			QuadSort2Tools.parity_merge<
				T, TIndexer, PtrIndexer<T>, TReference, PtrReference<T>, TLessThan
			>(arr, pta, swp, swp0, 16, 32, cmp);
		}

		if (nmemb % 32 > 8)
		{
			tail_merge(arr, pta, swp, 32, nmemb % 32, 8, cmp);
		}

		return 0;
	}

	public static void quad_merge_block(
		TIndexer arr, TReference arr0,
		PtrIndexer<T> swp,
		int block,
		TLessThan cmp)
	{
		var swp0 = swp.Ref0;

		// register VAR *pts, *c, *c_max;
		TReference c, c_max;
		PtrReference<T> pts;
		var block_x_2 = block * 2;

		c_max = arr0.Add(block);

		// if (cmp(c_max - 1, c_max) <= 0)
		if (cmp.LtEq(arr[c_max.Dec()], arr[c_max]))
		{
			// c_max += block_x_2;
			c_max = c_max.Add(block_x_2);

			// if (cmp(c_max - 1, c_max) <= 0)
			if (cmp.LtEq(arr[c_max.Dec()], arr[c_max]))
			{
				// c_max -= block;
				c_max = c_max.Sub(block);

				// if (cmp(c_max - 1, c_max) <= 0)
				if (cmp.LtEq(arr[c_max.Dec()], arr[c_max]))
					return;

				pts = swp0;
				c = arr0;

				// do *pts++ = *c++; while (c < c_max); // step 1
				do
				{
					swp[pts] = arr[c];
					pts = pts.Inc();
					c = c.Inc();
				}
				while (c.Lt(c_max)); // step 1

				// c_max = c + block_x_2;
				c_max = c.Add(block_x_2);

				// do *pts++ = *c++; while (c < c_max); // step 2
				do
				{
					swp[pts] = arr[c];
					pts = pts.Inc();
					c = c.Inc();
				}
				while (c.Lt(c_max)); // step 2

				// return FUNC(forward_merge)(array, swap, block_x_2, cmp); // step 3
				QuadSort2Tools.forward_merge<
					T, TIndexer, PtrIndexer<T>, TReference, PtrReference<T>, TLessThan
				>(arr, arr0, swp, swp0, block_x_2, cmp); // step 3

				return;
			}

			// pts = swap;
			pts = swp0;

			// c = array;
			c = arr0;

			// c_max = array + block_x_2;
			c_max = arr0.Add(block_x_2);

			// do *pts++ = *c++; while (c < c_max); // step 1
			do
			{
				swp[pts] = arr[c];
				pts = pts.Inc();
				c = c.Inc();
			}
			while (c.Lt(c_max)); // step 1
		}
		else
		{
			// FUNC(forward_merge)(swap, array, block, cmp); // step 1
			QuadSort2Tools.forward_merge<
				T, PtrIndexer<T>, TIndexer, PtrReference<T>, TReference, TLessThan
			>(swp, swp0, arr, arr0, block, cmp); // step 1
		}

		// FUNC(forward_merge)(swap + block_x_2, array + block_x_2, block, cmp); // step 2
		QuadSort2Tools.forward_merge<
			T, PtrIndexer<T>, TIndexer, PtrReference<T>, TReference, TLessThan
		>(swp, swp0.Add(block_x_2), arr, arr0.Add(block_x_2), block, cmp);

		// FUNC(forward_merge)(array, swap, block_x_2, cmp); // step 3
		QuadSort2Tools.forward_merge<
			T, TIndexer, PtrIndexer<T>, TReference, PtrReference<T>, TLessThan
		>(arr, arr0, swp, swp0, block_x_2, cmp);
	}

	public static void quad_merge(
		TIndexer arr, TReference arr0, PtrIndexer<T> swp,
		int swap_size, int nmemb, int block,
		TLessThan cmp)
	{
		// register VAR *pta, *pte;
		TReference pta, pte;

		// pte = array + nmemb;
		pte = arr0.Add(nmemb);

		block *= 4;

		while (block <= nmemb && block <= swap_size)
		{
			pta = arr0;

			do
			{
				// FUNC(quad_merge_block)(pta, swap, block / 4, cmp);
				quad_merge_block(arr, pta, swp, block / 4, cmp);

				// pta += block;
				pta = pta.Add(block);
			}
			// while (pta + block <= pte);
			while (pta.Add(block).LtEq(pte));

			// FUNC(tail_merge)(pta, swap, swap_size, pte - pta, block / 4, cmp);
			tail_merge(arr, pta, swp, swap_size, pte.Dif(pta), block / 4, cmp);

			block *= 4;
		}

		// FUNC(tail_merge)(array, swap, swap_size, nmemb, block / 4, cmp);
		tail_merge(arr, arr0, swp, swap_size, nmemb, block / 4, cmp);
	}

	public static void partial_forward_merge(
		TIndexer arr, TReference arr0,
		PtrIndexer<T> swp,
		int nmemb, int block,
		TLessThan cmp)
	{
		var swp0 = swp.Ref0;
		TReference r, e; // right, end
		PtrReference<T> m, s; // middle, swap

		r = arr0.Add(block);
		e = arr0.Add(nmemb - 1);

		// memcpy(swap, array, block * sizeof(VAR));
		arr.Export(arr0, swp0, block);

		s = swp0;
		m = swp0.Add(block - 1);

		// if (cmp(m, e) <= 0)
		if (cmp.LtEq(swp[m], arr[e]))
		{
			do
			{
				// while (cmp(s, r) > 0)
				while (cmp.Gt(swp[s], arr[r]))
				{
					// *array++ = *r++;
					arr[arr0] = arr[r];
					arr0 = arr0.Inc();
					r = r.Inc();
				}

				// *array++ = *s++;
				arr[arr0] = swp[s];
				arr0 = arr0.Inc();
				s = s.Inc();
			}
			// while (s <= m);
			while (s.LtEq(m));
		}
		else
		{
			do
			{
				// if (cmp(s, r) > 0)
				if (cmp.Gt(swp[s], arr[r]))
				{
					// *array++ = *r++;
					arr[arr0] = arr[r];
					arr0 = arr0.Inc();
					r = r.Inc();

					continue;
				}

				// *array++ = *s++;
				arr[arr0] = swp[s];
				arr0 = arr0.Inc();
				s = s.Inc();

				// if (cmp(s, r) > 0)
				if (cmp.Gt(swp[s], arr[r]))
				{
					// *array++ = *r++;
					arr[arr0] = arr[r];
					arr0 = arr0.Inc();
					r = r.Inc();

					continue;
				}

				// *array++ = *s++;
				arr[arr0] = swp[s];
				arr0 = arr0.Inc();
				s = s.Inc();

				// if (cmp(s, r) > 0)
				if (cmp.Gt(swp[s], arr[r]))
				{
					// *array++ = *r++;
					arr[arr0] = arr[r];
					arr0 = arr0.Inc();
					r = r.Inc();

					continue;
				}

				// *array++ = *s++;
				arr[arr0] = swp[s];
				arr0 = arr0.Inc();
				s = s.Inc();
			}
			// while (r <= e);
			while (r.LtEq(e));

			// do *array++ = *s++; while (s <= m);
			do
			{
				// *array++ = *s++;
				arr[arr0] = swp[s];
				arr0 = arr0.Inc();
				s = s.Inc();
			}
			while (s.LtEq(m));
		}
	}

	public static void partial_backward_merge(
		TIndexer arr, TReference arr0,
		PtrIndexer<T> swp,
		int nmemb, int block,
		TLessThan cmp)
	{
		var swp0 = swp.Ref0;

		TReference r, m, e; // right, middle, end
		PtrReference<T> s; // swap

		// m = array + block;
		m = arr0.Add(block);
		// e = array + nmemb - 1;
		e = arr0.Add(nmemb - 1);
		// r = m--;
		r = m;
		m = m.Dec();

		// if (cmp(m, r) <= 0)
		if (cmp.LtEq(arr[m], arr[r]))
			return;

		// while (cmp(m, e) <= 0)
		while (cmp.LtEq(arr[m], arr[e]))
		{
			e = e.Dec();
		}

		// s = swap;
		s = swp0;

		// do *s++ = *r++; while (r <= e);
		do
		{
			// *s++ = *r++;
			swp[s] = arr[r];
			s = s.Inc();
			r = r.Inc();
		}
		// while (r <= e);
		while (r.LtEq(e));

		// s--;
		s = s.Dec();

		// *e-- = *m--;
		arr[e] = arr[m];
		e = e.Dec();
		m = m.Dec();

		// if (cmp(array, swap) <= 0)
		if (cmp.LtEq(arr[arr0], swp[swp0]))
		{
			do
			{
				// while (cmp(m, s) > 0)
				while (cmp.Gt(arr[m], swp[s]))
				{
					// *e-- = *m--;
					arr[e] = arr[m];
					e = e.Dec();
					m = m.Dec();
				}

				// *e-- = *s--;
				arr[e] = swp[s];
				e = e.Dec();
				s = s.Dec();
			}
			// while (s >= swap);
			while (s.GtEq(swp0));
		}
		else
		{
			do
			{
				// if (cmp(m, s) > 0)
				if (cmp.Gt(arr[m], swp[s]))
				{
					// *e-- = *m--;
					arr[e] = arr[m];
					e = e.Dec();
					m = m.Dec();

					continue;
				}

				// *e-- = *s--;
				arr[e] = swp[s];
				e = e.Dec();
				s = s.Dec();

				// if (cmp(m, s) > 0)
				if (cmp.Gt(arr[m], swp[s]))
				{
					// *e-- = *m--;
					arr[e] = arr[m];
					e = e.Dec();
					m = m.Dec();

					continue;
				}

				// *e-- = *s--;
				arr[e] = swp[s];
				e = e.Dec();
				s = s.Dec();

				// if (cmp(m, s) > 0)
				if (cmp.Gt(arr[m], swp[s]))
				{
					// *e-- = *m--;
					arr[e] = arr[m];
					e = e.Dec();
					m = m.Dec();

					continue;
				}

				// *e-- = *s--;
				arr[e] = swp[s];
				e = e.Dec();
				s = s.Dec();
			}
			// while (m >= array);
			while (m.GtEq(arr0));

			// do *e-- = *s--; while (s >= swap);
			do
			{
				// *e-- = *s--;
				arr[e] = swp[s];
				e = e.Dec();
				s = s.Dec();
			}
			// while (s >= swap);
			while (s.GtEq(swp0));
		}
	}

	public static void tail_merge(
		TIndexer arr, TReference arr0,
		PtrIndexer<T> swp,
		int swap_size, int nmemb, int block,
		TLessThan cmp)
	{
		// register VAR *pta, *pte;
		TReference pta, pte;

		// pte = array + nmemb;
		pte = arr0.Add(nmemb);

		while (block < nmemb && block <= swap_size)
		{
			// for (pta = array ; pta + block < pte ; pta += block * 2)
			for (pta = arr0; pta.Add(block).Lt(pte); pta = pta.Add(block * 2))
			{
				// if (pta + block * 2 < pte)
				if (pta.Add(block * 2).Lt(pte))
				{
					partial_backward_merge(arr, pta, swp, block * 2, block, cmp);
					continue;
				}

				partial_backward_merge(arr, pta, swp, pte.Dif(pta), block, cmp);

				break;
			}

			block *= 2;
		}
	}

	public static void trinity_rotation(
		TIndexer arr, TReference arr0,
		PtrIndexer<T> swp,
		int swap_size, int nmemb, int left)
	{
		var swp0 = swp.Ref0;

		// size_t bridge, right = nmemb - left;
		int bridge, right = nmemb - left;

		if (left < right)
		{
			if (left <= swap_size)
			{
				// memcpy(swap, array, left * sizeof(VAR));
				arr.Export(arr0, swp0, left);

				// memmove(array, array + left, right * sizeof(VAR));
				arr.Copy(arr0.Add(left), arr0, right);

				// memcpy(array + right, swap, left * sizeof(VAR));
				arr.Import(arr0.Add(right), swp0, left);
			}
			else
			{
				// VAR *pta, *ptb, *ptc, *ptd;
				TReference pta, ptb, ptc, ptd;

				// pta = array;
				// ptb = pta + left;
				pta = arr0;
				ptb = pta.Add(left);

				bridge = right - left;

				if (bridge <= swap_size && bridge > 3)
				{
					// ptc = pta + right;
					// ptd = ptc + left;
					ptc = pta.Add(right);
					ptd = ptc.Add(left);

					// memcpy(swap, ptb, bridge * sizeof(VAR));
					arr.Export(ptb, swp0, bridge);

					while (left-- > 0)
					{
						// *--ptc = *--ptd;
						ptc = ptc.Dec();
						ptd = ptd.Dec();
						arr[ptc] = arr[ptd];

						// *ptd = *--ptb;
						ptb = ptb.Dec();
						arr[ptd] = arr[ptb];
					}

					// memcpy(pta, swap, bridge * sizeof(VAR));
					arr.Import(pta, swp0, bridge);
				}
				else
				{
					// ptc = ptb; ptd = ptc + right;
					ptc = ptb;
					ptd = ptc.Add(right);

					bridge = left / 2;

					while (bridge-- > 0)
					{
						// *swap = *--ptb; *ptb = *pta; *pta++ = *ptc; *ptc++ = *--ptd; *ptd = *swap;
						ptb = ptb.Dec();
						swp[swp0] = arr[ptb];
						arr[ptb] = arr[pta];
						arr[pta] = arr[ptc];
						pta = pta.Inc();
						ptd = ptd.Dec();
						arr[ptc] = arr[ptd];
						ptc = ptc.Inc();
						arr[ptd] = swp[swp0];
					}

					// bridge = (ptd - ptc) / 2;
					bridge = ptd.Dif(ptc) / 2;

					while (bridge-- > 0)
					{
						// *swap = *ptc; *ptc++ = *--ptd; *ptd = *pta; *pta++ = *swap;
						swp[swp0] = arr[ptc];
						ptd = ptd.Dec();
						arr[ptc] = arr[ptd];
						ptc = ptc.Inc();
						arr[ptd] = arr[pta];
						arr[pta] = swp[swp0];
						pta = pta.Inc();
					}

					// bridge = (ptd - pta) / 2;
					bridge = ptd.Dif(pta) / 2;

					while (bridge-- > 0)
					{
						// *swap = *pta; *pta++ = *--ptd; *ptd = *swap;
						swp[swp0] = arr[pta];
						ptd = ptd.Dec();
						arr[pta] = arr[ptd];
						pta = pta.Inc();
						arr[ptd] = swp[swp0];
					}
				}
			}
		}
		else if (right < left)
		{
			if (right <= swap_size)
			{
				// memcpy(swap, array + left, right * sizeof(VAR));
				arr.Export(arr0.Add(left), swp0, right);

				// memmove(array + right, array, left * sizeof(VAR));
				arr.Copy(arr0, arr0.Add(right), left);

				// memcpy(array, swap, right * sizeof(VAR));
				arr.Import(arr0, swp0, right);
			}
			else
			{
				// VAR *pta, *ptb, *ptc, *ptd;
				TReference pta, ptb, ptc, ptd;

				// pta = array; ptb = pta + left;
				pta = arr0;
				ptb = pta.Add(left);

				bridge = left - right;

				if (bridge <= swap_size && bridge > 3)
				{
					// ptc = pta + right; ptd = ptc + left;
					ptc = pta.Add(right);
					ptd = ptc.Add(left);

					// memcpy(swap, ptc, bridge * sizeof(VAR));
					arr.Export(ptc, swp0, bridge);

					while (right-- > 0)
					{
						// *ptc++ = *pta; *pta++ = *ptb++;
						arr[ptc] = arr[pta];
						ptc = ptc.Inc();
						arr[pta] = arr[ptb];
						pta = pta.Inc();
						ptb = ptb.Inc();
					}

					// memcpy(ptd - bridge, swap, bridge * sizeof(VAR));
					arr.Import(ptd.Sub(bridge), swp0, bridge);
				}
				else
				{
					// ptc = ptb; ptd = ptc + right;
					ptc = ptb;
					ptd = ptc.Add(right);

					bridge = right / 2;

					while (bridge-- > 0)
					{
						// *swap = *--ptb; *ptb = *pta; *pta++ = *ptc; *ptc++ = *--ptd; *ptd = *swap;
						ptb = ptb.Dec();
						swp[swp0] = arr[ptb];
						arr[ptb] = arr[pta];
						arr[pta] = arr[ptc];
						pta = pta.Inc();
						ptd = ptd.Dec();
						arr[ptc] = arr[ptd];
						ptc = ptc.Inc();
						arr[ptd] = swp[swp0];
					}

					bridge = ptb.Dif(pta) / 2;

					while (bridge-- > 0)
					{
						// *swap = *--ptb; *ptb = *pta; *pta++ = *--ptd; *ptd = *swap;
						ptb = ptb.Dec();
						swp[swp0] = arr[ptb];
						arr[ptb] = arr[pta];
						ptd = ptd.Dec();
						arr[pta] = arr[ptd];
						pta = pta.Inc();
						arr[ptd] = swp[swp0];
					}

					bridge = ptd.Dif(pta) / 2;

					while (bridge-- > 0)
					{
						// *swap = *pta; *pta++ = *--ptd; *ptd = *swap;
						swp[swp0] = arr[pta];
						ptd = ptd.Dec();
						arr[pta] = arr[ptd];
						pta = pta.Inc();
						arr[ptd] = swp[swp0];
					}
				}
			}
		}
		else
		{
			// VAR *pta, *ptb;
			TReference pta, ptb;

			// pta = array; ptb = pta + left;
			pta = arr0;
			ptb = pta.Add(left);

			while (left-- > 0)
			{
				// *swap = *pta; *pta++ = *ptb; *ptb++ = *swap;
				swp[swp0] = arr[pta];
				arr[pta] = arr[ptb];
				pta = pta.Inc();
				arr[ptb] = swp[swp0];
				ptb = ptb.Inc();
			}
		}
	}

	public static int monobound_binary_first(
		TIndexer arr, TReference arr0, TReference value, int top, TLessThan cmp)
	{
		// VAR *end; size_t mid;
		TReference end;
		int mid;

		// end = array + top;
		end = arr0.Add(top);

		while (top > 1)
		{
			mid = top / 2;

			// if (cmp(value, end - mid) <= 0)
			if (cmp.LtEq(arr[value], arr[end.Sub(mid)]))
			{
				// end -= mid;
				end = end.Sub(mid);
			}

			top -= mid;
		}

		// if (cmp(value, end - 1) <= 0)
		if (cmp.LtEq(arr[value], arr[end.Dec()]))
		{
			end = end.Dec();
		}

		return end.Dif(arr0);
	}

	public static void blit_merge_block(
		TIndexer arr, TReference arr0,
		PtrIndexer<T> swp,
		int swap_size, int block, int right,
		TLessThan cmp)
	{
		int left;

		// if (cmp(array + block - 1, array + block) <= 0)
		if (cmp.LtEq(arr[arr0.Add(block - 1)], arr[arr0.Add(block)]))
			return;

		// left = FUNC(monobound_binary_first)(array + block, array + block / 2, right, cmp);
		left = monobound_binary_first(arr, arr0.Add(block), arr0.Add(block / 2), right, cmp);

		right -= left;

		block /= 2;

		if (left != 0)
		{
			// FUNC(trinity_rotation)(array + block, swap, swap_size, block + left, block);
			trinity_rotation(arr, arr0.Add(block), swp, swap_size, block + left, block);

			if (left <= swap_size)
			{
				// FUNC(partial_backward_merge)(array, swap, block + left, block, cmp);
				partial_backward_merge(arr, arr0, swp, block + left, block, cmp);
			}
			else if (block <= swap_size)
			{
				// FUNC(partial_forward_merge)(array, swap, block + left, block, cmp);
				partial_forward_merge(arr, arr0, swp, block + left, block, cmp);
			}
			else
			{
				// FUNC(blit_merge_block)(array, swap, swap_size, block, left, cmp);
				blit_merge_block(arr, arr0, swp, swap_size, block, left, cmp);
			}
		}

		if (right != 0)
		{
			var arrN = arr0.Add(block + left);

			if (right <= swap_size)
			{
				// FUNC(partial_backward_merge)(array + block + left, swap, block + right, block, cmp);
				partial_backward_merge(arr, arrN, swp, block + right, block, cmp);
			}
			else if (block <= swap_size)
			{
				// FUNC(partial_forward_merge)(array + block + left, swap, block + right, block, cmp);
				partial_forward_merge(arr, arrN, swp, block + right, block, cmp);
			}
			else
			{
				// FUNC(blit_merge_block)(array + block + left, swap, swap_size, block, right, cmp);
				blit_merge_block(arr, arrN, swp, swap_size, block, right, cmp);
			}
		}
	}

	public static void blit_merge(
		TIndexer arr, TReference arr0,
		PtrIndexer<T> swp,
		int swap_size, int nmemb, int block,
		TLessThan cmp)
	{
		// VAR *pta, *pte;
		TReference pta, pte;

		// pte = array + nmemb;
		pte = arr0.Add(nmemb);

		while (block < nmemb)
		{
			// for (pta = array ; pta + block < pte ; pta += block * 2)
			for (pta = arr0; pta.Add(block).Lt(pte); pta = pta.Add(block * 2))
			{
				// if (pta + block * 2 < pte)
				if (pta.Add(block * 2).Lt(pte))
				{
					// FUNC(blit_merge_block)(pta, swap, swap_size, block, block, cmp);
					blit_merge_block(arr, pta, swp, swap_size, block, block, cmp);

					continue;
				}

				// FUNC(blit_merge_block)(pta, swap, swap_size, block, pte - pta - block, cmp);
				blit_merge_block(arr, pta, swp, swap_size, block, pte.Dif(pta) - block, cmp);

				break;
			}

			block *= 2;
		}
	}

	public static unsafe void quad_sort(TIndexer arr, int nmemb, TLessThan cmp)
	{
		var arr0 = arr.Ref0;

		if (nmemb < 32)
		{
			// FUNC(tail_swap)(array, nmemb, cmp);
			tail_swap(arr, arr0, nmemb, cmp);
		}
		// else if (FUNC(quad_swap)(array, nmemb, cmp) == 0)
		else if (quad_swap(arr, arr0, nmemb, cmp) == 0)
		{
			var swap_size = 32;

			while (swap_size * 4 <= nmemb)
				swap_size *= 4;

			var swapN = new T[swap_size];
			fixed (byte* swapP = &Unsafe.As<T, byte>(ref swapN[0]))
			{
				var swp = new PtrIndexer<T>(swapP);

				// FUNC(quad_merge)(array, swap, swap_size, nmemb, 32, cmp);
				quad_merge(arr, arr0, swp, swap_size, nmemb, 32, cmp);

				// FUNC(blit_merge)(array, swap, swap_size, nmemb, swap_size * 2, cmp);
				blit_merge(arr, arr0, swp, swap_size, nmemb, swap_size * 2, cmp);
			}
		}
	}
}