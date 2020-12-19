using System.Collections.Generic;

namespace patternMatching
{
    public static class Extensions
    {
        public static IEnumerable<TEval> Search<TAlphabet, TEval>(this ITrie<TAlphabet, TEval> trie, IEnumerable<TEval> inputStream)
            where TEval : IEnumerable<TAlphabet>
        {
            foreach(var input in inputStream) {
                foreach(var match in trie.Search(input)) {
                    yield return match;
                }
            }
        }
    }
}
