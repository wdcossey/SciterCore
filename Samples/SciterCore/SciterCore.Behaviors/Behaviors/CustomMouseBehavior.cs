using System;
using System.Collections.Generic;
using System.Linq;
using SciterCore.Attributes;

namespace SciterCore.Behaviors.Behaviors
{
    [SciterBehavior("mouse-behavior")]
    public class CustomMouseBehavior : SciterEventHandler
    {
        List<SciterPoint> _points = new List<SciterPoint>()
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
                    if (args.ButtonState == MouseButton.Secondary)
                        _points = new List<SciterPoint>()
                        {
                            new SciterPoint(args.ElementPosition.X, args.ElementPosition.Y)
                        };
                        
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
                    _points.Add(new SciterPoint(args.ElementPosition.X, args.ElementPosition.Y));
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
            
            var scale = args.Area.Width < args.Area.Height ? args.Area.Width / 300.0f : args.Area.Height / 300.0f;
            
            //if (args.Event == MouseEvents.MouseClick)
            using (var graphics = SciterGraphics.Create(args.Handle))
            {

                graphics.SaveState()

                    .Translate(args.Area.Left, args.Area.Top)
                    .SetLineWidth(2)
                    .SetLineJoin(LineJoinType.Round)
                    .SetLineGradientLinear(
                        1, 1, args.Area.Width, args.Area.Height,
                        SciterColorStop.Create(0f, 0, 255, 0, 1f),
                        SciterColorStop.Create(.5f, 255, 255, 0, 1f),
                        SciterColorStop.Create(1f, 255, 0, 0, 1f))
                    .SetFillGradientLinear(
                        1, 1, args.Area.Width, args.Area.Height,
                        SciterColorStop.Create(0f, 0, 255, 0, .5f),
                        SciterColorStop.Create(.5f, 255, 255, 0, .5f),
                        SciterColorStop.Create(1f, 255, 0, 0, .5f));

                var path = SciterPath.Create();
                path.MoveTo(_points[0].X,_points[0].Y,false);
                
                for (var i = 0; i < _points.Count; i++)
                {
                    
                    path.LineTo(_points[i].X,_points[i].Y,false);
                    //path.BezierCurveTo(_points[i].X,_points[i].Y, _points[i].X,_points[i].Y, _points[i].X,_points[i].Y,false);
                }

                graphics.SaveState()
                    .DrawPath(path, DrawPathMode.FillOnly )
                    .RestoreState();
                
                graphics.SaveState()
                    .DrawPath(path, DrawPathMode.StrokeOnly )
                    .RestoreState();

                //graphics.SaveState()
                //    .DrawPolyline(() => { return _points.Select(s => PolylinePoint.Create(s.X, s.Y)); })
                //    .RestoreState();

                //for (int i = 1; i < _points.Count; i++)
                //{
                //    
                //    graphics.DrawLine(_points[i-1].X, _points[i-1].Y, _points[i].X, _points[i].Y);
                //}
                //graphics.RestoreState();

                graphics.RestoreState();
            }

            return true;
        }
    }
}