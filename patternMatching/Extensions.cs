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
                builder.Add(searchTerm, match);
            }
        }
        public static IEnumerable<TSearchOutput> Search<TAlphabet, TSearchOutput>(this ISearch<TAlphabet, TSearchOutput> search, IEnumerable<IEnumerable<TAlphabet>> inputStream)
        {
            foreach(var input in inputStream) {
                foreach(var match in search.Search(input)) {
                    yield return match;
                }
            }
        }
    }
}
