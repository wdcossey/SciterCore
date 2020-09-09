namespace SciterCore
{
    /// <summary>
    /// GESTURE_PARAMS
    /// </summary>
    public class GestureArgs
    {
        /// <summary>
        /// GESTURE_EVENTS
        /// </summary>
        public GestureEvent Event { get; internal set; }
        
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
        /// <para>for GESTURE_REQUEST combination of GESTURE_FLAGs.</para>
        /// <para>for others it is a combination of GESTURE_STATEs</para>
        /// </summary>
        public int Flags { get; internal set; }
        
        /// <summary>
        /// Period of time from previous event.
        /// </summary>
        public int DeltaTime { get; internal set; } 
        
        /// <summary>
        /// For GESTURE_PAN it is a direction vector 
        /// </summary>
        public SciterSize DeltaXY { get; internal set; } 
        
        /// <summary>
        /// <para>for GESTURE_ROTATE - delta angle (radians) </para>
        /// <para>for GESTURE_ZOOM - zoom value, is less or greater than 1.0 </para>
        /// </summary>
        public double DeltaV { get; internal set; }

        //public override string ToString()
        //{
        //    return $"{nameof(Event)}: {Event} | {nameof(Flags)}: {Flags} {(GestureTypeFlags)Flags} {(GestureState)Flags} | {nameof(DeltaXY)}: {DeltaXY} | {nameof(DeltaTime)}: {DeltaTime} | {nameof(DeltaV)}: {DeltaV}";
        //}
    }
}