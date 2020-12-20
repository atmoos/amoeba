using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace patternMatching.Naive
{
    public sealed class MultiPatternSearch : ISearchBuilder<Char, String>
    {
        private readonly List<String> dictionary = new List<String>();
        public void Add(String pattern) => this.dictionary.Add(pattern);

        public ISearch<Char, String> Build() => new NaiveSearch(this.dictionary);

        private sealed class NaiveSearch : ISearch<Char, String>
        {
            private readonly List<String> dictionary;

            public NaiveSearch(List<String> dictionary) => this.dictionary = new List<String>(dictionary.Distinct());

            public IEnumerable<String> Search(String input)
            {
                foreach(var pattern in this.dictionary) {
                    var match = new HashSet<StringBuilder>(pattern.Length);
                    foreach(var letter in input) {
                        StringBuilder remove = null;
                        foreach(var m in match) {
                            m.Append(letter);
                            if(m.Length >= pattern.Length) {
                                if(m.ToString() == pattern) {
                                    yield return pattern;
                                }
                                remove = m;
                            }
                        }
                        match.Remove(remove);
                        match.Add(new StringBuilder(letter));
                    }
                }
            }
        }
    }
}
