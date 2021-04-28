using SciterCore.Attributes;
using SciterCore.SkiaSharp.Extensions;
using SkiaSharp;

namespace SciterCore.SkiaSharp.Behaviors
{
    [SciterBehavior("draw-solid-background")]
    internal class SolidBitmapBehavior : InfoBitmapBehavior
    {
        protected readonly DrawEvent DrawEvent = DrawEvent.Background;

        public SolidBitmapBehavior()
        {

        }

        public SolidBitmapBehavior(DrawEvent drawEvent)
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
            using (var canvas = new SKCanvas(bitmap: bitmap))
            {
                canvas.Clear(color: SKColor.Parse("#595F44").WithAlpha(255));

                var img = bitmap.ToSciterImage();
                var gfx = SciterGraphics.Create(args.Handle);
                gfx.BlendImage(img, args.Area.Left, args.Area.Top);
            }

            return base.OnDraw(se, args);
        }
    }
}