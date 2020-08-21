using System;
using SciterCore;
using SciterCore.Attributes;
using SciterCore.Interop;
using SciterTest.Graphics.Extensions;
using SkiaSharp;
using SciterGraphics = SciterCore.SciterGraphics;

namespace SciterTest.Graphics.Behaviors
{

    [SciterBehavior("draw-checkered-background")]
    internal class CheckeredBackgroundBitmapBehavior : SciterEventHandler
    {
        protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
        {
            if (prms.cmd == SciterBehaviors.DRAW_EVENTS.DRAW_BACKGROUND)
            {
                /*using (var bitmap = new SKBitmap(width: prms.area.Width, height: prms.area.Height, colorType: SKColorType.Rgba8888, alphaType: SKAlphaType.Premul))
                using (var canvas = new SKCanvas(bitmap: bitmap))
                {
                    
                    var color1 = SKColor.Parse("#0000FF");
                    var color2 = SKColor.Parse("#FF0000");
                    var scale = 10.0f;
                    SKPath path = new SKPath();
                    path.AddRect(new SKRect(0, 0, scale, scale));
                    SKMatrix matrix = SKMatrix.MakeScale(2 * scale, scale);
                    matrix.SkewX = 0;
                    matrix.SkewY = 0;
                    SKPaint paint = new SKPaint();
                    paint.PathEffect = SKPathEffect.Create2DPath(matrix, path);
                    paint.IsAntialias = true;
                    paint.Color = color2;
                    canvas.Clear(color1.WithAlpha(255));
                    SKRect bounds = new SKRect(0, 0, 256, 256);
                    bounds.Offset(scale, scale);
                    canvas.DrawRect(bounds, paint);
                    
                    //canvas.Clear(color: SKColor.Parse("#FF0059").WithAlpha(255));
//
                    var img = bitmap.ToSciterImage();
                    var gfx = new SciterGraphics(prms.gfx);
                    gfx.BlendImage(img, prms.area.Left, prms.area.Top);
                }*/
            }

            // Resume normal drawing
            return false;
        }
    } 
    
    [SciterBehavior("draw-alt-attribute")]
    internal class InfoBitmapBehavior : SciterEventHandler
    {
        protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
        {
            if (prms.cmd == SciterBehaviors.DRAW_EVENTS.DRAW_FOREGROUND)
            {
                using (SKBitmap bitmap = new SKBitmap(width: prms.area.Width, height: prms.area.Height, colorType: SKColorType.Rgba8888, alphaType: SKAlphaType.Premul))
                using (SKCanvas canvas = new SKCanvas(bitmap))
                using (SKPaint paint = new SKPaint())
                {
                    canvas.Clear();

                    paint.IsAntialias = true;
                    paint.TextSize = 14f;

                    var hasAltText = se.Attributes.TryGetValue("alt", out var altText);

                    if (hasAltText)
                    {
                        SKRect textBounds = new SKRect();
                        paint.MeasureText(altText, ref textBounds);

                        paint.Color = new SKColor(0, 0, 0, 127);
                        canvas.DrawRect(bitmap.Width, bitmap.Height, -(textBounds.Width + 10),
                            -(textBounds.Height + 10), paint);

                        paint.Color = new SKColor(255, 255, 255);
                        paint.TextAlign = SKTextAlign.Right;
                        canvas.DrawText(altText, bitmap.Width - 5, bitmap.Height - 5, paint);
                    }

                    var img = bitmap.ToSciterImage();
                    var gfx = new SciterGraphics(prms.gfx);
                    gfx.BlendImage(img, prms.area.Left, prms.area.Top);
                }
            }

            // Resume normal drawing
            return false;
        }
    }

    [SciterBehavior("draw-solid-background")]
    internal class SolidBitmapBehavior : InfoBitmapBehavior
    {
        protected readonly SciterBehaviors.DRAW_EVENTS _drawEvent = SciterBehaviors.DRAW_EVENTS.DRAW_BACKGROUND;

        public SolidBitmapBehavior()
        {

        }

        public SolidBitmapBehavior(SciterBehaviors.DRAW_EVENTS drawEvent)
            : this()
        {
            _drawEvent = drawEvent;
        }

        protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
        {
            if (prms.cmd == _drawEvent)
            {
                using (var bitmap = new SKBitmap(width: prms.area.Width, height: prms.area.Height, colorType: SKColorType.Rgba8888, alphaType: SKAlphaType.Premul))
                using (var canvas = new SKCanvas(bitmap: bitmap))
                {
                    canvas.Clear(color: SKColor.Parse("#445F59").WithAlpha(255));

                    var img = bitmap.ToSciterImage();
                    var gfx = new SciterGraphics(prms.gfx);
                    gfx.BlendImage(img, prms.area.Left, prms.area.Top);
                }
            }

            return base.OnDraw(se, prms);
        }
    }

    [SciterBehavior("draw-solid-foreground")]
    internal class SolidForegroundBitmapBehavior : SolidBitmapBehavior
    {

        public SolidForegroundBitmapBehavior()
            : base(SciterBehaviors.DRAW_EVENTS.DRAW_FOREGROUND)
        {

        }
    }

