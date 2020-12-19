using System;
using System.Collections;
using System.Collections.Generic;

namespace patternMatching
{
    // This implementation follow
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
            private readonly IDictionary<Char, Node> children = new Dictionary<Char, Node>();

            // Red
            public Node Suffix { get; set; }

            // Blue
            public Node Output { get; set; }

            public Char Letter { get; }
            public String Pattern { get; set; }
            private Node()
            {
                Suffix = Output = this; // mark as root
            }

            private Node(Char letter, Node root)
            {
                Letter = letter;
                Suffix = root;
            }

            // Why not return the longest suffix here?
            public Node Next(Char letter) => this.children.TryGetValue(letter, out var match) ? match : null;

            public Node Add(Char letter, Node root)
            {
                if(this.children.TryGetValue(letter, out var child)) {
                    return child;
                }

                return this.children[letter] = new Node(letter, root);
            }

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
                        child.Suffix = temp.Next(child.Letter) ?? root;
                        nodes.Enqueue(child);
                    }
                    current.Output = current.Suffix == root ? current.Suffix.Output : current.Suffix;
                }
                return root;
            }

            public IEnumerator<string> GetEnumerator()
            {
                if(Pattern != null) {
                    yield return Pattern;
                }
                var temp = Output;
                while(temp?.Pattern != null) {
                    yield return temp.Pattern;
                    temp = temp.Output;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
