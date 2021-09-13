using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using K4os.Data.TimSort.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReferenceImplementation;

// ReSharper disable UnusedParameter.Local

namespace K4os.Data.TimSort.App
{
	internal static class Program
	{
		public static void Main(string[] args)
		{
			var loggerFactory = new LoggerFactory();
			loggerFactory.AddProvider(new ColorConsoleProvider());
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddSingleton<ILoggerFactory>(loggerFactory);

			Configure(serviceCollection);
			var serviceProvider = serviceCollection.BuildServiceProvider();
			Execute(loggerFactory, serviceProvider, args);
		}

		private static void Configure(ServiceCollection serviceCollection) { }

		private static unsafe void Execute(
			ILoggerFactory loggerFactory, IServiceProvider serviceProvider, string[] args)
		{
			var array = BuildArray(1_000_000, 1);
			TimSort.Sort(array, TimComparer.Default<int>());

			ValidateArray(array);
		}

		private static int[] BuildArray(int size, int seed)
		{
			var array = new int[size];
			var random = new Random(seed);
			for (var i = 0; i < array.Length; i++) array[i] = random.Next();
			return array;
		}

		private static void ValidateArray(IReadOnlyList<int> array)
		{
			for (var i = 1; i < array.Count; i++)
			{
				if (array[i] < array[i - 1])
					throw new Exception("Not sorted!");
			}
		}
	}
}
