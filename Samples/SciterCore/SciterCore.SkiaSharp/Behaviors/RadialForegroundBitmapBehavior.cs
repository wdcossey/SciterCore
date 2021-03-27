using SciterCore.Attributes;

namespace SciterCore.SkiaSharp.Behaviors
{
    [SciterBehavior("draw-radial-foreground")]
    internal class RadialForegroundBitmapBehavior : RadialBitmapBehavior
    {
        public RadialForegroundBitmapBehavior()
            :base(DrawEvent.Foreground)
        {

        }
    }
}