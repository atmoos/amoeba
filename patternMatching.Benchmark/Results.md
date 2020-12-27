# Benchmark results

# Run Environment
```
BenchmarkDotNet=v0.12.0, OS=arch 
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.108
  [Host]     : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
  DefaultJob : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
```

## 27 December 2020
---

### This Commit

#### Search
|            Method | WordsInDictionary | TextWordCount | WordSize |     Mean |   Error |  StdDev | Ratio | RatioSD |
|------------------ |------------------ |-------------- |--------- |---------:|--------:|--------:|------:|--------:|
| AhoCorasickSearch |                60 |           800 |       12 | 205.7 us | 1.32 us | 1.24 us |  1.90 |    0.01 |
|   DoubleTrieParse |                60 |           800 |       12 | 108.0 us | 0.36 us | 0.32 us |  1.00 |    0.00 |
|                   |                   |               |          |          |         |         |       |         |
| AhoCorasickSearch |                60 |           800 |       36 | 579.1 us | 8.88 us | 8.31 us |  1.96 |    0.02 |
|   DoubleTrieParse |                60 |           800 |       36 | 297.0 us | 1.95 us | 1.52 us |  1.00 |    0.00 |
|                   |                   |               |          |          |         |         |       |         |
| AhoCorasickSearch |               120 |           800 |       12 | 214.2 us | 1.02 us | 0.96 us |  1.97 |    0.01 |
|   DoubleTrieParse |               120 |           800 |       12 | 108.5 us | 0.50 us | 0.42 us |  1.00 |    0.00 |
|                   |                   |               |          |          |         |         |       |         |
| AhoCorasickSearch |               120 |           800 |       36 | 587.8 us | 3.12 us | 2.92 us |  1.86 |    0.01 |
|   DoubleTrieParse |               120 |           800 |       36 | 315.6 us | 1.40 us | 1.31 us |  1.00 |    0.00 |

### Build
|               Method | WordsInDictionary | WordSize |        Mean |     Error |    StdDev | Ratio | RatioSD |     Gen 0 |     Gen 1 |    Gen 2 |   Allocated |
|--------------------- |------------------ |--------- |------------:|----------:|----------:|------:|--------:|----------:|----------:|---------:|------------:|
|     AhoCorasickBuild |                60 |       12 |    350.7 us |   3.53 us |   3.30 us |  0.29 |    0.02 |  100.5859 |   41.5039 |        - |   492.29 KB |
|      DoubleTrieBuild |                60 |       12 |  1,255.6 us |  28.12 us |  81.58 us |  1.00 |    0.00 | 1052.7344 | 1015.6250 | 990.2344 | 19203.78 KB |
| AhoCorasickFullBuild |                60 |       12 |    454.4 us |   5.09 us |   4.76 us |  0.37 |    0.02 |  108.3984 |   53.7109 |        - |   572.09 KB |
|  DoubleTrieFullBuild |                60 |       12 |  1,336.1 us |  63.67 us | 187.72 us |  1.07 |    0.17 | 1037.1094 | 1001.9531 | 974.6094 | 19204.01 KB |
|                      |                   |          |             |           |           |       |         |           |           |          |             |
|     AhoCorasickBuild |                60 |       36 |  1,834.7 us |   6.60 us |   5.85 us |  0.88 |    0.01 |  320.3125 |  160.1563 |        - |  1965.92 KB |
|      DoubleTrieBuild |                60 |       36 |  2,075.9 us |  13.39 us |  11.18 us |  1.00 |    0.00 | 1128.9063 | 1105.4688 | 980.4688 | 19834.24 KB |
| AhoCorasickFullBuild |                60 |       36 |  2,226.8 us |  33.43 us |  29.64 us |  1.07 |    0.01 |  371.0938 |  183.5938 |        - |  2280.11 KB |
|  DoubleTrieFullBuild |                60 |       36 |  2,385.0 us |  46.61 us |  60.60 us |  1.14 |    0.04 | 1136.7188 | 1117.1875 | 988.2813 | 19834.11 KB |
|                      |                   |          |             |           |           |       |         |           |           |          |             |
|     AhoCorasickBuild |               180 |       12 |  1,334.6 us |   3.75 us |   3.33 us |  0.61 |    0.04 |  236.3281 |  117.1875 |        - |  1454.24 KB |
|      DoubleTrieBuild |               180 |       12 |  2,128.3 us |  44.75 us | 123.26 us |  1.00 |    0.00 | 1121.0938 | 1050.7813 | 980.4688 | 19711.82 KB |
| AhoCorasickFullBuild |               180 |       12 |  1,599.8 us |  31.08 us |  39.31 us |  0.74 |    0.06 |  275.3906 |  136.7188 |        - |  1685.66 KB |
|  DoubleTrieFullBuild |               180 |       12 |  2,570.2 us |  56.76 us | 161.95 us |  1.21 |    0.10 | 1128.9063 | 1062.5000 | 988.2813 | 19711.89 KB |
|                      |                   |          |             |           |           |       |         |           |           |          |             |
|     AhoCorasickBuild |               180 |       36 | 10,759.0 us | 211.26 us | 375.51 us |  2.36 |    0.10 |  968.7500 |  328.1250 | 109.3750 |  5574.84 KB |
|      DoubleTrieBuild |               180 |       36 |  4,528.9 us |  62.50 us |  58.46 us |  1.00 |    0.00 | 1390.6250 | 1328.1250 | 960.9375 | 21487.46 KB |
| AhoCorasickFullBuild |               180 |       36 | 11,184.6 us | 223.65 us | 239.30 us |  2.47 |    0.06 | 1156.2500 |  484.3750 | 218.7500 |  6467.58 KB |
|  DoubleTrieFullBuild |               180 |       36 |  5,160.5 us |  57.73 us |  54.00 us |  1.14 |    0.02 | 1406.2500 | 1343.7500 | 976.5625 | 21487.25 KB |

