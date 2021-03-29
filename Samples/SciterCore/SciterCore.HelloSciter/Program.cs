using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SciterCore.Enums;
using SciterCore.HelloSciter.Behaviors;

namespace SciterCore.HelloSciter
{
    class Program
    {
        [MTAThread]
        static Task Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) => throw ((Exception) eventArgs.ExceptionObject);
            
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
                //.AddSciterHost<AppHost>()
                .AddSciterBehavior<DragDropBehavior>()
                .AddSciterBehavior<SecondBehavior>()
                .AddSciter<AppHost, AppEventHandler>((options)  =>
                {
                    options
                        .SetArchiveUri("this://app/")
                        .SetHomePage("index.html")
                        .SetWindowOptions<SciterWindow>(windowOptions =>
                        {
                            windowOptions
                                .SetTitle("SciterCore::Hello1")
                                .SetDimensions(800, 600)
                                .SetPosition(SciterWindowPosition.CenterScreen);
                        });
                });

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.RunSciterAsync();
        }
    }
}
