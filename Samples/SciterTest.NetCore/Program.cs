using System;
using System.IO;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using DevZH.UI;
using DevZH.UI.Drawing;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
using SciterCore;
using SciterCore.Interop;
using DevZH.UI.Interop;
//using Gtk;
//using Application = Gtk.Application;
//using Window = Gtk.Window;
//using Label = Gtk.Label;

namespace SciterTest.NetCore
{
    class Program
    {
        private SciterXDef.SCITER_WINDOW_DELEGATE _proc;

        [STAThread]
        static async Task Main(string[] args)
        {

            //Application.Init();
            //Window win = new Window("Hello GTK");
            //Label lbl = new Label("This is a test GTK App written with C# for GNU/Linux and Windows");
            ////win.DeleteEvent += Win_DeleteEvent;
            //win.Add(lbl);
            //win.ShowAll();
            //Application.Run();

            //var app = new Application();
            //var window = new DevZH.UI.Window("libui Control Gallery", 640, 480, true);
            //window.AllowMargins = true;

            InitOptions options = new InitOptions()
            {
                Size = UIntPtr.Zero
            };

            NativeMethods.Init(ref options);

#if DEBUG
            var oResult = Sciter.Api.SciterSetOption(
                IntPtr.Zero,
                SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_DEBUG_MODE,
                new IntPtr(1));
#endif

            Sciter.Api.SciterSetOption(
                IntPtr.Zero,
                SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_GFX_LAYER,
                new IntPtr((int)SciterXDef.GFX_LAYER.GFX_LAYER_AUTO));

            var sciterApp = new SciterApplication();
            //
            //PInvokeUtils.RECT frame = new PInvokeUtils.RECT(640, 480);
            //
            //var hwnd = Sciter.Api.SciterCreateWindow(
            //    SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_TITLEBAR |
            //    SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_MAIN |
            //    SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CONTROLS |
            //    SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_OWNS_VM |
            //    SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE |
            //    SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ENABLE_DEBUG,
            //    ref frame,
            //    null,
            //    IntPtr.Zero,
            //    IntPtr.Zero
            //);
            //
  
            var sciterWindow = new SciterWindow().CreateMainWindow(640, 480)
                .CenterTopLevelWindow()
                .SetTitle("SciterTest.NetCore");
  
            var host = new Host(sciterWindow);
  
            sciterApp.Run(host);

            //NativeMethods.WindowSetChild(window.Handle, sciterWindow.Handle);

            //window.Child = sciterWindow;
            //var window = new DevZH.UI.Window(sciterWindow.Handle);
            //window.AllowMargins = true;


            //new DevZH.UI.Control();

            NativeMethods.Main();

            //app.Run(window);
  //
  //          
  //
  //          
  //
  //          //var host = new HostBuilder()
  //          //    .ConfigureHostConfiguration(configHost =>
  //          //    {
  //          //        configHost.SetBasePath(Directory.GetCurrentDirectory());
  //          //        //configHost.AddJsonFile(_hostsettings, optional: true);
  //          //        //configHost.AddEnvironmentVariables(prefix: _prefix);
  //          //        configHost.AddCommandLine(args);
  //          //    })
  //          //    .ConfigureAppConfiguration((hostContext, configApp) =>
  //          //    {
  //          //        configApp.SetBasePath(Directory.GetCurrentDirectory());
  //          //        //configApp.AddJsonFile(_appsettings, optional: true);
  //          //        configApp.AddJsonFile(
  //          //            $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
  //          //            optional: true);
  //          //        //configApp.AddEnvironmentVariables(prefix: _prefix);
  //          //        configApp.AddCommandLine(args);
  //          //    })
  //          //    .ConfigureServices((hostContext, services) =>
  //          //    {
  //          //        services.AddLogging();
  //          //        services.AddHostedService<Startup>();
  //
  //          //    })
  //          //    .ConfigureLogging((hostContext, configLogging) =>
  //          //    {
  //          //        configLogging.AddConsole();
  //
  //          //    })
  //          //    .UseConsoleLifetime()
  //          //    .Build();
  //
  //
  //          //await host.RunAsync();
  //

        }
    }
}
