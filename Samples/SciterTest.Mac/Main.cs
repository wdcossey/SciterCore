using System;
using System.Diagnostics;
using AppKit;
using SciterCore;
using SciterCore.Interop;
using SciterCoreShared.Extensions;

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


			host = new Host(() =>
			{
				return new SciterWindow()
					.CreateMainWindow(800, 600);
			});

			host.DebugInspect();

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