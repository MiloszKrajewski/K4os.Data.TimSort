using System;
using System.Linq.Expressions;
using System.Reflection;
using K4os.Data.TimSort.Indexers;
using K4os.Data.TimSort.Internals;

namespace K4os.Data.TimSort.Sorters
{
	internal class ComparableSortHelper<T, TSorter, TIndexer, TReference>
		where TSorter: ISortAlgorithm
		where TIndexer: IIndexer<T, TReference>
		where TReference: struct, IReference<TReference>
	{
		public delegate void CallbackType(
			TSorter sorter, TIndexer indexer, TReference lo, TReference hi);

		private static readonly CallbackType? CallbackFunc = BuildCallback();

		private static bool IsValidType() =>
			typeof(T).IsAssignableTo(typeof(IComparable<T>));

		private static void ThrowNotComparable() =>
			throw new InvalidOperationException(
				$"Type {typeof(T).FullName} cannot be handled by ComparableSortHelper");

		private static CallbackType? BuildCallback()
		{
			if (!IsValidType()) 
				return null;

			var sorterArg = Expression.Parameter(typeof(TSorter));
			var indexerArg = Expression.Parameter(typeof(TIndexer));
			var loArg = Expression.Parameter(typeof(TReference));
			var hiArg = Expression.Parameter(typeof(TReference));

			var methodInfo = typeof(SortAlgorithmExtensions).GetMethod(
				nameof(SortAlgorithmExtensions.ComparableSort),
				BindingFlags.Static | BindingFlags.NonPublic);
			if (methodInfo is null)
				return null;
			
			var methodImpl = methodInfo.MakeGenericMethod(
				typeof(T), typeof(TSorter), typeof(TIndexer), typeof(TReference));

			var body = Expression.Call(methodImpl, sorterArg, indexerArg, loArg, hiArg);

			var lambda = Expression.Lambda<CallbackType>(
				body, sorterArg, indexerArg, loArg, hiArg);

			return lambda.Compile();
		}

		public static void Sort(TSorter sorter, TIndexer indexer, TReference lo, TReference hi)
		{
			if (CallbackFunc is null)
				ThrowNotComparable();
			else
				CallbackFunc(sorter, indexer, lo, hi);
		}
	}
}
