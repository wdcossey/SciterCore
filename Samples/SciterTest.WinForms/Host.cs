using SciterCore;
using SciterCore.Interop;
using System;
using System.Diagnostics;
using System.IO;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.WinForms
{
	class Host : BaseHost
	{
		// Things to do here:
		// -override OnLoadData() to customize or track resource loading
		// -override OnPostedNotification() to handle notifications generated with SciterHost.PostNotification()
	}

	class HostEvh : SciterEventHandler
	{
		protected override bool OnScriptCall(SciterElement se, string name, SciterValue[] args, out SciterValue result)
		{
			switch(name)
			{
				case "Host_HelloWorld":
					result = new SciterValue("Hello World! (from native side)");
					return true;
			}

			result = null;
			return false;
		}
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
		protected SciterWindow _wnd;

		public BaseHost()
		{
		#if !DEBUG
			_archive.Open(SciterAppResource.ArchiveResource.resources);
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
			string path = Environment.CurrentDirectory + "/../../res/" + page_from_res_folder;
			Debug.Assert(File.Exists(path));
            path = path.Replace('\\', '/');

			string url = "file:///" + path;
		#else
			string url = "archive://app/" + page_from_res_folder;
		#endif

			_wnd.LoadPage(fileName: url);
		}

		protected override SciterXDef.LoadResult OnLoadData(SciterXDef.SCN_LOAD_DATA sld)
		{
			if(sld.uri.StartsWith("archive://app/"))
			{
				// load resource from SciterArchive
				string path = sld.uri.Substring(14);
				byte[] data = _archive.Get(path);
				if(data!=null)
					_api.SciterDataReady(_wnd._hwnd, sld.uri, data, (uint) data.Length);
			}
			return base.OnLoadData(sld);
        }
    }
}