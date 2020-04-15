using SciterCore;
using SciterCore.Interop;
using System;
using System.Diagnostics;
using System.IO;
using SciterValue = SciterCore.SciterValue;

namespace SciterTest.Core
{
	class Host : BaseHost
	{
	}

	class HostEventHandler : SciterEventHandler
	{
		public bool Host_HelloWorld(SciterElement element, SciterValue[] @params, out SciterValue result)
		{
			result = new SciterValue(argss =>
			{
				return new SciterValue();
			});
			return true;
		}

		public bool Host_DoSomething(SciterElement element, SciterValue[] @params, out SciterValue result)
		{
			result = null;
			return true;
		}
	}


	class BaseHost : SciterHost
	{
		protected static Sciter.SciterApi _api = Sciter.Api;
		protected SciterArchive _archive = new SciterArchive();
		protected SciterWindow _window;

		public BaseHost()
		{
#if !DEBUG
			_archive.Open("SiteResource");
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
			string path = Path.GetFullPath(Environment.CurrentDirectory + "/../../res/" + page);
			Debug.Assert(File.Exists(path));
            path = path.Replace('\\', '/');

			string url = "file://" + path;
		#else
			string url = "archive://app/" + page;
		#endif

			_window.LoadPage(url: url);
		}

		protected override SciterXDef.LoadResult OnLoadData(SciterXDef.SCN_LOAD_DATA sld)
		{
			if(sld.uri.StartsWith("archive://app/"))
			{
				// load resource from SciterArchive
				string path = sld.uri.Substring(14);
				byte[] data = _archive.Get(path);
				if(data!=null)
					_api.SciterDataReady(_window.Handle, sld.uri, data, (uint) data.Length);
			}
			return base.OnLoadData(sld);
		}
	}
}