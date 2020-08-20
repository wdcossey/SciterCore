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
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using SciterCore.Interop;

namespace SciterCore
{
	public class SciterElement : IDisposable
	{
		private static readonly Sciter.SciterApi Api = Sciter.Api;
		private readonly IntPtr _elementHandle;

		public IntPtr Handle => _elementHandle;
		
		public SciterElement(IntPtr elementHandle)
		{
			Debug.Assert(elementHandle != IntPtr.Zero);
			if(elementHandle == IntPtr.Zero)
				throw new ArgumentException("IntPtr.Zero received at SciterElement constructor");

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

			Api.Sciter_UseElement(_elementHandle);
			_elementHandle = elementHandle;
		}

		#region IDisposable Support
		private bool _disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if(!_disposedValue)
			{
				if(disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
				Api.Sciter_UnuseElement(_elementHandle);

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

		public static SciterElement Create(string tagName, string text = null)
		{
			TryCreate(tagName: tagName, element: out var element, text: text);
			return element;
		}

		public static bool TryCreate(string tagName, out SciterElement element, string text = null)
		{
			var result =
				Api.SciterCreateElement(tagName, text, out var elementHandle) == SciterXDom.SCDOM_RESULT.SCDOM_OK &&
				elementHandle != IntPtr.Zero;
			
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
			var result = Api.SciterGetElementType(_elementHandle, out var tagPtr) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
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
			return Api.SciterSetElementHtml(_elementHandle, data, (uint) data.Length, @where) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
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

			var domResult = Api.SciterGetElementHtmlCB(_elementHandle, outerHtml, (IntPtr bytes, uint numBytes, IntPtr param) =>
			{
				htmlValue = Marshal.PtrToStringAnsi(bytes, Convert.ToInt32(numBytes));
			}, IntPtr.Zero);
			
			value = htmlValue;

			return domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK || (domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED && value == null);
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

			var domResult = Api.SciterGetElementTextCB(_elementHandle, (IntPtr strPtr, uint strLength, IntPtr param) =>
			{
				outText = Marshal.PtrToStringUni(strPtr, (int)strLength);
			}, IntPtr.Zero);
			
			text = outText;
			
			return domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK || (domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED && text == null);
		}
		
		internal void SetTextInternal(string text)
		{
			TrySetTextInternal(text: text);
		}
		
		internal bool TrySetTextInternal(string text)
		{
			return Api.SciterSetElementText(_elementHandle, text, (uint) text.Length) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}
		
		#endregion

		#region Attributes and Styles
		
		public Dictionary<string, string> Attributes
		{
			get
			{
				var result = new Dictionary<string, string>();
				for (uint n = 0; n < GetAttributeCountInternal(); n++)
				{
					result[GetAttributeNameInternal(n)] = GetAttributeValueInternal(n);
				}
				return result;
			}
		}

		internal uint GetAttributeCountInternal()
		{
			TryGetAttributeCountInternal(out var result);
			return result;
		}

		internal bool TryGetAttributeCountInternal(out uint result)
		{
			result = default;
			return Api.SciterGetAttributeCount(_elementHandle, out result) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal string GetAttributeValueInternal(uint index)
		{
			TryGetAttributeValueInternal(index: index, out var result);
			return result;
		}

		internal bool TryGetAttributeValueInternal(uint index, out string value)
		{
			string outValue = default;
			
			var domResult = Api.SciterGetNthAttributeValueCB(_elementHandle, index, (IntPtr str, uint strLength, IntPtr param) =>
			{
				outValue = Marshal.PtrToStringUni(str, (int)strLength);
			}, IntPtr.Zero);

			var result = domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK ||
				(domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED && outValue == null);

			value = outValue;
			
			return result;
		}

		internal string GetAttributeValueInternal(string key)
		{
			TryGetAttributeValueInternal(key: key, value: out var result);
			return result;
		}

		internal bool TryGetAttributeValueInternal(string key, out string value)
		{
			string outValue = default;
			
			var domResult = Api.SciterGetAttributeByNameCB(_elementHandle, key, (IntPtr str, uint strLength, IntPtr param) =>
			{
				outValue = Marshal.PtrToStringUni(str, (int) strLength);
			}, IntPtr.Zero);

			var result = domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK ||
			             (domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED && outValue == null);

			value = outValue;
			
			return result;
		}

		internal string GetAttributeNameInternal(uint index)
		{
			TryGetAttributeNameInternal(index: index, out var result);
			return result;
		}

		internal bool TryGetAttributeNameInternal(uint index, out string value)
		{
			string outValue = default;
			
			var domResult = Api.SciterGetNthAttributeNameCB(_elementHandle, index, (IntPtr str, uint strLength, IntPtr param) =>
			{
				outValue = Marshal.PtrToStringAnsi(str, (int) strLength);
			}, IntPtr.Zero);

			var result = domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK ||
			             (domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED && outValue == null);

			value = outValue;
			
			return result;
		}

        internal void SetAttributeValueInternal(string key, string value)
        {
            var r = Api.SciterSetAttributeByName(_elementHandle, key, value);
            Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
        }

        internal void RemoveAttributeInternal(string key)
		{
			Api.SciterSetAttributeByName(_elementHandle, key, null);
		}
		
        internal string GetStyleValueInternal(string key)
		{
			TryGetStyleValueInternal(key: key, out var result);
			return result;
		}
		
        internal bool TryGetStyleValueInternal(string key, out string value)
		{
			string outStyle = default;
			
			var domResult = Api.SciterGetStyleAttributeCB(_elementHandle, key, (IntPtr str, uint strLength, IntPtr param) =>
			{
				outStyle = Marshal.PtrToStringUni(str, Convert.ToInt32(strLength));
			}, IntPtr.Zero);
			
			value = outStyle;
			
			return domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK || (domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED && value == null);
		}
		
        internal void SetStyleValueInternal(string key, string value)
		{
			TrySetStyleValueInternal(key: key, value: value);
		}
		
        internal bool TrySetStyleValueInternal(string key, string value)
		{
			return Api.SciterSetStyleAttribute(_elementHandle, key, value) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}
		
		#endregion

		#region State
		public SciterXDom.ELEMENT_STATE_BITS State => GetElementStateInternal();

		internal SciterXDom.ELEMENT_STATE_BITS GetElementStateInternal()
		{
			TryGetElementStateInternal(out var stateBits);
			return stateBits;
		}

		internal bool TryGetElementStateInternal(out SciterXDom.ELEMENT_STATE_BITS stateBits)
		{
			var domResult = Api.SciterGetElementState(_elementHandle, out var bits);
			var result = domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK;
			
			stateBits = result ? (SciterXDom.ELEMENT_STATE_BITS) bits : default(SciterXDom.ELEMENT_STATE_BITS);
			
			return result;
		}

		internal void SetElementStateInternal(SciterXDom.ELEMENT_STATE_BITS bitsToSet, SciterXDom.ELEMENT_STATE_BITS bitsToClear = 0, bool update = true)
		{
			TrySetElementStateInternal(bitsToSet: bitsToSet, bitsToClear: bitsToClear, update: update);
		}

		internal bool TrySetElementStateInternal(SciterXDom.ELEMENT_STATE_BITS bitsToSet, SciterXDom.ELEMENT_STATE_BITS bitsToClear = 0, bool update = true)
		{
			return Api.SciterSetElementState(_elementHandle, (uint) bitsToSet, (uint) bitsToClear, update) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}
		
		#endregion

		internal string CombineUrlInternal(string url = "")
		{
			var buffer = PInvokeUtils.NativeUtf16FromString(url, 2048);
			var domResult = Api.SciterCombineURL(_elementHandle, buffer, 2048);
			var result = PInvokeUtils.StringFromNativeUtf16(buffer);
			PInvokeUtils.NativeUtf16FromString_FreeBuffer(buffer);
			return result;
		}

		#region Integers
		public IntPtr GetNativeHwnd(bool rootWindow = true)
		{
			Api.SciterGetElementHwnd(_elementHandle, out var result, rootWindow);
			return result;
		}

		public SciterWindow Window
		{
			get
			{
				var hwnd = GetNativeHwnd();
				return hwnd != IntPtr.Zero ? new SciterWindow(hwnd) : null;
			}
		}

		public uint UID
		{
			get
			{
				uint uid;
				Api.SciterGetElementUID(_elementHandle, out uid);
				return uid;
			}
		}

		public uint Index
		{
			get
			{
				Api.SciterGetElementIndex(_elementHandle, out var result);
				return result;
			}
		}

		internal uint ChildCountInternal
		{
			get
			{
				Api.SciterGetChildrenCount(_elementHandle, out var result);
				return result;
			}
		}
		
		#endregion

		internal bool TryDeleteInternal()
		{
			return Api.SciterDeleteElement(_elementHandle) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal bool TryDetachInternal()
		{
			return Api.SciterDetachElement(_elementHandle) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal SciterElement CloneInternal()
		{
			TryCloneInternal(out var result);
			return result;
		}

		internal bool TryCloneInternal(out SciterElement element)
		{
			var result = Api.SciterCloneElement(_elementHandle, out var cloneResult) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
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
			var result = Api.SciterNodeCastFromElement(_elementHandle, out var castResult) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
			node = result ? new SciterNode(castResult) : null;
			return result;
		}

		/// <summary>
		/// Deeply Enabled
		/// </summary>
		internal bool IsEnabledInternal()
		{
			Api.SciterIsElementEnabled(_elementHandle, out var result);
			return result;
		}

		/// <summary>
		/// Deeply Visible
		/// </summary>
		internal bool IsVisibleInternal()
		{
			Api.SciterIsElementVisible(_elementHandle, out var result);
			return result;
		}

		#region Operators and overrides
		
		public static bool operator ==(SciterElement a, SciterElement b)
		{
			if((object)a == null || (object)b == null)
				return Equals(a, b);
			return a.Handle == b.Handle;
		}
		
		public static bool operator !=(SciterElement a, SciterElement b)
		{
			return !(a == b);
		}

		public SciterElement this[uint index] => GetChildAtIndexInternal(index);

		public string this[string name]
		{
			get => GetAttributeValueInternal(name);
			set => SetAttributeValueInternal(name, value);
		}

		public override bool Equals(object o)
		{
			return object.ReferenceEquals(this, o);
		}

		public override int GetHashCode()
		{
			return _elementHandle.ToInt32();
		}

		public override string ToString()
		{
			string tag = Tag;
			string id = GetAttributeValueInternal("id");
			string classes = GetAttributeValueInternal("class");
			uint childCount = this.ChildCountInternal;

			StringBuilder str = new StringBuilder();
			str.Append("<" + tag);
			if(id != null)
				str.Append(" #" + id);
			if(classes != null)
				str.Append(" ." + String.Join(".", classes.Split(' ')));
			if(childCount == 0)
				str.Append(" />");
			else
				str.Append(">...</" + tag + ">");

			return str.ToString();
		}
		#endregion

		#region DOM navigation
		
		internal SciterElement GetChildAtIndexInternal(uint index)
		{
			TryGetChildAtIndexInternal(index, out var element);
			return element;
		}
		
		internal bool TryGetChildAtIndexInternal(uint index, out SciterElement element)
		{
			var result = Api.SciterGetNthChild(_elementHandle, index, out var child_he) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
			element = (result && child_he != IntPtr.Zero) ? new SciterElement(child_he) : null;
			return (result && child_he != IntPtr.Zero);
		}

		public IEnumerable<SciterElement> Children
		{
			get
			{
				var list = new List<SciterElement>();
				for(uint i = 0; i < ChildCountInternal; i++)
					list.Add(this[i]);
				return list;
			}
		}

		public SciterElement Parent
		{
			get
			{
				Api.SciterGetParentElement(_elementHandle, out var out_he);
				return out_he == IntPtr.Zero ? null : new SciterElement(out_he);
			}
		}

		internal SciterElement NextSiblingInternal => this.Parent?.GetChildAtIndexInternal(this.Index + 1);

		internal SciterElement PreviousSiblingInternal => this.Parent?.GetChildAtIndexInternal(this.Index - 1);

		internal SciterElement FirstSiblingInternal => this.Parent?.GetChildAtIndexInternal(0);

		internal SciterElement LastSiblingInternal => this.Parent?.GetChildAtIndexInternal(this.Parent.ChildCountInternal - 1);

		#endregion

		#region DOM query/select
		internal SciterElement SelectFirstByIdInternal(string id)
		{
			return SelectFirstInternal("[id='" + id + "']");
		}

		internal SciterElement SelectFirstInternal(string selector)
		{
			SciterElement result = null;
			
			Api.SciterSelectElementsW(_elementHandle, selector, (IntPtr he, IntPtr param) =>
			{
				result = new SciterElement(he);
				return true;// true stops enumeration
			}, IntPtr.Zero);
			
			return result;
		}

		internal IEnumerable<SciterElement> SelectAllInternal(string selector)
		{
			var result = new List<SciterElement>();

			Api.SciterSelectElementsW(_elementHandle, selector, 
				(IntPtr he, IntPtr param) =>
						{
							result.Add(new SciterElement(he));
							return false;// false continue enumeration
						}, IntPtr.Zero);
			
			return result;
		}

		internal SciterElement SelectNearestParentInternal(string selector)
		{
			Api.SciterSelectParentW(_elementHandle, selector, 0, out var heFound);
			return heFound == IntPtr.Zero ? null : new SciterElement(heFound);
		}
		#endregion

		#region DOM sub-tree manipulation
		
		internal bool InsertElementInternal(SciterElement element, uint index = 0)
		{
			return Api.SciterInsertElement(element._elementHandle, _elementHandle, index) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal bool AppendElementInternal(SciterElement element)
		{
			return Api.SciterInsertElement(element._elementHandle, _elementHandle, int.MaxValue) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal bool TryAppendElementInternal(SciterElement element)
		{
			return Api.SciterInsertElement(element._elementHandle, _elementHandle, int.MaxValue) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal bool AppendElementInternal(string tagName, string text = null)
        {
            var element = Create(tagName, text);
            return AppendElementInternal(element);
        }

		internal bool TryAppendElementInternal(string tagName, out SciterElement element, string text = null)
        {
            element = Create(tagName, text);
            return AppendElementInternal(element);
        }

		internal bool SwapElementsInternal(SciterElement swapElement)
		{
			return Api.SciterSwapElements(_elementHandle, swapElement._elementHandle) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal bool ClearTextInternal()
		{
			return Api.SciterSetElementText(_elementHandle, null, 0) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal bool TransformHtmlInternal(string html, SciterXDom.SET_ELEMENT_HTML replacement = SciterXDom.SET_ELEMENT_HTML.SIH_REPLACE_CONTENT)
		{
			var bytes = Encoding.UTF8.GetBytes(html);
			return TransformHtmlInternal(bytes: bytes, replacement: replacement);
		}

		internal bool TransformHtmlInternal(byte[] bytes, SciterXDom.SET_ELEMENT_HTML replacement = SciterXDom.SET_ELEMENT_HTML.SIH_REPLACE_CONTENT)
		{
			return Api.SciterSetElementHtml(_elementHandle, bytes, (uint) bytes.Length, replacement) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}
		
		#endregion

		#region Events
		
		internal bool AttachEventHandlerInternal(SciterEventHandler evh)
		{
			return Api.SciterAttachEventHandler(_elementHandle, evh.EventProc, IntPtr.Zero) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}
		
		public void DetachEventHandler(SciterEventHandler evh)
		{
			Debug.Assert(evh != null);
			var r = Api.SciterDetachEventHandler(_elementHandle, evh.EventProc, IntPtr.Zero);
			Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
		}

		public bool SendEvent(uint event_code, uint reason = 0, SciterElement heSource = null)
		{
			bool handled;
			Api.SciterSendEvent(_elementHandle, event_code, heSource == null ? IntPtr.Zero : heSource._elementHandle, new IntPtr(reason), out handled);
			return handled;
		}

		public void PostEvent(uint event_code, uint reason = 0, SciterElement heSource = null)
		{
			Api.SciterPostEvent(_elementHandle, event_code, heSource == null ? IntPtr.Zero : heSource._elementHandle, new IntPtr(reason));
		}

		public bool FireEvent(SciterBehaviors.BEHAVIOR_EVENT_PARAMS evt, bool post = true)
		{
			bool handled;
			Api.SciterFireEvent(ref evt, post, out handled);
			return handled;
		}
		#endregion

		#region Location and Size
		public PInvokeUtils.RECT GetLocation(SciterXDom.ELEMENT_AREAS area = SciterXDom.ELEMENT_AREAS.ROOT_RELATIVE | SciterXDom.ELEMENT_AREAS.CONTENT_BOX)
		{
			PInvokeUtils.RECT rc;
			Api.SciterGetElementLocation(_elementHandle, out rc, area);
			return rc;
		}

		public PInvokeUtils.SIZE SizePadding
		{
			get
			{
				PInvokeUtils.RECT rc;
				Api.SciterGetElementLocation(_elementHandle, out rc, SciterXDom.ELEMENT_AREAS.ROOT_RELATIVE | SciterXDom.ELEMENT_AREAS.PADDING_BOX);
				return new PInvokeUtils.SIZE() { cx = rc.Width, cy = rc.Height };
			}
		}
		#endregion

		/// <summary>
		/// Test this element against CSS selector(s)
		/// </summary>
		public bool Test(string selector)
		{
			Api.SciterSelectParent(_elementHandle, selector, 1, out var heFound);
			return heFound != IntPtr.Zero;
		}

		internal bool TryUpdateInternal(bool forceRender = false)
		{
			return Api.SciterUpdateElement(_elementHandle, forceRender) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal bool TryRefreshInternal(PInvokeUtils.RECT rect)
		{
			return Api.SciterRefreshElementArea(_elementHandle, rect) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}
		
		internal bool TryRefreshInternal()
		{
			return Api.SciterRefreshElementArea(_elementHandle, GetLocation(SciterXDom.ELEMENT_AREAS.SELF_RELATIVE | SciterXDom.ELEMENT_AREAS.CONTENT_BOX)) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		#region Scripting
		public SciterValue Value
		{
			get
			{
				Interop.SciterValue.VALUE val;
				Api.SciterGetValue(_elementHandle, out val);
				return new SciterValue(val);
			}

			set
			{
				var val = value.ToVALUE();
				Api.SciterSetValue(_elementHandle, ref val);
			}
		}

		public SciterValue ExpandoValue
		{
			get
			{
				Interop.SciterValue.VALUE val;
				Api.SciterGetExpando(_elementHandle, out val, true);
				return new SciterValue(val);
			}
		}


		// call scripting method attached to the element (directly or through of scripting behavior)  
		// Example, script:
		//   var elem = ...
		//   elem.foo = function() {...}
		// Native code: 
		//   SciterElement elem = ...
		//   elem.CallMethod("foo");
		public SciterValue CallMethod(string name, params SciterValue[] args)
		{
			Debug.Assert(name != null);

			Interop.SciterValue.VALUE vret;
			Api.SciterCallScriptingMethod(_elementHandle, name, SciterValue.ToVALUEArray(args), (uint) args.Length, out vret);
			return new SciterValue(vret);
		}

		// call scripting function defined on global level   
		// Example, script:
		//   function foo() {...}
		// Native code: 
		//   dom::element root = ... get root element of main document or some frame inside it
		//   root.call_function("foo"); // call the function
		public SciterValue CallFunction(string name, params SciterValue[] args)
		{
			Debug.Assert(name != null);

			Interop.SciterValue.VALUE vret;
			Api.SciterCallScriptingFunction(_elementHandle, name, SciterValue.ToVALUEArray(args), (uint) args.Length, out vret);
			return new SciterValue(vret);
		}

		public SciterValue Eval(string script)
		{
			Interop.SciterValue.VALUE rv;
			Api.SciterEvalElementScript(_elementHandle, script, (uint) script.Length, out rv);
			return new SciterValue(rv);
		}
		#endregion

		#region Highlight set/get
		public bool Highlight
		{
			set => Api.SciterSetHighlightedElement(GetNativeHwnd(), value ? _elementHandle : IntPtr.Zero);
		}
		#endregion
	}

	public class SciterNode
	{
		private static Sciter.SciterApi _api = Sciter.Api;
		public IntPtr _hn { get; private set; }

		public SciterNode(IntPtr hn)
		{
			Debug.Assert(hn != IntPtr.Zero);
			if(hn == IntPtr.Zero)
				throw new ArgumentException("IntPtr.Zero received at SciterNode constructor");

			_hn = hn;
		}

		public static SciterNode MakeTextNode(string text)
		{
			IntPtr hn;
			_api.SciterCreateTextNode(text, (uint)text.Length, out hn);
			if(hn != IntPtr.Zero)
				return new SciterNode(hn);
			return null;
		}

		public static SciterNode MakeCommentNode(string text)
		{
			IntPtr hn;
			_api.SciterCreateCommentNode(text, (uint)text.Length, out hn);
			if(hn != IntPtr.Zero)
				return new SciterNode(hn);
			return null;
		}

		public uint ChildrenCount
		{
			get
			{
				uint n;
				_api.SciterNodeChildrenCount(_hn, out n);
				return n;
			}
		}

		public SciterElement ToElement()
		{
			IntPtr he;
			var r = _api.SciterNodeCastToElement(_hn, out he);
			Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
			return new SciterElement(he);
		}

		public bool IsText
		{
			get
			{
				SciterXDom.NODE_TYPE nodeType;
				var r = _api.SciterNodeType(_hn, out nodeType);
				Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
				return nodeType == SciterXDom.NODE_TYPE.NT_TEXT;
			}
		}
		public bool IsComment
		{
			get
			{
				SciterXDom.NODE_TYPE nodeType;
				var r = _api.SciterNodeType(_hn, out nodeType);
				Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
				return nodeType == SciterXDom.NODE_TYPE.NT_COMMENT;
			}
		}
		public bool IsElement
		{
			get
			{
				SciterXDom.NODE_TYPE nodeType;
				var r = _api.SciterNodeType(_hn, out nodeType);
				Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
				return nodeType == SciterXDom.NODE_TYPE.NT_ELEMENT;
			}
		}

		public SciterNode this[uint idx]
		{
			get
			{
				return GetChild(idx);
			}
		}

		public string Text
		{
			get
			{
				string strval = null;
				SciterXDom.LPCWSTR_RECEIVER frcv = (IntPtr str, uint str_length, IntPtr param) =>
				{
					if(str != IntPtr.Zero)
						strval = Marshal.PtrToStringUni(str, (int)str_length);
				};

				var r = _api.SciterNodeGetText(_hn, frcv, IntPtr.Zero);
				if(r == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED)
					Debug.Assert(strval == null);
				return strval;
			}
		}

		#region DOM navigation
		public SciterNode GetChild(uint idx)
		{
			IntPtr child_hn;
			_api.SciterNodeNthChild(_hn, idx, out child_hn);
			if(child_hn == IntPtr.Zero)
				return null;
			return new SciterNode(child_hn);
		}
		#endregion
	}
}