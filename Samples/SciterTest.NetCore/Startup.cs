using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SciterCore;
using SciterCore.Interop;

namespace SciterTest.NetCore
{
    public class Startup : IHostedService
    {
        public static ApplicationHost<Window> ApplicationHost { get; private set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //MessageBox.Show (IntPtr.Zero, "ola", "mnundo");

#if WINDOWS || NETCOREAPP
			// Sciter needs this for drag'n'drop support
            SciterPlatform.EnableDragAndDrop();
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

            // Prepares SciterHost and then load the page
            ApplicationHost = new ApplicationHost<Window>(() => new Window());
            
            // Run message loop
            PInvokeUtils.RunMsgLoop();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new System.NotImplementedException();
        }
    }
}