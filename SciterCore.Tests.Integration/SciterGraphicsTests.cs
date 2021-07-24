using System;
using System.Collections.Generic;
using System.Drawing;
using Moq;
using NUnit.Framework;
using SciterCore.Attributes;
using SciterCore.Internal;
using SciterCore.Tests.Integration.TestHelpers;

namespace SciterCore.Tests.Integration
{
    public class SciterGraphicsTests
    {
        private SciterWindow _sciterWindow;
        private Mock<INamedBehaviorResolver> _mockBehaviorResolver;
        
        [SetUp]
        public void Setup()
        {
            _sciterWindow = 
                new SciterWindow()
                    .CreateMainWindow(320, 240)
                    //.CenterWindow()
                    .SetTitle(nameof(SciterGraphicsTests));
            
            var pageData = "<html><head><style>" +
                           "html {" +
                           //"background: black;" +
                           //"behavior: draw-content;" +
                           "}" +
                           "</style></head></html>";
            
            
            _sciterWindow.LoadHtml(pageData);

            _mockBehaviorResolver = new Mock<INamedBehaviorResolver>();
            
            _mockBehaviorResolver
                .Setup(resolver => resolver.ContainsKey(It.IsAny<string>()))
                .Returns(true);
        }

        [TearDown]
        public void TearDown()
        {
            
        }
        
        [SciterBehavior("draw-content")]
        class DrawContentBehavior : SciterEventHandler
        {
            private readonly SciterWindow _window;
            private readonly Func<SciterElement, DrawArgs, bool> _drawCallback;

            public DrawContentBehavior(SciterWindow window, Func<SciterElement, DrawArgs, bool> drawCallback)
            {
                _window = window;
                _drawCallback = drawCallback;
            }

            protected override bool OnDraw(SciterElement se, DrawArgs args)
            {
                return _drawCallback.Invoke(se, args);
            }
        }
        
        [Timeout(2500)]
        [TestCase("draw-lines-round-bevel", LineCapType.Round, LineJoinType.Bevel)]
        [TestCase("draw-lines-round-miter", LineCapType.Round, LineJoinType.Miter)]
        [TestCase("draw-lines-round-round", LineCapType.Round, LineJoinType.Round)]
        [TestCase("draw-lines-round-miterOrBevel", LineCapType.Round, LineJoinType.MiterOrBevel)]
        
        [TestCase("draw-lines-butt-bevel", LineCapType.Butt, LineJoinType.Bevel)]
        [TestCase("draw-lines-butt-miter", LineCapType.Butt, LineJoinType.Miter)]
        [TestCase("draw-lines-butt-round", LineCapType.Butt, LineJoinType.Round)]
        [TestCase("draw-lines-butt-miterOrBevel", LineCapType.Butt, LineJoinType.MiterOrBevel)]

        [TestCase("draw-lines-square-bevel", LineCapType.Square, LineJoinType.Bevel)]
        [TestCase("draw-lines-square-miter", LineCapType.Square, LineJoinType.Miter)]
        [TestCase("draw-lines-square-round", LineCapType.Square, LineJoinType.Round)]
        [TestCase("draw-lines-square-miterOrBevel", LineCapType.Square, LineJoinType.MiterOrBevel)]

