using SciterCore;
using SciterCore.Interop;
using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.Gtk
{
    class Host : BaseHost
	{
		public Host(SciterWindow window)
		{
			var host = this;
			host.Setup(window);
			host.AttachEventHandler(new HostEventHandler());
			host.SetupPage(page: "index.html");
			window.Show();
		}

		// Things to do here:
		// -override OnLoadData() to customize or track resource loading
		// -override OnPostedNotification() to handle notifications generated with SciterHost.PostNotification()
	}

	class HostEventHandler : SciterEventHandler
	{
		// A dynamic script call handler. Any call in TIScript to function 'view.Host_HelloWorld()' with invoke this method
		// Notice that signature of these handlers is always the same
		// (Hint: install OmniCode snippets which adds the 'ssh' snippet to C# editor so you can easily declare 'Siter Handler' methods)
		// (see: https://github.com/MISoftware/OmniCode-Snippets)
		public bool Host_HelloWorld(SciterElement el, SciterValue[] args, out SciterValue result)
		{
			result = new SciterValue("Hello Sciter! (from native side)");
			return true;
		}

		// (Hint: to overload C# methods of SciterEventHandler base class, type 'override', press space, and VS/Xamarin will suggest the methods you can override)
	}

	// This base class overrides OnLoadData and does the resource loading strategy
	// explained at http://misoftware.rs/Bootstrap/Dev
	//
	// - in DEBUG mode: resources loaded directly from the file system
	// - in RELEASE mode: resources loaded from by a SciterArchive (packed binary data contained as C# code in ArchiveResource.cs)
	class BaseHost : SciterHost
	{
		protected static Sciter.SciterApi _api = Sciter.Api;
		protected SciterArchive _archive = new SciterArchive();
		protected SciterWindow _window;

		public BaseHost()
		{
		#if !DEBUG
			_archive.Open("SiteResource");
		#endif
		}

		public void Setup(SciterWindow window)
		{
			_window = window;
			SetupWindow(window);
		}

		public void SetupPage(string page)
		{
		#if DEBUG
			string cwd = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ).Replace('\\', '/');

			#if OSX
			Environment.CurrentDirectory = cwd + "/../../../../..";
			#else
			Environment.CurrentDirectory = cwd + "/../..";
			#endif

			string path = Environment.CurrentDirectory + "/res/" + page;
			Debug.Assert(File.Exists(path));

			string url = "file://" + path;
		#else
		    string url = "archive://app/" + page_from_res_folder;
		#endif

			_window.LoadPage(url: url);
		}

		protected override SciterXDef.LoadResult OnLoadData(SciterXDef.SCN_LOAD_DATA sld)
		{
			if(sld.uri.StartsWith("archive://app/"))
			{
				// load resource from SciterArchive
				string path = sld.uri.Substring(14);
				byte[] data = _archive.Get(path);
				if(data!=null)
					_api.SciterDataReady(_window.Hwnd, sld.uri, data, (uint) data.Length);
			}

			// call base to ensure LibConsole is loaded
			return base.OnLoadData(sld);
		}
	}
}