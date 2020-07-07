using System;
using System.Collections.Generic;
using SciterCore.Interop;

namespace SciterCore.ILSpy.Behaviors
{
	class IconBehavior : SciterEventHandler
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
						Tuple.Create(100.0f, 0.0f),
						Tuple.Create(150.0f, 150.0f),
						Tuple.Create(50.0f, 150.0f)
					};

					g.LineColor = new RGBAColor(0, 0, 255);
					g.FillColor = new RGBAColor(255, 0, 0);
					g.LineWidth = 5;
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