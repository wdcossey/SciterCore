using SciterCore;
using SciterCore.Attributes;
using SciterCore.Interop;
using SciterGraphics = SciterCore.SciterGraphics;

namespace SciterTest.Graphics.Behaviors
{
	[SciterBehavior("draw-text")]
	class DrawTextBehavior : SciterEventHandler
	{
		protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
		{
			if (prms.cmd != SciterBehaviors.DRAW_EVENTS.DRAW_CONTENT) 
				return false;
			
			var txt = SciterText.CreateForElement("hi", se);

			using(var g = new SciterGraphics(prms.gfx))
			{
				g.DrawText(txt, 0, 0, 1);
			}

			return true;
		}
	}
}