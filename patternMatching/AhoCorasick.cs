using System;
using System.Collections;
using System.Collections.Generic;
using datastructures;

namespace patternMatching;

// This implementation follows the following tutorial:
// https://iq.opengenus.org/aho-corasick-algorithm/
public sealed class AhoCorasick<TAlphabet, TOnMatch> : ISearchBuilder<TAlphabet, TOnMatch>
    where TAlphabet : notnull
{
    private readonly TrieBuilder<TAlphabet> trie = new();
    private readonly Dictionary<Node<TAlphabet>, TOnMatch> matchValues = new();
    public void Add(IEnumerable<TAlphabet> pattern, in TOnMatch onMatch)
    {
        this.matchValues[this.trie.AddKey(pattern)] = onMatch;
    }
    public ISearch<TAlphabet, TOnMatch> Build()
    {
        var (matches, map) = CompileMatchesMap();
        return new TrieSearch(State.Compile(in this.trie, in map), matches);
    }
    private (TOnMatch[] matches, Dictionary<Node<TAlphabet>, Int32> map) CompileMatchesMap()
    {
        var index = 0;
        var matches = new TOnMatch[this.matchValues.Count];
        var map = new Dictionary<Node<TAlphabet>, Int32>(matches.Length);
        foreach (var (node, match) in this.matchValues) {
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
            foreach (TAlphabet letter in input) {
                if ((state = state.Next(in letter) ?? this.root).HasMatch) {
                    foreach (var index in state) {
                        yield return this.matches[index];
                    }
                }
            }
        }
    }

    private sealed class State : IEnumerable<Int32>
    {
        private readonly State? output;
        private readonly Int32 matchIndex = -1;
        private readonly Dictionary<TAlphabet, State> next;
        public Boolean HasMatch => this.matchIndex >= 0 || this.output != null;
        private State(in Int32 capacity) : this(null, in capacity) { }
        private State(State? output, in Int32 capacity) => (this.output, this.next) = (output, new Dictionary<TAlphabet, State>(capacity));
        private State(in Int32 matchIndex, State? output, Int32 capacity) : this(output, in capacity) => this.matchIndex = matchIndex;
        public State? Next(in TAlphabet letter) => this.next.TryGetValue(letter, out var nextState) ? nextState : null;

        public IEnumerator<Int32> GetEnumerator()
        {
            var output = this.matchIndex >= 0 ? this : this.output;
            while (output != null) {
                yield return output.matchIndex;
                output = output.output;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        private void Merge(State suffix)
        {
            foreach (var (letter, state) in suffix.next) {
                this.next.TryAdd(letter, state);
            }
            this.next.TrimExcess();
        }

        public static State Root(in Int32 capacity) => new(capacity);
        public static State Compile(in TrieBuilder<TAlphabet> trie, in Dictionary<Node<TAlphabet>, Int32> indexMap)
        {
            var trieRoot = trie.Root;
            var rootState = Root(trieRoot.Count);
            var breadthFirstQueue = new Queue<Node<TAlphabet>>();
            var nodes = new Map<Node<TAlphabet>, State>(trie.Size); // do not add root!
            var suffixes = new Map<Node<TAlphabet>, Node<TAlphabet>>(trie.Size);
            foreach (var (letter, child) in trieRoot) {
                suffixes[child] = trieRoot;
                rootState.next[letter] = nodes[child] = Create(in indexMap, in child, null, trieRoot.Count);
                breadthFirstQueue.Enqueue(child);
            }
            while (breadthFirstQueue.TryDequeue(out var node)) {
                var current = nodes[node];
                var currentSuffix = suffixes[node];
                foreach (var (letter, child) in node) {
                    Node<TAlphabet> probe;
                    var suffix = currentSuffix;
                    var state = suffix.Walk();
                    while ((probe = state.Next(in letter)) == null && suffix != trieRoot) {
                        suffix = suffixes[suffix];
                        state = suffix.Walk();
                    }
                    var output = suffix = suffixes[child] = probe ?? trieRoot;
                    while (!output.EndOfWord && output != trieRoot) {
                        output = suffixes[output];
                    }
                    current.next[letter] = nodes[child] = Create(in indexMap, in child, nodes[output], suffix.Count);
                    breadthFirstQueue.Enqueue(child);
                }
                // Flatten the trie such that each state has all next states, given a known letter.
                current.Merge(currentSuffix == trieRoot ? rootState : nodes[currentSuffix]);
            }
            return rootState;
        }
        private static State Create(in Dictionary<Node<TAlphabet>, Int32> indexMap, in Node<TAlphabet> node, in State? output, in Int32 suffixCapacity)
        {
            var initialCapacity = node.Count + (Int32)(0.33 * Math.Sqrt(node.Count * suffixCapacity)); // allow an excess of one third the geometric mean.
            return node.EndOfWord ? new State(indexMap[node], output, initialCapacity) : new State(output, initialCapacity);
        }
    }
}
