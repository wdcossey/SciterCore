using SciterCore.Attributes;

namespace SciterCore.SkiaSharp.Behaviors
{
    [SciterBehavior("draw-solid-foreground")]
    internal class SolidForegroundBitmapBehavior : SolidBitmapBehavior
    {

        public SolidForegroundBitmapBehavior()
            : base(DrawEvent.Foreground)
        {

        }
    }
}