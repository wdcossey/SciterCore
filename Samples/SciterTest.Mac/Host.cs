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

			Uri uri = new Uri(path, UriKind.Absolute);

			Debug.Assert(uri.IsFile);

			Debug.Assert(File.Exists(uri.AbsolutePath));
			
#else
			Uri uri = new Uri("file:///Users/midiway/Documents/SciterSharp/Tests/SciterTest.Mac/res/index.html", UriKind.Absolute);
#endif

			window.LoadPage(uri: uri)
                .CenterTopLevelWindow()
                .Show();
		}

		protected override LoadResult OnLoadData(object sender, LoadDataEventArgs args)
		{
			return base.OnLoadData(sender: sender, args: args);
		}
	}

	class HostEventHandler : SciterEventHandler
	{

	}
}