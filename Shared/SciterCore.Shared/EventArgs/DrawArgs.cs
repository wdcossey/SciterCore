using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SciterCore
{
	/// <summary>
	/// DRAW_PARAMS
	/// </summary>
    public struct DrawArgs
    {
	    public DrawEvent DrawEvent { get; internal set; }
        
        /// <summary>
        /// Handle of the <see cref="SciterGraphics"/> object
        /// </summary>
		public IntPtr Handle { get; internal set; }
		
		/// <summary>
		/// <see cref="SciterElement"/> area, to get invalid area to paint use GetClipBox,
		/// </summary>
		public SciterRectangle Area { get; internal set; }
		
		/// <summary>
		/// <para>For <see cref="SciterCore.DrawEvent.Background"/>/<see cref="SciterCore.DrawEvent.Foreground"/> - it is a border box.</para><br/>
		/// For <see cref="SciterCore.DrawEvent.Content"/> - it is a content box.
		/// </summary>
		public int Reserved { get; internal set; }
    }
}