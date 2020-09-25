using System;
using System.Collections.Generic;
using System.Linq;
using SciterCore.Interop;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore
{
    public static class SciterElementExtensions
    {
        #region Enabled

        /// <summary>
        /// Is the <paramref name="element"/> visible
        /// </summary>
        /// <param name="element">The <see cref="SciterElement"/></param>
        /// <returns>true if Visible, false if not visible of the <paramref name="element"/> is null</returns>
        public static bool IsEnabled(this SciterElement element)
        {
            return element?.IsEnabledInternal() == true;
        }

        #endregion Enabled
        
        #region Visibility

        public static bool IsVisible(this SciterElement element)
        {
            return element?.IsVisibleInternal() == true;
        }

        #endregion

        #region Rendering

        public static SciterElement Update(this SciterElement element, bool forceRender = false)
        {
            element?.TryUpdate(forceRender: forceRender);
            return element;
        }
        
        public static bool TryUpdate(this SciterElement element, bool forceRender = false)
        {
            return element?.TryUpdateInternal(forceRender: forceRender) == true;
        }

        public static SciterElement Refresh(this SciterElement element)
        {
            element?.TryRefresh();
            return element;
        }

        public static bool TryRefresh(this SciterElement element)
        {
            return element?.TryRefreshInternal() == true;
        }

        public static SciterElement Refresh(this SciterElement element, SciterRectangle rectangle)
        {
            element?.TryRefresh(rectangle: rectangle);
            return element;
        }

        public static bool TryRefresh(this SciterElement element, SciterRectangle rectangle)
        {
            return element?.TryRefreshInternal(rectangle: rectangle) == true;
        }
        

        #endregion
        
        #region Element

        /// <summary>
        /// Creates, appends and returns a new <see cref="SciterElement"/> with the given <paramref name="tag"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag"></param>
        /// <param name="text"></param>
        /// <returns>The newly created child <see cref="SciterElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="parent"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If the <paramref name="tag"/> is null.</exception>
        public static SciterElement AppendChildElement(this SciterElement parent, string tag, string text = null)
        {
            return parent?.AppendChildElement(childElement: SciterElement.Create(tag: tag, text));
        }

        //TODO: Add TryAppendChildElement

        /// <summary>
        /// Appends and returns the the given <paramref name="childElement"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childElement"></param>
        /// <returns>The newly created child <see cref="SciterElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="parent"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If the <paramref name="childElement"/> is null.</exception>
        public static SciterElement AppendChildElement(this SciterElement parent, SciterElement childElement)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent), @"Parent element cannot be null.");
            
            if (childElement == null)
                throw new ArgumentNullException(nameof(childElement), @"Element cannot be null or empty.");
            
            parent.AppendElement(childElement);

            return childElement;
        }
        
        //TODO: Add TryAppendChildElement
        
        
        public static void Delete(this SciterElement element)
        {
            element?.TryDelete();
        }
        
        public static bool TryDelete(this SciterElement element)
        {
            return element?.TryDeleteInternal() == true;
        }

        public static SciterElement Detach(this SciterElement element)
        {
            element?.TryDetach();
            return element;
        }
        
        public static bool TryDetach(this SciterElement element)
        {
            return element?.TryDetachInternal() == true;
        }

        public static SciterElement Clone(this SciterElement element)
        {
            return element?.CloneInternal();
        }
        
        public static bool TryClone(this SciterElement element, out SciterElement clonedElement)
        {
            clonedElement = default;
            return element?.TryCloneInternal(out clonedElement) == true;
        }

        public static SciterElement GetChildAtIndex(this SciterElement element, int index)
        {
            return element.GetChildAtIndexInternal(index: index);
        }
        
        public static bool TryGetChildAtIndex(this SciterElement element, int index, out SciterElement childElement)
        {
            childElement = default;
            return element?.TryGetChildAtIndexInternal(index: index, out childElement) == true;
        }

        public static SciterNode CastToNode(this SciterElement element)
        {
            return element?.CastToNodeInternal();
        }

        public static bool TryCastToNode(this SciterElement element, out SciterNode node)
        {
            node = default;
            return element?.TryCastToNodeInternal(out node) == true;
        }

        #endregion Element

        #region Siblings

        public static SciterElement NextSibling(this SciterElement element)
        {
            return element?.NextSiblingInternal;
        }
        
        public static SciterElement PreviousSibling(this SciterElement element)
        {
            return element?.PreviousSiblingInternal;
        }
        
        public static SciterElement FirstSibling(this SciterElement element)
        {
            return element?.FirstSiblingInternal;
        }
        
        public static SciterElement LastSibling(this SciterElement element)
        {
            return element?.LastSiblingInternal;
        }
        
        #endregion

        #region Classes

        private const string ClassSeparator = " ";

        private static readonly string[] ClassSeparatorArray = { ClassSeparator };
        
        public static bool HasClass(this SciterElement element, params string[] input)
        {
            if (element == null || input?.Any() != true)
                return false;
            
            var classes = element.GetAttributeValue("class")?.Split(separator: ClassSeparatorArray, options: StringSplitOptions.RemoveEmptyEntries)?.ToList() ?? new List<string>();
            
            return classes.Any(x => input.Any(y => y == x));
        }

        public static SciterElement AddClass(this SciterElement element, params string[] input)
        {
            if (element == null || input?.Any() != true)
                return element;
            
            var classes = element.GetAttributeValue("class")?.Split(separator: ClassSeparatorArray, options: StringSplitOptions.RemoveEmptyEntries)?.ToList() ?? new List<string>();

            var modified = false;
                
            foreach (var newClass in input)
            {
                if (classes.Contains(newClass))
                    continue;

                classes.Add(newClass);
                modified = true;
            }

            if (modified)
                element.SetAttributeValue("class", string.Join(ClassSeparator, classes));
            
            return element;
        }

        public static SciterElement RemoveClass(this SciterElement element, params string[] input)
        {
            if (element == null || input?.Any() != true)
                return element;
            
            var classes = element.GetAttributeValue("class")?.Split(separator: ClassSeparatorArray, options: StringSplitOptions.RemoveEmptyEntries)?.ToList() ?? new List<string>();

            var modified = false;
                
            foreach (var oldClass in input)
            {
                if (!classes.Contains(oldClass))
                    continue;

                classes.Remove(oldClass);
                modified = true;
            }

            if (modified)
                element.SetAttributeValue("class", string.Join(" ", classes));
            
            return element;
        }

        //TODO: Implement ToggleClass
        //public static int ToggleClass(this SciterElement element, params string[] @class)
        //{
        //    return element?.GetAttributeCountInternal() ?? 0;
        //}

        #endregion
        
        #region Attributes

        public static int AttributeCount(this SciterElement element)
        {
            return element?.GetAttributeCountInternal() ?? 0;
        }

        public static string GetAttributeValue(this SciterElement element, int index)
        {
            return element?.GetAttributeValueInternal(index: index);
        }
        
        public static bool TryGetAttributeValue(this SciterElement element, int index, out string value)
        {
            value = default;
            return element?.TryGetAttributeValueInternal(index: index, value: out value) == true;
        }
        
        public static string GetAttributeValue(this SciterElement element, string key)
        {
            return element?.GetAttributeValueInternal(key: key);
        }
        
        public static bool TryGetAttributeValue(this SciterElement element, string key, out string value)
        {
            value = default;
            return element?.TryGetAttributeValueInternal(key: key, value: out value) == true;
        }
        
        public static string GetAttributeName(this SciterElement element, int index)
        {
            return element?.GetAttributeNameInternal(index: index);
        }
        
        public static bool TryGetAttributeName(this SciterElement element, int index, out string value)
        {
            value = default;
            return element?.TryGetAttributeNameInternal(index: index, out value) == true;
        }
        
        public static SciterElement RemoveAttribute(this SciterElement element, string key)
        {
            element?.RemoveAttributeInternal(key: key);
            return element;
        }

        public static SciterElement SetAttributeValue(this SciterElement element, string key, string value)
        {
            element?.SetAttributeValueInternal(key: key, value: value);
            return element;
        }
        
        public static SciterElement SetAttributeValue(this SciterElement element, string key, Func<string> func)
        {
            return element.SetAttributeValue(key: key, value: func?.Invoke());
        }

        public static SciterElement SetAttributeValue<T>(this SciterElement element, string key, Func<T, string> func, T @object)
        {
            return element.SetAttributeValue(key: key, value: func?.Invoke(@object));
        }
        
        public static bool TrySetAttributeValue(this SciterElement element, string key, string value)
        {
            return element?.TrySetAttributeValueInternal(key: key, value: value) == true;
        }
        
        public static bool TrySetAttributeValue(this SciterElement element, string key, Func<string> func)
        {
            return element.TrySetAttributeValue(key: key, value: func?.Invoke());
        }

        public static bool TrySetAttributeValue<T>(this SciterElement element, string key, Func<T, string> func, T @object)
        {
            return element.TrySetAttributeValue(key: key, value: func?.Invoke(@object));
        }
        
        #endregion Attributes
        
        #region Styles
        
        public static string GetStyleValue(this SciterElement element, string key)
        {
            return element?.GetStyleValueInternal(key: key);
        }
        
        public static bool TryGetStyleValue(this SciterElement element, string key, out string style)
        {
            style = null;
            return element?.TryGetStyleValueInternal(key: key, out style) == true;
        }
        
        public static SciterElement SetStyleValue(this SciterElement element, string key, string value)
        {
            element?.SetStyleValueInternal(key: key, value: value);
            return element;
        }
        
        public static bool TrySetStyleValue(this SciterElement element, string key, string value)
        {
            return element?.TrySetStyleValueInternal(key: key, value: value) == true;
        }
        
        #endregion Styles

        #region Url
        
        public static string CombineUrl(this SciterElement element, string url = "")
        {
            return element.CombineUrlInternal(url: url);
        }
        
        public static bool TryCombineUrl(this SciterElement element, string url, out string value)
        {
            return element.TryCombineUrlInternal(url: url, value: out value);
        }
        
        #endregion Url

        #region Child
        
        public static int GetChildCount(this SciterElement element)
        {
            return element?.GetChildCountInternal() ?? 0;
        }
        
        public static bool TryGetChildCount(this SciterElement element, out int value)
        {
            value = default;
            return element?.TryGetChildCountInternal(out value) == true;
        }
        
        #endregion

        #region DOM queries

        /// <summary>
        /// Selects the first <see cref="SciterElement"/> by the given <paramref name="id"/>
        /// </summary>
        /// <param name="element"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SciterElement SelectFirstById(this SciterElement element, string id)
        {
            return element?.SelectFirstByIdInternal(id: id);
        }

        public static SciterElement SelectFirst(this SciterElement element, string selector)
        {
            return element?.SelectFirstInternal(selector: selector);
        }

        public static IEnumerable<SciterElement> SelectAll(this SciterElement element, string selector)
        {
            return element?.SelectAllInternal(selector: selector);
        }

        public static SciterElement SelectNearestParent(this SciterElement element, string selector)
        {
            return element?.SelectNearestParentInternal(selector: selector);
        }

        #endregion DOM queries

        #region DOM Manipulation

        /// <summary>
        /// Inserts a <see cref="SciterElement"/> at the desired <paramref name="index"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        /// <param name="index"></param>
        /// <returns>The Parent <see cref="SciterElement"/> of the newly inserted <see cref="SciterElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="element"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="index"/> is &lt; 0.</exception>
        public static SciterElement InsertElement(this SciterElement parent, SciterElement element, int index = 0)
        {
            parent?.TryInsertElement(element: element, index: index);
            return parent;
        }

        /// <summary>
        /// Inserts a <see cref="SciterElement"/> at the desired <paramref name="index"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        /// <param name="index"></param>
        /// <returns>The Parent <see cref="SciterElement"/> of the newly inserted <see cref="SciterElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="element"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="index"/> is &lt; 0.</exception>
        public static bool TryInsertElement(this SciterElement parent, SciterElement element, int index = 0)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element), @"Element cannot be null.");
            
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), @"Index must be greater than or equal to 0.");

            return parent?.InsertElementInternal(element: element, index: index) == true;
        }

        /// <summary>
        /// Appends a <see cref="SciterElement"/> to the <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        /// <param name="callback"></param>
        /// <returns>The Parent <see cref="SciterElement"/> of the newly created <see cref="SciterElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="element"/> is null.</exception>
        public static SciterElement AppendElement(this SciterElement parent, SciterElement element, Action<SciterElement> callback = null)
        {
            parent?.TryAppendElement(element: element, callback: callback);
            return parent;
        }

        /// <summary>
        /// Appends a <see cref="SciterElement"/> to the <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        /// <param name="callback"></param>
        /// <returns>The Parent <see cref="SciterElement"/> of the newly created <see cref="SciterElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="element"/> is null.</exception>
        public static bool TryAppendElement(this SciterElement parent, SciterElement element, Action<SciterElement> callback = null)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element), @"Element cannot be null.");

            var result = parent?.AppendElementInternal(element: element) == true;
            
            callback?.Invoke(element);

            return result;
        }

        /// <summary>
        /// Creates a new <see cref="SciterElement"/> with the given <paramref name="tag"/> and appends it to the <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag"></param>
        /// <param name="text"></param>
        /// <param name="callback"></param>
        /// <returns>The Parent <see cref="SciterElement"/> of the newly created <see cref="SciterElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="tag"/> is null.</exception>
        public static SciterElement AppendElement(this SciterElement parent, string tag, string text = null, Action<SciterElement> callback = null)
        { 
            parent?.TryAppendElement(tag: tag, element: out _, text: text, callback: callback);
            return parent;
        }

        /// <summary>
        /// Creates a new <see cref="SciterElement"/> with the given <paramref name="tag"/> and appends it to the <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag"></param>
        /// <param name="element">The newly created <see cref="SciterElement"/></param>
        /// <param name="text"></param>
        /// <param name="callback"></param>
        /// <returns>The Parent <see cref="SciterElement"/> of the newly created <see cref="SciterElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="tag"/> is null.</exception>
        public static bool TryAppendElement(this SciterElement parent, string tag, out SciterElement element, string text = null, Action<SciterElement> callback = null)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentNullException(nameof(tag), @"Tag cannot be null or empty.");

            element = null;
            
            var result = parent?.TryAppendElementInternal(tag: tag, out element, text: text) == true;
            
            callback?.Invoke(element);

            return result;
        }

        /// <summary>
        /// Swaps the <paramref name="element"/> with the given <paramref name="swapElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="swapElement"></param>
        /// <returns>The <see cref="SciterElement"/> specified in <paramref name="swapElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="swapElement"/> is null.</exception>
        public static SciterElement SwapElements(this SciterElement element, SciterElement swapElement)
        {
            if (swapElement == null)
                throw new ArgumentNullException(nameof(swapElement), @"Swap Element cannot be null.");
            
            element?.TrySwapElements(swapElement: swapElement);
            
            return swapElement;
        }

        /// <summary>
        /// Attempts to swaps the <paramref name="element"/> with the given <paramref name="swapElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="swapElement"></param>
        /// <returns>The <see cref="SciterElement"/> specified in <paramref name="swapElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="swapElement"/> is null.</exception>
        public static bool TrySwapElements(this SciterElement element, SciterElement swapElement)
        {
            if (swapElement == null)
                throw new ArgumentNullException(nameof(swapElement), @"Swap Element cannot be null.");
            
            return element?.SwapElementsInternal(swapElement: swapElement) == true;
        }

        /// <summary>
        /// Clears the <see cref="SciterElement"/> text property.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>The <see cref="SciterElement"/> specified in <paramref name="element"/></returns>
        public static SciterElement ClearText(this SciterElement element)
        {
            element?.TryClearText();
            return element;
        }

        /// <summary>
        /// Attempts to clear the <see cref="SciterElement"/> text property.
        /// </summary>
        /// <param name="element"></param>
        public static bool TryClearText(this SciterElement element)
        {
            return element?.ClearTextInternal() == true;
        }

        public static SciterElement TransformHtml(this SciterElement element, string html, ElementHtmlReplacement replacement)
        {
            element?.TryTransformHtml(html: html, replacement: replacement);
            return element;
        }

        public static bool TryTransformHtml(this SciterElement element, string html, ElementHtmlReplacement replacement)
        {
            return element?.TransformHtmlInternal(html: html, replacement: replacement) == true;
        }

        #endregion DOM Manipulation
        
        #region Events
		
        public static SciterElement AttachEventHandler(this SciterElement element, SciterEventHandler eventHandler)
        {
            element?.TryAttachEventHandler(eventHandler);
            return element;
        }
		
        public static SciterElement OnCustomEvent(this SciterElement element, Action<string, SciterElement, SciterElement, SciterValue> callback)
        {
            element?.OnCustomEventInternal(callback);
            return element;
        }
		
        public static bool TryAttachEventHandler(this SciterElement element, SciterEventHandler eventHandler)
        {
            if (eventHandler == null)
                throw new ArgumentNullException(nameof(eventHandler), @"EventHandler cannot be null.");
            
            return element?.TryAttachEventHandlerInternal(eventHandler) == true;
        }
		
        public static SciterElement AttachEventHandlerWithoutValidation(this SciterElement element, SciterEventHandler eventHandler)
        {
            element?.TryAttachEventHandlerWithoutValidation(eventHandler);
            return element;
        }
		
        public static bool TryAttachEventHandlerWithoutValidation(this SciterElement element, SciterEventHandler eventHandler)
        {
            if (eventHandler == null)
                return false;
            
            return element?.TryAttachEventHandlerInternal(eventHandler) == true;
        }
		
        public static SciterElement AttachEventHandler(this SciterElement element, Func<SciterEventHandler> eventHandlerFunc)
        {
            element?.TryAttachEventHandler(eventHandlerFunc);
            return element;
        }
		
        public static bool TryAttachEventHandler(this SciterElement element, Func<SciterEventHandler> eventHandlerFunc)
        {
            if (eventHandlerFunc == null)
                throw new ArgumentNullException(nameof(eventHandlerFunc), @"EventHandler method cannot be null.");
            
            return element?.TryAttachEventHandler(eventHandler: eventHandlerFunc.Invoke()) == true;
        }
		
        public static SciterElement AttachEventHandlerWithoutValidation(this SciterElement element, Func<SciterEventHandler> eventHandlerFunc)
        {
            element?.TryAttachEventHandlerWithoutValidation(eventHandlerFunc);
            return element;
        }
		
        public static bool TryAttachEventHandlerWithoutValidation(this SciterElement element, Func<SciterEventHandler> eventHandlerFunc)
        {
            if (eventHandlerFunc == null)
                return false;
            
            return element?.TryAttachEventHandlerWithoutValidation(eventHandler: eventHandlerFunc.Invoke()) == true;
        }

        public static SciterElement DetachEventHandler(this SciterElement element, SciterEventHandler eventHandler)
        {
            element.TryDetachEventHandler(eventHandler: eventHandler);
            return element;
        }

        public static bool TryDetachEventHandler(this SciterElement element, SciterEventHandler eventHandler)
        {
            if (eventHandler == null)
                throw new ArgumentNullException(nameof(eventHandler), @"EventHandler cannot be null.");
            
            return element?.TryDetachEventHandlerInternal(eventHandler: eventHandler) == true;
        }
        
        public static SciterElement DetachEventHandlerWithoutValidation(this SciterElement element, SciterEventHandler eventHandler)
        {
            element?.DetachEventHandlerInternal(eventHandler: eventHandler);
            return element;
        }
		
        public static bool TryDetachEventHandlerWithoutValidation(this SciterElement element, SciterEventHandler eventHandler)
        {
            if (eventHandler == null)
                return false;
            
            return element?.TryDetachEventHandlerInternal(eventHandler: eventHandler) == true;
        }
        
        public static void SendEvent(this SciterElement element, int eventCode, int reason = 0, SciterElement source = null)
        {
            element?.SendEventInternal(eventCode: eventCode, reason: reason, source: source);
        }
		
        public static bool TrySendEvent(this SciterElement element, int eventCode, out bool handled, int reason = 0, SciterElement source = null)
        {
            handled = default;
            return element?.TrySendEventInternal(eventCode: eventCode, handled: out handled, reason: reason, source: source) == true;
        }
        
        public static void PostEvent(this SciterElement element, int eventCode, int reason = 0, SciterElement source = null)
        {
            element?.PostEventInternal(eventCode: eventCode, reason: reason, source: source);
        }
		
        public static bool TryPostEvent(this SciterElement element, int eventCode, int reason = 0, SciterElement source = null)
        {
            return element?.TryPostEventInternal(eventCode: eventCode, reason: reason, source: source) == true;
        }
        
        public static void FireEvent(this SciterElement element, SciterBehaviors.BEHAVIOR_EVENT_PARAMS @params, bool post = true)
        {
            element?.FireEventInternal(@params: @params, post: post);
        }
		
        public static bool TryFireEvent(this SciterElement element, SciterBehaviors.BEHAVIOR_EVENT_PARAMS @params, out bool handled, bool post = true)
        {
            handled = default;
            return element?.TryFireEventInternal(@params: @params, out handled, post: post) == true;
        }
        
        #endregion Events
        
        #region Location and Size
        
        public static SciterRectangle GetLocation(this SciterElement element, ElementArea area = ElementArea.RootRelative | ElementArea.ContentBox)
        {
            return element.GetLocationInternal(area: area);
        }
        
        public static bool TryGetLocation(this SciterElement element, out SciterRectangle value, ElementArea area = ElementArea.RootRelative | ElementArea.ContentBox)
        {
            return element.TryGetLocationInternal(value: out value, area: area);
        }

        public static SciterSize GetSize(this SciterElement element)
        {
            return element.GetSizeInternal();
        }
		
        public static bool TryGetSize(this SciterElement element, out SciterSize value)
        {
            return element.TryGetSizeInternal(value: out value);
        }
        
        #endregion Location and Size
        
        #region State
        
        public static ElementState GetState(this SciterElement element)
        {
            return element.GetStateInternal();
        }

        public static bool TryGetState(this SciterElement element, out ElementState elementState)
        {
            elementState = default;
            var result = element?.TryGetStateInternal(state: out elementState) == true;
            return result;
        }
        
        /*public static ElementState GetState(this SciterElement element)
        {
            element.TryGetState(elementState: out var elementState);
            return elementState;
        }

        public static bool TryGetState(this SciterElement element, out ElementState elementState)
        {
            SciterXDom.ELEMENT_STATE_BITS stateBits = default;
            var result = element?.TryGetStateInternal(stateBits: out stateBits) == true;
            elementState = (ElementState)(uint)(result ? stateBits : 0);
            return result;
        }*/

        public static SciterElement SetState(this SciterElement element, ElementState statesToSet, ElementState statesToClear = ElementState.None, bool update = true)
        {
            element?.TrySetState(statesToSet: statesToSet, statesToClear: statesToClear, update: update);
            return element;
        }

        public static bool TrySetState(this SciterElement element, ElementState statesToSet, ElementState statesToClear = ElementState.None, bool update = true)
        {
            return element?.TrySetStateInternal(bitsToSet: (SciterXDom.ELEMENT_STATE_BITS)statesToSet, bitsToClear: (SciterXDom.ELEMENT_STATE_BITS)statesToClear, update: update) == true;
        }
        
        #endregion State
        
        #region Query HTML
        public static string GetTag(this SciterElement element)
        {
            return element.GetTagInternal();
        }
		
        public static bool TryGetTag(this SciterElement element, out string tag)
        {
            tag = default;
            return element?.TryGetTagInternal(out tag) == true;
        }
        
        public static string GetHtml(this SciterElement element)
        {
            return element.GetHtmlInternal();
        }
		
        public static bool TryGetHtml(this SciterElement element, out string html)
        {
            html = default;
            return element?.TryGetHtmlInternal(html: out html) == true;
        }
        
        //TODO: Missing parameters
        public static SciterElement SetHtml(this SciterElement element, string html)
        {
            element?.SetHtmlInternal(html: html);
            return element;
        }
		
        //TODO: Missing parameters
        public static bool TrySetHtml(this SciterElement element, string html)
        {
            return element?.TrySetHtmlInternal(html: html) == true;
        }
        
        public static string GetInnerHtml(this SciterElement element)
        {
            return element.GetInnerHtmlInternal();
        }
		
        public static bool TryGetInnerHtml(this SciterElement element, out string innerHtml)
        {
            innerHtml = default;
            return element?.TryGetInnerHtmlInternal(out innerHtml) == true;
        }
        
        public static string GetText(this SciterElement element)
        {
            return element.GetTextInternal();
        }
		
        public static bool TryGetText(this SciterElement element, out string text)
        {
            text = default;
            return element?.TryGetTextInternal(text: out text) == true;
        }
        
        public static SciterElement SetText(this SciterElement element, string text)
        {
            element?.SetTextInternal(text: text);
            return element;
        }
		
        public static bool TrySetText(this SciterElement element, string text)
        {
            return element?.TrySetTextInternal(text: text) == true;
        }

        #endregion Query HTML

        #region Handles

        /// <summary>
        /// Gets the native Handle of the containing window
        /// </summary>
        /// <param name="element"></param>
        /// <param name="rootWindow"><para>true: Sciter window.</para><para>false: Nearest windowed parent element</para></param>
        /// <returns>Native Handle of the containing window</returns>
        public static IntPtr GetWindowHandle(this SciterElement element, bool rootWindow = true)
        {
            return element?.GetWindowHandleInternal(rootWindow: rootWindow) ?? IntPtr.Zero;
        }

        /// <summary>
        /// Attempts to get the native Handle of the containing window
        /// </summary>
        /// <param name="element"></param>
        /// <param name="rootWindow"><para>true: Sciter window.</para><para>false: Nearest windowed parent element</para></param>
        /// <param name="value"><see cref="IntPtr"/> of the native handle</param>
        /// <returns><para>true: Successful.</para><para>false: Failed</para></returns>
        public static bool TryGetWindowHandle(this SciterElement element, bool rootWindow, out IntPtr value)
        {
            value = IntPtr.Zero;
            return element?.TryGetWindowHandleInternal(rootWindow: rootWindow, out value) == true;
        }

        #endregion Handles
        
        #region Helpers
		
        public static bool IsChildOf(this SciterElement element, SciterElement parent)
        {
            var parentElement = element;
			
            while(parentElement != null)
            {
                if (parentElement.Handle == parent.Handle)
                    return true;

                parentElement = parentElement?.Parent;
            }
            return false;
        }

        #endregion
        
        #region Scripting
        
        /// <summary>
        /// <para>Call scripting method attached to the element (directly or through of scripting behavior)</para>
        /// <para>Example: <br/>var elem = ...<br/>elem.foo = function() {...}</para>
        /// </summary>
        /// <param name="element"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static SciterValue CallMethod(this SciterElement element, string method, params SciterValue[] args)
        {
            return element?.CallMethodInternal(method: method, args: args);
        }

        /// <summary>
        /// <para>Tries calling scripting method attached to the element (directly or through of scripting behavior)</para>
        /// <para>Example: <br/>var elem = ...<br/>elem.foo = function() {...}</para>
        /// </summary>
        /// <param name="element"></param>
        /// <param name="method"></param>
        /// <param name="value"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool TryCallMethod(this SciterElement element, string method, out SciterValue value, params SciterValue[] args)
        {
            value = default;
            return element?.TryCallMethodInternal(method: method, value: out value, args: args) == true;
        }
        
        /// <summary>
        /// <para>Call scripting function defined on global level</para>
        /// <para>Example: <br/>function foo() {...}</para>
        /// </summary>
        /// <param name="element"></param>
        /// <param name="function"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static SciterValue CallFunction(this SciterElement element, string function, params SciterValue[] args)
        {
            return element?.CallFunctionInternal(function: function, args: args);
        }

        /// <summary>
        /// <para>Tries calling scripting function defined on global level</para>
        /// <para>Example: <br/>function foo() {...}</para>
        /// </summary>
        /// <param name="element"></param>
        /// <param name="function"></param>
        /// <param name="value"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool TryCallFunction(this SciterElement element, string function, out SciterValue value, params SciterValue[] args)
        {
            value = default;
            return element?.TryCallFunctionInternal(function: function, value: out value, args: args) == true;
        }
        
        /// <summary>
        /// <para>Evaluates the script in element context:
        /// `this` in script will be the element and in namespace of element's document.</para>
        /// </summary>
        /// <param name="element"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        public static SciterValue EvaluateScript(this SciterElement element, string script)
        {
            return element?.EvaluateScriptInternal(script: script);
        }
        
        /// <summary>
        /// <para>Attempts to evaluate script in element context:
        /// `this` in script will be the element and in namespace of element's document.</para>
        /// </summary>
        /// <param name="element"></param>
        /// <param name="script"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryEvaluateScript(this SciterElement element, string script, out SciterValue value)
        {
            value = default;
            return element?.TryEvaluateScriptInternal(script: script, value: out value) == true;
        }
        
        #endregion Scripting
        
        #region Highlight
        
        public static SciterElement SetHighlight(this SciterElement element, bool value)
        {
            element?.TrySetHighlight(value);
            return element;
        }
        
        public static bool TrySetHighlight(this SciterElement element, bool value)
        {
            return element?.TrySetHighlightInternal(value) == true;
        }
        
        #endregion
    }
}