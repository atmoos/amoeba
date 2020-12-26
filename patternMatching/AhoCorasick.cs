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
            var root = State.Compile(this.trie.Root);
            return new TrieSearch(root, this.matchValues);
        }

        private sealed class TrieSearch : ISearch<TAlphabet, TOnMatch>
        {
            private readonly State root;
            private readonly Dictionary<Node<TAlphabet>, TOnMatch> matchValues = new Dictionary<Node<TAlphabet>, TOnMatch>();
            public TrieSearch(State root, Dictionary<Node<TAlphabet>, TOnMatch> matchValues)
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

        private sealed class State : IEnumerable<Node<TAlphabet>>
        {
            private readonly State output;
            private readonly Node<TAlphabet> self;
            private readonly Dictionary<TAlphabet, State> next;
            public Boolean HasMatch => this.self.MarksEndOfWord || this.output != null;
            private State(Node<TAlphabet> self)
            {
                this.self = self;
                this.output = null;
                this.next = new Dictionary<TAlphabet, State>();
            }
            private State(Node<TAlphabet> self, State output)
            {
                this.self = self;
                this.output = output;
                this.next = new Dictionary<TAlphabet, State>();
            }
            public State Next(in TAlphabet letter) => this.next.TryGetValue(letter, out var child) ? child : null;

            public IEnumerator<Node<TAlphabet>> GetEnumerator()
            {
                var output = this.self.MarksEndOfWord ? this : this.output;
                while(output != null) {
                    yield return output.self;
                    output = output.output;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            public static State Root(Node<TAlphabet> root) => new State(root);
            public static State Compile(in Node<TAlphabet> trie)
            {
                var root = State.Root(trie);
                var breadthFirstQueue = new Queue<State>();
                var nodes = new Map<Node<TAlphabet>, State>(); // do not add root!
                var suffixes = new Map<Node<TAlphabet>, Node<TAlphabet>>();
                foreach(var (letter, child) in trie.Children) {
                    suffixes[child] = trie;
                    breadthFirstQueue.Enqueue(root.next[letter] = nodes[child] = new State(child));
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
                        var newChild = new State(child, nodes[output]);
                        breadthFirstQueue.Enqueue(current.next[letter] = nodes[child] = newChild);
                    }
                    Node<TAlphabet> suffixNode = suffixes[current.self];
                    var suffixState = suffixNode == trie ? root : nodes[suffixNode];
                    foreach(var (letter, state) in suffixState.next) {
                        if(!current.next.ContainsKey(letter)) {
                            current.next[letter] = state;
                        }
                    }
                }
                return root;
            }
        }
    }
}
