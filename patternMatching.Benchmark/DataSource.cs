using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace patternMatching.Benchmark;

internal sealed class DataSource
{
    private readonly Random rand;
    private readonly Int32 wordSize;
    public List<String> Dictionary { get; }
    public DataSource(Int32 wordSize, Int32 dictionarySize)
    {
        this.rand = new Random(42);
        this.wordSize = wordSize;
        Dictionary = Words(this.rand, (Int32)(wordSize / 1.4), dictionarySize).Select(w => new String(w.ToArray())).ToList();
    }
    public IEnumerable<Char> Text(Int32 wordCount) => Text(this.rand, this.wordSize, wordCount, ' ');
    public String TextAsString(Int32 wordCount) => Text(this.rand, this.wordSize, wordCount, ' ').Aggregate(new StringBuilder(), (sb, c) => sb.Append(c)).ToString();
    private IEnumerable<IEnumerable<Char>> Words(Random rand, Int32 wordSize, Int32 wordCount)
    {
        var halfWord = wordSize / 2;
        for (var count = 0; count < wordCount; ++count) {
            var nextSize = rand.Next(halfWord, wordSize + halfWord);
            yield return GenerateText(rand).Take(nextSize);
        }
    }
    private IEnumerable<Char> Text(Random rand, Int32 wordSize, Int32 wordCount, Char wordSep)
    {
        var halfWord = wordSize / 2;
        var generator = GenerateText(rand).GetEnumerator();
        for (var count = 0; count < wordCount; ++count) {
            var nextSize = rand.Next(halfWord, wordSize + halfWord);
            for (Int32 size = 0; size < nextSize; ++size) {
                yield return generator.Current;
                generator.MoveNext();
            }
            yield return wordSep;
        }
    }
    private static IEnumerable<Char> GenerateText(Random rand)
    {
        while (true) {
            foreach (var character in rand.Next().ToString()) {
                yield return character;
            }
        }
    }
}
