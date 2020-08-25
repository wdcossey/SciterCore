using SciterCore;
using SkiaSharp;

namespace SciterTest.Graphics.Extensions
{
    public static class ImageExtensions
    {
        public static SciterImage ToSciterImage(this SKBitmap bitmap)
        {
            return SciterImage.Create(bitmap.GetPixels(), (uint)bitmap.Width, (uint)bitmap.Height, true);
        }
    }
}
