``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1348 (21H1/May2021Update)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT


```
|                   Method |      Mean |     Error |    StdDev |
|------------------------- |----------:|----------:|----------:|
|          AddOffsetsFirst | 0.2410 ns | 0.0021 ns | 0.0019 ns |
|      AddOffsetsToPointer | 0.2517 ns | 0.0031 ns | 0.0027 ns |
| AddOffsetsToPointerNoSub | 0.2341 ns | 0.0033 ns | 0.0029 ns |