    [SciterBehavior("draw-bitmap")]
    internal class DrawBitmapBehavior : InfoBitmapBehavior
    {
		protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
		{
            if (prms.cmd == SciterBehaviors.DRAW_EVENTS.DRAW_BACKGROUND)
            {

                using (SKBitmap bitmap = new SKBitmap(width: prms.area.Width, height: prms.area.Height, colorType: SKColorType.Rgba8888, alphaType: SKAlphaType.Premul))
                using (SKCanvas canvas = new SKCanvas(bitmap))
                using (SKPaint paint = new SKPaint())
                {
                    paint.IsAntialias = true;

                    canvas.Clear();

                    paint.Shader = SKShader.CreateLinearGradient(
                                        //new SKPoint(prms.area.Width / 2f, prms.area.Height / 2f),
                                        new SKPoint(0, 0),
                                        new SKPoint(prms.area.Width, prms.area.Height),
                                        //Math.Max(prms.area.Width, prms.area.Height) / 10f,
                                        new SKColor[] { SKColor.Parse("#FF75B7FE"), SKColor.Parse("#00000000") },
                                        null,
                                        SKShaderTileMode.Clamp);

                    canvas.DrawRect(new SKRect(0, 0, prms.area.Width, prms.area.Height), paint);

                    var img = bitmap.ToSciterImage();
                    var gfx = new SciterGraphics(prms.gfx);
                    gfx.BlendImage(img, prms.area.Left, prms.area.Top);
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

                
            }

            return base.OnDraw(se, prms);
		}
	}

    [SciterBehavior("draw-linear-background")]
    internal class LinearBitmapBehavior : InfoBitmapBehavior
    {
        protected readonly SciterBehaviors.DRAW_EVENTS _drawEvent = SciterBehaviors.DRAW_EVENTS.DRAW_BACKGROUND;

        public LinearBitmapBehavior()
        {

        }

        public LinearBitmapBehavior(SciterBehaviors.DRAW_EVENTS drawEvent)
            : this()
        {
            _drawEvent = drawEvent;
        }

        protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
        {
            if (prms.cmd == _drawEvent)
            {
                using (SKBitmap bitmap = new SKBitmap(width: prms.area.Width, height: prms.area.Height, colorType: SKColorType.Rgba8888, alphaType: SKAlphaType.Premul))
                using (SKCanvas canvas = new SKCanvas(bitmap))
                using (SKPaint paint = new SKPaint())
                {
                    paint.IsAntialias = true;

                    canvas.Clear();

                    paint.Shader = SKShader.CreateLinearGradient(
                                        new SKPoint(prms.area.Width / 2f, 0),
                                        new SKPoint(prms.area.Width / 2f, prms.area.Height),
                                        new SKColor[] { SKColor.Parse("#00000000"), SKColor.Parse("#FF75B7FE") },
                                        null,
                                        SKShaderTileMode.Clamp);

                    canvas.DrawRect(new SKRect(0, 0, prms.area.Width, prms.area.Height), paint);

                    var img = bitmap.ToSciterImage();
                    var gfx = new SciterGraphics(prms.gfx);
                    gfx.BlendImage(img, prms.area.Left, prms.area.Top);
                    //return true;
                }
            }

            return base.OnDraw(se, prms);
        }
    }

    [SciterBehavior("draw-linear-foreground")]
    internal class LinearForegroundBitmapBehavior : LinearBitmapBehavior
    {

        public LinearForegroundBitmapBehavior()
            : base(SciterBehaviors.DRAW_EVENTS.DRAW_FOREGROUND)
        {

        }
    }

    [SciterBehavior("draw-radial-background")]
    internal class RadialBitmapBehavior : InfoBitmapBehavior
    {
        protected readonly SciterBehaviors.DRAW_EVENTS _drawEvent = SciterBehaviors.DRAW_EVENTS.DRAW_BACKGROUND;

        public RadialBitmapBehavior()
        {

        }

        public RadialBitmapBehavior(SciterBehaviors.DRAW_EVENTS drawEvent)
            : this()
        {
            _drawEvent = drawEvent;
        }

        protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
        {
            if (prms.cmd == _drawEvent)
            {
                using (SKBitmap bitmap = new SKBitmap(width: prms.area.Width, height: prms.area.Height, colorType: SKColorType.Rgba8888, alphaType: SKAlphaType.Premul))
                using (SKCanvas canvas = new SKCanvas(bitmap))
                using (SKPaint paint = new SKPaint())
                {
                    paint.IsAntialias = true;
                    canvas.Clear();

                    paint.Shader = SKShader.CreateRadialGradient(
                                        new SKPoint(prms.area.Width / 2f, prms.area.Height / 2f),
                                        Math.Max(prms.area.Width, prms.area.Height) / 10f,
                                        new SKColor[] { SKColor.Parse("#77FFFFFF"), SKColor.Parse("#33FFFFFF"), SKColor.Parse("#00000000") },
                                        null,
                                        SKShaderTileMode.Mirror);

                    canvas.DrawRect(new SKRect(0, 0, prms.area.Width, prms.area.Height), paint);

                    var img = bitmap.ToSciterImage();
                    var gfx = new SciterGraphics(prms.gfx);
                    gfx.BlendImage(img, prms.area.Left, prms.area.Top);

                    //return true;
                }
            }

            return base.OnDraw(se, prms);
        }
    }

    [SciterBehavior("draw-radial-foreground")]
    internal class RadialForegroundBitmapBehavior : RadialBitmapBehavior
    {

        public RadialForegroundBitmapBehavior()
            :base(SciterBehaviors.DRAW_EVENTS.DRAW_FOREGROUND)
        {

        }
    }

}