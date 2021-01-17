using System;
using System.Windows;
using SciterCore.Interop;
using SciterTest.Wpf;

namespace SciterCore.Windows.Wpf
{
    public class WpfSciterHost : SciterHost
    {
        private readonly SciterControl _control;

        public WpfSciterHost(SciterControl control)
        {
            _control = control;
        }

        protected override bool OnAttachBehavior(SciterElement element, string behaviorName, out SciterEventHandler eventHandler)
        {
            return base.OnAttachBehavior(element, behaviorName, out eventHandler);
        }

        protected override LoadResult OnLoadData(object sender, LoadDataArgs args)
        {
            Uri uri = new Uri(args.Uri.AbsolutePath, UriKind.Relative);
  
            var info = Application.GetContentStream(uri);

            info.Stream.Seek(0, System.IO.SeekOrigin.Begin);
            var buffer = new byte[info.Stream.Length];

            info.Stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false).GetAwaiter().GetResult();

            Sciter.SciterApi.SciterDataReady(Window.Handle, uri.OriginalString, buffer, (uint) buffer.Length);
            
            return base.OnLoadData(sender, args);
        }
    }
}