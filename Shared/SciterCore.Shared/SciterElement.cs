// Copyright 2016 Ramon F. Mendes
//
// This file is part of SciterSharp.
// 
// SciterSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// SciterSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with SciterSharp.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Runtime.InteropServices;
using SciterCore.Interop;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ArrangeThisQualifier
// ReSharper disable RedundantLambdaParameterType
// ReSharper disable ConvertToAutoProperty

namespace SciterCore
{
	public sealed class SciterElement : IDisposable
	{
		private static readonly Sciter.SciterApi Api = Sciter.Api;
		private readonly IntPtr _elementHandle;

		public IntPtr Handle => _elementHandle;
		
		public SciterElement(IntPtr elementHandle)
		{
			if(elementHandle == IntPtr.Zero)
				throw new ArgumentException($"IntPtr.Zero received at {nameof(SciterElement)} constructor.");

			Api.Sciter_UseElement(elementHandle);
			_elementHandle = elementHandle;
		}

		public SciterElement(SciterValue sv)
		{
			if(!sv.IsObject)
				throw new ArgumentException("The given SciterValue is not a TIScript Element reference");

			var elementHandle = sv.GetObjectData();
			if(elementHandle == IntPtr.Zero)
				throw new ArgumentException("IntPtr.Zero received at SciterElement constructor");

			Api.Sciter_UseElement(this.Handle);
			_elementHandle = elementHandle;
		}
		
		public static SciterElement Create(string tag, string text = null)
		{
			TryCreate(tag: tag, element: out var element, text: text);
			return element;
		}

		public static bool TryCreate(string tag, out SciterElement element, string text = null)
		{
			var result =
				Api.SciterCreateElement(tag, text, out var elementHandle)
					.IsOk() && elementHandle != IntPtr.Zero;
			
			element = result ? new SciterElement(elementHandle) : null;
			
			return result;
		}

		#region Query HTML

		public string Tag => GetTagInternal();

		internal string GetTagInternal()
		{
			TryGetTagInternal(out var result);
			return result;
		}
		
		internal bool TryGetTagInternal(out string tag)
		{
			var result = Api.SciterGetElementType(this.Handle, out var tagPtr)
				.IsOk();
			tag = result ? Marshal.PtrToStringAnsi(tagPtr) : default;
			return result;
		}
		
		public string Html 
		{
			get => GetHtmlInternal();
			set => SetHtmlInternal(value);
		} 

		internal string GetHtmlInternal()
		{
			TryGetHtmlInternal(out var result);
			return result;
		}
		
		internal bool TryGetHtmlInternal(out string html)
		{
			var result = TryGetElementHtmlValue(outerHtml: true, out var htmlValue);
			html = result ? htmlValue : default;
			return result;
		}

		internal void SetHtmlInternal(string html, SciterXDom.SET_ELEMENT_HTML where = SciterXDom.SET_ELEMENT_HTML.SIH_REPLACE_CONTENT)
		{
			TrySetHtmlInternal(html: html, where: where);
		}

		internal bool TrySetHtmlInternal(string html, SciterXDom.SET_ELEMENT_HTML where = SciterXDom.SET_ELEMENT_HTML.SIH_REPLACE_CONTENT)
		{
			if (html == null)
				return ClearTextInternal();

			var data = Encoding.UTF8.GetBytes(html);
			return Api.SciterSetElementHtml(this.Handle, data, (uint) data.Length, where)
				.IsOk();
		}
		
		public string InnerHtml => GetInnerHtmlInternal();

		internal string GetInnerHtmlInternal()
		{
			TryGetInnerHtmlInternal(out var result);
			return result;
		}
		
		internal bool TryGetInnerHtmlInternal(out string innerHtml)
		{
			var result = TryGetElementHtmlValue(outerHtml: false, out var htmlValue);
			innerHtml = result ? htmlValue : default;
			return result;
		}
		
		private bool TryGetElementHtmlValue(bool outerHtml, out string value)
		{
			string htmlValue = default;

			var domResult = Api.SciterGetElementHtmlCB(
				this.Handle, 
				outerHtml, 
				(IntPtr bytes, uint numBytes, IntPtr param) =>
					{
						htmlValue = Marshal.PtrToStringAnsi(bytes, Convert.ToInt32(numBytes));
					}, 
				IntPtr.Zero);
			
			value = htmlValue;

			return domResult.IsOk() || (domResult.IsOkNotHandled() && value == null);
		}

