using SciterCore;

namespace SciterTest.NetCore
{
	public class Window : SciterWindow
	{
		public Window()
		{
            CreateMainWindow(800, 600);
            CenterTopLevelWindow();
			SetTitle("SciterTest.NetCore");
			#if WINDOWS
				wnd.Icon = Properties.Resources.IconMain;
			#endif
        }
	}
}