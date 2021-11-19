``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1348 (21H1/May2021Update)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT


```
|         Method |   Size |  Order |    Mean |    Error |   StdDev | Ratio |
|--------------- |------- |------- |--------:|---------:|---------:|------:|
|        Default | 100000 | Random | 4.796 s | 0.0016 s | 0.0013 s |  1.00 |
|  UsingPointers | 100000 | Random | 4.795 s | 0.0007 s | 0.0006 s |  1.00 |
| UsingReference | 100000 | Random | 4.795 s | 0.0010 s | 0.0009 s |  1.00 |
