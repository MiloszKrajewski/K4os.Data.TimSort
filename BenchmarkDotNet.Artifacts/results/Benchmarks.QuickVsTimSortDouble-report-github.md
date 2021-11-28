``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1348 (21H2)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT


```
|            Method |   Size |      Order |             Mean |          Error |        StdDev |
|------------------ |------- |----------- |-----------------:|---------------:|--------------:|
|        **Background** |   **1000** |     **Random** |         **74.24 ns** |       **0.296 ns** |      **0.277 ns** |
|    TimSort_Native |   1000 |     Random |     55,996.25 ns |     363.032 ns |    321.819 ns |
|  IntroSort_Native |   1000 |     Random |     23,501.46 ns |     187.964 ns |    175.822 ns |
|   TimSort_Virtual |   1000 |     Random |     93,239.87 ns |     343.621 ns |    286.939 ns |
| IntroSort_Virtual |   1000 |     Random |     80,047.15 ns |     222.392 ns |    208.026 ns |
|        **Background** |   **1000** |  **Ascending** |         **72.64 ns** |       **0.340 ns** |      **0.318 ns** |
|    TimSort_Native |   1000 |  Ascending |        567.42 ns |       2.575 ns |      2.151 ns |
|  IntroSort_Native |   1000 |  Ascending |      6,336.24 ns |       9.923 ns |      8.286 ns |
|   TimSort_Virtual |   1000 |  Ascending |      3,363.26 ns |       9.460 ns |      8.386 ns |
| IntroSort_Virtual |   1000 |  Ascending |     31,812.41 ns |     140.659 ns |    117.456 ns |
|        **Background** |   **1000** | **Descending** |         **74.59 ns** |       **0.173 ns** |      **0.145 ns** |
|    TimSort_Native |   1000 | Descending |        850.82 ns |       2.690 ns |      2.385 ns |
|  IntroSort_Native |   1000 | Descending |     12,271.80 ns |      38.723 ns |     30.232 ns |
|   TimSort_Virtual |   1000 | Descending |      3,880.45 ns |      18.792 ns |     17.578 ns |
| IntroSort_Virtual |   1000 | Descending |     61,265.45 ns |     170.463 ns |    159.451 ns |
|        **Background** |  **10000** |     **Random** |      **1,270.36 ns** |       **1.385 ns** |      **1.156 ns** |
|    TimSort_Native |  10000 |     Random |    870,602.72 ns |   3,013.790 ns |  2,671.646 ns |
|  IntroSort_Native |  10000 |     Random |    486,478.67 ns |   1,079.718 ns |  1,009.969 ns |
|   TimSort_Virtual |  10000 |     Random |  1,344,334.62 ns |   2,033.586 ns |  1,902.218 ns |
| IntroSort_Virtual |  10000 |     Random |  1,107,345.60 ns |   1,983.190 ns |  1,855.077 ns |
|        **Background** |  **10000** |  **Ascending** |      **1,271.60 ns** |       **1.647 ns** |      **1.460 ns** |
|    TimSort_Native |  10000 |  Ascending |      5,222.28 ns |      27.976 ns |     24.800 ns |
|  IntroSort_Native |  10000 |  Ascending |     83,380.78 ns |     299.269 ns |    279.936 ns |
|   TimSort_Virtual |  10000 |  Ascending |     39,884.55 ns |      69.438 ns |     61.555 ns |
| IntroSort_Virtual |  10000 |  Ascending |    437,834.80 ns |     814.258 ns |    761.657 ns |
|        **Background** |  **10000** | **Descending** |      **1,274.36 ns** |       **0.700 ns** |      **0.547 ns** |
|    TimSort_Native |  10000 | Descending |      7,830.07 ns |      65.275 ns |     61.058 ns |
|  IntroSort_Native |  10000 | Descending |    165,118.65 ns |   1,065.756 ns |    996.909 ns |
|   TimSort_Virtual |  10000 | Descending |     35,252.09 ns |      19.815 ns |     18.535 ns |
| IntroSort_Virtual |  10000 | Descending |    842,946.17 ns |   1,282.781 ns |  1,137.152 ns |
|        **Background** | **100000** |     **Random** |     **15,653.40 ns** |      **18.306 ns** |     **16.228 ns** |
|    TimSort_Native | 100000 |     Random | 10,948,736.83 ns |  17,136.804 ns | 15,191.330 ns |
|  IntroSort_Native | 100000 |     Random |  6,088,432.92 ns |  12,072.221 ns | 10,701.710 ns |
|   TimSort_Virtual | 100000 |     Random | 16,891,674.79 ns |  64,119.884 ns | 59,977.779 ns |
| IntroSort_Virtual | 100000 |     Random | 14,528,048.65 ns | 104,995.643 ns | 98,212.990 ns |
|        **Background** | **100000** |  **Ascending** |     **16,847.01 ns** |      **13.965 ns** |     **13.062 ns** |
|    TimSort_Native | 100000 |  Ascending |     54,685.35 ns |     190.692 ns |    169.044 ns |
|  IntroSort_Native | 100000 |  Ascending |    997,404.35 ns |   3,999.201 ns |  3,740.855 ns |
|   TimSort_Virtual | 100000 |  Ascending |    401,466.63 ns |     494.347 ns |    462.412 ns |
| IntroSort_Virtual | 100000 |  Ascending |  5,413,754.95 ns |  14,411.099 ns | 13,480.151 ns |
|        **Background** | **100000** | **Descending** |     **16,780.15 ns** |      **10.827 ns** |      **9.041 ns** |
|    TimSort_Native | 100000 | Descending |    174,934.77 ns |   1,483.711 ns |  1,387.865 ns |
|  IntroSort_Native | 100000 | Descending |  2,006,769.06 ns |   2,436.921 ns |  2,160.267 ns |
|   TimSort_Virtual | 100000 | Descending |    443,201.00 ns |   2,704.296 ns |  2,529.600 ns |
| IntroSort_Virtual | 100000 | Descending | 10,710,377.79 ns |  22,024.537 ns | 19,524.178 ns |
