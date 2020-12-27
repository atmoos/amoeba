using System;
using System.Linq;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using NReco.Text;

namespace patternMatching.Benchmark
{
    public class AhoCorasickSearchBenchmark
    {

        [Params(60, 120)]
        public Int32 WordsInDictionary { get; set; }

        [Params(800)]
        public Int32 TextWordCount { get; set; }

        [Params(12, 36)]
        public Int32 WordSize { get; set; }

        private String text;
        private DataSource source;
        private ISearch<Char, String> aho;
        private AhoCorasickDoubleArrayTrie<String> doubleTrie;

        [GlobalSetup]
        public void Setup()
        {
            this.source = new DataSource(WordSize, WordsInDictionary);
            this.aho = new AhoCorasick<Char, String>() { this.source.Dictionary }.Build();
            this.doubleTrie = new AhoCorasickDoubleArrayTrie<String>(this.source.Dictionary.Select(s => new KeyValuePair<String, String>(s, s)));
            this.text = source.TextAsString(TextWordCount);
        }

        [Benchmark]
        public List<String> AhoCorasickSearch() => this.aho.Search(this.text).ToList();

        [Benchmark(Baseline = true)]
        public Object DoubleTrieParse() => this.doubleTrie.ParseText(this.text);
    }
}
