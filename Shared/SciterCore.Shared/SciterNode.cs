using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SciterCore.Interop;

namespace SciterCore
{
    public class SciterNode
	{
		private static readonly Sciter.SciterApi Api = Sciter.Api;
		
		private readonly IntPtr _nodeHandle;

		public IntPtr Handle => _nodeHandle;
		
		public SciterNode(IntPtr nodeHandle)
		{
			Debug.Assert(nodeHandle != IntPtr.Zero);
			if(nodeHandle == IntPtr.Zero)
				throw new ArgumentException("IntPtr.Zero received at SciterNode constructor");

			_nodeHandle = nodeHandle;
		}

		public static SciterNode MakeTextNode(string text)
		{
			Api.SciterCreateTextNode(text, (uint)text.Length, out var nodeHandle);
			return nodeHandle != IntPtr.Zero ? new SciterNode(nodeHandle) : null;
		}

		public static SciterNode MakeCommentNode(string text)
		{
			Api.SciterCreateCommentNode(text, (uint)text.Length, out var nodeHandle);
			return nodeHandle != IntPtr.Zero ? new SciterNode(nodeHandle) : null;
		}

		public uint ChildrenCount
		{
			get
			{
				Api.SciterNodeChildrenCount(Handle, out var count);
				return count;
			}
		}

		public SciterElement ToElement()
		{
			var r = Api.SciterNodeCastToElement(Handle, out var elementHandle);
			Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
			return new SciterElement(elementHandle);
		}

		public bool IsText
		{
			get
			{
				var r = Api.SciterNodeType(Handle, out var nodeType);
				Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
				return nodeType == SciterXDom.NODE_TYPE.NT_TEXT;
			}
		}
		
		public bool IsComment
		{
			get
			{
				var r = Api.SciterNodeType(Handle, out var nodeType);
				Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
				return nodeType == SciterXDom.NODE_TYPE.NT_COMMENT;
			}
		}
		
		public bool IsElement
		{
			get
			{
				var r = Api.SciterNodeType(Handle, out var nodeType);
				Debug.Assert(r == SciterXDom.SCDOM_RESULT.SCDOM_OK);
				return nodeType == SciterXDom.NODE_TYPE.NT_ELEMENT;
			}
		}

		public SciterNode this[uint idx] => GetChild(idx);

		public string Text
		{
			get
			{
				string outText = null;

				var domResult = Api.SciterNodeGetText(
					Handle, 
					(IntPtr str, uint strLength, IntPtr param) =>
						{
							if(str != IntPtr.Zero)
								outText = Marshal.PtrToStringUni(str, (int)strLength);
						}, 
					IntPtr.Zero);
				
				if(domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED)
					Debug.Assert(outText == null);
				
				return (domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK || (domResult == SciterXDom.SCDOM_RESULT.SCDOM_OK_NOT_HANDLED && outText == null)) ? outText : null;
			}
		}

		#region DOM navigation
		
		public SciterNode GetChild(uint idx)
		{
			Api.SciterNodeNthChild(Handle, idx, out var nodeHandle);
			return nodeHandle == IntPtr.Zero ? null : new SciterNode(nodeHandle);
		}
		
		#endregion
	}
}