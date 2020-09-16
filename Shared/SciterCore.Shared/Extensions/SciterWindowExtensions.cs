// ReSharper disable UnusedMember.Global
// ReSharper disable ArgumentsStyleNamedExpression
// ReSharper disable RedundantTypeSpecificationInDefaultExpression
namespace SciterCore
{
    public static class SciterWindowExtensions
    {
        #region Title
        
        public static string GetTitle(this SciterWindow window)
        {
            return window.GetTitleInternal();
        }
        
        public static SciterWindow SetTitle(this SciterWindow window, string title)
        {
            window.SetTitleInternal(title);
            return window;
        }
        
        #endregion

        #region Elements

        public static SciterElement GetRootElement(this SciterWindow window)
        {
            return window?.GetRootElementInternal();
        }

        public static bool TryGetRootElement(this SciterWindow window, out SciterElement element)
        {
	        element = default(SciterElement);
            return window?.TryGetRootElementInternal(out element) == true;
        }
        
        /// <summary>
		/// Find element at point x/y of the window, client area relative
		/// </summary>
		public static SciterElement GetElementAtPoint(this SciterWindow window, int x, int y)
        {
	        return window?.GetElementAtPointInternal(x: x, y: y);
        }

		/// <summary>
		/// Find element at point x/y of the window, client area relative
		/// </summary>
		public static bool TryGetElementAtPoint(this SciterWindow window, out SciterElement element, int x, int y)
		{
			element = default(SciterElement);
			return window?.TryGetElementAtPointInternal(value: out element, x: x, y: y) == true;
		}

		/// <summary>
		/// Find element at the <see cref="SciterPoint"/> of the window, client area relative
		/// </summary>
		public static bool TryGetElementAtPoint(this SciterWindow window, out SciterElement element, SciterPoint point)
		{
			element = default(SciterElement);
			return window?.TryGetElementAtPointInternal(value: out element, point: point) == true;
		}

		/// <summary>
		/// Find element at the <see cref="SciterPoint"/> of the window, client area relative
		/// </summary>
		public static SciterElement GetElementAtPoint(this SciterWindow window, SciterPoint point)
		{
			return window?.GetElementAtPointInternal(point: point);
		}

		/// <summary>
		/// Searches this window DOM tree for element with the given UID
		/// </summary>
		/// <returns>The element, or null if it doesn't exists</returns>
		public static SciterElement GetElementByUid(this SciterWindow window, uint uid)
		{
			return window?.GetElementByUidInternal(uid: uid);
		}

		/// <summary>
		/// Searches this window DOM tree for element with the given UID
		/// </summary>
		/// <returns>The element, or null if it doesn't exists</returns>
		public static bool TryGetElementByUid(this SciterWindow window, out SciterElement element, uint uid)
		{
			element = default(SciterElement);
			return window?.TryGetElementByUidInternal(value: out element, uid: uid) == true;
		}

        #endregion

        #region Dimensions

        public static int GetMinWidth(this SciterWindow window)
        {
	        return window?.GetMinWidthInternal() ?? default(int);
        }

        public static int GetMinHeight(this SciterWindow window, int width)
        {
	        return window?.GetMinHeightInternal(width) ?? default(int);
        }

        #endregion

        #region 

        public static SciterWindow UpdateWindow(this SciterWindow window)
        {
	        window?.UpdateWindowInternal();
	        return window;
        }

        public static bool TryUpdateWindow(this SciterWindow window)
        {
	        return window?.TryUpdateWindowInternal() == true;
        }

        #endregion
    }
}