using TagCloud.TextHandlers;

namespace TagCloudTests.TestData;

public class FileTextHandlerTestData
{
    public static IEnumerable<TestCaseData> Data
    {
        get
        {
            yield return new TestCaseData(
                    "два\nодин\nдва\nа\nтри\nтри\nтри",
                    new List<TextData>
                    {
                        new() { Value = "один", Weight = 1 }, new() { Value = "два", Weight = 2 },
                        new() { Value = "три", Weight = 3 }
                    }
                )
                .SetName("ShouldExcludeConjAndNotExcludeOtherWords");
            yield return new TestCaseData("и а или", new List<TextData>())
                .SetName("ShouldExcludeAllConj");
            yield return new TestCaseData(
                    "КАПС\nКаПС\nКАПс",
                    new List<TextData>
                    {
                        new() { Value = "капс", Weight = 3 }
                    }
                )
                .SetName("ShouldTransformToLowerCase");
        }
    }
}