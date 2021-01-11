using System;
using SciterCore;
using SciterCore.Attributes;
using SciterGraphics = SciterCore.SciterGraphics;

namespace SciterTest.NetCore.Behaviors
{
	[SciterBehavior("draw-line")]
	public class CustomDrawBehavior : SciterEventHandler
	{
		protected override EventGroups SubscriptionsRequest(SciterElement element)
		{
			return EventGroups.HandleDraw;
		}

		protected override bool OnTimer(SciterElement element, IntPtr? extTimerId)
		{
			//dom::element(he).refresh(); // ref
			element.Refresh();
			return true;
		}

		protected override bool OnDraw(SciterElement se, DrawArgs args)
		{

			if (args.DrawEvent != DrawEvent.Content)
				return false;

			se.Attributes.TryGetValue("fill", out var fillStyle);

			var random = new Random(50);
			var padding = 8;

			using (var graphics = SciterGraphics.Create(args.Handle))
			{
				for (var i = 0; i < 50; i++)
				{
					var lineX1 = random.Next(padding, args.Area.Width - padding);
					var lineY1 = random.Next(padding, args.Area.Height - padding);
					var lineX2 = random.Next(padding, args.Area.Width - padding);
					var lineY2 = random.Next(padding, args.Area.Height - padding);
					var alpha = (byte) random.Next(byte.MinValue, byte.MaxValue);
					var lineCap = (LineCapType) random.Next((int)LineCapType.Butt, ((int)LineCapType.Round) + 1);
					var lineJoin = (LineJoinType) random.Next((int)LineJoinType.Miter, ((int)LineJoinType.MiterOrBevel) + 1);

					var lineColorR = (byte) random.Next(byte.MinValue, byte.MaxValue);
					var lineColorG = (byte) random.Next(byte.MinValue, byte.MaxValue);
					var lineColorB = (byte) random.Next(byte.MinValue, byte.MaxValue);

					var lineColorWidth = random.Next(2, 10);

					switch ((fillStyle ?? string.Empty).ToLowerInvariant())
					{

						case "linear-gradient":
							graphics.SaveState()
								.Translate(args.Area.Left, args.Area.Top)
								.SetLineWidth(lineColorWidth)
								.SetLineCap(lineCap)
								.SetLineJoin(lineJoin)
								.SetLineGradientLinear(
									lineX1, lineY1, lineX2, lineY2,
									SciterColorStop.Create(0f, 0, 255, 0, alpha),
									SciterColorStop.Create(.5f, 255, 255, 0, alpha),
									SciterColorStop.Create(1f, 255, 0, 0, alpha))
								.DrawLine(lineX1, lineY1, lineX2, lineY2)

								.RestoreState();
							break;

						case "alt-linear-gradient":
							graphics.SaveState()
								.Translate(args.Area.Left, args.Area.Top)
								.SetLineWidth(lineColorWidth)
								.SetLineCap(lineCap)
								.SetLineJoin(lineJoin)
								.SetLineGradientLinear(
									0f,
									0f,
									args.Area.Width,
									args.Area.Height,
									SciterColorStop.Create(0f, 0, 255, 0, alpha),
									SciterColorStop.Create(.5f, 255, 255, 0, alpha),
									SciterColorStop.Create(1f, 255, 0, 0, alpha))
								.DrawLine(lineX1, lineY1, lineX2, lineY2)

								//.SetFillGradientLinear(
								//	0f,
								//	0f,
								//	args.Area.Width,
								//	args.Area.Height,
								//	SciterColorStop.Create(0f, 0, 255, 0, alpha),
								//	SciterColorStop.Create(.5f, 255, 255, 0, alpha),
								//	SciterColorStop.Create(1f, 255, 0, 0, alpha))
								//.DrawRectangle(lineX1, lineY1, lineX2, lineY2)

								.RestoreState();
							break;

						default:
							graphics.SaveState()
								.Translate(args.Area.Left, args.Area.Top)
								.SetLineWidth(lineColorWidth)
								.SetLineColor(
									lineColorR,
									lineColorG,
									lineColorB,
									alpha)
								.SetLineCap(lineCap)
								.SetLineJoin(lineJoin)
								.DrawLine(lineX1, lineY1, lineX2, lineY2)

								.RestoreState();
							break;
					}

				}
				
				
			}

			return true;
		}
	}
}
