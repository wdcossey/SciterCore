using SciterCore;
using SciterCore.Interop;
using System;
using System.Diagnostics;
using System.IO;
using SciterValue = SciterCore.SciterValue;
using System.Reflection;

namespace SciterTest.Idioms
{
	class Host : BaseHost
	{
		public Host(SciterWindow window)
			: base(window: window)
		{

		}

		// Things to do here:
		// -override OnLoadData() to customize or track resource loading
		// -override OnPostedNotification() to handle notifications generated with SciterHost.PostNotification()
	}

	class HostEventHandler : SciterEventHandler
	{
		protected override bool OnScriptCall(SciterElement se, string name, SciterValue[] args, out SciterValue result)
		{
			switch(name)
			{
				case "Host_HelloWorld":
					result = new SciterValue($"Hello World! (from {new StackTrace().GetFrame(1).GetMethod().Name})");
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

		public BaseHost(SciterWindow window)
			: base(window: window)
		{
			_window = window;

#if !DEBUG
			_archive.Open($"{this.GetType().Namespace}.SiteResource.bin");
#endif
		}

		public void SetupPage(string page)
		{
#if DEBUG
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
				_api.SciterDataReady(_window.Handle, path, data, (uint)data.Length);
			});

			//if(_archive?.IsOpen == true && sld.uri.StartsWith(_archive.Uri.GetLeftPart(UriPartial.Path)))
			//{
			//	// load resource from SciterArchive
			//	var uri = new Uri(sld.uri);
			//	byte[] data = _archive.Get(uri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped));
			//	if(data!=null)
			//		_api.SciterDataReady(_window.Handle, sld.uri, data, (uint) data.Length);
			//}

			return base.OnLoadData(args);
        }
    }
}