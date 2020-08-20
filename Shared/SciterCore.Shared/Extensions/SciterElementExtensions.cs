using System;
using System.Collections.Generic;
using SciterCore.Interop;

// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore
{
    public static class SciterElementExtensions
    {

        #region Element

        /// <summary>
        /// Creates, appends and returns a new <see cref="SciterElement"/> with the given <paramref name="tagName"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tagName"></param>
        /// <param name="text"></param>
        /// <returns>The newly created child <see cref="SciterElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="parent"/> is null.</exception>
        /// <exception cref="ArgumentNullException">If the <paramref name="tagName"/> is null.</exception>
        public static SciterElement AppendChildElement(this SciterElement parent, string tagName, string text = null)
        {
            return parent?.AppendChildElement(childElement: SciterElement.Create(tagName: tagName, text));
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
        
        
        public static void DetachElement(this SciterElement element)
        {
            element?.TryDetachElementInternal();
        }
        
        public static bool TryDetachElement(this SciterElement element)
        {
            return element?.TryDetachElementInternal() == true;
        }

        public static SciterElement CloneElement(this SciterElement element)
        {
            return element?.CloneElementInternal();
        }
        
        public static bool TryCloneElement(this SciterElement element, out SciterElement clonedElement)
        {
            clonedElement = default;
            return element?.TryCloneElementInternal(out clonedElement) == true;
        }

        public static SciterElement GetChildElement(this SciterElement element, int index)
        {
            return element?.GetChildElementInternal(index: Convert.ToUInt32(index));
        }
        
        public static bool TryGetChildElement(this SciterElement element, int index, out SciterElement getElement)
        {
            getElement = default;
            return element?.TryGetChildElementInternal(index: Convert.ToUInt32(index), out getElement) == true;
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
        
        #region Attribute

        public static int AttributeCount(this SciterElement element)
        {
            return Convert.ToInt32(element?.AttributeCountInternal ?? 0);
        }
        
        public static string GetAttribute(this SciterElement element, uint n)
        {
            return element?.GetAttributeInternal(n: n);
        }
        
        public static string GetAttribute(this SciterElement element, string name)
        {
            return element?.GetAttributeInternal(name: name);
        }
        
        public static string GetAttributeName(this SciterElement element, uint n)
        {
            return element?.GetAttributeNameInternal(n: n);
        }
        
        public static SciterElement RemoveAttributeInternal(this SciterElement element, string name)
        {
            element?.RemoveAttributeInternal(name: name);
            return element;
        }
        
        public static SciterElement SetAttribute(this SciterElement element, string name, object value)
        {
            element?.SetAttributeInternal(name: name, value: value);
            return element;
        }
        
        public static SciterElement SetAttribute(this SciterElement element, string name, Func<object> func)
        {
            return element?.SetAttribute(name: name, value: func?.Invoke());
        }
        
        public static SciterElement SetAttribute(this SciterElement element, string name, Func<SciterElement, object> func)
        {
            return element?.SetAttribute(name: name, value: func?.Invoke(element));
        }
        
        #endregion Attribute
        
        #region Style
        
        public static string GetStyle(this SciterElement element, string name)
        {
            return element?.GetStyleInternal(name: name);
        }
        
        public static SciterElement SetStyle(this SciterElement element, string name, string value)
        {
            element?.SetStyleInternal(name: name, value: value);
            return element;
        }
        
        #endregion Style

        #region Url
        
        public static string CombineUrlInternal(this SciterElement element, string url = "")
        {
            return element.CombineUrlInternal(url: url);
        }
        
        #endregion Url

        #region Child
        
        public static int ChildCount(this SciterElement element)
        {
            return Convert.ToInt32(element?.ChildCountInternal ?? 0);
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

            return parent?.InsertElementInternal(element: element, index: Convert.ToUInt32(index)) == true;
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
        /// Creates a new <see cref="SciterElement"/> with the given <paramref name="tagName"/> and appends it to the <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tagName"></param>
        /// <param name="text"></param>
        /// <param name="callback"></param>
        /// <returns>The Parent <see cref="SciterElement"/> of the newly created <see cref="SciterElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="tagName"/> is null.</exception>
        public static SciterElement AppendElement(this SciterElement parent, string tagName, string text = null, Action<SciterElement> callback = null)
        { 
            parent?.TryAppendElement(tagName: tagName, element: out _, text: text, callback: callback);
            return parent;
        }

        /// <summary>
        /// Creates a new <see cref="SciterElement"/> with the given <paramref name="tagName"/> and appends it to the <paramref name="parent"/>.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tagName"></param>
        /// <param name="element">The newly created <see cref="SciterElement"/></param>
        /// <param name="text"></param>
        /// <param name="callback"></param>
        /// <returns>The Parent <see cref="SciterElement"/> of the newly created <see cref="SciterElement"/></returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="tagName"/> is null.</exception>
        public static bool TryAppendElement(this SciterElement parent, string tagName, out SciterElement element, string text = null, Action<SciterElement> callback = null)
        {
            if (string.IsNullOrWhiteSpace(tagName))
                throw new ArgumentNullException(nameof(tagName), @"TagName cannot be null or empty.");

            element = null;
            
            var result = parent?.TryAppendElementInternal(tagName: tagName, out element, text: text) == true;
            
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
            var value = Convert.ToUInt32((int)replacement);
            return element?.TransformHtmlInternal(html: html, replacement: (SciterXDom.SET_ELEMENT_HTML)value) == true;
        }

        #endregion DOM Manipulation
        
        #region Events
		
        public static SciterElement AttachEventHandler(this SciterElement element, SciterEventHandler eventHandler)
        {
            element?.TryAttachEventHandler(eventHandler);
            return element;
        }
		
        public static bool TryAttachEventHandler(this SciterElement element, SciterEventHandler eventHandler)
        {
            if (eventHandler == null)
                throw new ArgumentNullException(nameof(eventHandler), @"EventHandler cannot be null.");
            
            return element?.AttachEventHandlerInternal(eventHandler) == true;
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
            
            return element?.AttachEventHandlerInternal(eventHandler) == true;
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
        
        #endregion
        
        #region State

        public static ElementState GetElementState(this SciterElement element)
        {
            element.TryGetElementState(elementState: out var elementState);
            return elementState;
        }

        public static bool TryGetElementState(this SciterElement element, out ElementState elementState)
        {
            SciterXDom.ELEMENT_STATE_BITS stateBits = default;
            var result = element?.TryGetElementStateInternal(stateBits: out stateBits) == true;
            elementState = (ElementState)(uint)(result ? stateBits : 0);
            return result;
        }

        public static SciterElement SetElementState(this SciterElement element, ElementState statesToSet, ElementState statesToClear = ElementState.None, bool update = true)
        {
            element?.TrySetElementState(statesToSet: statesToSet, statesToClear: statesToClear, update: update);
            return element;
        }

        public static bool TrySetElementState(this SciterElement element, ElementState statesToSet, ElementState statesToClear = ElementState.None, bool update = true)
        {
            return element?.TrySetElementStateInternal(bitsToSet: (SciterXDom.ELEMENT_STATE_BITS)statesToSet, bitsToClear: (SciterXDom.ELEMENT_STATE_BITS)statesToClear, update: update) == true;
        }
        
        #endregion
    }
}