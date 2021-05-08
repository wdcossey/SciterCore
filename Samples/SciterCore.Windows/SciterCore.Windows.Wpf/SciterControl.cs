using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using SciterCore.Interop;

namespace SciterCore.Windows.Wpf
{
    public class SciterControl : HwndHost
    {
        private static readonly string DefaultHtml =
            "<html theme=\"dark\"><head><style> html {{ background: transparent }} " +
            "</style></head><body>" +
            $"<code>Use the <b>{nameof(Content)}</b> property of this <b>{nameof(SciterControl)}</b> to load a page or content.</code>" +
            "<br/><br/>" +
            "<pre><code>" +
            "    &lt;sciter:SciterControl <b>Content</b>=<i>&quot;this://app/index.html&quot;</i>&gt; <br/>" +
            "    <br/>" +
            "    &lt;/sciter:SciterControl&gt;" +
            "    <br/><br/>" +
            "    <b>OR</b>" +
            "    <br/><br/>" +
            "    &lt;sciter:SciterControl&gt; <br/>" +
            "        &lt;sciter:SciterControl.Content&gt; <br/>" +
            "            Hello WPF!<br/>" +
            "        &lt;/sciter:SciterControl.Content&gt; <br/>" +
            "    &lt;/sciter:SciterControl&gt;" +
            "    </code></pre>" +
            "</body></html>";

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

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            SciterWindow = SciterWindow
                .CreateChildWindow(hwndParent.Handle);
            
            Host.SetupWindow(SciterWindow.Handle)
                .AttachEventHandler<WpfHostEventHandler>();

            OnContentChanged(this, new DependencyPropertyChangedEventArgs(ContentProperty, null, Content));

            return new HandleRef(this, SciterWindow.Handle);
        }
        
        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            PInvokeWindows.DestroyWindow(hwnd: hwnd.Handle);
        }
    }
}