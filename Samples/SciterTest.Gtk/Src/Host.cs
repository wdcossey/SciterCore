using SciterCore;
using SciterCore.Interop;
using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.Gtk
{
    class Host : BaseArchiveHost
	{
		public Host(SciterWindow window)
            : base(window: window, archiveName: "SiteResource")
		{
			var host = this;
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
	class BaseArchiveHost : SciterHost
	{
		protected static Sciter.SciterApi _api = Sciter.Api;
		protected SciterArchive _archive = new SciterArchive();
		protected SciterWindow _window;

		public BaseArchiveHost(SciterWindow window, string archiveName)
            : base(window: window)
		{
			_window = window;
#if !DEBUG
			_archive.Open(archiveName);
#endif
		}

		public void SetupPage(string page)
		{
#if DEBUG
			string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

#if OSX
			location += "\\..\\..\\..\\..\\..";
#else
			location += "\\..\\..";
#endif

			string path = Path.Combine(location, "res", page);
			Debug.Assert(File.Exists(path));

			Uri uri = new Uri(path, UriKind.Absolute);
#else
			Uri uri = new Uri(baseUri: _archive.Uri, page);
#endif

			_window.LoadPage(uri: uri);
		}

		protected override SciterXDef.LoadResult OnLoadData(SciterXDef.SCN_LOAD_DATA sld)
		{
			if(sld.uri.StartsWith(_archive.Uri.AbsoluteUri))
			{
				// load resource from SciterArchive
				string path = sld.uri.Substring(14);
                byte[] data = _archive?.Get(path);
				if(data!=null)
					_api.SciterDataReady(_window.Handle, sld.uri, data, (uint) data.Length);
			}

			// call base to ensure LibConsole is loaded
			return base.OnLoadData(sld);
		}
	}
}