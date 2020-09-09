using System;

namespace SciterCore
{
    [Flags]
    public enum EventGroups : int
    {
        /// <summary>
        /// Attached/Detached
        /// </summary>
        HandleInitialization        = 0x0000,
        
        /// <summary>
        /// Mouse events
        /// </summary>
        HandleMouse                 = 0x0001, 
        
        /// <summary>
        /// Key events
        /// </summary>
        HandleKey                   = 0x0002,
        
        /// <summary>
        /// Focus events, if this flag is set it also means that element it attached to is focusable
        /// </summary>
        HandleFocus                 = 0x0004,
        
        /// <summary>
        /// Scroll events
        /// </summary>
        HandleScroll                = 0x0008,
        
        /// <summary>
        /// Timer event
        /// </summary>
        HandleTimer                 = 0x0010,
        
        /// <summary>
        /// Size changed event
        /// </summary>
        HandleSize                   = 0x0020,
        
        /// <summary>
        /// Drawing request (event)
        /// </summary>
        HandleDraw                  = 0x0040,
        
        /// <summary>
        /// Requested data () has been delivered
        /// </summary>
        HandleDataArrived           = 0x080,
        
        /// <summary>
        /// <para>
        /// Logical, Synthetic events:<br/>
        /// BUTTON_CLICK, HYPERLINK_CLICK, etc.,<br/>
        /// a.k.a. notifications from intrinsic behaviors
        /// </para>
        /// </summary>
        HandleBehaviorEvent         = 0x0100,
													
													
        /// <summary>
        /// Behavior specific methods
        /// </summary>
        HandleMethodCall            = 0x0200,
        
        /// <summary>
        /// Behavior specific methods
        /// </summary>
        HandleScriptingMethodCall   = 0x0400,
        
        /// <summary>
        /// <para>Behavior specific methods using direct tiscript::value's</para>
        /// </summary>
        [Obsolete("As of Sciter 4.4.3.24")]
        // ReSharper disable once InconsistentNaming
        HandleTIScriptMethodCall    = 0x0800,
        
        /// <summary>
        /// System Drag-n-Drop
        /// </summary>
        HandleExchange              = 0x1000,
        
        /// <summary>
        /// Touch input events
        /// </summary>
        HandleGesture               = 0x2000,

        /// <summary>
        /// som_asset_t request
        /// </summary>
        HandleSom                   = 0x8000,

        /// <summary>
        /// All of them
        /// </summary>
        HandleAll                   = 0xFFFF,

        //TODO: Verify that unchecked() has the desired effect!
        /// <summary>
        /// Special value for getting subscription flags
        /// </summary>
        SubscriptionsRequest        = unchecked((int)0xFFFFFFFF), /*  */
    }
}