### Commit ID _7f2ff0f4_

#### Build
|               Method | WordsInDictionary | WordSize |        Mean |     Error |    StdDev |      Median | Ratio | RatioSD |     Gen 0 |     Gen 1 |    Gen 2 |   Allocated |
|--------------------- |------------------ |--------- |------------:|----------:|----------:|------------:|------:|--------:|----------:|----------:|---------:|------------:|
|     AhoCorasickBuild |                60 |       12 |    227.8 us |   2.91 us |   2.72 us |    226.7 us |  0.18 |    0.00 |   47.8516 |   19.0430 |        - |   244.02 KB |
|      DoubleTrieBuild |                60 |       12 |  1,237.5 us |  20.42 us |  20.97 us |  1,235.3 us |  1.00 |    0.00 | 1052.7344 | 1017.5781 | 990.2344 | 19204.62 KB |
| AhoCorasickFullBuild |                60 |       12 |    302.1 us |   1.45 us |   1.36 us |    302.0 us |  0.24 |    0.00 |   57.6172 |   28.8086 |        - |   323.81 KB |
|  DoubleTrieFullBuild |                60 |       12 |  1,348.3 us |  54.05 us | 159.36 us |  1,396.5 us |  1.11 |    0.12 | 1042.9688 | 1007.8125 | 980.4688 | 19204.13 KB |
|                      |                   |          |             |           |           |             |       |         |           |           |          |             |
|     AhoCorasickBuild |                60 |       36 |  1,188.5 us |   2.98 us |   2.65 us |  1,188.4 us |  0.57 |    0.01 |  156.2500 |   78.1250 |        - |   965.04 KB |
|      DoubleTrieBuild |                60 |       36 |  2,077.0 us |  29.64 us |  24.75 us |  2,073.8 us |  1.00 |    0.00 | 1113.2813 | 1078.1250 | 960.9375 | 19834.33 KB |
| AhoCorasickFullBuild |                60 |       36 |  1,437.4 us |  28.43 us |  26.59 us |  1,441.0 us |  0.69 |    0.02 |  208.9844 |  103.5156 |        - |  1279.22 KB |
|  DoubleTrieFullBuild |                60 |       36 |  2,355.2 us |  46.43 us |  88.34 us |  2,390.2 us |  1.14 |    0.05 | 1136.7188 | 1117.1875 | 988.2813 | 19834.13 KB |
|                      |                   |          |             |           |           |             |       |         |           |           |          |             |
|     AhoCorasickBuild |               180 |       12 |    861.0 us |   2.84 us |   2.52 us |    860.6 us |  0.37 |    0.03 |  117.1875 |   58.5938 |        - |   716.22 KB |
|      DoubleTrieBuild |               180 |       12 |  2,359.4 us |  91.74 us | 270.51 us |  2,284.9 us |  1.00 |    0.00 | 1121.0938 | 1050.7813 | 980.4688 | 19711.91 KB |
| AhoCorasickFullBuild |               180 |       12 |  1,043.6 us |   5.12 us |   4.79 us |  1,045.2 us |  0.46 |    0.04 |  154.2969 |   76.1719 |        - |   947.65 KB |
|  DoubleTrieFullBuild |               180 |       12 |  2,577.1 us |  72.45 us | 213.63 us |  2,601.0 us |  1.11 |    0.16 | 1078.1250 | 1023.4375 | 941.4063 | 19711.98 KB |
|                      |                   |          |             |           |           |             |       |         |           |           |          |             |
|     AhoCorasickBuild |               180 |       36 |  9,175.0 us | 151.69 us | 134.47 us |  9,143.0 us |  2.02 |    0.03 |  453.1250 |  187.5000 |  62.5000 |  2643.87 KB |
|      DoubleTrieBuild |               180 |       36 |  4,543.7 us |  74.56 us |  69.74 us |  4,529.8 us |  1.00 |    0.00 | 1390.6250 | 1320.3125 | 960.9375 |  21487.8 KB |
| AhoCorasickFullBuild |               180 |       36 | 12,456.2 us | 243.59 us | 250.15 us | 12,363.3 us |  2.75 |    0.08 |  640.6250 |  281.2500 |  93.7500 |  3536.67 KB |
|  DoubleTrieFullBuild |               180 |       36 |  4,868.1 us |  57.69 us |  53.97 us |  4,861.7 us |  1.07 |    0.02 | 1406.2500 | 1343.7500 | 976.5625 | 21487.29 KB |

