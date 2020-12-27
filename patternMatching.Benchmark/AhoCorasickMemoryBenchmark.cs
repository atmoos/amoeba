using System;
using System.Linq;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using NReco.Text;

namespace patternMatching.Benchmark
{

    [ShortRunJob]
    [MemoryDiagnoser]
    public class AhoCorasickMemoryBenchmark
    {

        [Params(60, 240)]
        public Int32 WordsInDictionary { get; set; }

        [Params(12, 24)]
        public Int32 WordSize { get; set; }
        private DataSource source;
        private KeyValuePair<String, String>[] doubleTrieDict;

        [GlobalSetup]
        public void Setup()
        {
            this.source = new DataSource(WordSize, WordsInDictionary);
            this.doubleTrieDict = this.source.Dictionary.Select(s => new KeyValuePair<String, String>(s, s)).ToArray();
        }

        [Benchmark]

        public ISearch<Char, String> NaiveFullBuild() => new Naive.MultiPatternSearch<Char, String> { this.source.Dictionary }.Build();

        [Benchmark]
        public ISearch<Char, String> AhoCorasickFullBuild() => new AhoCorasick<Char, String> { this.source.Dictionary }.Build();

        [Benchmark]
        public AhoCorasickDoubleArrayTrie<String> DoubleTrieFullBuild() => new AhoCorasickDoubleArrayTrie<String>(this.doubleTrieDict);
    }
}