        [TestCase("draw-lines-round-bevel", LineCapType.Round, LineJoinType.Bevel)]
        [TestCase("draw-lines-round-miter", LineCapType.Round, LineJoinType.Miter)]
        [TestCase("draw-lines-round-round", LineCapType.Round, LineJoinType.Round)]
        [TestCase("draw-lines-round-miterOrBevel", LineCapType.Round, LineJoinType.MiterOrBevel)]
        public void DrawLine_with_lineCap_and_lineJoin(string behaviorName, LineCapType lineCap, LineJoinType lineJoin)
        {
            var random = new Random();

            _mockBehaviorResolver.Setup(resolver => resolver.GetBehaviorHandler(behaviorName))
                .Returns(() =>
                {
                    return new DrawContentBehavior(_sciterWindow, (element, args) =>
                    {
                        if (args.DrawEvent != DrawEvent.Content)
                            return false;

                        using (var graphics = SciterGraphics.Create(args.Handle))
                        {
                            for (var i = 0; i < byte.MaxValue; i++)
                            {
                                graphics.SaveState()
                                    .Translate(args.Area.Left, args.Area.Top)
                                    .SetLineColor(
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue))
                                    .SetFillColor(
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue))
                                    .SetLineWidth(random.Next(2, 10))
                                    .SetLineCap(lineCap)
                                    .SetLineJoin(lineJoin)
                                    .DrawLine(random.Next(byte.MinValue, args.Area.Width),
                                        random.Next(byte.MinValue, args.Area.Height),
                                        random.Next(byte.MinValue, args.Area.Width),
                                        random.Next(byte.MinValue, args.Area.Height))
                                    .SetLineGradientLinear(
                                        0f,
                                        0f,
                                        args.Area.Width / 2f,
                                        args.Area.Height,
                                        SciterColorStop.Create(0f,
                                            (byte) random.Next(byte.MinValue, byte.MaxValue),
                                            (byte) random.Next(byte.MinValue, byte.MaxValue),
                                            (byte) random.Next(byte.MinValue, byte.MaxValue),
                                            (byte) random.Next(byte.MinValue, byte.MaxValue)),
                                        SciterColorStop.Create(.5f,
                                            (byte) random.Next(byte.MinValue, byte.MaxValue),
                                            (byte) random.Next(byte.MinValue, byte.MaxValue),
                                            (byte) random.Next(byte.MinValue, byte.MaxValue),
                                            (byte) random.Next(byte.MinValue, byte.MaxValue)),
                                        SciterColorStop.Create(1f,
                                            (byte) random.Next(byte.MinValue, byte.MaxValue),
                                            (byte) random.Next(byte.MinValue, byte.MaxValue),
                                            (byte) random.Next(byte.MinValue, byte.MaxValue),
                                            (byte) random.Next(byte.MinValue, byte.MaxValue)))
                                    .DrawLine(random.Next(byte.MinValue, args.Area.Width),
                                        random.Next(byte.MinValue, args.Area.Height),
                                        random.Next(byte.MinValue, args.Area.Width),
                                        random.Next(byte.MinValue, args.Area.Height))
                                    .RestoreState();
                            }
                        }

                        element?.Window?.Close();

                        return true;

                    });
                });
            
            _ = new TestableSciterHost(_sciterWindow)
                .SetBehaviorResolver(_mockBehaviorResolver.Object);
            
            _sciterWindow.Show();
            
            _sciterWindow.RootElement.AppendElement("body", elm => elm)
                .SetStyleValue("background", $"rgb({random.Next(byte.MinValue, byte.MaxValue)}, {random.Next(byte.MinValue, byte.MaxValue)}, {random.Next(byte.MinValue, byte.MaxValue)})")
                .SetStyleValue("behavior", behaviorName);

            SciterPlatform.RunMessageLoop();
            
