using SciterCore;
using SciterCore.Interop;

namespace SciterTest.NetCore
{
	public class Window : SciterWindow
	{
		public Window()
		{
			var wnd = this;
            CreateMainWindow(800, 600);
            CenterTopLevelWindow();
			SetTitle("SciterTest.NetCore");
			#if WINDOWS
			wnd.Icon = Properties.Resources.IconMain;
			#endif
        }
	}
}