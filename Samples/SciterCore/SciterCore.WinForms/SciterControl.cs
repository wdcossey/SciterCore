using System;
using System.Windows.Forms;
using SciterCore.Interop;

namespace SciterCore.WinForms
{
    public partial class SciterControl : UserControl
    {
        private static readonly ISciterApi SciterApi = Sciter.SciterApi;
        
        private static string DEFAULT_HTML = 
            "<body><code>Use the <b>LoadHtml</b> event of {0} to load some html.</code>" + 
            "<br/><br/>" +
            "<pre><code>    {0}.LoadHtml += (sender, args) => <br/>" + 
            "    {<br/>" + 
            "        args.Html = \"&lt;body&gt;Hello &lt;b&gt;World&lt;/b&gt;&lt;/body&gt;\";<br/>" + 
            "    }</code></pre>";

        public SciterWindow SciterWindow { get; private set; }

        public SciterControl()
        {
            InitializeComponent();
            Host = new SciterArchiveHost();
        }

        public string Html { get; set; }

        public event EventHandler<LoadUriEventArgs> LoadUri;

        public event EventHandler<LoadHtmlEventArgs> LoadHtml;

        private SciterArchiveHost Host { get; }
        
        private WinFormsHostEventHandler HostEventHandler { get; } = new WinFormsHostEventHandler();
        
        #region Overrided Methods

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            
            SciterApi.SciterSetOption(IntPtr.Zero, SciterXDef.SCITER_RT_OPTIONS.SCITER_SET_DEBUG_MODE, new IntPtr(1));
            
            SciterWindow = SciterWindow
                .CreateChildWindow(Handle);

            Host.SetupWindow(SciterWindow)
                .AttachEventHandler(HostEventHandler);
            
            if (LoadUri != null)
            {
                var loadUriEventArgs = new LoadUriEventArgs();
                
                LoadUri?.Invoke(this, loadUriEventArgs);
                Host.Window.TryLoadPage(loadUriEventArgs.Uri);
            }
            else
            {
                var loadHtmlEventArgs = new LoadHtmlEventArgs()
                {
                    Html = Html
                };
                
                LoadHtml?.Invoke(this, loadHtmlEventArgs);
                Host.Window.LoadHtml(loadHtmlEventArgs?.Html ?? this.Html ?? string.Format(DEFAULT_HTML, Name));
            }
            
            OnClientSizeChanged(EventArgs.Empty);
            Host.Window.Show();
            //Host.ConnectToInspector();
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            if(Host?.Window != null && Host?.Window?.Handle != IntPtr.Zero)
            {
                var clientSize = this.Size;
                PInvokeWindows.MoveWindow(hWnd: Host.Window.Handle, X: 0, Y: 0, nWidth: clientSize.Width, nHeight: clientSize.Height, bRepaint: true);
            }
            base.OnClientSizeChanged(e: e);
        }

        #endregion
    }

    public class LoadHtmlEventArgs : EventArgs
    {
        public LoadHtmlEventArgs() { }

        public string Html { get; set; }

    }

    public class LoadUriEventArgs : EventArgs
    {
        public LoadUriEventArgs() { }

        public Uri Uri { get; set; }

    }
}
