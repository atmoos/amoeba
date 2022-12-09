using System;
using System.Collections.Generic;

namespace datastructures;

public class Trie<TCharacter> : ITrie<TCharacter>
    where TCharacter : notnull
{
    private readonly Node root;
    public required Int32 Size { get; init; }
    public required Int32 Count { get; init; }
    private Trie(Trie<TCharacter>.Node root) => this.root = root;
    public Boolean Contains(IEnumerable<TCharacter> key)
    {
        using var characters = key.GetEnumerator();
        return this.root.Contains(characters, EqualityComparer<TCharacter>.Default);
    }

    public IEnumerator<TCharacter[]> GetEnumerator() => this.root.DepthFirst(new List<TCharacter>()).GetEnumerator();

    internal static Trie<TCharacter> Compress(TrieBuilder<TCharacter> trie)
    {
        return new(Node.Compress(trie.Root, trie.Count)) { Count = trie.Count, Size = trie.Size };
    }

    private sealed class Node
    {
        private readonly TCharacter[] prefix;
        private readonly Dictionary<TCharacter, Node> children;
        public Boolean MarksEndOfWord { get; } = false;
        private Node(Int32 childCount)
        {
            this.prefix = Array.Empty<TCharacter>();
            this.MarksEndOfWord = false;
            this.children = new Dictionary<TCharacter, Node>(childCount);
        }
        private Node(TCharacter[] prefix, Int32 childCount, Boolean isEndOfWord)
        {
            this.prefix = prefix;
            this.MarksEndOfWord = isEndOfWord;
            this.children = new Dictionary<TCharacter, Node>(childCount);
        }

        internal Boolean Contains(IEnumerator<TCharacter> characters, IEqualityComparer<TCharacter> comparer)
        {
            Boolean move;
            Int32 count = 0;
            while ((move = characters.MoveNext()) && count < this.prefix.Length) {
                if (!comparer.Equals(characters.Current, this.prefix[count++])) {
                    return false;
                }
            }
            if (move && this.children.TryGetValue(characters.Current, out var child)) {
                return child.Contains(characters, comparer);
            }
            // if we can still move but there is no more child to traverse into, the characters
            // are not fully part of the trie: return false.
            // if, however, we can no longer move and have come this far, the characters have all matched :-)
            return !move;
        }

        public IEnumerable<TCharacter[]> DepthFirst(List<TCharacter> prefix)
        {
            prefix.AddRange(this.prefix);
            if (this.MarksEndOfWord) {
                yield return prefix.ToArray();
            }
            foreach (var (label, child) in this.children) {
                foreach (var word in child.DepthFirst(new List<TCharacter>(prefix) { label })) {
                    yield return word;
                }
            }
        }

        public static Node Compress(TrieBuilder<TCharacter>.Node root, Int32 count)
        {
            var queue = new Queue<Node>();
            var compressed = new Node(root.Count);
            queue.Enqueue(compressed);
            var map = new Map<Node, TrieBuilder<TCharacter>.Node>(count) { [compressed] = root };
            while (queue.TryDequeue(out var node)) {
                var original = map[node] ?? throw new InvalidOperationException("Parent not found!");
                foreach (var (label, child) in original) {
                    var (prefix, leaf) = child.Compress();
                    var compacted = node.children[label] = new Node(prefix, leaf.Count, leaf.MarksEndOfWord);
                    map[compacted] = leaf;
                    queue.Enqueue(compacted);
                }
            }
            return compressed;
        }
    }
}
