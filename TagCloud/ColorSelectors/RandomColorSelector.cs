using System.Drawing;

namespace TagCloud.ColorSelectors;

public class RandomColorSelector
{
    private readonly Random random = new(DateTime.Now.Microsecond);

    public Color SetColor()
    {
        var color = random.Next(0, 255);
        return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
    }
    
}