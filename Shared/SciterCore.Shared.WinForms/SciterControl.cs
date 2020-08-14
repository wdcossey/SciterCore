using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        private SciterHostComponent _host;

        //private TextureBrush _brush;

        public SciterWindow SciterWnd { get; private set; }

		public SciterControl()
		{
			
		}

		public SciterHostComponent Host
		{
			get => _host;
			set
			{
				_host = value;

				if (SciterWnd != null && SciterWnd?.Handle != IntPtr.Zero)
				{
					_host?.FormsHost?.SetWindow(SciterWnd);
				}
			}
		}

		public string Html { get; set; }

        [Category("Sciter")]
		public event EventHandler<LoadHtmlEventArgs> LoadHtml;

		#region Overrided Methods
		
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
		}
		
		//protected override void OnPaintBackground(PaintEventArgs e)
		//{
		//	e.Graphics.Clear(SystemColors.Control);
		//	
		//	var size = 8;
//
		//	using (var bmp = new Bitmap(size * 2, size * 2))
		//	{
		//		using (var brush = new SolidBrush(SystemColors.ControlDark))
		//		using (var graphics = Graphics.FromImage(bmp))
		//		{
		//			graphics.FillRectangle(brush, 0, 0, size, size);
		//			graphics.FillRectangle(brush, size, size, size, size);
		//		}
		//		
		//		using (var textureBrush = new TextureBrush(bmp, WrapMode.Tile))
		//		{
		//			e.Graphics.FillRectangle(textureBrush, e.ClipRectangle);
		//		}
		//	}
		//}

		protected override void OnHandleCreated(EventArgs e)
		{
			SciterWnd = new SciterWindow()
				.CreateChildWindow(Handle);
			
			if (SciterWnd != null && SciterWnd?.Handle != IntPtr.Zero)
			{
				
				_host?.FormsHost?.SetWindow(SciterWnd);
				
				var loadHtmlEventArgs = new LoadHtmlEventArgs()
				{
					Html = this.Html
				};

				LoadHtml?.Invoke(this, loadHtmlEventArgs);

				SciterWnd.LoadHtml(loadHtmlEventArgs?.Html ?? this.Html ?? DEFAULT_HTML);

				SciterWnd.Show();
			}

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