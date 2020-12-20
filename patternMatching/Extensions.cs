using System;
using System.Linq;
using System.Collections.Generic;

namespace patternMatching
{
    public static class Extensions
    {
        public static void Add<TAlphabet, TText>(this ISearchBuilder<TAlphabet, TText> builder, IEnumerable<TText> searchTerms)
            where TText : IEnumerable<TAlphabet>
        {
            builder.Add<TAlphabet, TText, TText>(searchTerms.Select(t => (t, t)));
        }
        public static void Add<TAlphabet, TText, TSearchOutput>(this ISearchBuilder<TAlphabet, TSearchOutput> builder, IEnumerable<(TText, TSearchOutput)> searchTerms)
            where TText : IEnumerable<TAlphabet>
        {
            foreach(var (searchTerm, match) in searchTerms) {
                builder.Add(searchTerm, in match);
            }
        }
        public static IEnumerable<TSearchOutput> Search<TAlphabet, TSearchOutput>(this ISearch<TAlphabet, TSearchOutput> search, IEnumerable<IEnumerable<TAlphabet>> inputStream)
        {
            return search.Search<TAlphabet, IEnumerable<TAlphabet>, TSearchOutput>(inputStream);
        }
        public static IEnumerable<TSearchOutput> Search<TAlphabet, TText, TSearchOutput>(this ISearch<TAlphabet, TSearchOutput> search, IEnumerable<TText> inputStream)
            where TText : IEnumerable<TAlphabet>
        {
            foreach(var input in inputStream) {
                foreach(var match in search.Search(input)) {
                    yield return match;
                }
            }
        }
        public static IEnumerable<(UInt64 position, TSearchOutput match)> IndexSearch<TAlphabet, TText, TSearchOutput>(this ISearch<TAlphabet, TSearchOutput> search, TText inputStream)
            where TText : IEnumerable<TAlphabet>
        {
            UInt64 position = 0;
            IEnumerable<TAlphabet> TrackPosition(TText source)
            {
                foreach(var character in source) {
                    yield return character;
                    position++;
                }
            }

            foreach(var match in search.Search(TrackPosition(inputStream))) {
                yield return (position, match);
            }
        }
    }
}
