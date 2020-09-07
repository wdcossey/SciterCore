using System;

namespace SciterCore
{
    public class ExchangeEventArgs : EventArgs
    {

        /// <summary>
        /// EXCHANGE_EVENTS
        /// </summary>
        public ExchangeEvent Event { get; internal set; }

        /// <summary>
        /// Target Element
        /// </summary>
        public SciterElement TargetElement { get; internal set; }
        
        /// <summary>
        /// <para>Source Element</para>
        /// Can be null if DragAndDrop is from external window.
        /// </summary>
        public SciterElement SourceElement { get; internal set; }

        /// <summary>
        /// Position of cursor relative to the Element
        /// </summary>
        public SciterPoint ElementPosition { get; internal set; }
        
        /// <summary>
        /// Position of cursor relative to the View
        /// </summary>
        public SciterPoint ViewPosition { get; internal set; }
        
        /// <summary>
        /// DD_MODE
        /// </summary>
        public DragAndDropMode Mode { get; internal set; }
        
        /// <summary>
        /// Packaged drag data
        /// </summary>
        public SciterValue Value { get; internal set; }
    }
}