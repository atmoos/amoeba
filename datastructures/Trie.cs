using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace datastructures
{
    public interface INode<out TLabel>
    {
        TLabel Label { get; }
        Boolean MarksEndOfWord { get; }
    }
    public interface INode<TLabel, out TNode> : INode<TLabel>
        where TNode : INode<TLabel>
    {
        TNode Next(in TLabel label);
    }
    internal sealed class Node<TLabel> : INode<TLabel, Node<TLabel>>, IEnumerable<Node<TLabel>>
    {
        private readonly Dictionary<TLabel, Node<TLabel>> children;
        public TLabel Label { get; }
        public Boolean MarksEndOfWord { get; } = false;
        private Node() => this.children = new Dictionary<TLabel, Node<TLabel>>();
        private Node(TLabel label, Boolean isEndOfWord) : this() => (Label, MarksEndOfWord) = (label, isEndOfWord);
        private Node(Node<TLabel> node, Boolean isEndOfWord)
            : this()
        {
            Label = node.Label;
            MarksEndOfWord = isEndOfWord;
            this.children = node.children;
        }
        public Node<TLabel> Add(in TLabel label)
        {
            if(this.children.TryGetValue(label, out var child)) {
                return child;
            }
            return this.children[label] = new Node<TLabel>(label, false);
        }
        public Node<TLabel> AddEnd(in TLabel label)
        {
            if(this.children.TryGetValue(label, out var child)) {
                if(child.MarksEndOfWord) {
                    return child;
                }
                return this.children[label] = new Node<TLabel>(child, true);
            }
            return this.children[label] = new Node<TLabel>(label, true);
        }
        public Node<TLabel> Next(in TLabel label) => this.children.TryGetValue(label, out var child) ? child : null;
        public static Node<TLabel> Root() => new Node<TLabel>();

        public IEnumerator<Node<TLabel>> GetEnumerator() => this.children.Values.SelectMany(c => c.children.Values).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public sealed class Trie<TLabel, TValue> : IEnumerable<TValue>
    {
        private readonly Trie<TLabel> lookup = new Trie<TLabel>();

        // This is somewhat pointless, but is useful as proof of concept.
        private readonly Dictionary<Node<TLabel>, TValue> values = new Dictionary<Node<TLabel>, TValue>();
        public void Add(IEnumerable<TLabel> key, TValue value) => this.values[this.lookup.AddKey(key)] = value;
        public void Add<TKey>(IEnumerable<(TKey, TValue)> pairs)
            where TKey : IEnumerable<TLabel>
        {
            foreach(var (key, value) in pairs) {
                Add(key, value);
            }
        }
        public Boolean TryGetValue(IEnumerable<TLabel> key, out TValue value)
        {
            var keyNode = this.lookup.FindKey(key);
            if(keyNode != null && this.values.TryGetValue(keyNode, out value)) {
                return true;
            }
            value = default;
            return false;
        }

        public IEnumerator<TValue> GetEnumerator() => values.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public sealed class Trie<TLabel> : IEnumerable
    {
        private readonly Node<TLabel> root = Node<TLabel>.Root();
        public void Add(IEnumerable<TLabel> key) => AddKey(key);
        public void Add<TKey>(IEnumerable<TKey> keys)
            where TKey : IEnumerable<TLabel>
        {
            foreach(var key in keys) {
                Add(key);
            }
        }
        public Boolean Contains(IEnumerable<TLabel> key) => FindKey(key) != null;
        internal Node<TLabel> AddKey(IEnumerable<TLabel> key)
        {
            (Node<TLabel> node, TLabel label) next = (null, default);
            foreach(var label in key) {
                if(next.node == null) {
                    next = (this.root, label);
                    continue;
                }
                next = (next.node.Add(next.label), label);
            }
            return next.node.AddEnd(next.label);
        }
        internal Node<TLabel> FindKey(IEnumerable<TLabel> key)
        {
            var child = this.root;
            foreach(var label in key) {
                if((child = child.Next(in label)) == null) {
                    return null;
                }
            }
            return child;
        }
        internal IEnumerable<Node<TLabel>> Labels() => this.root;

        IEnumerator IEnumerable.GetEnumerator()
        {
            // Implemented in order to enable the collection initialiser.
            throw new NotImplementedException("This method is not meant to be called.");
        }
    }
}
