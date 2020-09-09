using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SciterCore
{
    /// <summary>
    /// KEY_PARAMS
    /// </summary>
    public class KeyArgs : EventArgs
    {
        /// <summary>
        /// KEY_EVENTS
        /// </summary>
        public KeyEvent	Event { get; internal set; }
        
        /// <summary>
        /// Target element
        /// </summary>
        public SciterElement TargetElement { get; internal set; }
        
        /// <summary>
        /// key scan code, or character unicode for KEY_CHAR
        /// </summary>
        public int KeyCode { get; internal set; }
        
        /// <summary>
        /// KEYBOARD_STATES
        /// </summary>
        public KeyboardStates KeyboardState { get; internal set; }
    }
}