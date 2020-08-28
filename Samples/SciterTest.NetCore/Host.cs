using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using SciterCore;
using SciterCore.Interop;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.NetCore
{
	public class Host<TWindow> : BaseHost
		where TWindow : SciterWindow 
	{
		public static TWindow AppWindow { get; private set; }// must keep a reference to survive GC
		
		public Host(TWindow wnd)
		{
			AppWindow = wnd;
			var host = this;
			host.Setup(wnd);
			host.AttachEventHandler(new HostEvh());
			host.SetupPage("index.html");
			wnd.Show();
		}
		
		public Host(Func<TWindow> wndFunc)
		: this(wndFunc.Invoke())
		{
			
		}

		// Things to do here:
		// -override OnLoadData() to customize or track resource loading
		// -override OnPostedNotification() to handle notifications generated with SciterHost.PostNotification()
	}

	public class HostEvh : SciterEventHandler
	{
		/// A dynamic script call handler. Any call in TIScript to function 'view.Host_HelloSciter()' with invoke this method
		/// Notice that signature of these handlers is always the same
		/// (Hint: install OmniCode snippets which adds the 'ssh' snippet to C# editor so you can easily declare 'Siter Handler' methods)
		/// (see: https://github.com/MISoftware/OmniCode-Snippets)
		public bool Host_HelloSciter(SciterElement el, SciterValue[] args, out SciterValue result)
		{
			var stackTrace = new StackTrace(true);
			var stackFrame = stackTrace.GetFrame(0);
				
			/*result = new SciterValue(System.Text.Json.JsonSerializer.Serialize(new
			{
				method = stackFrame?.GetMethod()?.Name,
				uri = new Uri(stackFrame?.GetFileName())?.AbsoluteUri,
				fileName = Path.GetFileName(stackFrame?.GetFileName()),
				lineNumber = stackFrame?.GetFileLineNumber(),
				columnNumber = stackFrame?.GetFileColumnNumber()
			}));*/
			
			result = new SciterValue($"<h2>Hello Sciter from C# in .Net Core!</h2><code>Method: {stackFrame?.GetMethod()?.Name}<br/>File: <a href=\"{new Uri(stackFrame?.GetFileName())?.AbsoluteUri}\">{Path.GetFileName(stackFrame?.GetFileName())}</a><br/>Line: {stackFrame?.GetFileLineNumber()}<br/>Column: {stackFrame?.GetFileColumnNumber()}</code>");
			return true;
		}

		// (Hint: to overload C# methods of SciterEventHandler base class, type 'override', press space, and VS/Xamarin will suggest the methods you can override)
	}

	// This base class overrides OnLoadData and does the resource loading strategy
	// explained at http://misoftware.rs/Bootstrap/Dev
	//
	// - in DEBUG mode: resources loaded directly from the file system
	// - in RELEASE mode: resources loaded from by a SciterArchive (packed binary data contained as C# code in ArchiveResource.cs)
	public class BaseHost : SciterHost
	{
		protected static Sciter.SciterApi _api = Sciter.Api;
		protected SciterArchive _archive = new SciterArchive();
		protected SciterWindow _window;

		public BaseHost()
		{
			_archive.Open();
		}

		public void Setup(SciterWindow window)
		{
			_window = window;
			SetupWindow(window);
		}

		public void SetupPage(string page)
		{
//#if DEBUG
//			string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//
//#if OSX
//			location += "\\..\\..\\..\\..\\..\\..";
//#else
//			location += "\\..\\..\\..";
//#endif
//
//			string path = Path.Combine(location, "res", page);
//
//			Uri uri = new Uri(path, UriKind.Absolute);
//
//			Debug.Assert(uri.IsFile);
//
//			Debug.Assert(File.Exists(uri.AbsolutePath));
//#else
			Uri uri = new Uri(baseUri: _archive.Uri, page);
//#endif

			_window.LoadPage(uri: uri);
		}

		protected override LoadResult OnLoadData(LoadData args)
		{
			// load resource from SciterArchive
			_archive?.GetItem(args.Uri, (data, path) => 
			{
				_api.SciterDataReady(_window.Handle, path, data, (uint) data.Length);
			});

			// call base to ensure LibConsole is loaded
			return base.OnLoadData(args);
		}
	}
}