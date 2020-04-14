using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using SciterCore;
using SciterCore.Interop;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.NetCore
{
	public class Host : BaseHost
	{
		public Host(SciterWindow wnd)
		{
			var host = this;
			host.Setup(wnd);
			host.AttachEventHandler(new HostEvh());
			host.SetupPage("index.html");
			wnd.Show();
		}

		// Things to do here:
		// -override OnLoadData() to customize or track resource loading
		// -override OnPostedNotification() to handle notifications generated with SciterHost.PostNotification()
	}

	public class HostEvh : SciterEventHandler
	{
		// A dynamic script call handler. Any call in TIScript to function 'view.Host_HelloWorld()' with invoke this method
		// Notice that signature of these handlers is always the same
		// (Hint: install OmniCode snippets which adds the 'ssh' snippet to C# editor so you can easily declare 'Siter Handler' methods)
		// (see: https://github.com/MISoftware/OmniCode-Snippets)
		public bool Host_HelloWorld(SciterElement el, SciterValue[] args, out SciterValue result)
		{
			result = new SciterValue("Hello World! (from native side)");
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
		protected SciterWindow _wnd;

		public BaseHost()
		{
		#if !DEBUG
			_archive.Open<BaseHost>("SiteResource");
		#endif
		}

		public void Setup(SciterWindow wnd)
		{
			_wnd = wnd;
			SetupWindow(wnd);
		}

		public void SetupPage(string page_from_res_folder)
		{
		#if DEBUG
			string rootPath = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ).Replace('\\', '/');

			#if OSX
			rootPath = rootPath + "/../../../../..";
			#else

            rootPath = rootPath + "/../../..";
			#endif

			string path = rootPath + "/res/" + page_from_res_folder;
			Debug.Assert(File.Exists(path));

			string url = "file://" + path;
		#else
			string url = "archive://app/" + page_from_res_folder;
		#endif

			_wnd.LoadPage(url: url);
		}

		protected override SciterXDef.LoadResult OnLoadData(SciterXDef.SCN_LOAD_DATA sld)
		{
			if(sld.uri.StartsWith("archive://app/"))
			{
				// load resource from SciterArchive
				string path = sld.uri.Substring(14);
				byte[] data = _archive.Get(path);
				if(data!=null)
					_api.SciterDataReady(_wnd.Handle, sld.uri, data, (uint) data.Length);
			}

			// call base to ensure LibConsole is loaded
			return base.OnLoadData(sld);
		}
	}
}