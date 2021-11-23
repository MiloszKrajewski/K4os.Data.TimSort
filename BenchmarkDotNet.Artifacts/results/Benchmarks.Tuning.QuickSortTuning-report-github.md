``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1348 (21H2)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT


```
| Method |   Size |  Order |     Mean |     Error |    StdDev | Ratio |
|------- |------- |------- |---------:|----------:|----------:|------:|
| Theirs | 100000 | Random | 6.110 ms | 0.0046 ms | 0.0041 ms |  1.00 |
|   Mine | 100000 | Random | 6.061 ms | 0.0116 ms | 0.0103 ms |  0.99 |
