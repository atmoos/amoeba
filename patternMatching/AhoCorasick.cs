using System;
using System.Collections.Generic;
using System.Diagnostics;
using datastructures;

namespace patternMatching
{
    // This implementation follows the following tutorial:
    // https://iq.opengenus.org/aho-corasick-algorithm/
    public sealed class AhoCorasick<TAlphabet, TOnMatch> : ISearchBuilder<TAlphabet, TOnMatch>
    {
        private readonly Trie<TAlphabet> trie = new Trie<TAlphabet>();
        private readonly Dictionary<Node<TAlphabet>, TOnMatch> matchValues = new Dictionary<Node<TAlphabet>, TOnMatch>();
        public void Add(IEnumerable<TAlphabet> pattern, in TOnMatch onMatch)
        {
            this.matchValues[this.trie.AddKey(pattern)] = onMatch;
        }
        public ISearch<TAlphabet, TOnMatch> Build()
        {
            var root = Node.Compile(this.trie.Root);
            return new TrieSearch(root, this.matchValues);
        }

        private sealed class TrieSearch : ISearch<TAlphabet, TOnMatch>
        {
            private readonly Node root;
            private readonly Dictionary<Node<TAlphabet>, TOnMatch> matchValues = new Dictionary<Node<TAlphabet>, TOnMatch>();
            public TrieSearch(Node root, Dictionary<Node<TAlphabet>, TOnMatch> matchValues)
            {
                this.root = root;
                this.matchValues = matchValues;
            }

            public IEnumerable<TOnMatch> Search<TText>(TText input)
                where TText : IEnumerable<TAlphabet>
            {
                var next = this.root;
                foreach(TAlphabet letter in input) {
                    if(!(next = next.Next(in letter) ?? this.root).HasMatch) {
                        continue;
                    }
                    foreach(var match in next) {
                        yield return this.matchValues[match];
                    }
                }
            }
        }
        private sealed class Map<TKey, TValue> where TValue : class
        {
            private readonly Dictionary<TKey, TValue> map = new Dictionary<TKey, TValue>();
            public TValue this[in TKey key] {
                get => this.map.TryGetValue(key, out var value) ? value : null;
                set => this.map[key] = value;
            }

        }

        private sealed class Node : IEnumerable<Node<TAlphabet>>
        {
            private readonly Node output;
            private readonly Node suffix;
            private readonly Node<TAlphabet> self;
            private readonly Dictionary<TAlphabet, Node> children;
            public Boolean HasMatch => this.self.MarksEndOfWord || this.output != null;
            private Node(Node<TAlphabet> self)
            {
                this.self = self;
                this.children = new Dictionary<TAlphabet, Node>();
            }
            private Node(Node<TAlphabet> self, Node suffix, Node output)
            {
                this.self = self;
                this.suffix = suffix;
                this.output = output;
                this.children = new Dictionary<TAlphabet, Node>();
            }
            public Node Next(in TAlphabet letter) => this.children.TryGetValue(letter, out var child) ? child : this.suffix?.Next(in letter);

            public IEnumerator<Node<TAlphabet>> GetEnumerator()
            {
                var output = this.self.MarksEndOfWord ? this : this.output;
                while(output != null) {
                    yield return output.self;
                    output = output.output;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            public static Node Root(Node<TAlphabet> root) => new Node(root);
            public static Node Compile(in Node<TAlphabet> trie)
            {
                var root = Node.Root(trie);
                var breadthFirstQueue = new Queue<Node>();
                var nodes = new Map<Node<TAlphabet>, Node>(); // do not add root!
                var suffixes = new Map<Node<TAlphabet>, Node<TAlphabet>>();
                foreach(var (letter, child) in trie.Children) {
                    suffixes[child] = trie;
                    breadthFirstQueue.Enqueue(root.children[letter] = nodes[child] = new Node(child, root, null));
                }
                while(breadthFirstQueue.TryDequeue(out var current)) {
                    foreach(var (letter, child) in current.self.Children) {
                        Node<TAlphabet> probe;
                        var suffix = suffixes[current.self];
                        while((probe = suffix.Next(in letter)) == null && suffix != trie) {
                            suffix = suffixes[suffix];
                        }
                        var output = suffix = suffixes[child] = probe ?? trie;
                        while(!output.MarksEndOfWord && output != trie) {
                            output = suffixes[output];
                        }
                        var newChild = new Node(child, nodes[suffix] ?? root, nodes[output]);
                        breadthFirstQueue.Enqueue(current.children[letter] = nodes[child] = newChild);
                    }
                }
                return root;
            }
        }
    }
}
