using SciterCore;
using SciterCore.Interop;
using SciterGraphics = SciterCore.SciterGraphics;

namespace SciterTest.Graphics
{

	class DrawTextBehavior : SciterEventHandler
	{
		protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
		{
			if(prms.cmd == SciterBehaviors.DRAW_EVENTS.DRAW_CONTENT)
			{
				SciterText txt = SciterText.Create("hi", se._he);

				using(SciterGraphics g = new SciterGraphics(prms.gfx))
				{
					g.DrawText(txt, 0, 0, 7);
				}

				return true;
			}
			return false;
		}
	}
}