#### Search
|            Method | WordsInDictionary | TextWordCount | WordSize |     Mean |    Error |   StdDev |   Median | Ratio | RatioSD |
|------------------ |------------------ |-------------- |--------- |---------:|---------:|---------:|---------:|------:|--------:|
| AhoCorasickSearch |                60 |           800 |       12 | 224.1 us |  4.47 us |  9.03 us | 222.2 us |  1.95 |    0.14 |
|   DoubleTrieParse |                60 |           800 |       12 | 114.7 us |  2.29 us |  6.41 us | 113.3 us |  1.00 |    0.00 |
|                   |                   |               |          |          |          |          |          |       |         |
| AhoCorasickSearch |                60 |           800 |       36 | 614.5 us | 13.13 us | 38.72 us | 608.7 us |  2.08 |    0.13 |
|   DoubleTrieParse |                60 |           800 |       36 | 293.6 us |  5.79 us |  8.31 us | 290.1 us |  1.00 |    0.00 |
|                   |                   |               |          |          |          |          |          |       |         |
| AhoCorasickSearch |               120 |           800 |       12 | 220.0 us |  4.36 us | 11.50 us | 217.8 us |  2.02 |    0.09 |
|   DoubleTrieParse |               120 |           800 |       12 | 109.0 us |  2.15 us |  2.39 us | 108.1 us |  1.00 |    0.00 |
|                   |                   |               |          |          |          |          |          |       |         |
| AhoCorasickSearch |               120 |           800 |       36 | 594.5 us | 10.78 us | 17.70 us | 585.2 us |  1.87 |    0.11 |
|   DoubleTrieParse |               120 |           800 |       36 | 316.1 us |  6.26 us | 12.05 us | 311.8 us |  1.00 |    0.00 |


### Commit Id _3e307d5b_

