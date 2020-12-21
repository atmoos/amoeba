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
                next = next.Add(in letter);
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
                    next = next.Next(in letter) ?? this.root;
                    foreach(var pattern in next) {
                        yield return pattern;
                    }
                }
            }
        }

        private sealed class Node : IEnumerable<TOnMatch>
        {
            private Node output;
            private Node suffix;
            private TOnMatch match;
            private Boolean hasMatch;
            private readonly TAlphabet letter;
            private readonly Dictionary<TAlphabet, Node> children = new Dictionary<TAlphabet, Node>();
            public TOnMatch Match {
                set
                {
                    this.hasMatch = true;
                    this.match = value;
                }
            }
            private Node() { }
            private Node(Node suffix, TAlphabet letter) => (this.suffix, this.letter) = (suffix, letter);
            public Node Next(in TAlphabet letter) => this.children.TryGetValue(letter, out var match) ? match : this.suffix?.Next(in letter);
            private Node Probe(in TAlphabet letter) => this.children.TryGetValue(letter, out var match) ? match : null;
            public Node Add(in TAlphabet letter)
            {
                if(this.children.TryGetValue(letter, out var child)) {
                    return child;
                }
                return this.children[letter] = new Node(this, letter);
            }
            public void SetSuffix(Node suffix, in Node root)
            {
                this.suffix = suffix ?? root;
                if(suffix != null && suffix != root) {
                    this.output = suffix;
                }
            }

            public IEnumerator<TOnMatch> GetEnumerator()
            {
                var temp = this.hasMatch ? this : this.output;
                while(temp != null && temp.hasMatch) {
                    yield return temp.match;
                    temp = temp.output;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public static Node Root() => new Node();
            public static Node Compile(in Node root)
            {
                var nodes = new Queue<Node>(root.children.Values);
                while(nodes.TryDequeue(out var current)) {
                    foreach(var (letter, child) in current.children) {
                        Node probe;
                        var suffix = current.suffix;
                        while((probe = suffix.Probe(in letter)) == null && suffix != root) {
                            suffix = suffix.suffix;
                        }
                        child.SetSuffix(probe, in root);
                        nodes.Enqueue(child);
                    }
                }
                return root;
            }
        }
    }
}
