# Benchmark results

# Run Environment
```
BenchmarkDotNet=v0.12.0, OS=arch 
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.108
  [Host]     : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
  DefaultJob : .NET Core 3.1.8 (CoreCLR 4.700.20.41105, CoreFX 4.700.20.41903), X64 RyuJIT
```

## 26 December 2020

### Build

|               Method | WordsInDictionary | WordSize |        Mean |     Error |     StdDev | Ratio | RatioSD |
|--------------------- |------------------ |--------- |------------:|----------:|-----------:|------:|--------:|
|           NaiveBuild |                60 |       12 |    32.43 us |  0.645 us |   1.583 us |  0.02 |    0.00 |
|     AhoCorasickBuild |                60 |       12 |   386.26 us |  3.865 us |   3.615 us |  0.30 |    0.00 |
|      DoubleTrieBuild |                60 |       12 | 1,296.50 us | 24.169 us |  22.608 us |  1.00 |    0.00 |
| AhoCorasickFullBuild |                60 |       12 |   457.86 us |  3.271 us |   2.731 us |  0.35 |    0.01 |
|  DoubleTrieFullBuild |                60 |       12 | 1,527.18 us | 74.528 us | 219.748 us |  1.20 |    0.16 |

### Search

|            Method | WordsInDictionary | TextWordCount | WordSize |        Mean |     Error |    StdDev |  Ratio | RatioSD |
|------------------ |------------------ |-------------- |--------- |------------:|----------:|----------:|-------:|--------:|
|       NaiveSearch |                60 |           800 |       12 | 66,760.3 us | 785.71 us | 734.96 us | 622.02 |   10.42 |
| AhoCorasickSearch |                60 |           800 |       12 |    225.2 us |   4.49 us |   5.84 us |   2.12 |    0.07 |
|   DoubleTrieParse |                60 |           800 |       12 |    107.5 us |   2.07 us |   2.04 us |   1.00 |    0.00 |

## Memory
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

