using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SciterCore;
using SciterCore.Interop;

namespace SciterTest.NetCore
{
    public class Startup : IHostedService
    {
        //public static Window AppWindow { get; private set; }// must keep a reference to survive GC
        public static Host AppHost { get; private set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //MessageBox.Show (IntPtr.Zero, "ola", "mnundo");

#if WINDOWS || NETCOREAPP
			// Sciter needs this for drag'n'drop support; STAThread is required for OleInitialize succeess
			//int oleres = PInvokeWindows.OleInitialize(IntPtr.Zero);
			//Debug.Assert(oleres == 0);
#endif
#if GTKMONO
			PInvokeGTK.gtk_init(IntPtr.Zero, IntPtr.Zero);
			Mono.Setup();
#endif

            /*
                NOTE:
                In Linux, if you are getting a System.TypeInitializationException below, it is because you don't have 'libsciter-gtk-64.so' in your LD_LIBRARY_PATH.
                Run 'sudo bash install-libsciter.sh' contained in this package to install it in your system.
            */
            // Create the window
            //AppWindow = new Window();

            // Prepares SciterHost and then load the page
            //AppHost = new Host(AppWindow);


            // Run message loop
            PInvokeUtils.RunMsgLoop();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new System.NotImplementedException();
        }
    }
}