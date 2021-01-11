using System;
using System.Runtime.InteropServices;
using SciterCore.Interop;

// ReSharper disable RedundantLambdaParameterType
// ReSharper disable ArrangeThisQualifier
// ReSharper disable UnusedMember.Global

namespace SciterCore
{
    public class SciterNode
	{
		private static readonly ISciterApi SciterApi = Sciter.SciterApi;
		
		private readonly IntPtr _nodeHandle;

		public IntPtr Handle => _nodeHandle;
		
		public SciterNode(IntPtr nodeHandle)
		{
			if(nodeHandle == IntPtr.Zero)
				throw new ArgumentException("IntPtr.Zero received at SciterNode constructor");

			_nodeHandle = nodeHandle;
		}

		internal SciterNode CreateTextNodeInternal(string text)
		{
			TryCreateTextNodeInternal(text: text, out var result);
			return result;
		}

		internal bool TryCreateTextNodeInternal(string text, out SciterNode value)
		{
			var result = SciterApi.SciterCreateTextNode(text, System.Convert.ToUInt32(text.Length), out var nodeHandle)
				.IsOk();
			
			value = result ? new SciterNode(nodeHandle) : default;
			
			return result;
		}

		internal SciterNode CreateCommentNodeInternal(string text)
		{
			TryCreateCommentNodeInternal(text: text, out var result);
			return result;
		}

		internal bool TryCreateCommentNodeInternal(string text, out SciterNode value)
		{
			var result= SciterApi.SciterCreateCommentNode(text, System.Convert.ToUInt32(text.Length), out var nodeHandle)
				.IsOk();
			
			value = result ? new SciterNode(nodeHandle) : default;
			
			return result;
		}

		public int ChildCount => GetChildCountInternal();

		internal int GetChildCountInternal()
		{
			TryGetChildCountInternal(out var result);
			return result;
		}

		internal bool TryGetChildCountInternal(out int value)
		{
			var result = SciterApi.SciterNodeChildrenCount(this.Handle, out var count)
				.IsOk();
			value = result ? System.Convert.ToInt32(count) : 0;
			return result;
		}

		internal SciterElement CastToElementInternal()
		{
			TryCastToElementInternal(out var result);
			return result;
		}

		internal bool TryCastToElementInternal(out SciterElement value)
		{
			var result = SciterApi.SciterNodeCastToElement(this.Handle, out var elementHandle).IsOk();
			value = result ? new SciterElement(elementHandle) : default;
			return result;
		}

		public NodeType NodeType => GetNodeType();
		
		private NodeType GetNodeType()
		{
			var result = SciterApi.SciterNodeType(this.Handle, out var nodeType)
				.IsOk();

			return result ? (NodeType)(int)nodeType : NodeType.Undefined;
		}

		public SciterNode this[int childIndex] => GetChildInternal(childIndex);

		#region Text

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

			var domResult = SciterApi.SciterNodeGetText(
				this.Handle, 
				(IntPtr strPtr, uint strLength, IntPtr param) =>
				{
					outText = Marshal.PtrToStringUni(strPtr, System.Convert.ToInt32(strLength));
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
			return SciterApi.SciterNodeSetText(this.Handle, text, System.Convert.ToUInt32(text.Length))
				.IsOk();
		}
		
		#endregion Text
		
		#region DOM Navigation
		
		internal SciterNode GetChildInternal(int index)
		{
			SciterApi.SciterNodeNthChild(this.Handle, System.Convert.ToUInt32(index), out var nodeHandle);
			return nodeHandle == IntPtr.Zero ? null : new SciterNode(nodeHandle);
		}
		
		internal bool TryGetChildInternal(int index, out SciterNode value)
		{
			var result = SciterApi.SciterNodeNthChild(this.Handle, System.Convert.ToUInt32(index), out var nodeHandle)
				.IsOk();
			
			value = result ? new SciterNode(nodeHandle) : default;
			return result;
		}
		
		#endregion DOM Navigation
	}
}