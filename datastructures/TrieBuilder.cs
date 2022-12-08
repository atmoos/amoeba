using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace datastructures;

public sealed class TrieBuilder<TCharacter, TValue> : ITrieBuilder<TCharacter, TValue>, ITrie<TCharacter, TValue>
    where TCharacter : notnull
{
    private Int32 size;
    private readonly Node root = Node.Root();
    private readonly Dictionary<Node, TValue> values = new();
    public Node Root => this.root;
    public Int32 Size => this.size;
    public Int32 Count => this.values.Count;
    public TValue this[IEnumerable<TCharacter> key]
    {
        get {
            if (TryGetValue(key, out var value)) {
                return value;
            }
            String keyString = String.Join(String.Empty, key);
            throw new KeyNotFoundException($"Key '{keyString}' not found.");
        }
    }

    public void Add(IEnumerable<TCharacter> key, TValue value) => this.values[AddKey(key)] = value;
    public Boolean Contains(IEnumerable<TCharacter> key) => TryGetValue(key, out _);
    private Node AddKey(IEnumerable<TCharacter> key)
    {
        Node node = this.root;
        foreach (var label in key) {
            node = node.Add(label, ref this.size);
        }
        return node;
    }
    public Boolean TryGetValue(IEnumerable<TCharacter> key, [NotNullWhen(true)] out TValue? value)
    {
        var child = this.root;
        foreach (var label in key) {
            if ((child = child.Next(in label)) == null) {
                value = default;
                return false;
            }
        }
        return this.values.TryGetValue(child, out value!);
    }

    public IEnumerator<(TCharacter[] key, TValue value)> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public ITrie<TCharacter, TValue> Build()
    {
        return this; // ToDo: Add compression!
    }

    public sealed class Node : IEnumerable<KeyValuePair<TCharacter, Node>>
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
