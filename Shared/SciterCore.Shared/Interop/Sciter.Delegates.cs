using System;
using System.Runtime.InteropServices;
using SciterCore.Interop;

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
			[SciterStructMap(nameof(DynamicSciterApi.SciterClassName))]
			public delegate IntPtr SciterClassName();

			// UINT function(BOOL major) SciterVersion;
			[SciterStructMap(nameof(DynamicSciterApi.SciterVersion))]
			public delegate uint SciterVersion(bool major);

			/// <summary> 
			/// BOOL function(HWINDOW hwnd, LPCWSTR uri, LPCBYTE data, UINT dataLength) SciterDataReady;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="uri"></param>
			/// <param name="data"></param>
			/// <param name="dataLength"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterDataReady))]
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
			[SciterStructMap(nameof(DynamicSciterApi.SciterDataReadyAsync))]
			public delegate bool SciterDataReadyAsync(IntPtr hwnd, string uri, byte[] data, uint dataLength,
				IntPtr requestId);

			/// <summary>
			/// LRESULT function(HWINDOW hwnd, UINT msg, WPARAM wParam, LPARAM lParam) SciterProc;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="msg"></param>
			/// <param name="wParam"></param>
			/// <param name="lParam"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterProc))]
			public delegate IntPtr SciterProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

			/// <summary>
			/// LRESULT function(HWINDOW hwnd, UINT msg, WPARAM wParam, LPARAM lParam, BOOL* pbHandled) SciterProcND;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="msg"></param>
			/// <param name="wParam"></param>
			/// <param name="lParam"></param>
			/// <param name="pbHandled"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterProcND))]
			public delegate IntPtr SciterProcNd(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam,
				ref bool pbHandled);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, LPCWSTR filename) SciterLoadFile;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="filename"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterLoadFile))]
			public delegate bool SciterLoadFile(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string filename);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, LPCBYTE html, UINT htmlSize, LPCWSTR baseUrl) SciterLoadHtml;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="html"></param>
			/// <param name="htmlSize"></param>
			/// <param name="baseUrl"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterLoadHtml))]
			public delegate bool SciterLoadHtml(IntPtr hwnd, byte[] html, uint htmlSize, string baseUrl);

			/// <summary>
			/// VOID function(HWINDOW hWndSciter, LPSciterHostCallback cb, LPVOID cbParam) SciterSetCallback;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="cb"></param>
			/// <param name="param"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetCallback))]
			public delegate void SciterSetCallback(IntPtr hwnd, MulticastDelegate cb, IntPtr param); // TODO

			/// <summary>
			/// BOOL function(LPCBYTE utf8, UINT numBytes) SciterSetMasterCSS;
			/// </summary>
			/// <param name="utf8"></param>
			/// <param name="numBytes"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetMasterCSS))]
			public delegate bool SciterSetMasterCss(byte[] utf8, uint numBytes);

			/// <summary>
			/// BOOL function(LPCBYTE utf8, UINT numBytes) SciterAppendMasterCSS;
			/// </summary>
			/// <param name="utf8"></param>
			/// <param name="numBytes"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterAppendMasterCSS))]
			public delegate bool SciterAppendMasterCss(byte[] utf8, uint numBytes);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, LPCBYTE utf8, UINT numBytes, LPCWSTR baseUrl, LPCWSTR mediaType) SciterSetCSS;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="utf8"></param>
			/// <param name="numBytes"></param>
			/// <param name="baseUrl"></param>
			/// <param name="mediaType"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetCSS))]
			public delegate bool SciterSetCss(IntPtr hwnd, byte[] utf8, uint numBytes,
				[MarshalAs(UnmanagedType.LPWStr)] string baseUrl, [MarshalAs(UnmanagedType.LPWStr)] string mediaType);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, LPCWSTR mediaType) SciterSetMediaType;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="mediaType"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetMediaType))]
			public delegate bool SciterSetMediaType(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string mediaType);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, const SCITER_VALUE *mediaVars) SciterSetMediaVars;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="mediaVars"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetMediaVars))]
			public delegate bool SciterSetMediaVars(IntPtr hwnd, ref SciterValue.VALUE mediaVars);

			/// <summary>
			/// UINT function(HWINDOW hWndSciter) SciterGetMinWidth;
			/// </summary>
			/// <param name="hwnd"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetMinWidth))]
			public delegate uint SciterGetMinWidth(IntPtr hwnd);

			/// <summary>
			/// UINT function(HWINDOW hWndSciter, UINT width) SciterGetMinHeight;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="width"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetMinHeight))]
			public delegate uint SciterGetMinHeight(IntPtr hwnd, uint width);

			/// <summary>
			/// BOOL function(HWINDOW hWnd, LPCSTR functionName, UINT argc, const SCITER_VALUE* argv, SCITER_VALUE* retval) SciterCall;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="functionName"></param>
			/// <param name="argc"></param>
			/// <param name="argv"></param>
			/// <param name="retval"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterCall))]
			public delegate bool SciterCall(IntPtr hwnd, [MarshalAs(UnmanagedType.LPStr)] string functionName,
				uint argc, SciterValue.VALUE[] argv, out SciterValue.VALUE retval);

			/// <summary>
			/// BOOL function(HWINDOW hwnd, LPCWSTR script, UINT scriptLength, SCITER_VALUE* pretval) SciterEval;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="script"></param>
			/// <param name="scriptLength"></param>
			/// <param name="pretval"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterEval))]
			public delegate bool SciterEval(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string script,
				uint scriptLength, out SciterValue.VALUE pretval);

			/// <summary>
			/// VOID function(HWINDOW hwnd) SciterUpdateWindow;
			/// </summary>
			/// <param name="hwnd"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterUpdateWindow))]
			public delegate bool SciterUpdateWindow(IntPtr hwnd);

			/// <summary>
			/// BOOL function(MSG* lpMsg) SciterTranslateMessage;
			/// </summary>
			/// <param name="lpMsg"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterTranslateMessage))]
			public delegate bool SciterTranslateMessage(IntPtr lpMsg); // TODO: MSG

			/// <summary>
			/// BOOL function(HWINDOW hWnd, UINT option, UINT_PTR value ) SciterSetOption;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="option"></param>
			/// <param name="value"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetOption))]
			public delegate bool SciterSetOption(IntPtr hwnd, SciterXDef.SCITER_RT_OPTIONS option, IntPtr value);

			/// <summary>
			/// VOID function(HWINDOW hWndSciter, UINT* px, UINT* py) SciterGetPPI;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="px"></param>
			/// <param name="py"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetPPI))]
			public delegate void SciterGetPpi(IntPtr hwnd, ref uint px, ref uint py);

			/// <summary>
			/// BOOL function(HWINDOW hwnd, VALUE* pval) SciterGetViewExpando;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetViewExpando))]
			public delegate bool SciterGetViewExpando(IntPtr hwnd, out SciterValue.VALUE pval);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, ID2D1RenderTarget* prt) SciterRenderD2D;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="prt"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterRenderD2D))]
			public delegate bool SciterRenderD2D(IntPtr hwnd, IntPtr prt); // TODO

			/// <summary>
			/// BOOL function(ID2D1Factory ** ppf) SciterD2DFactory;
			/// </summary>
			/// <param name="ppf"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterD2DFactory))]
			public delegate bool SciterD2DFactory(IntPtr ppf); // TODO

			/// <summary>
			/// BOOL function(IDWriteFactory ** ppf) SciterDWFactory;
			/// </summary>
			/// <param name="ppf"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterDWFactory))]
			public delegate bool SciterDwFactory(IntPtr ppf); // TODO

			/// <summary>
			/// BOOL function(LPUINT pcaps) SciterGraphicsCaps;
			/// </summary>
			/// <param name="pcaps"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGraphicsCaps))]
			public delegate bool SciterGraphicsCaps(ref uint pcaps);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, LPCWSTR baseUrl) SciterSetHomeURL;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="baseUrl"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetHomeURL))]
			public delegate bool SciterSetHomeUrl(IntPtr hwnd, string baseUrl);

			/// <summary>
			/// HWINDOW function( LPRECT frame ) SciterCreateNSView;// returns NSView*
			/// </summary>
			/// <param name="frame"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterCreateNSView))]
			public delegate IntPtr SciterCreateNsView(ref PInvokeUtils.RECT frame);

			/// <summary>
			/// HWINDOW SCFN( SciterCreateWidget )( LPRECT frame ); // returns GtkWidget
			/// </summary>
			/// <param name="frame"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterCreateWidget))]
			public delegate IntPtr SciterCreateWidget(ref PInvokeUtils.RECT frame);

			/// <summary>
			/// HWINDOW function(UINT creationFlags, LPRECT frame, SciterWindowDelegate* delegt, LPVOID delegateParam, HWINDOW parent) SciterCreateWindow;
			/// </summary>
			/// <param name="creationFlags"></param>
			/// <param name="frame"></param>
			/// <param name="delegt"></param>
			/// <param name="delegateParam"></param>
			/// <param name="parent"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterCreateWindow))]
			public delegate IntPtr SciterCreateWindow(SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags,
				ref PInvokeUtils.RECT frame, MulticastDelegate delegt, IntPtr delegateParam, IntPtr parent);

			/// <summary>
			/// VOID function(HWINDOW hwndOrNull, LPVOID param, DEBUG_OUTPUT_PROC     pfOutput) SciterSetupDebugOutput;
			/// </summary>
			/// <param name="hwndOrNull">HWINDOW or null if this is global output handler</param>
			/// <param name="param">param to be passed "as is" to the pfOutput</param>
			/// <param name="pfOutput">output function, output stream alike thing.</param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetupDebugOutput))]
			public delegate void SciterSetupDebugOutput(IntPtr hwndOrNull, IntPtr param,
				SciterXDef.DEBUG_OUTPUT_PROC pfOutput);

			#region DOM Element API

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) Sciter_UseElement;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(DynamicSciterApi.Sciter_UseElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterUseElement(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) Sciter_UnuseElement;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(DynamicSciterApi.Sciter_UnuseElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterUnuseElement(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HWINDOW hwnd, HELEMENT *phe) SciterGetRootElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetRootElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetRootElement(IntPtr hwnd, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HWINDOW hwnd, HELEMENT *phe) SciterGetFocusElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetFocusElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetFocusElement(IntPtr hwnd, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HWINDOW hwnd, POINT pt, HELEMENT* phe) SciterFindElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="pt"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterFindElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterFindElement(IntPtr hwnd, PInvokeUtils.POINT pt,
				out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT* count) SciterGetChildrenCount;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="count"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetChildrenCount))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetChildrenCount(IntPtr he, out uint count);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT n, HELEMENT* phe) SciterGetNthChild;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="n"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetNthChild))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetNthChild(IntPtr he, uint n, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, HELEMENT* p_parent_he) SciterGetParentElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pParentHe"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetParentElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetParentElement(IntPtr he, out IntPtr pParentHe);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, BOOL outer, LPCBYTE_RECEIVER rcv, LPVOID rcv_param) SciterGetElementHtmlCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="outer"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementHtmlCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementHtmlCb(IntPtr he, bool outer,
				SciterXDom.LPCBYTE_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetElementTextCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementTextCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementTextCb(IntPtr he,
				SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR utf16, UINT length) SciterSetElementText;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="utf16"></param>
			/// <param name="length"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetElementText))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetElementText(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string utf16, uint length);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPUINT p_count) SciterGetAttributeCount;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pCount"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetAttributeCount))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetAttributeCount(IntPtr he, out uint pCount);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT n, LPCSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetNthAttributeNameCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="n"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetNthAttributeNameCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetNthAttributeNameCb(IntPtr he, uint n,
				SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT n, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetNthAttributeValueCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="n"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetNthAttributeValueCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetNthAttributeValueCb(IntPtr he, uint n,
				SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetAttributeByNameCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetAttributeByNameCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetAttributeByNameCb(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR value) SciterSetAttributeByName;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="value"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetAttributeByName))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetAttributeByName(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterClearAttributes;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterClearAttributes))]
			public delegate SciterXDom.SCDOM_RESULT SciterClearAttributes(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPUINT p_index) SciterGetElementIndex;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pIndex"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementIndex))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementIndex(IntPtr he, out uint pIndex);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR* p_type) SciterGetElementType;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pType"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementType))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementType(IntPtr he, out IntPtr pType);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetElementTypeCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementTypeCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementTypeCb(IntPtr he,
				SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetStyleAttributeCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetStyleAttributeCB))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetStyleAttributeCb(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR value) SciterSetStyleAttribute;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="value"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetStyleAttribute))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetStyleAttribute(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPRECT p_location, UINT areas /*ELEMENT_AREAS*/) SciterGetElementLocation;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pLocation"></param>
			/// <param name="areas"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementLocation))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementLocation(IntPtr he,
				out PInvokeUtils.RECT pLocation, SciterXDom.ELEMENT_AREAS areas);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT SciterScrollFlags) SciterScrollToView;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="sciterScrollFlags"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterScrollToView))]
			public delegate SciterXDom.SCDOM_RESULT SciterScrollToView(IntPtr he, uint sciterScrollFlags);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, BOOL andForceRender) SciterUpdateElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="andForceRender"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterUpdateElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterUpdateElement(IntPtr he, bool andForceRender);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, RECT rc) SciterRefreshElementArea;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="rc"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterRefreshElementArea))]
			public delegate SciterXDom.SCDOM_RESULT SciterRefreshElementArea(IntPtr he, PInvokeUtils.RECT rc);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterSetCapture;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetCapture))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetCapture(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterReleaseCapture;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterReleaseCapture))]
			public delegate SciterXDom.SCDOM_RESULT SciterReleaseCapture(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, HWINDOW* p_hwnd, BOOL rootWindow) SciterGetElementHwnd;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pHwnd"></param>
			/// <param name="rootWindow"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementHwnd))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementHwnd(IntPtr he, out IntPtr pHwnd,
				bool rootWindow);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPWSTR szUrlBuffer, UINT UrlBufferSize) SciterCombineURL;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="szUrlBuffer"></param>
			/// <param name="urlBufferSize"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterCombineURL))]
			public delegate SciterXDom.SCDOM_RESULT SciterCombineUrl(IntPtr he, /*[MarshalAs(UnmanagedType.LPWStr)]*/
				IntPtr szUrlBuffer, uint urlBufferSize);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT  he, LPCSTR    CSS_selectors, SciterElementCallback callback, LPVOID param) SciterSelectElements;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="cssSelectors"></param>
			/// <param name="callback"></param>
			/// <param name="param"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSelectElements))]
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
			[SciterStructMap(nameof(DynamicSciterApi.SciterSelectElementsW))]
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
			[SciterStructMap(nameof(DynamicSciterApi.SciterSelectParent))]
			public delegate SciterXDom.SCDOM_RESULT SciterSelectParent(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string selector, uint depth, out IntPtr heFound);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR selector, UINT depth, HELEMENT* heFound) SciterSelectParentW;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="selector"></param>
			/// <param name="depth"></param>
			/// <param name="heFound"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSelectParentW))]
			public delegate SciterXDom.SCDOM_RESULT SciterSelectParentW(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string selector, uint depth, out IntPtr heFound);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, const BYTE* html, UINT htmlLength, UINT where) SciterSetElementHtml;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="html"></param>
			/// <param name="htmlLength"></param>
			/// <param name="where"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetElementHtml))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetElementHtml(IntPtr he, byte[] html, uint htmlLength,
				SciterXDom.SET_ELEMENT_HTML where);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT* puid) SciterGetElementUID;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="puid"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementUID))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementUid(IntPtr he, out uint puid);

			/// <summary>
			/// SCDOM_RESULT function(HWINDOW hwnd, UINT uid, HELEMENT* phe) SciterGetElementByUID;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="uid"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementByUID))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementByUid(IntPtr hwnd, uint uid, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT hePopup, HELEMENT heAnchor, UINT placement) SciterShowPopup;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="heAnchor"></param>
			/// <param name="placement"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterShowPopup))]
			public delegate SciterXDom.SCDOM_RESULT SciterShowPopup(IntPtr he, IntPtr heAnchor, uint placement);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT hePopup, POINT pos, UINT placement) SciterShowPopupAt;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pos"></param>
			/// <param name="placement"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterShowPopupAt))]
			public delegate SciterXDom.SCDOM_RESULT SciterShowPopupAt(IntPtr he, PInvokeUtils.POINT pos,
				uint placement);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterHidePopup;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterHidePopup))]
			public delegate SciterXDom.SCDOM_RESULT SciterHidePopup(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT* pstateBits) SciterGetElementState;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pstateBits"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementState))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementState(IntPtr he, out uint pstateBits);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT stateBitsToSet, UINT stateBitsToClear, BOOL updateView) SciterSetElementState;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="stateBitsToSet"></param>
			/// <param name="stateBitsToClear"></param>
			/// <param name="updateView"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetElementState))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetElementState(IntPtr he, uint stateBitsToSet,
				uint stateBitsToClear, bool updateView);

			/// <summary>
			/// SCDOM_RESULT function( LPCSTR tagname, LPCWSTR textOrNull, /*out*/ HELEMENT *phe ) SciterCreateElement;
			/// </summary>
			/// <param name="tagname"></param>
			/// <param name="textOrNull"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterCreateElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterCreateElement(
				[MarshalAs(UnmanagedType.LPStr)] string tagname, [MarshalAs(UnmanagedType.LPWStr)] string textOrNull,
				out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, /*out*/ HELEMENT *phe ) SciterCloneElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterCloneElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterCloneElement(IntPtr he, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, HELEMENT hparent, UINT index ) SciterInsertElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="hparent"></param>
			/// <param name="index"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterInsertElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterInsertElement(IntPtr he, IntPtr hparent, uint index);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he ) SciterDetachElement;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterDetachElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterDetachElement(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterDeleteElement;
			/// </summary>
			/// <param name="he"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterDeleteElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterDeleteElement(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT milliseconds, UINT_PTR timer_id ) SciterSetTimer;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="milliseconds"></param>
			/// <param name="timerId"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetTimer))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetTimer(IntPtr he, uint milliseconds, IntPtr timerId);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPELEMENT_EVENT_PROC pep, LPVOID tag ) SciterDetachEventHandler;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pep"></param>
			/// <param name="tag"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterDetachEventHandler))]
			public delegate SciterXDom.SCDOM_RESULT SciterDetachEventHandler(IntPtr he, MulticastDelegate pep,
				IntPtr tag);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPELEMENT_EVENT_PROC pep, LPVOID tag ) SciterAttachEventHandler;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pep"></param>
			/// <param name="tag"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterAttachEventHandler))]
			public delegate SciterXDom.SCDOM_RESULT SciterAttachEventHandler(IntPtr he, MulticastDelegate pep,
				IntPtr tag);

			/// <summary>
			/// SCDOM_RESULT function( HWINDOW hwndLayout, LPELEMENT_EVENT_PROC pep, LPVOID tag, UINT subscription ) SciterWindowAttachEventHandler;
			/// </summary>
			/// <param name="hwndLayout"></param>
			/// <param name="pep"></param>
			/// <param name="tag"></param>
			/// <param name="subscription"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterWindowAttachEventHandler))]
			public delegate SciterXDom.SCDOM_RESULT SciterWindowAttachEventHandler(IntPtr hwndLayout,
				MulticastDelegate pep, IntPtr tag, uint subscription);

			/// <summary>
			/// SCDOM_RESULT function( HWINDOW hwndLayout, LPELEMENT_EVENT_PROC pep, LPVOID tag ) SciterWindowDetachEventHandler;
			/// </summary>
			/// <param name="hwndLayout"></param>
			/// <param name="pep"></param>
			/// <param name="tag"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterWindowDetachEventHandler))]
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
			[SciterStructMap(nameof(DynamicSciterApi.SciterSendEvent))]
			public delegate SciterXDom.SCDOM_RESULT SciterSendEvent(IntPtr he, uint appEventCode, IntPtr heSource,
				IntPtr reason, out bool handled);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT appEventCode, HELEMENT heSource, UINT_PTR reason) SciterPostEvent;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="appEventCode"></param>
			/// <param name="heSource"></param>
			/// <param name="reason"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterPostEvent))]
			public delegate SciterXDom.SCDOM_RESULT SciterPostEvent(IntPtr he, uint appEventCode, IntPtr heSource,
				IntPtr reason);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, METHOD_PARAMS* params) SciterCallBehaviorMethod;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="param"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterCallBehaviorMethod))]
			public delegate SciterXDom.SCDOM_RESULT SciterCallBehaviorMethod(IntPtr he,
				ref SciterXDom.METHOD_PARAMS param);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR url, UINT dataType, HELEMENT initiator) SciterRequestElementData;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="url"></param>
			/// <param name="dataType"></param>
			/// <param name="initiator"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterRequestElementData))]
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
			[SciterStructMap(nameof(DynamicSciterApi.SciterHttpRequest))]
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
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetScrollInfo))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetScrollInfo(IntPtr he, out PInvokeUtils.POINT scrollPos,
				out PInvokeUtils.RECT viewRect, out PInvokeUtils.SIZE contentSize);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, POINT scrollPos, BOOL smooth ) SciterSetScrollPos;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="scrollPos"></param>
			/// <param name="smooth"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetScrollPos))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetScrollPos(IntPtr he, PInvokeUtils.POINT scrollPos,
				bool smooth);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, INT* pMinWidth, INT* pMaxWidth ) SciterGetElementIntrinsicWidths;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pMinWidth"></param>
			/// <param name="pMaxWidth"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementIntrinsicWidths))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementIntrinsicWidths(IntPtr he, out int pMinWidth,
				out int pMaxWidth);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, INT forWidth, INT* pHeight ) SciterGetElementIntrinsicHeight;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="forWidth"></param>
			/// <param name="pHeight"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementIntrinsicHeight))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementIntrinsicHeight(IntPtr he, int forWidth,
				out int pHeight);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, BOOL* pVisible) SciterIsElementVisible;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pVisible"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterIsElementVisible))]
			public delegate SciterXDom.SCDOM_RESULT SciterIsElementVisible(IntPtr he, out bool pVisible);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, BOOL* pEnabled ) SciterIsElementEnabled;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pEnabled"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterIsElementEnabled))]
			public delegate SciterXDom.SCDOM_RESULT SciterIsElementEnabled(IntPtr he, out bool pEnabled);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT firstIndex, UINT lastIndex, ELEMENT_COMPARATOR* cmpFunc, LPVOID cmpFuncParam ) SciterSortElements;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="firstIndex"></param>
			/// <param name="lastIndex"></param>
			/// <param name="cmpFunc"></param>
			/// <param name="cmpFuncParam"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSortElements))]
			public delegate SciterXDom.SCDOM_RESULT SciterSortElements(IntPtr he, uint firstIndex, uint lastIndex,
				SciterXDom.ELEMENT_COMPARATOR cmpFunc, IntPtr cmpFuncParam);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he1, HELEMENT he2 ) SciterSwapElements;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="he2"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSwapElements))]
			public delegate SciterXDom.SCDOM_RESULT SciterSwapElements(IntPtr he, IntPtr he2);

			/// <summary>
			/// SCDOM_RESULT function( UINT evt, LPVOID eventCtlStruct, BOOL* bOutProcessed ) SciterTraverseUIEvent;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="eventCtlStruct"></param>
			/// <param name="bOutProcessed"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterTraverseUIEvent))]
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
			[SciterStructMap(nameof(DynamicSciterApi.SciterCallScriptingMethod))]
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
			[SciterStructMap(nameof(DynamicSciterApi.SciterCallScriptingFunction))]
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
			[SciterStructMap(nameof(DynamicSciterApi.SciterEvalElementScript))]
			public delegate SciterXDom.SCDOM_RESULT SciterEvalElementScript(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string script, uint scriptLength, out SciterValue.VALUE retval);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, HWINDOW hwnd) SciterAttachHwndToElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="hwnd"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterAttachHwndToElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterAttachHwndToElement(IntPtr he, IntPtr hwnd);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, /*CTL_TYPE*/ UINT *pType ) SciterControlGetType;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pType"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterControlGetType))]
			public delegate SciterXDom.SCDOM_RESULT SciterControlGetType(IntPtr he, out uint pType);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, VALUE* pval ) SciterGetValue;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetValue))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetValue(IntPtr he, out SciterValue.VALUE pval);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, const VALUE* pval ) SciterSetValue;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetValue))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetValue(IntPtr he, ref SciterValue.VALUE pval);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, VALUE* pval, BOOL forceCreation ) SciterGetExpando;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			/// <param name="forceCreation"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetExpando))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetExpando(IntPtr he, out SciterValue.VALUE pval,
				bool forceCreation);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, tiscript_value* pval, BOOL forceCreation ) SciterGetObject;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			/// <param name="forceCreation"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetObject))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetObject(IntPtr he, out IntPtr pval,
				bool forceCreation);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, tiscript_value* pval) SciterGetElementNamespace;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementNamespace))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementNamespace(IntPtr he,
				out IntPtr pval);

			/// <summary>
			/// SCDOM_RESULT function( HWINDOW hwnd, HELEMENT* phe) SciterGetHighlightedElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="phe"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetHighlightedElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetHighlightedElement(IntPtr hwnd, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function( HWINDOW hwnd, HELEMENT he) SciterSetHighlightedElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="he"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetHighlightedElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterSetHighlightedElement(IntPtr hwnd, IntPtr he);

			#endregion

			#region DOM Node API

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn) SciterNodeAddRef;
			/// </summary>
			/// <param name="hn"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeAddRef))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeAddRef(IntPtr hn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn) SciterNodeRelease;
			/// </summary>
			/// <param name="hn"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeRelease))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeRelease(IntPtr hn);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, HNODE* phn) SciterNodeCastFromElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="phn"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeCastFromElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeCastFromElement(IntPtr he, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HELEMENT* he) SciterNodeCastToElement;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="he"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeCastToElement))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeCastToElement(IntPtr hn, out IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodeFirstChild;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="phn"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeFirstChild))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeFirstChild(IntPtr hn, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodeLastChild;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="phn"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeLastChild))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeLastChild(IntPtr hn, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodeNextSibling;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="phn"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeNextSibling))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeNextSibling(IntPtr hn, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodePrevSibling;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="phn"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodePrevSibling))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodePrevSibling(IntPtr hn, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, HELEMENT* pheParent) SciterNodeParent;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="pheParent"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeParent))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeParent(IntPtr hn, out IntPtr pheParent);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, UINT n, HNODE* phn) SciterNodeNthChild;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="n"></param>
			/// <param name="phn"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeNthChild))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeNthChild(IntPtr hn, uint n, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, UINT* pn) SciterNodeChildrenCount;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="pn"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeChildrenCount))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeChildrenCount(IntPtr hn, out uint pn);

			/// <summary>
			/// /SCDOM_RESULT function(HNODE hnode, UINT* pNodeType /*NODE_TYPE*/) SciterNodeType;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="pn"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeType))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeType(IntPtr hn, out SciterXDom.NODE_TYPE pn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterNodeGetText;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeGetText))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeGetText(IntPtr hn, SciterXDom.LPCWSTR_RECEIVER rcv,
				IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, LPCWSTR text, UINT textLength) SciterNodeSetText;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="text"></param>
			/// <param name="textLength"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeSetText))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeSetText(IntPtr hn,
				[MarshalAs(UnmanagedType.LPWStr)] string text, uint textLength);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, UINT where /*NODE_INS_TARGET*/, HNODE what) SciterNodeInsert;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="where"></param>
			/// <param name="what"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeInsert))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeInsert(IntPtr hn, uint where, IntPtr what);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, BOOL finalize) SciterNodeRemove;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="finalize"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeRemove))]
			public delegate SciterXDom.SCDOM_RESULT SciterNodeRemove(IntPtr hn, bool finalize);

			/// <summary>
			/// SCDOM_RESULT function(LPCWSTR text, UINT textLength, HNODE* phnode) SciterCreateTextNode;
			/// </summary>
			/// <param name="text"></param>
			/// <param name="textLength"></param>
			/// <param name="phnode"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterCreateTextNode))]
			public delegate SciterXDom.SCDOM_RESULT SciterCreateTextNode(
				[MarshalAs(UnmanagedType.LPWStr)] string text, uint textLength, out IntPtr phnode);

			/// <summary>
			/// SCDOM_RESULT function(LPCWSTR text, UINT textLength, HNODE* phnode) SciterCreateCommentNode;
			/// </summary>
			/// <param name="text"></param>
			/// <param name="textLength"></param>
			/// <param name="phnode"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterCreateCommentNode))]
			public delegate SciterXDom.SCDOM_RESULT SciterCreateCommentNode(
				[MarshalAs(UnmanagedType.LPWStr)] string text, uint textLength, out IntPtr phnode);

			#endregion

			#region Value API

			/// <summary>
			/// UINT function( VALUE* pval ) ValueInit;
			/// </summary>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueInit))]
			public delegate SciterValue.VALUE_RESULT ValueInit(out SciterValue.VALUE pval);

			/// <summary>
			/// UINT function( VALUE* pval ) ValueClear;
			/// </summary>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueClear))]
			public delegate SciterValue.VALUE_RESULT ValueClear(out SciterValue.VALUE pval);

			/// <summary>
			/// UINT function( const VALUE* pval1, const VALUE* pval2 ) ValueCompare;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pval2"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueCompare))]
			public delegate SciterValue.VALUE_RESULT ValueCompare(ref SciterValue.VALUE pval, ref IntPtr pval2);

			/// <summary>
			/// UINT function( VALUE* pdst, const VALUE* psrc ) ValueCopy;
			/// </summary>
			/// <param name="pdst"></param>
			/// <param name="psrc"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueCopy))]
			public delegate SciterValue.VALUE_RESULT ValueCopy(out SciterValue.VALUE pdst, ref SciterValue.VALUE psrc);

			/// <summary>
			/// UINT function( VALUE* pdst ) ValueIsolate;
			/// </summary>
			/// <param name="pdst"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueIsolate))]
			public delegate SciterValue.VALUE_RESULT ValueIsolate(ref SciterValue.VALUE pdst);

			/// <summary>
			/// UINT function( const VALUE* pval, UINT* pType, UINT* pUnits ) ValueType;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pType"></param>
			/// <param name="pUnits"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueType))]
			public delegate SciterValue.VALUE_RESULT ValueType(ref SciterValue.VALUE pval, out uint pType,
				out uint pUnits);

			/// <summary>
			/// UINT function( const VALUE* pval, LPCWSTR* pChars, UINT* pNumChars ) ValueStringData;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pChars"></param>
			/// <param name="pNumChars"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueStringData))]
			public delegate SciterValue.VALUE_RESULT ValueStringData(ref SciterValue.VALUE pval, out IntPtr pChars,
				out uint pNumChars);

			/// <summary>
			/// UINT function( VALUE* pval, LPCWSTR chars, UINT numChars, UINT units ) ValueStringDataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="chars"></param>
			/// <param name="numChars"></param>
			/// <param name="units"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueStringDataSet))]
			public delegate SciterValue.VALUE_RESULT ValueStringDataSet(ref SciterValue.VALUE pval,
				[MarshalAs(UnmanagedType.LPWStr)] string chars, uint numChars, uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, INT* pData ) ValueIntData;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pData"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueIntData))]
			public delegate SciterValue.VALUE_RESULT ValueIntData(ref SciterValue.VALUE pval, out int pData);

			/// <summary>
			/// UINT function( VALUE* pval, INT data, UINT type, UINT units ) ValueIntDataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="data"></param>
			/// <param name="type"></param>
			/// <param name="units"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueIntDataSet))]
			public delegate SciterValue.VALUE_RESULT ValueIntDataSet(ref SciterValue.VALUE pval, int data, uint type,
				uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, INT64* pData ) ValueInt64Data;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pData"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueInt64Data))]
			public delegate SciterValue.VALUE_RESULT ValueInt64Data(ref SciterValue.VALUE pval, out long pData);

			/// <summary>
			/// UINT function( VALUE* pval, INT64 data, UINT type, UINT units ) ValueInt64DataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="data"></param>
			/// <param name="type"></param>
			/// <param name="units"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueInt64DataSet))]
			public delegate SciterValue.VALUE_RESULT ValueInt64DataSet(ref SciterValue.VALUE pval, long data,
				uint type, uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, FLOAT_VALUE* pData ) ValueFloatData;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pData"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueFloatData))]
			public delegate SciterValue.VALUE_RESULT ValueFloatData(ref SciterValue.VALUE pval, out double pData);

			/// <summary>
			/// UINT function( VALUE* pval, FLOAT_VALUE data, UINT type, UINT units ) ValueFloatDataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="data"></param>
			/// <param name="type"></param>
			/// <param name="units"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueFloatDataSet))]
			public delegate SciterValue.VALUE_RESULT ValueFloatDataSet(ref SciterValue.VALUE pval, double data,
				uint type, uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, LPCBYTE* pBytes, UINT* pnBytes ) ValueBinaryData;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pBytes"></param>
			/// <param name="pnBytes"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueBinaryData))]
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
			[SciterStructMap(nameof(DynamicSciterApi.ValueBinaryDataSet))]
			public delegate SciterValue.VALUE_RESULT ValueBinaryDataSet(ref SciterValue.VALUE pval,
				[MarshalAs(UnmanagedType.LPArray)] byte[] pBytes, uint nBytes, uint type, uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, INT* pn) ValueElementsCount;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pn"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueElementsCount))]
			public delegate SciterValue.VALUE_RESULT ValueElementsCount(ref SciterValue.VALUE pval, out int pn);

			/// <summary>
			/// UINT function( const VALUE* pval, INT n, VALUE* pretval) ValueNthElementValue;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="n"></param>
			/// <param name="pretval"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueNthElementValue))]
			public delegate SciterValue.VALUE_RESULT ValueNthElementValue(ref SciterValue.VALUE pval, int n,
				out SciterValue.VALUE pretval);

			/// <summary>
			/// UINT function( VALUE* pval, INT n, const VALUE* pval_to_set) ValueNthElementValueSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="n"></param>
			/// <param name="pvalToSet"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueNthElementValueSet))]
			public delegate SciterValue.VALUE_RESULT ValueNthElementValueSet(ref SciterValue.VALUE pval, int n,
				ref SciterValue.VALUE pvalToSet);

			/// <summary>
			/// UINT function( const VALUE* pval, INT n, VALUE* pretval) ValueNthElementKey;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="n"></param>
			/// <param name="pretval"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueNthElementKey))]
            			public delegate SciterValue.VALUE_RESULT ValueNthElementKey(ref SciterValue.VALUE pval, int n,
				out SciterValue.VALUE pretval);

			/// <summary>
			/// UINT function( VALUE* pval, KeyValueCallback* penum, LPVOID param) ValueEnumElements;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="penum"></param>
			/// <param name="param"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueEnumElements))]
			public delegate SciterValue.VALUE_RESULT ValueEnumElements(ref SciterValue.VALUE pval,
				SciterValue.KEY_VALUE_CALLBACK penum, IntPtr param);

			/// <summary>
			/// UINT function( VALUE* pval, const VALUE* pkey, const VALUE* pval_to_set) ValueSetValueToKey;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pkey"></param>
			/// <param name="pvalToSet"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueSetValueToKey))]
			public delegate SciterValue.VALUE_RESULT ValueSetValueToKey(ref SciterValue.VALUE pval,
				ref SciterValue.VALUE pkey, ref SciterValue.VALUE pvalToSet);

			/// <summary>
			/// UINT function( const VALUE* pval, const VALUE* pkey, VALUE* pretval) ValueGetValueOfKey;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pkey"></param>
			/// <param name="pretval"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueGetValueOfKey))]
			public delegate SciterValue.VALUE_RESULT ValueGetValueOfKey(ref SciterValue.VALUE pval,
				ref SciterValue.VALUE pkey, out SciterValue.VALUE pretval);

			/// <summary>
			/// UINT function( VALUE* pval, /*VALUE_STRING_CVT_TYPE*/ UINT how ) ValueToString;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="how"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueToString))]
			public delegate SciterValue.VALUE_RESULT ValueToString(ref SciterValue.VALUE pval,
				SciterValue.VALUE_STRING_CVT_TYPE how);

			/// <summary>
			/// UINT function( VALUE* pval, LPCWSTR str, UINT strLength, /*VALUE_STRING_CVT_TYPE*/ UINT how ) ValueFromString;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="str"></param>
			/// <param name="strLength"></param>
			/// <param name="how"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueFromString))]
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
			[SciterStructMap(nameof(DynamicSciterApi.ValueInvoke))]
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
			[SciterStructMap(nameof(DynamicSciterApi.ValueNativeFunctorSet))]
			public delegate SciterValue.VALUE_RESULT ValueNativeFunctorSet(ref SciterValue.VALUE pval,
				SciterValue.NATIVE_FUNCTOR_INVOKE pinvoke, SciterValue.NATIVE_FUNCTOR_RELEASE prelease, IntPtr tag);

			/// <summary>
			/// BOOL function( const VALUE* pval) ValueIsNativeFunctor;
			/// </summary>
			/// <param name="pval"></param>
			[SciterStructMap(nameof(DynamicSciterApi.ValueIsNativeFunctor))]
			public delegate SciterValue.VALUE_RESULT ValueIsNativeFunctor(ref SciterValue.VALUE pval);

			#endregion

			#region Used to be script VM API (Deprecated in v4.4.3.24)
			
