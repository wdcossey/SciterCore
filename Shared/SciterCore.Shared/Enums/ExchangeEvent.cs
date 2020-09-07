namespace SciterCore
{
    public enum ExchangeEvent : int
    {
        /// <summary>
        /// Drag enters the <see cref="SciterElement"/>  
        /// </summary>
        DragEnter = 0,
        
        /// <summary>
        /// Drag leaves the <see cref="SciterElement"/>  
        /// </summary>
        DragLeave = 1,
        
        /// <summary>
        /// Drag over the <see cref="SciterElement"/>  
        /// </summary>
        Drag = 2,
        
        /// <summary>
        /// Data dropped on the <see cref="SciterElement"/>  
        /// </summary>
        Drop = 3,
        
        /// <summary>
        /// N/A
        /// </summary>
        Paste = 4,
       
        /// <summary>
        /// N/A
        /// </summary>
        DragRequest = 5,
        
        /// <summary>
        /// Drag cancelled (e.g. by pressing VK_ESCAPE)
        /// </summary>
        DragCancel = 6,
        
        /// <summary>
        /// Drop <see cref="ExchangeEventArgs.TargetElement"/> shall consume this event in order to receive <see cref="Drop"/>
        /// </summary>
        WillAcceptDrop = 7,
    }
}