using System;

namespace SciterCore
{
    public static class SciterElementExtensions
    {

        #region Element

        public static SciterElement AppendChild(this SciterElement parent, string tagName, string text = null)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent), "Parent element cannot be null.");
            
            var element = SciterElement.Create(tagName: tagName, text);

            parent.Append(element);

            return element;
        }
        
        public static void DetachElement(this SciterElement element)
        {
            element?.DetachElementInternal();
        }
        
        public static SciterElement CloneElement(this SciterElement element)
        {
            return element?.CloneElementInternal();
        }
        
        public static SciterElement GetChildElement(this SciterElement element, int index)
        {
            return element?.GetChildElementInternal(index: Convert.ToUInt32(index));
        }
        
        public static SciterNode CastToNode(this SciterElement element)
        {
            return element?.CastToNodeInternal();
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
    }
}