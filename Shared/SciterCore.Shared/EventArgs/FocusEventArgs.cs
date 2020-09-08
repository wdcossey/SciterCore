using System;

namespace SciterCore
{
    public class FocusEventArgs : EventArgs
    {
        public FocusEvents Event { get; internal set; }
        
        public SciterElement TargetElement { get; internal set; }
        
        public bool IsMouseClick { get; internal set; }
        
        public bool Cancel { get; internal set; }
    }
}