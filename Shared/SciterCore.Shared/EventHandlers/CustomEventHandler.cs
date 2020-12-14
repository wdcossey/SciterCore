using System;
using SciterCore.Interop;

namespace SciterCore.EventHandlers
{
    internal class CustomEventHandler : SciterEventHandler
    {
        private readonly SciterElement _element;
        private readonly Action<string, SciterElement, SciterElement, SciterValue> _callbackAction;

        public CustomEventHandler(SciterElement element, Action<string, SciterElement, SciterElement, SciterValue> callbackAction)
        {
            _element = element;
            _callbackAction = callbackAction;
        }
        
        protected override EventGroups SubscriptionsRequest(SciterElement element)
        {
            return EventGroups.HandleBehaviorEvent;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceElement"><para>Source element e.g. in SELECTION_CHANGED it is new selected &lt;option&gt;, in MENU_ITEM_CLICK it is menu item (LI) element</para></param>
        /// <param name="targetElement"><para>Target element, in MENU_ITEM_CLICK this is owner element that caused this menu - e.g. context menu owner<br/>In scripting this field named as Event.owner</para></param>
        /// <param name="type"></param>
        /// <param name="reason"><para>CLICK_REASON or EDIT_CHANGED_REASON - UI action causing change.<br/>In case of custom event notifications this may be any application specific value.</para></param>
        /// <param name="data"><para>Auxiliary data accompanied with the event. E.g. FORM_SUBMIT event is using this field to pass collection of values.</para></param>
        /// <param name="eventName"><para>name of custom event (when <paramref name="type"></paramref>  == <see cref="BehaviorEvents"/>)</para></param>
        /// <returns></returns>
        protected override bool OnEvent(SciterElement sourceElement, SciterElement targetElement, BehaviorEvents type,
            IntPtr reason,
            SciterValue data, string eventName)
        {
            if (type == BehaviorEvents.Custom)
                _callbackAction?.Invoke(eventName, sourceElement, targetElement, data);

            return base.OnEvent(sourceElement, targetElement, type, reason, data, eventName);
        }
    }
}