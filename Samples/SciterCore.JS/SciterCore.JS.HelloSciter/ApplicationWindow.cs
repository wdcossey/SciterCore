namespace SciterCore.JS.HelloSciter
{
	public class ApplicationWindow : SciterWindow
	{
		public ApplicationWindow()
		{
			CreateMainWindow(800, 600)
				.CenterWindow()
				.SetTitle("SciterCore.JS::NetCore");
		}
	}
}