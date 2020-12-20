using System;
using System.Linq;
using System.Collections.Generic;

namespace patternMatching.Naive
{
    public sealed class MultiPatternSearch<TAlphabet, TMatch> : ISearchBuilder<TAlphabet, TMatch>
    {
        private readonly List<(IEnumerable<TAlphabet>, TMatch)> dictionary = new List<(IEnumerable<TAlphabet>, TMatch)>();
        public void Add(IEnumerable<TAlphabet> pattern, TMatch match) => this.dictionary.Add((pattern, match));

        public ISearch<TAlphabet, TMatch> Build() => new NaiveSearch(this.dictionary.Select(Build));

        private static (List<TAlphabet>, TMatch match) Build((IEnumerable<TAlphabet> p, TMatch m) value) => (value.p.ToList(), value.m);

        private sealed class NaiveSearch : ISearch<TAlphabet, TMatch>
        {
            private readonly List<(List<TAlphabet> pattern, TMatch match)> dictionary;

            public NaiveSearch(IEnumerable<(List<TAlphabet>, TMatch)> dictionary)
            {
                this.dictionary = new List<(List<TAlphabet> pattern, TMatch match)>(dictionary);
            }
            public IEnumerable<TMatch> Search(IEnumerable<TAlphabet> input)
            {
                foreach(var (pattern, match) in this.dictionary) {
                    var comparators = new HashSet<List<TAlphabet>>();
                    foreach(var letter in input) {
                        List<TAlphabet> remove = null;
                        foreach(var comparator in comparators) {
                            comparator.Add(letter);
                            if(comparator.Count >= pattern.Count) {
                                if(comparator.SequenceEqual(pattern)) {
                                    yield return match;
                                }
                                remove = comparator;
                            }
                        }
                        comparators.Remove(remove);
                        comparators.Add(new List<TAlphabet> { letter });
                    }
                }
            }
        }
    }
}
