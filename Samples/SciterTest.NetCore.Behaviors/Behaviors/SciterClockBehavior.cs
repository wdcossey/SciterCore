using System;
using System.Collections.Generic;
using System.Linq;
using SciterCore;
using SciterCore.Attributes;

namespace SciterTest.NetCore.Behaviors
{
    [SciterBehavior("sciter-clock-behavior")]
    public class SciterClockBehavior : SciterEventHandler
    {
        const float Pi = 3.1415926535897932384626433832795f;
		
        protected override EventGroups SubscriptionsRequest(SciterElement element)
        {
            return EventGroups.HandleDraw | EventGroups.HandleTimer;
        }

        protected override void Attached(SciterElement element)
        {
            element.StartTimer(250);
            base.Attached(element);
        }

        protected override void Detached(SciterElement element)
        {
            element.StopTimer();
            base.Detached(element);
        }

        protected override bool OnTimer(SciterElement element, IntPtr extTimerId)
        {
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
				

                //float w = 0f + args.Area.Right - args.Area.Left;
                //float h = 0f + args.Area.Bottom - args.Area.Top;
                float scale = args.Area.Width < args.Area.Height ? args.Area.Width / 300.0f : args.Area.Height / 300.0f;
				
                se.Attributes.TryGetValue("face", out var clockFace);

                var timeInfo = DateTime.Now;
				
                switch (clockFace)
                {
                    case "swiss":
                        DrawSwissClock(graphics, args.Area, scale, timeInfo);
                        break;
                    case "circles":
                        DrawCirclesClock(graphics, args.Area, scale, timeInfo);
                        break;
                    default:
                        DrawDefaultClock(graphics, args.Area, scale, timeInfo);
                        break;
                }
				
            }

            return true;
        }

        private void DrawDefaultClock(
            SciterGraphics graphics, 
            SciterRectangle area, 
            float scale, 
            DateTime timeInfo)
        {
            var markColor = SciterColor.White;
            var hourColor = SciterColor.White;
            var minuteColor = SciterColor.White;
            var secondColor = SciterColor.Create(0xF3, 0x7F, 0x14);
				

            graphics
                .SaveState()
                .Translate(area.Left + area.Width / 2.0f, area.Top + area.Height / 2.0f)
                .Scale(scale, scale)
                .Rotate(-Pi / 2)
                .SetLineColor(SciterColor.Transparent)
                .SetLineCap(LineCapType.Round);

            // Hour marks
            graphics
                .SaveState()
                .SetLineWidth(10f)
                .SetLineColor(markColor);

            for (int i1 = 0; i1 < 12; ++i1)
            {
                graphics.Rotate(Pi / 6, 0, 0);
                graphics.DrawLine(125f, 0, 140f, 0);
            }

            graphics.RestoreState();


            // Minute marks
            graphics
                .SaveState()
                .SetLineWidth(2f)
                .SetLineColor(markColor);
            for (int i = 0; i < 60; ++i)
            {
                if (i % 5 != 0)
                    graphics.DrawLine(134f, 0, 144f, 0);
                graphics.Rotate(Pi / 30f);
            }

            graphics.RestoreState();


            int sec = timeInfo.Second;
            int min = timeInfo.Minute;
            int hr = timeInfo.Hour;
            hr = hr >= 12 ? hr - 12 : hr;

            // draw Hours
            graphics.SaveState()
                .Rotate(hr * (Pi / 6) + (Pi / 360) * min + (Pi / 21600) * sec)
                .SetLineWidth(10f)
                .SetLineColor(hourColor)
                .DrawLine(-20, 0, 50, 0)
                .RestoreState();

            // draw Minutes
            graphics
                .SaveState()
                .Rotate((Pi / 30) * min + (Pi / 1800) * sec)
                .SetLineWidth(10f)
                .SetLineColor(minuteColor)
                .DrawLine(-20, 0, 100, 0)
                .RestoreState();

            graphics
                .SaveState()
                .Rotate(sec * Pi / 30)
                .SetLineColor(secondColor)
                .SetFillColor(secondColor)
                .SetLineWidth(6f)
                .DrawLine(-20f, 0, 80, 0)
                .DrawEllipse(0, 0, 10, 10)
                .SetFillColor(SciterColor.Transparent)
                .DrawEllipse(90, 0, 8, 8)
                .SetLineWidth(1f)
                .SetLineColor(SciterColor.Goldenrod)
                .SetFillColor(SciterColor.Gold)
                .DrawEllipse(0, 0, 2, 2)
                .RestoreState();

            graphics.RestoreState();

            //graphics.DrawText(
            //	SciterText.CreateForElementAndStyle($"{timeinfo.Hour:00}:{timeinfo.Second:00}", se,
            //		$"font-size:24pt;color:{secondColorHex}"), area.Left + area.Width / 2.0f,
            //	area.Top + area.Height / 4.0f, 5);

        }
		
