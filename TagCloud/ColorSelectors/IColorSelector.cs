using System.Drawing;
using TagCloudApplication;

namespace TagCloudTests;

public interface IColorSelector
{
    Color SetColor();
    bool IsMatch(Options options);
}