using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Resources;
using SciterCore;
using SciterCore.Interop;

namespace SciterTest.Wpf
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

        public SciterWindow SciterWnd { get; private set; }
        
        private IntPtr _sciterHandle = IntPtr.Zero;
        

        public SciterControl()
        {

        }

        public static readonly DependencyProperty SourceProperty =
           DependencyProperty.Register(
               nameof(Source), typeof(object), typeof(SciterControl),
               new FrameworkPropertyMetadata(DefaultHtml, new PropertyChangedCallback(OnSourceChanged)));

        private static async void OnSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SciterControl control = (SciterControl)obj;
            if (!_hwnd.Equals(IntPtr.Zero))
            {
                Uri uri = new Uri($"{args.NewValue}", UriKind.Relative);
                StreamResourceInfo info = Application.GetContentStream(uri);

                info.Stream.Seek(0, System.IO.SeekOrigin.Begin);
                var buffer = new byte[info.Stream.Length];

                await info.Stream.ReadAsync(buffer, 0, buffer.Length);
                Sciter.SciterApi.SciterLoadHtml(control.Handle, buffer, (uint)buffer.Length, null);
            }
        }

        /// <summary>
        /// Gets or sets the value assigned to the control.
        /// </summary>
        public object Source
       {          
           get { return (string)GetValue(SourceProperty); }
           set { SetValue(SourceProperty, value); }
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
            //SciterWnd = new SciterWindow(hwndParent.Handle);
            //SciterWnd.CreateChildWindow(hwndParent.Handle, SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CHILD | SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE | SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ALPHA);
            ////SciterWnd.LoadHtml(/*loadHtmlEventArgs?.Html ?? this.Html ??*/ DefaultHtml);
            ////SciterWnd.Show();

            //var result = new HandleRef(this, SciterWnd.Handle);

            //_hwnd = result.Handle;

            //OnSourceChanged(this, new DependencyPropertyChangedEventArgs(SourceProperty, null, Source));

            //return result;

            string wndclass = Sciter.SciterApi.SciterClassName();
            _sciterHandle = PInvokeWindows.CreateWindowEx(
                (int)0,
                wndclass,
                null,
                (int)(PInvokeWindows.WindowStyles.WS_CHILD),
                (int)VisualOffset.X,
                (int)VisualOffset.Y,
                (int)ActualWidth,
                (int)ActualHeight,
                hwndParent.Handle, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

            //var bytes = Encoding.UTF8.GetBytes(Html);

            //Sciter.Api.SciterLoadHtml(child, bytes, (uint)bytes.Length, null);

            var result = new HandleRef(this, _sciterHandle);

            _hwnd = result.Handle;

            OnSourceChanged(this, new DependencyPropertyChangedEventArgs(SourceProperty, null, Source));

            return result;
        }

        private static IntPtr _hwnd = IntPtr.Zero;

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            PInvokeWindows.DestroyWindow(hwnd: hwnd.Handle);
        }

        //private void PresentationSourceOnContentRendered(object sender, EventArgs e)
        //{
        //    HwndSource source = (HwndSource)sender;
        //    SciterWnd = new SciterWindow(source.Handle);
        //    SciterWnd.CreateChildWindow(source.Handle);
        //    SciterWnd.LoadHtml(/*loadHtmlEventArgs?.Html ?? this.Html ??*/ DefaultHtml);
        //    SciterWnd.Show();
        //    if (SciterWnd.Handle.ToInt32() != 0)
        //    {
        //        //PInvokeWindows.MoveWindow(source.Handle, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
        //        PInvokeWindows.MoveWindow(SciterWnd.Handle, (int)VisualOffset.X, (int)VisualOffset.Y, (int)ActualWidth, (int)ActualHeight, true);
        //    }
        //}
    }
}