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
		private static Sciter.SciterApi _api = Sciter.Api;
		public IntPtr _he { get; private set; }

		public SciterElement(IntPtr he)
		{
			Debug.Assert(he != IntPtr.Zero);
			if(he == IntPtr.Zero)
				throw new ArgumentException("IntPtr.Zero received at SciterElement constructor");

			_api.Sciter_UseElement(he);
			_he = he;
		}

		public SciterElement(SciterValue sv)
		{
			if(!sv.IsObject)
				throw new ArgumentException("The given SciterValue is not a TIScript Element reference");

			IntPtr he = sv.GetObjectData();
			if(he == IntPtr.Zero)
				throw new ArgumentException("IntPtr.Zero received at SciterElement constructor");

			_api.Sciter_UseElement(_he);
			_he = he;
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if(!disposedValue)
			{
				if(disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
				_api.Sciter_UnuseElement(_he);

				disposedValue = true;
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
			IntPtr he;
			_api.SciterCreateElement(tagName, text, out he);
			if (he != IntPtr.Zero)
				return new SciterElement(he);
			return null;
		}

		#region Query HTML
		public string Tag
		{
			get
			{
				IntPtr ptrtag;
				var r = _api.SciterGetElementType(_he, out ptrtag);
				Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
				return Marshal.PtrToStringAnsi(ptrtag);
			}
		}

		public string HTML
		{
			get
			{
				string strval = null;
				SciterXDom.LPCBYTE_RECEIVER frcv = (IntPtr bytes, uint num_bytes, IntPtr param) =>
				{
					strval = Marshal.PtrToStringAnsi(bytes, (int)num_bytes);
				};

				var r = _api.SciterGetElementHtmlCB(_he, true, frcv, IntPtr.Zero);
				if(r == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED)
					Debug.Assert(strval == null);
				return strval;
			}
		}

		public string InnerHTML
		{
			get
			{
				string strval = null;
				SciterXDom.LPCBYTE_RECEIVER frcv = (IntPtr bytes, uint num_bytes, IntPtr param) =>
				{
					strval = Marshal.PtrToStringAnsi(bytes, (int)num_bytes);
				};

				var r = _api.SciterGetElementHtmlCB(_he, false, frcv, IntPtr.Zero);
				if(r == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED)
					Debug.Assert(strval == null);
				return strval;
			}
		}

		public string Text
		{
			get
			{
				string strval = null;
				SciterXDom.LPCWSTR_RECEIVER frcv = (IntPtr str, uint str_length, IntPtr param) =>
				{
					strval = Marshal.PtrToStringUni(str, (int)str_length);
				};

				var r = _api.SciterGetElementTextCB(_he, frcv, IntPtr.Zero);
				if(r == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED)
					Debug.Assert(strval == null);
				return strval;
			}

			set
			{
				_api.SciterSetElementText(_he, value, (uint) value.Length);
			}
		}

		public void SetHTML(string html, SciterXDom.SET_ELEMENT_HTML where = SciterXDom.SET_ELEMENT_HTML.SIH_REPLACE_CONTENT)
		{
			if(html==null)
				ClearTextInternal();
			else
			{
				var data = Encoding.UTF8.GetBytes(html);
				_api.SciterSetElementHtml(_he, data, (uint) data.Length, where);
			}
		}
		#endregion

		#region Attributes and Styles
		
		public Dictionary<string, string> Attributes
		{
			get
			{
				var result = new Dictionary<string, string>();
				for (uint n = 0; n < AttributeCountInternal; n++)
				{
					result[GetAttributeNameInternal(n)] = GetAttributeInternal(n);
				}
				return result;
			}
		}

		internal uint AttributeCountInternal
		{
			get
			{
				_api.SciterGetAttributeCount(_he, out var result);
				return result;
			}
		}

		internal string GetAttributeInternal(uint n)
		{
			string strval = null;
			SciterXDom.LPCWSTR_RECEIVER frcv = (IntPtr str, uint str_length, IntPtr param) =>
			{
				strval = Marshal.PtrToStringUni(str, (int)str_length);
			};

			var r = _api.SciterGetNthAttributeValueCB(_he, n, frcv, IntPtr.Zero);
			if(r == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED)
				Debug.Assert(strval == null);
			return strval;
		}

		internal string GetAttributeInternal(string name)
		{
			string strval = null;
			SciterXDom.LPCWSTR_RECEIVER frcv = (IntPtr str, uint str_length, IntPtr param) =>
			{
				strval = Marshal.PtrToStringUni(str, (int) str_length);
			};

			var r = _api.SciterGetAttributeByNameCB(_he, name, frcv, IntPtr.Zero);
			if(r == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED)
				Debug.Assert(strval == null);
			return strval;
		}

		internal string GetAttributeNameInternal(uint n)
		{
			string strval = null;
			SciterXDom.LPCSTR_RECEIVER frcv = (IntPtr str, uint str_length, IntPtr param) =>
			{
				strval = Marshal.PtrToStringAnsi(str, (int)str_length);
			};

			var r = _api.SciterGetNthAttributeNameCB(_he, n, frcv, IntPtr.Zero);
			if(r == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED)
				Debug.Assert(strval == null);
			return strval;
		}

        internal void SetAttributeInternal(string name, object value)
        {
            var r = _api.SciterSetAttributeByName(_he, name, $"{value}");
            Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
        }

        internal void RemoveAttributeInternal(string name)
		{
			_api.SciterSetAttributeByName(_he, name, null);
		}
		
        internal string GetStyleInternal(string name)
		{
			var result = default(string);
			SciterXDom.LPCWSTR_RECEIVER receiver = (IntPtr str, uint str_length, IntPtr param) =>
			{
				result = Marshal.PtrToStringUni(str, (int)str_length);
			};

			var r = _api.SciterGetStyleAttributeCB(_he, name, receiver, IntPtr.Zero);
			if(r == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED)
				Debug.Assert(result == null);
			return result;
		}
		
        internal void SetStyleInternal(string name, string value)
		{
			_api.SciterSetStyleAttribute(_he, name, value);
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
			var domResult = _api.SciterGetElementState(_he, out var bits);
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
			return _api.SciterSetElementState(_he, (uint) bitsToSet, (uint) bitsToClear, update) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}
		
		#endregion

		internal string CombineUrlInternal(string url = "")
		{
			var buffer = PInvokeUtils.NativeUtf16FromString(url, 2048);
			var domResult = _api.SciterCombineURL(_he, buffer, 2048);
			var result = PInvokeUtils.StringFromNativeUtf16(buffer);
			PInvokeUtils.NativeUtf16FromString_FreeBuffer(buffer);
			return result;
		}

		#region Integers
		public IntPtr GetNativeHwnd(bool rootWindow = true)
		{
			_api.SciterGetElementHwnd(_he, out var result, rootWindow);
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
				_api.SciterGetElementUID(_he, out uid);
				return uid;
			}
		}

		public uint Index
		{
			get
			{
				_api.SciterGetElementIndex(_he, out var result);
				return result;
			}
		}

		internal uint ChildCountInternal
		{
			get
			{
				_api.SciterGetChildrenCount(_he, out var result);
				return result;
			}
		}
		
		#endregion

		public void Delete()
		{
			_api.SciterDeleteElement(_he);
		}
		
		internal void DetachElementInternal()
		{
			_api.SciterDetachElement(_he);
		}

		internal SciterElement CloneElementInternal()
		{
			_api.SciterCloneElement(_he, out var result);
			return new SciterElement(result);
		}

		internal SciterNode CastToNodeInternal()
		{
			_api.SciterNodeCastFromElement(_he, out var result);
			return new SciterNode(result);
		}

		/// <summary>
		/// Deeply Enabled
		/// </summary>
		public bool IsEnabled
		{
			get
			{
				_api.SciterIsElementEnabled(_he, out var result);
				return result;
			}
		}

		/// <summary>
		/// Deeply Visible
		/// </summary>
		public bool IsVisible
		{
			get
			{
				_api.SciterIsElementVisible(_he, out var result);
				return result;
			}
		}

		#region Operators and overrides
		
		public static bool operator ==(SciterElement a, SciterElement b)
		{
			if((object)a == null || (object)b == null)
				return Object.Equals(a, b);
			return a._he == b._he;
		}
		
		public static bool operator !=(SciterElement a, SciterElement b)
		{
			return !(a == b);
		}

		public SciterElement this[uint idx] => GetChildElementInternal(idx);

		public string this[string name]
		{
			get => GetAttributeInternal(name);
			set => SetAttributeInternal(name, value);
		}

		public override bool Equals(object o)
		{
			return object.ReferenceEquals(this, o);
		}

		public override int GetHashCode()
		{
			return _he.ToInt32();
		}

		public override string ToString()
		{
			string tag = Tag;
			string id = GetAttributeInternal("id");
			string classes = GetAttributeInternal("class");
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
		internal SciterElement GetChildElementInternal(uint index)
		{
			_api.SciterGetNthChild(_he, index, out var child_he);
			return child_he == IntPtr.Zero ? null : new SciterElement(child_he);
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
				_api.SciterGetParentElement(_he, out var out_he);
				return out_he == IntPtr.Zero ? null : new SciterElement(out_he);
			}
		}

		internal SciterElement NextSiblingInternal => this.Parent?.GetChildElementInternal(this.Index + 1);

		internal SciterElement PreviousSiblingInternal => this.Parent?.GetChildElementInternal(this.Index - 1);

		internal SciterElement FirstSiblingInternal => this.Parent?.GetChildElementInternal(0);

		internal SciterElement LastSiblingInternal => this.Parent?.GetChildElementInternal(this.Parent.ChildCountInternal - 1);

		#endregion

		#region DOM query/select
		internal SciterElement SelectFirstByIdInternal(string id)
		{
			return SelectFirstInternal("[id='" + id + "']");
		}

		internal SciterElement SelectFirstInternal(string selector)
		{
			SciterElement result = null;
			
			_api.SciterSelectElementsW(_he, selector, (IntPtr he, IntPtr param) =>
			{
				result = new SciterElement(he);
				return true;// true stops enumeration
			}, IntPtr.Zero);
			
			return result;
		}

		internal IEnumerable<SciterElement> SelectAllInternal(string selector)
		{
			var result = new List<SciterElement>();

			_api.SciterSelectElementsW(_he, selector, 
				(IntPtr he, IntPtr param) =>
						{
							result.Add(new SciterElement(he));
							return false;// false continue enumeration
						}, IntPtr.Zero);
			
			return result;
		}

		internal SciterElement SelectNearestParentInternal(string selector)
		{
			_api.SciterSelectParentW(_he, selector, 0, out var heFound);
			return heFound == IntPtr.Zero ? null : new SciterElement(heFound);
		}
		#endregion

		#region DOM sub-tree manipulation
		
		internal bool InsertElementInternal(SciterElement element, uint index = 0)
		{
			return _api.SciterInsertElement(element._he, _he, index) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal bool AppendElementInternal(SciterElement element)
		{
			return _api.SciterInsertElement(element._he, _he, int.MaxValue) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal bool TryAppendElementInternal(SciterElement element)
		{
			return _api.SciterInsertElement(element._he, _he, int.MaxValue) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
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
			return _api.SciterSwapElements(_he, swapElement._he) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal bool ClearTextInternal()
		{
			return _api.SciterSetElementText(_he, null, 0) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}

		internal bool TransformHtmlInternal(string html, SciterXDom.SET_ELEMENT_HTML replacement = SciterXDom.SET_ELEMENT_HTML.SIH_REPLACE_CONTENT)
		{
			var bytes = Encoding.UTF8.GetBytes(html);
			return TransformHtmlInternal(bytes: bytes, replacement: replacement);
		}

		internal bool TransformHtmlInternal(byte[] bytes, SciterXDom.SET_ELEMENT_HTML replacement = SciterXDom.SET_ELEMENT_HTML.SIH_REPLACE_CONTENT)
		{
			return _api.SciterSetElementHtml(_he, bytes, (uint) bytes.Length, replacement) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}
		
		#endregion

		#region Events
		
		internal bool AttachEventHandlerInternal(SciterEventHandler evh)
		{
			return _api.SciterAttachEventHandler(_he, evh.EventProc, IntPtr.Zero) == SciterXDom.SCDOM_RESULT.SCDOM_OK;
		}
		
		public void DetachEventHandler(SciterEventHandler evh)
		{
			Debug.Assert(evh != null);
			var r = _api.SciterDetachEventHandler(_he, evh.EventProc, IntPtr.Zero);
			Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
		}

		public bool SendEvent(uint event_code, uint reason = 0, SciterElement heSource = null)
		{
			bool handled;
			_api.SciterSendEvent(_he, event_code, heSource == null ? IntPtr.Zero : heSource._he, new IntPtr(reason), out handled);
			return handled;
		}

		public void PostEvent(uint event_code, uint reason = 0, SciterElement heSource = null)
		{
			_api.SciterPostEvent(_he, event_code, heSource == null ? IntPtr.Zero : heSource._he, new IntPtr(reason));
		}

		public bool FireEvent(SciterBehaviors.BEHAVIOR_EVENT_PARAMS evt, bool post = true)
		{
			bool handled;
			_api.SciterFireEvent(ref evt, post, out handled);
			return handled;
		}
		#endregion

		#region Location and Size
		public PInvokeUtils.RECT GetLocation(SciterXDom.ELEMENT_AREAS area = SciterXDom.ELEMENT_AREAS.ROOT_RELATIVE | SciterXDom.ELEMENT_AREAS.CONTENT_BOX)
		{
			PInvokeUtils.RECT rc;
			_api.SciterGetElementLocation(_he, out rc, area);
			return rc;
		}

		public PInvokeUtils.SIZE SizePadding
		{
			get
			{
				PInvokeUtils.RECT rc;
				_api.SciterGetElementLocation(_he, out rc, SciterXDom.ELEMENT_AREAS.ROOT_RELATIVE | SciterXDom.ELEMENT_AREAS.PADDING_BOX);
				return new PInvokeUtils.SIZE() { cx = rc.Width, cy = rc.Height };
			}
		}
		#endregion

		/// <summary>
		/// Test this element against CSS selector(s)
		/// </summary>
		public bool Test(string selector)
		{
			IntPtr heFound;
			_api.SciterSelectParent(_he, selector, 1, out heFound);
			return heFound != IntPtr.Zero;
		}

		public void Update(bool andForceRender = false)
		{
			_api.SciterUpdateElement(_he, andForceRender);
		}

		public void Refresh(PInvokeUtils.RECT rc)
		{
			_api.SciterRefreshElementArea(_he, rc);
		}
		public void Refresh()
		{
			_api.SciterRefreshElementArea(_he, GetLocation(SciterXDom.ELEMENT_AREAS.SELF_RELATIVE | SciterXDom.ELEMENT_AREAS.CONTENT_BOX));
		}

		#region Scripting
		public SciterValue Value
		{
			get
			{
				Interop.SciterValue.VALUE val;
				_api.SciterGetValue(_he, out val);
				return new SciterValue(val);
			}

			set
			{
				var val = value.ToVALUE();
				_api.SciterSetValue(_he, ref val);
			}
		}

		public SciterValue ExpandoValue
		{
			get
			{
				Interop.SciterValue.VALUE val;
				_api.SciterGetExpando(_he, out val, true);
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
			_api.SciterCallScriptingMethod(_he, name, SciterValue.ToVALUEArray(args), (uint) args.Length, out vret);
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
			_api.SciterCallScriptingFunction(_he, name, SciterValue.ToVALUEArray(args), (uint) args.Length, out vret);
			return new SciterValue(vret);
		}

		public SciterValue Eval(string script)
		{
			Interop.SciterValue.VALUE rv;
			_api.SciterEvalElementScript(_he, script, (uint) script.Length, out rv);
			return new SciterValue(rv);
		}
		#endregion

		#region Highlight set/get
		public bool Highlight
		{
			set
			{
				if(value)
					_api.SciterSetHighlightedElement(GetNativeHwnd(), _he);
				else
					_api.SciterSetHighlightedElement(GetNativeHwnd(), IntPtr.Zero);
			}
		}
		#endregion

		#region Helpers
		
		public bool IsChildOf(SciterElement element)
		{
			var parentElement = this;
			
			while(true)
			{
				if (parentElement._he == element._he)
					return true;

				parentElement = parentElement.Parent;
				
				if (parentElement == null)
					break;
			}
			return false;
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