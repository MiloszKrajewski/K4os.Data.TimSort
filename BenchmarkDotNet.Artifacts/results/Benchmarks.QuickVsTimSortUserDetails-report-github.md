``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1348 (21H2)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT


```
|            Method |   Size |      Order |             Mean |         Error |        StdDev |
|------------------ |------- |----------- |-----------------:|--------------:|--------------:|
|        **Background** |   **1000** |     **Random** |         **137.6 ns** |       **0.25 ns** |       **0.22 ns** |
|    TimSort_Native |   1000 |     Random |     474,912.7 ns |     501.06 ns |     444.18 ns |
|  IntroSort_Native |   1000 |     Random |     490,650.4 ns |     808.19 ns |     755.98 ns |
|   TimSort_Virtual |   1000 |     Random |     458,601.7 ns |     741.04 ns |     693.17 ns |
| IntroSort_Virtual |   1000 |     Random |     455,142.0 ns |     331.42 ns |     276.75 ns |
|        **Background** |   **1000** |  **Ascending** |         **137.9 ns** |       **0.37 ns** |       **0.33 ns** |
|    TimSort_Native |   1000 |  Ascending |      45,645.5 ns |     116.21 ns |     108.71 ns |
|  IntroSort_Native |   1000 |  Ascending |     303,196.9 ns |     719.03 ns |     672.58 ns |
|   TimSort_Virtual |   1000 |  Ascending |      41,874.3 ns |     108.18 ns |      95.90 ns |
| IntroSort_Virtual |   1000 |  Ascending |     287,398.6 ns |     456.25 ns |     404.46 ns |
|        **Background** |   **1000** | **Descending** |         **139.2 ns** |       **0.63 ns** |       **0.59 ns** |
|    TimSort_Native |   1000 | Descending |      42,355.7 ns |      79.41 ns |      70.40 ns |
|  IntroSort_Native |   1000 | Descending |     580,172.8 ns |   1,955.30 ns |   1,828.99 ns |
|   TimSort_Virtual |   1000 | Descending |      41,155.1 ns |     101.85 ns |      95.27 ns |
| IntroSort_Virtual |   1000 | Descending |     570,688.7 ns |   1,919.72 ns |   1,701.78 ns |
|        **Background** |  **10000** |     **Random** |       **1,719.0 ns** |       **5.86 ns** |       **5.20 ns** |
|    TimSort_Native |  10000 |     Random |   6,509,234.3 ns |   8,552.00 ns |   7,999.54 ns |
|  IntroSort_Native |  10000 |     Random |   6,681,386.2 ns |   7,878.03 ns |   7,369.11 ns |
|   TimSort_Virtual |  10000 |     Random |   6,037,482.3 ns |   6,597.28 ns |   6,171.10 ns |
| IntroSort_Virtual |  10000 |     Random |   6,311,327.3 ns |   4,835.89 ns |   4,038.19 ns |
|        **Background** |  **10000** |  **Ascending** |       **1,511.7 ns** |       **2.69 ns** |       **2.25 ns** |
|    TimSort_Native |  10000 |  Ascending |     418,708.8 ns |     837.25 ns |     742.20 ns |
|  IntroSort_Native |  10000 |  Ascending |   4,087,515.4 ns |   3,318.39 ns |   2,941.67 ns |
|   TimSort_Virtual |  10000 |  Ascending |     403,880.6 ns |     260.67 ns |     231.08 ns |
| IntroSort_Virtual |  10000 |  Ascending |   4,068,468.4 ns |   3,113.56 ns |   2,760.09 ns |
|        **Background** |  **10000** | **Descending** |       **1,512.3 ns** |       **5.13 ns** |       **4.55 ns** |
|    TimSort_Native |  10000 | Descending |     399,313.4 ns |     609.69 ns |     570.31 ns |
|  IntroSort_Native |  10000 | Descending |   8,109,427.9 ns |  19,422.49 ns |  18,167.81 ns |
|   TimSort_Virtual |  10000 | Descending |     393,262.0 ns |     371.21 ns |     309.98 ns |
| IntroSort_Virtual |  10000 | Descending |   7,719,925.8 ns |  15,367.85 ns |  13,623.20 ns |
|        **Background** | **100000** |     **Random** |      **19,000.8 ns** |      **28.13 ns** |      **26.31 ns** |
|    TimSort_Native | 100000 |     Random |  80,427,392.4 ns | 348,347.55 ns | 325,844.51 ns |
|  IntroSort_Native | 100000 |     Random |  85,424,052.4 ns |  84,437.84 ns |  74,851.95 ns |
|   TimSort_Virtual | 100000 |     Random |  78,457,803.1 ns | 328,659.81 ns | 291,348.36 ns |
| IntroSort_Virtual | 100000 |     Random |  76,847,509.5 ns | 128,849.81 ns | 120,526.19 ns |
|        **Background** | **100000** |  **Ascending** |      **19,147.6 ns** |      **28.19 ns** |      **24.99 ns** |
|    TimSort_Native | 100000 |  Ascending |   4,741,146.7 ns |  11,744.43 ns |  10,985.75 ns |
|  IntroSort_Native | 100000 |  Ascending |  55,559,020.5 ns | 165,454.13 ns | 138,161.69 ns |
|   TimSort_Virtual | 100000 |  Ascending |   4,565,961.9 ns |  14,391.73 ns |  13,462.03 ns |
| IntroSort_Virtual | 100000 |  Ascending |  51,753,906.0 ns | 193,553.48 ns | 181,050.05 ns |
|        **Background** | **100000** | **Descending** |      **18,747.8 ns** |      **21.55 ns** |      **17.99 ns** |
|    TimSort_Native | 100000 | Descending |   4,613,096.7 ns |   4,583.42 ns |   3,578.43 ns |
|  IntroSort_Native | 100000 | Descending | 109,858,678.7 ns | 272,075.90 ns | 254,499.97 ns |
|   TimSort_Virtual | 100000 | Descending |   4,509,166.8 ns |   8,531.95 ns |   7,980.79 ns |
| IntroSort_Virtual | 100000 | Descending |  98,207,008.9 ns | 628,168.90 ns | 587,589.58 ns |
