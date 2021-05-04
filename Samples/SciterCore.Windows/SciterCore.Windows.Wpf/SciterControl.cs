using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Resources;
using SciterCore.Interop;

namespace SciterCore.Windows.Wpf
{
    public class SciterControl : HwndHost
    {
        private static readonly string DefaultHtml = 
            $"<html window-frame=\"none\" window-blurbehind=\"none\" theme=\"dark\"><head><style> html {{ background: transparent }} </style></head><body><code>Use the <b>LoadHtml</b> event of {nameof(SciterControl)} to load some html.</code>" + 
            "<br/><br/>" +
            $"<pre><code>    {nameof(SciterControl)}.LoadHtml += (sender, args) => <br/>" + 
            "    {<br/>" + 
            "        args.Html = \"&lt;body&gt;Hello &lt;b&gt;World&lt;/b&gt;&lt;/body&gt;\";<br/>" + 
            "    }</code></pre></body></html>";

        public SciterControl()
        {
            Host = new SciterArchiveHost();
            Content = DefaultHtml;
        }
        
        public SciterWindow SciterWindow { get; private set; }
        
        public SciterArchiveHost Host { get; set; }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
                nameof(Content), typeof(object), typeof(SciterControl),
                new FrameworkPropertyMetadata(DefaultHtml, new PropertyChangedCallback(OnContentChanged)));

        private static void OnContentChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (!(obj is SciterControl control) || (control.SciterWindow == null || control.SciterWindow.WindowHandle.Equals(IntPtr.Zero)))
                return;

            if (Uri.TryCreate($"{args.NewValue}", UriKind.Absolute, out var uri))
            {
                control.Host.Window.LoadPage(uri);
                return;
            }
            
            control.Host.Window.LoadHtml((string)args.NewValue);
        }

        public object Content
        {
            get => (string) GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

       // public static readonly DependencyProperty HtmlProperty =
       //    DependencyProperty.Register(
       //        "Html", typeof(string), typeof(SciterControl),
       //        new FrameworkPropertyMetadata(DefaultHtml, new PropertyChangedCallback(OnHtmlChanged)));

       // private static void OnHtmlChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
       // {
       //     SciterControl control = (SciterControl)obj;
       //     var bytes = Encoding.UTF8.GetBytes((string)args.NewValue);
       //     Sciter.Api.SciterLoadHtml(control.Handle, bytes, (uint)bytes.Length, null);
       // }

       // /// <summary>
       // /// Gets or sets the value assigned to the control.
       // /// </summary>
       // public string Html
       //{          
       //    get { return (string)GetValue(HtmlProperty); }
       //    set { SetValue(HtmlProperty, value); }
       //}

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            SciterWindow = SciterWindow
                .CreateChildWindow(hwndParent.Handle);
            
            Host.SetupWindow(SciterWindow.Handle)
                .AttachEventHandler<WpfHostEventHandler>();

            OnContentChanged(this, new DependencyPropertyChangedEventArgs(ContentProperty, null, Content));

            return new HandleRef(this, SciterWindow.Handle);
        }

        private static IntPtr _hwnd = IntPtr.Zero;

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            PInvokeWindows.DestroyWindow(hwnd: hwnd.Handle);
        }
    }
}