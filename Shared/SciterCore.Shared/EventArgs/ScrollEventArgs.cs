using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SciterCore
{
    public class ScrollEventArgs : EventArgs
    {
        public ScrollEvents Event { get; internal set; }
        
        public SciterElement TargetElement { get; internal set; }
        
        public int Position { get; internal set; }
        
        public bool IsVertical { get; internal set; }
        
        /// <summary>
        /// SCROLL_SOURCE
        /// </summary>
        public ScrollSource	Source { get; internal set; }
        
        /// <summary>
        /// Key or SCROLLBAR_PART
        /// </summary>
        public int Reason { get; internal set; }
    }
}