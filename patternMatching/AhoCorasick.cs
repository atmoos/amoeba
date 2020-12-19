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
            // Build the trie along the column;
            var next = this.trieRoot;
            foreach(var letter in pattern) {
                next = next.Add(letter, this.trieRoot);
            }
            next.Pattern = pattern;
        }

        public ITrie<Char, String> Build() => new Trie(Node.Reduce(this.trieRoot));

        private sealed class Trie : ITrie<Char, String>
        {
            private readonly Node root;
            public Trie(Node root) => this.root = root;

            public IEnumerable<String> Search(String input)
            {
                var parent = this.root;
                foreach(Char c in input) {
                    var child = parent.Next(c);
                    if(child != null) {
                        parent = child;
                        if(parent.Pattern != null) {
                            yield return parent.Pattern;
                        }
                        var temp = parent.Output;
                        while(temp?.Pattern != null) {
                            yield return temp.Pattern;
                            temp = temp.Output;
                        }
                        continue;
                    }
                    while(parent != this.root && parent.Next(c) == null) {
                        parent = parent.Suffix;
                    }
                }
            }
        }

        private sealed class Node
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
            public static Node Reduce(Node root)
            {
                var top = root;
                var nodes = new Queue<Node>(root.children.Values);

                while(nodes.TryDequeue(out var current)) {
                    foreach(var child in current.children.Values) {
                        var temp = current.Suffix;
                        while(temp.Next(child.Letter) == null && temp != root) {
                            temp = temp.Suffix;
                        }
                        var suffix = temp.Next(child.Letter);
                        child.Suffix = suffix ?? root;
                        nodes.Enqueue(child);
                    }

                    if(current.Suffix != root) {
                        current.Output = current.Suffix;
                        continue;
                    }
                    current.Output = current.Suffix.Output;
                }
                return root;
            }
        }
    }
}
