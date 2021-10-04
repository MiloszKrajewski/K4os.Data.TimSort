using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace K4os.Data.TimSort.Internals
{
	/// <summary>
	/// Helper class to extract array of item from <see cref="List{T}"/>.
	/// Similar to <c>CollectionsMarshal.AsSpan(...)</c>.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	public class ArrayExtractor<T>
	{
		private static readonly Func<List<T>, T[]> Extractor = BuildExtractor();

		private static Func<List<T>, T[]> BuildExtractor()
		{
			var argument = Expression.Parameter(typeof(List<T>));
			var member = Expression.Field(argument, "_items");
			var lambda = Expression.Lambda<Func<List<T>, T[]>>(member, argument);
			return lambda.Compile();
		}

		/// <summary>Gets array which is encapsulated by given <paramref name="list"/>.
		/// Please note, this method assumes name of private property inside <see cref="List{T}"/>.
		/// This works, because this property is marked as "do not rename". See:
		/// https://source.dot.net/#System.Private.CoreLib/List.cs
		/// </summary>
		/// <param name="list"><see cref="List{T}"/> to extract array from.</param>
		/// <returns>Internal array.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] GetArray(List<T> list) => Extractor(list);
	}
}
