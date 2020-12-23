using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace datastructures
{
    internal sealed class Node<TLabel> : IEnumerable<Node<TLabel>>
    {
        private readonly Dictionary<TLabel, Node<TLabel>> children = new Dictionary<TLabel, Node<TLabel>>();
        public TLabel Label { get; }
        private Node() { }
        private Node(TLabel label) => Label = label;
        public Node<TLabel> Add(in TLabel label)
        {
            if(this.children.TryGetValue(label, out var child)) {
                return child;
            }
            return this.children[label] = new Node<TLabel>(label);
        }
        public Node<TLabel> Next(in TLabel label) => this.children.TryGetValue(label, out var child) ? child : null;
        public static Node<TLabel> Root() => new Node<TLabel>();

        public IEnumerator<Node<TLabel>> GetEnumerator() => this.children.Values.SelectMany(c => c.children.Values).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public sealed class Trie<TLabel, TValue> : IEnumerable<TValue>
    {
        private readonly Trie<TLabel> lookup = new Trie<TLabel>();
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
            var next = this.root;
            foreach(var label in key) {
                next = next.Add(in label);
            }
            return next;
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