            //Assert.NotNull(_sciterGraphics);
        }

        [Timeout(2500)]
        [TestCase("draw-polygon")]
        public void Polygon(string behaviorName)
        {
            var random = new Random();

            _mockBehaviorResolver.Setup(resolver => resolver.GetBehaviorHandler(behaviorName))
                .Returns(() =>
                {
                    return new DrawContentBehavior(_sciterWindow, (element, prms) =>
                    {
                        if (prms.DrawEvent != DrawEvent.Content)
                            return false;

                        using (var graphics = SciterGraphics.Create(prms.Handle))
                        {
                            for (int i = 0; i < byte.MaxValue; i++)
                            {
                                graphics.SaveState()
                                    .Translate(prms.Area.Left, prms.Area.Top)
                                    .SetFillColor(
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue))
                                    .DrawPolygon((g) =>
                                    {
                                        var result = new List<PolygonPoint>();
                                        for (var j = 0; j < random.Next(3, 12); j++)
                                        {
                                            result.Add(PolygonPoint.Create(
                                                random.Next(byte.MinValue, prms.Area.Width),
                                                random.Next(byte.MinValue, prms.Area.Height)));
                                        }

                                        return result;
                                    })
                                    .RestoreState();
                            }
                        }

                        element?.Window?.Close();

                        return true;

                    });
                });
            
            _ = new TestableSciterHost(_sciterWindow)
                .SetBehaviorResolver(_mockBehaviorResolver.Object);
            
            _sciterWindow.Show();

            var backgroundColor = random.Next(byte.MinValue, 80);
            
            _sciterWindow.RootElement.AppendElement("body", elm => elm)
                .SetStyleValue("background", $"rgb({backgroundColor}, {backgroundColor}, {backgroundColor})")
                .SetStyleValue("behavior", behaviorName);

            SciterPlatform.RunMessageLoop();
            
            //Assert.NotNull(_sciterGraphics);
        }
        
        [Timeout(2500)]
        [TestCase("draw-polyline-round-bevel", LineCapType.Round, LineJoinType.Bevel)]
        [TestCase("draw-polyline-round-miter", LineCapType.Round, LineJoinType.Miter)]
        [TestCase("draw-polyline-round-round", LineCapType.Round, LineJoinType.Round)]
        [TestCase("draw-polyline-round-miterOrBevel", LineCapType.Round, LineJoinType.MiterOrBevel)]

        [TestCase("draw-polyline-butt-bevel", LineCapType.Butt, LineJoinType.Bevel)]
        [TestCase("draw-polyline-butt-miter", LineCapType.Butt, LineJoinType.Miter)]
        [TestCase("draw-polyline-butt-round", LineCapType.Butt, LineJoinType.Round)]
        [TestCase("draw-polyline-butt-miterOrBevel", LineCapType.Butt, LineJoinType.MiterOrBevel)]

        [TestCase("draw-polyline-square-bevel", LineCapType.Square, LineJoinType.Bevel)]
        [TestCase("draw-polyline-square-miter", LineCapType.Square, LineJoinType.Miter)]
        [TestCase("draw-polyline-square-round", LineCapType.Square, LineJoinType.Round)]
        [TestCase("draw-polyline-square-miterOrBevel", LineCapType.Square, LineJoinType.MiterOrBevel)]

        [TestCase("draw-polyline-round-bevel", LineCapType.Round, LineJoinType.Bevel)]
        [TestCase("draw-polyline-round-miter", LineCapType.Round, LineJoinType.Miter)]
        [TestCase("draw-polyline-round-round", LineCapType.Round, LineJoinType.Round)]
        [TestCase("draw-polyline-round-miterOrBevel", LineCapType.Round, LineJoinType.MiterOrBevel)]
        public void Polyline(string behaviorName, LineCapType lineCap, LineJoinType lineJoin)
        {
            var random = new Random();

            _mockBehaviorResolver
                .Setup(resolver => resolver.GetBehaviorHandler(behaviorName))
                .Returns(() =>
                {
                    return new DrawContentBehavior(_sciterWindow, (element, prms) =>
                    {
                        if (prms.DrawEvent != DrawEvent.Content)
                            return false;

                        using (var graphics = SciterGraphics.Create(prms.Handle))
                        {
                            for (var i = 0; i < byte.MaxValue; i++)
                            {
                                graphics.SaveState()
                                    .Translate(prms.Area.Left, prms.Area.Top)
                                    .SetLineColor(
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue))
                                    .SetLineCap(lineCap)
                                    .SetLineJoin(lineJoin)
                                    .SetLineWidth(random.Next(2, 10))
                                    .DrawPolyline(() =>
                                    {
                                        var result = new List<PolylinePoint>();
                                        for (var j = 0; j < random.Next(3, 12); j++)
                                        {
                                            result.Add(PolylinePoint.Create(
                                                random.Next(byte.MinValue, prms.Area.Width),
                                                random.Next(byte.MinValue, prms.Area.Height)));
                                        }
                                        return result;
                                    })
                                    .RestoreState();
                            }
                        }

                        element?.Window?.Close();

                        return true;

                    });
                });
                
            _ = new TestableSciterHost(_sciterWindow)
                .SetBehaviorResolver(_mockBehaviorResolver.Object);
   
            _sciterWindow.Show();

            var backgroundColor = random.Next(byte.MinValue, 80);
            
            _sciterWindow.RootElement.AppendElement("body", elm => elm)
                .SetStyleValue("background", $"rgb({backgroundColor}, {backgroundColor}, {backgroundColor})")
                .SetStyleValue("behavior", behaviorName);

            SciterPlatform.RunMessageLoop();
            
            //Assert.NotNull(_sciterGraphics);
        }
        
        [Timeout(2500)]
        [TestCase("draw-ellipse")]
        public void DrawEllipse(string behaviorName)
        {
            var random = new Random();

            _mockBehaviorResolver.Setup(resolver => resolver.GetBehaviorHandler(behaviorName))
                .Returns(() =>
                {
                    return new DrawContentBehavior(_sciterWindow, (element, prms) =>
                    {
                        switch (prms.DrawEvent)
                        {
                            case DrawEvent.Content:
                                using (var graphics = SciterGraphics.Create(prms.Handle))
                                {
                                    for (int i = 0; i < byte.MaxValue; i++)
                                    {
                                        var color = SciterColor.Create(
                                            (byte) random.Next(byte.MinValue, byte.MaxValue),
                                            (byte) random.Next(byte.MinValue, byte.MaxValue),
                                            (byte) random.Next(byte.MinValue, byte.MaxValue),
                                            (byte) random.Next(byte.MinValue, byte.MaxValue));

                                        graphics.SaveState()
                                            .Translate(prms.Area.Left, prms.Area.Top)
                                            .SetLineColor(color)
                                            .SetLineWidth(random.Next(1, 4))
                                            .DrawEllipse(prms.Area.Width / 2f, prms.Area.Height / 2f,
                                                random.Next(byte.MinValue, prms.Area.Width / 2),
                                                random.Next(byte.MinValue, prms.Area.Height / 2))
                                            //.DrawPath(
                                            //    SciterPath
                                            //        .Create()
                                            //        .MoveTo(prms.area.Width / 2, prms.area.Height / 2)
                                            //        .ArcTo(random.Next(byte.MinValue, prms.area.Width), random.Next(byte.MinValue, prms.area.Height), random.Next(0, 360), random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue), false, true)
                                            //        .LineTo(random.Next(byte.MinValue, prms.area.Width / 2), random.Next(byte.MinValue, prms.area.Height / 2))
                                            //        .BezierCurveTo(random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, prms.area.Width / 2), random.Next(byte.MinValue, prms.area.Height / 2))
                                            //        .QuadraticCurveTo(random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, byte.MaxValue), random.Next(byte.MinValue, prms.area.Width / 2), random.Next(byte.MinValue, prms.area.Height / 2))
                                            //    , DrawPathMode.StrokeOnly)
                                            //.DrawEllipse(prms.area.Width / 2, prms.area.Height / 2, 50f, 50f)
                                            //.SetLineColor(SciterColor.Transparent)
                                            //.SetFillColor(color)
                                            //.DrawPath(
                                            //    SciterPath
                                            //        .Create()
                                            //        .MoveTo(prms.area.Width / 2, prms.area.Height / 2 - 50)
                                            //        .ArcTo(prms.area.Width / 2, prms.area.Height / 2, 90, 25,  25, false, true)
                                            //        .ArcTo(prms.area.Width / 2, prms.area.Height / 2 + 50, 90, 25,  25, false, false)
                                            //        .ArcTo(prms.area.Width / 2, prms.area.Height / 2 - 50, 90, 50,  50, false, false)
                                            //    , DrawPathMode.FillOnly)
//
                                            //.DrawEllipse(prms.area.Width / 2, prms.area.Height / 2 - 25, 6f, 6f)
                                            //.DrawEllipse(prms.area.Width / 2, prms.area.Height / 2 + 25, 6f, 6f)
                                            .RestoreState();
                                    }
                                }

                                break;
                            case DrawEvent.Background:
                            case DrawEvent.Foreground:
                            case DrawEvent.Outline:
                            default:
                                return false;
                        }

                        element?.Window?.Close();

                        return true;

                    });
                });
            
            _ = new TestableSciterHost(_sciterWindow)
                .SetBehaviorResolver(_mockBehaviorResolver.Object);
            
            _sciterWindow.Show();

            var backgroundColor = random.Next(byte.MinValue, 80);
            
            _sciterWindow.RootElement.AppendElement("body", elm => elm)
                .SetStyleValue("background", $"rgb({backgroundColor}, {backgroundColor}, {backgroundColor})")
                .SetStyleValue("behavior", behaviorName);

            SciterPlatform.RunMessageLoop();
            
            //Assert.NotNull(_sciterGraphics);
        }
        
        [Timeout(2500)]
        [TestCase("draw-rectangle")]
        public void DrawRectangle(string behaviorName)
        {
            var random = new Random();

            _mockBehaviorResolver
                .Setup(resolver => resolver.GetBehaviorHandler(behaviorName)).Returns(() =>
                {
                    return new DrawContentBehavior(_sciterWindow, (element, prms) =>
                    {
                        if (prms.DrawEvent != DrawEvent.Content)
                            return false;

                        using (var graphics = SciterGraphics.Create(prms.Handle))
                        {
                            for (int i = 0; i < byte.MaxValue; i++)
                            {
                                graphics.SaveState()
                                    .Translate(prms.Area.Left, prms.Area.Top)
                                    .SetLineColor(
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue))
                                    .SetFillColor(
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue))
                                    .SetLineWidth(random.Next(2, 10))
                                    .DrawRectangle(random.Next(byte.MinValue, prms.Area.Width),
                                        random.Next(byte.MinValue, prms.Area.Height),
                                        random.Next(byte.MinValue, prms.Area.Width),
                                        random.Next(byte.MinValue, prms.Area.Height))
                                    .RestoreState();
                            }
                        }

                        element?.Window?.Close();

                        return true;

                    });
                });
            
            _ = new TestableSciterHost(_sciterWindow)
                .SetBehaviorResolver(_mockBehaviorResolver.Object);
 
            _sciterWindow.Show();

            var backgroundColor = random.Next(byte.MinValue, 80);
            
            _sciterWindow.RootElement.AppendElement("body", elm => elm)
                .SetStyleValue("background", $"rgb({backgroundColor}, {backgroundColor}, {backgroundColor})")
                .SetStyleValue("behavior", behaviorName);

            SciterPlatform.RunMessageLoop();
            
            //Assert.NotNull(_sciterGraphics);
        }
        
        [Timeout(2500)]
        [TestCase("draw-line")]
        public void DrawLine(string behaviorName)
        {
            var random = new Random();

            _mockBehaviorResolver
                .Setup(resolver => resolver.GetBehaviorHandler(behaviorName)).Returns(() =>
                {
                    return new DrawContentBehavior(_sciterWindow, (element, prms) =>
                    {
                        if (prms.DrawEvent != DrawEvent.Content)
                            return false;

                        using (var graphics = SciterGraphics.Create(prms.Handle))
                        {
                            for (var i = 0; i < byte.MaxValue; i++)
                            {
                                graphics.SaveState()
                                    .Translate(prms.Area.Left, prms.Area.Top)
                                    .SetLineColor((byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue))
                                    .SetFillColor((byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue))
                                    .SetLineWidth(random.Next(2, 10))
                                    .DrawText(
                                        SciterText.CreateForElementAndStyle(
                                            "The quick brown fox jumps over the lazy dog", element.Handle,
                                            $"color: rgba({random.Next(byte.MinValue, byte.MaxValue)}, {random.Next(byte.MinValue, byte.MaxValue)}, {random.Next(byte.MinValue, byte.MaxValue)});font-size: {random.Next(12, 48)}dip;"),
                                        random.Next(byte.MinValue, prms.Area.Width),
                                        random.Next(byte.MinValue, prms.Area.Height), 5)
                                    .RestoreState();
                            }
                        }

                        element?.Window?.Close();

                        return true;

                    });
                });
                        
            _ = new TestableSciterHost(_sciterWindow)
                .SetBehaviorResolver(_mockBehaviorResolver.Object);

            _sciterWindow.Show();
            
            _sciterWindow.RootElement.AppendElement("body", elm => elm)
                .SetStyleValue("background-color", $"black")
                .SetStyleValue("behavior", behaviorName);

            SciterPlatform.RunMessageLoop();
            
            //Assert.NotNull(_sciterGraphics);
        }
        
        [Timeout(2500)]
        [TestCase("draw-lines-linear-gradient")]
        public void Draw_line_with_linear_gradient(string behaviorName)
        {
            var random = new Random();

            _mockBehaviorResolver
                .Setup(resolver => resolver.GetBehaviorHandler(behaviorName)).Returns(() =>
            {
                return new DrawContentBehavior(_sciterWindow, (element, args) =>
                {
                    if (args.DrawEvent != DrawEvent.Content)
                        return false;

                    using (var graphics = SciterGraphics.Create(args.Handle))
                    {
                        for (var i = 0; i < 10; i++)
                        {
                            graphics.SaveState()
                                .Translate(args.Area.Left, args.Area.Top)
                                .SetLineWidth(random.Next(5, 15))
                                .SetLineGradientLinear(
                                    0f,
                                    0f,
                                    args.Area.Width,
                                    args.Area.Height,
                                    SciterColorStop.Create(0f, Color.Aqua),

                                    SciterColorStop.Create(.25f,
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (float) random.NextDouble()),
                                    SciterColorStop.Create(.5f,
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue)),
                                    SciterColorStop.Create(.75f,
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue)),
                                    SciterColorStop.Create(1f, SciterColor.Lime))

                                .DrawLine(random.Next(byte.MinValue, args.Area.Width),
                                    random.Next(byte.MinValue, args.Area.Height),
                                    random.Next(byte.MinValue, args.Area.Width),
                                    random.Next(byte.MinValue, args.Area.Height));

                            graphics.SaveState()
                                .Translate((args.Area.Right - args.Area.Left) / 2f,
                                    (args.Area.Height - args.Area.Top) / 2f)

                                .SetLineGradientLinear(
                                    0f,
                                    0f,
                                    args.Area.Width,
                                    args.Area.Height,
                                    SciterColorStop.Create(0f, Color.Orange),

                                    SciterColorStop.Create(.25f,
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (float) random.NextDouble()),
                                    SciterColorStop.Create(.5f,
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue)),
                                    SciterColorStop.Create(.75f,
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue)),
                                    SciterColorStop.Create(1f, SciterColor.Indigo))

                                .DrawEllipse(0,
                                    random.Next(0),
                                    random.Next(byte.MinValue, args.Area.Width / 2),
                                    random.Next(byte.MinValue, args.Area.Height / 2))

                                .DrawRectangle(random.Next(byte.MinValue, args.Area.Width),
                                    random.Next(byte.MinValue, args.Area.Height),
                                    random.Next(byte.MinValue, args.Area.Width / 2),
                                    random.Next(byte.MinValue, args.Area.Height / 2))

                                .RestoreState();

                            graphics.SaveState()
                                .Translate(args.Area.Left, args.Area.Top)

                                .SetLineGradientLinear(
                                    0f,
                                    0f,
                                    args.Area.Width,
                                    args.Area.Height,
                                    SciterColorStop.Create(0f, Color.Coral),

                                    SciterColorStop.Create(.25f,
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (float) random.NextDouble()),
                                    SciterColorStop.Create(.5f,
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue)),
                                    SciterColorStop.Create(.75f,
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue),
                                        (byte) random.Next(byte.MinValue, byte.MaxValue)),
                                    SciterColorStop.Create(1f, SciterColor.Magenta))

                                .DrawRectangle(random.Next(byte.MinValue, args.Area.Width),
                                    random.Next(byte.MinValue, args.Area.Height),
                                    random.Next(byte.MinValue, args.Area.Width / 2),
                                    random.Next(byte.MinValue, args.Area.Height / 2))

                                .RestoreState();

                            graphics.RestoreState();
                        }
                    }

                    element?.Window?.Close();

                    return true;
                });
            });
                
            _ = new TestableSciterHost(_sciterWindow)
                .SetBehaviorResolver(_mockBehaviorResolver.Object);
            
            _sciterWindow.Show();
            
            _sciterWindow.RootElement.AppendElement("body", elm => elm)
                .SetStyleValue("background-color", $"rgb({random.Next(byte.MinValue, byte.MaxValue)}, {random.Next(byte.MinValue, byte.MaxValue)}, {random.Next(byte.MinValue, byte.MaxValue)})")
                .SetStyleValue("behavior", behaviorName);

            SciterPlatform.RunMessageLoop();
        }

    }
}