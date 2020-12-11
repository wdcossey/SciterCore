using System;
using System.Runtime.InteropServices;

namespace SciterCore.Interop
{
	public static partial class Sciter
	{
		internal static class SciterApiDelegates
		{
			
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			public delegate void VoidReserved();
			
			/// <summary>
			/// LPCWSTR function() SciterClassName;
			/// Use Marshal.PtrToStringUni(returned IntPtr) to get the actual string
			/// </summary>
			[SciterStructMap(nameof(WindowsSciterApi.SciterClassName))]
			public delegate IntPtr SciterClassName();

			// UINT function(BOOL major) SciterVersion;
			[SciterStructMap(nameof(WindowsSciterApi.SciterVersion))]
			public delegate uint SciterVersion(bool major);

			/// <summary> 
			/// BOOL function(HWINDOW hwnd, LPCWSTR uri, LPCBYTE data, UINT dataLength) SciterDataReady;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="uri"></param>
			/// <param name="data"></param>
			/// <param name="dataLength"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterDataReady))]
			public delegate bool SciterDataReady(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string uri,
				byte[] data, uint dataLength);

			/// <summary>
			/// BOOL function(HWINDOW hwnd, LPCWSTR uri, LPCBYTE data, UINT dataLength, LPVOID requestId) SciterDataReadyAsync;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="uri"></param>
			/// <param name="data"></param>
			/// <param name="dataLength"></param>
			/// <param name="requestId"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterDataReadyAsync))]
			public delegate bool SciterDataReadyAsync(IntPtr hwnd, string uri, byte[] data, uint dataLength,
				IntPtr requestId);

			/// <summary>
			/// LRESULT function(HWINDOW hwnd, UINT msg, WPARAM wParam, LPARAM lParam) SciterProc;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="msg"></param>
			/// <param name="wParam"></param>
			/// <param name="lParam"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterProc))]
			public delegate IntPtr SciterProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

