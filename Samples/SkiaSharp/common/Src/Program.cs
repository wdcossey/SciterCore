using System;
using System.Diagnostics;
using SciterCore;
using SciterCore.Interop;

namespace SciterTest.Graphics
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
#if WINDOWS
            // Sciter needs this for drag'n'drop support; STAThread is required for OleInitialize succeess
            int oleres = PInvokeWindows.OleInitialize(IntPtr.Zero);
			Debug.Assert(oleres == 0);
#endif

			Console.WriteLine("Sciter: " + Sciter.Version());

			// Create the window
			var window = new SciterWindow()
				.CreateMainWindow(1500, 800)
				.CenterTopLevelWindow()
				.SetTitle("SciterCore::Framework::Graphics::SkiaSharp");
#if WINDOWS
			window.SetIcon(Properties.Resources.IconMain);
#endif

			// Prepares SciterHost and then load the page
			var host = new Host(window: window);
            /*
			host.RegisterBehaviorHandler(typeof(DrawBitmapBehavior), "DrawBitmap")
				.RegisterBehaviorHandler(typeof(DrawTextBehavior), "DrawText")
				.RegisterBehaviorHandler(typeof(DrawGeometryBehavior), "DrawGeometry")
				.AttachEventHandler(new HostEventHandler());

			host.SetupPage("index.html");

			// Show window and Run message loop
			window.Show();
            */
			PInvokeUtils.RunMsgLoop();

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}
}