#pragma warning disable 618
			
			[SciterStructMap(nameof(DynamicSciterApi.GetTIScriptApi))]
			public delegate IntPtr GetTIScriptApi();
			
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetVM))]
			public delegate IntPtr SciterGetVM(IntPtr hwnd);

			[SciterStructMap(nameof(DynamicSciterApi.Sciter_v2V))]
			public delegate bool Sciter_v2V(IntPtr vm, SciterScript.ScriptValue scriptValue, ref SciterValue.VALUE value, bool isolate);

			[SciterStructMap(nameof(DynamicSciterApi.Sciter_V2v))]
			public delegate bool Sciter_V2v(IntPtr vm, ref SciterValue.VALUE value, ref SciterScript.ScriptValue scriptValue);
			
			/// <summary>
			/// 
			/// </summary>
			[SciterStructMap(nameof(DynamicSciterApi.reserved1))]
			public delegate void Reserved1();
			
			/// <summary>
			/// 
			/// </summary>
			[SciterStructMap(nameof(DynamicSciterApi.reserved2))]
			public delegate void Reserved2();
			
			/// <summary>
			/// 
			/// </summary>
			[SciterStructMap(nameof(DynamicSciterApi.reserved3))]
			public delegate void Reserved3();

			/// <summary>
			/// 
			/// </summary>

			[SciterStructMap(nameof(DynamicSciterApi.reserved4))]
			public delegate void Reserved4();
			