        private void DrawSwissClock(
            SciterGraphics graphics, 
            SciterRectangle area, 
            float scale, 
            DateTime timeInfo)
        {
            var markColor = SciterColor.Black;
            var hourColor = SciterColor.Black;
            var minuteColor = SciterColor.Black;
            var secondColor = SciterColor.Create(0xA9, 0x33, 0x2A);
			
            graphics
                .SaveState()
                .Translate(area.Left + area.Width / 2.0f, area.Top + area.Height / 2.0f)
                .Scale(scale, scale)
                .Rotate(-Pi / 2)
                .SetLineColor(SciterColor.Transparent)
                .SetLineCap(LineCapType.Square);

            // Hour marks
            graphics
                .SaveState()
                .SetLineWidth(10f)
                .SetLineColor(markColor);

            for (int i1 = 0; i1 < 12; ++i1)
            {
                graphics.Rotate(Pi / 6, 0, 0);
                graphics.DrawLine(125f, 0, 140f, 0);
            }

            graphics.RestoreState();


            // Minute marks
            graphics
                .SaveState()
                .SetLineWidth(2f)
                .SetLineColor(markColor);
            for (int i = 0; i < 60; ++i)
            {
                if (i % 5 != 0)
                    graphics.DrawLine(134f, 0, 144f, 0);
                graphics.Rotate(Pi / 30f);
            }

            graphics.RestoreState();


            int sec = timeInfo.Second;
            int min = timeInfo.Minute;
            int hr = timeInfo.Hour;
            hr = hr >= 12 ? hr - 12 : hr;

            // draw Hours
            graphics.SaveState()
                .Rotate(hr * (Pi / 6) + (Pi / 360) * min + (Pi / 21600) * sec)
                .SetLineWidth(10f)
                .SetLineColor(hourColor)
                .DrawLine(-40, 0, 80, 0)
                .RestoreState();

            // draw Minutes
            graphics
                .SaveState()
                .Rotate((Pi / 30) * min + (Pi / 1800) * sec)
                .SetLineWidth(10f)
                .SetLineColor(minuteColor)
                .DrawLine(-40, 0, 130, 0)
                .RestoreState();

            graphics
                .SaveState()
                .Rotate(sec * Pi / 30)
                .SetLineColor(secondColor)
                .SetFillColor(secondColor)
                .SetLineWidth(4)
                .DrawLine(-40, 0, 110, 0)
                .DrawEllipse(0, 0, 3, 3)
                .SetFillColor(SciterColor.Gold)
                .SetLineWidth(1f)
                .DrawEllipse(0, 0, 2, 2)
                .SetFillColor(secondColor)
                .DrawEllipse(105, 0, 10, 10)
                .RestoreState();

            graphics.RestoreState();
        }
		
