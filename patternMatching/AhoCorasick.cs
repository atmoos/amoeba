using System;
using System.Collections;
using System.Collections.Generic;

namespace patternMatching
{
    // This implementation follows the following tutorial:
    // https://iq.opengenus.org/aho-corasick-algorithm/
    public sealed class AhoCorasick<TAlphabet, TOnMatch> : ISearchBuilder<TAlphabet, TOnMatch>
    {
        private readonly Node trieRoot = Node.Root();
        public void Add(IEnumerable<TAlphabet> pattern, in TOnMatch onMatch)
        {
            var next = this.trieRoot;
            foreach(var letter in pattern) {
                next = next.Add(in letter, this.trieRoot);
            }
            next.Match = onMatch;
        }
        public ISearch<TAlphabet, TOnMatch> Build() => new TrieSearch(Node.Compile(in this.trieRoot));

        private sealed class TrieSearch : ISearch<TAlphabet, TOnMatch>
        {
            private readonly Node root;
            public TrieSearch(Node root) => this.root = root;

            public IEnumerable<TOnMatch> Search<TText>(TText input)
                where TText : IEnumerable<TAlphabet>
            {
                var next = this.root;
                foreach(TAlphabet letter in input) {
                    Node child = null;
                    while((child = next.Next(in letter)) == null && next != this.root) {
                        next = next.Suffix;
                    }
                    next = child ?? this.root;
                    foreach(var pattern in next) {
                        yield return pattern;
                    }
                }
            }
        }

        private sealed class Node : IEnumerable<TOnMatch>
        {
            private Node output;
            private TOnMatch match;
            private Boolean hasMatch;
            private readonly Dictionary<TAlphabet, Node> children = new Dictionary<TAlphabet, Node>();
            public TAlphabet Letter { get; }
            public TOnMatch Match {
                set
                {
                    this.hasMatch = true;
                    this.match = value;
                }
            }
            public Node Suffix { get; private set; }
            private Node() { }

            private Node(TAlphabet letter, Node root)
            {
                Letter = letter;
                Suffix = root;
            }
            public Node Next(in TAlphabet letter) => this.children.TryGetValue(letter, out var match) ? match : null;
            public Node Add(in TAlphabet letter, Node root)
            {
                if(this.children.TryGetValue(letter, out var child)) {
                    return child;
                }
                return this.children[letter] = new Node(letter, root);
            }
            public void SetSuffix(Node suffix, in Node root)
            {
                this.Suffix = suffix ?? root;
                if(suffix != null && suffix != root) {
                    this.output = suffix;
                }
            }

            public IEnumerator<TOnMatch> GetEnumerator()
            {
                if(this.hasMatch) {
                    yield return this.match;
                }
                var temp = this.output;
                while(temp != null) {
                    if(temp.hasMatch) {
                        yield return temp.match;
                    }
                    temp = temp.output;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public static Node Root() => new Node();
            public static Node Compile(in Node root)
            {
                var nodes = new Queue<Node>(root.children.Values);

                while(nodes.TryDequeue(out var current)) {
                    foreach(var child in current.children.Values) {
                        var temp = current.Suffix;
                        while(temp != root && temp.Next(child.Letter) == null) {
                            temp = temp.Suffix;
                        }
                        child.SetSuffix(temp.Next(child.Letter), in root);
                        nodes.Enqueue(child);
                    }
                }
                return root;
            }
        }
    }
}
