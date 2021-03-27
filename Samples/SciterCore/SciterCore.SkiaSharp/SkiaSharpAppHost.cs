using System;
using SciterCore.Attributes;
using SciterCore.SkiaSharp.Behaviors;

namespace SciterCore.SkiaSharp
{
    [SciterHostBehaviorHandler(typeof(DrawBitmapBehavior))]
    [SciterHostBehaviorHandler(typeof(InfoBitmapBehavior))]
    [SciterHostBehaviorHandler(typeof(SolidBitmapBehavior))]
    [SciterHostBehaviorHandler(typeof(SolidForegroundBitmapBehavior))]
    [SciterHostBehaviorHandler(typeof(LinearBitmapBehavior))]
    [SciterHostBehaviorHandler(typeof(LinearForegroundBitmapBehavior))]
    [SciterHostBehaviorHandler(typeof(RadialBitmapBehavior))]
    [SciterHostBehaviorHandler(typeof(RadialForegroundBitmapBehavior))]
    [SciterHostWindow(homePage: "this://app/index.html", 800, 600)]
    public class SkiaSharpAppHost : SciterArchiveHost
    {
        public SkiaSharpAppHost()
        {
            OnCreated += OnCreatedHost;
        }

        private void OnCreatedHost(object sender, HostCreatedEventArgs args)
        {
            args.Window.LoadPage(new Uri("this://app/index.html"));
            args.Window.CenterWindow();
        }
    }
}