        private void DrawCirclesClock(
            SciterGraphics graphics, 
            SciterRectangle area, 
            float scale, 
            DateTime timeInfo)
        {
            var markColor = SciterColor.Parse("#969696");
            var hourColor = SciterColor.Parse("#969696");
            var minuteColor = SciterColor.Parse("#969696");
            var secondColor = SciterColor.Parse("#64C439");
			
            graphics
                .SaveState()
                .Translate(area.Left + area.Width / 2.0f, area.Top + area.Height / 2.0f)
                .Scale(scale, scale)
                .Rotate(-Pi / 2)
                .SetLineColor(SciterColor.Transparent)
                .SetLineCap(LineCapType.Square);

            int sec = timeInfo.Second;
            int min = timeInfo.Minute;
            int hr = timeInfo.Hour;
            hr = hr >= 12 ? hr - 12 : hr;

            // Background
            graphics
                .SaveState()
                .Translate(0, 0)
                .SetLineColor(SciterColor.Transparent)
                .SetFillGradientLinear(0f, 75f, 150f, 75f, SciterColorStop.Create(0f, SciterColor.Parse("#242424")), SciterColorStop.Create(1f, SciterColor.Parse("#545454")))
                .DrawEllipse(0, 0, 150, 150)
                .SetFillColor(SciterColor.Transparent)
                .SetLineColor(minuteColor)
                .DrawEllipse(0, 0, 150, 150)
                .RestoreState();
            
            graphics
                .SaveState()
                .Translate(-(area.Height / 3.0f), 0)
                .SetLineWidth(2f)
                .SetLineColor(SciterColor.Black)
                .SetFillColor(SciterColor.Parse("#222222"))
                .DrawEllipse(0, 0, 50, 50)
                .SetLineColor(markColor)
                .Using(@ref =>
                {
                    for (var i = 0; i < 12; ++i)
                    {
                        @ref
                            .Rotate(Pi / 6, 0, 0)
                            .DrawLine(34, 0, 44f, 0);

                    }
                })
                .RestoreState();

            graphics
                .SaveState()
                .Translate(-(area.Height / 3.0f), 0)
                .SetLineColor(secondColor)
                .Using(@ref =>
                {
                    @ref
                        .SetLineWidth(2f)
                        .SetLineColor(markColor);
					
                    for (var i = 0; i < 60; ++i)
                    {
                        if (i % 5 != 0)
                            @ref.DrawLine(40f, 0, 44f, 0);
                        @ref.Rotate(Pi / 30f);
                    }
                })
                .RestoreState();

            graphics
                .SaveState()
                .Translate(-(area.Height / 3.0f), 0)
                .Rotate(sec * Pi / 30)
                .SetLineColor(secondColor)
                .SetFillColor(secondColor)
                .SetLineWidth(2f)
                .DrawLine(0, 0, 44, 0)
                .DrawEllipse(0, 0, 3, 3)
                .RestoreState();
			
			
            // Hour marks
            graphics
                .SaveState()
                .SetLineColor(markColor)
                .Using(@ref =>
                {
                    for (var i = 0; i < 12; ++i)
                    {
                        @ref.Rotate(Pi / 6, 0, 0);
                        if (new [] {2, 5, 8, 11}.Contains(i))
                        {
                            @ref
                                .SetLineWidth(8f)
                                .DrawLine(125f, 0, 142f, 0);
                        }
                        else
                        {
                            @ref
                                .SetLineWidth(2f)
                                .DrawLine(125f, 0, 145f, 0);
                        }
                    }
                })
                .RestoreState();


            // Minute marks
            graphics
                .SaveState()
                .SetLineWidth(2f)
                .SetLineColor(markColor)
                .Using(@ref =>
                {
                    for (var i = 0; i < 60; ++i)
                    {
                        if (i % 5 != 0)
                            @ref.DrawLine(138f, 0, 144f, 0);
                        @ref.Rotate(Pi / 30f);
                    }
                })
                .SetLineColor(minuteColor)
                .DrawEllipse(0, 0, 150, 150)
                .RestoreState();

            // draw Hours
            graphics.SaveState()
                .Rotate(hr * (Pi / 6) + (Pi / 360) * min + (Pi / 21600) * sec)
                .SetLineWidth(1f)
                .SetFillGradientLinear(50f, -3f, 50f, 3f, 
                    SciterColorStop.Create(0f, SciterColor.Parse("#c4c4c4")), 
                    SciterColorStop.Create(1f, SciterColor.Parse("#B2B2B2")))
                .DrawPolygon(() => new List<PolygonPoint>()
                {
                    PolygonPoint.Create(0f, -3f),
                    PolygonPoint.Create(90f, -3f),
                    PolygonPoint.Create(100f, 0f),
                    PolygonPoint.Create(90f, 3f),
                    PolygonPoint.Create(0f, 3f),
                })
                //.SetLineWidth(0f)
                //.DrawEllipse(0, 0, 10, 10)
                .RestoreState();

            // draw Minutes
            graphics
                .SaveState()
                .Rotate((Pi / 30) * min + (Pi / 1800) * sec)
                //.SetLineColor(SciterColor.Transparent)
                //.SetLineWidth(0f)
                //.SetFillColor(SciterColor.Create(0, 0, 0, .5f))
                //.DrawEllipse(0, 0, 10, 10)
				
                .SetLineWidth(1f)
                .SetLineColor(minuteColor)
                .SetFillColor(minuteColor)
                .SetFillGradientLinear(65, -3f, 65f, 3f, 
                    SciterColorStop.Create(0f, SciterColor.Parse("#c4c4c4")), 
                    SciterColorStop.Create(1f, SciterColor.Parse("#B2B2B2")))
                .DrawPolygon(() => new List<PolygonPoint>()
                {
                    PolygonPoint.Create(0, -3),
                    PolygonPoint.Create(120, -3),
                    PolygonPoint.Create(130, 0),
                    PolygonPoint.Create(120, 3),
                    PolygonPoint.Create(0, 3),
                })
                .DrawEllipse(0, 0, 8, 8)
                .SetLineColor(SciterColor.Parse("#686868"))
                .SetFillColor(SciterColor.Parse("#2E2E2E"))
                .SetLineWidth(2f)
                .DrawEllipse(0, 0, 4, 4)
                //.SetLineWidth(1f)
                
                .RestoreState();

            // Outer ring
            graphics
                .SaveState()
                .SetLineColor(minuteColor)
                .DrawEllipse(0, 0, 150, 150)
                .RestoreState();
            
            graphics.RestoreState();
        }

