using System;
using System.Windows.Forms;
using SciterCore;
using SciterCore.Interop;
using TestWinForms;

namespace SciterTest.WinForms
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			sciterControl1.HandleCreated += SciterControl1_HandleCreated;
		}

		private SciterWindow AppWnd;
		private Host AppHost = new Host();

		private void SciterControl1_HandleCreated(object sender, EventArgs e)
		{
			var vm = Sciter.Api.SciterGetVM(sciterControl1.Handle);
			//AppWnd = new SciterWindow(sciterControl1.Handle);
			//AppHost.Setup(AppWnd);
		}
	}
}