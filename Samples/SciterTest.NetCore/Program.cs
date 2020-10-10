using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SciterCore;
using SciterTest.NetCore.Behaviors;


namespace SciterTest.NetCore
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Sciter needs this for drag'n'drop support
            SciterPlatform.EnableDragAndDrop();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            
            var services = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder
                        .ClearProviders()
                        .AddConfiguration(configuration.GetSection("Logging"))
                        .AddConsole();
                })
                .AddSingleton<IConfiguration>(provider => configuration)
                .AddSingleton<ApplicationWindow>()
                .AddSingleton<HostEventHandler>()
                .AddSingleton<SciterHost, ApplicationHost>()
                .AddSingleton<SciterApplication>();

            var serviceProvider = services.BuildServiceProvider();
            
            var app = serviceProvider.GetRequiredService<SciterApplication>();

            app.Run();
            
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