#### Memory
|               Method | WordsInDictionary | WordSize |        Mean |        Error |     StdDev |     Gen 0 |     Gen 1 |    Gen 2 |   Allocated |
|--------------------- |------------------ |--------- |------------:|-------------:|-----------:|----------:|----------:|---------:|------------:|
|       NaiveFullBuild |                60 |       12 |    29.44 us |     0.851 us |   0.047 us |    3.4790 |    0.0610 |        - |    14.41 KB |
| AhoCorasickFullBuild |                60 |       12 |   287.20 us |     6.971 us |   0.382 us |   57.1289 |   27.3438 |        - |   323.81 KB |
|  DoubleTrieFullBuild |                60 |       12 | 1,154.04 us |   378.446 us |  20.744 us | 1048.8281 | 1028.3203 | 985.3516 | 19204.24 KB |
|       NaiveFullBuild |                60 |       24 |    37.80 us |    16.402 us |   0.899 us |    5.0049 |    0.0610 |        - |    20.48 KB |
| AhoCorasickFullBuild |                60 |       24 |   829.01 us |    42.729 us |   2.342 us |  135.7422 |   67.3828 |        - |   830.77 KB |
|  DoubleTrieFullBuild |                60 |       24 | 1,599.68 us | 3,352.518 us | 183.763 us | 1128.9063 | 1123.0469 | 994.1406 | 19523.91 KB |
|       NaiveFullBuild |               240 |       12 |   316.18 us |     1.782 us |   0.098 us |   14.1602 |    0.4883 |        - |    58.34 KB |
| AhoCorasickFullBuild |               240 |       12 | 1,548.91 us |   382.361 us |  20.959 us |  207.0313 |  103.5156 |        - |     1270 KB |
|  DoubleTrieFullBuild |               240 |       12 | 2,905.61 us | 3,721.127 us | 203.968 us | 1234.3750 |  988.2813 | 988.2813 | 19971.75 KB |
|       NaiveFullBuild |               240 |       24 |   320.92 us |     6.176 us |   0.339 us |   19.5313 |         - |        - |    81.59 KB |
| AhoCorasickFullBuild |               240 |       24 | 4,995.45 us |   246.551 us |  13.514 us |  507.8125 |  250.0000 |        - |  3114.55 KB |
|  DoubleTrieFullBuild |               240 |       24 | 4,643.54 us | 1,271.333 us |  69.686 us | 1359.3750 | 1335.9375 | 976.5625 | 21216.93 KB |

## 26 December 2020
---

### Commit ID _dbe81fb7_

#### Build
|               Method | WordsInDictionary | WordSize |        Mean |     Error |     StdDev | Ratio | RatioSD |
|--------------------- |------------------ |--------- |------------:|----------:|-----------:|------:|--------:|
|           NaiveBuild |                60 |       12 |    31.32 us |  0.602 us |   1.160 us |  0.02 |    0.00 |
|     AhoCorasickBuild |                60 |       12 |   331.27 us |  1.861 us |   1.741 us |  0.24 |    0.02 |
|      DoubleTrieBuild |                60 |       12 | 1,298.24 us | 35.287 us | 100.677 us |  1.00 |    0.00 |
| AhoCorasickFullBuild |                60 |       12 |   359.45 us |  1.501 us |   1.172 us |  0.26 |    0.02 |
|  DoubleTrieFullBuild |                60 |       12 | 1,352.83 us | 56.461 us | 166.475 us |  1.05 |    0.17 |

#### Search
|            Method | WordsInDictionary | TextWordCount | WordSize |        Mean |     Error |    StdDev |  Ratio | RatioSD |
|------------------ |------------------ |-------------- |--------- |------------:|----------:|----------:|-------:|--------:|
|       NaiveSearch |                60 |           800 |       12 | 63,733.4 us | 595.27 us | 556.82 us | 585.80 |    5.75 |
| AhoCorasickSearch |                60 |           800 |       12 |    207.1 us |   1.55 us |   1.37 us |   1.90 |    0.02 |
|   DoubleTrieParse |                60 |           800 |       12 |    108.8 us |   0.58 us |   0.49 us |   1.00 |    0.00 |

