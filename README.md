# K4os.Data.TimSort
TimSort - "fastest general purpose sorting algorithm you never heard of" for .NET/Core

[![NuGet Stats](https://img.shields.io/nuget/v/K4os.Data.TimSort.svg)](https://www.nuget.org/packages/K4os.Data.TimSort)

TimSort is relatively new sorting algorithm invented by Tim Peters in 2002, 
which is a hybrid of adaptive MergeSort and InsertionSort. In term of 
algorithm complexity it is not worse than IntroSort (which is modified version 
of QuickSort and is used as default sorting algorithm in .NET). 

TimSort's average case performance is O(n log n) (same as QuickSort) but both best case 
and worst case performances are better then QuickSort: O(n) and O(n log n) 
respectively (QuickSort is O(n log n) and O(n^2)).

|     Algorithm |    Best    |  Average   |   Worst    |  Memory  |  
|--------------:|:----------:|:----------:|:----------:|:--------:|
|       TimSort |    O(n)    | O(n log n) | O(n log n) |  O(n/2)  | 
|     IntroSort | O(n log n) | O(n log n) | O(n log n) | O(log n) | 
|     QuickSort | O(n log n) | O(n log n) |   O(n^2)   | O(log n) |
|      HeapSort | O(n log n) | O(n log n) | O(n log n) |   O(1)   |
| InsertionSort |    O(n)    |   O(n^2)   |   O(n^2)   |   O(1)   |

NOTE: TimSort needs O(n/2) memory in worst case scenario, but just 
fixed size buffer on average. Yet, this is important factor. Because of
memory allocation TimSort may not perform better than IntroSort for 
small arrays and simple data types, as cost of memory allocation can be
relatively high.

NOTE: IntroSort is a combination of QuickSort, HeapSort and InsertionSort. 
It falls back to HeapSort when QuickSort seems to be progressing slowly 
(too many levels of recursion), and uses InsertionSort on small slices of 
sorted collection.

# Usage

Both `TimSort` and `IntroSort` provide same interface.
They are implemented as multiple extension methods, accepting different kinds of *arrays* and different kinds of *comparators*.

Allowed types of "arrays" are: `T[]`, `Span<T>`, `List<T>`, `IList<T>`.
Optionally, all "arrays" can be passed with `int offset, int length` allowing sorting only part of it.
Allowed types of "comparators" are: none, `Comparison<T>`, `Comparer<T>`, `IComparer<T>`, or native to this library `ILessThan<T>`.

You can mix and match any type of "arrays" and "comparators". If comparator is not specified, then default one is used. 

For example:

```csharp
var array = new int[1000];
FillWithRandomNumbers(array);
array.TimSort();
```

will sort array of ints in ascending order, while:

```csharp
var array = new int[1000];
FillWithRandomNumbers(array);
array.TimSort(500, 500, (a, b) => b.CompareTo(a));
```

will sort only last 500 elements of array in descending order.

Absolutely same rules apply to `IntroSort`.

# Performance

*QuickSort* is fast, actually very fast. *TimSort* is not generally faster, but comes with some advantages:

* It is stable, if array is somewhat sorted already it will retain original order
* It uses less comparisons, so when compare function is slow (complex object) it will do better than *QuickSort*
* It handles presorted "islands" of elements, so it is faster if data is more or less sorted already (which is actually common case)




# Build

```shell
./build.cmd
```
