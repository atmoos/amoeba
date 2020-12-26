using System;
using System.Linq;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using NReco.Text;

namespace patternMatching.Benchmark
{
    public class AhoCorasickBuildBenchmark
    {

        [Params(60)]
        public Int32 WordsInDictionary { get; set; }

        [Params(12)]
        public Int32 WordSize { get; set; }
        private DataSource source;
        private ISearchBuilder<Char, String> naive;
        private ISearchBuilder<Char, String> aho;
        private AhoCorasickDoubleArrayTrie<String> doubleTrie;
        private KeyValuePair<String, String>[] doubleTrieDict;

        [GlobalSetup]
        public void Setup()
        {
            this.source = new DataSource(WordSize, WordsInDictionary);
            this.aho = new AhoCorasick<Char, String>() { this.source.Dictionary };
            this.naive = new Naive.MultiPatternSearch<Char, String> { this.source.Dictionary };
            this.doubleTrie = new AhoCorasickDoubleArrayTrie<String>();
            this.doubleTrieDict = this.source.Dictionary.Select(s => new KeyValuePair<String, String>(s, s)).ToArray();
        }

        [Benchmark]

        public ISearch<Char, String> NaiveBuild() => this.naive.Build();

        [Benchmark]
        public ISearch<Char, String> AhoCorasickBuild() => this.aho.Build();

        [Benchmark(Baseline = true)]
        public AhoCorasickDoubleArrayTrie<String> DoubleTrieBuild()
        {
            this.doubleTrie.Build(this.doubleTrieDict);
            return this.doubleTrie;
        }

        [Benchmark]
        public ISearch<Char, String> AhoCorasickFullBuild() => new AhoCorasick<Char, String>() { this.source.Dictionary }.Build();

        [Benchmark]
        public AhoCorasickDoubleArrayTrie<String> DoubleTrieFullBuild() => new AhoCorasickDoubleArrayTrie<String>(this.doubleTrieDict);
    }
}
