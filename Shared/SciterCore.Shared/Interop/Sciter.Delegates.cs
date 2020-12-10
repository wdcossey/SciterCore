using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace SciterCore.Interop
{
	public static partial class Sciter
	{
		internal static class SciterApi
		{
			/// <summary>
			/// LPCWSTR	function() SciterClassName;
			/// Use Marshal.PtrToStringUni(returned IntPtr) to get the actual string
			/// </summary>
			public delegate IntPtr SCITER_CLASS_NAME();

			// UINT	function(BOOL major) SciterVersion;
			public delegate uint SCITER_VERSION(bool major);

			/// <summary> 
			/// BOOL	function(HWINDOW hwnd, LPCWSTR uri, LPCBYTE data, UINT dataLength) SciterDataReady;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="uri"></param>
			/// <param name="data"></param>
			/// <param name="dataLength"></param>
			public delegate bool SCITER_DATA_READY(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string uri,
				byte[] data, uint dataLength);

			/// <summary>
			/// BOOL	function(HWINDOW hwnd, LPCWSTR uri, LPCBYTE data, UINT dataLength, LPVOID requestId) SciterDataReadyAsync;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="uri"></param>
			/// <param name="data"></param>
			/// <param name="dataLength"></param>
			/// <param name="requestId"></param>
			public delegate bool SCITER_DATA_READY_ASYNC(IntPtr hwnd, string uri, byte[] data, uint dataLength,
				IntPtr requestId);

			/// <summary>
			/// LRESULT	function(HWINDOW hwnd, UINT msg, WPARAM wParam, LPARAM lParam) SciterProc;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="msg"></param>
			/// <param name="wParam"></param>
			/// <param name="lParam"></param>
			public delegate IntPtr SCITER_PROC(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

			/// <summary>
			/// LRESULT	function(HWINDOW hwnd, UINT msg, WPARAM wParam, LPARAM lParam, BOOL* pbHandled) SciterProcND;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="msg"></param>
			/// <param name="wParam"></param>
			/// <param name="lParam"></param>
			/// <param name="pbHandled"></param>
			public delegate IntPtr SCITER_PROC_ND(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam,
				ref bool pbHandled);

			/// <summary>
			/// BOOL	function(HWINDOW hWndSciter, LPCWSTR filename) SciterLoadFile;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="filename"></param>
			public delegate bool SCITER_LOAD_FILE(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string filename);

			/// <summary>
			/// BOOL function(HWINDOW hWndSciter, LPCBYTE html, UINT htmlSize, LPCWSTR baseUrl) SciterLoadHtml;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="html"></param>
			/// <param name="htmlSize"></param>
			/// <param name="baseUrl"></param>
			public delegate bool SCITER_LOAD_HTML(IntPtr hwnd, byte[] html, uint htmlSize, string baseUrl);

			/// <summary>
			/// VOID	function(HWINDOW hWndSciter, LPSciterHostCallback cb, LPVOID cbParam) SciterSetCallback;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="cb"></param>
			/// <param name="param"></param>
			public delegate void SCITER_SET_CALLBACK(IntPtr hwnd, MulticastDelegate cb, IntPtr param); // TODO

			/// <summary>
			/// BOOL	function(LPCBYTE utf8, UINT numBytes) SciterSetMasterCSS;
			/// </summary>
			/// <param name="utf8"></param>
			/// <param name="numBytes"></param>
			public delegate bool SCITER_SET_MASTER_CSS(byte[] utf8, uint numBytes);

			/// <summary>
			/// BOOL	function(LPCBYTE utf8, UINT numBytes) SciterAppendMasterCSS;
			/// </summary>
			/// <param name="utf8"></param>
			/// <param name="numBytes"></param>
			public delegate bool SCITER_APPEND_MASTER_CSS(byte[] utf8, uint numBytes);

			/// <summary>
			/// BOOL	function(HWINDOW hWndSciter, LPCBYTE utf8, UINT numBytes, LPCWSTR baseUrl, LPCWSTR mediaType) SciterSetCSS;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="utf8"></param>
			/// <param name="numBytes"></param>
			/// <param name="baseUrl"></param>
			/// <param name="mediaType"></param>
			public delegate bool SCITER_SET_CSS(IntPtr hwnd, byte[] utf8, uint numBytes,
				[MarshalAs(UnmanagedType.LPWStr)] string baseUrl, [MarshalAs(UnmanagedType.LPWStr)] string mediaType);

			/// <summary>
			/// BOOL	function(HWINDOW hWndSciter, LPCWSTR mediaType) SciterSetMediaType;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="mediaType"></param>
			public delegate bool SCITER_SET_MEDIA_TYPE(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string mediaType);

			/// <summary>
			/// BOOL	function(HWINDOW hWndSciter, const SCITER_VALUE *mediaVars) SciterSetMediaVars;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="mediaVars"></param>
			public delegate bool SCITER_SET_MEDIA_VARS(IntPtr hwnd, ref SciterValue.VALUE mediaVars);

			/// <summary>
			/// UINT	function(HWINDOW hWndSciter) SciterGetMinWidth;
			/// </summary>
			/// <param name="hwnd"></param>
			public delegate uint SCITER_GET_MIN_WIDTH(IntPtr hwnd);

			/// <summary>
			/// UINT	function(HWINDOW hWndSciter, UINT width) SciterGetMinHeight;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="width"></param>
			public delegate uint SCITER_GET_MIN_HEIGHT(IntPtr hwnd, uint width);

			/// <summary>
			/// BOOL	function(HWINDOW hWnd, LPCSTR functionName, UINT argc, const SCITER_VALUE* argv, SCITER_VALUE* retval) SciterCall;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="functionName"></param>
			/// <param name="argc"></param>
			/// <param name="argv"></param>
			/// <param name="retval"></param>
			public delegate bool SCITER_CALL(IntPtr hwnd, [MarshalAs(UnmanagedType.LPStr)] string functionName,
				uint argc, SciterValue.VALUE[] argv, out SciterValue.VALUE retval);

			/// <summary>
			/// BOOL	function(HWINDOW hwnd, LPCWSTR script, UINT scriptLength, SCITER_VALUE* pretval) SciterEval;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="script"></param>
			/// <param name="scriptLength"></param>
			/// <param name="pretval"></param>
			public delegate bool SCITER_EVAL(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string script,
				uint scriptLength, out SciterValue.VALUE pretval);

			/// <summary>
			/// VOID	function(HWINDOW hwnd) SciterUpdateWindow;
			/// </summary>
			/// <param name="hwnd"></param>
			public delegate bool SCITER_UPDATE_WINDOW(IntPtr hwnd);

			/// <summary>
			/// BOOL	function(MSG* lpMsg) SciterTranslateMessage;
			/// </summary>
			/// <param name="lpMsg"></param>
			public delegate bool SCITER_TRANSLATE_MESSAGE(IntPtr lpMsg); // TODO: MSG

			/// <summary>
			/// BOOL	function(HWINDOW hWnd, UINT option, UINT_PTR value ) SciterSetOption;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="option"></param>
			/// <param name="value"></param>
			public delegate bool SCITER_SET_OPTION(IntPtr hwnd, SciterXDef.SCITER_RT_OPTIONS option, IntPtr value);

			/// <summary>
			/// VOID	function(HWINDOW hWndSciter, UINT* px, UINT* py) SciterGetPPI;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="px"></param>
			/// <param name="py"></param>
			public delegate void SCITER_GET_PPI(IntPtr hwnd, ref uint px, ref uint py);

			/// <summary>
			/// BOOL	function(HWINDOW hwnd, VALUE* pval) SciterGetViewExpando;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="pval"></param>
			public delegate bool SCITER_GET_VIEW_EXPANDO(IntPtr hwnd, out SciterValue.VALUE pval);

			/// <summary>
			/// BOOL	function(HWINDOW hWndSciter, ID2D1RenderTarget* prt) SciterRenderD2D;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="prt"></param>
			public delegate bool SCITER_RENDER_D2D(IntPtr hwnd, IntPtr prt); // TODO

			/// <summary>
			/// BOOL	function(ID2D1Factory ** ppf) SciterD2DFactory;
			/// </summary>
			/// <param name="ppf"></param>
			public delegate bool SCITER_D2D_FACTORY(IntPtr ppf); // TODO

			/// <summary>
			/// BOOL	function(IDWriteFactory ** ppf) SciterDWFactory;
			/// </summary>
			/// <param name="ppf"></param>
			public delegate bool SCITER_DW_FACTORY(IntPtr ppf); // TODO

			/// <summary>
			/// BOOL	function(LPUINT pcaps) SciterGraphicsCaps;
			/// </summary>
			/// <param name="pcaps"></param>
			public delegate bool SCITER_GRAPHICS_CAPS(ref uint pcaps);

			/// <summary>
			/// BOOL	function(HWINDOW hWndSciter, LPCWSTR baseUrl) SciterSetHomeURL;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="baseUrl"></param>
			public delegate bool SCITER_SET_HOME_URL(IntPtr hwnd, string baseUrl);

			/// <summary>
			/// HWINDOW function( LPRECT frame ) SciterCreateNSView;// returns NSView*
			/// </summary>
			/// <param name="frame"></param>
			public delegate IntPtr SCITER_CREATE_NS_VIEW(ref PInvokeUtils.RECT frame);

			/// <summary>
			/// HWINDOW SCFN( SciterCreateWidget )( LPRECT frame ); // returns GtkWidget
			/// </summary>
			/// <param name="frame"></param>
			public delegate IntPtr SCITER_CREATE_WIDGET(ref PInvokeUtils.RECT frame);

			/// <summary>
			/// HWINDOW	function(UINT creationFlags, LPRECT frame, SciterWindowDelegate* delegt, LPVOID delegateParam, HWINDOW parent) SciterCreateWindow;
			/// </summary>
			/// <param name="creationFlags"></param>
			/// <param name="frame"></param>
			/// <param name="delegt"></param>
			/// <param name="delegateParam"></param>
			/// <param name="parent"></param>
			public delegate IntPtr SCITER_CREATE_WINDOW(SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags,
				ref PInvokeUtils.RECT frame, MulticastDelegate delegt, IntPtr delegateParam, IntPtr parent);

			/// <summary>
			/// VOID	function(HWINDOW hwndOrNull, LPVOID param, DEBUG_OUTPUT_PROC     pfOutput) SciterSetupDebugOutput;
			/// </summary>
			/// <param name="hwndOrNull">HWINDOW or null if this is global output handler</param>
			/// <param name="param">param to be passed "as is" to the pfOutput</param>
			/// <param name="pfOutput">output function, output stream alike thing.</param>
			public delegate void SCITER_SETUP_DEBUG_OUTPUT(IntPtr hwndOrNull, IntPtr param,
				SciterXDef.DEBUG_OUTPUT_PROC pfOutput);

			#region DOM Element API

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) Sciter_UseElement;
			/// </summary>
			/// <param name="he"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_USE_ELEMENT(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) Sciter_UnuseElement;
			/// </summary>
			/// <param name="he"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_UNUSE_ELEMENT(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HWINDOW hwnd, HELEMENT *phe) SciterGetRootElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="phe"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ROOT_ELEMENT(IntPtr hwnd, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HWINDOW hwnd, HELEMENT *phe) SciterGetFocusElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="phe"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_FOCUS_ELEMENT(IntPtr hwnd, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HWINDOW hwnd, POINT pt, HELEMENT* phe) SciterFindElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="pt"></param>
			/// <param name="phe"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_FIND_ELEMENT(IntPtr hwnd, PInvokeUtils.POINT pt,
				out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT* count) SciterGetChildrenCount;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="count"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_CHILDREN_COUNT(IntPtr he, out uint count);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT n, HELEMENT* phe) SciterGetNthChild;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="n"></param>
			/// <param name="phe"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_NTH_CHILD(IntPtr he, uint n, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, HELEMENT* p_parent_he) SciterGetParentElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pParentHe"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_PARENT_ELEMENT(IntPtr he, out IntPtr pParentHe);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, BOOL outer, LPCBYTE_RECEIVER rcv, LPVOID rcv_param) SciterGetElementHtmlCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="outer"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_HTML_CB(IntPtr he, bool outer,
				SciterXDom.LPCBYTE_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetElementTextCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_TEXT_CB(IntPtr he,
				SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR utf16, UINT length) SciterSetElementText;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="utf16"></param>
			/// <param name="length"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_ELEMENT_TEXT(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string utf16, uint length);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPUINT p_count) SciterGetAttributeCount;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pCount"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ATTRIBUTE_COUNT(IntPtr he, out uint pCount);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT n, LPCSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetNthAttributeNameCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="n"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_NTH_ATTRIBUTE_NAME_CB(IntPtr he, uint n,
				SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT n, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetNthAttributeValueCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="n"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_NTH_ATTRIBUTE_VALUE_CB(IntPtr he, uint n,
				SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetAttributeByNameCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ATTRIBUTE_BY_NAME_CB(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR value) SciterSetAttributeByName;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="value"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_ATTRIBUTE_BY_NAME(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterClearAttributes;
			/// </summary>
			/// <param name="he"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_CLEAR_ATTRIBUTES(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPUINT p_index) SciterGetElementIndex;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pIndex"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_INDEX(IntPtr he, out uint pIndex);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR* p_type) SciterGetElementType;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pType"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_TYPE(IntPtr he, out IntPtr pType);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetElementTypeCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_TYPE_CB(IntPtr he,
				SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetStyleAttributeCB;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_STYLE_ATTRIBUTE_CB(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR value) SciterSetStyleAttribute;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="value"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_STYLE_ATTRIBUTE(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPRECT p_location, UINT areas /*ELEMENT_AREAS*/) SciterGetElementLocation;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pLocation"></param>
			/// <param name="areas"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_LOCATION(IntPtr he,
				out PInvokeUtils.RECT pLocation, SciterXDom.ELEMENT_AREAS areas);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT SciterScrollFlags) SciterScrollToView;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="sciterScrollFlags"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SCROLL_TO_VIEW(IntPtr he, uint sciterScrollFlags);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, BOOL andForceRender) SciterUpdateElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="andForceRender"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_UPDATE_ELEMENT(IntPtr he, bool andForceRender);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, RECT rc) SciterRefreshElementArea;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="rc"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_REFRESH_ELEMENT_AREA(IntPtr he, PInvokeUtils.RECT rc);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterSetCapture;
			/// </summary>
			/// <param name="he"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_CAPTURE(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterReleaseCapture;
			/// </summary>
			/// <param name="he"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_RELEASE_CAPTURE(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, HWINDOW* p_hwnd, BOOL rootWindow) SciterGetElementHwnd;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pHwnd"></param>
			/// <param name="rootWindow"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_HWND(IntPtr he, out IntPtr pHwnd,
				bool rootWindow);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPWSTR szUrlBuffer, UINT UrlBufferSize) SciterCombineURL;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="szUrlBuffer"></param>
			/// <param name="urlBufferSize"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_COMBINE_URL(IntPtr he, /*[MarshalAs(UnmanagedType.LPWStr)]*/
				IntPtr szUrlBuffer, uint urlBufferSize);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT  he, LPCSTR    CSS_selectors, SciterElementCallback callback, LPVOID param) SciterSelectElements;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="cssSelectors"></param>
			/// <param name="callback"></param>
			/// <param name="param"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SELECT_ELEMENTS(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string cssSelectors, SciterXDom.SCITER_ELEMENT_CALLBACK callback,
				IntPtr param);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT  he, LPCWSTR   CSS_selectors, SciterElementCallback callback, LPVOID param) SciterSelectElementsW;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="cssSelectors"></param>
			/// <param name="callback"></param>
			/// <param name="param"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SELECT_ELEMENTS_W(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string cssSelectors, SciterXDom.SCITER_ELEMENT_CALLBACK callback,
				IntPtr param);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT  he, LPCSTR    selector, UINT      depth, HELEMENT* heFound) SciterSelectParent;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="selector"></param>
			/// <param name="depth"></param>
			/// <param name="heFound"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SELECT_PARENT(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string selector, uint depth, out IntPtr heFound);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR selector, UINT depth, HELEMENT* heFound) SciterSelectParentW;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="selector"></param>
			/// <param name="depth"></param>
			/// <param name="heFound"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SELECT_PARENT_W(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string selector, uint depth, out IntPtr heFound);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, const BYTE* html, UINT htmlLength, UINT where) SciterSetElementHtml;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="html"></param>
			/// <param name="htmlLength"></param>
			/// <param name="where"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_ELEMENT_HTML(IntPtr he, byte[] html, uint htmlLength,
				SciterXDom.SET_ELEMENT_HTML where);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, UINT* puid) SciterGetElementUID;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="puid"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_UID(IntPtr he, out uint puid);

			/// <summary>
			/// SCDOM_RESULT function(HWINDOW hwnd, UINT uid, HELEMENT* phe) SciterGetElementByUID;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="uid"></param>
			/// <param name="phe"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_BY_UID(IntPtr hwnd, uint uid, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT hePopup, HELEMENT heAnchor, UINT placement) SciterShowPopup;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="heAnchor"></param>
			/// <param name="placement"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SHOW_POPUP(IntPtr he, IntPtr heAnchor, uint placement);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT hePopup, POINT pos, UINT placement) SciterShowPopupAt;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pos"></param>
			/// <param name="placement"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SHOW_POPUP_AT(IntPtr he, PInvokeUtils.POINT pos,
				uint placement);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterHidePopup;
			/// </summary>
			/// <param name="he"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_HIDE_POPUP(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT* pstateBits) SciterGetElementState;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pstateBits"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_STATE(IntPtr he, out uint pstateBits);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT stateBitsToSet, UINT stateBitsToClear, BOOL updateView) SciterSetElementState;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="stateBitsToSet"></param>
			/// <param name="stateBitsToClear"></param>
			/// <param name="updateView"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_ELEMENT_STATE(IntPtr he, uint stateBitsToSet,
				uint stateBitsToClear, bool updateView);

			/// <summary>
			/// SCDOM_RESULT function( LPCSTR tagname, LPCWSTR textOrNull, /*out*/ HELEMENT *phe ) SciterCreateElement;
			/// </summary>
			/// <param name="tagname"></param>
			/// <param name="textOrNull"></param>
			/// <param name="phe"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_CREATE_ELEMENT(
				[MarshalAs(UnmanagedType.LPStr)] string tagname, [MarshalAs(UnmanagedType.LPWStr)] string textOrNull,
				out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, /*out*/ HELEMENT *phe ) SciterCloneElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="phe"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_CLONE_ELEMENT(IntPtr he, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, HELEMENT hparent, UINT index ) SciterInsertElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="hparent"></param>
			/// <param name="index"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_INSERT_ELEMENT(IntPtr he, IntPtr hparent, uint index);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he ) SciterDetachElement;
			/// </summary>
			/// <param name="he"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_DETACH_ELEMENT(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he) SciterDeleteElement;
			/// </summary>
			/// <param name="he"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_DELETE_ELEMENT(IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT milliseconds, UINT_PTR timer_id ) SciterSetTimer;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="milliseconds"></param>
			/// <param name="timerId"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_TIMER(IntPtr he, uint milliseconds, IntPtr timerId);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPELEMENT_EVENT_PROC pep, LPVOID tag ) SciterDetachEventHandler;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pep"></param>
			/// <param name="tag"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_DETACH_EVENT_HANDLER(IntPtr he, MulticastDelegate pep,
				IntPtr tag);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPELEMENT_EVENT_PROC pep, LPVOID tag ) SciterAttachEventHandler;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pep"></param>
			/// <param name="tag"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_ATTACH_EVENT_HANDLER(IntPtr he, MulticastDelegate pep,
				IntPtr tag);

			/// <summary>
			/// SCDOM_RESULT function( HWINDOW hwndLayout, LPELEMENT_EVENT_PROC pep, LPVOID tag, UINT subscription ) SciterWindowAttachEventHandler;
			/// </summary>
			/// <param name="hwndLayout"></param>
			/// <param name="pep"></param>
			/// <param name="tag"></param>
			/// <param name="subscription"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_WINDOW_ATTACH_EVENT_HANDLER(IntPtr hwndLayout,
				MulticastDelegate pep, IntPtr tag, uint subscription);

			/// <summary>
			/// SCDOM_RESULT function( HWINDOW hwndLayout, LPELEMENT_EVENT_PROC pep, LPVOID tag ) SciterWindowDetachEventHandler;
			/// </summary>
			/// <param name="hwndLayout"></param>
			/// <param name="pep"></param>
			/// <param name="tag"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_WINDOW_DETACH_EVENT_HANDLER(IntPtr hwndLayout,
				MulticastDelegate pep, IntPtr tag);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT appEventCode, HELEMENT heSource, UINT_PTR reason, /*out*/ BOOL* handled) SciterSendEvent;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="appEventCode"></param>
			/// <param name="heSource"></param>
			/// <param name="reason"></param>
			/// <param name="handled"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SEND_EVENT(IntPtr he, uint appEventCode, IntPtr heSource,
				IntPtr reason, out bool handled);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT appEventCode, HELEMENT heSource, UINT_PTR reason) SciterPostEvent;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="appEventCode"></param>
			/// <param name="heSource"></param>
			/// <param name="reason"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_POST_EVENT(IntPtr he, uint appEventCode, IntPtr heSource,
				IntPtr reason);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, METHOD_PARAMS* params) SciterCallBehaviorMethod;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="param"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_CALL_BEHAVIOR_METHOD(IntPtr he,
				ref SciterXDom.METHOD_PARAMS param);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, LPCWSTR url, UINT dataType, HELEMENT initiator) SciterRequestElementData;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="url"></param>
			/// <param name="dataType"></param>
			/// <param name="initiator"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_REQUEST_ELEMENT_DATA(IntPtr he,
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
			public delegate SciterXDom.SCDOM_RESULT SCITER_HTTP_REQUEST(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string url, uint dataType, uint requestType,
				ref SciterXDom.REQUEST_PARAM requestParams, uint nParams);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPPOINT scrollPos, LPRECT viewRect, LPSIZE contentSize ) SciterGetScrollInfo;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="scrollPos"></param>
			/// <param name="viewRect"></param>
			/// <param name="contentSize"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_SCROLL_INFO(IntPtr he, out PInvokeUtils.POINT scrollPos,
				out PInvokeUtils.RECT viewRect, out PInvokeUtils.SIZE contentSize);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, POINT scrollPos, BOOL smooth ) SciterSetScrollPos;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="scrollPos"></param>
			/// <param name="smooth"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_SCROLL_POS(IntPtr he, PInvokeUtils.POINT scrollPos,
				bool smooth);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, INT* pMinWidth, INT* pMaxWidth ) SciterGetElementIntrinsicWidths;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pMinWidth"></param>
			/// <param name="pMaxWidth"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_INTRINSIC_WIDTHS(IntPtr he, out int pMinWidth,
				out int pMaxWidth);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, INT forWidth, INT* pHeight ) SciterGetElementIntrinsicHeight;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="forWidth"></param>
			/// <param name="pHeight"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_INTRINSIC_HEIGHT(IntPtr he, int forWidth,
				out int pHeight);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, BOOL* pVisible) SciterIsElementVisible;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pVisible"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_IS_ELEMENT_VISIBLE(IntPtr he, out bool pVisible);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, BOOL* pEnabled ) SciterIsElementEnabled;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pEnabled"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_IS_ELEMENT_ENABLED(IntPtr he, out bool pEnabled);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, UINT firstIndex, UINT lastIndex, ELEMENT_COMPARATOR* cmpFunc, LPVOID cmpFuncParam ) SciterSortElements;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="firstIndex"></param>
			/// <param name="lastIndex"></param>
			/// <param name="cmpFunc"></param>
			/// <param name="cmpFuncParam"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SORT_ELEMENTS(IntPtr he, uint firstIndex, uint lastIndex,
				SciterXDom.ELEMENT_COMPARATOR cmpFunc, IntPtr cmpFuncParam);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he1, HELEMENT he2 ) SciterSwapElements;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="he2"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SWAP_ELEMENTS(IntPtr he, IntPtr he2);

			/// <summary>
			/// SCDOM_RESULT function( UINT evt, LPVOID eventCtlStruct, BOOL* bOutProcessed ) SciterTraverseUIEvent;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="eventCtlStruct"></param>
			/// <param name="bOutProcessed"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_TRAVERSE_UI_EVENT(IntPtr he, IntPtr eventCtlStruct,
				out bool bOutProcessed);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPCSTR name, const VALUE* argv, UINT argc, VALUE* retval ) SciterCallScriptingMethod;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="name"></param>
			/// <param name="argv"></param>
			/// <param name="argc"></param>
			/// <param name="retval"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_CALL_SCRIPTING_METHOD(IntPtr he,
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
			public delegate SciterXDom.SCDOM_RESULT SCITER_CALL_SCRIPTING_FUNCTION(IntPtr he,
				[MarshalAs(UnmanagedType.LPStr)] string name, SciterValue.VALUE[] argv, uint argc,
				out SciterValue.VALUE retval);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, LPCWSTR script, UINT scriptLength, VALUE* retval ) SciterEvalElementScript;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="script"></param>
			/// <param name="scriptLength"></param>
			/// <param name="retval"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_EVAL_ELEMENT_SCRIPT(IntPtr he,
				[MarshalAs(UnmanagedType.LPWStr)] string script, uint scriptLength, out SciterValue.VALUE retval);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, HWINDOW hwnd) SciterAttachHwndToElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="hwnd"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_ATTACH_HWND_TO_ELEMENT(IntPtr he, IntPtr hwnd);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, /*CTL_TYPE*/ UINT *pType ) SciterControlGetType;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pType"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_CONTROL_GET_TYPE(IntPtr he, out uint pType);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, VALUE* pval ) SciterGetValue;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_VALUE(IntPtr he, out SciterValue.VALUE pval);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, const VALUE* pval ) SciterSetValue;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_VALUE(IntPtr he, ref SciterValue.VALUE pval);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, VALUE* pval, BOOL forceCreation ) SciterGetExpando;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			/// <param name="forceCreation"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_EXPANDO(IntPtr he, out SciterValue.VALUE pval,
				bool forceCreation);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, tiscript_value* pval, BOOL forceCreation ) SciterGetObject;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			/// <param name="forceCreation"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_OBJECT(IntPtr he, out TIScript.tiscript_value pval,
				bool forceCreation);

			/// <summary>
			/// SCDOM_RESULT function( HELEMENT he, tiscript_value* pval) SciterGetElementNamespace;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="pval"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_NAMESPACE(IntPtr he,
				out TIScript.tiscript_value pval);

			/// <summary>
			/// SCDOM_RESULT function( HWINDOW hwnd, HELEMENT* phe) SciterGetHighlightedElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="phe"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_HIGHLIGHTED_ELEMENT(IntPtr hwnd, out IntPtr phe);

			/// <summary>
			/// SCDOM_RESULT function( HWINDOW hwnd, HELEMENT he) SciterSetHighlightedElement;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="he"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_HIGHLIGHTED_ELEMENT(IntPtr hwnd, IntPtr he);

			#endregion

			#region DOM Node API

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn) SciterNodeAddRef;
			/// </summary>
			/// <param name="hn"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_ADD_REF(IntPtr hn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn) SciterNodeRelease;
			/// </summary>
			/// <param name="hn"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_RELEASE(IntPtr hn);

			/// <summary>
			/// SCDOM_RESULT function(HELEMENT he, HNODE* phn) SciterNodeCastFromElement;
			/// </summary>
			/// <param name="he"></param>
			/// <param name="phn"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_CAST_FROM_ELEMENT(IntPtr he, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HELEMENT* he) SciterNodeCastToElement;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="he"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_CAST_TO_ELEMENT(IntPtr hn, out IntPtr he);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodeFirstChild;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="phn"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_FIRST_CHILD(IntPtr hn, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodeLastChild;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="phn"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_LAST_CHILD(IntPtr hn, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodeNextSibling;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="phn"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_NEXT_SIBLING(IntPtr hn, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodePrevSibling;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="phn"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_PREV_SIBLING(IntPtr hn, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, HELEMENT* pheParent) SciterNodeParent;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="pheParent"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_PARENT(IntPtr hn, out IntPtr pheParent);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, UINT n, HNODE* phn) SciterNodeNthChild;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="n"></param>
			/// <param name="phn"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_NTH_CHILD(IntPtr hn, uint n, out IntPtr phn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, UINT* pn) SciterNodeChildrenCount;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="pn"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_CHILDREN_COUNT(IntPtr hn, out uint pn);

			/// <summary>
			/// /SCDOM_RESULT function(HNODE hnode, UINT* pNodeType /*NODE_TYPE*/) SciterNodeType;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="pn"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_TYPE(IntPtr hn, out SciterXDom.NODE_TYPE pn);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterNodeGetText;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="rcv"></param>
			/// <param name="rcvParam"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_GET_TEXT(IntPtr hn, SciterXDom.LPCWSTR_RECEIVER rcv,
				IntPtr rcvParam);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, LPCWSTR text, UINT textLength) SciterNodeSetText;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="text"></param>
			/// <param name="textLength"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_SET_TEXT(IntPtr hn,
				[MarshalAs(UnmanagedType.LPWStr)] string text, uint textLength);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, UINT where /*NODE_INS_TARGET*/, HNODE what) SciterNodeInsert;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="where"></param>
			/// <param name="what"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_INSERT(IntPtr hn, uint where, IntPtr what);

			/// <summary>
			/// SCDOM_RESULT function(HNODE hnode, BOOL finalize) SciterNodeRemove;
			/// </summary>
			/// <param name="hn"></param>
			/// <param name="finalize"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_REMOVE(IntPtr hn, bool finalize);

			/// <summary>
			/// SCDOM_RESULT function(LPCWSTR text, UINT textLength, HNODE* phnode) SciterCreateTextNode;
			/// </summary>
			/// <param name="text"></param>
			/// <param name="textLength"></param>
			/// <param name="phnode"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_CREATE_TEXT_NODE(
				[MarshalAs(UnmanagedType.LPWStr)] string text, uint textLength, out IntPtr phnode);

			/// <summary>
			/// SCDOM_RESULT function(LPCWSTR text, UINT textLength, HNODE* phnode) SciterCreateCommentNode;
			/// </summary>
			/// <param name="text"></param>
			/// <param name="textLength"></param>
			/// <param name="phnode"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_CREATE_COMMENT_NODE(
				[MarshalAs(UnmanagedType.LPWStr)] string text, uint textLength, out IntPtr phnode);

			#endregion

			#region Value API

			/// <summary>
			/// UINT function( VALUE* pval ) ValueInit;
			/// </summary>
			/// <param name="pval"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_INIT(out SciterValue.VALUE pval);

			/// <summary>
			/// UINT function( VALUE* pval ) ValueClear;
			/// </summary>
			/// <param name="pval"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_CLEAR(out SciterValue.VALUE pval);

			/// <summary>
			/// UINT function( const VALUE* pval1, const VALUE* pval2 ) ValueCompare;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pval2"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_COMPARE(ref SciterValue.VALUE pval, ref IntPtr pval2);

			/// <summary>
			/// UINT function( VALUE* pdst, const VALUE* psrc ) ValueCopy;
			/// </summary>
			/// <param name="pdst"></param>
			/// <param name="psrc"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_COPY(out SciterValue.VALUE pdst, ref SciterValue.VALUE psrc);

			/// <summary>
			/// UINT function( VALUE* pdst ) ValueIsolate;
			/// </summary>
			/// <param name="pdst"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_ISOLATE(ref SciterValue.VALUE pdst);

			/// <summary>
			/// UINT function( const VALUE* pval, UINT* pType, UINT* pUnits ) ValueType;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pType"></param>
			/// <param name="pUnits"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_TYPE(ref SciterValue.VALUE pval, out uint pType,
				out uint pUnits);

			/// <summary>
			/// UINT function( const VALUE* pval, LPCWSTR* pChars, UINT* pNumChars ) ValueStringData;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pChars"></param>
			/// <param name="pNumChars"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_STRING_DATA(ref SciterValue.VALUE pval, out IntPtr pChars,
				out uint pNumChars);

			/// <summary>
			/// UINT function( VALUE* pval, LPCWSTR chars, UINT numChars, UINT units ) ValueStringDataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="chars"></param>
			/// <param name="numChars"></param>
			/// <param name="units"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_STRING_DATA_SET(ref SciterValue.VALUE pval,
				[MarshalAs(UnmanagedType.LPWStr)] string chars, uint numChars, uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, INT* pData ) ValueIntData;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pData"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_INT_DATA(ref SciterValue.VALUE pval, out int pData);

			/// <summary>
			/// UINT function( VALUE* pval, INT data, UINT type, UINT units ) ValueIntDataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="data"></param>
			/// <param name="type"></param>
			/// <param name="units"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_INT_DATA_SET(ref SciterValue.VALUE pval, int data, uint type,
				uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, INT64* pData ) ValueInt64Data;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pData"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_INT_64DATA(ref SciterValue.VALUE pval, out long pData);

			/// <summary>
			/// UINT function( VALUE* pval, INT64 data, UINT type, UINT units ) ValueInt64DataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="data"></param>
			/// <param name="type"></param>
			/// <param name="units"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_INT_64DATA_SET(ref SciterValue.VALUE pval, long data,
				uint type, uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, FLOAT_VALUE* pData ) ValueFloatData;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pData"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_FLOAT_DATA(ref SciterValue.VALUE pval, out double pData);

			/// <summary>
			/// UINT function( VALUE* pval, FLOAT_VALUE data, UINT type, UINT units ) ValueFloatDataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="data"></param>
			/// <param name="type"></param>
			/// <param name="units"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_FLOAT_DATA_SET(ref SciterValue.VALUE pval, double data,
				uint type, uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, LPCBYTE* pBytes, UINT* pnBytes ) ValueBinaryData;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pBytes"></param>
			/// <param name="pnBytes"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_BINARY_DATA(ref SciterValue.VALUE pval, out IntPtr pBytes,
				out uint pnBytes);

			/// <summary>
			/// UINT function( VALUE* pval, LPCBYTE pBytes, UINT nBytes, UINT type, UINT units ) ValueBinaryDataSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pBytes"></param>
			/// <param name="nBytes"></param>
			/// <param name="type"></param>
			/// <param name="units"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_BINARY_DATA_SET(ref SciterValue.VALUE pval,
				[MarshalAs(UnmanagedType.LPArray)] byte[] pBytes, uint nBytes, uint type, uint units);

			/// <summary>
			/// UINT function( const VALUE* pval, INT* pn) ValueElementsCount;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pn"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_ELEMENTS_COUNT(ref SciterValue.VALUE pval, out int pn);

			/// <summary>
			/// UINT function( const VALUE* pval, INT n, VALUE* pretval) ValueNthElementValue;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="n"></param>
			/// <param name="pretval"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_NTH_ELEMENT_VALUE(ref SciterValue.VALUE pval, int n,
				out SciterValue.VALUE pretval);

			/// <summary>
			/// UINT function( VALUE* pval, INT n, const VALUE* pval_to_set) ValueNthElementValueSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="n"></param>
			/// <param name="pvalToSet"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_NTH_ELEMENT_VALUE_SET(ref SciterValue.VALUE pval, int n,
				ref SciterValue.VALUE pvalToSet);

			/// <summary>
			/// UINT function( const VALUE* pval, INT n, VALUE* pretval) ValueNthElementKey;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="n"></param>
			/// <param name="pretval"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_NTH_ELEMENT_KEY(ref SciterValue.VALUE pval, int n,
				out SciterValue.VALUE pretval);

			/// <summary>
			/// UINT function( VALUE* pval, KeyValueCallback* penum, LPVOID param) ValueEnumElements;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="penum"></param>
			/// <param name="param"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_ENUM_ELEMENTS(ref SciterValue.VALUE pval,
				SciterValue.KEY_VALUE_CALLBACK penum, IntPtr param);

			/// <summary>
			/// UINT function( VALUE* pval, const VALUE* pkey, const VALUE* pval_to_set) ValueSetValueToKey;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pkey"></param>
			/// <param name="pvalToSet"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_SET_VALUE_TO_KEY(ref SciterValue.VALUE pval,
				ref SciterValue.VALUE pkey, ref SciterValue.VALUE pvalToSet);

			/// <summary>
			/// UINT function( const VALUE* pval, const VALUE* pkey, VALUE* pretval) ValueGetValueOfKey;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pkey"></param>
			/// <param name="pretval"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_GET_VALUE_OF_KEY(ref SciterValue.VALUE pval,
				ref SciterValue.VALUE pkey, out SciterValue.VALUE pretval);

			/// <summary>
			/// UINT function( VALUE* pval, /*VALUE_STRING_CVT_TYPE*/ UINT how ) ValueToString;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="how"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_TO_STRING(ref SciterValue.VALUE pval,
				SciterValue.VALUE_STRING_CVT_TYPE how);

			/// <summary>
			/// UINT function( VALUE* pval, LPCWSTR str, UINT strLength, /*VALUE_STRING_CVT_TYPE*/ UINT how ) ValueFromString;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="str"></param>
			/// <param name="strLength"></param>
			/// <param name="how"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_FROM_STRING(ref SciterValue.VALUE pval,
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
			public delegate SciterValue.VALUE_RESULT VALUE_INVOKE(ref SciterValue.VALUE pval,
				ref SciterValue.VALUE pthis, uint argc, SciterValue.VALUE[] argv, out SciterValue.VALUE pretval,
				[MarshalAs(UnmanagedType.LPWStr)] string url);

			/// <summary>
			/// UINT function( VALUE* pval, NATIVE_FUNCTOR_INVOKE*  pinvoke, NATIVE_FUNCTOR_RELEASE* prelease, VOID* tag) ValueNativeFunctorSet;
			/// </summary>
			/// <param name="pval"></param>
			/// <param name="pinvoke"></param>
			/// <param name="prelease"></param>
			/// <param name="tag"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_NATIVE_FUNCTOR_SET(ref SciterValue.VALUE pval,
				SciterValue.NATIVE_FUNCTOR_INVOKE pinvoke, SciterValue.NATIVE_FUNCTOR_RELEASE prelease, IntPtr tag);

			/// <summary>
			/// BOOL function( const VALUE* pval) ValueIsNativeFunctor;
			/// </summary>
			/// <param name="pval"></param>
			public delegate SciterValue.VALUE_RESULT VALUE_IS_NATIVE_FUNCTOR(ref SciterValue.VALUE pval);

			#endregion

			#region TIScript

			/// <summary>
			/// tiscript_native_interface* function() TIScriptAPI;
			/// </summary>
			public delegate IntPtr TI_SCRIPT_API();

			#endregion

			/// <summary>
			/// HVM function(HWINDOW hwnd) SciterGetVM;
			/// </summary>
			/// <param name="hwnd"></param>
			public delegate IntPtr SCITER_GET_VM(IntPtr hwnd);

			/// <summary>
			/// BOOL function(HVM vm, tiscript_value script_value, VALUE* value, BOOL isolate) Sciter_v2V;
			/// </summary>
			/// <param name="vm"></param>
			/// <param name="scriptValue"></param>
			/// <param name="value"></param>
			/// <param name="isolate"></param>
			public delegate bool SCITER_v2V(IntPtr vm, TIScript.tiscript_value scriptValue, ref SciterValue.VALUE value,
				bool isolate);

			/// <summary>
			/// BOOL function(HVM vm, const VALUE* valuev, tiscript_value* script_value) Sciter_V2v;
			/// </summary>
			/// <param name="vm"></param>
			/// <param name="value"></param>
			/// <param name="scriptValue"></param>
			public delegate bool SCITER_V2v(IntPtr vm, ref SciterValue.VALUE value,
				ref TIScript.tiscript_value scriptValue);

			#region Archive

			/// <summary>
			/// HSARCHIVE function(LPCBYTE archiveData, UINT archiveDataLength) SciterOpenArchive;
			/// </summary>
			/// <param name="archiveData"></param>
			/// <param name="archiveDataLength"></param>
			public delegate IntPtr
				SCITER_OPEN_ARCHIVE(IntPtr archiveData,
					uint archiveDataLength); // archiveData must point to a pinned byte[] array!

			/// <summary>
			/// BOOL function(HSARCHIVE harc, LPCWSTR path, LPCBYTE* pdata, UINT* pdataLength) SciterGetArchiveItem;
			/// </summary>
			/// <param name="harc"></param>
			/// <param name="path"></param>
			/// <param name="pdata"></param>
			/// <param name="pdataLength"></param>
			public delegate bool SCITER_GET_ARCHIVE_ITEM(IntPtr harc, [MarshalAs(UnmanagedType.LPWStr)] string path,
				out IntPtr pdata, out uint pdataLength);

			/// <summary>
			/// BOOL function(HSARCHIVE harc) SciterCloseArchive;
			/// </summary>
			/// <param name="harc"></param>
			public delegate bool SCITER_CLOSE_ARCHIVE(IntPtr harc);

			#endregion

			/// <summary>
			/// SCDOM_RESULT function( const BEHAVIOR_EVENT_PARAMS* evt, BOOL post, BOOL *handled ) SciterFireEvent;
			/// </summary>
			/// <param name="evt"></param>
			/// <param name="post"></param>
			/// <param name="handled"></param>
			public delegate SciterXDom.SCDOM_RESULT SCITER_FIRE_EVENT(ref SciterBehaviors.BEHAVIOR_EVENT_PARAMS evt,
				bool post, out bool handled);

			/// <summary>
			/// LPVOID function(HWINDOW hwnd) SciterGetCallbackParam;
			/// </summary>
			/// <param name="hwnd"></param>
			public delegate IntPtr SCITER_GET_CALLBACK_PARAM(IntPtr hwnd);

			/// <summary>
			/// UINT_PTR function(HWINDOW hwnd, UINT_PTR wparam, UINT_PTR lparam, UINT timeoutms) SciterPostCallback;
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="wparam"></param>
			/// <param name="lparam"></param>
			/// <param name="timeoutms">if > 0 then it is a send, not a post</param>
			public delegate IntPtr SCITER_POST_CALLBACK(IntPtr hwnd, IntPtr wparam, IntPtr lparam, uint timeoutms);

			/// <summary>
			/// LPSciterGraphicsAPI function() GetSciterGraphicsAPI;
			/// </summary>
			public delegate IntPtr GET_SCITER_GRAPHICS_API();

			/// <summary>
			/// LPSciterRequestAPI SCFN(GetSciterRequestAPI )();
			/// </summary>
			public delegate IntPtr GET_SCITER_REQUEST_API();

			#region DirectX API

			/// <summary>
			/// BOOL SCFN(SciterCreateOnDirectXWindow ) (HWINDOW hwnd, IDXGISwapChain* pSwapChain);
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="pSwapChain"></param>
			public delegate bool SCITER_CREATE_ON_DIRECT_X_WINDOW(IntPtr hwnd, IntPtr pSwapChain);

			/// <summary>
			/// BOOL SCFN(SciterRenderOnDirectXWindow ) (HWINDOW hwnd, HELEMENT elementToRenderOrNull, BOOL frontLayer);
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="elementToRenderOrNull"></param>
			/// <param name="frontLayer"></param>
			public delegate bool SCITER_RENDER_ON_DIRECT_X_WINDOW(IntPtr hwnd, IntPtr elementToRenderOrNull,
				bool frontLayer);

			/// <summary>
			/// BOOL SCFN(SciterRenderOnDirectXTexture ) (HWINDOW hwnd, HELEMENT elementToRenderOrNull, IDXGISurface* surface);
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="elementToRenderOrNull"></param>
			/// <param name="surface"></param>
			public delegate bool SCITER_RENDER_ON_DIRECT_X_TEXTURE(IntPtr hwnd, IntPtr elementToRenderOrNull,
				IntPtr surface);

			#endregion

			/// <summary>
			/// BOOL SCFN(SciterProcX)(HWINDOW hwnd, SCITER_X_MSG* pMsg );
			/// </summary>
			/// <param name="hwnd"></param>
			/// <param name="pMsg"></param>
			/// <returns>TRUE if handled</returns>
			public delegate bool SCITER_PROC_X(IntPtr hwnd, IntPtr pMsg);
		}
	}
}