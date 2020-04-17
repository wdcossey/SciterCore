using System;
using System.Collections.Generic;
using SciterCore;
using SciterCore.Interop;
using SciterGraphics = SciterCore.SciterGraphics;

namespace SciterTest.Graphics
{
	class DrawGeometryBehavior : SciterEventHandler
	{
		protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
		{
			if(prms.cmd == SciterBehaviors.DRAW_EVENTS.DRAW_CONTENT)
			{
				using(SciterGraphics g = new SciterGraphics(prms.gfx))
				{
					g.StateSave();
					g.Translate(prms.area.Left, prms.area.Top);

					List<Tuple<float, float>> points = new List<Tuple<float, float>>
					{
						Tuple.Create(51.0f, 58.0f),
						Tuple.Create(70.0f, 28.0f),
						Tuple.Create(48.0f, 1.0f),
						Tuple.Create(15.0f, 14.0f),
						Tuple.Create(17.0f, 49.0f),
					};

                    g.LineColor = new RGBAColor(0, 255, 255, .75d);
					g.FillColor = new RGBAColor(127, 78, 194, .75d);
					g.LineWidth = 4;

					g.Polygon(points);
					g.Ellipse(200, 50, 50, 50);

					g.StateRestore();
				}

				return true;
			}
			return false;
		}
	}
}