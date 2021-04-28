using System;
using SciterCore.Attributes;
using SciterCore.SkiaSharp.Extensions;
using SkiaSharp;

namespace SciterCore.SkiaSharp.Behaviors
{
    [SciterBehavior("draw-radial-background")]
    internal class RadialBitmapBehavior : InfoBitmapBehavior
    {
        
        protected readonly DrawEvent DrawEvent = DrawEvent.Background;

        public RadialBitmapBehavior()
        {

        }

        public RadialBitmapBehavior(DrawEvent drawEvent)
            : this()
        {
            DrawEvent = drawEvent;
        }

        protected override bool OnDraw(SciterElement se, DrawArgs args)
        {
            if (args.DrawEvent != DrawEvent) 
                return base.OnDraw(se, args);
            
            //Sciter requires BGRA data, ensure you use the correct SKColorType `Bgra8888`! 
            using (var bitmap = new SKBitmap(width: args.Area.Width, height: args.Area.Height, colorType: SKColorType.Bgra8888, alphaType: SKAlphaType.Premul))
            using (var canvas = new SKCanvas(bitmap))
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                canvas.Clear();

                paint.Shader = SKShader.CreateRadialGradient(
                    new SKPoint(args.Area.Width / 2f, args.Area.Height / 2f),
                     30f,
                    new SKColor[] { SKColors.Black.WithAlpha(0), SKColors.Green, SKColors.Black.WithAlpha(0), SKColors.Red, SKColors.Black.WithAlpha(0), SKColors.Blue, SKColors.Black.WithAlpha(0) },
                    null,
                    SKShaderTileMode.Mirror);

                canvas.DrawRect(new SKRect(0, 0, args.Area.Width, args.Area.Height), paint);

                var img = bitmap.ToSciterImage();
                var gfx = SciterGraphics.Create(args.Handle);
                gfx.BlendImage(img, args.Area.Left, args.Area.Top);
            }

            return base.OnDraw(se, args);
        }
    }
}