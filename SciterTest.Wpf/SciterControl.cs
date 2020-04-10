using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using SciterCore;
using SciterCore.Interop;

namespace SciterTest.Wpf
{
    public class SciterControl : FrameworkElement
    {
        private static string DEFAULT_HTML = 
            $"<body><code>Use the <b>LoadHtml</b> event of {nameof(SciterControl)} to load some html.</code>" + 
            "<br/><br/>" +
            $"<pre><code>    {nameof(SciterControl)}.LoadHtml += (sender, args) => <br/>" + 
            "    {<br/>" + 
            "        args.Html = \"&lt;body&gt;Hello &lt;b&gt;World&lt;/b&gt;&lt;/body&gt;\";<br/>" + 
            "    }</code></pre>";

        public SciterWindow SciterWnd { get; private set; }

        public SciterControl()
        {
            SciterWnd = new SciterWindow();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            PresentationSource presentationSource = PresentationSource.FromVisual((Visual)sender);
            if (presentationSource != null)
            {
                presentationSource.ContentRendered += PresentationSourceOnContentRendered;
            }
        }

        private void PresentationSourceOnContentRendered(object sender, EventArgs e)
        {
            HwndSource source = (HwndSource)sender;

            SciterWnd.CreateChildWindow(source.Handle);
            SciterWnd.LoadHtml(/*loadHtmlEventArgs?.Html ?? this.Html ??*/ DEFAULT_HTML);
            SciterWnd.Show();
            
            if(SciterWnd._hwnd.ToInt32()!=0)
            {
                PInvokeWindows.MoveWindow(SciterWnd._hwnd, 0, 0, (int)ActualWidth, (int)ActualHeight, true);
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    base.OnSourceInitialized(e);
        //    var handle = new WindowInteropHelper(this).Handle;
        //    // -- or --
        //    var hwndSource = (HwndSource)PresentationSource.FromVisual(this);
        //    var handle = hwndSource.Handle;
        //}

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo info)
        {
            base.OnRenderSizeChanged(info);

            if(SciterWnd._hwnd.ToInt32()!=0)
            {
                PInvokeWindows.MoveWindow(SciterWnd._hwnd, 0, 0, (int)info.NewSize.Width, (int)info.NewSize.Height, true);
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            //HwndSource source = (HwndSource)PresentationSource.FromVisual(this);
    
            //SciterWnd.CreateChildWindow(source.Handle);

            ////var loadHtmlEventArgs = new LoadHtmlEventArgs()
            ////{
            ////    Html = this.Html
            ////};

            ////LoadHtml?.Invoke(this, loadHtmlEventArgs);

            //SciterWnd.LoadHtml(/*loadHtmlEventArgs?.Html ?? this.Html ??*/ DEFAULT_HTML);

            //SciterWnd.Show();
        }


    }
}