			/// <summary>
			/// LRESULT function(HWINDOW hwnd, UINT msg, WPARAM wParam, LPARAM lParam, BOOL* pbHandled) SciterProcND;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="msg"></param>
			/// <param name="wParam"></param>
			/// <param name="lParam"></param>
			/// <param name="pbHandled"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterProcND))]
			public delegate IntPtr SciterProcNd(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam,
				ref bool pbHandled);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, LPCWSTR filename) SciterLoadFile;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="filename"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterLoadFile))]
			public delegate bool SciterLoadFile(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string filename);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, LPCBYTE html, UINT htmlSize, LPCWSTR baseUrl) SciterLoadHtml;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="html"></param>
			/// <param name="htmlSize"></param>
			/// <param name="baseUrl"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterLoadHtml))]
			public delegate bool SciterLoadHtml(IntPtr hwnd, byte[] html, uint htmlSize, string baseUrl);

			/// <summary>
			/// VOID function(HWINDOW hWndSciter, LPSciterHostCallback cb, LPVOID cbParam) SciterSetCallback;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="cb"></param>
			/// <param name="param"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetCallback))]
			public delegate void SciterSetCallback(IntPtr hwnd, MulticastDelegate cb, IntPtr param); // TODO

			/// <summary>
			/// BOOL function(LPCBYTE utf8, UINT numBytes) SciterSetMasterCSS;
			/// </summary>
			/// <param name="utf8"></param>
			/// <param name="numBytes"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetMasterCSS))]
			public delegate bool SciterSetMasterCss(byte[] utf8, uint numBytes);

			/// <summary>
			/// BOOL function(LPCBYTE utf8, UINT numBytes) SciterAppendMasterCSS;
			/// </summary>
			/// <param name="utf8"></param>
			/// <param name="numBytes"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterAppendMasterCSS))]
			public delegate bool SciterAppendMasterCss(byte[] utf8, uint numBytes);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, LPCBYTE utf8, UINT numBytes, LPCWSTR baseUrl, LPCWSTR mediaType) SciterSetCSS;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="utf8"></param>
			/// <param name="numBytes"></param>
			/// <param name="baseUrl"></param>
			/// <param name="mediaType"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetCSS))]
			public delegate bool SciterSetCss(IntPtr hwnd, byte[] utf8, uint numBytes,
				[MarshalAs(UnmanagedType.LPWStr)] string baseUrl, [MarshalAs(UnmanagedType.LPWStr)] string mediaType);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, LPCWSTR mediaType) SciterSetMediaType;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="mediaType"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetMediaType))]
			public delegate bool SciterSetMediaType(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string mediaType);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, const SCITER_VALUE *mediaVars) SciterSetMediaVars;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="mediaVars"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetMediaVars))]
			public delegate bool SciterSetMediaVars(IntPtr hwnd, ref SciterValue.VALUE mediaVars);

			/// <summary>
			/// UINT function(HWINDOW hWndSciter) SciterGetMinWidth;
			/// </summary>
			/// <param name="hwnd"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetMinWidth))]
			public delegate uint SciterGetMinWidth(IntPtr hwnd);

			/// <summary>
			/// UINT function(HWINDOW hWndSciter, UINT width) SciterGetMinHeight;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="width"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetMinHeight))]
			public delegate uint SciterGetMinHeight(IntPtr hwnd, uint width);

			/// <summary>
			/// BOOL function(HWINDOW hWnd, LPCSTR functionName, UINT argc, const SCITER_VALUE* argv, SCITER_VALUE* retval) SciterCall;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="functionName"></param>
			/// <param name="argc"></param>
			/// <param name="argv"></param>
			/// <param name="retval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterCall))]
			public delegate bool SciterCall(IntPtr hwnd, [MarshalAs(UnmanagedType.LPStr)] string functionName,
				uint argc, SciterValue.VALUE[] argv, out SciterValue.VALUE retval);

			/// <summary>
			/// BOOL function(HWINDOW hwnd, LPCWSTR script, UINT scriptLength, SCITER_VALUE* pretval) SciterEval;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="script"></param>
			/// <param name="scriptLength"></param>
			/// <param name="pretval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterEval))]
			public delegate bool SciterEval(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string script,
				uint scriptLength, out SciterValue.VALUE pretval);

			/// <summary>
			/// VOID function(HWINDOW hwnd) SciterUpdateWindow;
			/// </summary>
			/// <param name="hwnd"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterUpdateWindow))]
			public delegate bool SciterUpdateWindow(IntPtr hwnd);

			/// <summary>
			/// BOOL function(MSG* lpMsg) SciterTranslateMessage;
			/// </summary>
			/// <param name="lpMsg"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterTranslateMessage))]
			public delegate bool SciterTranslateMessage(IntPtr lpMsg); // TODO: MSG

			/// <summary>
			/// BOOL function(HWINDOW hWnd, UINT option, UINT_PTR value ) SciterSetOption;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="option"></param>
			/// <param name="value"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetOption))]
			public delegate bool SciterSetOption(IntPtr hwnd, SciterXDef.SCITER_RT_OPTIONS option, IntPtr value);

			/// <summary>
			/// VOID function(HWINDOW hWndSciter, UINT* px, UINT* py) SciterGetPPI;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="px"></param>
			/// <param name="py"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetPPI))]
			public delegate void SciterGetPpi(IntPtr hwnd, ref uint px, ref uint py);

			/// <summary>
			/// BOOL function(HWINDOW hwnd, VALUE* pval) SciterGetViewExpando;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetViewExpando))]
			public delegate bool SciterGetViewExpando(IntPtr hwnd, out SciterValue.VALUE pval);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, ID2D1RenderTarget* prt) SciterRenderD2D;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="prt"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterRenderD2D))]
			public delegate bool SciterRenderD2D(IntPtr hwnd, IntPtr prt); // TODO

			/// <summary>
			/// BOOL function(ID2D1Factory ** ppf) SciterD2DFactory;
			/// </summary>
			/// <param name="ppf"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterD2DFactory))]
			public delegate bool SciterD2DFactory(IntPtr ppf); // TODO

			/// <summary>
			/// BOOL function(IDWriteFactory ** ppf) SciterDWFactory;
			/// </summary>
			/// <param name="ppf"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterDWFactory))]
			public delegate bool SciterDwFactory(IntPtr ppf); // TODO

			/// <summary>
			/// BOOL function(LPUINT pcaps) SciterGraphicsCaps;
			/// </summary>
			/// <param name="pcaps"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGraphicsCaps))]
			public delegate bool SciterGraphicsCaps(ref uint pcaps);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, LPCWSTR baseUrl) SciterSetHomeURL;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="baseUrl"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetHomeURL))]
			public delegate bool SciterSetHomeUrl(IntPtr hwnd, string baseUrl);

			/// <summary>
			/// HWINDOW function( LPRECT frame ) SciterCreateNSView;// returns NSView*
			/// </summary>
			/// <param name="frame"></param>
			[SciterStructMap(nameof(MacOsSciterApi.SciterCreateNSView))]
			public delegate IntPtr SciterCreateNsView(ref PInvokeUtils.RECT frame);

			/// <summary>
			/// HWINDOW SCFN( SciterCreateWidget )( LPRECT frame ); // returns GtkWidget
			/// </summary>
			/// <param name="frame"></param>
			[SciterStructMap(nameof(LinuxSciterApi.SciterCreateWidget))]
			public delegate IntPtr SciterCreateWidget(ref PInvokeUtils.RECT frame);

			/// <summary>
			/// HWINDOW function(UINT creationFlags, LPRECT frame, SciterWindowDelegate* delegt, LPVOID delegateParam, HWINDOW parent) SciterCreateWindow;
			/// </summary>
			/// <param name="creationFlags"></param>
			/// <param name="frame"></param>
			/// <param name="delegt"></param>
			/// <param name="delegateParam"></param>
			/// <param name="parent"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterCreateWindow))]
			public delegate IntPtr SciterCreateWindow(SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags,
				ref PInvokeUtils.RECT frame, MulticastDelegate delegt, IntPtr delegateParam, IntPtr parent);

			/// <summary>
			/// VOID function(HWINDOW hwndOrNull, LPVOID param, DEBUG_OUTPUT_PROC     pfOutput) SciterSetupDebugOutput;
			/// </summary>
			/// <param name="hwndOrNull">HWINDOW or null if this is global output handler</param>
			/// <param name="param">param to be passed "as is" to the pfOutput</param>
			/// <param name="pfOutput">output function, output stream alike thing.</param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetupDebugOutput))]
			public delegate void SciterSetupDebugOutput(IntPtr hwndOrNull, IntPtr param,
				SciterXDef.DEBUG_OUTPUT_PROC pfOutput);

			#region DOM Element API

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) Sciter_UseElement;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(WindowsSciterApi.Sciter_UseElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterUseElement(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) Sciter_UnuseElement;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(WindowsSciterApi.Sciter_UnuseElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterUnuseElement(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HWINDOW hwnd, HELEMENT *phe) SciterGetRootElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetRootElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetRootElement(IntPtr hwnd, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HWINDOW hwnd, HELEMENT *phe) SciterGetFocusElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetFocusElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetFocusElement(IntPtr hwnd, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HWINDOW hwnd, POINT pt, HELEMENT* phe) SciterFindElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="pt"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterFindElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterFindElement(IntPtr hwnd, PInvokeUtils.POINT pt,
				out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT* count) SciterGetChildrenCount;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="count"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetChildrenCount))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetChildrenCount(IntPtr he, out uint count);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT n, HELEMENT* phe) SciterGetNthChild;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="n"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetNthChild))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetNthChild(IntPtr he, uint n, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, HELEMENT* p_parent_he) SciterGetParentElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pParentHe"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetParentElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetParentElement(IntPtr he, out IntPtr pParentHe);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, BOOL outer, LPCBYTE_RECEIVER rcv, LPVOID rcv_param) SciterGetElementHtmlCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="outer"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementHtmlCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementHtmlCb(IntPtr he, bool outer,
				SciterXDom.LPCBYTE_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetElementTextCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementTextCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementTextCb(IntPtr he,
				SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR utf16, UINT length) SciterSetElementText;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="utf16"></param>
			/// <param name="length"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetElementText))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetElementText(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string utf16, uint length);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPUINT p_count) SciterGetAttributeCount;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pCount"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetAttributeCount))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetAttributeCount(IntPtr he, out uint pCount);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT n, LPCSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetNthAttributeNameCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="n"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetNthAttributeNameCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetNthAttributeNameCb(IntPtr he, uint n,
				SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT n, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetNthAttributeValueCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="n"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetNthAttributeValueCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetNthAttributeValueCb(IntPtr he, uint n,
				SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetAttributeByNameCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetAttributeByNameCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetAttributeByNameCb(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR value) SciterSetAttributeByName;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="value"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetAttributeByName))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetAttributeByName(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterClearAttributes;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterClearAttributes))]
			public delegate SciterXDom.SCDOM_RESULT SciterClearAttributes(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPUINT p_index) SciterGetElementIndex;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pIndex"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementIndex))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementIndex(IntPtr he, out uint pIndex);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR* p_type) SciterGetElementType;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pType"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementType))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementType(IntPtr he, out IntPtr pType);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetElementTypeCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementTypeCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementTypeCb(IntPtr he,
				SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetStyleAttributeCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetStyleAttributeCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetStyleAttributeCb(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR value) SciterSetStyleAttribute;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="value"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetStyleAttribute))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetStyleAttribute(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPRECT p_location, UINT areas /*ELEMENT_AREAS*/) SciterGetElementLocation;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pLocation"></param>
			/// <param name="areas"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementLocation))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementLocation(IntPtr he,
				out PInvokeUtils.RECT pLocation, SciterXDom.ELEMENT_AREAS areas);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT SciterScrollFlags) SciterScrollToView;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="sciterScrollFlags"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterScrollToView))]
			public delegate SciterXDom.SCDOM_RESULT SciterScrollToView(IntPtr he, uint sciterScrollFlags);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, BOOL andForceRender) SciterUpdateElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="andForceRender"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterUpdateElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterUpdateElement(IntPtr he, bool andForceRender);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, RECT rc) SciterRefreshElementArea;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="rc"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterRefreshElementArea))]
			public delegate SciterXDom.SCDOM_RESULT SciterRefreshElementArea(IntPtr he, PInvokeUtils.RECT rc);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterSetCapture;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetCapture))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetCapture(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterReleaseCapture;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterReleaseCapture))]
			public delegate SciterXDom.SCDOM_RESULT SciterReleaseCapture(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, HWINDOW* p_hwnd, BOOL rootWindow) SciterGetElementHwnd;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pHwnd"></param>
			/// <param name="rootWindow"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementHwnd))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementHwnd(IntPtr he, out IntPtr pHwnd,
				bool rootWindow);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPWSTR szUrlBuffer, UINT UrlBufferSize) SciterCombineURL;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="szUrlBuffer"></param>
			/// <param name="urlBufferSize"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterCombineURL))]
			public delegate SciterXDom.SCDOM_RESULT SciterCombineUrl(IntPtr he, /*[MarshalAs(UnmanagedType.LPWStr)]*/
				IntPtr szUrlBuffer, uint urlBufferSize);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT  he, LPCSTR    CSS_selectors, SciterElementCallback callback, LPVOID param) SciterSelectElements;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="cssSelectors"></param>
			/// <param name="callback"></param>
			/// <param name="param"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSelectElements))]
			public delegate SciterXDom.SCDOM_RESULT SciterSelectElements(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string cssSelectors, SciterXDom.SCITER_ELEMENT_CALLBACK callback,
				IntPtr param);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT  he, LPCWSTR   CSS_selectors, SciterElementCallback callback, LPVOID param) SciterSelectElementsW;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="cssSelectors"></param>
			/// <param name="callback"></param>
			/// <param name="param"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSelectElementsW))]
			public delegate SciterXDom.SCDOM_RESULT SciterSelectElementsW(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string cssSelectors, SciterXDom.SCITER_ELEMENT_CALLBACK callback,
				IntPtr param);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT  he, LPCSTR    selector, UINT      depth, HELEMENT* heFound) SciterSelectParent;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="selector"></param>
			/// <param name="depth"></param>
			/// <param name="heFound"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSelectParent))]
			public delegate SciterXDom.SCDOM_RESULT SciterSelectParent(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string selector, uint depth, out IntPtr heFound);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR selector, UINT depth, HELEMENT* heFound) SciterSelectParentW;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="selector"></param>
			/// <param name="depth"></param>
			/// <param name="heFound"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSelectParentW))]
			public delegate SciterXDom.SCDOM_RESULT SciterSelectParentW(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string selector, uint depth, out IntPtr heFound);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, const BYTE* html, UINT htmlLength, UINT where) SciterSetElementHtml;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="html"></param>
			/// <param name="htmlLength"></param>
			/// <param name="where"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetElementHtml))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetElementHtml(IntPtr he, byte[] html, uint htmlLength,
				SciterXDom.SET_ELEMENT_HTML where);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT* puid) SciterGetElementUID;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="puid"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementUID))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementUid(IntPtr he, out uint puid);

			/// <summary>
			/// SCDOM_RESULT function(HWINDOW hwnd, UINT uid, HELEMENT* phe) SciterGetElementByUID;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="uid"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementByUID))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementByUid(IntPtr hwnd, uint uid, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT hePopup, HELEMENT heAnchor, UINT placement) SciterShowPopup;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="heAnchor"></param>
			/// <param name="placement"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterShowPopup))]
			public delegate SciterXDom.SCDOM_RESULT SciterShowPopup(IntPtr he, IntPtr heAnchor, uint placement);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT hePopup, POINT pos, UINT placement) SciterShowPopupAt;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pos"></param>
			/// <param name="placement"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterShowPopupAt))]
			public delegate SciterXDom.SCDOM_RESULT SciterShowPopupAt(IntPtr he, PInvokeUtils.POINT pos,
				uint placement);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterHidePopup;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterHidePopup))]
			public delegate SciterXDom.SCDOM_RESULT SciterHidePopup(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT* pstateBits) SciterGetElementState;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pstateBits"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementState))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementState(IntPtr he, out uint pstateBits);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT stateBitsToSet, UINT stateBitsToClear, BOOL updateView) SciterSetElementState;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="stateBitsToSet"></param>
			/// <param name="stateBitsToClear"></param>
			/// <param name="updateView"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetElementState))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetElementState(IntPtr he, uint stateBitsToSet,
				uint stateBitsToClear, bool updateView);

			/// <summary>
			/// SCDOM_RESULT function( LPCSTR tagname, LPCWSTR textOrNull, /*out*/ HELEMENT *phe ) SciterCreateElement;
			/// </summary>
			/// <param name="tagname"></param>
			/// <param name="textOrNull"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterCreateElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterCreateElement(
				[MarshalAs(UnmanagedType.LPStr)] string tagname, [MarshalAs(UnmanagedType.LPWStr)] string textOrNull,
				out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, /*out*/ HELEMENT *phe ) SciterCloneElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterCloneElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterCloneElement(IntPtr he, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, HELEMENT hparent, UINT index ) SciterInsertElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="hparent"></param>
			/// <param name="index"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterInsertElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterInsertElement(IntPtr he, IntPtr hparent, uint index);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he ) SciterDetachElement;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterDetachElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterDetachElement(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterDeleteElement;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterDeleteElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterDeleteElement(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT milliseconds, UINT_PTR timer_id ) SciterSetTimer;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="milliseconds"></param>
			/// <param name="timerId"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetTimer))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetTimer(IntPtr he, uint milliseconds, IntPtr timerId);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPELEMENT_EVENT_PROC pep, LPVOID tag ) SciterDetachEventHandler;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pep"></param>
			/// <param name="tag"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterDetachEventHandler))]
			public delegate SciterXDom.SCDOM_RESULT SciterDetachEventHandler(IntPtr he, MulticastDelegate pep,
				IntPtr tag);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPELEMENT_EVENT_PROC pep, LPVOID tag ) SciterAttachEventHandler;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pep"></param>
			/// <param name="tag"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterAttachEventHandler))]
			public delegate SciterXDom.SCDOM_RESULT SciterAttachEventHandler(IntPtr he, MulticastDelegate pep,
				IntPtr tag);

			/// <summary>
			/// SCDOM_RESULT function( HWINDOW hwndLayout, LPELEMENT_EVENT_PROC pep, LPVOID tag, UINT subscription ) SciterWindowAttachEventHandler;
			/// </summary>
			/// <param name="hwndLayout"></param>
			/// <param name="pep"></param>
			/// <param name="tag"></param>
			/// <param name="subscription"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterWindowAttachEventHandler))]
			public delegate SciterXDom.SCDOM_RESULT SciterWindowAttachEventHandler(IntPtr hwndLayout,
				MulticastDelegate pep, IntPtr tag, uint subscription);

			/// <summary>
			/// SCDOM_RESULT function( HWINDOW hwndLayout, LPELEMENT_EVENT_PROC pep, LPVOID tag ) SciterWindowDetachEventHandler;
			/// </summary>
			/// <param name="hwndLayout"></param>
			/// <param name="pep"></param>
			/// <param name="tag"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterWindowDetachEventHandler))]
			public delegate SciterXDom.SCDOM_RESULT SciterWindowDetachEventHandler(IntPtr hwndLayout,
				MulticastDelegate pep, IntPtr tag);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT appEventCode, HELEMENT heSource, UINT_PTR reason, /*out*/ BOOL* handled) SciterSendEvent;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="appEventCode"></param>
			/// <param name="heSource"></param>
			/// <param name="reason"></param>
			/// <param name="handled"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSendEvent))]
			public delegate SciterXDom.SCDOM_RESULT SciterSendEvent(IntPtr he, uint appEventCode, IntPtr heSource,
				IntPtr reason, out bool handled);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT appEventCode, HELEMENT heSource, UINT_PTR reason) SciterPostEvent;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="appEventCode"></param>
			/// <param name="heSource"></param>
			/// <param name="reason"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterPostEvent))]
			public delegate SciterXDom.SCDOM_RESULT SciterPostEvent(IntPtr he, uint appEventCode, IntPtr heSource,
				IntPtr reason);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, METHOD_PARAMS* params) SciterCallBehaviorMethod;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="param"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterCallBehaviorMethod))]
			public delegate SciterXDom.SCDOM_RESULT SciterCallBehaviorMethod(IntPtr he,
				ref SciterXDom.METHOD_PARAMS param);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR url, UINT dataType, HELEMENT initiator) SciterRequestElementData;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="url"></param>
			/// <param name="dataType"></param>
			/// <param name="initiator"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterRequestElementData))]
			public delegate SciterXDom.SCDOM_RESULT SciterRequestElementData(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string url, uint dataType, IntPtr initiator);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR url, UINT dataType, UINT requestType, REQUEST_PARAM* requestParams, UINT nParams) SciterHttpRequest;
			/// </summary>
			/// <param name="he">element to deliver data</param>
			/// <param name="url">url</param>
			/// <param name="dataType">data type, see SciterResourceType.</param>
			/// <param name="requestType">one of REQUEST_TYPE values</param>
			/// <param name="requestParams">parameters</param>
			/// <param name="nParams">number of parameters </param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterHttpRequest))]
			public delegate SciterXDom.SCDOM_RESULT SciterHttpRequest(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string url, uint dataType, uint requestType,
				ref SciterXDom.REQUEST_PARAM requestParams, uint nParams);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPPOINT scrollPos, LPRECT viewRect, LPSIZE contentSize ) SciterGetScrollInfo;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="scrollPos"></param>
			/// <param name="viewRect"></param>
			/// <param name="contentSize"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetScrollInfo))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetScrollInfo(IntPtr he, out PInvokeUtils.POINT scrollPos,
				out PInvokeUtils.RECT viewRect, out PInvokeUtils.SIZE contentSize);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, POINT scrollPos, BOOL smooth ) SciterSetScrollPos;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="scrollPos"></param>
			/// <param name="smooth"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetScrollPos))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetScrollPos(IntPtr he, PInvokeUtils.POINT scrollPos,
				bool smooth);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, INT* pMinWidth, INT* pMaxWidth ) SciterGetElementIntrinsicWidths;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pMinWidth"></param>
			/// <param name="pMaxWidth"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementIntrinsicWidths))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementIntrinsicWidths(IntPtr he, out int pMinWidth,
				out int pMaxWidth);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, INT forWidth, INT* pHeight ) SciterGetElementIntrinsicHeight;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="forWidth"></param>
			/// <param name="pHeight"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementIntrinsicHeight))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementIntrinsicHeight(IntPtr he, int forWidth,
				out int pHeight);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, BOOL* pVisible) SciterIsElementVisible;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pVisible"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterIsElementVisible))]
			public delegate SciterXDom.SCDOM_RESULT SciterIsElementVisible(IntPtr he, out bool pVisible);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, BOOL* pEnabled ) SciterIsElementEnabled;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pEnabled"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterIsElementEnabled))]
			public delegate SciterXDom.SCDOM_RESULT SciterIsElementEnabled(IntPtr he, out bool pEnabled);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT firstIndex, UINT lastIndex, ELEMENT_COMPARATOR* cmpFunc, LPVOID cmpFuncParam ) SciterSortElements;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="firstIndex"></param>
			/// <param name="lastIndex"></param>
			/// <param name="cmpFunc"></param>
			/// <param name="cmpFuncParam"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSortElements))]
			public delegate SciterXDom.SCDOM_RESULT SciterSortElements(IntPtr he, uint firstIndex, uint lastIndex,
				SciterXDom.ELEMENT_COMPARATOR cmpFunc, IntPtr cmpFuncParam);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he1, HELEMENT he2 ) SciterSwapElements;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="he2"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSwapElements))]
			public delegate SciterXDom.SCDOM_RESULT SciterSwapElements(IntPtr he, IntPtr he2);

			/// <summary>
			/// SCDOM_RESULT function( UINT evt, LPVOID eventCtlStruct, BOOL* bOutProcessed ) SciterTraverseUIEvent;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="eventCtlStruct"></param>
			/// <param name="bOutProcessed"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterTraverseUIEvent))]
			public delegate SciterXDom.SCDOM_RESULT SciterTraverseUiEvent(IntPtr he, IntPtr eventCtlStruct,
				out bool bOutProcessed);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPCSTR name, const VALUE* argv, UINT argc, VALUE* retval ) SciterCallScriptingMethod;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="argv"></param>
			/// <param name="argc"></param>
			/// <param name="retval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterCallScriptingMethod))]
			public delegate SciterXDom.SCDOM_RESULT SciterCallScriptingMethod(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, SciterValue.VALUE[] argv, uint argc,
				out SciterValue.VALUE retval);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPCSTR name, const VALUE* argv, UINT argc, VALUE* retval ) SciterCallScriptingFunction;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="argv"></param>
			/// <param name="argc"></param>
			/// <param name="retval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterCallScriptingFunction))]
			public delegate SciterXDom.SCDOM_RESULT SciterCallScriptingFunction(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, SciterValue.VALUE[] argv, uint argc,
				out SciterValue.VALUE retval);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPCWSTR script, UINT scriptLength, VALUE* retval ) SciterEvalElementScript;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="script"></param>
			/// <param name="scriptLength"></param>
			/// <param name="retval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterEvalElementScript))]
			public delegate SciterXDom.SCDOM_RESULT SciterEvalElementScript(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string script, uint scriptLength, out SciterValue.VALUE retval);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, HWINDOW hwnd) SciterAttachHwndToElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="hwnd"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterAttachHwndToElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterAttachHwndToElement(IntPtr he, IntPtr hwnd);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, /*CTL_TYPE*/ UINT *pType ) SciterControlGetType;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pType"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterControlGetType))]
			public delegate SciterXDom.SCDOM_RESULT SciterControlGetType(IntPtr he, out uint pType);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, VALUE* pval ) SciterGetValue;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetValue))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetValue(IntPtr he, out SciterValue.VALUE pval);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, const VALUE* pval ) SciterSetValue;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetValue))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetValue(IntPtr he, ref SciterValue.VALUE pval);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, VALUE* pval, BOOL forceCreation ) SciterGetExpando;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			/// <param name="forceCreation"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetExpando))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetExpando(IntPtr he, out SciterValue.VALUE pval,
				bool forceCreation);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, tiscript_value* pval, BOOL forceCreation ) SciterGetObject;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			/// <param name="forceCreation"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetObject))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetObject(IntPtr he, out IntPtr pval,
				bool forceCreation);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, tiscript_value* pval) SciterGetElementNamespace;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetElementNamespace))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementNamespace(IntPtr he,
				out IntPtr pval);

			/// <summary>
			/// SCDOM_RESULT function( HWINDOW hwnd, HELEMENT* phe) SciterGetHighlightedElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetHighlightedElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetHighlightedElement(IntPtr hwnd, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function( HWINDOW hwnd, HELEMENT he) SciterSetHighlightedElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="he"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterSetHighlightedElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetHighlightedElement(IntPtr hwnd, IntPtr he);

			#endregion

			#region DOM Node API

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn) SciterNodeAddRef;
			/// </summary>
			/// <param name="hn"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeAddRef))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeAddRef(IntPtr hn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn) SciterNodeRelease;
			/// </summary>
			/// <param name="hn"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeRelease))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeRelease(IntPtr hn);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, HNODE* phn) SciterNodeCastFromElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="phn"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeCastFromElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeCastFromElement(IntPtr he, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HELEMENT* he) SciterNodeCastToElement;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="he"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeCastToElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeCastToElement(IntPtr hn, out IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodeFirstChild;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="phn"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeFirstChild))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeFirstChild(IntPtr hn, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodeLastChild;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="phn"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeLastChild))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeLastChild(IntPtr hn, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodeNextSibling;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="phn"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeNextSibling))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeNextSibling(IntPtr hn, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodePrevSibling;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="phn"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodePrevSibling))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodePrevSibling(IntPtr hn, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, HELEMENT* pheParent) SciterNodeParent;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="pheParent"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeParent))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeParent(IntPtr hn, out IntPtr pheParent);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, UINT n, HNODE* phn) SciterNodeNthChild;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="n"></param>
			/// <param name="phn"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeNthChild))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeNthChild(IntPtr hn, uint n, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, UINT* pn) SciterNodeChildrenCount;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="pn"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeChildrenCount))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeChildrenCount(IntPtr hn, out uint pn);

			/// <summary>
			/// /SCDOM_RESULT function(HNODE hnode, UINT* pNodeType /*NODE_TYPE*/) SciterNodeType;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="pn"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeType))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeType(IntPtr hn, out SciterXDom.NODE_TYPE pn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterNodeGetText;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeGetText))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeGetText(IntPtr hn, SciterXDom.LPCWSTR_RECEIVER rcv,
				IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, LPCWSTR text, UINT textLength) SciterNodeSetText;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="text"></param>
			/// <param name="textLength"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeSetText))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeSetText(IntPtr hn,
				[MarshalAs(UnmanagedType.LPWStr)] string text, uint textLength);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, UINT where /*NODE_INS_TARGET*/, HNODE what) SciterNodeInsert;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="where"></param>
			/// <param name="what"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeInsert))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeInsert(IntPtr hn, uint where, IntPtr what);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, BOOL finalize) SciterNodeRemove;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="finalize"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterNodeRemove))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeRemove(IntPtr hn, bool finalize);

			/// <summary>
			/// SCDOM_RESULT function(LPCWSTR text, UINT textLength, HNODE* phnode) SciterCreateTextNode;
			/// </summary>
			/// <param name="text"></param>
			/// <param name="textLength"></param>
			/// <param name="phnode"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterCreateTextNode))]
			public delegate SciterXDom.SCDOM_RESULT SciterCreateTextNode(
				[MarshalAs(UnmanagedType.LPWStr)] string text, uint textLength, out IntPtr phnode);

			/// <summary>
			/// SCDOM_RESULT function(LPCWSTR text, UINT textLength, HNODE* phnode) SciterCreateCommentNode;
			/// </summary>
			/// <param name="text"></param>
			/// <param name="textLength"></param>
			/// <param name="phnode"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterCreateCommentNode))]
			public delegate SciterXDom.SCDOM_RESULT SciterCreateCommentNode(
				[MarshalAs(UnmanagedType.LPWStr)] string text, uint textLength, out IntPtr phnode);

			#endregion

			#region Value API

			/// <summary>
			/// UINT function( VALUE* pval ) ValueInit;
			/// </summary>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueInit))]
			public delegate SciterValue.VALUE_RESULT ValueInit(out SciterValue.VALUE pval);

			/// <summary>
			/// UINT function( VALUE* pval ) ValueClear;
			/// </summary>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueClear))]
			public delegate SciterValue.VALUE_RESULT ValueClear(out SciterValue.VALUE pval);

			/// <summary>
			/// UINT function( const VALUE* pval1, const VALUE* pval2 ) ValueCompare;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pval2"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueCompare))]
			public delegate SciterValue.VALUE_RESULT ValueCompare(ref SciterValue.VALUE pval, ref IntPtr pval2);

			/// <summary>
			/// UINT function( VALUE* pdst, const VALUE* psrc ) ValueCopy;
			/// </summary>
			/// <param name="pdst"></param>
			/// <param name="psrc"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueCopy))]
			public delegate SciterValue.VALUE_RESULT ValueCopy(out SciterValue.VALUE pdst, ref SciterValue.VALUE psrc);

			/// <summary>
			/// UINT function( VALUE* pdst ) ValueIsolate;
			/// </summary>
			/// <param name="pdst"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueIsolate))]
			public delegate SciterValue.VALUE_RESULT ValueIsolate(ref SciterValue.VALUE pdst);

			/// <summary>
			/// UINT function( const VALUE* pval, UINT* pType, UINT* pUnits ) ValueType;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pType"></param>
			/// <param name="pUnits"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueType))]
			public delegate SciterValue.VALUE_RESULT ValueType(ref SciterValue.VALUE pval, out uint pType,
				out uint pUnits);

			/// <summary>
			/// UINT function( const VALUE* pval, LPCWSTR* pChars, UINT* pNumChars ) ValueStringData;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pChars"></param>
			/// <param name="pNumChars"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueStringData))]
			public delegate SciterValue.VALUE_RESULT ValueStringData(ref SciterValue.VALUE pval, out IntPtr pChars,
				out uint pNumChars);

			/// <summary>
			/// UINT function( VALUE* pval, LPCWSTR chars, UINT numChars, UINT units ) ValueStringDataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="chars"></param>
			/// <param name="numChars"></param>
			/// <param name="units"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueStringDataSet))]
			public delegate SciterValue.VALUE_RESULT ValueStringDataSet(ref SciterValue.VALUE pval,
				[MarshalAs(UnmanagedType.LPWStr)] string chars, uint numChars, uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, INT* pData ) ValueIntData;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pData"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueIntData))]
			public delegate SciterValue.VALUE_RESULT ValueIntData(ref SciterValue.VALUE pval, out int pData);

			/// <summary>
			/// UINT function( VALUE* pval, INT data, UINT type, UINT units ) ValueIntDataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="data"></param>
			/// <param name="type"></param>
			/// <param name="units"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueIntDataSet))]
			public delegate SciterValue.VALUE_RESULT ValueIntDataSet(ref SciterValue.VALUE pval, int data, uint type,
				uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, INT64* pData ) ValueInt64Data;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pData"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueInt64Data))]
			public delegate SciterValue.VALUE_RESULT ValueInt64Data(ref SciterValue.VALUE pval, out long pData);

			/// <summary>
			/// UINT function( VALUE* pval, INT64 data, UINT type, UINT units ) ValueInt64DataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="data"></param>
			/// <param name="type"></param>
			/// <param name="units"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueInt64DataSet))]
			public delegate SciterValue.VALUE_RESULT ValueInt64DataSet(ref SciterValue.VALUE pval, long data,
				uint type, uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, FLOAT_VALUE* pData ) ValueFloatData;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pData"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueFloatData))]
			public delegate SciterValue.VALUE_RESULT ValueFloatData(ref SciterValue.VALUE pval, out double pData);

			/// <summary>
			/// UINT function( VALUE* pval, FLOAT_VALUE data, UINT type, UINT units ) ValueFloatDataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="data"></param>
			/// <param name="type"></param>
			/// <param name="units"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueFloatDataSet))]
			public delegate SciterValue.VALUE_RESULT ValueFloatDataSet(ref SciterValue.VALUE pval, double data,
				uint type, uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, LPCBYTE* pBytes, UINT* pnBytes ) ValueBinaryData;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pBytes"></param>
			/// <param name="pnBytes"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueBinaryData))]
			public delegate SciterValue.VALUE_RESULT ValueBinaryData(ref SciterValue.VALUE pval, out IntPtr pBytes,
				out uint pnBytes);

			/// <summary>
			/// UINT function( VALUE* pval, LPCBYTE pBytes, UINT nBytes, UINT type, UINT units ) ValueBinaryDataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pBytes"></param>
			/// <param name="nBytes"></param>
			/// <param name="type"></param>
			/// <param name="units"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueBinaryDataSet))]
			public delegate SciterValue.VALUE_RESULT ValueBinaryDataSet(ref SciterValue.VALUE pval,
				[MarshalAs(UnmanagedType.LPArray)] byte[] pBytes, uint nBytes, uint type, uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, INT* pn) ValueElementsCount;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pn"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueElementsCount))]
			public delegate SciterValue.VALUE_RESULT ValueElementsCount(ref SciterValue.VALUE pval, out int pn);

			/// <summary>
			/// UINT function( const VALUE* pval, INT n, VALUE* pretval) ValueNthElementValue;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="n"></param>
			/// <param name="pretval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueNthElementValue))]
			public delegate SciterValue.VALUE_RESULT ValueNthElementValue(ref SciterValue.VALUE pval, int n,
				out SciterValue.VALUE pretval);

			/// <summary>
			/// UINT function( VALUE* pval, INT n, const VALUE* pval_to_set) ValueNthElementValueSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="n"></param>
			/// <param name="pvalToSet"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueNthElementValueSet))]
			public delegate SciterValue.VALUE_RESULT ValueNthElementValueSet(ref SciterValue.VALUE pval, int n,
				ref SciterValue.VALUE pvalToSet);

			/// <summary>
			/// UINT function( const VALUE* pval, INT n, VALUE* pretval) ValueNthElementKey;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="n"></param>
			/// <param name="pretval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueNthElementKey))]
            			public delegate SciterValue.VALUE_RESULT ValueNthElementKey(ref SciterValue.VALUE pval, int n,
				out SciterValue.VALUE pretval);

			/// <summary>
			/// UINT function( VALUE* pval, KeyValueCallback* penum, LPVOID param) ValueEnumElements;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="penum"></param>
			/// <param name="param"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueEnumElements))]
			public delegate SciterValue.VALUE_RESULT ValueEnumElements(ref SciterValue.VALUE pval,
				SciterValue.KEY_VALUE_CALLBACK penum, IntPtr param);

			/// <summary>
			/// UINT function( VALUE* pval, const VALUE* pkey, const VALUE* pval_to_set) ValueSetValueToKey;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pkey"></param>
			/// <param name="pvalToSet"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueSetValueToKey))]
			public delegate SciterValue.VALUE_RESULT ValueSetValueToKey(ref SciterValue.VALUE pval,
				ref SciterValue.VALUE pkey, ref SciterValue.VALUE pvalToSet);

			/// <summary>
			/// UINT function( const VALUE* pval, const VALUE* pkey, VALUE* pretval) ValueGetValueOfKey;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pkey"></param>
			/// <param name="pretval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueGetValueOfKey))]
			public delegate SciterValue.VALUE_RESULT ValueGetValueOfKey(ref SciterValue.VALUE pval,
				ref SciterValue.VALUE pkey, out SciterValue.VALUE pretval);

			/// <summary>
			/// UINT function( VALUE* pval, /*VALUE_STRING_CVT_TYPE*/ UINT how ) ValueToString;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="how"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueToString))]
			public delegate SciterValue.VALUE_RESULT ValueToString(ref SciterValue.VALUE pval,
				SciterValue.VALUE_STRING_CVT_TYPE how);

			/// <summary>
			/// UINT function( VALUE* pval, LPCWSTR str, UINT strLength, /*VALUE_STRING_CVT_TYPE*/ UINT how ) ValueFromString;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="str"></param>
			/// <param name="strLength"></param>
			/// <param name="how"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueFromString))]
			public delegate SciterValue.VALUE_RESULT ValueFromString(ref SciterValue.VALUE pval,
				[MarshalAs(UnmanagedType.LPWStr)] string str, uint strLength, uint how);

			/// <summary>
			/// UINT function( VALUE* pval, VALUE* pthis, UINT argc, const VALUE* argv, VALUE* pretval, LPCWSTR url) ValueInvoke;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pthis"></param>
			/// <param name="argc"></param>
			/// <param name="argv"></param>
			/// <param name="pretval"></param>
			/// <param name="url"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueInvoke))]
			public delegate SciterValue.VALUE_RESULT ValueInvoke(ref SciterValue.VALUE pval,
				ref SciterValue.VALUE pthis, uint argc, SciterValue.VALUE[] argv, out SciterValue.VALUE pretval,
				[MarshalAs(UnmanagedType.LPWStr)] string url);

			/// <summary>
			/// UINT function( VALUE* pval, NATIVE_FUNCTOR_INVOKE*  pinvoke, NATIVE_FUNCTOR_RELEASE* prelease, VOID* tag) ValueNativeFunctorSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pinvoke"></param>
			/// <param name="prelease"></param>
			/// <param name="tag"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueNativeFunctorSet))]
			public delegate SciterValue.VALUE_RESULT ValueNativeFunctorSet(ref SciterValue.VALUE pval,
				SciterValue.NATIVE_FUNCTOR_INVOKE pinvoke, SciterValue.NATIVE_FUNCTOR_RELEASE prelease, IntPtr tag);

			/// <summary>
			/// BOOL function( const VALUE* pval) ValueIsNativeFunctor;
			/// </summary>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(WindowsSciterApi.ValueIsNativeFunctor))]
			public delegate SciterValue.VALUE_RESULT ValueIsNativeFunctor(ref SciterValue.VALUE pval);

			#endregion

			#region Used to be script VM API (Deprecated in v4.4.3.24)
			
