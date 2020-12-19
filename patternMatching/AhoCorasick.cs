using System;
using System.Collections.Generic;

namespace patternMatching
{
    // This implementation follow
    public sealed class AhoCorasick : ITrieBuilder<Char, String>
    {
        private readonly Node trieRoot = Node.Root();

        public void Add(String pattern)
        {
            throw new NotImplementedException();
        }

        public ITrie<Char, String> Build() => new Trie(Node.Reduce(this.trieRoot));

        private sealed class Trie : ITrie<Char, String>
        {
            private readonly Node root;
            public Trie(Node root) => this.root = root;

            public IEnumerable<String> Search(String input)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class Node
        {
            private Node()
            {
            }

            public static Node Root() => new Node();
            public static Node Reduce(Node node) => node;
        }
    }
}
