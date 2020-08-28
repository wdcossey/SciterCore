using SciterCore;
using SciterCore.Interop;
using System;
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
		protected SciterWindow _window;

		public BaseHost()
		{
		#if !DEBUG
			_archive.Open();
		#endif
		}

		public void Setup(SciterWindow window)
		{
			_window = window;
			SetupWindow(window);
		}

		public void SetupPage(string page)
		{
#if !DEBUG
			string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			location += "\\..\\..";

			string path = Path.Combine(location, "res", page);
			Debug.Assert(File.Exists(path));

			Uri uri = new Uri(path, UriKind.Absolute);
#else
			Uri uri = new Uri(baseUri: _archive.Uri, page);
#endif

			_window.LoadPage(uri: uri);
		}

		protected override LoadResult OnLoadData(LoadData args)
		{
			// load resource from SciterArchive
			_archive?.GetItem(args.Uri, (data, path) => 
			{ 
				_api.SciterDataReady(_window.Handle, path, data, (uint) data.Length);
			});

			return base.OnLoadData(args);
        }
    }
}