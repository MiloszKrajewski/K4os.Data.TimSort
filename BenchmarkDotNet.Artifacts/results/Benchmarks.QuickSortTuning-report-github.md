``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1348 (21H1/May2021Update)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT


```
|  Method |   Size |  Order |     Mean |     Error |    StdDev | Ratio |
|-------- |------- |------- |---------:|----------:|----------:|------:|
| Default | 100000 | Random | 6.196 ms | 0.0129 ms | 0.0114 ms |  1.00 |
|    Mine | 100000 | Random | 6.087 ms | 0.0093 ms | 0.0087 ms |  0.98 |