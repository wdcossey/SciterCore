using SciterCore;
using SciterCore.Interop;
using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using SciterValue = SciterCore.SciterValue;
using SciterTest.Gtk.Behaviors;

namespace SciterTest.Gtk
{
    class Host : BaseArchiveHost
	{
		public Host(SciterWindow window)
            : base(window: window)
		{
			var host = this;

			host.RegisterBehaviorHandler(typeof(DrawGeometryBehavior))
				.AttachEventHandler(new HostEventHandler());

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
		public bool Host_HelloSciter(SciterElement el, SciterValue[] args, out SciterValue result)
		{
			var stackFrame = new StackTrace(true).GetFrame(0);//.GetFileName();
			result = SciterValue.Create($"<h2>Hello Sciter from C#!</h2><code>Method: {stackFrame.GetMethod().Name}<br/>File: <a href=\"{new Uri(stackFrame.GetFileName()).AbsoluteUri}\">{Path.GetFileName(stackFrame.GetFileName())}</a><br/>Line: {stackFrame.GetFileLineNumber()}<br/>Column: {stackFrame.GetFileColumnNumber()}</code>");
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

		public BaseArchiveHost(SciterWindow window)
            : base(window: window)
		{
			_window = window;
//#if !DEBUG
			_archive.Open();
//#endif
		}

		public void SetupPage(string page)
		{
#if DEBUG
			string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			
			string path = Path.Combine(location ?? string.Empty, "wwwroot", page);

			Uri uri = new Uri(path, UriKind.Absolute);

			Debug.Assert(uri.IsFile);

			Debug.Assert(File.Exists(uri.AbsolutePath));

#else
			Uri uri = new Uri(baseUri: _archive.Uri, page);
#endif

			_window.LoadPage(uri: uri);
		}

		protected override LoadResult OnLoadData(object sender, LoadDataArgs args)
		{
			// load resource from SciterArchive
			_archive?.GetItem(args.Uri, (data, path) => 
			{ 
				_api.SciterDataReady(_window.Handle, path, data, (uint) data.Length);
			});

			// call base to ensure LibConsole is loaded
			return base.OnLoadData(sender: sender, args: args);
		}
	}
}