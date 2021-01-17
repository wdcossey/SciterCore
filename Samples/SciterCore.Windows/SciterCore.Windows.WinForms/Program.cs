using System;
using System.Windows.Forms;

namespace SciterCore.Windows.WinForms
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			SciterPlatform.EnableDragAndDrop();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}