using System.Runtime.CompilerServices;

namespace ReferenceImplementation.Quad;

public unsafe class QuadSort1
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int I(bool v) => Unsafe.As<bool, byte>(ref v);
		
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int N(int v) => v ^ 1;
		
	public static void parity_merge_two(
		double* array,
		double* swap,
		out int x, out int y,
		out double* ptl,
		out double* ptr,
		out double* pts)
	{
		ptl = array + 0; ptr = array + 2; pts = swap + 0;
		x = I(*ptl <= *ptr); y = N(x); pts[x] = *ptr; ptr += y; pts[y] = *ptl; ptl += x; pts++;
		*pts = *ptl <= *ptr ? *ptl : *ptr;
			
		ptl = array + 1; ptr = array + 3; pts = swap + 3;
		x = I(*ptl <= *ptr); y = N(x); pts--; pts[x] = *ptr; ptr -= x; pts[y] = *ptl; ptl -= y;
		*pts = *ptl > *ptr ? *ptl : *ptr;
	}
		
	public static void parity_merge_four(
		double* array, 
		double* swap, 
		out int x, 
		out int y, 
		out double* ptl, 
		out double* ptr, 
		out double* pts)
	{
		ptl = array + 0; ptr = array + 4; pts = swap;
		x = I(*ptl <= *ptr); y = N(x); pts[x] = *ptr; ptr += y; pts[y] = *ptl; ptl += x; pts++;
		x = I(*ptl <= *ptr); y = N(x); pts[x] = *ptr; ptr += y; pts[y] = *ptl; ptl += x; pts++;
		x = I(*ptl <= *ptr); y = N(x); pts[x] = *ptr; ptr += y; pts[y] = *ptl; ptl += x; pts++;
		*pts = *ptl <= *ptr ? *ptl : *ptr;

		ptl = array + 3; ptr = array + 7; pts = swap + 7;
		x = I(*ptl <= *ptr); y = N(x); pts--; pts[x] = *ptr; ptr -= x; pts[y] = *ptl; ptl -= y;
		x = I(*ptl <= *ptr); y = N(x); pts--; pts[x] = *ptr; ptr -= x; pts[y] = *ptl; ptl -= y;
		x = I(*ptl <= *ptr); y = N(x); pts--; pts[x] = *ptr; ptr -= x; pts[y] = *ptl; ptl -= y;
		*pts = *ptl > *ptr ? *ptl : *ptr;
	}
		
	public static void unguarded_insert(double* array, int offset, int nmemb)
	{
		for (var i = offset; i < nmemb; i++)
		{
			double* end;
			var pta = end = array + i;

			if (*--pta <= *end)
				continue;

			var key = *end;

			if (*array > key)
			{
				var top = i;

				do
				{
					*end-- = *pta--;
				}
				while (--top > 0);

				*end = key;
			}
			else
			{
				do
				{
					*end-- = *pta--;
				}
				while (*pta > key);

				*end = key;
			}
		}
	}

	public static void parity_swap_four(double* array)
	{
		var swap4 = new Array4<double>();
		var swap = (double*)Unsafe.AsPointer(ref swap4);

		double* ptl, ptr, pts;
		int x, y;

		x = I(*(array + 0) <= *(array + 1)); y = N(x); swap[0 + y] = array[0]; swap[0 + x] = array[1];
		x = I(*(array + 2) <= *(array + 3)); y = N(x); swap[2 + y] = array[2]; swap[2 + x] = array[3];

		parity_merge_two(swap, array, out x, out y, out ptl, out ptr, out pts);
	}

	public static void parity_swap_eight(double* array)
	{
		var swap8 = new Array8<double>();
		var swap = (double*)Unsafe.AsPointer(ref swap8);

		double* ptl, ptr, pts;
		int x, y;

		if (*(array + 0) > *(array + 1)) { swap[0] = array[0]; array[0] = array[1]; array[1] = swap[0]; }
		if (*(array + 2) > *(array + 3)) { swap[0] = array[2]; array[2] = array[3]; array[3] = swap[0]; }
		if (*(array + 4) > *(array + 5)) { swap[0] = array[4]; array[4] = array[5]; array[5] = swap[0]; }
		if (*(array + 6) > *(array + 7)) { swap[0] = array[6]; array[6] = array[7]; array[7] = swap[0]; } 
		else
		if (*(array + 1) <= *(array + 2) && *(array + 3) <= *(array + 4) && *(array + 5) <= *(array + 6))
		{
			return;
		}
			
		parity_merge_two(array + 0, swap + 0, out x, out y, out ptl, out ptr, out pts);
		parity_merge_two(array + 4, swap + 4, out x, out y, out ptl, out ptr, out pts);

		parity_merge_four(swap, array, out x, out y, out ptl, out ptr, out pts);
	}

	public static void parity_merge(double* dest, double* from, int block, int nmemb)
	{
		double *ptl, ptr, tpl, tpr, tpd, ptd;
		int x, y;

		ptl = from;
		ptr = from + block;
		ptd = dest;
		tpl = from + block - 1;
		tpr = from + nmemb - 1;
		tpd = dest + nmemb - 1;

		for (block--; block > 0; block--)
		{
			x = I(*ptl <= *ptr); y = N(x); ptd[x] = *ptr; ptr += y; ptd[y] = *ptl; ptl += x; ptd++;
			x = I(*tpl <= *tpr); y = N(x); tpd--; tpd[x] = *tpr; tpr -= x; tpd[y] = *tpl; tpl -= y;
		}
			
		*ptd = *ptl <= *ptr ? *ptl : *ptr;
		*tpd = *tpl > *tpr ? *tpl : *tpr;
	}

	public static void parity_swap_sixteen(double *array)
	{
		var swap16 = new Array16<double>();
		var swap = (double*)Unsafe.AsPointer(ref swap16);

		double* ptl, ptr, pts;
		int x, y;

		parity_swap_four(array +  0);
		parity_swap_four(array +  4);
		parity_swap_four(array +  8);
		parity_swap_four(array + 12);

		if (*(array + 3) <= *(array + 4) && *(array + 7) <= *(array + 8) && *(array + 11) <= *(array + 12))
		{
			return;
		}
			
		parity_merge_four(array + 0, swap + 0, out x, out y, out ptl, out ptr, out pts);
		parity_merge_four(array + 8, swap + 8, out x, out y, out ptl, out ptr, out pts);

		parity_merge(array, swap, 8, 16);
	}

	public static void tail_swap(double* array, int nmemb)
	{
		if (nmemb < 4)
		{
			unguarded_insert(array, 1, nmemb);
			return;
		}
		if (nmemb < 8)
		{
			parity_swap_four(array);
			unguarded_insert(array, 4, nmemb);
			return;
		}
		if (nmemb < 16)
		{
			parity_swap_eight(array);
			unguarded_insert(array, 8, nmemb);
			return;
		}
		parity_swap_sixteen(array);
		unguarded_insert(array, 16, nmemb);
	} 
}