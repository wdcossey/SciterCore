namespace SciterCore
{
    //[Flags]
    public enum MouseEvents : int
    {
        Enter = 0,
        Leave,
        Move,
        Up,
        Down,
        DoubleClick,
        Wheel,
        
        /// <summary>
        /// Mouse pressed ticks
        /// </summary>
        Tick,
        
        /// <summary>
        /// Mouse stay idle for some time
        /// </summary>
        Idle,
        
        /// <summary>
        /// Item dropped, target is that dropped item 
        /// </summary>
        Drop        = 9,
        
        /// <summary>
        /// Drag arrived to the target element that is one of current drop targets.
        /// </summary>
        DragEnter  = 0xA,
        
        /// <summary>
        /// Drag left one of current drop targets. target is the drop target element.
        /// </summary>
        DragLeave  = 0xB,
        
        /// <summary>
        /// Drag src notification before drag start. To cancel - return true from handler.
        /// </summary>
        DragRequest = 0xC,
        
        /// <summary>
        /// mouse click event
        /// </summary>
        MouseClick = 0xFF,
        
        /// <summary>
        /// <para>This flag is 'ORed' with MOUSE_ENTER..MOUSE_DOWN codes if dragging operation is in effect.</para>
        /// E.g. event DRAGGING | MOUSE_MOVE is sent to underlying DOM elements while dragging.
        /// </summary>
        Dragging = 0x100,
    }
}