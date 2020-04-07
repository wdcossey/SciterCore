using System;
using System.IO;
using System.Reflection;
using SciterCore;

namespace SciterTest.Mac
{
	public class Host : SciterHost
	{
		public Host(SciterWindow wnd)
			: base(wnd)
		{
			RegisterBehaviorHandler(typeof(ImgDrawBehavior));

			AttachEvh(new HostEVH());
			
#if DEBUG
			var path = Environment.CurrentDirectory;//Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			string url = "file:///Users/wdcossey/RiderProjects/SciterCore/Samples/SciterTest.Mac/res/index.html";
#else
			string url = "file:///Users/midiway/Documents/SciterSharp/Tests/SciterTest.Mac/res/index.html";
#endif
			
			wnd.LoadPage(url);
			wnd.CenterTopLevelWindow();
			wnd.Show();
		}

		protected override global::SciterCore.Interop.SciterXDef.LoadResult OnLoadData(global::SciterCore.Interop.SciterXDef.SCN_LOAD_DATA sld)
		{
			return base.OnLoadData(sld);
		}
	}

	class HostEVH : SciterEventHandler
	{

	}
}