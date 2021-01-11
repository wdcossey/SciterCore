using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SciterCore;

namespace HelloSciterJS
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build();

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
                .AddSciterHost<ApplicationHost>()
                .AddSingleton<SciterApplication>();

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<SciterApplication>();

            app.Run();
        }

        public static IHostBuilder CreateHostBuilderXXX(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.UseConsoleLifetime()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    throw new NotImplementedException();
                })
                .ConfigureServices((context, collection) =>
                {
                    throw new NotImplementedException();
                });
        
        public static ISciterHostBuilder CreateHostBuilder(string[] args) =>
            TempHost.CreateDefaultBuilder(args)
                //.UseConsoleLifetime()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    //throw new NotImplementedException();
                })
                .ConfigureServices((context, collection) =>
                {
                    //throw new NotImplementedException();
                });
    }
}