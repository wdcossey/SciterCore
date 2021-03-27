using SciterCore.Attributes;
using SciterCore.SkiaSharp.Extensions;
using SkiaSharp;

namespace SciterCore.SkiaSharp.Behaviors
{
    [SciterBehavior("draw-bitmap")]
    internal class DrawBitmapBehavior : InfoBitmapBehavior
    {
		protected override bool OnDraw(SciterElement se, DrawArgs args)
		{
            if (args.DrawEvent != DrawEvent.Background) 
                return base.OnDraw(se, args);
            
            //Sciter requires BGRA data, ensure you use the correct SKColorType `Bgra8888`! 
            using (var bitmap = new SKBitmap(width: args.Area.Width, height: args.Area.Height, colorType: SKColorType.Bgra8888, alphaType: SKAlphaType.Premul))
            using (var canvas = new SKCanvas(bitmap))
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;

                canvas.Clear();

                paint.Shader = SKShader.CreateLinearGradient(
                    //new SKPoint(prms.area.Width / 2f, prms.area.Height / 2f),
                    new SKPoint(0, 0),
                    new SKPoint(args.Area.Width, args.Area.Height),
                    //Math.Max(prms.area.Width, prms.area.Height) / 10f,
                    new SKColor[] { SKColor.Parse("#FF75B7FE"), SKColor.Parse("#00000000") },
                    null,
                    SKShaderTileMode.Clamp);

                canvas.DrawRect(new SKRect(0, 0, args.Area.Width, args.Area.Height), paint);

                var img = bitmap.ToSciterImage();
                var gfx = SciterGraphics.Create(args.Handle);
                gfx.BlendImage(img, args.Area.Left, args.Area.Top);
                //return true;
            }

            //// set up drawing tools
            //using (var paint = new SKPaint())
            //{
            //    paint.IsAntialias = true;
            //    paint.Color = new SKColor(127, 78, 194, 50);
            //    paint.StrokeCap = SKStrokeCap.Round;

            //    // create the Xamagon path
            //    using (var path = new SKPath())
            //    {
            //        path.MoveTo(71.4311121f, 56f);
            //        path.CubicTo(68.6763107f, 56.0058575f, 65.9796704f, 57.5737917f, 64.5928855f, 59.965729f);
            //        path.LineTo(43.0238921f, 97.5342563f);
            //        path.CubicTo(41.6587026f, 99.9325978f, 41.6587026f, 103.067402f, 43.0238921f, 105.465744f);
            //        path.LineTo(64.5928855f, 143.034271f);
            //        path.CubicTo(65.9798162f, 145.426228f, 68.6763107f, 146.994582f, 71.4311121f, 147f);
            //        path.LineTo(114.568946f, 147f);
            //        path.CubicTo(117.323748f, 146.994143f, 120.020241f, 145.426228f, 121.407172f, 143.034271f);
            //        path.LineTo(142.976161f, 105.465744f);
            //        path.CubicTo(144.34135f, 103.067402f, 144.341209f, 99.9325978f, 142.976161f, 97.5342563f);
            //        path.LineTo(121.407172f, 59.965729f);
            //        path.CubicTo(120.020241f, 57.5737917f, 117.323748f, 56.0054182f, 114.568946f, 56f);
            //        path.LineTo(71.4311121f, 56f);
            //        path.Close();

            //        // draw the Xamagon path
            //        canvas.DrawPath(path, paint);
            //    }

            //    paint.Color = new SKColor(255, 255, 255);
            //    paint.TextSize = 24f;
            //    canvas.DrawText("Hello from SkiaSharp", 0, 5 * b.Height / 6, paint);
            //}

            //var b = new Image(406, 400);
            //using (var g = System.Drawing.Graphics.FromImage(b))
            //{
            //	LinearGradientBrush linGrBrush = new LinearGradientBrush(
            //		new Point(0, 10),
            //		new Point(200, 10),
            //		Color.FromArgb(255, 255, 0, 0),   // Opaque red
            //		Color.FromArgb(255, 0, 0, 255));  // Opaque blue
            //	g.SmoothingMode = SmoothingMode.AntiAlias;
            //	g.FillEllipse(linGrBrush, 0, 30, 200, 100);
            //}

            return base.OnDraw(se, args);
		}
	}
}