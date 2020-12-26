using System;
using System.Linq;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using NReco.Text;

namespace patternMatching.Benchmark
{
    public class AhoCorasickComparison
    {

        [Params(20)]
        public Int32 WordsInDictionary { get; set; }

        [Params(400)]
        public Int32 TextWordCount { get; set; }

        [Params(24)]
        public Int32 WordSize { get; set; }

        private String text;
        private ISearch<Char, String> search;
        private AhoCorasickDoubleArrayTrie<String> trie;

        [GlobalSetup]
        public void Setup()
        {
            var source = new DataSource(WordSize, WordsInDictionary);
            this.search = new AhoCorasick<Char, String>() { source.Dictionary }.Build();
            this.trie = new AhoCorasickDoubleArrayTrie<String>();
            this.trie.Build(source.Dictionary.Select(s => new KeyValuePair<String, String>(s, s)));
            this.text = source.TextAsString(TextWordCount);
        }

        [Benchmark(Baseline = true)]
        public List<String> AhoCorasickSearch() => this.search.Search(this.text).ToList();


        [Benchmark]
        public Object DoubleAhoCorasickParse() => this.trie.ParseText(this.text);
    }
}
