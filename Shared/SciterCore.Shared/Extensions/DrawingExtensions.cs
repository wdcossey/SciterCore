using System.Drawing;

namespace SciterCore.Interop
{
    internal static class DrawingExtensions
    {
        internal static PInvokeUtils.RECT ToRect(this Rectangle rectangle)
        {
            return new PInvokeUtils.RECT
            {
                Left = rectangle.Left,
                Top = rectangle.Top,
                Right = rectangle.Right,
                Bottom = rectangle.Bottom
            };
        }
        
        internal static Rectangle ToRectangle(this PInvokeUtils.RECT rect)
        {
            return new Rectangle(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }
        
        internal static Size ToSize(this PInvokeUtils.RECT rect)
        {
            return new Size(rect.Width, rect.Height);
        }
        
    }
}