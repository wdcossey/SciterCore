using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DevZH.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SciterCore;
using SciterCore.Interop;

namespace SciterTest.NetCore
{
    class Program
    {
        private static SciterXDef.SCITER_WINDOW_DELEGATE _proc;

        private static IntPtr InternalProcessSciterWindowMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, IntPtr pParam, ref bool handled)
		{
			Debug.Assert(pParam.ToInt32() == 0);
			//Debug.Assert(Handle.ToInt32() == 0 || hwnd == Handle);


            Debug.WriteLine($"hwnd: {hwnd}, msg: {msg}, wParam: {wParam}, lParam: {lParam}, pParam: {pParam}, ");

            IntPtr lResult = IntPtr.Zero;

            lResult = Sciter.Api.SciterProcND(hwnd, msg, wParam, lParam, ref handled);

            if (handled)
            {
                return lResult;
            }


			handled = ProcessWindowMessage(hwnd, msg, wParam, lParam, ref lResult);
			return lResult;
		}

        protected static bool ProcessWindowMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, ref IntPtr lResult)// overrisable
		{
			return false;
		}

        [STAThread]
        static void Main(string[] args)
        {

            //var app = new SciterApplication();
            //var window = new Window();
            //var host = new Host(window);
            //app.Run(host);
            
            var app = new Application();
            
             var window = new DevZH.UI.Window("libui Control Gallery", 640, 480, true);

            PInvokeUtils.RECT frame = new PInvokeUtils.RECT(0, 0, 640, 480);
			PInvokeWindows.GetClientRect(window.Handle, out frame);

            _proc = InternalProcessSciterWindowMessage;

            var x = Sciter.Api.SciterCreateWindow(
                SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_MAIN | 
                SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE | 
                SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_TITLEBAR | 
                //SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_GLASSY | 
                //SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ALPHA | 
                SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CONTROLS | 
                SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ENABLE_DEBUG, 
                ref frame, _proc, IntPtr.Zero, IntPtr.Zero);

            //var x = Sciter.Api.SciterCreateWindow(
            //    SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CHILD | SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE | SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ENABLE_DEBUG, 
            //    ref frame, _proc, IntPtr.Zero,  window.Handle);

            //Sciter.Api.SciterSetOption(x, SciterXDef.SCITER_RT_OPTIONS.SCITER_TRANSPARENT_WINDOW, new IntPtr(1));
            //Sciter.Api.SciterSetOption(x, SciterXDef.SCITER_RT_OPTIONS.SCITER_ALPHA_WINDOW, new IntPtr(1));
            
            Sciter.Api.SciterSetOption(x, SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_UX_THEMING, new IntPtr(1));

            Sciter.Api.SciterUpdateWindow(x);

 

            //var sciterWindow = new Window()
            //        .CreateChildWindow(window.Handle)
            //        .CenterTopLevelWindow()
            //        .SetTitle("SciterTest.NetCore");

			string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			location += "/../../..";

			string path = Path.Combine(location, "res", "index.html");

			Uri uri = new Uri(path, UriKind.Absolute);

			Debug.Assert(uri.IsFile);

			Debug.Assert(File.Exists(uri.AbsolutePath));

            var load = Sciter.Api.SciterLoadFile(x,  uri.AbsoluteUri.Replace(":///", "://"));
			//sciterWindow.LoadPage(uri: uri);

            window.AllowMargins = true;
            app.Run(window);

            

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