		public string Text
		{
			get => GetTextInternal();
			set => SetTextInternal(value);
		}
		
		internal string GetTextInternal()
		{
			TryGetTextInternal(out var result);
			return result;
		}
		
		internal bool TryGetTextInternal(out string text)
		{
			string outText = default;

			var domResult = Api.SciterGetElementTextCB(
				this.Handle, 
				(IntPtr strPtr, uint strLength, IntPtr param) =>
					{
						outText = Marshal.PtrToStringUni(strPtr, (int)strLength);
					}, 
				IntPtr.Zero);
			
			text = outText;
			
			return domResult.IsOk() || (domResult.IsOkNotHandled() && text == null);
		}
		
		internal void SetTextInternal(string text)
		{
			TrySetTextInternal(text: text);
		}
		
		internal bool TrySetTextInternal(string text)
		{
			return Api.SciterSetElementText(this.Handle, text, Convert.ToUInt32(text.Length))
				.IsOk();
		}
		
		#endregion

		#region Attributes and Styles
		
		public IReadOnlyDictionary<string, string> Attributes
		{
			get
			{
				var writeableDictionary = new Dictionary<string, string>();
				for (var n = 0; n < GetAttributeCountInternal(); n++)
				{
					writeableDictionary[GetAttributeNameInternal(n)] = GetAttributeValueInternal(n);
				}
				return new ReadOnlyDictionary<string, string>(writeableDictionary);
			}
		}

		public int AttributeCount => GetAttributeCountInternal();

		internal int GetAttributeCountInternal()
		{
			TryGetAttributeCountInternal(out var result);
			return result;
		}

		internal bool TryGetAttributeCountInternal(out int value)
		{
			var result = Api.SciterGetAttributeCount(this.Handle, out var count)
				.IsOk();
			value = result ? Convert.ToInt32(count) : 0;
			return result;
		}

		internal string GetAttributeValueInternal(int index)
		{
			TryGetAttributeValueInternal(index: index, out var result);
			return result;
		}

		internal bool TryGetAttributeValueInternal(int index, out string value)
		{
			string outValue = default;
			
			var domResult = Api.SciterGetNthAttributeValueCB(this.Handle, Convert.ToUInt32(index), (IntPtr str, uint strLength, IntPtr param) =>
			{
				outValue = Marshal.PtrToStringUni(str, (int)strLength);
			}, IntPtr.Zero);

			value = outValue;
			
			return domResult.IsOk() || (domResult.IsOkNotHandled() && outValue == null);
		}

		internal string GetAttributeValueInternal(string key)
		{
			TryGetAttributeValueInternal(key: key, value: out var result);
			return result;
		}

		internal bool TryGetAttributeValueInternal(string key, out string value)
		{
			string outValue = default;
			
			var domResult = Api.SciterGetAttributeByNameCB(this.Handle, key, (IntPtr str, uint strLength, IntPtr param) =>
			{
				outValue = Marshal.PtrToStringUni(str, (int) strLength);
			}, IntPtr.Zero);

			value = outValue;
			
			return domResult.IsOk() || (domResult.IsOkNotHandled() && outValue == null);
		}

		internal string GetAttributeNameInternal(int index)
		{
			TryGetAttributeNameInternal(index: index, out var result);
			return result;
		}

		internal bool TryGetAttributeNameInternal(int index, out string value)
		{
			string outValue = default;
			
			var domResult = Api.SciterGetNthAttributeNameCB(this.Handle, Convert.ToUInt32(index), (IntPtr str, uint strLength, IntPtr param) =>
			{
				outValue = Marshal.PtrToStringAnsi(str, (int) strLength);
			}, IntPtr.Zero);

			value = outValue;
			
			return domResult.IsOk() || (domResult.IsOkNotHandled() && outValue == null);
		}

        internal void SetAttributeValueInternal(string key, string value)
        {
	        TrySetAttributeValueInternal(key: key, value: value);
        }

        internal bool TrySetAttributeValueInternal(string key, string value)
        {
            return Api.SciterSetAttributeByName(this.Handle, key, value)
	            .IsOk();
        }

