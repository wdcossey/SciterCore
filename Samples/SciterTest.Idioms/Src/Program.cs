using SciterCore;
using SciterCore.Interop;
using System;

namespace SciterTest.Idioms
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			// Sciter needs this for drag'n'drop support
			SciterPlatform.EnableDragAndDrop();
			
			// Create the window
			var window = new SciterWindow()
				.CreateMainWindow(1500, 800)
				.CenterWindow()
				.SetTitle("SciterTest.Idioms")
				.SetIcon(Properties.Resources.IconMain);

			// Prepares SciterHost and then load the page
			var host = new Host(window: window);
			host.AttachEventHandler(new HostEventHandler());
			host.SetupPage("index.html");

			// Show window and Run message loop
			window.Show();
			PInvokeUtils.RunMsgLoop();
		}
	}
}