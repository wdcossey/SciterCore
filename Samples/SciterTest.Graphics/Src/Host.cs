using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using SciterCore;
using SciterCore.Interop;
using SciterGraphics = SciterCore.Interop.SciterGraphics;
using SciterValue = SciterCore.SciterValue;
using System.Reflection;

namespace SciterTest.Graphics
{
	class Host : BaseHost
	{

		public Host(SciterWindow window)
			: base(window)
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
			var r = new SciterImage(args[0]);
			var b = r.Save(SciterGraphics.SCITER_IMAGE_ENCODING.SCITER_IMAGE_ENCODING_PNG);
			File.WriteAllBytes("d:/test.png", b);

			result = null;
			return true;
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
			: base(window)
		{
			_window = window;

#if !DEBUG
			_archive.Open("SciterTest.Graphics.SiteResource.bin");
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

		protected override SciterXDef.LoadResult OnLoadData(SciterXDef.SCN_LOAD_DATA sld)
		{
			if(_archive?.IsOpen == true && sld.uri.StartsWith(_archive.Uri.GetLeftPart(UriPartial.Path)))
			{
				// load resource from SciterArchive
				var uri = new Uri(sld.uri);
				byte[] data = _archive.Get(uri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped));
				if(data!=null)
					_api.SciterDataReady(_window.Handle, sld.uri, data, (uint) data.Length);
			}
			return base.OnLoadData(sld);
		}
	}
}