#### Memory
|               Method | WordsInDictionary | WordSize |        Mean |        Error |     StdDev |     Gen 0 |     Gen 1 |    Gen 2 |   Allocated |
|--------------------- |------------------ |--------- |------------:|-------------:|-----------:|----------:|----------:|---------:|------------:|
|       NaiveFullBuild |                60 |       12 |    30.09 us |     0.844 us |   0.046 us |    3.4790 |         - |        - |    14.41 KB |
| AhoCorasickFullBuild |                60 |       12 |   339.52 us |    14.957 us |   0.820 us |   82.0313 |   38.0859 |        - |   457.53 KB |
|  DoubleTrieFullBuild |                60 |       12 | 1,241.26 us | 1,107.287 us |  60.694 us | 1033.2031 | 1015.6250 | 970.7031 | 19204.24 KB |
|       NaiveFullBuild |                60 |       24 |    38.14 us |     0.652 us |   0.036 us |    5.0049 |    0.0610 |        - |    20.48 KB |
| AhoCorasickFullBuild |                60 |       24 | 1,001.79 us |    10.468 us |   0.574 us |  185.5469 |   91.7969 |        - |  1143.09 KB |
|  DoubleTrieFullBuild |                60 |       24 | 1,696.13 us |    65.352 us |   3.582 us | 1105.4688 | 1091.7969 | 970.7031 | 19523.93 KB |
|       NaiveFullBuild |               240 |       12 |   320.13 us |    10.972 us |   0.601 us |   14.1602 |    0.4883 |        - |    58.34 KB |
| AhoCorasickFullBuild |               240 |       12 | 2,039.35 us |   156.842 us |   8.597 us |  296.8750 |  148.4375 |        - |  1828.44 KB |
|  DoubleTrieFullBuild |               240 |       12 | 2,690.74 us | 6,886.705 us | 377.484 us | 1167.9688 | 1128.9063 | 988.2813 | 19971.74 KB |
|       NaiveFullBuild |               240 |       24 |   333.22 us |    10.669 us |   0.585 us |   19.5313 |    0.4883 |        - |    81.59 KB |
| AhoCorasickFullBuild |               240 |       24 | 7,354.22 us | 1,907.810 us | 104.574 us |  734.3750 |  335.9375 | 117.1875 |  4473.06 KB |
|  DoubleTrieFullBuild |               240 |       24 | 4,666.52 us |   647.087 us |  35.469 us | 1359.3750 | 1328.1250 | 976.5625 | 21216.86 KB |

### Commit ID _f5fb39b7_

#### Memory
|               Method | WordsInDictionary | WordSize |        Mean |        Error |     StdDev |     Gen 0 |     Gen 1 |    Gen 2 |   Allocated |
|--------------------- |------------------ |--------- |------------:|-------------:|-----------:|----------:|----------:|---------:|------------:|
|       NaiveFullBuild |                60 |       12 |    29.25 us |     1.768 us |   0.097 us |    3.4790 |    0.0610 |        - |    14.41 KB |
| AhoCorasickFullBuild |                60 |       12 |   389.48 us |     6.462 us |   0.354 us |   82.5195 |   41.0156 |        - |   478.47 KB |
|  DoubleTrieFullBuild |                60 |       12 | 1,384.10 us |   249.831 us |  13.694 us | 1011.7188 |  986.3281 | 949.2188 | 19204.04 KB |
|       NaiveFullBuild |                60 |       24 |    36.35 us |     2.507 us |   0.137 us |    5.0049 |    0.0610 |        - |    20.48 KB |
| AhoCorasickFullBuild |                60 |       24 | 1,159.92 us |    84.507 us |   4.632 us |  195.3125 |   97.6563 |        - |  1195.78 KB |
|  DoubleTrieFullBuild |                60 |       24 | 1,763.77 us |   318.993 us |  17.485 us | 1130.8594 | 1111.3281 | 994.1406 | 19523.93 KB |
|       NaiveFullBuild |               240 |       12 |   310.25 us |    12.685 us |   0.695 us |   14.1602 |    0.4883 |        - |    58.34 KB |
| AhoCorasickFullBuild |               240 |       12 | 2,300.43 us |   101.034 us |   5.538 us |  312.5000 |  156.2500 |        - |  1911.56 KB |
|  DoubleTrieFullBuild |               240 |       12 | 2,929.03 us | 2,977.755 us | 163.221 us | 1187.5000 | 1089.8438 | 988.2813 | 19971.74 KB |
|       NaiveFullBuild |               240 |       24 |   331.87 us |     4.808 us |   0.264 us |   19.5313 |         - |        - |    81.59 KB |
| AhoCorasickFullBuild |               240 |       24 | __8,686.82__ us |   646.092 us |  35.414 us |  781.2500 |  375.0000 | 156.2500 |  4679.63 KB |
|  DoubleTrieFullBuild |               240 |       24 | 4,582.92 us |   183.081 us |  10.035 us | 1359.3750 | 1328.1250 | 976.5625 | 21216.86 KB |

