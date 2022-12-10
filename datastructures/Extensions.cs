namespace Data.Structures;

public static class Extensions
{

    public static void Add<TCharacter>(this ITrieBuilder<TCharacter> trie, IEnumerable<TCharacter> value) => trie.AddKey(value);
    public static void Add<TCharacter, TValue>(this ITrieBuilder<TCharacter> trie, IEnumerable<TValue> values)
        where TValue : IEnumerable<TCharacter>
    {
        foreach (var value in values) {
            trie.Add(value);
        }
    }
    public static void Add(this ITrieBuilder<Char, String> trie, String value)
    {
        trie.Add(value, value);
    }
    public static void Add(this ITrieBuilder<Char, String> trie, IEnumerable<String> values)
    {
        foreach (var value in values) {
            trie.Add(value, value);
        }
    }
}
