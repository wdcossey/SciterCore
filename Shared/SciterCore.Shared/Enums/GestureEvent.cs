namespace SciterCore
{
    /// <summary>
    /// GESTURE_CMD
    /// </summary>
    public enum GestureEvent
    {
        /// <summary>
        /// Return true and fill flags if it will handle gestures.
        /// </summary>
        Request = 0,
        
        /// <summary>
        /// The zoom gesture.
        /// </summary>
        Zoom,
        
        /// <summary>
        /// The pan gesture.
        /// </summary>
        Pan,
        
        /// <summary>
        /// The rotation gesture.
        /// </summary>
        Rotate,
        
        /// <summary>
        /// The tap gesture.
        /// </summary>
        Tap1,
        
        /// <summary>
        /// The two-finger tap gesture.
        /// </summary>
        Tap2,
    }
}