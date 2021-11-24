``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1348 (21H2)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT


```
|    Method | Size |  Order |     Mean |   Error |  StdDev | Ratio |
|---------- |----- |------- |---------:|--------:|--------:|------:|
| Insertion |   24 | Random | 114.2 ns | 0.46 ns | 0.41 ns |  1.00 |
|    Binary |   24 | Random | 318.9 ns | 1.12 ns | 1.05 ns |  2.79 |
