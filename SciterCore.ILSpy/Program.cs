using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SciterCore.Interop;

namespace SciterCore.ILSpy
{
    class Program
    {
        public static Window AppWindow { get; private set; }// must keep a reference to survive GC
        public static Host AppHost { get; private set; }

        [STAThread]
        static void Main(string[] args)
        {

#if WINDOWS || NETCOREAPP
            // Sciter needs this for drag'n'drop support; STAThread is required for OleInitialize succeess
            int oleres = PInvokeWindows.OleInitialize(IntPtr.Zero);
            Debug.Assert(oleres == 0);
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
            AppWindow = new Window();

            // Prepares SciterHost and then load the page
            AppHost = new Host(AppWindow);


            // Run message loop
            PInvokeUtils.RunMsgLoop();

            //var host = new HostBuilder()
            //    .ConfigureHostConfiguration(configHost =>
            //    {
            //        configHost.SetBasePath(Directory.GetCurrentDirectory());
            //        //configHost.AddJsonFile(_hostsettings, optional: true);
            //        //configHost.AddEnvironmentVariables(prefix: _prefix);
            //        configHost.AddCommandLine(args);
            //    })
            //    .ConfigureAppConfiguration((hostContext, configApp) =>
            //    {
            //        configApp.SetBasePath(Directory.GetCurrentDirectory());
            //        //configApp.AddJsonFile(_appsettings, optional: true);
            //        configApp.AddJsonFile(
            //            $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
            //            optional: true);
            //        //configApp.AddEnvironmentVariables(prefix: _prefix);
            //        configApp.AddCommandLine(args);
            //    })
            //    .ConfigureServices((hostContext, services) =>
            //    {
            //        services.AddLogging();
            //        services.AddHostedService<Startup>();

            //    })
            //    .ConfigureLogging((hostContext, configLogging) =>
            //    {
            //        configLogging.AddConsole();

            //    })
            //    .UseConsoleLifetime()
            //    .Build();


            //await host.RunAsync();

        }
    }
}
