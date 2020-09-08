using System;

namespace SciterCore
{
    public class DrawEventArgs
    {
	    public DrawEvent DrawEvent;
        
        /// <summary>
        /// Handle of the <see cref="SciterGraphics"/> object
        /// </summary>
		public IntPtr Handle;
		
		/// <summary>
		/// <see cref="SciterElement"/> area, to get invalid area to paint use GetClipBox,
		/// </summary>
		public SciterRectangle Area;
		
		/// <summary>
		/// <para>For <see cref="SciterCore.DrawEvent.Background"/>/<see cref="SciterCore.DrawEvent.Foreground"/> - it is a border box.</para><br/>
		/// For <see cref="SciterCore.DrawEvent.Content"/> - it is a content box.
		/// </summary>
		public uint Reserved;
    }
}