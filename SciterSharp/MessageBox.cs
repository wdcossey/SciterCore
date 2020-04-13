using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SciterCore.Interop;
#if OSX && XAMARIN
using AppKit;
using Foundation;
#endif

namespace SciterCore
{
	public static class MessageBox
	{
		public static void Show(IntPtr owner, string text, string caption)
		{
#if WINDOWS || NETCORE
			PInvokeWindows.MessageBox(owner, text, caption, PInvokeWindows.MessageBoxOptions.OkOnly | PInvokeWindows.MessageBoxOptions.IconExclamation);
#elif OSX && XAMARIN
			NSAlert alert = new NSAlert();
			alert.MessageText = text;
			alert.RunModal();
#elif GTKMONO
			throw new Exception("MessageBox.Show not implemented in GTK");
#endif
		}

		/// <summary>
		/// Show a system message-box owned by this Sciter window. If caption is null, it will be the title of the Sciter window
		/// </summary>
		/// <param name="window"></param>
		/// <param name="text"></param>
		/// <param name="caption"></param>
		public static void ShowMessageBox(this SciterWindow window, string text, string caption = null)
		{
			Show(owner: window.Hwnd, text: text, caption: caption ?? window.Title);
		}
	}
}