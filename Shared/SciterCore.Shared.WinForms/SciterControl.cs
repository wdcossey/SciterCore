using System;
using System.ComponentModel;
using System.Windows.Forms;
using SciterCore.Interop;

namespace SciterCore.WinForms
{
	/// <summary>
	/// Represents a SciterWindow control.
	/// </summary>
	[DisplayName("SciterControl")]
    [DesignerCategory("Sciter")]
    [Category("Sciter")]
	public class SciterControl : UserControl
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

		public SciterHostComponent Host { get; set; }

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
			if(SciterWnd != null && SciterWnd?.Handle != IntPtr.Zero)
            {
                var sz = this.Size;
                PInvokeWindows.MoveWindow(hWnd: SciterWnd.Handle, X: 0, Y: 0, nWidth: sz.Width, nHeight: sz.Height, bRepaint: true);
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