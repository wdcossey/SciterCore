using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using SciterCore.Interop;

namespace SciterCore.Windows.Core
{

	class ApplicationHost : SciterArchiveHost
	{
		protected static ISciterApi _api = Sciter.SciterApi;
		protected SciterArchive _archive = new SciterArchive();
		protected SciterWindow _window;

		public ApplicationHost(SciterWindow window)
		{
			_window = window;
		}
		
		public SciterHost SetupPage(string page)
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

			return this;
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
	
	class HostEventHandler : SciterEventHandler
	{
		public bool Host_HelloWorld(SciterElement element, SciterValue[] @params, out SciterValue result)
		{
			result = new SciterValue(argss => SciterValue.Undefined);
			return true;
		}

		public bool Host_DoSomething(SciterElement element, SciterValue[] @params, out SciterValue result)
		{
			result = null;
			return true;
		}
	}
}