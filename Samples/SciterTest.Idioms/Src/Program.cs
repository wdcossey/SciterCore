using SciterCore;
using SciterCore.Interop;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciterTest.Idioms
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			// Sciter needs this for drag'n'drop support; STAThread is required for OleInitialize succeess
			int oleres = PInvokeWindows.OleInitialize(IntPtr.Zero);
			Debug.Assert(oleres == 0);
			
			// Create the window
			var window = new SciterWindow()
				.CreateMainWindow(1500, 800)
				.CenterTopLevelWindow()
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