        internal void RemoveAttributeInternal(string key)
		{
			Api.SciterSetAttributeByName(this.Handle, key, null);
		}
		
        internal string GetStyleValueInternal(string key)
		{
			TryGetStyleValueInternal(key: key, out var result);
			return result;
		}
		
        internal bool TryGetStyleValueInternal(string key, out string value)
		{
			string outStyle = default;
			
			var domResult = Api.SciterGetStyleAttributeCB(this.Handle, key, (IntPtr str, uint strLength, IntPtr param) =>
			{
				outStyle = Marshal.PtrToStringUni(str, Convert.ToInt32(strLength));
			}, IntPtr.Zero);
			
			value = outStyle;
			
			return domResult.IsOk() || (domResult.IsOkNotHandled() && value == null);
		}
		
        internal void SetStyleValueInternal(string key, string value)
		{
			TrySetStyleValueInternal(key: key, value: value);
		}
		
        internal bool TrySetStyleValueInternal(string key, string value)
		{
			return Api.SciterSetStyleAttribute(this.Handle, key, value)
				.IsOk();
		}
		
		#endregion

		#region State
		public ElementState State => GetStateInternal();

		internal ElementState GetStateInternal()
		{
			TryGetStateInternal(state: out var state);
			return state;
		}

		internal bool TryGetStateInternal(out ElementState state)
		{
			var result = Api.SciterGetElementState(this.Handle, out var bits)
				.IsOk();
			
			state = (ElementState)(result ? (SciterXDom.ELEMENT_STATE_BITS)bits : 0);
			
			return result;
		}

		/*public SciterXDom.ELEMENT_STATE_BITS State => GetStateInternal();

		internal SciterXDom.ELEMENT_STATE_BITS GetStateInternal()
		{
			TryGetStateInternal(out var stateBits);
			return stateBits;
		}

		internal bool TryGetStateInternal(out SciterXDom.ELEMENT_STATE_BITS stateBits)
		{
			var result = Api.SciterGetElementState(this.Handle, out var bits)
				.IsOk();

			stateBits = result ? (SciterXDom.ELEMENT_STATE_BITS) bits : default;
			
			return result;
		}*/
		
		internal void SetElementStateInternal(SciterXDom.ELEMENT_STATE_BITS bitsToSet, SciterXDom.ELEMENT_STATE_BITS bitsToClear = 0, bool update = true)
		{
			TrySetStateInternal(bitsToSet: bitsToSet, bitsToClear: bitsToClear, update: update);
		}

		internal bool TrySetStateInternal(SciterXDom.ELEMENT_STATE_BITS bitsToSet, SciterXDom.ELEMENT_STATE_BITS bitsToClear = 0, bool update = true)
		{
			return Api.SciterSetElementState(this.Handle, (uint) bitsToSet, (uint) bitsToClear, update)
				.IsOk();
		}
		
		#endregion

		internal string CombineUrlInternal(string url = "")
		{
			TryCombineUrlInternal(url, out var result);
			return result;
		}

		internal bool TryCombineUrlInternal(string url, out string value)
		{
			var buffer = PInvokeUtils.NativeUtf16FromString(url, 2048);
			var result = Api.SciterCombineURL(this.Handle, buffer, 2048)
				.IsOk();
			value = PInvokeUtils.StringFromNativeUtf16(buffer);
			PInvokeUtils.NativeUtf16FromString_FreeBuffer(buffer);
			return result;
		}

		#region Integers
		
		public SciterWindow Window
		{
			get
			{
				var hwnd = GetWindowHandleInternal();
				return hwnd != IntPtr.Zero ? new SciterWindow(hwnd) : null;
			}
		}
		
		internal IntPtr GetWindowHandleInternal(bool rootWindow = true)
		{
			TryGetWindowHandleInternal(rootWindow: rootWindow, out var result);
			return result;
		}
		
		internal bool TryGetWindowHandleInternal(bool rootWindow, out IntPtr value)
		{
			return Api.SciterGetElementHwnd(this.Handle, out value, rootWindow)
				.IsOk();
		}
		
