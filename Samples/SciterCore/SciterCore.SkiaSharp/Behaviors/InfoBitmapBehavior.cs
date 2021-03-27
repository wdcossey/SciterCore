using SciterCore.Attributes;
using SciterCore.SkiaSharp.Extensions;
using SkiaSharp;

namespace SciterCore.SkiaSharp.Behaviors
{
    [SciterBehavior("draw-alt-attribute")]
    internal class InfoBitmapBehavior : SciterEventHandler
    {
        protected override bool OnDraw(SciterElement se, DrawArgs args)
        {
            if (args.DrawEvent != DrawEvent.Foreground) 
                return false;
            
            //Sciter requires BGRA data, ensure you use the correct SKColorType `Bgra8888`! 
            using (var bitmap = new SKBitmap(width: args.Area.Width, height: args.Area.Height, colorType: SKColorType.Bgra8888, alphaType: SKAlphaType.Premul))
            using (var canvas = new SKCanvas(bitmap))
            using (var paint = new SKPaint())
            {
                canvas.Clear();

                paint.IsAntialias = true;
                paint.TextSize = 14f;

                var hasAltText = se.Attributes.TryGetValue("alt", out var altText);

                if (hasAltText)
                {
                    var textBounds = new SKRect();
                    paint.MeasureText(altText, ref textBounds);

                    paint.Color = new SKColor(0, 0, 0, 127);
                    canvas.DrawRect(bitmap.Width, bitmap.Height, -(textBounds.Width + 10),
                        -(textBounds.Height + 10), paint);

                    paint.Color = new SKColor(255, 255, 255);
                    paint.TextAlign = SKTextAlign.Right;
                    canvas.DrawText(altText, bitmap.Width - 5, bitmap.Height - 5, paint);
                }

                var img = bitmap.ToSciterImage();
                var gfx = SciterGraphics.Create(args.Handle);
                gfx.BlendImage(img, args.Area.Left, args.Area.Top);
            }

            // Resume normal drawing
            return false;
        }
    }
}