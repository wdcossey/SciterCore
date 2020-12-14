using SciterCore;

namespace SciterTest.Gtk
{
    public class Window : SciterWindow
	{
		public Window()
		{
			CreateMainWindow(800, 600)
                .CenterWindow()
                .SetTitle("SciterCore::Gtk");
#if WINDOWS
			SetIcon(Properties.Resources.IconMain);
#endif
		}
	}
}