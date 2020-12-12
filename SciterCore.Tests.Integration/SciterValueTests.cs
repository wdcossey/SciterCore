using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SciterCore.Attributes;
using SciterCore.Interop;

namespace SciterCore.Tests.Integration
{
    
    public class SciterValueTests
    {
        private SciterWindow _sciterWindow;
        
        [SetUp]
        public void Setup()
        {
            _sciterWindow = 
                new SciterWindow()
                    .CreateMainWindow(320, 240)
                    //.CenterTopLevelWindow()
                    .SetTitle(nameof(SciterGraphicsTests));
        }

        [TearDown]
        public void TearDown()
        {
            
        }
        
        private void TranslateAndDispatch(CancellationToken token)
        {
            while(!token.IsCancellationRequested && PInvokeWindows.GetMessage(lpMsg: out var msg, hWnd: IntPtr.Zero, wMsgFilterMin: 0, wMsgFilterMax: 0) != 0)
            {
                PInvokeWindows.TranslateMessage(ref msg);
                PInvokeWindows.DispatchMessage(ref msg);
            }
        }
        
        class TestEventHandler : SciterEventHandler
        {
            private readonly Action<SciterElement, MethodInfo, SciterValue[], ScriptEventResult> _onScriptCallCallback;
            private readonly Action<SciterWindow> _functionCallback;

            public TestEventHandler(
                Action<SciterElement, MethodInfo, SciterValue[], ScriptEventResult> onScriptCallCallback = null, 
                Action<SciterWindow> functionCallback = null)
            {
                _onScriptCallCallback = onScriptCallCallback;
                _functionCallback = functionCallback;
            }

            public void SyncFunction(SciterElement element)
            {
                _functionCallback?.Invoke(Element.Window);
            }

            public Task AsyncFunction(SciterElement element)
            {
                _functionCallback?.Invoke(Element.Window);
                return Task.CompletedTask;
            }

            [SciterFunctionName("customFunction")]
            public void AliasFunction(SciterElement element)
            {
                _functionCallback?.Invoke(Element.Window);
            }

            public bool InvalidFunction(SciterElement element)
            {
                return false;
            }
            
            protected override bool OnEvent(SciterElement sourceElement, SciterElement targetElement, SciterBehaviors.BEHAVIOR_EVENTS type, IntPtr reason,
                SciterValue data, string eventName)
            {
                return base.OnEvent(sourceElement, targetElement, type, reason, data, eventName);
            }

            protected override ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
            {
                var result = base.OnScriptCall(element, method, args);
                _onScriptCallCallback?.Invoke(element, method, args, result);
                return result;
            }
            
        }
        
        class TestHost : SciterHost
        {
            private readonly SciterWindow _window;
            
            public TestHost(SciterWindow window)
            : base(window)
            {
                _window = window;
            }

            protected override LoadResult OnLoadData(object sender, LoadDataArgs args)
            {
                return base.OnLoadData(sender, args);
                
            }
        }
        
        [TestCase(nameof(TestEventHandler.SyncFunction))]
        [TestCase(nameof(TestEventHandler.AsyncFunction))]
        public async Task Ensure_the_function_is_executed_when_the_page_loads(string methodName)
        {
            var cSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            string actualMethodName = null;
            
            await Task.Run(() =>
            {
                var sciterWindow = 
                    new SciterWindow()
                        .CreateMainWindow(320, 240)
                        .CenterWindow()
                        .SetTitle(nameof(SciterGraphicsTests));
                
                var host = new TestHost(sciterWindow);

                host.AttachEventHandler(() => new TestEventHandler(
                    (element, info, args, result) => { actualMethodName = info.Name;  },
                    (window) => { window?.Close(); }));

                var pageData = "<html><head><style></style>" +
                               "<script type=\"text/tiscript\"> " +
                               "function self.ready() {" +
                               $"view.{methodName}();" +
                               "}" +
                               "</script>" +
                               "</head></html>";
                
                host.Window.LoadHtml(pageData);

                host.Window.Show();

                TranslateAndDispatch(cSource.Token);
                
            }, cSource.Token);

            Assert.NotNull(actualMethodName);
            Assert.AreEqual(methodName, actualMethodName);
            Assert.IsFalse(cSource.IsCancellationRequested);
        }
        
        [TestCase(nameof(TestEventHandler.AliasFunction), "customFunction")]
        public async Task Ensure_the_function_is_executed_using_an_alias(string methodName, string aliasName)
        {
            var cSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            string actualMethodName = null;
            var scriptEventResult = ScriptEventResult.Successful();

            await Task.Run(() =>
            {
                var sciterWindow = 
                    new SciterWindow()
                        .CreateMainWindow(320, 240)
                        .CenterWindow()
                        .SetTitle(nameof(SciterGraphicsTests));
                
                var host = new TestHost(sciterWindow);

                host.AttachEventHandler(() => new TestEventHandler(
                    onScriptCallCallback: (element, info, args, result) =>
                    {
                        actualMethodName = info.Name;
                        scriptEventResult = result;
                        sciterWindow?.Close();
                    },
                    (window) => { window?.Close(); }));

                var pageData = "<html><head><style></style>" +
                               "<script type=\"text/tiscript\"> " +
                               $"view.{aliasName}();" +
                               "</script>" +
                               "</head></html>";
                
                host.Window.LoadHtml(pageData);

                host.Window.Show();

                TranslateAndDispatch(cSource.Token);
                
            }, cSource.Token);

            Assert.NotNull(actualMethodName);
            Assert.AreEqual(methodName, actualMethodName);
            Assert.IsTrue(scriptEventResult.IsSuccessful);
            Assert.IsFalse(cSource.IsCancellationRequested);
        }
        
        [TestCase(nameof(TestEventHandler.InvalidFunction))]
        public async Task Invalid_return_type_on_method_fails_to_execute(string methodName)
        {
            var cSource = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            string actualMethodName = null;
            var scriptEventResult = ScriptEventResult.Successful();
            
            await Task.Run(() =>
            {
                var sciterWindow = 
                    new SciterWindow()
                        .CreateMainWindow(320, 240)
                        .CenterWindow()
                        .SetTitle(nameof(SciterGraphicsTests));
                
                var host = new TestHost(sciterWindow);

                host.AttachEventHandler(() => new TestEventHandler(
                    onScriptCallCallback: (element, info, args, result) =>
                    {
                        actualMethodName = info.Name;
                        scriptEventResult = result;
                        sciterWindow?.Close();
                    }));

                var pageData = "<html><head><style></style>" +
                               "<script type=\"text/tiscript\"> " +
                               $"view.{methodName}();" +
                               "</script>" +
                               "</head></html>";
                
                host.Window.LoadHtml(pageData);

                host.Window.Show();

                TranslateAndDispatch(cSource.Token);
                
            }, cSource.Token);

            Assert.NotNull(actualMethodName);
            Assert.AreEqual(methodName, actualMethodName);
            Assert.IsFalse(scriptEventResult.IsSuccessful);
            Assert.IsNull(scriptEventResult.Value);
            Assert.IsFalse(cSource.IsCancellationRequested);
        }
    }
}