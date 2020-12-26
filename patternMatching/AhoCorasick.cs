using System;
using System.Collections.Generic;
using datastructures;

namespace patternMatching
{
    // This implementation follows the following tutorial:
    // https://iq.opengenus.org/aho-corasick-algorithm/
    public sealed class AhoCorasick<TAlphabet, TOnMatch> : ISearchBuilder<TAlphabet, TOnMatch>
    {
        private readonly Trie<TAlphabet> trie = new Trie<TAlphabet>();
        private readonly Dictionary<Trie<TAlphabet>.Node, TOnMatch> matchValues = new Dictionary<Trie<TAlphabet>.Node, TOnMatch>();
        public void Add(IEnumerable<TAlphabet> pattern, in TOnMatch onMatch)
        {
            this.matchValues[this.trie.AddKey(pattern)] = onMatch;
        }
        public ISearch<TAlphabet, TOnMatch> Build()
        {
            var (matches, map) = CompileMatchesMap();
            return new TrieSearch(State.Compile(this.trie, map), matches);
        }
        private (TOnMatch[] matches, Dictionary<Trie<TAlphabet>.Node, Int32> map) CompileMatchesMap()
        {
            var matches = new TOnMatch[this.matchValues.Count];
            var map = new Dictionary<Trie<TAlphabet>.Node, Int32>(matches.Length);

            var index = 0;
            foreach(var (node, match) in this.matchValues) {
                matches[index] = match;
                map[node] = index++;
            }
            return (matches, map);
        }

        private sealed class TrieSearch : ISearch<TAlphabet, TOnMatch>
        {
            private readonly State root;
            private readonly TOnMatch[] matches;
            public TrieSearch(State root, TOnMatch[] matches)
            {
                this.root = root;
                this.matches = matches;
            }

            public IEnumerable<TOnMatch> Search<TText>(TText input)
                where TText : IEnumerable<TAlphabet>
            {
                var next = this.root;
                foreach(TAlphabet letter in input) {
                    if(!(next = next.Next(in letter) ?? this.root).HasMatch) {
                        continue;
                    }
                    foreach(var index in next) {
                        yield return this.matches[index];
                    }
                }
            }
        }
        private sealed class Map<TKey, TValue> where TValue : class
        {
            private readonly Dictionary<TKey, TValue> map = new Dictionary<TKey, TValue>();
            public Map(Int32 size) => this.map = new Dictionary<TKey, TValue>(size);
            public TValue this[in TKey key] {
                get => this.map.TryGetValue(key, out var value) ? value : null;
                set => this.map[key] = value;
            }

        }

        private sealed class State : IEnumerable<Int32>
        {
            private readonly State output;
            private readonly Dictionary<TAlphabet, State> next = new Dictionary<TAlphabet, State>();
            public readonly Int32 matchIndex = -1;
            public Boolean HasMatch => this.matchIndex >= 0 || this.output != null;
            private State() => this.output = null;
            private State(State output)
            {
                this.output = output;
            }
            private State(Int32 matchIndex, State output)
            {
                this.output = output;
                this.matchIndex = matchIndex;
            }
            public State Next(in TAlphabet letter) => this.next.TryGetValue(letter, out var child) ? child : null;

            public IEnumerator<Int32> GetEnumerator()
            {
                var output = this.matchIndex >= 0 ? this : this.output;
                while(output != null) {
                    yield return output.matchIndex;
                    output = output.output;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            public static State Root(Trie<TAlphabet>.Node root) => new State();
            public static State Compile(in Trie<TAlphabet> trie, Dictionary<Trie<TAlphabet>.Node, Int32> indexMap)
            {
                var trieRoot = trie.Root;
                var rootState = State.Root(trieRoot);
                var breadthFirstQueue = new Queue<(State state, Trie<TAlphabet>.Node node)>();
                var nodes = new Map<Trie<TAlphabet>.Node, State>(trie.Size); // do not add root!
                var suffixes = new Map<Trie<TAlphabet>.Node, Trie<TAlphabet>.Node>(trie.Size);
                foreach(var (letter, child) in trieRoot.Children) {
                    suffixes[child] = trieRoot;
                    breadthFirstQueue.Enqueue((rootState.next[letter] = nodes[child] = Create(in indexMap, in child, null), child));
                }
                while(breadthFirstQueue.TryDequeue(out var current)) {
                    foreach(var (letter, child) in current.node.Children) {
                        Trie<TAlphabet>.Node probe;
                        var suffix = suffixes[current.node];
                        while((probe = suffix.Next(in letter)) == null && suffix != trieRoot) {
                            suffix = suffixes[suffix];
                        }
                        var output = suffix = suffixes[child] = probe ?? trieRoot;
                        while(!output.MarksEndOfWord && output != trieRoot) {
                            output = suffixes[output];
                        }
                        var newChild = current.state.next[letter] = nodes[child] = Create(indexMap, child, nodes[output]);
                        breadthFirstQueue.Enqueue((newChild, child));
                    }
                    Trie<TAlphabet>.Node suffixNode = suffixes[current.node];
                    var suffixState = suffixNode == trieRoot ? rootState : nodes[suffixNode];
                    foreach(var (letter, state) in suffixState.next) {
                        if(!current.state.next.ContainsKey(letter)) {
                            current.state.next[letter] = state;
                        }
                    }
                }
                return rootState;
            }

            private static State Create(in Dictionary<Trie<TAlphabet>.Node, Int32> indexMap, in Trie<TAlphabet>.Node node, in State output)
            {
                return node.MarksEndOfWord ? new State(indexMap[node], output) : new State(output);
            }
        }
    }
}
