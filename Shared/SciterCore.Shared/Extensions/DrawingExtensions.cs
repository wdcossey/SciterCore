namespace SciterCore.Interop
{
    internal static class DrawingExtensions
    {
        internal static PInvokeUtils.RECT ToRect(this SciterRectangle rectangle)
        {
            return new PInvokeUtils.RECT
            {
                Left = rectangle.Left,
                Top = rectangle.Top,
                Right = rectangle.Right,
                Bottom = rectangle.Bottom
            };
        }
        
        internal static SciterRectangle ToRectangle(this PInvokeUtils.RECT rect)
        {
            return new SciterRectangle(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }
        
        internal static SciterSize ToSize(this PInvokeUtils.RECT rect)
        {
            return new SciterSize(rect.Width, rect.Height);
        }
        
    }
}