using SciterCore;
using SkiaSharp;

namespace SciterTest.Graphics.Extensions
{
    public static class ImageExtensions
    {
        public static SciterImage ToSciterImage(this SKBitmap bitmap)
        {
            return new SciterImage(bitmap.GetPixels(), (uint)bitmap.Width, (uint)bitmap.Height, true);
        }
    }
}
