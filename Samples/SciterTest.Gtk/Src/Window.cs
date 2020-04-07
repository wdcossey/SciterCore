using SciterCore;
using SciterCore.Interop;

namespace SciterTest.Gtk
{
	public class Window : SciterWindow
	{
		public Window()
		{
			var wnd = this;
            wnd.CreateMainWindow(800, 600,
                SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_MAIN |
                SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CONTROLS |
                SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_TITLEBAR |
                SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_GLASSY |
                SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ENABLE_DEBUG |
                SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE);
			wnd.CenterTopLevelWindow();
			wnd.Title = "SciterTest.Gtk";
			#if WINDOWS
			wnd.Icon = Properties.Resources.IconMain;
			#endif
		}
	}
}