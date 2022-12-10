namespace patternMatching;

public static class Extensions
{
    public static void Add<TAlphabet, TText>(this ISearchBuilder<TAlphabet, TText> builder, IEnumerable<TText> searchTerms)
        where TText : IEnumerable<TAlphabet>
    {
        builder.Add(searchTerms.Select(t => (t, t)));
    }
    public static void Add<TAlphabet, TText, TSearchOutput>(this ISearchBuilder<TAlphabet, TSearchOutput> builder, IEnumerable<(TText, TSearchOutput)> searchTerms)
        where TText : IEnumerable<TAlphabet>
    {
        foreach (var (searchTerm, match) in searchTerms) {
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
        foreach (var input in inputStream) {
            foreach (var match in search.Search(input)) {
                yield return match;
            }
        }
    }
    public static IEnumerable<(UInt64 position, TSearchOutput match)> SearchForStartIndices<TAlphabet, TText, TSearchOutput>(this ISearch<TAlphabet, TSearchOutput> search, TText inputStream)
        where TText : IEnumerable<TAlphabet>
        where TSearchOutput : ICollection<TAlphabet>
    {
        return search.SearchForEndIndices(inputStream).Shift<TAlphabet, TSearchOutput>();
    }
    public static IEnumerable<(UInt64 position, TSearchOutput match)> SearchForStartIndices<TAlphabet, TText, TSearchOutput>(this ISearch<TAlphabet, TSearchOutput> search, TText inputStream, Func<TSearchOutput, UInt64> length)
        where TText : IEnumerable<TAlphabet>
    {
        return search.SearchForEndIndices(inputStream).Shift(length);
    }
    public static IEnumerable<(UInt64 position, TSearchOutput match)> SearchForEndIndices<TAlphabet, TText, TSearchOutput>(this ISearch<TAlphabet, TSearchOutput> search, TText inputStream)
        where TText : IEnumerable<TAlphabet>
    {
        UInt64 position = 0;
        IEnumerable<TAlphabet> TrackPosition(TText source)
        {
            foreach (var character in source) {
                yield return character;
                position++;
            }
        }

        foreach (var match in search.Search(TrackPosition(inputStream))) {
            yield return (position, match);
        }
    }

    public static IEnumerable<(UInt64 position, TSearchOutput match)> Shift<TSearchOutput>(this IEnumerable<(UInt64 position, TSearchOutput match)> matches, Func<TSearchOutput, UInt64> length)
    {
        return matches.Select(m => (m.position - length(m.match) + 1, m.match));
    }
    public static IEnumerable<(UInt64 position, TSearchOutput match)> Shift<TAlphabet, TSearchOutput>(this IEnumerable<(UInt64 position, TSearchOutput match)> matches)
    where TSearchOutput : ICollection<TAlphabet>
    {
        return matches.Select(m => (m.position - (UInt64)m.match.Count + 1, m.match));
    }
}
