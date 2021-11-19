``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1348 (21H1/May2021Update)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT


```
|    Method |   Size | Threshold |  Order |      Mean |    Error |   StdDev | Ratio | RatioSD |
|---------- |------- |---------- |------- |----------:|---------:|---------:|------:|--------:|
|      Linq | 100000 |         0 | Random | 578.08 μs | 9.833 μs | 8.717 μs |  7.96 |    0.10 |
|    Manual | 100000 |         0 | Random |  72.66 μs | 0.295 μs | 0.261 μs |  1.00 |    0.00 |
| Dedicated | 100000 |         0 | Random |  72.64 μs | 0.382 μs | 0.357 μs |  1.00 |    0.01 |
|   Generic | 100000 |         0 | Random |  72.56 μs | 0.266 μs | 0.207 μs |  1.00 |    0.01 |