		/// <summary>
		/// Get element UID - identifier suitable for storage
		/// </summary>
		public uint UniqueId
		{
			get
			{
				Api.SciterGetElementUID(this.Handle, out var uid);
				return uid;
			}
		}

		public int Index
		{
			get
			{
				Api.SciterGetElementIndex(this.Handle, out var result);
				return Convert.ToInt32(result);
			}
		}

		public int ChildCount => GetChildCountInternal();

		internal int GetChildCountInternal()
		{
			TryGetChildCountInternal(out var result);
			return result;
		}

		internal bool TryGetChildCountInternal(out int value)
		{
			var result = Api.SciterGetChildrenCount(this.Handle, out var count)
				.IsOk();
			value = result ? Convert.ToInt32(count) : 0;
			return result;
		}
		
		#endregion

		internal bool TryDeleteInternal()
		{
			return Api.SciterDeleteElement(this.Handle)
				.IsOk();
		}

		internal bool TryDetachInternal()
		{
			return Api.SciterDetachElement(this.Handle)
				.IsOk();
		}

		internal SciterElement CloneInternal()
		{
			TryCloneInternal(out var result);
			return result;
		}

		internal bool TryCloneInternal(out SciterElement element)
		{
			var result = Api.SciterCloneElement(this.Handle, out var cloneResult)
				.IsOk();
			element = result ? new SciterElement(cloneResult) : null;
			return result;
		}

		internal SciterNode CastToNodeInternal()
		{
			TryCastToNodeInternal(out var result);
			return result;
		}

		internal bool TryCastToNodeInternal(out SciterNode node)
		{
			var result = Api.SciterNodeCastFromElement(this.Handle, out var castResult)
				.IsOk();
			node = result ? new SciterNode(castResult) : null;
			return result;
		}

		/// <summary>
		/// Deeply Enabled
		/// </summary>
		internal bool IsEnabledInternal()
		{
			Api.SciterIsElementEnabled(this.Handle, out var result);
			return result;
		}

		/// <summary>
		/// Deeply Visible
		/// </summary>
		internal bool IsVisibleInternal()
		{
			Api.SciterIsElementVisible(this.Handle, out var result);
			return result;
		}

		#region Operators and overrides
		
		public static bool operator == (SciterElement a, SciterElement b)
		{
			if((object)a == null || (object)b == null)
				return Equals(a, b);
			return a.Handle == b.Handle;
		}
		
		public static bool operator != (SciterElement a, SciterElement b)
		{
			return !(a == b);
		}

		public SciterElement this[int childIndex] => GetChildAtIndexInternal(childIndex);

		public string this[string attributeName]
		{
			get => GetAttributeValueInternal(attributeName);
			set => SetAttributeValueInternal(attributeName, value);
		}

		public override bool Equals(object o)
		{
			return object.ReferenceEquals(this, o);
		}

		public override int GetHashCode()
		{
			return this.Handle.ToInt32();
		}

		public override string ToString()
		{
			var tag = Tag;
			var id = GetAttributeValueInternal("id");
			var classes = GetAttributeValueInternal("class");
			var childCount = this.ChildCount;

			var str = new StringBuilder();
			str.Append("<" + tag);
			if(id != null)
				str.Append(" #" + id);
			if(classes != null)
				str.Append(" ." + string.Join(".", classes.Split(' ')));
			if(childCount == 0)
				str.Append(" />");
			else
				str.Append(">...</" + tag + ">");

			return str.ToString();
		}
		#endregion

		#region DOM navigation
		
		internal SciterElement GetChildAtIndexInternal(int index)
		{
			return GetChildAtIndexInternal(Convert.ToUInt32(index));
		}

		private SciterElement GetChildAtIndexInternal(uint index)
		{
			TryGetChildAtIndexInternal(index, out var element);
			return element;
		}
		
		internal bool TryGetChildAtIndexInternal(int index, out SciterElement element)
		{
			return TryGetChildAtIndexInternal(index: Convert.ToUInt32(index), out element);
		}
		
		private bool TryGetChildAtIndexInternal(uint index, out SciterElement element)
		{
			var result = Api.SciterGetNthChild(this.Handle, index, out var childHandle)
				.IsOk();
			element = (result && childHandle != IntPtr.Zero) ? new SciterElement(childHandle) : null;
			return (result && childHandle != IntPtr.Zero);
		}

