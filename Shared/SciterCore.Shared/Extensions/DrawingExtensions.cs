namespace SciterCore.Interop
{
    internal static class DrawingExtensions
    {
        internal static PInvokeUtils.RECT ToRect(this SciterRectangle rectangle) =>
            new PInvokeUtils.RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);

        internal static SciterRectangle ToRectangle(this PInvokeUtils.RECT rect) =>
            new SciterRectangle(rect.Left, rect.Top, rect.Right, rect.Bottom);
        
        internal static SciterSize ToSize(this PInvokeUtils.RECT rect) => 
            new SciterSize(rect.Width, rect.Height);
        
        internal static SciterSize ToSize(this SciterRectangle rectangle) => 
            new SciterSize(rectangle.Width, rectangle.Height);
        
        public static PInvokeUtils.POINT ToPoint(this SciterPoint point) =>
            new PInvokeUtils.POINT(point.X, point.Y);
        
        public static SciterPoint ToPoint(this PInvokeUtils.POINT point) =>
            new SciterPoint(point.X, point.Y);
        
        public static PInvokeUtils.SIZE ToSize(this SciterSize size) =>
            new PInvokeUtils.SIZE(size.Width, size.Height);
        
        public static SciterSize ToSize(this PInvokeUtils.SIZE size) =>
            new SciterSize(size.cx, size.cy);
        
    }
}