using TagCloud.TextHandlers;
using TagCloudTests;

namespace TagCloud;

public class TagCloudGenerator
{
    private readonly ITextHandler handler;
    private readonly CloudLayouter layouter;
    private readonly ICloudDrawer drawer;
    private readonly TextMeasurer measurer;

    public TagCloudGenerator(ITextHandler handler, CloudLayouter layouter, ICloudDrawer drawer, TextMeasurer measurer)
    {
        this.handler = handler;
        this.layouter = layouter;
        this.drawer = drawer;
        this.measurer = measurer;
    }

    public void Generate()
    {
        var rectangles = new List<TextRectangle>();

        foreach (var word in handler.Handle().OrderByDescending(pair => pair.Value))
        {
            var (size, font) = measurer.GetTextRectangleSize(word.Value, word.Weight);
            rectangles.Add(new TextRectangle(
                layouter.PutNextRectangle(size),
                word.Value,
                font
            ));
        }

        drawer.Draw(rectangles);
    }
}