		public IEnumerable<SciterElement> Children
		{
			get
			{
				var list = new List<SciterElement>();
				for(var i = 0; i < this.ChildCount; i++)
					list.Add(this[i]);
				return list;
			}
		}

		public SciterElement Parent
		{
			get
			{
				Api.SciterGetParentElement(this.Handle, out var parentHandle);
				return parentHandle == IntPtr.Zero ? null : new SciterElement(parentHandle);
			}
		}

		internal SciterElement NextSiblingInternal => this.Parent?.GetChildAtIndexInternal(this.Index + 1);

		internal SciterElement PreviousSiblingInternal => this.Parent?.GetChildAtIndexInternal(this.Index - 1);

		internal SciterElement FirstSiblingInternal => this.Parent?.GetChildAtIndexInternal(0);

		internal SciterElement LastSiblingInternal => this.Parent?.GetChildAtIndexInternal(this.Parent.ChildCount - 1);

		#endregion

		#region DOM query/select
		internal SciterElement SelectFirstByIdInternal(string id)
		{
			return SelectFirstInternal("[id='" + id + "']");
		}

		internal SciterElement SelectFirstInternal(string selector)
		{
			SciterElement result = null;
			
			Api.SciterSelectElementsW(
				this.Handle, 
				selector, 
				(IntPtr he, IntPtr param) =>
					{
						result = new SciterElement(he);
						return true;// true stops enumeration
					}, 
				IntPtr.Zero);
			
			return result;
		}

		internal IEnumerable<SciterElement> SelectAllInternal(string selector)
		{
			var result = new List<SciterElement>();

			Api.SciterSelectElementsW(
				this.Handle, 
				selector, 
				(IntPtr elementHandle, IntPtr param) =>
						{
							result.Add(new SciterElement(elementHandle));
							return false;// false continue enumeration
						}, 
				IntPtr.Zero);
			
			return result;
		}

		internal SciterElement SelectNearestParentInternal(string selector)
		{
			Api.SciterSelectParentW(this.Handle, selector, 0, out var heFound);
			return heFound == IntPtr.Zero ? null : new SciterElement(heFound);
		}
		#endregion

		#region DOM sub-tree manipulation
		
		internal bool InsertElementInternal(SciterElement element, int index = 0)
		{
			return Api.SciterInsertElement(element.Handle, this.Handle, Convert.ToUInt32(index))
				.IsOk();
		}

		internal bool AppendElementInternal(SciterElement element)
		{
			return Api.SciterInsertElement(element.Handle, this.Handle, int.MaxValue)
				.IsOk();
		}

		internal bool TryAppendElementInternal(SciterElement element)
		{
			return Api.SciterInsertElement(element.Handle, this.Handle, int.MaxValue)
				.IsOk();
		}

		internal bool AppendElementInternal(string tag, string text = null)
        {
            var element = Create(tag, text);
            return AppendElementInternal(element);
        }

		internal bool TryAppendElementInternal(string tag, out SciterElement element, string text = null)
        {
            element = Create(tag, text);
            return AppendElementInternal(element);
        }

		internal bool SwapElementsInternal(SciterElement swapElement)
		{
			return Api.SciterSwapElements(this.Handle, swapElement.Handle)
				.IsOk();
		}

		internal bool ClearTextInternal()
		{
			return Api.SciterSetElementText(this.Handle, null, 0)
				.IsOk();
		}

		internal bool TransformHtmlInternal(string html, ElementHtmlReplacement replacement = ElementHtmlReplacement.ReplaceContent)
		{
			var value = (SciterXDom.SET_ELEMENT_HTML)Convert.ToUInt32(replacement);
			
			var bytes = Encoding.UTF8.GetBytes(html);
			return TransformHtmlInternal(bytes: bytes, replacement: value);
		}

		internal bool TransformHtmlInternal(byte[] bytes, SciterXDom.SET_ELEMENT_HTML replacement = SciterXDom.SET_ELEMENT_HTML.SIH_REPLACE_CONTENT)
		{
			return Api.SciterSetElementHtml(this.Handle, bytes, (uint) bytes.Length, replacement)
				.IsOk();
		}
		
		#endregion

		#region Events
		
