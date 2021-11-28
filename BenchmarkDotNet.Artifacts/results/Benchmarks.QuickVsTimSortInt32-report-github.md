``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1348 (21H2)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT


```
|            Method |   Size |      Order |             Mean |         Error |        StdDev |           Median |
|------------------ |------- |----------- |-----------------:|--------------:|--------------:|-----------------:|
|        **Background** |   **1000** |     **Random** |         **44.49 ns** |      **0.089 ns** |      **0.079 ns** |         **44.49 ns** |
|    TimSort_Native |   1000 |     Random |     47,390.92 ns |    246.642 ns |    230.709 ns |     47,412.11 ns |
|  IntroSort_Native |   1000 |     Random |     16,038.99 ns |    306.076 ns |    286.303 ns |     15,998.52 ns |
|   TimSort_Virtual |   1000 |     Random |     65,556.82 ns |    210.029 ns |    186.185 ns |     65,593.66 ns |
| IntroSort_Virtual |   1000 |     Random |     38,380.46 ns |     99.409 ns |     88.124 ns |     38,389.59 ns |
|        **Background** |   **1000** |  **Ascending** |         **46.28 ns** |      **0.126 ns** |      **0.118 ns** |         **46.26 ns** |
|    TimSort_Native |   1000 |  Ascending |        422.66 ns |      1.922 ns |      1.704 ns |        423.06 ns |
|  IntroSort_Native |   1000 |  Ascending |      5,639.16 ns |     33.385 ns |     29.595 ns |      5,642.39 ns |
|   TimSort_Virtual |   1000 |  Ascending |      1,840.78 ns |      4.795 ns |      4.250 ns |      1,839.25 ns |
| IntroSort_Virtual |   1000 |  Ascending |     15,948.14 ns |     45.657 ns |     42.707 ns |     15,945.72 ns |
|        **Background** |   **1000** | **Descending** |         **46.34 ns** |      **0.105 ns** |      **0.088 ns** |         **46.35 ns** |
|    TimSort_Native |   1000 | Descending |        667.89 ns |      3.765 ns |      3.338 ns |        667.10 ns |
|  IntroSort_Native |   1000 | Descending |     11,125.51 ns |     36.953 ns |     32.758 ns |     11,124.35 ns |
|   TimSort_Virtual |   1000 | Descending |      2,085.33 ns |      7.223 ns |      6.756 ns |      2,082.54 ns |
| IntroSort_Virtual |   1000 | Descending |     31,624.00 ns |     88.999 ns |     83.250 ns |     31,646.07 ns |
|        **Background** |  **10000** |     **Random** |        **665.42 ns** |      **0.364 ns** |      **0.304 ns** |        **665.42 ns** |
|    TimSort_Native |  10000 |     Random |    729,107.52 ns |  2,416.911 ns |  2,260.780 ns |    728,142.97 ns |
|  IntroSort_Native |  10000 |     Random |    391,472.81 ns |    831.141 ns |    777.450 ns |    391,279.00 ns |
|   TimSort_Virtual |  10000 |     Random |    969,830.57 ns |  1,234.356 ns |  1,030.743 ns |    969,537.99 ns |
| IntroSort_Virtual |  10000 |     Random |    590,492.36 ns |    772.287 ns |    722.397 ns |    590,479.49 ns |
|        **Background** |  **10000** |  **Ascending** |        **644.12 ns** |      **0.335 ns** |      **0.280 ns** |        **644.18 ns** |
|    TimSort_Native |  10000 |  Ascending |      3,681.86 ns |     27.304 ns |     25.540 ns |      3,689.40 ns |
|  IntroSort_Native |  10000 |  Ascending |     73,422.73 ns |    335.218 ns |    313.563 ns |     73,489.06 ns |
|   TimSort_Virtual |  10000 |  Ascending |     17,563.08 ns |     12.427 ns |     11.016 ns |     17,561.22 ns |
| IntroSort_Virtual |  10000 |  Ascending |    214,290.38 ns |    442.904 ns |    414.293 ns |    214,251.32 ns |
|        **Background** |  **10000** | **Descending** |        **644.49 ns** |      **1.115 ns** |      **1.043 ns** |        **643.91 ns** |
|    TimSort_Native |  10000 | Descending |      6,171.23 ns |     31.806 ns |     29.751 ns |      6,164.59 ns |
|  IntroSort_Native |  10000 | Descending |    151,367.18 ns |    874.163 ns |    817.692 ns |    151,588.70 ns |
|   TimSort_Virtual |  10000 | Descending |     20,047.93 ns |     82.830 ns |     73.427 ns |     20,019.36 ns |
| IntroSort_Virtual |  10000 | Descending |    443,519.32 ns |    490.509 ns |    434.823 ns |    443,302.93 ns |
|        **Background** | **100000** |     **Random** |      **7,886.75 ns** |      **8.637 ns** |      **8.079 ns** |      **7,883.76 ns** |
|    TimSort_Native | 100000 |     Random |  9,102,453.85 ns | 22,767.172 ns | 21,296.427 ns |  9,102,389.06 ns |
|  IntroSort_Native | 100000 |     Random |  4,832,850.21 ns | 12,404.690 ns | 11,603.354 ns |  4,829,440.62 ns |
|   TimSort_Virtual | 100000 |     Random | 12,488,648.75 ns | 32,967.209 ns | 30,837.548 ns | 12,481,346.88 ns |
| IntroSort_Virtual | 100000 |     Random |  7,443,858.92 ns |  8,563.006 ns |  7,150.498 ns |  7,441,352.73 ns |
|        **Background** | **100000** |  **Ascending** |      **8,438.56 ns** |     **13.178 ns** |     **12.327 ns** |      **8,434.76 ns** |
|    TimSort_Native | 100000 |  Ascending |     36,580.31 ns |     63.077 ns |     55.916 ns |     36,576.89 ns |
|  IntroSort_Native | 100000 |  Ascending |    926,328.00 ns | 27,592.014 ns | 81,355.675 ns |    886,283.98 ns |
|   TimSort_Virtual | 100000 |  Ascending |    175,644.21 ns |    270.052 ns |    239.394 ns |    175,603.25 ns |
| IntroSort_Virtual | 100000 |  Ascending |  2,625,597.24 ns |  5,504.444 ns |  5,148.861 ns |  2,627,335.55 ns |
|        **Background** | **100000** | **Descending** |      **7,902.09 ns** |     **11.820 ns** |     **10.478 ns** |      **7,897.31 ns** |
|    TimSort_Native | 100000 | Descending |    166,461.51 ns |  1,038.055 ns |    970.997 ns |    166,876.98 ns |
|  IntroSort_Native | 100000 | Descending |  1,883,438.67 ns |  7,218.162 ns |  6,398.712 ns |  1,883,671.78 ns |
|   TimSort_Virtual | 100000 | Descending |    304,533.52 ns |  1,269.451 ns |  1,125.335 ns |    304,864.23 ns |
| IntroSort_Virtual | 100000 | Descending |  5,566,361.18 ns |  6,199.024 ns |  5,176.465 ns |  5,566,773.44 ns |