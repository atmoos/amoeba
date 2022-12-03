using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace datastructures;

public sealed class Trie<TLabel> : IEnumerable
{
    private Int32 size;
    private readonly Node root = Node.Root();
    public Node Root => this.root;
    public Int32 Size => this.size;
    public void Add(IEnumerable<TLabel> key) => AddKey(key);
    public void Add<TKey>(IEnumerable<TKey> keys)
        where TKey : IEnumerable<TLabel>
    {
        foreach (var key in keys) {
            Add(key);
        }
    }
    public Boolean Contains(IEnumerable<TLabel> key) => FindKey(key) != null;
    public Node AddKey(IEnumerable<TLabel> key)
    {
        (Node node, TLabel label) next = (null, default);
        foreach (var label in key) {
            if (next.node == null) {
                next = (this.root, label);
                continue;
            }
            next = (next.node.Add(next.label, ref this.size), label);
        }
        return next.node.AddEnd(next.label, ref this.size);
    }
    internal Node FindKey(IEnumerable<TLabel> key)
    {
        var child = this.root;
        foreach (var label in key) {
            if ((child = child.Next(in label)) == null) {
                return null;
            }
        }
        return child;
    }

    public IEnumerable<IEnumerable<Node>> WalkBreadthFirst()
    {
        foreach (var row in BreadthFirst(this.root).GroupBy(e => e.level)) {
            yield return row.Select(d => d.node);
        }
    }

    private static IEnumerable<(Int32 level, Node node)> BreadthFirst(Node root)
    {
        var queue = new Queue<(Int32 level, Node node)>(root.Select(c => (1, c.Value)));
        while (queue.TryDequeue(out var element)) {
            var level = element.level + 1;
            foreach (var (_, child) in element.node) {
                queue.Enqueue((level, child));
            }
            yield return element;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        // Implemented in order to enable the collection initialiser.
        throw new NotImplementedException("This method is not meant to be called.");
    }
    public sealed class Node : IEnumerable<KeyValuePair<TLabel, Node>>
    {
        private readonly Dictionary<TLabel, Node> children;
        public TLabel Label { get; }
        public Boolean MarksEndOfWord { get; } = false;
        public Int32 Count => this.children.Count;
        private Node() => this.children = new Dictionary<TLabel, Node>();
        private Node(TLabel label, Boolean isEndOfWord) : this() => (Label, MarksEndOfWord) = (label, isEndOfWord);
        private Node(Node node, Boolean isEndOfWord)
            : this()
        {
            Label = node.Label;
            MarksEndOfWord = isEndOfWord;
            this.children = node.children;
        }
        internal Node Add(in TLabel label, ref Int32 counter)
        {
            if (this.children.TryGetValue(label, out var child)) {
                return child;
            }
            counter++;
            return this.children[label] = new Node(label, false);
        }
        internal Node AddEnd(in TLabel label, ref Int32 counter)
        {
            if (this.children.TryGetValue(label, out var child)) {
                if (child.MarksEndOfWord) {
                    return child;
                }
                return this.children[label] = new Node(child, true);
            }
            counter++;
            return this.children[label] = new Node(label, true);
        }
        public Node Next(in TLabel label) => this.children.TryGetValue(label, out var child) ? child : null;

        public override String ToString()
        {
            String mark = String.Empty;
            if (this.MarksEndOfWord) {
                mark = this.children.Count == 0 ? "*" : "|*";
            }
            return $"{String.Join(";", this.children.Keys)}{mark}";
        }
        internal static Node Root() => new();

        public IEnumerator<KeyValuePair<TLabel, Node>> GetEnumerator() => this.children.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
