using System;

namespace SciterCore
{
    public class MouseEventArgs : EventArgs
    {
        /// <summary>
        /// MOUSE_EVENTS
        /// </summary>
        public MouseEvents Event { get; internal set; }
        
        /// <summary>
        /// Target Element
        /// </summary>
        public SciterElement TargetElement { get; internal set; }
        
        /// <summary>
        /// Position of cursor relative to the Element
        /// </summary>
        public SciterPoint ElementPosition { get; internal set; }
        
        /// <summary>
        /// Position of cursor relative to the View
        /// </summary>
        public SciterPoint ViewPosition { get; internal set; }
        
        /// <summary>
        /// Actually SciterXBehaviors MOUSE_BUTTONS, but for MOUSE_EVENTS.MOUSE_WHEEL event it is the delta
        /// </summary>
        public MouseButton ButtonState { get; internal set; }
        
        /// <summary>
        /// KEYBOARD_STATES
        /// </summary>
        public KeyboardStates KeyboardState { get; internal set; }
        
        /// <summary>
        /// CURSOR_TYPE to set, see CURSOR_TYPE
        /// </summary>
        public CursorType Cursor { get; internal set; }
        
        /// <summary>
        /// Mouse is over icon (foreground-image, foreground-repeat:no-repeat)
        /// </summary>
        public bool	IsOverIcon { get; internal set; }
        
        /// <summary>
        /// <para>Element that is being dragged over.</para>
        /// This field is not <b>null</b> if <see cref="Event"/> &amp; <see cref="DragMode"/> != 0
        /// </summary>
        public SciterElement DragTarget { get; internal set; }
        
        /// <summary>
        /// See DRAGGING_TYPE
        /// </summary>
        public DraggingMode	DragMode { get; internal set; }
    }
}