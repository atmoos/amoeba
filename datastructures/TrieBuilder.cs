using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace datastructures;

public sealed class TrieBuilder<TCharacter> : ITrieBuilder<TCharacter>, ITrie<TCharacter>
    where TCharacter : notnull
{
    private Int32 size;
    private readonly Node root = Node.Root();
    internal Node Root => this.root;
    public Int32 Size => this.size;
    public Int32 Count { get; private set; }
    public void Add(IEnumerable<TCharacter> value) => AddKey(value);
    public Boolean Contains(IEnumerable<TCharacter> key)
    {
        var child = this.root;
        foreach (var label in key) {
            if ((child = child.Next(in label)) == null) {
                return false;
            }
        }
        return true;
    }
    private Node AddKey(IEnumerable<TCharacter> key)
    {
        (Node node, TCharacter label)? shift = null;
        foreach (var label in key) {
            if (shift is (Node, TCharacter) prev) {
                var next = prev.node.Add(prev.label, ref this.size);
                shift = (next, label);
                continue;
            }
            shift = (this.root, label);
        }
        if (shift is (Node, TCharacter) end) {
            var prevSize = this.size;
            var tailMark = end.node.AddEnd(end.label, ref this.size);
            if (this.size > prevSize) {
                this.Count++;
            }
            return tailMark;
        }
        return this.root;
    }

    public ITrie<TCharacter> Build()
    {
        return Trie<TCharacter>.Compress(this);
    }

    public IEnumerator<TCharacter[]> GetEnumerator() => this.root.DepthFirst(new List<TCharacter>()).GetEnumerator();

    internal sealed class Node : IEnumerable<KeyValuePair<TCharacter, Node>>
    {
        private readonly Dictionary<TCharacter, Node> children;
        public Boolean MarksEndOfWord { get; } = false;
        public Int32 Count => this.children.Count;
        private Node() => this.children = new Dictionary<TCharacter, Node>();
        private Node(Boolean isEndOfWord) : this() => MarksEndOfWord = isEndOfWord;
        private Node(Node node, Boolean isEndOfWord)
        {
            MarksEndOfWord = isEndOfWord;
            this.children = node.children;
        }
        internal Node Add(in TCharacter label, ref Int32 counter)
        {
            if (this.children.TryGetValue(label, out var child)) {
                return child;
            }
            counter++;
            return this.children[label] = new Node(false);
        }
        internal Node AddEnd(in TCharacter label, ref Int32 counter)
        {
            if (this.children.TryGetValue(label, out var child)) {
                if (child.MarksEndOfWord) {
                    return child;
                }
                return this.children[label] = new Node(child, true);
            }
            counter++;
            return this.children[label] = new Node(true);
        }

        public Node? Next(in TCharacter label) => this.children.TryGetValue(label, out var child) ? child : null;

        public IEnumerable<TCharacter[]> DepthFirst(List<TCharacter> prefix)
        {
            if (this.MarksEndOfWord) {
                yield return prefix.ToArray();
            }
            foreach (var (label, child) in this.children) {
                foreach (var word in child.DepthFirst(new List<TCharacter>(prefix) { label })) {
                    yield return word;
                }
            }
        }
        public (TCharacter[] prefix, Node leaf) Compress()
        {
            if (this.MarksEndOfWord || this.children.Count > 1) {
                return (Array.Empty<TCharacter>(), this);
            }
            var children = this.children;
            var (label, onlyChild) = children.Single();
            var prefix = new List<TCharacter>() { label };
            while (!onlyChild.MarksEndOfWord && (children = onlyChild.children).Count == 1) {
                (label, onlyChild) = children.Single();
                prefix.Add(label);
            }
            return (prefix.ToArray(), onlyChild);
        }

        public override String ToString()
        {
            String mark = String.Empty;
            if (this.MarksEndOfWord) {
                mark = this.children.Count == 0 ? "*" : "|*";
            }
            return $"{String.Join(";", this.children.Keys)}{mark}";
        }
        internal static Node Root() => new();

        public IEnumerator<KeyValuePair<TCharacter, Node>> GetEnumerator() => this.children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
