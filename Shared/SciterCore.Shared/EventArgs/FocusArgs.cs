// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SciterCore
{
    /// <summary>
    /// FOCUS_PARAMS
    /// </summary>
    public struct FocusArgs
    {
        public FocusEvents Event { get; internal set; }
        
        public SciterElement TargetElement { get; internal set; }
        
        public bool IsMouseClick { get; internal set; }
        
        public bool Cancel { get; internal set; }
    }
}