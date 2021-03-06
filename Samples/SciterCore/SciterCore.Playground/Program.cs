﻿using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SciterCore.Playground
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
                .AddTransient<ApplicationWindow>()
                
                .AddTransient<HostEventHandler>()
                //.AddScoped<SciterHost>()
                .AddTransient<BaseHost>()
                .AddTransient<ApplicationHost>()
                
                .AddTransient<CustomHostEventHandler>()
                .AddTransient<CustomHost>()
                .AddTransient<CustomWindow>()
                .AddTransient<SciterApplication>();

            var serviceProvider = services.BuildServiceProvider();
            
            var app = serviceProvider.GetRequiredService<SciterApplication>();

            app.Run(serviceProvider.GetRequiredService<ApplicationHost>());
            
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