### Commit ID _5c1d914a_

#### Build
|               Method | WordsInDictionary | WordSize |        Mean |     Error |     StdDev |      Median | Ratio | RatioSD |
|--------------------- |------------------ |--------- |------------:|----------:|-----------:|------------:|------:|--------:|
|           NaiveBuild |                60 |       12 |    30.70 us |  0.240 us |   0.225 us |    30.76 us |  0.03 |    0.00 |
|     AhoCorasickBuild |                60 |       12 |   384.90 us |  4.088 us |   3.823 us |   384.86 us |  0.33 |    0.04 |
|      DoubleTrieBuild |                60 |       12 | 1,217.58 us | 37.314 us | 110.022 us | 1,263.60 us |  1.00 |    0.00 |
| AhoCorasickFullBuild |                60 |       12 |   431.20 us |  1.559 us |   1.302 us |   431.20 us |  0.37 |    0.04 |
|  DoubleTrieFullBuild |                60 |       12 | 1,315.19 us | 71.857 us | 211.871 us | 1,290.04 us |  1.09 |    0.21 |

#### Search
|            Method | WordsInDictionary | TextWordCount | WordSize |        Mean |       Error |      StdDev |  Ratio | RatioSD |
|------------------ |------------------ |-------------- |--------- |------------:|------------:|------------:|-------:|--------:|
|       NaiveSearch |                60 |           800 |       12 | 67,699.1 us | 1,290.94 us | 1,486.64 us | 621.40 |   16.61 |
| AhoCorasickSearch |                60 |           800 |       12 |    213.6 us |     1.77 us |     1.66 us |   1.98 |    0.01 |
|   DoubleTrieParse |                60 |           800 |       12 |    108.2 us |     0.76 us |     0.64 us |   1.00 |    0.00 |

#### Memory
|               Method | WordsInDictionary | WordSize |         Mean |        Error |     StdDev |     Gen 0 |     Gen 1 |    Gen 2 |   Allocated |
|--------------------- |------------------ |--------- |-------------:|-------------:|-----------:|----------:|----------:|---------:|------------:|
|       NaiveFullBuild |                60 |       12 |     29.89 us |     1.141 us |   0.063 us |    3.4790 |         - |        - |    14.41 KB |
| AhoCorasickFullBuild |                60 |       12 |    430.51 us |    23.631 us |   1.295 us |   88.8672 |   44.4336 |        - |   502.34 KB |
|  DoubleTrieFullBuild |                60 |       12 |  1,243.06 us | 1,885.853 us | 103.370 us | 1056.6406 | 1039.0625 | 994.1406 | 19203.83 KB |
|       NaiveFullBuild |                60 |       24 |     40.04 us |     1.158 us |   0.063 us |    5.0049 |    0.0610 |        - |    20.48 KB |
| AhoCorasickFullBuild |                60 |       24 |  1,238.94 us |    36.410 us |   1.996 us |  201.1719 |   99.6094 |        - |  1239.05 KB |
|  DoubleTrieFullBuild |                60 |       24 |  1,731.09 us |   625.243 us |  34.272 us | 1128.9063 | 1111.3281 | 994.1406 | 19523.94 KB |
|       NaiveFullBuild |               240 |       12 |    335.61 us |     2.268 us |   0.124 us |   14.1602 |         - |        - |    58.34 KB |
| AhoCorasickFullBuild |               240 |       12 |  2,410.50 us |   187.362 us |  10.270 us |  328.1250 |  164.0625 |        - |  2023.46 KB |
|  DoubleTrieFullBuild |               240 |       12 |  2,930.87 us | 3,062.232 us | 167.851 us | 1234.3750 |  988.2813 | 988.2813 | 19971.74 KB |
|       NaiveFullBuild |               240 |       24 |    324.75 us |     4.024 us |   0.221 us |   19.5313 |         - |        - |    81.59 KB |
| AhoCorasickFullBuild |               240 |       24 | 11,775.56 us | 2,342.140 us | 128.381 us |  859.3750 |  406.2500 | 187.5000 |  4916.18 KB |
|  DoubleTrieFullBuild |               240 |       24 |  4,467.75 us | 1,210.015 us |  66.325 us | 1359.3750 | 1343.7500 | 976.5625 | 21216.87 KB |


### Commit Id _68723f40_

