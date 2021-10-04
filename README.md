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

| Algorithm     | Best       | Average    | Worst      | Memory     |  
|--------------:|:----------:|:----------:|:----------:|:----------:|
| TimSort       | O(n)       | O(n log n) | O(n log n) | O(n/2)     | 
| IntroSort     | O(n log n) | O(n log n) | O(n log n) | O(n log n) | 
| QuickSort     | O(n log n) | O(n log n) | O(n^2)     | O(n log n) |
| HeapSort      | O(n log n) | O(n log n) | O(n log n) | O(1)       |
| InsertionSort | O(n)       | O(n^2)     | O(n^2)     | O(1)       |

NOTE: TimSort needs O(n/2) memory in worst case scenario, but just 
fixed size buffer on average. Yet, this is important factor. Because of
memory allocation TimSort may not perform better than IntroSort for 
small arrays and simple data types, as cost of memory allocation can be
relatively high.

NOTE: IntroSort is a combination of QuickSort, HeapSort and InsertionSort. 
It falls back to HeapSort when QuickSort seems to be progressing slowly 
(too many levels of recursion), and uses InsertionSort on small slices of 
sorted collection.

# Performance

TBD

# Usage

TBD

# Build

```shell
paket install
fake build
```
