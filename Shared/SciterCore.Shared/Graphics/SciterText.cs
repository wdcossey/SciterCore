using System;

namespace SciterCore
{
    public class SciterText
	{
		private static readonly Interop.SciterGraphics.SciterGraphicsApi GraphicsApi = Interop.Sciter.GraphicsApi;

		private readonly IntPtr _textHandle;

		public IntPtr Handle => _textHandle;
		
		// non-user usable
		private SciterText(IntPtr textHandle)
		{
			if(textHandle == IntPtr.Zero)
				throw new ArgumentException($"IntPtr.Zero received at {nameof(SciterText)} constructor.");
			
			_textHandle = textHandle;
		}

		public static SciterText CreateForElement(string text, SciterElement element, string className = null)
		{
			return CreateForElement(text: text, elementHandle: element.Handle, className: className);
		}

		public static SciterText CreateForElement(string text, IntPtr elementHandle, string className = null)
		{
			TryCreateForElement(out var result, text: text, elementHandle: elementHandle, className: className);
			return result;
		}

		public static bool TryCreateForElement(out SciterText sciterText, string text, SciterElement element, string className = null)
		{
			return TryCreateForElement(out sciterText, text: text, elementHandle: element.Handle, className: className);
		}

		public static bool TryCreateForElement(out SciterText sciterText, string text, IntPtr elementHandle, string className = null)
		{
			if (elementHandle == IntPtr.Zero)
				throw new ArgumentOutOfRangeException(nameof(elementHandle), $"IntPtr.Zero received at {nameof(SciterText)} constructor.");
			
			var result = GraphicsApi.textCreateForElement(out var textHandle, text, (uint) text.Length, elementHandle, className)
				.IsOk();
			
			sciterText = result ? new SciterText(textHandle) : default;
			return result;
		}

		/// <summary>
		/// create text layout using explicit style declaration
		/// </summary>
		/// <param name="text"></param>
		/// <param name="elementHandle"></param>
		/// <param name="style"></param>
		/// <returns></returns>
		public static SciterText CreateForElementAndStyle(string text, IntPtr elementHandle, string style)
		{
			TryCreateForElementAndStyle(sciterText: out var result, text: text, elementHandle: elementHandle,
				style: style);
			return result;
		}

		/// <summary>
		/// create text layout using explicit style declaration
		/// </summary>
		/// <param name="sciterText"></param>
		/// <param name="text"></param>
		/// <param name="elementHandle"></param>
		/// <param name="style"></param>
		/// <returns></returns>
		public static bool TryCreateForElementAndStyle(out SciterText sciterText, string text, IntPtr elementHandle, string style)
		{
			var result = GraphicsApi.textCreateForElementAndStyle(out var textHandle, text, (uint)text.Length, elementHandle, style, (uint) style.Length)
				.IsOk();
			
			sciterText = result ? new SciterText(textHandle) : default;
			return result;
		}
		
		public static SciterText FromValue(SciterValue sciterValue)
		{
			TryFromValue(sciterText: out var result, sciterValue: sciterValue);
			return result;
		}
		
		public static bool TryFromValue(out SciterText sciterText, SciterValue sciterValue)
		{
			var value = sciterValue.ToVALUE();
			var result = GraphicsApi.vUnWrapText(ref value, out var textHandle)
				.IsOk();

			sciterText = result ? new SciterText(textHandle) : default;
			return result;
		}

		internal SciterValue ToValueInternal()
		{
			TryToValueInternal(out var result);
			return result;
		}

		internal bool TryToValueInternal(out SciterValue sciterValue)
		{
			var result = GraphicsApi.vWrapText(this.Handle, out var value)
				.IsOk();
			
			sciterValue = result ? new SciterValue(value) : default;
			return result;
		}

		public TextMetrics Metrics => GetMetricsInternal();

		internal TextMetrics GetMetricsInternal()
		{
			TryGetMetricsInternal(out var result);
			return result;
		}

		internal bool TryGetMetricsInternal(out TextMetrics textMetrics)
		{
			var result = GraphicsApi.textGetMetrics(this.Handle, out var minWidth, out var maxWidth, out var height,
					out var ascent, out var descent, out var lines)
				.IsOk();

			textMetrics = result ? TextMetrics.Create(minWidth: minWidth, maxWidth: maxWidth, height: height, ascent: ascent, descent: descent, noLines: Convert.ToInt32(lines)) : default;
			return result;
		}
		
		internal void SetBoxInternal(float width, float height)
		{
			TrySetBoxInternal(width: width, height: height);
		}
		
		internal bool TrySetBoxInternal(float width, float height)
		{
			return GraphicsApi.textSetBox(this.Handle, width, height)
				.IsOk();
		}
	}
}