		internal bool TryAttachEventHandlerInternal(SciterEventHandler eventHandler)
		{
			return Api.SciterAttachEventHandler(this.Handle, eventHandler.EventProc, IntPtr.Zero)
				.IsOk();
		}
		
		internal void DetachEventHandlerInternal(SciterEventHandler eventHandler)
		{
			TryDetachEventHandlerInternal(eventHandler: eventHandler);
		}
		
		internal bool TryDetachEventHandlerInternal(SciterEventHandler eventHandler)
		{
			return Api.SciterDetachEventHandler(this.Handle, eventHandler.EventProc, IntPtr.Zero)
				.IsOk();
		}

		internal void SendEventInternal(int eventCode, int reason = 0, SciterElement source = null)
		{
			TrySendEventInternal(eventCode: eventCode, handled: out _, reason: reason, source: source);
		}

		internal bool TrySendEventInternal(int eventCode, out bool handled, int reason = 0, SciterElement source = null)
		{
			var result = Api.SciterSendEvent(this.Handle, Convert.ToUInt32(eventCode), source == null ? IntPtr.Zero : source.Handle, new IntPtr(Convert.ToUInt32(reason)), out handled)
				.IsOk();

			return result && handled;
		}

		internal void PostEventInternal(int eventCode, int reason = 0, SciterElement source = null)
		{
			TryPostEventInternal(eventCode: eventCode, reason: reason, source: source);
		}

		internal bool TryPostEventInternal(int eventCode, int reason = 0, SciterElement source = null)
		{
			return Api.SciterPostEvent(this.Handle, Convert.ToUInt32(eventCode), source == null ? IntPtr.Zero : source.Handle, new IntPtr(Convert.ToUInt32(reason)))
				.IsOk();
		}

		internal void FireEventInternal(SciterBehaviors.BEHAVIOR_EVENT_PARAMS @params, bool post = true)
		{
			TryFireEventInternal(@params: @params, handled: out _, post: post);
		}

		internal bool TryFireEventInternal(SciterBehaviors.BEHAVIOR_EVENT_PARAMS @params, out bool handled, bool post = true)
		{
			return Api.SciterFireEvent(ref @params, post, out handled).IsOk();
		}
		
		#endregion

		#region Location and Size
		internal SciterRectangle GetLocationInternal(ElementArea area = ElementArea.RootRelative | ElementArea.ContentBox)
		{
			TryGetLocationInternal(out var result, area);
			return result;
		}
		
		internal bool TryGetLocationInternal(out SciterRectangle value, ElementArea area = ElementArea.RootRelative | ElementArea.ContentBox)
		{
			var result = Api.SciterGetElementLocation(this.Handle, out var rect, (SciterXDom.ELEMENT_AREAS)Convert.ToUInt32(area))
				.IsOk();
			value = result ? rect.ToRectangle() : default;
			return result;
		}

		public SciterSize Size => GetSizeInternal();

		internal SciterSize GetSizeInternal()
		{
			TryGetSizeInternal(value: out var result);
			return result;
		}
		
		internal bool TryGetSizeInternal(out SciterSize value)
		{
			var result = Api.SciterGetElementLocation(this.Handle, out var rect, SciterXDom.ELEMENT_AREAS.ROOT_RELATIVE | SciterXDom.ELEMENT_AREAS.PADDING_BOX)
				.IsOk();
			value = result ? rect.ToSize() : default;
			return result;
		}

		#endregion

		/// <summary>
		/// Test this element against CSS selector(s)
		/// </summary>
		public bool Test(string selector)
		{
			Api.SciterSelectParent(this.Handle, selector, 1, out var value);
			return value != IntPtr.Zero;
		}

		internal bool TryUpdateInternal(bool forceRender = false)
		{
			return Api.SciterUpdateElement(this.Handle, forceRender)
				.IsOk();
		}

		internal bool TryRefreshInternal(SciterRectangle rectangle)
		{
			return Api.SciterRefreshElementArea(this.Handle, rectangle.ToRect())
				.IsOk();
		}
		
		internal bool TryRefreshInternal()
		{
			var rectangle = GetLocationInternal(ElementArea.SelfRelative | ElementArea.ContentBox);
			
			return Api.SciterRefreshElementArea(this.Handle, rectangle.ToRect())
				.IsOk();
		}

