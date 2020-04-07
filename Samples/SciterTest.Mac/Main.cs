using System;
using System.Diagnostics;
using AppKit;
using SciterCore;
using SciterCore.Interop;

namespace SciterTest.Mac
{
    static class MainClass
	{
		static SciterMessages sm = new SciterMessages();
		static Host host;

		static void Main(string[] args)
		{
			Debug.WriteLine(Sciter.Version());
			Sciter.Api.SciterSetOption(
				IntPtr.Zero, 
				SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_GFX_LAYER, 
				new IntPtr((int) SciterXDef.GFX_LAYER.GFX_LAYER_CG));

			NSApplication.Init();

			SciterWindow wnd = new SciterWindow();
			wnd.CreateMainWindow(800, 600, 
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_MAIN |
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE |
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ENABLE_DEBUG |
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_GLASSY);
			host = new Host(wnd);
			//host.DebugInspect();

			NSApplication.Main(args);
		}
	}

	class SciterMessages : SciterDebugOutputHandler
	{
		protected override void OnOutput(SciterCore.Interop.SciterXDef.OUTPUT_SUBSYTEM subsystem, SciterCore.Interop.SciterXDef.OUTPUT_SEVERITY severity, string text)
		{
			Console.WriteLine(text);
		}
	}
}