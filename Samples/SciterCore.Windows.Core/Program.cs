using System;
using System.Collections.Generic;
using System.Diagnostics;
using SciterCore.Interop;

namespace SciterCore.Windows.Core
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
		public static AppHost AppHost;

		[STAThread]
		static void Main(string[] args)
		{
			var list = new List<int> { 123 };

			var ss = SciterValue.Create(new { aa = list });

			Console.WriteLine($@"Sciter: {Sciter.SciterApi.SciterVersion()}");
			Console.WriteLine("Bitness: " + IntPtr.Size);

			// Platform specific (required for GTK)
			SciterPlatform.Initialize();
			// Sciter needs this for drag 'n drop support
			SciterPlatform.EnableDragAndDrop();
			
			// Create the window
			AppWindow = new SciterWindow()
				.CreateMainWindow(800, 600)
                .CenterWindow()
                .SetTitle("SciterCore.Windows::Core")
                .SetIcon(SciterTest.Core.Properties.Resources.IconMain);
			
			// Prepares SciterHost and then load the page
			AppHost = new AppHost(AppWindow);

			AppHost
				.SetupWindow(AppWindow)
				.AttachEventHandler(new AppEventHandler());

			AppHost.SetupPage("index.html");
			
			//AppHost.ConnectToInspector();

			//byte[] css_bytes = File.ReadAllBytes(@"D:\ProjetosSciter\AssetsDrop\AssetsDrop\res\css\global.css");
			//SciterX.API.SciterAppendMasterCSS(css_bytes, (uint) css_bytes.Length);
			Debug.Assert(!AppHost.EvalScript("Utils").IsUndefined);

			// Show window and Run message loop
			AppWindow.Show();
			SciterPlatform.RunMessageLoop();
		}
	}
}