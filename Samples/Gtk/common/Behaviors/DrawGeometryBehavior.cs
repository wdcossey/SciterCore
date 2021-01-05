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
		protected override EventGroups SubscriptionsRequest(SciterElement element)
		{
			return EventGroups.HandleDraw;
		}

		protected override bool OnDraw(SciterElement se, DrawArgs args)
		{
			if (args.DrawEvent == DrawEvent.Content)
			{
				using (var graphics = SciterGraphics.Create(args.Handle))
				{
					graphics
						.SaveState()
						.Translate(args.Area.Left, args.Area.Top)
						.SetLineColor(0, 255, 255, .75f)
	                    .SetFillColor(127, 78, 194, .75f)
	                    .SetLineWidth(4)
	                    .DrawPolygon(
		                    PolygonPoint.Create(51.0f, 58.0f),
		                    PolygonPoint.Create(70.0f, 28.0f),
		                    PolygonPoint.Create(48.0f, 1.0f),
		                    PolygonPoint.Create(15.0f, 14.0f),
		                    PolygonPoint.Create(17.0f, 49.0f)
	                    )
						.SetLineWidth(2)
						.SetLineColor(SciterColor.IndianRed)
						.SetFillColor(SciterColor.CornflowerBlue)
						.DrawRectangle(25, 125, 75, 175)
						.SetLineColor(SciterColor.Goldenrod)
						.DrawLine(50,125, 50, 175)
						.DrawLine(25,150, 75, 150)
						
	                    .SetLineWidth(0)
	                    .SetLineColor(127, 78, 194, .75f)
	                    .SetFillColor(0, 255, 255, .75f)
	                    .DrawEllipse(200, 50, 50, 50)
	                    .SetFillColor(127, 78, 194, .75f)
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
