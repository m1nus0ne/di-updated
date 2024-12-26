using System.Drawing;
using TagCloudTests.TestData;
using ColorConverter = TagCloud.Extensions.ColorConverter;

namespace TagCloudTests;

public class ColorConverterTests
{
    [TestCaseSource(typeof(ColorConverterTestData), nameof(ColorConverterTestData.RightCases))]
    public Color Converter_ShouldConvertStringToColor_WhenStringInRightFormat(string hexString)
    {
        if (ColorConverter.TryConvert(hexString, out var color))
            return color;
        throw new Exception();
    }
}