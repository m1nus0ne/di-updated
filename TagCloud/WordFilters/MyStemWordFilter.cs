using MyStemWrapper;
using TagCloud.Excluders;
using TagCloud.TextHandlers;

namespace TagCloud.WordFilters;

public class MyStemWordFilter : IWordFilter
{
    private static readonly string[] ForbidenSpeechParts = new[]
    {
        "PR", // предлог
        "PART", // частица
        "CONJ", // союз
        "INTJ" // междометие
    };

    public IEnumerable<TextData> ExcludeWords(IEnumerable<TextData> data)
    {
        var stem = new MyStem();
        stem.Parameters = "-lig";
        foreach (var word in data)
        {
            var analysis = stem.Analysis(word.Value);
            if (string.IsNullOrEmpty(analysis))
                continue;

            analysis = analysis.Substring(1, analysis.Length - 2);
            var analysisResults = analysis.Split(",");
            var partsOfSpeech = analysisResults[0]
                .Split("=|")
                .Select(part => part.Split("=")[1]);

            if (partsOfSpeech.Any(ForbidenSpeechParts.Contains))
                continue;
            yield return word;
        }
    }
}