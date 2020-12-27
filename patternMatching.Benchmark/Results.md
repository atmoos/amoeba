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

