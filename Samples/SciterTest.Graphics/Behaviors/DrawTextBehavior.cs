using SciterCore;
using SciterCore.Attributes;
using SciterCore.Interop;
using SciterGraphics = SciterCore.SciterGraphics;

namespace SciterTest.Graphics.Behaviors
{
	[SciterBehavior("draw-text")]
	class DrawTextBehavior : SciterEventHandler
	{
		protected override bool OnDraw(SciterElement se, DrawEventArgs args)
		{
			if (args.DrawEvent != DrawEvent.Content) 
				return false;
			
			var txt = SciterText.CreateForElement("hi", se);

			using(var g = SciterGraphics.Create(args.Handle))
			{
				g.DrawText(txt, 0, 0, 1);
			}

			return true;
		}
	}
}