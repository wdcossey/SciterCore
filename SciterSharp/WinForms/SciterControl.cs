#if WINDOWS && NET45
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SciterCore.Interop;

namespace SciterCore.WinForms
{
	/// <summary>
	/// Represents a SciterWindow control.
	/// </summary>
	public class SciterControl : Control
	{

        private static string DEFAULT_HTML = 
            "<body><code>Use the <b>LoadHtml</b> event of {0} to load some html.</code>" + 
            "<br/><br/>" +
            "<pre><code>    {0}.LoadHtml += (sender, args) => <br/>" + 
            "    {<br/>" + 
            "        args.Html = \"&lt;body&gt;Hello &lt;b&gt;World&lt;/b&gt;&lt;/body&gt;\";<br/>" + 
            "    }</code></pre>";

		public SciterWindow SciterWnd { get; private set; }

		public SciterControl()
		{
			SciterWnd = new SciterWindow();
		}

        public string Html { get; set; }

		public event EventHandler<LoadHtmlEventArgs> LoadHtml;

		#region Overrided Methods
		protected override void OnHandleCreated(EventArgs e)
		{
			SciterWnd.CreateChildWindow(Handle);

            var loadHtmlEventArgs = new LoadHtmlEventArgs()
            {
                Html = this.Html
            };

            LoadHtml?.Invoke(this, loadHtmlEventArgs);

            SciterWnd.LoadHtml(loadHtmlEventArgs?.Html ?? this.Html ?? DEFAULT_HTML);

			SciterWnd.Show();

			base.OnHandleCreated(e);
		}

		protected override void OnClientSizeChanged(EventArgs e)
		{
			if(SciterWnd.Handle.ToInt32()!=0)
			{
				var sz = this.Size;
				PInvokeWindows.MoveWindow(SciterWnd.Handle, 0, 0, sz.Width, sz.Height, true);
			}
			base.OnClientSizeChanged(e);
		}
		#endregion
	}

    public class LoadHtmlEventArgs : EventArgs
    {
        public LoadHtmlEventArgs()
        {
            
        }

        public string Html { get; set; }

    }
}
#endif