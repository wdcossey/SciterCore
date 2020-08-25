using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using SciterCore.Interop;

namespace SciterCore.UnitTests
{
	public class UnitTests
	{
		[Test]
		public void TestSciterElement()
		{
			SciterElement el = SciterElement.Create("div");
			SciterElement el2 = new SciterElement(el.Handle);
			Assert.IsTrue(el == el2);
		}

		[Test]
		public void TestSciterValue()
		{
			//string[] arr = new string[] { "A", "B", "C" };
			int[] arr = new int[] { 1, 2, 3 };
			//SciterValue res = SciterValue.FromList(arr);
			SciterCore.SciterValue res = new SciterCore.SciterValue();
			res.Append(new SciterCore.SciterValue(1));
			res.Append(new SciterCore.SciterValue(1));
			res.Append(new SciterCore.SciterValue(1));
			string r = res.ToString();
			string r2 = res.ToString();
			string r3 = res.ToJsonString(SciterCore.Interop.SciterValue.VALUE_STRING_CVT_TYPE.CVT_JSON_LITERAL);

			{
				// http://sciter.com/forums/topic/erasing-sequence-elements-with-scitervalue/
				SciterCore.SciterValue sv = SciterCore.SciterValue.FromJsonString("[1,2,3,4,5])");
				sv[0] = SciterCore.SciterValue.Undefined;
				sv[2] = SciterCore.SciterValue.Undefined;

				SciterCore.SciterValue sv2 = SciterCore.SciterValue.FromJsonString("{one: 1, two: 2, three: 3}");
				sv2["one"] = SciterCore.SciterValue.Undefined;
				Assert.IsTrue(sv2["two"].Get(0)==2);
			}

			// Datetime
			{
				var now = DateTime.Now;
				SciterCore.SciterValue sv = new SciterCore.SciterValue(now);
				Assert.IsTrue(sv.GetDate() == now);
			}

			// SciterValue.AsDictionary
			{
				SciterCore.SciterValue sv = SciterCore.SciterValue.FromJsonString("{ a: 1, b: true }");
				var dic = sv.AsDictionary();
				Assert.IsTrue(dic.Count == 2);
			}
		}

		[Test]
		public void TestGraphics()
		{
			var gapi = Sciter.GraphicsApi;

			IntPtr himg;
			var a = gapi.imageCreate(out himg, 400, 400, true);

			IntPtr hgfx;
			gapi.gCreate(himg, out hgfx);

			IntPtr hpath;
			gapi.pathCreate(out hpath);

			var b = gapi.gPushClipPath(hgfx, hpath, 0.5f);
			var c = gapi.gPopClip(hgfx);

			// RGBA
			var rgba = new SciterColor(1, 2, 3, 4);
			Assert.AreEqual(1, rgba.R);
			Assert.AreEqual(2, rgba.G);
			Assert.AreEqual(3, rgba.B);
			Assert.AreEqual(4, rgba.A);
		}

		[Test]
		public void TestColor_Invalid()
		{			
			var invalid = SciterColors.Invalid;

			var rgba = new SciterColor(-1, -255, -1000);
			Assert.AreEqual(invalid.Value, rgba.Value);
		}

		private class TestableDebugOutputHandler : SciterDebugOutputHandler
		{
            private readonly SciterWindow _window;

            public TestableDebugOutputHandler(SciterWindow window)
			    :base()
            {
                _window = window;
            }

			public List<Tuple<SciterXDef.OUTPUT_SUBSYTEM, SciterXDef.OUTPUT_SEVERITY, string>> msgs = new List<Tuple<SciterXDef.OUTPUT_SUBSYTEM, SciterXDef.OUTPUT_SEVERITY, string>>();

            protected override void OnOutput(SciterXDef.OUTPUT_SUBSYTEM subsystem, SciterXDef.OUTPUT_SEVERITY severity, string text)
			{
				msgs.Add(Tuple.Create(subsystem, severity, text));

				Thread.Sleep(1000);
				_window.Close();
			}
		}

		[Test]
		public void TestThings()
		{
			SciterWindow window = 
                new SciterWindow()
                    .CreateMainWindow(640, 480)
                    .SetTitle("Wtf");
			_ = new SciterHost(window);
			window.LoadHtml("<html></html>");

			var sv = window.EvalScript("Utils.readStreamToEnd");
			Assert.IsTrue(!sv.IsUndefined);
		}

		[Test]
		[Ignore("")]
		public void TestDebugOutputHandler()
        {
            SciterWindow window = new SciterWindow()
	            .CreateMainWindow(640, 480)
	            .SetTitle("Wtf");

            TestableDebugOutputHandler odh = new TestableDebugOutputHandler(window: window);

            window
                
                .LoadHtml(@"
<html>
<style>
	body { wtf: 123; }
</style>
<script type='text/tiscript'>
</script>
</html>",
                out var loadResult);

			Assert.IsTrue(condition: loadResult);

			PInvokeWindows.MSG msg;
            while(PInvokeWindows.GetMessage(lpMsg: out msg, hWnd: IntPtr.Zero, wMsgFilterMin: 0, wMsgFilterMax: 0) != 0)
			{
                if (!window.IsVisible)
                {
                    window.Show();
                }

                PInvokeWindows.TranslateMessage(ref msg);
				PInvokeWindows.DispatchMessage(ref msg);
			}

			Assert.AreEqual(1, odh.msgs.Count);
		}
	}
}