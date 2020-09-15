using SciterCore;
using SciterCore.Interop;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
			string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
			
			string path = Path.Combine(location, "wwwroot", page);

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
			
			return base.OnLoadData(sender: sender, args: args);
		}
	}
}