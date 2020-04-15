using SciterCore;
using SciterCore.Interop;
using System.Drawing;
using System.Drawing.Drawing2D;
using SciterGraphics = SciterCore.SciterGraphics;

namespace SciterTest.Graphics
{
	class DrawBitmapBehavior : SciterEventHandler
	{
		protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
		{
			var b = new Bitmap(406, 400);
			using(var g = System.Drawing.Graphics.FromImage(b))
			{
				LinearGradientBrush linGrBrush = new LinearGradientBrush(
					new Point(0, 10),
					new Point(200, 10),
					Color.FromArgb(255, 255, 0, 0),   // Opaque red
					Color.FromArgb(255, 0, 0, 255));  // Opaque blue
				g.SmoothingMode = SmoothingMode.AntiAlias;
				g.FillEllipse(linGrBrush, 0, 30, 200, 100);
			}

			var img = new SciterImage(b);
			var gfx = new SciterGraphics(prms.gfx);
			gfx.BlendImage(img, 0, 0);
			return true;
		}
	}
}