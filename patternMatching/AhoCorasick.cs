using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace patternMatching
{
    // This implementation follows the following tutorial:
    // https://iq.opengenus.org/aho-corasick-algorithm/
    public sealed class AhoCorasick : ITrieBuilder<Char, String>
    {
        private readonly Node trieRoot = Node.Root();

        public void Add(String pattern)
        {
            var next = this.trieRoot;
            foreach(var letter in pattern) {
                next = next.Add(letter, this.trieRoot);
            }
            next.Pattern = pattern;
        }

        public ITrie<Char, String> Build() => new Trie(Node.Compile(this.trieRoot));

        private sealed class Trie : ITrie<Char, String>
        {
            private readonly Node root;
            public Trie(Node root) => this.root = root;

            public IEnumerable<String> Search(String input)
            {
                var next = this.root;
                foreach(Char c in input) {
                    Node child = null;
                    while((child = next.Next(c)) == null && next != this.root) {
                        next = next.Suffix;
                    }

                    next = child ?? this.root;
                    foreach(var pattern in next) {
                        yield return pattern;
                    }
                }
            }
        }

        private sealed class Node : IEnumerable<String>
        {
            private Node output;
            private readonly IDictionary<Char, Node> children = new Dictionary<Char, Node>();
            public Char Letter { get; }
            public String Pattern { get; set; }
            public Node Suffix { get; private set; }
            private Node() { }

            private Node(Char letter, Node root)
            {
                Letter = letter;
                Suffix = root;
            }

            public Node Next(Char letter) => this.children.TryGetValue(letter, out var match) ? match : null;

            public Node Add(Char letter, Node root)
            {
                if(this.children.TryGetValue(letter, out var child)) {
                    return child;
                }
                return this.children[letter] = new Node(letter, root);
            }

            public void SetSuffix(Node suffix, Node root)
            {
                this.Suffix = suffix ?? root;
                if(suffix != null && suffix != root) {
                    this.output = suffix;
                }
            }

            public IEnumerator<string> GetEnumerator()
            {
                if(Pattern != null) {
                    yield return Pattern;
                }
                var temp = this.output;
                while(temp?.Pattern != null) {
                    yield return temp.Pattern;
                    temp = temp.output;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public static Node Root() => new Node();
            public static Node Compile(Node root)
            {
                var nodes = new Queue<Node>(root.children.Values);

                while(nodes.TryDequeue(out var current)) {
                    foreach(var child in current.children.Values) {
                        var temp = current.Suffix;
                        while(temp != root && temp.Next(child.Letter) == null) {
                            temp = temp.Suffix;
                        }
                        child.SetSuffix(temp.Next(child.Letter), root);
                        nodes.Enqueue(child);
                    }
                }
                return root;
            }
        }
    }
}
