using System.Collections.Generic;
using SciterCore;
using SciterCore.Attributes;
using SciterCore.Interop;
using SciterGraphics = SciterCore.SciterGraphics;

namespace SciterTest.Graphics.Behaviors
{
	[SciterBehavior("draw-geometry")]
	class DrawGeometryBehavior : SciterEventHandler
	{
		protected override bool OnDraw(SciterElement se, DrawArgs args)
		{
			if (args.DrawEvent == DrawEvent.Content)
			{
				using(var graphics = SciterGraphics.Create(args.Handle))
				{
					graphics.SaveState()
						.Translate(args.Area.Left, args.Area.Top)
						.SetLineColor(SciterColor.Create(0, 255, 255, .75f))
						.SetFillColor(SciterColor.Create(127, 78, 194, .75f))
						.SetLineWidth(4)
						.DrawPolygon(() => new List<PolygonPoint>
						{
							PolygonPoint.Create(51.0f, 58.0f),
							PolygonPoint.Create(70.0f, 28.0f),
							PolygonPoint.Create(48.0f, 1.0f),
							PolygonPoint.Create(15.0f, 14.0f),
							PolygonPoint.Create(17.0f, 49.0f),
						})
						.DrawEllipse(200, 50, 50, 50)
						.RestoreState();
				}

				return true;
			}
			return false;
		}
	}
}