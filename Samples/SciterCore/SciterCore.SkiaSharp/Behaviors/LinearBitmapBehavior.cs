using System.Linq;
using SciterCore.Attributes;
using SciterCore.SkiaSharp.Extensions;
using SkiaSharp;

namespace SciterCore.SkiaSharp.Behaviors
{
    [SciterBehavior("draw-linear-background")]
    internal class LinearBitmapBehavior : InfoBitmapBehavior
    {
        protected readonly DrawEvent DrawEvent = DrawEvent.Background;

        public LinearBitmapBehavior()
        {

        }

        public LinearBitmapBehavior(DrawEvent drawEvent)
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

                var colorArray = new SKColor[] {SKColor.Parse("#00FEB775"), SKColor.Parse("#FEB775")};

                paint.Shader = SKShader.CreateLinearGradient(
                    new SKPoint(args.Area.Width / 2f, 0),
                    new SKPoint(args.Area.Width / 2f, args.Area.Height),
                    colorArray,
                    null,
                    SKShaderTileMode.Clamp);

                canvas.DrawRect(new SKRect(0, 0, args.Area.Width, args.Area.Height), paint);

                var img = bitmap.ToSciterImage();
                var gfx = SciterGraphics.Create(args.Handle);
                gfx.BlendImage(img, args.Area.Left, args.Area.Top);
                //return true;
            }

            return base.OnDraw(se, args);
        }
    }
}