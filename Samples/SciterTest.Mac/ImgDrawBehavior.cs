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
			SciterElement element, 
			SciterCore.Interop.SciterBehaviors.DRAW_PARAMS prms)
		{
			if(prms.cmd != SciterCore.Interop.SciterBehaviors.DRAW_EVENTS.DRAW_CONTENT)
				return false;
			
			using(SciterGraphics graphics = SciterGraphics.Create(prms.gfx))
			{
				graphics
					.SaveState()
					.Translate(prms.area.Left, prms.area.Top)
					.SetFillColor(255, 0, 0)
					.SetLineColor(SciterColor.Black)
					.SetLineWidth(1)
					//.DrawPath(_svg._spath, SciterSharp.Interop.SciterXGraphics.DRAW_PATH_MODE.DRAW_FILL_AND_STROKE);
					.Translate(prms.area.Left+10, prms.area.Top+10)
					.BlendImage(_simg, 0, 0)
					.RestoreState();
			}
			return true;
		}
	}
}