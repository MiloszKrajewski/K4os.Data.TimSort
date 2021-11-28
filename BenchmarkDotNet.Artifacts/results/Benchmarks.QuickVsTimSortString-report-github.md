``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1348 (21H2)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT


```
|            Method |   Size |      Order |            Mean |         Error |        StdDev |
|------------------ |------- |----------- |----------------:|--------------:|--------------:|
|        **Background** |   **1000** |     **Random** |        **139.7 ns** |       **0.70 ns** |       **0.62 ns** |
|    TimSort_Native |   1000 |     Random |    450,441.4 ns |     418.29 ns |     391.27 ns |
|  IntroSort_Native |   1000 |     Random |    451,107.4 ns |   1,435.46 ns |   1,342.73 ns |
|   TimSort_Virtual |   1000 |     Random |    392,027.6 ns |     439.77 ns |     411.37 ns |
| IntroSort_Virtual |   1000 |     Random |    385,727.7 ns |     401.74 ns |     375.79 ns |
|        **Background** |   **1000** |  **Ascending** |        **141.2 ns** |       **0.48 ns** |       **0.45 ns** |
|    TimSort_Native |   1000 |  Ascending |     39,567.7 ns |      90.34 ns |      84.50 ns |
|  IntroSort_Native |   1000 |  Ascending |    274,609.3 ns |     809.64 ns |     757.33 ns |
|   TimSort_Virtual |   1000 |  Ascending |     35,356.3 ns |     140.95 ns |     131.84 ns |
| IntroSort_Virtual |   1000 |  Ascending |    235,147.2 ns |     515.24 ns |     481.96 ns |
|        **Background** |   **1000** | **Descending** |        **142.2 ns** |       **0.42 ns** |       **0.39 ns** |
|    TimSort_Native |   1000 | Descending |     36,250.1 ns |     119.86 ns |     112.11 ns |
|  IntroSort_Native |   1000 | Descending |    510,148.4 ns |   1,007.96 ns |     942.85 ns |
|   TimSort_Virtual |   1000 | Descending |     34,661.0 ns |      52.06 ns |      46.15 ns |
| IntroSort_Virtual |   1000 | Descending |    445,046.0 ns |     756.33 ns |     670.47 ns |
|        **Background** |  **10000** |     **Random** |      **1,707.6 ns** |       **1.47 ns** |       **1.22 ns** |
|    TimSort_Native |  10000 |     Random |  6,161,145.9 ns |   8,727.86 ns |   7,288.16 ns |
|  IntroSort_Native |  10000 |     Random |  6,141,136.7 ns |  10,123.47 ns |   9,469.50 ns |
|   TimSort_Virtual |  10000 |     Random |  5,181,490.2 ns |   9,347.04 ns |   8,285.91 ns |
| IntroSort_Virtual |  10000 |     Random |  5,090,963.8 ns |  11,430.14 ns |  10,691.76 ns |
|        **Background** |  **10000** |  **Ascending** |      **1,708.2 ns** |       **4.59 ns** |       **4.07 ns** |
|    TimSort_Native |  10000 |  Ascending |    397,242.9 ns |     831.03 ns |     777.35 ns |
|  IntroSort_Native |  10000 |  Ascending |  3,787,092.7 ns |   5,484.82 ns |   5,130.51 ns |
|   TimSort_Virtual |  10000 |  Ascending |    368,152.1 ns |     266.51 ns |     236.25 ns |
| IntroSort_Virtual |  10000 |  Ascending |  3,358,199.6 ns |  11,783.51 ns |  11,022.30 ns |
|        **Background** |  **10000** | **Descending** |      **1,715.1 ns** |       **3.69 ns** |       **3.27 ns** |
|    TimSort_Native |  10000 | Descending |    402,759.7 ns |     413.11 ns |     344.97 ns |
|  IntroSort_Native |  10000 | Descending |  7,126,411.9 ns |   6,736.36 ns |   5,971.61 ns |
|   TimSort_Virtual |  10000 | Descending |    377,185.1 ns |     807.21 ns |     755.06 ns |
| IntroSort_Virtual |  10000 | Descending |  6,112,885.7 ns |   6,099.53 ns |   5,407.08 ns |
|        **Background** | **100000** |     **Random** |     **18,733.9 ns** |      **20.90 ns** |      **19.55 ns** |
|    TimSort_Native | 100000 |     Random | 79,607,509.5 ns | 502,018.41 ns | 469,588.34 ns |
|  IntroSort_Native | 100000 |     Random | 77,719,220.0 ns | 349,848.01 ns | 327,248.05 ns |
|   TimSort_Virtual | 100000 |     Random | 71,320,139.0 ns | 289,691.68 ns | 270,977.78 ns |
| IntroSort_Virtual | 100000 |     Random | 67,731,995.0 ns | 129,931.41 ns | 121,537.93 ns |
|        **Background** | **100000** |  **Ascending** |     **18,553.7 ns** |      **36.72 ns** |      **34.35 ns** |
|    TimSort_Native | 100000 |  Ascending |  4,943,131.6 ns |  95,168.78 ns |  89,020.94 ns |
|  IntroSort_Native | 100000 |  Ascending | 53,753,127.3 ns | 441,302.05 ns | 412,794.21 ns |
|   TimSort_Virtual | 100000 |  Ascending |  4,363,449.8 ns |  77,296.17 ns |  82,706.03 ns |
| IntroSort_Virtual | 100000 |  Ascending | 46,791,640.9 ns | 866,908.55 ns | 851,420.12 ns |
|        **Background** | **100000** | **Descending** |     **18,658.1 ns** |      **51.78 ns** |      **48.44 ns** |
|    TimSort_Native | 100000 | Descending |  4,714,281.9 ns |  91,553.84 ns |  85,639.52 ns |
|  IntroSort_Native | 100000 | Descending | 99,492,153.6 ns | 519,147.45 ns | 460,210.70 ns |
|   TimSort_Virtual | 100000 | Descending |  4,502,502.3 ns |  83,803.82 ns | 165,420.56 ns |
| IntroSort_Virtual | 100000 | Descending | 86,060,374.4 ns | 897,221.44 ns | 839,261.49 ns |