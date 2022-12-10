using BenchmarkDotNet.Attributes;

namespace Data.Structures.Benchmark;

[ShortRunJob]
[MemoryDiagnoser]
[IterationCount(7)]
public class TrieMemoryFootprint
{
    private static readonly TrieBuilder<Char> empty = new();

    private String[] words = Array.Empty<String>();
    private TrieBuilder<Char> builder = empty;

    [Params(30, 300, 3000)]
    public Int32 WordCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var words = "englishWords.txt".ReadLines().Randomize(WordCount).ToArray();
        this.builder = new TrieBuilder<Char> { words };
        this.words = words;
    }

    [Benchmark(Baseline = true)]
    public Object RawTrie() => new TrieBuilder<Char>() { this.words };

    [Benchmark]
    public Object CompressedTrie() => this.builder.Build();
}

/* The Raw trie speeds up by a factor of three after approx 30 iterations!
// * Summary *

BenchmarkDotNet=v0.12.1, OS=arch 
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=7.0.100
  [Host]     : .NET Core 7.0.0 (CoreCLR 7.0.22.56001, CoreFX 7.0.22.56001), X64 RyuJIT
  Job-EPOXVX : .NET Core 7.0.0 (CoreCLR 7.0.22.56001, CoreFX 7.0.22.56001), X64 RyuJIT

IterationCount=7  LaunchCount=1  WarmupCount=3  

|         Method | WordCount |        Mean |      Error |    StdDev | Ratio | RatioSD |    Gen 0 |    Gen 1 |   Gen 2 |  Allocated |
|--------------- |---------- |------------:|-----------:|----------:|------:|--------:|---------:|---------:|--------:|-----------:|
|        RawTrie |        30 |    11.84 us |   0.062 us |  0.022 us |  1.00 |    0.00 |   6.1798 |   1.0986 |       - |   27.51 KB |
| CompressedTrie |        30 |    13.11 us |   0.651 us |  0.289 us |  1.10 |    0.03 |   4.8218 |   0.0153 |       - |   19.74 KB |
|                |           |             |            |           |       |         |          |          |         |            |
|        RawTrie |       300 |   258.65 us |   4.039 us |  1.793 us |  1.00 |    0.00 |  37.5977 |  37.1094 |       - |  232.27 KB |
| CompressedTrie |       300 |   169.73 us |  13.064 us |  5.800 us |  0.66 |    0.02 |  28.8086 |  28.5645 |       - |  177.61 KB |
|                |           |             |            |           |       |         |          |          |         |            |
|        RawTrie |      3000 | 6,170.70 us | 187.807 us | 66.974 us |  1.00 |    0.00 | 398.4375 | 226.5625 | 62.5000 | 2093.17 KB |
| CompressedTrie |      3000 | 4,292.10 us | 137.870 us | 61.215 us |  0.70 |    0.01 | 296.8750 | 171.8750 | 46.8750 | 1703.13 KB |
*/
