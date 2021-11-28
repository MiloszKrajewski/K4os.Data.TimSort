``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1348 (21H2)
AMD Ryzen 5 3600, 1 CPU, 12 logical and 6 physical cores
.NET SDK=5.0.300
  [Host]     : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT
  DefaultJob : .NET 5.0.12 (5.0.1221.52207), X64 RyuJIT


```
|            Method |   Size |      Order |            Mean |         Error |        StdDev |
|------------------ |------- |----------- |----------------:|--------------:|--------------:|
|        **Background** |   **1000** |     **Random** |        **170.6 ns** |       **0.33 ns** |       **0.26 ns** |
|    TimSort_Native |   1000 |     Random |    107,628.7 ns |     229.54 ns |     214.71 ns |
|  IntroSort_Native |   1000 |     Random |     71,821.8 ns |     413.86 ns |     387.12 ns |
|   TimSort_Virtual |   1000 |     Random |    110,131.5 ns |     172.78 ns |     153.17 ns |
| IntroSort_Virtual |   1000 |     Random |     82,001.6 ns |     501.63 ns |     469.22 ns |
|        **Background** |   **1000** |  **Ascending** |        **192.9 ns** |       **0.22 ns** |       **0.19 ns** |
|    TimSort_Native |   1000 |  Ascending |      2,989.0 ns |      10.18 ns |       8.50 ns |
|  IntroSort_Native |   1000 |  Ascending |     28,097.5 ns |      62.02 ns |      54.98 ns |
|   TimSort_Virtual |   1000 |  Ascending |      3,725.9 ns |      23.08 ns |      20.46 ns |
| IntroSort_Virtual |   1000 |  Ascending |     33,882.6 ns |     102.10 ns |      90.51 ns |
|        **Background** |   **1000** | **Descending** |        **189.0 ns** |       **0.18 ns** |       **0.17 ns** |
|    TimSort_Native |   1000 | Descending |      3,644.5 ns |      10.32 ns |       9.15 ns |
|  IntroSort_Native |   1000 | Descending |     59,023.9 ns |     934.55 ns |     780.39 ns |
|   TimSort_Virtual |   1000 | Descending |      4,343.2 ns |      12.17 ns |      11.38 ns |
| IntroSort_Virtual |   1000 | Descending |     69,600.5 ns |     230.00 ns |     203.89 ns |
|        **Background** |  **10000** |     **Random** |      **3,069.2 ns** |       **3.49 ns** |       **3.27 ns** |
|    TimSort_Native |  10000 |     Random |  1,593,565.4 ns |   3,163.39 ns |   2,959.03 ns |
|  IntroSort_Native |  10000 |     Random |  1,068,694.3 ns |   9,967.45 ns |   9,323.56 ns |
|   TimSort_Virtual |  10000 |     Random |  1,603,636.3 ns |   2,572.27 ns |   2,406.10 ns |
| IntroSort_Virtual |  10000 |     Random |  1,171,494.4 ns |   1,356.32 ns |   1,058.92 ns |
|        **Background** |  **10000** |  **Ascending** |      **3,063.7 ns** |       **1.81 ns** |       **1.69 ns** |
|    TimSort_Native |  10000 |  Ascending |     29,923.4 ns |      73.91 ns |      69.14 ns |
|  IntroSort_Native |  10000 |  Ascending |    384,369.2 ns |   2,963.27 ns |   2,626.86 ns |
|   TimSort_Virtual |  10000 |  Ascending |     36,073.9 ns |     149.14 ns |     139.51 ns |
| IntroSort_Virtual |  10000 |  Ascending |    469,566.6 ns |   3,035.23 ns |   2,534.56 ns |
|        **Background** |  **10000** | **Descending** |      **3,062.6 ns** |       **1.16 ns** |       **0.97 ns** |
|    TimSort_Native |  10000 | Descending |     35,196.1 ns |     120.39 ns |     112.62 ns |
|  IntroSort_Native |  10000 | Descending |    831,947.0 ns |  11,955.15 ns |  13,767.57 ns |
|   TimSort_Virtual |  10000 | Descending |     41,135.9 ns |     127.99 ns |     113.46 ns |
| IntroSort_Virtual |  10000 | Descending |    964,056.7 ns |   4,645.50 ns |   4,118.12 ns |
|        **Background** | **100000** |     **Random** |     **76,675.9 ns** |     **157.06 ns** |     **139.23 ns** |
|    TimSort_Native | 100000 |     Random | 20,330,783.5 ns |  56,959.68 ns |  53,280.12 ns |
|  IntroSort_Native | 100000 |     Random | 13,185,739.1 ns |  44,161.11 ns |  39,147.67 ns |
|   TimSort_Virtual | 100000 |     Random | 20,301,457.7 ns |  48,226.40 ns |  45,111.01 ns |
| IntroSort_Virtual | 100000 |     Random | 15,229,234.9 ns |  73,391.14 ns |  68,650.12 ns |
|        **Background** | **100000** |  **Ascending** |     **77,243.3 ns** |     **563.29 ns** |     **526.90 ns** |
|    TimSort_Native | 100000 |  Ascending |    373,824.8 ns |   2,127.01 ns |   1,776.15 ns |
|  IntroSort_Native | 100000 |  Ascending |  4,546,126.8 ns |  27,736.42 ns |  21,654.77 ns |
|   TimSort_Virtual | 100000 |  Ascending |    462,793.2 ns |   1,452.75 ns |   1,287.82 ns |
| IntroSort_Virtual | 100000 |  Ascending |  5,927,589.8 ns |  18,872.08 ns |  16,729.61 ns |
|        **Background** | **100000** | **Descending** |     **77,351.1 ns** |     **565.18 ns** |     **471.95 ns** |
|    TimSort_Native | 100000 | Descending |    415,746.5 ns |   1,995.13 ns |   1,768.63 ns |
|  IntroSort_Native | 100000 | Descending | 10,942,042.1 ns | 218,226.72 ns | 543,460.26 ns |
|   TimSort_Virtual | 100000 | Descending |    491,809.0 ns |   2,621.13 ns |   2,323.56 ns |
| IntroSort_Virtual | 100000 | Descending | 11,286,387.2 ns |  57,100.03 ns |  53,411.40 ns |