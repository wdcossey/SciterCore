using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SciterCore.Interop;

namespace SciterCore.Tests.Unit
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
			res.Append(SciterValue.Create(1));
			res.Append(SciterValue.Create(1));
			res.Append(SciterValue.Create(1));
			res.Append(SciterValue.MakeSymbol("symbol").Append(SciterValue.Create(1)));
			string r = res.ToString();
			string r2 = res.ToString();
			string r3 = res.AsJsonString(StringConversionType.JsonLiteral);

			{
				// http://sciter.com/forums/topic/erasing-sequence-elements-with-scitervalue/
				SciterCore.SciterValue sv = SciterCore.SciterValue.FromJsonString("[1,2,3,4,5])");
				sv[0] = SciterCore.SciterValue.Undefined;
				sv[2] = SciterCore.SciterValue.Undefined;

				SciterCore.SciterValue sv2 = SciterCore.SciterValue.FromJsonString("{one: 1, two: 2, three: 3}");
				sv2["one"] = SciterCore.SciterValue.Undefined;
				Assert.IsTrue(sv2["two"].AsInt32(0)==2);
			}

			// Datetime
			{
				var now = DateTime.Now;
				SciterValue sv = SciterValue.Create(now);
				Assert.IsTrue(sv.AsDateTime(false) == now);
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
			var a = gapi.ImageCreate(out himg, 400, 400, true);

			IntPtr hgfx;
			gapi.GraphicsCreate(himg, out hgfx);

			IntPtr hpath;
			gapi.PathCreate(out hpath);

			var b = gapi.GraphicsPushClipPath(hgfx, hpath, 0.5f);
			var c = gapi.GraphicsPopClip(hgfx);

			// RGBA
			var actual = SciterColor.Create(1, 2, 3, 4);
			Assert.AreEqual(1, actual.R);
			Assert.AreEqual(2, actual.G);
			Assert.AreEqual(3, actual.B);
			Assert.AreEqual(4, actual.A);
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

		private class TestableDebugOutputHandler : SciterDebugOutputHandler
		{
			// ReSharper disable once SuggestBaseTypeForParameter
			public TestableDebugOutputHandler(SciterWindow window)
				:base(window.Handle)
			{
				
			}

			public readonly List<(SciterXDef.OUTPUT_SUBSYTEM subsystem, SciterXDef.OUTPUT_SEVERITY severity, string text)> Messages = new List<(SciterXDef.OUTPUT_SUBSYTEM subsystem, SciterXDef.OUTPUT_SEVERITY severity, string text)>();

			public event EventHandler<System.EventArgs> OnMessage;
			
			protected override void OnOutput(SciterXDef.OUTPUT_SUBSYTEM subsystem, SciterXDef.OUTPUT_SEVERITY severity, string text)
			{
				Messages.Add((subsystem, severity, text));
				OnMessage?.Invoke(this, System.EventArgs.Empty);
			}
		}
		
		[Test]
		public void TestDebugOutputHandler()
        {
	        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

	        Assert.DoesNotThrowAsync(() =>
		        Task.Run(() =>
		        {
			        SciterWindow window = new SciterWindow()
				        .CreateMainWindow(640, 480)
				        .SetTitle(nameof(TestDebugOutputHandler));

			        var odh = new TestableDebugOutputHandler(window: window);

			        odh.OnMessage += (sender, tuple) =>
			        {
				        window.Close();
			        };
		            
			        var loadResult = window
				        .TryLoadHtml(
					        @"<html>
	<script type='text/tiscript'>
		function self.ready() {
			throw 'throw an exception!';
		}   
	</script>
	</html>");

			        Assert.IsTrue(condition: loadResult);

			        while(PInvokeWindows.GetMessage(lpMsg: out var msg, hWnd: IntPtr.Zero, wMsgFilterMin: 0, wMsgFilterMax: 0) != 0)
			        {
				        //if (!window.IsVisible)
					    //    window.Show();

				        PInvokeWindows.TranslateMessage(ref msg);
				        PInvokeWindows.DispatchMessage(ref msg);
			        }

			        Assert.NotNull(odh.Messages);
			        Assert.GreaterOrEqual(odh.Messages.Count, 1);
				
			        Assert.IsTrue(odh.Messages.Any(tuple => 
				        tuple.severity == SciterXDef.OUTPUT_SEVERITY.OS_ERROR && 
				        tuple.subsystem == SciterXDef.OUTPUT_SUBSYTEM.OT_TIS && 
				        tuple.text.Equals("throw an exception!\n")));
		        }, cts.Token));
        }
	}
}