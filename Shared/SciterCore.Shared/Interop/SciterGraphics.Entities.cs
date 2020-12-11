using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo

namespace SciterCore.Interop
{
    public static partial class SciterGraphics
    {
        // ReSharper disable InconsistentNaming
		[StructLayout(LayoutKind.Sequential)]
		public struct COLOR_STOP
		{
			uint color;
			
			/// <summary>
			/// 0.0 ... 1.0
			/// </summary>
			float offset;
		}

		public enum GRAPHIN_RESULT
		{
			/// <summary>
			/// e.g. Not enough memory
			/// </summary>
			GRAPHIN_PANIC = -1,	
			
			GRAPHIN_OK = 0,
			
			/// <summary>
			/// Bad parameter
			/// </summary>
			GRAPHIN_BAD_PARAM = 1,
			
			/// <summary>
			/// Operation failed. <br/>
			/// e.g. restore() without save()
			/// </summary>
			GRAPHIN_FAILURE = 2,
			
			/// <summary>
			/// The platform does not support requested feature
			/// </summary>
			GRAPHIN_NOTSUPPORTED = 3
		}

		public enum DRAW_PATH_MODE
		{
			DRAW_FILL_ONLY = 1,
			
			DRAW_STROKE_ONLY = 2,
			
			DRAW_FILL_AND_STROKE = 3,
		}

		public enum SCITER_LINE_JOIN_TYPE
		{
			SCITER_JOIN_MITER = 0,
			
			SCITER_JOIN_ROUND = 1,
			
			SCITER_JOIN_BEVEL = 2,
			
			SCITER_JOIN_MITER_OR_BEVEL = 3,
		}

		public enum SCITER_LINE_CAP_TYPE
		{
			SCITER_LINE_CAP_BUTT = 0,
			
			SCITER_LINE_CAP_SQUARE = 1,
			
			SCITER_LINE_CAP_ROUND = 2,
		}

		/*public enum SCITER_TEXT_ALIGNMENT
		{
			TEXT_ALIGN_DEFAULT,
			
			TEXT_ALIGN_START,
			
			TEXT_ALIGN_END,
			
			TEXT_ALIGN_CENTER,
		}

		public enum SCITER_TEXT_DIRECTION
		{
			TEXT_DIRECTION_DEFAULT,
			
			TEXT_DIRECTION_LTR,
			
			TEXT_DIRECTION_RTL,
			
			TEXT_DIRECTION_TTB,
		}*/

		public enum SCITER_IMAGE_ENCODING
		{
			/// <summary>
			/// [a,b,g,r,a,b,g,r,...] vector
			/// </summary>
			SCITER_IMAGE_ENCODING_RAW,
			
			SCITER_IMAGE_ENCODING_PNG,
			
			SCITER_IMAGE_ENCODING_JPG,
			
			SCITER_IMAGE_ENCODING_WEBP,
		}

		/*[StructLayout(LayoutKind.Sequential)]
		public struct SCITER_TEXT_FORMAT
		{
			/// <summary>
			/// 
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string fontFamily;
			
			/// <summary>
			/// 100...900, 400 - normal, 700 - bold
			/// </summary>
			public uint fontWeight;
			
			/// <summary>
			/// 
			/// </summary>
			public bool fontItalic;
			
			/// <summary>
			/// dips
			/// </summary>
			public float fontSize;
			
			/// <summary>
			/// dips
			/// </summary>
			public float lineHeight;
			
			/// <summary>
			/// 
			/// </summary>
			public SCITER_TEXT_DIRECTION textDirection;
			
			/// <summary>
			/// Horizontal alignment
			/// </summary>
			public SCITER_TEXT_ALIGNMENT textAlignment;
			
			/// <summary>
			/// a.k.a. Vertical alignment for roman writing systems
			/// </summary>
			public SCITER_TEXT_ALIGNMENT lineAlignment;
			
			/// <summary>
			/// 
			/// </summary>
			[MarshalAs(UnmanagedType.LPWStr)]
			public string localeName;
		}*/
		
		// ReSharper restore InconsistentNaming
    }
}