#### Build

|               Method | WordsInDictionary | WordSize |        Mean |     Error |     StdDev | Ratio | RatioSD |
|--------------------- |------------------ |--------- |------------:|----------:|-----------:|------:|--------:|
|           NaiveBuild |                60 |       12 |    32.43 us |  0.645 us |   1.583 us |  0.02 |    0.00 |
|     AhoCorasickBuild |                60 |       12 |   386.26 us |  3.865 us |   3.615 us |  0.30 |    0.00 |
|      DoubleTrieBuild |                60 |       12 | 1,296.50 us | 24.169 us |  22.608 us |  1.00 |    0.00 |
| AhoCorasickFullBuild |                60 |       12 |   457.86 us |  3.271 us |   2.731 us |  0.35 |    0.01 |
|  DoubleTrieFullBuild |                60 |       12 | 1,527.18 us | 74.528 us | 219.748 us |  1.20 |    0.16 |

#### Search

|            Method | WordsInDictionary | TextWordCount | WordSize |        Mean |     Error |    StdDev |  Ratio | RatioSD |
|------------------ |------------------ |-------------- |--------- |------------:|----------:|----------:|-------:|--------:|
|       NaiveSearch |                60 |           800 |       12 | 66,760.3 us | 785.71 us | 734.96 us | 622.02 |   10.42 |
| AhoCorasickSearch |                60 |           800 |       12 |    225.2 us |   4.49 us |   5.84 us |   2.12 |    0.07 |
|   DoubleTrieParse |                60 |           800 |       12 |    107.5 us |   2.07 us |   2.04 us |   1.00 |    0.00 |

#### Memory
Note: Tests during search showed that all three searches consumed only a couple of hundred Bytes at worst. Hence, I choose not to keep track of search memory consumption.
|               Method | WordsInDictionary | WordSize |         Mean |        Error |     StdDev |     Gen 0 |     Gen 1 |    Gen 2 |   Allocated |
|--------------------- |------------------ |--------- |-------------:|-------------:|-----------:|----------:|----------:|---------:|------------:|
|       NaiveFullBuild |                60 |       12 |     30.36 us |     1.379 us |   0.076 us |    3.5095 |    0.0305 |        - |    14.41 KB |
| AhoCorasickFullBuild |                60 |       12 |    420.16 us |     6.084 us |   0.334 us |   83.0078 |   41.5039 |        - |   498.88 KB |
|  DoubleTrieFullBuild |                60 |       12 |  1,242.27 us | 1,301.522 us |  71.341 us | 1048.8281 | 1024.4141 | 985.3516 | 19204.46 KB |
|       NaiveFullBuild |                60 |       24 |     36.94 us |     1.769 us |   0.097 us |    5.0049 |    0.0610 |        - |    20.48 KB |
| AhoCorasickFullBuild |                60 |       24 |  1,243.30 us |    38.471 us |   2.109 us |  201.1719 |   99.6094 |        - |  1235.59 KB |
|  DoubleTrieFullBuild |                60 |       24 |  1,788.22 us | 1,004.972 us |  55.086 us | 1128.9063 | 1113.2813 | 994.1406 | 19523.94 KB |
|       NaiveFullBuild |               240 |       12 |    317.50 us |    13.361 us |   0.732 us |   14.1602 |    0.9766 |        - |    58.34 KB |
| AhoCorasickFullBuild |               240 |       12 |  2,650.79 us |   201.459 us |  11.043 us |  328.1250 |  164.0625 |        - |  2011.02 KB |
|  DoubleTrieFullBuild |               240 |       12 |  2,662.73 us | 5,896.389 us | 323.201 us | 1160.1563 | 1140.6250 | 988.2813 | 19971.82 KB |
|       NaiveFullBuild |               240 |       24 |    331.99 us |     8.878 us |   0.487 us |   19.5313 |    0.4883 |        - |    81.59 KB |
| AhoCorasickFullBuild |               240 |       24 | 13,187.25 us | 9,227.782 us | 505.806 us |  859.3750 |  390.6250 | 171.8750 |  4902.17 KB |
|  DoubleTrieFullBuild |               240 |       24 |  4,396.02 us |   568.991 us |  31.188 us | 1359.3750 | 1351.5625 | 976.5625 | 21216.89 KB |

