namespace TagCloud.TextHandlers;

public interface ITextHandler
{
    IEnumerable<TextData> Handle();
}