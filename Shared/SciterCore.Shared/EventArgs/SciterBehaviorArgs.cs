using System;

namespace SciterCore
{
    public struct SciterBehaviorArgs
    {
        /// <summary>
        /// BEHAVIOR_EVENTS
        /// </summary>
        public BehaviorEvents Command { get; set; }
			
        /// <summary>
        /// target element handler, in MENU_ITEM_CLICK this is owner element that caused this menu - e.g. context menu owner
        /// In scripting this field named as Event.owner
        /// </summary>
        public SciterElement Target { get; set; }
			
        /// <summary>
        /// source element e.g. in SELECTION_CHANGED it is new selected &lt;option&gt;, in MENU_ITEM_CLICK it is menu item (LI) element
        /// </summary>
        public SciterElement Source { get; set; }
			
        /// <summary>
        /// CLICK_REASON or EDIT_CHANGED_REASON - UI action causing change.
        /// In case of custom event notifications this may be any application specific value.
        /// </summary>
        public IntPtr Reason { get; set; } // UINT_PTR
			
        /// <summary>
        /// auxiliary data accompanied with the event. E.g. FORM_SUBMIT event is using this field to pass collection of values.
        /// </summary>
        public SciterValue Data { get; set; }// SCITER_VALUE

        /// <summary>
        /// Name of custom event (when <see cref="Command"/> == <see cref="BehaviorEvents.Custom"/>)
        /// </summary>
        public string Name { get; set; }
    }
}