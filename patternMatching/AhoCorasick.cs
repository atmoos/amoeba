﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

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
                    if((next = next.Next(in letter) ?? this.root) == this.root || !next.HasMatch) {
                        continue;
                    }
                    foreach(var match in next) {
                        yield return match;
                    }
                }
            }
        }

        [DebuggerDisplay("n: {letter}")]
        private sealed class Node : IEnumerable<TOnMatch>
        {
            private Node output;
            private Node suffix;
            private TOnMatch match;
            private Boolean hasMatch;
            private readonly TAlphabet letter;
            private readonly Dictionary<TAlphabet, Node> children = new Dictionary<TAlphabet, Node>();
            public Boolean HasMatch => this.hasMatch || output != null;
            public TOnMatch Match {
                set => (this.match, this.hasMatch) = (value, true);
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

            public IEnumerator<TOnMatch> GetEnumerator()
            {
                var output = this.hasMatch ? this : this.output;
                while(output != null) {
                    yield return output.match;
                    output = output.output;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            public static Node Root() => new Node();
            public static Node Compile(in Node root)
            {
                var nodes = new Queue<Node>(root.children.Values);
                while(nodes.TryDequeue(out var current)) {
                    foreach(var (letter, child) in current.children) {
                        Node sProbe;
                        var suffix = current.suffix;
                        while((sProbe = suffix.Probe(in letter)) == null && suffix != root) {
                            suffix = suffix.suffix;
                        }
                        child.suffix = sProbe ?? root;
                        nodes.Enqueue(child);
                    }
                    var output = current.suffix;
                    while(!output.hasMatch && output != root) {
                        output = output.suffix;
                    }
                    if(output != root) {
                        current.output = output;
                    }
                }
                return root;
            }
        }
    }
}
