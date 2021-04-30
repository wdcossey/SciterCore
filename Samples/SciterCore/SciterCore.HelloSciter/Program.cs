using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, eventArgs) => throw ((Exception) eventArgs.ExceptionObject);

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
                .AddSciter<AppHost, AppEventHandler>(hostOptions =>
                        hostOptions
                            .SetArchiveUri("this://app/")
                            .SetHomePage(provider =>
                            {
                                string indexPage = null;

                                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                                {
                                    var osVersion = Environment.OSVersion.Version;
                                    
                                    if (osVersion.Major < 6 ||
                                        (osVersion.Major == 6 && osVersion.Minor < 1))
                                        indexPage = "index-win.html"; // TODO: Add fallback page
                                    else
                                        indexPage = "index-win.html";
                                }
                                    

                                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                                    indexPage = "index-lnx.html";

                                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                                    indexPage = "index-macos.html";

                                if (string.IsNullOrWhiteSpace(indexPage))
                                    throw new PlatformNotSupportedException();

#if DEBUG
                                //Used for development/debugging (load file(s) from the disk)
                                //See this .csproj file for the SciterCorePackDirectory and SciterCorePackCopyToOutput properties
                                var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                                var path = Path.Combine(location ?? string.Empty, "wwwroot", indexPage);
                                var uri = new Uri(path, UriKind.Absolute);
                                Debug.Assert(uri.IsFile);
                                Debug.Assert(File.Exists(uri.AbsolutePath));
#else
				                var uri = new Uri(uriString: indexPage, UriKind.Relative);
#endif
                                return uri;
                            }),
                    windowOptions => windowOptions
                        .SetTitle("SciterCore::Hello")
                        .SetDimensions(800, 600)
                        .SetPosition(SciterWindowPosition.CenterScreen));

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.RunSciterAsync();
        }
    }
}
