﻿using System;

namespace SciterCore
{
    /// <summary>
    /// GESTURE_STATE
    /// </summary>
    [Flags]
    public enum GestureState
    {
        /// <summary>
        /// Starts
        /// </summary>
        Begin   = 1,
        
        /// <summary>
        /// Events generated by inertia processor
        /// </summary>
        Inertia = 2,
        
        /// <summary>
        /// End, last event of the gesture sequence
        /// </summary>
        End     = 4,
    }
}