#pragma warning disable 618
			/// <summary>
			/// 
			/// </summary>
			[SciterStructMap(nameof(WindowsSciterApi.reserved1))]
			public delegate void Reserved1();
			
			/// <summary>
			/// 
			/// </summary>
			[SciterStructMap(nameof(WindowsSciterApi.reserved2))]
			public delegate void Reserved2();
			
			/// <summary>
			/// 
			/// </summary>
			[SciterStructMap(nameof(WindowsSciterApi.reserved3))]
			public delegate void Reserved3();

			/// <summary>
			/// 
			/// </summary>

			[SciterStructMap(nameof(WindowsSciterApi.reserved4))]
			public delegate void Reserved4();
			
#pragma warning restore 618			
			#endregion
			
			#region Archive

			/// <summary>
			/// HSARCHIVE function(LPCBYTE archiveData, UINT archiveDataLength) SciterOpenArchive;
			/// </summary>
			/// <param name="archiveData"></param>
			/// <param name="archiveDataLength"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterOpenArchive))]
			public delegate IntPtr
				SciterOpenArchive(IntPtr archiveData,
					uint archiveDataLength); // archiveData must point to a pinned byte[] array!

			/// <summary>
			/// BOOL function(HSARCHIVE harc, LPCWSTR path, LPCBYTE* pdata, UINT* pdataLength) SciterGetArchiveItem;
			/// </summary>
			/// <param name="harc"></param>
			/// <param name="path"></param>
			/// <param name="pdata"></param>
			/// <param name="pdataLength"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetArchiveItem))]
			public delegate bool SciterGetArchiveItem(IntPtr harc, [MarshalAs(UnmanagedType.LPWStr)] string path,
				out IntPtr pdata, out uint pdataLength);

			/// <summary>
			/// BOOL function(HSARCHIVE harc) SciterCloseArchive;
			/// </summary>
			/// <param name="harc"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterCloseArchive))]
			public delegate bool SciterCloseArchive(IntPtr harc);

			#endregion

			/// <summary>
			/// SCDOM_RESULT function( const BEHAVIOR_EVENT_PARAMS* evt, BOOL post, BOOL *handled ) SciterFireEvent;
			/// </summary>
			/// <param name="evt"></param>
			/// <param name="post"></param>
			/// <param name="handled"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterFireEvent))]
			public delegate SciterXDom.SCDOM_RESULT SciterFireEvent(ref SciterBehaviors.BEHAVIOR_EVENT_PARAMS evt,
				bool post, out bool handled);

			/// <summary>
			/// LPVOID function(HWINDOW hwnd) SciterGetCallbackParam;
			/// </summary>
			/// <param name="hwnd"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterGetCallbackParam))]
			public delegate IntPtr SciterGetCallbackParam(IntPtr hwnd);

			/// <summary>
			/// UINT_PTR function(HWINDOW hwnd, UINT_PTR wparam, UINT_PTR lparam, UINT timeoutms) SciterPostCallback;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="wparam"></param>
			/// <param name="lparam"></param>
			/// <param name="timeoutms">if > 0 then it is a send, not a post</param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterPostCallback))]
			public delegate IntPtr SciterPostCallback(IntPtr hwnd, IntPtr wparam, IntPtr lparam, uint timeoutms);

			/// <summary>
			/// LPSciterGraphicsAPI function() GetSciterGraphicsAPI;
			/// </summary>
			[SciterStructMap(nameof(WindowsSciterApi.GetSciterGraphicsAPI))]
			public delegate IntPtr GetSciterGraphicsApi();

			/// <summary>
			/// LPSciterRequestAPI SCFN(GetSciterRequestAPI )();
			/// </summary>
			[SciterStructMap(nameof(WindowsSciterApi.GetSciterRequestAPI))]
			public delegate IntPtr GetSciterRequestApi();

			#region DirectX API

			/// <summary>
			/// BOOL SCFN(SciterCreateOnDirectXWindow ) (HWINDOW hwnd, IDXGISwapChain* pSwapChain);
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="pSwapChain"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterCreateOnDirectXWindow))]
			public delegate bool SciterCreateOnDirectXWindow(IntPtr hwnd, IntPtr pSwapChain);

			/// <summary>
			/// BOOL SCFN(SciterRenderOnDirectXWindow ) (HWINDOW hwnd, HELEMENT elementToRenderOrNull, BOOL frontLayer);
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="elementToRenderOrNull"></param>
			/// <param name="frontLayer"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterRenderOnDirectXWindow))]
			public delegate bool SciterRenderOnDirectXWindow(IntPtr hwnd, IntPtr elementToRenderOrNull,
				bool frontLayer);

			/// <summary>
			/// BOOL SCFN(SciterRenderOnDirectXTexture ) (HWINDOW hwnd, HELEMENT elementToRenderOrNull, IDXGISurface* surface);
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="elementToRenderOrNull"></param>
			/// <param name="surface"></param>
			[SciterStructMap(nameof(WindowsSciterApi.SciterRenderOnDirectXTexture))]
			public delegate bool SciterRenderOnDirectXTexture(IntPtr hwnd, IntPtr elementToRenderOrNull,
				IntPtr surface);

			#endregion

			/// <summary>
			/// BOOL SCFN(SciterProcX)(HWINDOW hwnd, SCITER_X_MSG* pMsg );
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="pMsg"></param>
			/// <returns>TRUE if handled</returns>
			[SciterStructMap(nameof(WindowsSciterApi.SciterProcX))]
			public delegate bool SciterProcX(IntPtr hwnd, IntPtr pMsg);
		}
	}
}