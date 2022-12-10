namespace Data.Structures.Benchmark;

public static class Extensions
{
    public static IEnumerable<String> ReadLines(this String file)
    {
        String? line;
        using var reader = File.OpenText(Path.Combine("data", file));
        while ((line = reader.ReadLine()) != null) {
            yield return line;
        }
    }
    public static IEnumerable<T> Randomize<T>(this IEnumerable<T> items, Int32 count)
    {
        var rand = new Random(count);
        return items.Take(count).OrderBy(_ => rand.Next());
    }
}
