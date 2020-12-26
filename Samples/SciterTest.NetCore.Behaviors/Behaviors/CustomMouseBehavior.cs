using System;
using System.Collections.Generic;
using SciterCore;
using SciterCore.Attributes;

namespace SciterTest.NetCore.Behaviors
{
    [SciterBehavior("mouse-behavior")]
    public class CustomMouseBehavior : SciterEventHandler
    {
        List<SciterPoint> points = new List<SciterPoint>()
        {
            SciterPoint.Empty
        };
            
        protected override EventGroups SubscriptionsRequest(SciterElement element)
        {
            return EventGroups.HandleAll;
        }

        protected override bool OnMouse(SciterElement element, MouseArgs args)
        {
            
            switch (args.Event)
            {
                case MouseEvents.Enter:
                    break;
                case MouseEvents.Leave:
                    break;
                case MouseEvents.Move:
                    break;
                case MouseEvents.Up:
                    break;
                case MouseEvents.Down:
                    break;
                case MouseEvents.DoubleClick:
                    break;
                case MouseEvents.Wheel:
                    break;
                case MouseEvents.Tick:
                    break;
                case MouseEvents.Idle:
                    break;
                case MouseEvents.Drop:
                    break;
                case MouseEvents.DragEnter:
                    break;
                case MouseEvents.DragLeave:
                    break;
                case MouseEvents.DragRequest:
                    break;
                case MouseEvents.MouseClick:
                    points.Add(new SciterPoint(args.ElementPosition.X, args.ElementPosition.Y));
                    break;
                case MouseEvents.Dragging:
                    break;
                default:
                    return base.OnMouse(element, args);;
            }

            

            return base.OnMouse(element, args);
        }

        protected override bool OnDraw(SciterElement element, DrawArgs args)
        {

            if (args.DrawEvent != DrawEvent.Foreground)
                return false;
            
            var random = new Random(50);
            var padding = 8;
            
            //if (args.Event == MouseEvents.MouseClick)
            using (var graphics = SciterGraphics.Create(args.Handle))
            {
                    
                graphics.SaveState()
                        
                    .Translate(args.Area.Left, args.Area.Top)
                    .PushClipBox(args.Area.Left, args.Area.Top, args.Area.Width, args.Area.Height)
                    .SetLineWidth(2)
                    .SetLineJoin(LineJoinType.Round)
                    .SetLineGradientLinear(
                        1, 1, 100, 100,
                        SciterColorStop.Create(0f, 0, 255, 0, 1f),
                        SciterColorStop.Create(.5f, 255, 255, 0, 1f),
                        SciterColorStop.Create(1f, 255, 0, 0, 1f));

                for (int i = 1; i < points.Count; i++)
                {
                    graphics.DrawLine(points[i-1].X, points[i-1].Y, points[i].X, points[i].Y);
                }
                    
                foreach (var point in points)
                {
                        
                }
                        

                graphics.RestoreState();
            }

            return true;
        }
    }
}