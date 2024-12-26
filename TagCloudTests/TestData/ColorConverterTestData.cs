using System.Drawing;

namespace TagCloudTests.TestData;

public class ColorConverterTestData
{
    public static IEnumerable<TestCaseData> RightCases
    {
        get
        {
            yield return new TestCaseData("#FFFFFF").Returns(Color.FromArgb(255, 255, 255));
            yield return new TestCaseData("#FF0000").Returns(Color.FromArgb(255, 0, 0));
            yield return new TestCaseData("#808080").Returns(Color.FromArgb(128, 128, 128));
            yield return new TestCaseData("#123456").Returns(Color.FromArgb(18, 52, 86));
        }
    }
    
}