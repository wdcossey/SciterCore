using System;
using NUnit.Framework;
using SciterCore.Attributes;
using SciterCore.Interop;

namespace SciterCore.Tests.Unit.Graphics
{
    public class SciterGraphicsTests
    {
        private SciterWindow _sciterWindow;


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
        
        [SetUp]
        public void Setup()
        {
            _sciterWindow = 
                new SciterWindow()
                    .CreateMainWindow(320, 240)
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

        
    }
}