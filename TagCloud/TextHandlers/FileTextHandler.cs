using MyStemWrapper;
using TagCloud.Excluders;

namespace TagCloud.TextHandlers;

public class FileTextHandler : ITextHandler
{
    private readonly Stream stream;
    private readonly IWordFilter filter;

    public FileTextHandler(Stream stream, IWordFilter filter)
    {
        this.stream = stream;
        this.filter = filter;
    }

    public IEnumerable<TextData> Handle()
    {
        var wordCounts = new Dictionary<string, int>();
        using var sr = new StreamReader(stream);

        while (!sr.EndOfStream)
        {
            var word = sr.ReadLine()?.ToLower();
            if (word == null)
                continue;
            wordCounts.TryAdd(word, 0);
            wordCounts[word]++;
        }

        var data = wordCounts.Select(pair => new TextData() { Value = pair.Key, Weight = pair.Value });
        data = filter.ExcludeWords(data);

        return data;
    }
}