        [SciterFunctionName("nativeGetPath")]
        public SciterValue NativeGetPath(SciterValue vx, SciterValue vy, SciterValue vw, SciterValue vh, SciterValue vt, SciterValue vclosed)
        {
            double x = vx.AsDouble();
            double y = vy.AsDouble();
            double w = vw.AsDouble();
            double h = vh.AsDouble();
            double t = vt.AsDouble();
            bool  closed = vclosed.AsBoolean();

            double[] samples = new double[6];
            double[] sx = new double[6];
            double[] sy = new double[6];
			
            double dx = w/5.0f;
			
            samples[0] = (1+Math.Sin(t*1.2345f+Math.Cos(t*0.33457f)*0.44f))*0.5f;
            samples[1] = (1+Math.Sin(t*0.68363f+Math.Cos(t*1.3f)*1.55f))*0.5f;
            samples[2] = (1+Math.Sin(t*1.1642f+Math.Cos(t*0.33457f)*1.24f))*0.5f;
            samples[3] = (1+Math.Sin(t*0.56345f+Math.Cos(t*1.63f)*0.14f))*0.5f;
            samples[4] = (1+Math.Sin(t*1.6245f+Math.Cos(t*0.254f)*0.3f))*0.5f;
            samples[5] = (1+Math.Sin(t*0.345f+Math.Cos(t*0.03f)*0.6f))*0.5f;

            for (int i = 0; i < 6; i++) {
                sx[i] = x+i*dx;
                sy[i] = y+h*samples[i]*0.8f;
            }

            // creating path:
            var p = SciterPath.Create();

            p.MoveTo(sx[0], sy[0],false);
            for(var i = 1; i < 6; ++i)
                p.BezierCurveTo((sx[i-1])+dx*0.5f,sy[i-1], sx[i]-dx*0.5f,sy[i], sx[i],sy[i],false);

            if (closed) {
                p.LineTo(x+w,y+h,false);
                p.LineTo(x+0,y+h,false);
                p.ClosePath();
            }
			
            return p.ToValue(); // wrap the path into sciter::value;
        }
		
		

    }
}