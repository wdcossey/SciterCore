using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SciterCore;

namespace HelloSciterJS
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
                .AddSingleton<ApplicationWindow>()
                .AddSingleton<HostEventHandler>()
                .AddSingleton<SciterHost, ApplicationHost>()
                .AddSingleton<SciterApplication>();

            var serviceProvider = services.BuildServiceProvider();
            
            var app = serviceProvider.GetRequiredService<SciterApplication>();

            app.Run();

        }
    }
}