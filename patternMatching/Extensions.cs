using System.Collections.Generic;

namespace patternMatching
{
    public static class Extensions
    {
        public static void Add<TAlphabet, TEval>(this ISearchBuilder<TAlphabet, TEval> builder, IEnumerable<TEval> searchTerms)
            where TEval : IEnumerable<TAlphabet>
        {
            foreach(var searchTerm in searchTerms) {
                builder.Add(searchTerm);
            }
        }
        public static IEnumerable<TEval> Search<TAlphabet, TEval>(this ISearch<TAlphabet, TEval> search, IEnumerable<TEval> inputStream)
            where TEval : IEnumerable<TAlphabet>
        {
            foreach(var input in inputStream) {
                foreach(var match in search.Search(input)) {
                    yield return match;
                }
            }
        }
    }
}
