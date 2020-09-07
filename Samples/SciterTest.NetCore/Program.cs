using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SciterCore;
using SciterCore.Interop;

namespace SciterTest.NetCore
{
    class Program
    {
        [STAThread]
        static async Task Main(string[] args)
        {
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                //throw new ThreadStateException(SR.Get(SRID.OleServicesContext_ThreadMustBeSTA));
            }
            
            var oleThread = new Thread(() => 
            {
                var oleResult = PInvokeWindows.OleInitialize(IntPtr.Zero);
                Debug.Assert(oleResult == 0);
            });
            oleThread.SetApartmentState(ApartmentState.STA);
            oleThread.Start();
            oleThread.Join();

            var app = new SciterApplication();
            app.Run(() =>
            {
                return new Host<Window>(() => new Window());
            });
            
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
