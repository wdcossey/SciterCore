using SciterCore;
using CoreGraphics;

namespace SciterTest.Mac
{
	public class ImgDrawBehavior : SciterEventHandler
	{
		private SciterImage _simg;

		protected override void Attached(
			SciterElement element)
		{
			int width = 400;
			int height = 400;

			byte[] data = new byte[width * height * 4];
			var ctx = new CGBitmapContext(data, width, height, 8, width * 4, CGColorSpace.CreateGenericRgb(), CGImageAlphaInfo.PremultipliedLast);

			ctx.SetFillColor(new CGColor(100, 0, 0));
			ctx.SetStrokeColor(new CGColor(0, 0, 100));
			ctx.SetLineWidth(2);

			//ctx.AddPath(_svg._cgpath);
			//ctx.DrawPath(CGPathDrawingMode.FillStroke);

			_simg = SciterImage.Create(ctx.ToImage());
			//element.SetStyle("width", img.Width + "px");
			//element.SetStyle("height", img.Height + "px");
		}

		protected override bool OnDraw(
			SciterElement element, DrawArgs args)
		{
			if(args.DrawEvent != DrawEvent.Content)
				return false;
			
			using(SciterGraphics graphics = SciterGraphics.Create(args.Handle))
			{
				graphics
					.SaveState()
					.Translate(args.Area.Left, args.Area.Top)
					.SetFillColor(255, 0, 0)
					.SetLineColor(SciterColor.Black)
					.SetLineWidth(1)
					//.DrawPath(_svg._spath, SciterSharp.Interop.SciterXGraphics.DRAW_PATH_MODE.DRAW_FILL_AND_STROKE);
					.Translate(args.Area.Left+10, args.Area.Top+10)
					.BlendImage(_simg, 0, 0)
					.RestoreState();
			}
			return true;
		}
	}
}