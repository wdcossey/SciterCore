using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SciterCore.SkiaSharp.Behaviors;

namespace SciterCore.SkiaSharp
{
    class Program
    {
        static Task Main(string[] args)
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

                .AddSciterBehavior<DrawBitmapBehavior>()
                .AddSciterBehavior<InfoBitmapBehavior>()
                .AddSciterBehavior<SolidBitmapBehavior>()
                .AddSciterBehavior<SolidForegroundBitmapBehavior>()
                .AddSciterBehavior<LinearBitmapBehavior>()
                .AddSciterBehavior<LinearForegroundBitmapBehavior>()
                .AddSciterBehavior<RadialBitmapBehavior>()
                .AddSciterBehavior<RadialForegroundBitmapBehavior>()
                
                .AddSciter<SkiaSharpAppHost>();

            var serviceProvider = services.BuildServiceProvider();
            
            var app = serviceProvider.GetRequiredService<SciterApplication>();

            return app.RunAsync();
        }
    }
}