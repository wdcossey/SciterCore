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

	        // Platform specific (required for GTK)
	        SciterPlatform.Initialize();
	        // Sciter needs this for drag 'n drop support
	        SciterPlatform.EnableDragAndDrop();

            Console.WriteLine($@"Sciter: {Sciter.SciterApi.SciterVersion()}");

			// Create the window
			var window = new SciterWindow()
				.CreateMainWindow(1500, 800)
				.CenterWindow()
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
            
            SciterPlatform.RunMessageLoop();

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}
}