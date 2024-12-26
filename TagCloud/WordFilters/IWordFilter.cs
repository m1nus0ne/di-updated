using TagCloud.TextHandlers;

namespace TagCloud.Excluders;

public interface IWordFilter
{
    IEnumerable<TextData> ExcludeWords(IEnumerable<TextData> data);
}