using System;
using System.Collections.Generic;
using NUnit.Framework;
using SciterCore.Attributes;
using SciterCore.Interop;

namespace SciterCore.UnitTests.Graphics
{
    public class SciterGraphicsTests
    {
        private SciterWindow _sciterWindow;


        [SciterBehavior("draw-content")]
        class DrawContentBehavior : SciterEventHandler
        {
            private readonly SciterWindow _window;
            private readonly Func<SciterElement, SciterBehaviors.DRAW_PARAMS, bool> _drawCallback;

            public DrawContentBehavior(SciterWindow window, Func<SciterElement, SciterBehaviors.DRAW_PARAMS, bool> drawCallback)
            {
                _window = window;
                _drawCallback = drawCallback;
            }

            protected override bool OnDraw(SciterElement se, SciterBehaviors.DRAW_PARAMS prms)
            {
                return _drawCallback.Invoke(se, prms);
            }
        }
        
        [SetUp]
        public void Setup()
        {
            _sciterWindow = 
                new SciterWindow()
                    .CreateMainWindow(640, 480)
                    //.CenterTopLevelWindow()
                    .SetTitle(nameof(SciterGraphicsTests));
            
            var pageData = "<html><head><style>" +
                           "html {" +
                           //"background: black;" +
                           //"behavior: draw-content;" +
                           "}" +
                           "</style></head></html>";
            
            _sciterWindow.LoadHtml(pageData);
        }
        
        [TearDown]
        public void TearDown()
        {
            
        }

        private void TranslateAndDispatch()
        {
            while(PInvokeWindows.GetMessage(lpMsg: out var msg, hWnd: IntPtr.Zero, wMsgFilterMin: 0, wMsgFilterMax: 0) != 0)
            {
                PInvokeWindows.TranslateMessage(ref msg);
                PInvokeWindows.DispatchMessage(ref msg);
            }
        }

        [TestCase(LineCapType.Round, LineJoinType.Bevel)]
        [TestCase(LineCapType.Round, LineJoinType.Miter)]
        [TestCase(LineCapType.Round, LineJoinType.Round)]
        [TestCase(LineCapType.Round, LineJoinType.MiterOrBevel)]
        
        [TestCase(LineCapType.Butt, LineJoinType.Bevel)]
        [TestCase(LineCapType.Butt, LineJoinType.Miter)]
        [TestCase(LineCapType.Butt, LineJoinType.Round)]
        [TestCase(LineCapType.Butt, LineJoinType.MiterOrBevel)]
        
        [TestCase(LineCapType.Square, LineJoinType.Bevel)]
        [TestCase(LineCapType.Square, LineJoinType.Miter)]
        [TestCase(LineCapType.Square, LineJoinType.Round)]
        [TestCase(LineCapType.Square, LineJoinType.MiterOrBevel)]
        
        [TestCase(LineCapType.Round, LineJoinType.Bevel)]
        [TestCase(LineCapType.Round, LineJoinType.Miter)]
        [TestCase(LineCapType.Round, LineJoinType.Round)]
        [TestCase(LineCapType.Round, LineJoinType.MiterOrBevel)]
        public void DrawLine_with_lineCap_and_lineJoin(LineCapType lineCap, LineJoinType lineJoin)
        {
            var random = new Random();
            
            var host = new SciterHost(_sciterWindow);
            host.RegisterBehaviorHandler(() => new DrawContentBehavior(_sciterWindow, (element, prms) =>
            {
                if (prms.cmd != SciterBehaviors.DRAW_EVENTS.DRAW_CONTENT) 
                    return false;
                
                using(var graphics = SciterGraphics.Create(prms.gfx))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        graphics.SaveState()
                            .Translate(prms.area.Left, prms.area.Top)
                            .SetLineColor(random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue))
                            .SetFillColor(random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue))
                            .SetLineWidth(random.Next(2, 10))
                            .SetLineCap(lineCap)
                            .SetLineJoin(lineJoin)
                            .DrawLine(random.Next(byte.MinValue, prms.area.Width),
                                random.Next(byte.MinValue, prms.area.Height),
                                random.Next(byte.MinValue, prms.area.Width),
                                random.Next(byte.MinValue, prms.area.Height))
                            .RestoreState();
                    }
                }
                
                element?.Window?.Close();
                
