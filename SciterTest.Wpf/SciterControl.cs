using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using SciterCore;
using SciterCore.Interop;

namespace SciterTest.Wpf
{
    public class SciterControl : HwndHost 
    {
        private static readonly string DefaultHtml = 
            $"<html window-frame=\"none\" window-blurbehind=\"dark\"  theme=\"dark\"><head><style> html {{ background: transparent }} </style></head><body><code>Use the <b>LoadHtml</b> event of {nameof(SciterControl)} to load some html.</code>" + 
            "<br/><br/>" +
            $"<pre><code>    {nameof(SciterControl)}.LoadHtml += (sender, args) => <br/>" + 
            "    {<br/>" + 
            "        args.Html = \"&lt;body&gt;Hello &lt;b&gt;World&lt;/b&gt;&lt;/body&gt;\";<br/>" + 
            "    }</code></pre><a href=\"https://www.google.com\">Google</a></body></html>";

        public SciterWindow SciterWnd { get; private set; }

        public SciterControl()
        {

        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            //SciterWnd = new SciterWindow(hwndParent.Handle);
            //SciterWnd.CreateChildWindow(hwndParent.Handle, SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CHILD | SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ALPHA);
            //SciterWnd.LoadHtml(/*loadHtmlEventArgs?.Html ?? this.Html ??*/ DefaultHtml);
            ////SciterWnd.Show();

            //return new HandleRef(this, SciterWnd.Handle);

            string wndclass = Marshal.PtrToStringUni(Sciter.Api.SciterClassName());
            var child = PInvokeWindows.CreateWindowEx((int)(PInvokeWindows.WindowStyles.WS_EX_TRANSPARENT), wndclass, null, (int)(PInvokeWindows.WindowStyles.WS_CHILD), (int)VisualOffset.X, (int)VisualOffset.Y, (int)ActualWidth, (int)ActualHeight,
                hwndParent.Handle, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            var bytes = Encoding.UTF8.GetBytes(DefaultHtml);
            Sciter.Api.SciterLoadHtml(child, bytes, (uint)bytes.Length, null);

            return new HandleRef(this, child);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            
        }

        //private void PresentationSourceOnContentRendered(object sender, EventArgs e)
        //{
        //    HwndSource source = (HwndSource)sender;
        //    SciterWnd = new SciterWindow(source.Handle);
        //    SciterWnd.CreateChildWindow(source.Handle);
        //    SciterWnd.LoadHtml(/*loadHtmlEventArgs?.Html ?? this.Html ??*/ DefaultHtml);
        //    SciterWnd.Show();
        //    if (SciterWnd._hwnd.ToInt32() != 0)
        //    {
        //        //PInvokeWindows.MoveWindow(source.Handle, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
        //        PInvokeWindows.MoveWindow(SciterWnd._hwnd, (int)VisualOffset.X, (int)VisualOffset.Y, (int)ActualWidth, (int)ActualHeight, true);
        //    }
        //}
    }
}