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
            // Sciter needs this for drag'n'drop support
            SciterPlatform.EnableDragAndDrop();

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
