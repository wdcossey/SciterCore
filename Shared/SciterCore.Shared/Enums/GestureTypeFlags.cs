using System;

namespace SciterCore
{
    /// <summary>
    /// GESTURE_TYPE_FLAGS - Requested
    /// </summary>
    [Flags]
    public enum GestureTypeFlags
    {
        /// <summary>
        /// 
        /// </summary>
        Zoom              = 0x0001,
        
        /// <summary>
        /// 
        /// </summary>
        Rotate            = 0x0002,
        
        /// <summary>
        /// 
        /// </summary>
        PanVertical       = 0x0004,
        
        /// <summary>
        /// 
        /// </summary>
        PanHorizontal     = 0x0008,
        
        /// <summary>
        /// press &amp; tap
        /// </summary>
        Tap1              = 0x0010,
        
        /// <summary>
        /// two fingers tap
        /// </summary>
        Tap2              = 0x0020,

        /// <summary>
        /// PAN_VERTICAL and PAN_HORIZONTAL modifiers
        /// </summary>
        PanWithGutter     = 0x4000,
        
        /// <summary>
        /// 
        /// </summary>
        PanWithInertia    = 0x8000,
        
        /// <summary>
        /// 
        /// </summary>
        All               = 0xFFFF,
    }
}