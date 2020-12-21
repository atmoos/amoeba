using System;
using System.Linq;
using System.Collections.Generic;

namespace patternMatching.Naive
{
    public sealed class MultiPatternSearch<TAlphabet, TMatch> : ISearchBuilder<TAlphabet, TMatch>
        where TAlphabet : IEquatable<TAlphabet>
    {
        private readonly List<(IEnumerable<TAlphabet>, TMatch)> dictionary = new List<(IEnumerable<TAlphabet>, TMatch)>();
        public void Add(IEnumerable<TAlphabet> pattern, in TMatch match) => this.dictionary.Add((pattern, match));
        public ISearch<TAlphabet, TMatch> Build() => new NaiveSearch(GuardForKeyCollisions(this.dictionary));
        private static List<(Scan pattern, TMatch match)> GuardForKeyCollisions(IEnumerable<(IEnumerable<TAlphabet> pattern, TMatch match)> dictionary)
        {
            var distinctKeys = new List<(Scan pattern, TMatch match)>();
            foreach(var (pattern, match) in dictionary) {
                var keyIsUnique = true;
                var scan = new Scan(pattern.ToArray());
                foreach(var (guard, _) in distinctKeys) {
                    if(guard.Equal(scan)) {
                        keyIsUnique = false;
                        break;
                    }
                }
                if(keyIsUnique) {
                    distinctKeys.Add((scan, match));
                }
            }
            return distinctKeys;
        }

        private sealed class NaiveSearch : ISearch<TAlphabet, TMatch>
        {
            private readonly Int32 lengthOfLongestPattern;
            private readonly List<(Scan pattern, TMatch match)> dictionary;
            public NaiveSearch(List<(Scan pattern, TMatch match)> dictionary)
            {
                this.dictionary = dictionary;
                this.lengthOfLongestPattern = this.dictionary.Select(e => e.pattern.Count).Max();
            }
            public IEnumerable<TMatch> Search<TText>(TText input)
                where TText : IEnumerable<TAlphabet>
            {
                return Search(input, this.dictionary, this.lengthOfLongestPattern);
            }
            private static IEnumerable<TMatch> Search<TText>(TText input, List<(Scan pattern, TMatch match)> dictionary, Int32 max)
                where TText : IEnumerable<TAlphabet>
            {
                var searches = new Queue<Scan>(max);
                searches.Enqueue(new Scan(max));
                foreach(var letter in input) {
                    foreach(var search in searches) {
                        search.Add(in letter);
                        foreach(var (pattern, match) in dictionary) {
                            if(pattern.Equal(in search)) {
                                yield return match;
                            }
                        }
                    }
                    Scan next = null;
                    if(searches.Count >= max) {
                        next = searches.Dequeue();
                        next.Clear();
                    }
                    searches.Enqueue(next ?? new Scan(max));
                }
            }
        }

        private sealed class Scan
        {
            private Int32 end;
            private readonly TAlphabet[] scan;
            public Int32 Count => this.end;
            public Scan(Int32 length)
            {
                this.end = 0;
                this.scan = new TAlphabet[length];
            }
            public Scan(TAlphabet[] source)
            {
                this.end = source.Length;
                this.scan = source;
            }
            public void Add(in TAlphabet character) => this.scan[this.end++] = character;
            public void Clear() => this.end = 0;
            public Boolean Equal(in Scan other)
            {
                if(this.end != other.end) {
                    return false;
                }
                for(Int32 pos = 0; pos < this.end; ++pos) {
                    if(!this.scan[pos].Equals(other.scan[pos])) {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
