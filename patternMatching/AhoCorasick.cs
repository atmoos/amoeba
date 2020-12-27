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
            return new TrieSearch(State.Compile(in this.trie, in map), matches);
        }
        private (TOnMatch[] matches, Dictionary<Trie<TAlphabet>.Node, Int32> map) CompileMatchesMap()
        {
            var index = 0;
            var matches = new TOnMatch[this.matchValues.Count];
            var map = new Dictionary<Trie<TAlphabet>.Node, Int32>(matches.Length);
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
            public TrieSearch(State root, TOnMatch[] matches) => (this.root, this.matches) = (root, matches);
            public IEnumerable<TOnMatch> Search<TText>(TText input)
                where TText : IEnumerable<TAlphabet>
            {
                var state = this.root;
                foreach(TAlphabet letter in input) {
                    if(!(state = state.Next(in letter) ?? this.root).HasMatch) {
                        continue;
                    }
                    foreach(var index in state) {
                        yield return this.matches[index];
                    }
                }
            }
        }

        private sealed class State : IEnumerable<Int32>
        {
            private readonly State output;
            private readonly Dictionary<TAlphabet, State> next;
            public readonly Int32 matchIndex = -1;
            public Boolean HasMatch => this.matchIndex >= 0 || this.output != null;
            private State(in Int32 capacity) : this(null, in capacity) { }
            private State(State output, in Int32 capacity)
            {
                this.output = output;
                this.next = new Dictionary<TAlphabet, State>(capacity);
            }
            private State(in Int32 matchIndex, State output, Int32 capacity) : this(output, in capacity) => this.matchIndex = matchIndex;
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
            public static State Root(in Int32 capacity) => new State(capacity);
            public static State Compile(in Trie<TAlphabet> trie, in Dictionary<Trie<TAlphabet>.Node, Int32> indexMap)
            {
                var trieRoot = trie.Root;
                var rootState = State.Root(trieRoot.Count);
                var breadthFirstQueue = new Queue<(State state, Trie<TAlphabet>.Node node)>();
                var nodes = new Map<Trie<TAlphabet>.Node, State>(trie.Size); // do not add root!
                var suffixes = new Map<Trie<TAlphabet>.Node, Trie<TAlphabet>.Node>(trie.Size);
                foreach(var (letter, child) in trieRoot) {
                    suffixes[child] = trieRoot;
                    breadthFirstQueue.Enqueue((rootState.next[letter] = nodes[child] = Create(in indexMap, in child, null, trieRoot.Count), child));
                }
                while(breadthFirstQueue.TryDequeue(out var current)) {
                    var currentSuffix = suffixes[current.node];
                    foreach(var (letter, child) in current.node) {
                        Trie<TAlphabet>.Node probe;
                        var suffix = currentSuffix;
                        while((probe = suffix.Next(in letter)) == null && suffix != trieRoot) {
                            suffix = suffixes[suffix];
                        }
                        var output = suffix = suffixes[child] = probe ?? trieRoot;
                        while(!output.MarksEndOfWord && output != trieRoot) {
                            output = suffixes[output];
                        }
                        var newChild = current.state.next[letter] = nodes[child] = Create(in indexMap, in child, nodes[output], suffix.Count);
                        breadthFirstQueue.Enqueue((newChild, child));
                    }
                    // Flatten the trie such that each state has all next states, given a known letter.
                    var suffixState = currentSuffix == trieRoot ? rootState : nodes[currentSuffix];
                    // ToDo: Create proper merge function on State class ;-)
                    current.state.next.EnsureCapacity(suffixState.next.Count);
                    foreach(var (letter, state) in suffixState.next) {
                        current.state.next.TryAdd(letter, state);
                    }
                }
                return rootState;
            }
            private static State Create(in Dictionary<Trie<TAlphabet>.Node, Int32> indexMap, in Trie<TAlphabet>.Node node, in State output, in Int32 suffixCapacity)
            {
                var initialCapacity = node.Count + suffixCapacity;
                return node.MarksEndOfWord ? new State(indexMap[node], output, initialCapacity) : new State(output, initialCapacity);
            }
        }
    }
}
