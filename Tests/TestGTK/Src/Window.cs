using System;
using SciterCore;

namespace SciterTest.Gtk
{
	public class Window : SciterWindow
	{
		public Window()
		{
			var wnd = this;
			wnd.CreateMainWindow(800, 600);
			wnd.CenterTopLevelWindow();
			wnd.Title = "TestGTK";
			#if WINDOWS
			wnd.Icon = Properties.Resources.IconMain;
			#endif
		}
	}
}