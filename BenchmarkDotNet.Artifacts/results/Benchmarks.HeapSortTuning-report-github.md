``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1348 (21H1/May2021Update)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT


```
| Method |   Size |  Order |      Mean |     Error |    StdDev | Ratio |
|------- |------- |------- |----------:|----------:|----------:|------:|
| Theirs | 100000 | Random | 10.166 ms | 0.0379 ms | 0.0355 ms |  1.00 |
|   Mine | 100000 | Random |  9.684 ms | 0.0196 ms | 0.0174 ms |  0.95 |