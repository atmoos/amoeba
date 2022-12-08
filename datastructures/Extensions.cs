using System;
using System.Collections.Generic;

namespace datastructures;

public static class Extensions
{
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
