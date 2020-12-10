using SciterCore;
using SciterCore.Interop;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
		protected override ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
		{
			switch(method.Name)
			{
				case "Host_HelloWorld":
					return  ScriptEventResult.Successful(SciterValue.Create("Hello World! (from native side)"));
			}

			return ScriptEventResult.Failed();
		}
	}

	// This base class overrides OnLoadData and does the resource loading strategy
	// explained at http://misoftware.rs/Bootstrap/Dev
	//
	// - in DEBUG mode: resources loaded directly from the file system
	// - in RELEASE mode: resources loaded from by a SciterArchive (packed binary data contained as C# code in ArchiveResource.cs)
	class BaseHost : SciterHost
	{
		protected static ISciterApi _api = Sciter.Api;
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
#if DEBUG
			var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var path = Path.Combine(location ?? string.Empty, "..\\..", "wwwroot", page);
			Debug.Assert(File.Exists(path));

			Uri uri = new Uri(path, UriKind.Absolute);
#else
			Uri uri = new Uri(baseUri: _archive.Uri, page);
#endif

			_window.LoadPage(uri: uri);
		}

		protected override LoadResult OnLoadData(object sender, LoadDataArgs args)
		{
			// load resource from SciterArchive
			_archive?.GetItem(args.Uri, (result) => 
			{ 
				if (result.IsSuccessful)
					_api.SciterDataReady(_window.Handle, result.Path, result.Data, (uint) result.Size);
			});

			return base.OnLoadData(sender: sender, args: args);
        }
    }
}