                return true;

            }), behaviorName: "draw-lines");
            
            _sciterWindow.Show();
            
            _sciterWindow.RootElement.AppendChildElement("body")
                .SetStyleValue("background", $"rgb({random.Next(byte.MinValue, byte.MaxValue)}, {random.Next(byte.MinValue, byte.MaxValue)}, {random.Next(byte.MinValue, byte.MaxValue)})")
                .SetStyleValue("behavior", "draw-lines");

            TranslateAndDispatch();
            
            //Assert.NotNull(_sciterGraphics);
        }
        
        [Test]
        public void Polygon()
        {
            var random = new Random();
            
            var host = new SciterHost(_sciterWindow);
            host.RegisterBehaviorHandler(() => new DrawContentBehavior(_sciterWindow, (element, prms) =>
            {
                if (prms.cmd != SciterBehaviors.DRAW_EVENTS.DRAW_CONTENT) 
                    return false;
                
                using(var graphics = SciterGraphics.Create(prms.gfx))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        graphics.SaveState()
                            .Translate(prms.area.Left, prms.area.Top)
                            .SetFillColor(random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue))
                            .DrawPolygon(() =>
                            {
                                var result = new List<PolygonPoint>();
                                for (var j = 0; j < random.Next(3, 12); j++)
                                {
                                    result.Add(PolygonPoint.Create(
                                        random.Next(byte.MinValue, prms.area.Width),
                                        random.Next(byte.MinValue, prms.area.Height)));
                                }
                                return result;
                            })
                            .RestoreState();
                    }
                }
                
                element?.Window?.Close();
                
                return true;

            }), behaviorName: "draw-polygon");
            
            _sciterWindow.Show();

            var backgroundColor = random.Next(byte.MinValue, 80);
            
            _sciterWindow.RootElement.AppendChildElement("body")
                .SetStyleValue("background", $"rgb({backgroundColor}, {backgroundColor}, {backgroundColor})")
                .SetStyleValue("behavior", "draw-polygon");

            TranslateAndDispatch();
            
            //Assert.NotNull(_sciterGraphics);
        }
        
        [TestCase(LineCapType.Round, LineJoinType.Bevel)]
        [TestCase(LineCapType.Round, LineJoinType.Miter)]
        [TestCase(LineCapType.Round, LineJoinType.Round)]
        [TestCase(LineCapType.Round, LineJoinType.MiterOrBevel)]
        
        [TestCase(LineCapType.Butt, LineJoinType.Bevel)]
        [TestCase(LineCapType.Butt, LineJoinType.Miter)]
        [TestCase(LineCapType.Butt, LineJoinType.Round)]
        [TestCase(LineCapType.Butt, LineJoinType.MiterOrBevel)]
        
        [TestCase(LineCapType.Square, LineJoinType.Bevel)]
        [TestCase(LineCapType.Square, LineJoinType.Miter)]
        [TestCase(LineCapType.Square, LineJoinType.Round)]
        [TestCase(LineCapType.Square, LineJoinType.MiterOrBevel)]
        
        [TestCase(LineCapType.Round, LineJoinType.Bevel)]
        [TestCase(LineCapType.Round, LineJoinType.Miter)]
        [TestCase(LineCapType.Round, LineJoinType.Round)]
        [TestCase(LineCapType.Round, LineJoinType.MiterOrBevel)]
        public void Polyline(LineCapType lineCap, LineJoinType lineJoin)
        {
            var random = new Random();
            
            var host = new SciterHost(_sciterWindow);
            host.RegisterBehaviorHandler(() => new DrawContentBehavior(_sciterWindow, (element, prms) =>
            {
                if (prms.cmd != SciterBehaviors.DRAW_EVENTS.DRAW_CONTENT) 
                    return false;
                
                using(var graphics = SciterGraphics.Create(prms.gfx))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        graphics.SaveState()
                            .Translate(prms.area.Left, prms.area.Top)
                            .SetLineColor(random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue))
                            .SetLineCap(lineCap)
                            .SetLineJoin(lineJoin)
                            .SetLineWidth(random.Next(2, 10))
                            .DrawPolyline(() =>
                            {
                                var result = new List<PolylinePoint>();
                                for (var j = 0; j < random.Next(3, 12); j++)
                                {
                                    result.Add(PolylinePoint.Create(
                                        random.Next(byte.MinValue, prms.area.Width),
                                        random.Next(byte.MinValue, prms.area.Height)));
                                }
                                return result;
                            })
                            .RestoreState();
                    }
                }
                
                element?.Window?.Close();
                
                return true;

            }), behaviorName: "draw-polyline");
            
            _sciterWindow.Show();

            var backgroundColor = random.Next(byte.MinValue, 80);
            
            _sciterWindow.RootElement.AppendChildElement("body")
                .SetStyleValue("background", $"rgb({backgroundColor}, {backgroundColor}, {backgroundColor})")
                .SetStyleValue("behavior", "draw-polyline");

            TranslateAndDispatch();
            
            //Assert.NotNull(_sciterGraphics);
        }
        
        [Test]
        public void DrawEllipse()
        {
            var random = new Random();
            
            var host = new SciterHost(_sciterWindow);
            host.RegisterBehaviorHandler(() => new DrawContentBehavior(_sciterWindow, (element, prms) =>
            {
                if (prms.cmd != SciterBehaviors.DRAW_EVENTS.DRAW_CONTENT) 
                    return false;
                
                using(var graphics = SciterGraphics.Create(prms.gfx))
                {
                    for (int i = 0; i < 20; i++)
                    {
                        graphics.SaveState()
                            .Translate(prms.area.Left, prms.area.Top)
                            .SetLineColor(random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue))
                            .SetLineWidth(random.Next(2, 10))
                            .DrawEllipse(prms.area.Width / 2, prms.area.Height / 2, random.Next(byte.MinValue, prms.area.Width / 2), random.Next(byte.MinValue, prms.area.Height / 2))
                            .RestoreState();
                    }
                }
                
                element?.Window?.Close();
                
                return true;

            }), behaviorName: "draw-ellipse");
            
            _sciterWindow.Show();

            var backgroundColor = random.Next(byte.MinValue, 80);
            
            _sciterWindow.RootElement.AppendChildElement("body")
                .SetStyleValue("background", $"rgb({backgroundColor}, {backgroundColor}, {backgroundColor})")
                .SetStyleValue("behavior", "draw-ellipse");

            TranslateAndDispatch();
            
            //Assert.NotNull(_sciterGraphics);
        }
        
        [Test]
        public void DrawRectangle()
        {
            var random = new Random();
            
            var host = new SciterHost(_sciterWindow);
            host.RegisterBehaviorHandler(() => new DrawContentBehavior(_sciterWindow, (element, prms) =>
            {
                if (prms.cmd != SciterBehaviors.DRAW_EVENTS.DRAW_CONTENT) 
                    return false;
                
                using(var graphics = SciterGraphics.Create(prms.gfx))
                {
                    for (int i = 0; i < 20; i++)
                    {
                        graphics.SaveState()
                            .Translate(prms.area.Left, prms.area.Top)
                            .SetLineColor(random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue))
                            .SetFillColor(random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue))
                            .SetLineWidth(random.Next(2, 10))
                            .DrawRectangle(random.Next(byte.MinValue, prms.area.Width), random.Next(byte.MinValue, prms.area.Height), random.Next(byte.MinValue, prms.area.Width), random.Next(byte.MinValue, prms.area.Height))
                            .RestoreState();
                    }
                }
                
                element?.Window?.Close();
                
                return true;

            }), behaviorName: "draw-ellipse");
            
            _sciterWindow.Show();

            var backgroundColor = random.Next(byte.MinValue, 80);
            
            _sciterWindow.RootElement.AppendChildElement("body")
                .SetStyleValue("background", $"rgb({backgroundColor}, {backgroundColor}, {backgroundColor})")
                .SetStyleValue("behavior", "draw-ellipse");

            TranslateAndDispatch();
            
            //Assert.NotNull(_sciterGraphics);
        }
        
        [Test]
        public void DrawLine()
        {
            var random = new Random();
            
            var host = new SciterHost(_sciterWindow);
            host.RegisterBehaviorHandler(() => new DrawContentBehavior(_sciterWindow, (element, prms) =>
            {
                if (prms.cmd != SciterBehaviors.DRAW_EVENTS.DRAW_CONTENT) 
                    return false;
                
                using(var graphics = SciterGraphics.Create(prms.gfx))
                {
                    for (int i = 0; i < 20; i++)
                    {
                        graphics.SaveState()
                            .Translate(prms.area.Left, prms.area.Top)
                            .SetLineColor(random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue))
                            .SetFillColor(random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue),
                                random.Next(byte.MinValue, byte.MaxValue))
                            .SetLineWidth(random.Next(2, 10))
                            .DrawText(SciterText.CreateForElementAndStyle("The quick brown fox jumps over the lazy dog", element.Handle, $"color: rgba({random.Next(byte.MinValue, byte.MaxValue)}, {random.Next(byte.MinValue, byte.MaxValue)}, {random.Next(byte.MinValue, byte.MaxValue)});font-size: {random.Next(12, 48)}dip;"), random.Next(byte.MinValue, prms.area.Width), random.Next(byte.MinValue, prms.area.Height), 5)
                            .RestoreState();
                    }
                }
                
                element?.Window?.Close();
                
                return true;

            }), behaviorName: "draw-ellipse");
            
            _sciterWindow.Show();

            var backgroundColor = random.Next(byte.MinValue, 80);
            
            _sciterWindow.RootElement.AppendChildElement("body")
                //.SetStyleValue("background", $"rgb({backgroundColor}, {backgroundColor}, {backgroundColor})")
                .SetStyleValue("background", $"black")
                .SetStyleValue("behavior", "draw-ellipse");

            TranslateAndDispatch();
            
            //Assert.NotNull(_sciterGraphics);
        }
    }
}