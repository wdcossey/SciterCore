#if WINDOWS || GTKMONO
using System;
using System.Diagnostics;
using SciterCore.Interop;
using SciterCore;

namespace SciterTest.Gtk
{
	class Program
	{
		public static Window AppWindow { get; private set; }// must keep a reference to survive GC
		public static Host AppHost { get; private set; }

		[STAThread]
		static void Main(string[] args)
		{
			MessageBox.Show (IntPtr.Zero, "ola", "mnundo");

			// Platform specific (required for GTK)
			SciterPlatform.Initialize();
			// Sciter needs this for drag 'n drop support
			SciterPlatform.EnableDragAndDrop();

#if GTKMONO
			Mono.Setup();
#endif

			/*
				NOTE:
				In Linux, if you are getting a System.TypeInitializationException below, it is because you don't have 'libsciter-gtk-64.so' in your LD_LIBRARY_PATH.
				Run 'sudo bash install-libsciter.sh' contained in this package to install it in your system.
			*/
			// Create the window
			AppWindow = new Window();

			// Prepares SciterHost and then load the page
			AppHost = new Host(AppWindow);

			// Run message loop
			SciterPlatform.RunMessageLoop();
		}
	}
}
#endif