#pragma warning restore 618			
			#endregion
			
			#region Archive

			/// <summary>
			/// HSARCHIVE function(LPCBYTE archiveData, UINT archiveDataLength) SciterOpenArchive;
			/// </summary>
			/// <param name="archiveData"></param>
			/// <param name="archiveDataLength"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterOpenArchive))]
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
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetArchiveItem))]
			public delegate bool SciterGetArchiveItem(IntPtr harc, [MarshalAs(UnmanagedType.LPWStr)] string path,
				out IntPtr pdata, out uint pdataLength);

			/// <summary>
			/// BOOL function(HSARCHIVE harc) SciterCloseArchive;
			/// </summary>
			/// <param name="harc"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterCloseArchive))]
			public delegate bool SciterCloseArchive(IntPtr harc);

			#endregion

			/// <summary>
			/// SCDOM_RESULT function( const BEHAVIOR_EVENT_PARAMS* evt, BOOL post, BOOL *handled ) SciterFireEvent;
			/// </summary>
			/// <param name="evt"></param>
			/// <param name="post"></param>
			/// <param name="handled"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterFireEvent))]
			public delegate SciterXDom.SCDOM_RESULT SciterFireEvent(ref SciterBehaviors.BEHAVIOR_EVENT_PARAMS evt,
				bool post, out bool handled);

			/// <summary>
			/// LPVOID function(HWINDOW hwnd) SciterGetCallbackParam;
			/// </summary>
			/// <param name="hwnd"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetCallbackParam))]
			public delegate IntPtr SciterGetCallbackParam(IntPtr hwnd);

			/// <summary>
			/// UINT_PTR function(HWINDOW hwnd, UINT_PTR wparam, UINT_PTR lparam, UINT timeoutms) SciterPostCallback;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="wparam"></param>
			/// <param name="lparam"></param>
			/// <param name="timeoutms">if > 0 then it is a send, not a post</param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterPostCallback))]
			public delegate IntPtr SciterPostCallback(IntPtr hwnd, IntPtr wparam, IntPtr lparam, uint timeoutms);

			/// <summary>
			/// LPSciterGraphicsAPI function() GetSciterGraphicsAPI;
			/// </summary>
			[SciterStructMap(nameof(DynamicSciterApi.GetSciterGraphicsAPI))]
			public delegate IntPtr GetSciterGraphicsApi();

			/// <summary>
			/// LPSciterRequestAPI SCFN(GetSciterRequestAPI )();
			/// </summary>
			[SciterStructMap(nameof(DynamicSciterApi.GetSciterRequestAPI))]
			public delegate IntPtr GetSciterRequestApi();

			#region DirectX API

			/// <summary>
			/// BOOL SCFN(SciterCreateOnDirectXWindow ) (HWINDOW hwnd, IDXGISwapChain* pSwapChain);
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="pSwapChain"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterCreateOnDirectXWindow))]
			public delegate bool SciterCreateOnDirectXWindow(IntPtr hwnd, IntPtr pSwapChain);

			/// <summary>
			/// BOOL SCFN(SciterRenderOnDirectXWindow ) (HWINDOW hwnd, HELEMENT elementToRenderOrNull, BOOL frontLayer);
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="elementToRenderOrNull"></param>
			/// <param name="frontLayer"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterRenderOnDirectXWindow))]
			public delegate bool SciterRenderOnDirectXWindow(IntPtr hwnd, IntPtr elementToRenderOrNull,
				bool frontLayer);

			/// <summary>
			/// BOOL SCFN(SciterRenderOnDirectXTexture ) (HWINDOW hwnd, HELEMENT elementToRenderOrNull, IDXGISurface* surface);
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="elementToRenderOrNull"></param>
			/// <param name="surface"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterRenderOnDirectXTexture))]
			public delegate bool SciterRenderOnDirectXTexture(IntPtr hwnd, IntPtr elementToRenderOrNull,
				IntPtr surface);

			#endregion

			/// <summary>
			/// BOOL SCFN(SciterProcX)(HWINDOW hwnd, SCITER_X_MSG* pMsg );
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="pMsg"></param>
			/// <returns>TRUE if handled</returns>
			[SciterStructMap(nameof(DynamicSciterApi.SciterProcX))]
			public delegate bool SciterProcX(IntPtr hwnd, IntPtr pMsg);
			
			#region Sciter 4.4.3.24
			
			/// <summary>
			/// UINT64 SCFN(SciterAtomValue)(const char* name);
			/// </summary>
			/// <param name="name"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterAtomValue))]
			public delegate ulong SciterAtomValue(string name);

			/// <summary>
			/// BOOL SCFN(SciterAtomNameCB)(UINT64 atomv, LPCSTR_RECEIVER* rcv, LPVOID rcv_param);
			/// </summary>
			/// <param name="atomv"></param>
			/// <param name="rxc"></param>
			/// <param name="rcvParam"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterAtomNameCB))]
			public delegate bool SciterAtomNameCB(ulong atomv, IntPtr rxc, IntPtr rcvParam);

			/// <summary>
			/// BOOL SCFN(SciterSetGlobalAsset)(som_asset_t* pass);
			/// </summary>
			/// <param name="pass"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetGlobalAsset))]
			public delegate bool SciterSetGlobalAsset(IntPtr pass);
			
			#endregion

			#region Sciter 4.4.4.7
			
			/// <summary>
			/// SCDOM_RESULT SCFN(SciterGetElementAsset)(HELEMENT el, UINT64 nameAtom, som_asset_t** ppass);
			/// </summary>
			/// <param name="el"></param>
			/// <param name="nameAtom"></param>
			/// <param name="ppass"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetElementAsset))]
			public delegate SciterXDom.SCDOM_RESULT SciterGetElementAsset(IntPtr el, ulong nameAtom, out IntPtr ppass);
			
			/// <summary>
			/// BOOL SCFN(SciterSetVariable)(HWINDOW hwndOrNull, LPCWSTR path, const VALUE* pval_to_set); (>= 4.4.4.6)
			/// </summary>
			/// <param name="hwndOrNull"></param>
			/// <param name="path"></param>
			/// <param name="pvalToSet"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetVariable4446))]
			public delegate bool SciterSetVariable4446(IntPtr hwndOrNull, [MarshalAs(UnmanagedType.LPWStr)] string path, ref SciterValue.VALUE pvalToSet);

			/// <summary>
			/// UINT SCFN(SciterSetVariable)(HWINDOW hwndOrNull, LPCWSTR path, const VALUE* pvalToSet); (>= 4.4.5.4)
			/// </summary>
			/// <param name="hwndOrNull"></param>
			/// <param name="path"></param>
			/// <param name="pvalToSet"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterSetVariable))]
			public delegate uint SciterSetVariable(IntPtr hwndOrNull, [MarshalAs(UnmanagedType.LPWStr)] string path, ref SciterValue.VALUE pvalToSet);

			/// <summary>
			/// BOOL SCFN(SciterGetVariable)(HWINDOW hwndOrNull, LPCWSTR path, VALUE* pval_to_get); (4.4.4.6)
			/// </summary>
			/// <param name="hwndOrNull"></param>
			/// <param name="path"></param>
			/// <param name="pvalToGet"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetVariable4446))]
			public delegate bool SciterGetVariable4446(IntPtr hwndOrNull, [MarshalAs(UnmanagedType.LPWStr)] string path, out SciterValue.VALUE pvalToGet);

			/// <summary>
			/// UINT SCFN(SciterGetVariable)(HWINDOW hwndOrNull, LPCWSTR path, VALUE* pvalToGet); (>= 4.4.5.4)
			/// </summary>
			/// <param name="hwndOrNull"></param>
			/// <param name="path"></param>
			/// <param name="pvalToGet"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterGetVariable))]
			public delegate uint SciterGetVariable(IntPtr hwndOrNull, [MarshalAs(UnmanagedType.LPWStr)] string path, out SciterValue.VALUE pvalToGet);

			#endregion

			#region Sciter 4.4.5.4

			/// <summary>
			/// UINT SCFN(SciterElementUnwrap)(const VALUE* pval, HELEMENT* ppElement);
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="ppElement"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterElementUnwrap))]
			public delegate uint SciterElementUnwrap(ref SciterValue.VALUE pval, out IntPtr ppElement);


			/// <summary>
			/// UINT SCFN(SciterElementWrap)(VALUE* pval, HELEMENT pElement)
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="ppElement"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterElementWrap))]
			public delegate uint SciterElementWrap(ref SciterValue.VALUE pval, IntPtr ppElement);


			/// <summary>
			/// UINT SCFN(SciterNodeUnwrap)(const VALUE* pval, HNODE* ppNode);
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="ppNode"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeUnwrap))]
			public delegate uint SciterNodeUnwrap(ref SciterValue.VALUE pval, out IntPtr ppNode);


			/// <summary>
			/// UINT SCFN(SciterNodeWrap)(VALUE* pval, HNODE pNode);
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="ppNode"></param>
			[SciterStructMap(nameof(DynamicSciterApi.SciterNodeWrap))]
			public delegate uint SciterNodeWrap(ref SciterValue.VALUE pval, IntPtr ppNode);

			
			#endregion
			
		}
	}
}