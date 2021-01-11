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
		protected override ScriptEventResult OnScriptCall(SciterElement element, MethodInfo method, SciterValue[] args)
		{
			switch(method.Name)
			{
				case "Host_HelloWorld":
					return ScriptEventResult.Successful(SciterValue.Create($"Hello World! (from {new StackTrace().GetFrame(1).GetMethod().Name})"));
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
		protected static ISciterApi _api = Sciter.SciterApi;
		protected SciterArchive _archive = new SciterArchive();
		protected SciterWindow _window;

		public BaseHost(SciterWindow window)
			: base(window: window)
		{
			_window = window;

#if !DEBUG
			_archive.Open();
#endif
		}

		public void SetupPage(string page)
		{
#if DEBUG
			var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var path = Path.Combine(location ?? string.Empty, "..\\..", "wwwroot", page);
			Debug.Assert(File.Exists(path));

			var uri = new Uri(path, UriKind.Absolute);
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

			//if(_archive?.IsOpen == true && sld.uri.StartsWith(_archive.Uri.GetLeftPart(UriPartial.Path)))
			//{
			//	// load resource from SciterArchive
			//	var uri = new Uri(sld.uri);
			//	byte[] data = _archive.Get(uri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped));
			//	if(data!=null)
			//		_api.SciterDataReady(_window.Handle, sld.uri, data, (uint) data.Length);
			//}

			return base.OnLoadData(sender: sender, args: args);
        }
    }
}