		#region Scripting
		public SciterValue Value
		{
			get
			{
				Api.SciterGetValue(this.Handle, out var val);
				return new SciterValue(val);
			}

			set
			{
				var val = value.ToVALUE();
				Api.SciterSetValue(this.Handle, ref val);
			}
		}

		public SciterValue ExpandoValue
		{
			get
			{
				Api.SciterGetExpando(this.Handle, out var value, true);
				return new SciterValue(value);
			}
		}
		
		// call scripting method attached to the element (directly or through of scripting behavior)  
		// Example, script:
		//   var elem = ...
		//   elem.foo = function() {...}
		// Native code: 
		//   SciterElement elem = ...
		//   elem.CallMethod("foo");
		internal SciterValue CallMethodInternal(string method, params SciterValue[] args)
		{
			TryCallMethodInternal(method: method, value: out var result, args: args);
			return result;
		}

		// call scripting method attached to the element (directly or through of scripting behavior)  
		// Example, script:
		//   var elem = ...
		//   elem.foo = function() {...}
		// Native code: 
		//   SciterElement elem = ...
		//   elem.CallMethod("foo");
		internal bool TryCallMethodInternal(string method, out SciterValue value, params SciterValue[] args)
		{
			if (string.IsNullOrWhiteSpace(method))
				throw new ArgumentNullException(nameof(method));

			var result = Api.SciterCallScriptingMethod(this.Handle, method, SciterValue.ToVALUEArray(args), (uint) args.Length, out var returnValue)
				.IsOk();
			value = result ? new SciterValue(returnValue) : default;
			return result;
		}

		// call scripting function defined on global level   
		// Example, script:
		//   function foo() {...}
		// Native code: 
		//   dom::element root = ... get root element of main document or some frame inside it
		//   root.call_function("foo"); // call the function
		internal SciterValue CallFunctionInternal(string function, params SciterValue[] args)
		{
			TryCallFunctionInternal(function: function, value: out var result, args: args);
			return result;
		}

		// call scripting function defined on global level   
		// Example, script:
		//   function foo() {...}
		// Native code: 
		//   dom::element root = ... get root element of main document or some frame inside it
		//   root.call_function("foo"); // call the function
		internal bool TryCallFunctionInternal(string function, out SciterValue value, params SciterValue[] args)
		{
			if (string.IsNullOrWhiteSpace(function))
				throw new ArgumentNullException(nameof(function));

			var result = Api.SciterCallScriptingFunction(this.Handle, function, SciterValue.ToVALUEArray(args), (uint) args.Length, out var returnValue)
				.IsOk();
			value = result ? new SciterValue(returnValue) : default;
			return result;
		}

		/// <summary>
		/// evaluate script in element context:
		/// 'this' in script will be the element
		/// and in namespace of element's document.
		/// </summary>
		/// <param name="script"></param>
		/// <returns></returns>
		internal SciterValue EvaluateScriptInternal(string script)
		{
			TryEvaluateScriptInternal(script: script, value: out var result);
			return result;
		}

		/// <summary>
		/// evaluate script in element context:
		/// 'this' in script will be the element
		/// and in namespace of element's document.
		/// </summary>
		/// <param name="script"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		internal bool TryEvaluateScriptInternal(string script, out SciterValue value)
		{
			var result = Api.SciterEvalElementScript(this.Handle, script, Convert.ToUInt32(script.Length), out var returnValue)
				.IsOk();
			value = result ? new SciterValue(returnValue) : default;
			return result;
		}
		
		#endregion

		#region Highlight set/get

		internal bool TrySetHighlightInternal(bool value)
		{
			return Api.SciterSetHighlightedElement(GetWindowHandleInternal(), value ? this.Handle : IntPtr.Zero)
				.IsOk();
		}
		
		#endregion
		
		#region IDisposable
		
		private bool _disposedValue = false; // To detect redundant calls

		private void Dispose(bool disposing)
		{
			if(!_disposedValue)
			{
				if(disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
				Api.Sciter_UnuseElement(this.Handle);

				_disposedValue = true;
			}
		}

		~SciterElement()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			GC.SuppressFinalize(this);
		}
		
		#endregion
	}
}