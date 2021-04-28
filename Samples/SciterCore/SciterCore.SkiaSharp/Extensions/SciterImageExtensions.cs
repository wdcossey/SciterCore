using System;
using SkiaSharp;

namespace SciterCore.SkiaSharp.Extensions
{
    public static class SciterImageExtensions
    {
        public static SciterImage ToSciterImage(this SKBitmap bitmap)
        {
            if (bitmap.ColorType != SKColorType.Bgra8888)
                throw new InvalidOperationException($"Please use `{nameof(SKColorType)}.{nameof(SKColorType.Bgra8888)}`");
                    
            return SciterImage.Create(bitmap.GetPixels(), (uint)bitmap.Width, (uint)bitmap.Height, true);
        }
    }
}