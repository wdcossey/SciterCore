using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SciterCore.JS.HelloSciter.Behaviors;

namespace SciterCore.JS.HelloSciter
{
    class Program
    {
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
                .AddSingleton<IConfiguration>(configuration)
                .AddSciterBehavior<RuntimeInformationBehavior>()
                .AddSciter<AppHost>();
                //.AddSingleton<SciterApplication>();

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<SciterApplication>();

            app.Run();
        }

    }
}