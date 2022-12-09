using System;
using System.Collections.Generic;

namespace datastructures;

public class Trie<TCharacter> : ITrie<TCharacter>
    where TCharacter : notnull
{
    private readonly ArrayNode root;
    public Node Root => this.root;
    public required Int32 Size { get; init; }
    public required Int32 Count { get; init; }
    private Trie(Trie<TCharacter>.ArrayNode root) => this.root = root;
    public Boolean Contains(IEnumerable<TCharacter> key)
    {
        using var characters = key.GetEnumerator();
        return this.root.Contains(characters, EqualityComparer<TCharacter>.Default);
    }

    public IEnumerator<TCharacter[]> GetEnumerator() => this.root.DepthFirst(new List<TCharacter>()).GetEnumerator();

    internal static Trie<TCharacter> Compress(TrieBuilder<TCharacter> trie)
    {
        return new(ArrayNode.Compress(trie.RecursionRoot, trie.Count)) { Count = trie.Count, Size = trie.Size };
    }

    private sealed class ArrayNode : Node
    {
        private readonly TCharacter[] prefix;
        private readonly Dictionary<TCharacter, ArrayNode> children;
        public override Int32 Count => this.children.Count;
        private ArrayNode(Int32 childCount)
        {
            this.prefix = Array.Empty<TCharacter>();
            this.children = new Dictionary<TCharacter, ArrayNode>(childCount);
        }
        private ArrayNode(TCharacter[] prefix, Int32 childCount)
        {
            this.prefix = prefix;
            this.children = new Dictionary<TCharacter, ArrayNode>(childCount);
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
        public override String ToString(String wordMark)
        {
            String siblings = String.Join(",", this.prefix);
            String children = String.Join(";", this.children.Keys);
            return $"({siblings})[{children}]{wordMark}";
        }

        public IEnumerable<TCharacter[]> DepthFirst(List<TCharacter> prefix)
        {
            prefix.AddRange(this.prefix);
            if (this.EndOfWord) {
                yield return prefix.ToArray();
            }
            foreach (var (label, child) in this.children) {
                foreach (var word in child.DepthFirst(new List<TCharacter>(prefix) { label })) {
                    yield return word;
                }
            }
        }

        public override IEnumerator<Node> GetEnumerator() => this.children.Values.GetEnumerator();

        public static ArrayNode Compress(TrieBuilder<TCharacter>.RecursiveNode root, Int32 count)
        {
            var queue = new Queue<ArrayNode>();
            var compressed = new ArrayNode(root.Count);
            queue.Enqueue(compressed);
            var map = new Map<ArrayNode, TrieBuilder<TCharacter>.RecursiveNode>(count) { [compressed] = root };
            while (queue.TryDequeue(out var node)) {
                var original = map[node] ?? throw new InvalidOperationException("Parent not found!");
                foreach (var (label, child) in original.Children) {
                    var (prefix, leaf) = child.Compress();
                    var compacted = node.children[label] = new ArrayNode(prefix, leaf.Count) { EndOfWord = leaf.EndOfWord };
                    map[compacted] = leaf;
                    queue.Enqueue(compacted);
                }
            }
            return compressed;
        }
    }
}
