using SciterCore.Attributes;

namespace SciterCore.SkiaSharp.Behaviors
{
    [SciterBehavior("draw-linear-foreground")]
    internal class LinearForegroundBitmapBehavior : LinearBitmapBehavior
    {

        public LinearForegroundBitmapBehavior()
            : base(DrawEvent.Foreground)
        {

        }
    }
}