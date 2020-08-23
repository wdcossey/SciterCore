namespace SciterCore
{
    public static class SciterNodeExtensions
    {

        #region Text

        public static string GetText(this SciterNode sciterNode)
        {
            return sciterNode?.GetTextInternal();
        }
		
        public static bool TryGetText(this SciterNode sciterNode, out string text)
        {
            text = default;
            return sciterNode?.TryGetTextInternal(text: out text) == true;
        }
		
        public static SciterNode SetText(this SciterNode sciterNode, string text)
        {
            sciterNode?.SetTextInternal(text: text);
            return sciterNode;
        }
		
        public static bool TrySetText(this SciterNode sciterNode, string text)
        {
            return sciterNode?.TrySetTextInternal(text: text) == true;
        }

        #endregion Text
        
        #region CreateTextNode

        public static SciterNode CreateTextNode(this SciterNode sciterNode, string text)
        {
            return sciterNode?.CreateTextNodeInternal(text: text);
        }

        public static bool TryCreateTextNode(this SciterNode sciterNode, string text, out SciterNode value)
        {
            value = default;
            return sciterNode?.TryCreateTextNodeInternal(text: text, value: out value) == true;
        }

        #endregion CreateTextNode

        #region CreateCommentNode

        public static SciterNode CreateCommentNode(this SciterNode sciterNode, string text)
        {
            return sciterNode?.CreateCommentNodeInternal(text: text);
        }

        public static bool TryCreateCommentNode(this SciterNode sciterNode, string text, out SciterNode value)
        {
            value = default;
            return sciterNode?.TryCreateCommentNodeInternal(text: text, value: out value) == true;
        }

        #endregion CreateCommentNode
        
        
        #region ChildCount

        public static int GetChildCount(this SciterNode sciterNode)
        {
            return sciterNode.GetChildCountInternal();
        }

        public static bool TryGetChildCount(this SciterNode sciterNode, out int value)
        {
            return sciterNode.TryGetChildCountInternal(out value);
        }

        #endregion ChildCount
        
        #region CastToElement
        public static SciterElement CastToElement(this SciterNode sciterNode)
        {
            return sciterNode?.CastToElementInternal();
        }

        public static bool TryCastToElement(this SciterNode sciterNode, out SciterElement value)
        {
            value = default;
            return sciterNode?.TryCastToElementInternal(value: out value) == true;
        }
        
        #endregion ToElement
        
        #region DOM Navigation
		
        public static SciterNode GetChild(this SciterNode sciterNode, int index)
        {
            return sciterNode?.GetChildInternal(index: index);
        }
		
        public static bool TryGetChild(this SciterNode sciterNode, int index, out SciterNode value)
        {
            value = default;
            return sciterNode?.TryGetChildInternal(index: index, value: out value) == true;
        }
		
        #endregion DOM Navigation
    }
}