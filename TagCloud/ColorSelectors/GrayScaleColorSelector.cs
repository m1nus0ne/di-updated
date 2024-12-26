using System.Drawing;

namespace TagCloudTests;

public class GrayScaleColorSelector : IColorSelector
{
    private readonly Random random = new(DateTime.Now.Microsecond);

    public Color SetColor()
    {
        var gray = random.Next(100, 200);
        return Color.FromArgb(gray, gray, gray);
    }
}