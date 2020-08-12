using System;
using System.Windows.Forms;
using SciterCore;

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
			//var vm = Sciter.Api.SciterGetVM(sciterControl1.SciterWnd.Handle);
			//AppWnd = new SciterWindow(sciterControl1.SciterWnd.Handle);
			//AppHost.Setup(AppWnd);
		}

		private void sciterControl1_LoadHtml(object sender, SciterCore.WinForms.LoadHtmlEventArgs e)
        {
            e.Html =
                "<body>" +
                "<code>Add an event handler to the <b>HandleCreated</b> event for any needed initialization (e.g.: load the HTML)</code><br /><br />" +
                "<code>In the handler, use the <b>SciterWnd</b> property of this control to access the SciterWindow instance.</code>" +
                "</body>";
        }
	}
}