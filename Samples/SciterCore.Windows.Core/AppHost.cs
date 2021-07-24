using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using SciterCore.Attributes;
using SciterCore.Interop;

namespace SciterCore.Windows.Core
{

	//[SciterHostEventHandlerAttribute(typeof(AppEventHandler))]
	[SciterHostArchive("this://app/")]
	class AppHost : SciterArchiveHost
	{
		protected static ISciterApi _api = Sciter.SciterApi;
		protected SciterWindow _window;

		public AppHost(SciterWindow window)
			:base()
		{
			_window = window;
			
			var assembly = Assembly.Load("SciterCore.SciterSharp.Utilities");

			var attributes = assembly.GetCustomAttributes<SciterCoreArchiveAttribute>();
			
			foreach (var attribute in attributes)
			{
				var archive = new SciterArchive(attribute.Uri)
					.Open(assembly: assembly, attribute.ResourceName);
				
				AttachedArchives.TryAdd(archive.Uri.Scheme, archive);
				
				if (attribute.InitScripts?.Any() != true) 
					continue;
				
				foreach (var initScript in attribute.InitScripts)
				{
					var byteArray = Encoding.UTF8.GetBytes($"include \"{initScript}\";");
					var pinnedArray = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
					var pointer = pinnedArray.AddrOfPinnedObject();
					Sciter.SciterApi.SciterSetOption(IntPtr.Zero, SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_INIT_SCRIPT,
						pointer);
					pinnedArray.Free();
				}
			}
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
			Uri uri = new Uri(baseUri: Archive.Uri, page);
#endif

			_window.LoadPage(uri: uri);

			return this;
		}

		protected override LoadResult OnLoadData(object sender, LoadDataArgs args)
		{
			return base.OnLoadData(sender: sender, args: args);
		}
	}
}