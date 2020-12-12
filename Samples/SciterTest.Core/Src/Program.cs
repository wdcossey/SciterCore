using SciterCore;
using SciterCore.Interop;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.Core
{
	class Program
	{
		class SciterMessages : SciterDebugOutputHandler
		{

			public SciterMessages()
			{

			}

			protected override void OnOutput(SciterXDef.OUTPUT_SUBSYTEM subsystem, SciterXDef.OUTPUT_SEVERITY severity, string text)
			{
				Console.WriteLine(text);
				//Debug.Write(text);// so I can see Debug output even if 'native debugging' is off
			}
		}

		public static SciterWindow AppWindow;
		public static Host AppHost;

		[STAThread]
		static void Main(string[] args)
		{
			var list = new List<int> { 123 };

			var ss = SciterValue.Create(new { aa = list });

			Console.WriteLine($@"Sciter: {Sciter.Api.SciterVersion()}");
			Console.WriteLine("Bitness: " + IntPtr.Size);

			// Sciter needs this for drag'n'drop support
			SciterPlatform.EnableDragAndDrop();
			
			// Create the window
			AppWindow = new SciterWindow()
				.CreateMainWindow(800, 600)
                .CenterWindow()
                .SetTitle("SciterTest.Core")
                .SetIcon(Properties.Resources.IconMain);
			
			// Prepares SciterHost and then load the page
			AppHost = new Host();
			var host = AppHost;
			host.Setup(AppWindow);
			host.AttachEventHandler(new HostEventHandler());
			host.SetupPage("index.html");
			//host.DebugInspect();

			//byte[] css_bytes = File.ReadAllBytes(@"D:\ProjetosSciter\AssetsDrop\AssetsDrop\res\css\global.css");
			//SciterX.API.SciterAppendMasterCSS(css_bytes, (uint) css_bytes.Length);
			Debug.Assert(!host.EvalScript("Utils").IsUndefined);

			// Show window and Run message loop
			AppWindow.Show();
			PInvokeUtils.RunMsgLoop();
		}
	}
}