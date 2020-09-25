using SciterCore;

namespace SciterTest.NetCore
{
	public class ApplicationWindow : SciterWindow
	{
		public ApplicationWindow()
		{
			CreateMainWindow(800, 600)
				.CenterTopLevelWindow()
				.SetTitle("SciterCore::NetCore");
		}
	}
}