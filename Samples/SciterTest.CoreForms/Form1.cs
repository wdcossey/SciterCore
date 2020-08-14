using System;
using System.Windows.Forms;
using SciterCore.Interop;

namespace SciterTest.CoreForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            sciterControl1.HandleCreated += SciterControl1HandleCreated;
            sciterControl1.LoadHtml += SciterControl1OnLoadHtml;
        }

        

        private void SciterControl1OnLoadHtml(object? sender, LoadHtmlEventArgs e)
        {
            e.Html =
                "<body>" +
                "<code>Add an event handler to the <b>HandleCreated</b> event for any needed initialization (e.g.: load the HTML)</code><br /><br />" +
                "<code>In the handler, use the <b>SciterWnd</b> property of this control to access the SciterWindow instance.</code>" +
                "</body>";
        }

        //private SciterWindow AppWnd;
        //private Host AppHost = new Host();

        private void SciterControl1HandleCreated(object sender, EventArgs e)
        {
            //var vm = Sciter.Api.SciterGetVM(sciterControl1.Handle);
            //AppWnd = new SciterWindow(sciterControl1.Handle);
            //AppHost.Setup(AppWnd);
        }

    }
}
