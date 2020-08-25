using System.Collections.Generic;
using SciterCore;
using SciterCore.Attributes;
using SciterCore.Interop;
using SciterGraphics = SciterCore.SciterGraphics;

namespace SciterTest.Gtk.Behaviors
{
	[SciterBehavior("draw-geometry")]
	public class DrawGeometryBehavior : SciterEventHandler
	{

		protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
		{
			if (prms.cmd == SciterBehaviors.DRAW_EVENTS.DRAW_CONTENT)
			{
				using (var graphics = new SciterGraphics(prms.gfx))
				{
					graphics
						.SaveState()
						.Translate(prms.area.Left, prms.area.Top)
						.SetLineColor(0, 255, 255, .75d)
	                    .SetFillColor(127, 78, 194, .75d)
	                    .SetLineWidth(4)
	                    .DrawPolygon(() => new List<PolygonPoint>
	                    {
		                    PolygonPoint.Create(51.0f, 58.0f),
		                    PolygonPoint.Create(70.0f, 28.0f),
		                    PolygonPoint.Create(48.0f, 1.0f),
		                    PolygonPoint.Create(15.0f, 14.0f),
		                    PolygonPoint.Create(17.0f, 49.0f),
	                    })
	                    .SetLineWidth(0)
	                    .SetLineColor(127, 78, 194, .75d)
	                    .SetFillColor(SciterColor.Create(0, 255, 255, .75d))
	                    .DrawEllipse(200, 50, 50, 50)
	                    .SetFillColor(SciterColor.Create(127, 78, 194, .75d))
	                    .DrawEllipse(225, 100, 50, 50)
	                    .SetFillColor(255, 0, 0, 127)
	                    .DrawEllipse(175, 100, 50, 50)
						.RestoreState();
				}

				return true;
			}
			return false;
		}
	}
}
