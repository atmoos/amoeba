using System;
using System.Linq;
using System.Collections.Generic;

namespace patternMatching.Naive
{
    public sealed class MultiPatternSearch<TAlphabet, TMatch> : ISearchBuilder<TAlphabet, TMatch>
    {
        private readonly List<(IEnumerable<TAlphabet>, TMatch)> dictionary = new List<(IEnumerable<TAlphabet>, TMatch)>();
        public void Add(IEnumerable<TAlphabet> pattern, in TMatch match) => this.dictionary.Add((pattern, match));

        public ISearch<TAlphabet, TMatch> Build() => new NaiveSearch(GuardForKeyCollisions(this.dictionary.Select(Build)));
        private static (List<TAlphabet>, TMatch match) Build((IEnumerable<TAlphabet> p, TMatch m) value) => (value.p.ToList(), value.m);
        private static IEnumerable<(List<TAlphabet> pattern, TMatch match)> GuardForKeyCollisions(IEnumerable<(List<TAlphabet> pattern, TMatch match)> dictionary)
        {
            var keyCollisionGuard = new List<List<TAlphabet>>();
            foreach(var kv in dictionary) {
                var keyIsUnique = true;
                foreach(var guard in keyCollisionGuard) {
                    if(guard.SequenceEqual(kv.pattern)) {
                        keyIsUnique = false;
                        break;
                    }
                }
                if(keyIsUnique) {
                    yield return kv;
                    keyCollisionGuard.Add(kv.pattern);
                }
            }
        }
        private sealed class NaiveSearch : ISearch<TAlphabet, TMatch>
        {
            private readonly Int32 lengthOfLongestPattern;
            private readonly List<(List<TAlphabet> pattern, TMatch match)> dictionary;

            public NaiveSearch(IEnumerable<(List<TAlphabet> pattern, TMatch match)> dictionary)
            {
                this.dictionary = dictionary.ToList();
                this.lengthOfLongestPattern = this.dictionary.Select(e => e.pattern.Count).Max();
            }
            public IEnumerable<TMatch> Search<TText>(TText input)
                where TText : IEnumerable<TAlphabet>
            {
                var searches = new Queue<List<TAlphabet>>(this.lengthOfLongestPattern);
                searches.Enqueue(new List<TAlphabet>());
                foreach(var letter in input) {
                    foreach(var search in searches) {
                        search.Add(letter);
                        foreach(var (pattern, match) in this.dictionary) {
                            if(pattern.SequenceEqual(search)) {
                                yield return match;
                            }
                        }
                    }
                    while(searches.Count >= this.lengthOfLongestPattern) {
                        searches.Dequeue();
                    }
                    searches.Enqueue(new List<TAlphabet>());
                }
            }
        }
    }
}
