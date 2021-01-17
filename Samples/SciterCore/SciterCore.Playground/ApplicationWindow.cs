namespace SciterCore.Playground
{
	public class ApplicationWindow : SciterWindow
	{
		public ApplicationWindow()
		{
			CreateMainWindow(800, 600)
				.CenterWindow()
				.SetTitle("SciterCore::Playground");
		}
	}
}