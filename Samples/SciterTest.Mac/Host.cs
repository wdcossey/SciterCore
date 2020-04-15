using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using SciterCore;

namespace SciterTest.Mac
{
	public class Host : SciterHost
	{

        public Host()
        {

        }

		public Host(Func<SciterWindow> windowFunc)
			: this(window: windowFunc?.Invoke())
		{

		}

		public Host(SciterWindow window)
			: base(window: window)
		{
			RegisterBehaviorHandler(typeof(ImgDrawBehavior))
                .AttachEventHandler(new HostEventHandler());
			
#if DEBUG
			string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			location += "\\..\\..\\..\\..\\..";

			string path = Path.Combine(location, "res", "index.html");
			Debug.Assert(File.Exists(path));

			Uri uri = new Uri(path, UriKind.Absolute);
#else
			Uri uri = new Uri("file:///Users/midiway/Documents/SciterSharp/Tests/SciterTest.Mac/res/index.html", UriKind.Absolute);
#endif

			window.LoadPage(uri: uri)
                .CenterTopLevelWindow()
                .Show();
		}

		protected override global::SciterCore.Interop.SciterXDef.LoadResult OnLoadData(global::SciterCore.Interop.SciterXDef.SCN_LOAD_DATA sld)
		{
			return base.OnLoadData(sld);
		}
	}

	class HostEventHandler : SciterEventHandler
	{

	}
}