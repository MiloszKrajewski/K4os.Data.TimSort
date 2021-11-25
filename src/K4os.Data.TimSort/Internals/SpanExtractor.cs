using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if NET5_0 || NET5_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace K4os.Data.TimSort.Internals
{
	/// <summary>Utilities to get <see cref="Span{T}"/> out of <see cref="List{T}"/>
	/// or <see cref="IList{T}"/></summary>
	public class SpanExtractor
	{
		#if NET5_0 || NET5_0_OR_GREATER
		
		/// <summary>Get underlying span out of <see cref="List{T}"/></summary>
		/// <param name="list">List.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns>Underlying <see cref="Span{T}"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Span<T> GetSpan<T>(List<T> list) => 
			CollectionsMarshal.AsSpan(list);

		#else

		/// <summary>Get underlying span out of <see cref="List{T}"/></summary>
		/// <param name="list">List.</param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns>Underlying <see cref="Span{T}"/></returns>
		public static Span<T> GetSpan<T>(List<T> list)
		{
			var array = ArrayExtractor<T>.GetArray(list);
			var length = list.Count;
			return array.AsSpan(0, length);
		}

		#endif

		/// <summary>Trys to get underlying span out of <see cref="ICollection{T}"/></summary>
		/// <param name="collection">Collection (will work for <see cref="IList{T}"/>)</param>
		/// <param name="span">Resulting <see cref="Span{T}"/></param>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns><c>true</c> if span was successfully extracted; <c>false</c> otherwise</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetSpan<T>(ICollection<T> collection, out Span<T> span)
		{
			switch (collection)
			{
				case T[] array:
					span = array.AsSpan();
					return true;
				case List<T> list:
					span = GetSpan(list);
					return true;
				default:
					span = Span<T>.Empty;
					return false;
			}
		}
	}
}
