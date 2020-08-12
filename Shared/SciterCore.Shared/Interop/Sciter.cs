// Copyright 2016 Ramon F. Mendes
//
// This file is part of SciterSharp.
// 
// SciterSharp is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// SciterSharp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with SciterSharp.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore.Interop
{
	public static class Sciter
	{
		public static SciterApi Api => GetSciterApi();

		public static SciterGraphics.SciterGraphicsApi GraphicsApi => GetGraphicsApi();

		public static SciterRequest.SciterRequestApi RequestApi => GetRequestApi();

		public static TIScript.SCITER_TI_SCRIPT_API ScriptApi => GetScriptApi();

        public static Version Version()
		{
			var api = Api;
			uint major = api.SciterVersion(true);
			uint minor = api.SciterVersion(false);

			return new Version(
				(int)((major >> 16) & 0xffff),
				(int)(major & 0xffff),
				(int)((minor >> 16) & 0xffff),
				(int)(minor & 0xffff)
                );
		}


		private static SciterApi? _sciterApi = null;
		private static SciterGraphics.SciterGraphicsApi? _sciterGraphicsApi = null;
		private static SciterRequest.SciterRequestApi? _sciterRequestApiInstance = null;
		private static TIScript.SCITER_TI_SCRIPT_API? _sciterScriptApi = null;

		//#if WINDOWS
		//		public static bool Use3264DLLNaming { get; set; }

		//		[DllImport("sciter", EntryPoint = "SciterAPI")]
		//		private static extern IntPtr SciterAPI();

		//		[DllImport("sciter32", EntryPoint = "SciterAPI")]
		//		private static extern IntPtr SciterAPI32();

		//		[DllImport("sciter64", EntryPoint = "SciterAPI")]
		//		private static extern IntPtr SciterAPI64();
		//#elif GTKMONO
		//		[DllImport("sciter-gtk-64.so")]
		//		private static extern IntPtr SciterAPI();
		//#elif OSX
		//		[DllImport("sciter-osx-64", EntryPoint = "SciterAPI")]
		//		private static extern IntPtr SciterAPI64();
		//#endif
#if WINDOWS || NETCORE
		[DllImport("sciter.dll", EntryPoint = "SciterAPI")]
#elif GTKMONO
		[DllImport("x64\\sciter-gtk-64.so")]
#elif OSX
		[DllImport("x64\\sciter-osx-64.dylib", EntryPoint = "SciterAPI")]
#endif
		private static extern IntPtr SciterAPI();

		private static SciterApi GetSciterApi()
		{
			if(_sciterApi==null)
			{
				int apiStructSize = Marshal.SizeOf(typeof(SciterApi));
				IntPtr apiPtr;

                
#if WINDOWS || NETCORE
				var codeBasePath = new Uri(typeof(SciterApi).Assembly.CodeBase).LocalPath;
                var codeBaseDirectory = Path.GetDirectoryName(codeBasePath);
                var is64 = Environment.Is64BitProcess;
                var bitDirectory = is64 ? "x64" : "x86";
                var libName = Path.Combine(codeBaseDirectory, bitDirectory, "sciter.dll");
                
                PInvokeWindows.LoadLibrary(libName);
#elif GTKMONO
				if(IntPtr.Size != 8)
					throw new Exception("SciterSharp GTK/Mono only supports 64bits builds");

				Debug.Assert(apiStructSize == 1304);
#elif OSX
				Debug.Assert(IntPtr.Size == 8);
				//Debug.Assert(apiStructSize == 648 * 2);
#endif
				apiPtr = SciterAPI();

				_sciterApi = (SciterApi)Marshal.PtrToStructure(ptr: apiPtr, structureType: typeof(SciterApi));

				// from time to time, Sciter changes its ABI
				// here we test the minimum Sciter version this library is compatible with
				uint major = _sciterApi.Value.SciterVersion(true);
				uint minor = _sciterApi.Value.SciterVersion(false);
				Debug.Assert(major >= 0x00040000);
				Debug.Assert(_sciterApi.Value.version>=0);
			}

			return _sciterApi.Value;
		}

		private static SciterGraphics.SciterGraphicsApi GetGraphicsApi()
		{
			if(_sciterGraphicsApi == null)
			{
				uint major = Api.SciterVersion(true);
				uint minor = Api.SciterVersion(false);
				Debug.Assert(major >= 0x00040000);

				int apiStructSize = Marshal.SizeOf(t: typeof(SciterGraphics.SciterGraphicsApi));
				
				if(IntPtr.Size == 8)
					Debug.Assert(apiStructSize == 276 * 2);
				else
					Debug.Assert(apiStructSize == 276);

				IntPtr apiPtr = Api.GetSciterGraphicsAPI();
				_sciterGraphicsApi = (SciterGraphics.SciterGraphicsApi)Marshal.PtrToStructure(ptr: apiPtr, structureType: typeof(SciterGraphics.SciterGraphicsApi));
			}
			return _sciterGraphicsApi.Value;
		}

		private static SciterRequest.SciterRequestApi GetRequestApi()
		{
			if(_sciterRequestApiInstance == null)
			{
				int apiStructSize = Marshal.SizeOf(t: typeof(SciterRequest.SciterRequestApi));
				
				if(IntPtr.Size == 8)
					Debug.Assert(apiStructSize == 104*2);
				else
					Debug.Assert(apiStructSize == 104);

				IntPtr apiPtr = Api.GetSciterRequestAPI();
				_sciterRequestApiInstance = (SciterRequest.SciterRequestApi)Marshal.PtrToStructure(ptr: apiPtr, structureType: typeof(SciterRequest.SciterRequestApi));
			}
			return _sciterRequestApiInstance.Value;
		}

		private static TIScript.SCITER_TI_SCRIPT_API GetScriptApi()
		{
			if(_sciterScriptApi == null)
			{
				int apiStructSize = Marshal.SizeOf(typeof(TIScript.SCITER_TI_SCRIPT_API));
				if(IntPtr.Size == 8)
					Debug.Assert(apiStructSize == 616);
				else
					Debug.Assert(apiStructSize == 308);

				IntPtr apiPtr = Api.TIScriptAPI();
                _sciterScriptApi = (TIScript.SCITER_TI_SCRIPT_API)Marshal.PtrToStructure(ptr: apiPtr, structureType: typeof(TIScript.SCITER_TI_SCRIPT_API));
			}
			return _sciterScriptApi.Value;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SciterApi
		{
			public readonly int version;
			public readonly SCITER_CLASS_NAME SciterClassName;
			public readonly SCITER_VERSION SciterVersion;
			public readonly SCITER_DATA_READY SciterDataReady;
			public readonly SCITER_DATA_READY_ASYNC SciterDataReadyAsync;
#if WINDOWS || NETCORE
			public readonly SCITER_PROC SciterProc;
			public readonly SCITER_PROC_ND SciterProcND;
#endif
			public readonly SCITER_LOAD_FILE SciterLoadFile;
			public readonly SCITER_LOAD_HTML SciterLoadHtml;
			public readonly SCITER_SET_CALLBACK SciterSetCallback;
			public readonly SCITER_SET_MASTER_CSS SciterSetMasterCSS;
			public readonly SCITER_APPEND_MASTER_CSS SciterAppendMasterCSS;
			public readonly SCITER_SET_CSS SciterSetCSS;
			public readonly SCITER_SET_MEDIA_TYPE SciterSetMediaType;
			public readonly SCITER_SET_MEDIA_VARS SciterSetMediaVars;
			public readonly SCITER_GET_MIN_WIDTH SciterGetMinWidth;
			public readonly SCITER_GET_MIN_HEIGHT SciterGetMinHeight;
			public readonly SCITER_CALL SciterCall;
			public readonly SCITER_EVAL SciterEval;
			public readonly SCITER_UPDATE_WINDOW SciterUpdateWindow;
#if WINDOWS || NETCORE
			public readonly SCITER_TRANSLATE_MESSAGE SciterTranslateMessage;
#endif
			public readonly SCITER_SET_OPTION SciterSetOption;
			public readonly SCITER_GET_PPI SciterGetPPI;
			public readonly SCITER_GET_VIEW_EXPANDO SciterGetViewExpando;
#if WINDOWS || NETCORE
			public readonly SCITER_RENDER_D2D SciterRenderD2D;
			public readonly SCITER_D2D_FACTORY SciterD2DFactory;
			public readonly SCITER_DW_FACTORY SciterDWFactory;
#endif
			public readonly SCITER_GRAPHICS_CAPS SciterGraphicsCaps;
			public readonly SCITER_SET_HOME_URL SciterSetHomeURL;
#if OSX
			public readonly SCITER_CREATE_NS_VIEW SciterCreateNSView;
#endif
#if GTKMONO
			public readonly SCITER_CREATE_WIDGET SciterCreateWidget;
#endif
			public readonly SCITER_CREATE_WINDOW SciterCreateWindow;
  
            public readonly SCITER_SETUP_DEBUG_OUTPUT SciterSetupDebugOutput;

			// DOM Element API 
			public readonly SCITER_USE_ELEMENT Sciter_UseElement;
			public readonly SCITER_UNUSE_ELEMENT Sciter_UnuseElement;
			public readonly SCITER_GET_ROOT_ELEMENT SciterGetRootElement;
			public readonly SCITER_GET_FOCUS_ELEMENT SciterGetFocusElement;
			public readonly SCITER_FIND_ELEMENT SciterFindElement;
			public readonly SCITER_GET_CHILDREN_COUNT SciterGetChildrenCount;
			public readonly SCITER_GET_NTH_CHILD SciterGetNthChild;
			public readonly SCITER_GET_PARENT_ELEMENT SciterGetParentElement;
			public readonly SCITER_GET_ELEMENT_HTML_CB SciterGetElementHtmlCB;
			public readonly SCITER_GET_ELEMENT_TEXT_CB SciterGetElementTextCB;
			public readonly SCITER_SET_ELEMENT_TEXT SciterSetElementText;
			public readonly SCITER_GET_ATTRIBUTE_COUNT SciterGetAttributeCount;
			public readonly SCITER_GET_NTH_ATTRIBUTE_NAME_CB SciterGetNthAttributeNameCB;
			public readonly SCITER_GET_NTH_ATTRIBUTE_VALUE_CB SciterGetNthAttributeValueCB;
			public readonly SCITER_GET_ATTRIBUTE_BY_NAME_CB SciterGetAttributeByNameCB;
			public readonly SCITER_SET_ATTRIBUTE_BY_NAME SciterSetAttributeByName;
			public readonly SCITER_CLEAR_ATTRIBUTES SciterClearAttributes;
			public readonly SCITER_GET_ELEMENT_INDEX SciterGetElementIndex;
			public readonly SCITER_GET_ELEMENT_TYPE SciterGetElementType;
			public readonly SCITER_GET_ELEMENT_TYPE_CB SciterGetElementTypeCB;
			public readonly SCITER_GET_STYLE_ATTRIBUTE_CB SciterGetStyleAttributeCB;
			public readonly SCITER_SET_STYLE_ATTRIBUTE SciterSetStyleAttribute;
			public readonly SCITER_GET_ELEMENT_LOCATION SciterGetElementLocation;
			public readonly SCITER_SCROLL_TO_VIEW SciterScrollToView;
			public readonly SCITER_UPDATE_ELEMENT SciterUpdateElement;
			public readonly SCITER_REFRESH_ELEMENT_AREA SciterRefreshElementArea;
			public readonly SCITER_SET_CAPTURE SciterSetCapture;
			public readonly SCITER_RELEASE_CAPTURE SciterReleaseCapture;
			public readonly SCITER_GET_ELEMENT_HWND SciterGetElementHwnd;
			public readonly SCITER_COMBINE_URL SciterCombineURL;
			public readonly SCITER_SELECT_ELEMENTS SciterSelectElements;
			public readonly SCITER_SELECT_ELEMENTS_W SciterSelectElementsW;
			public readonly SCITER_SELECT_PARENT SciterSelectParent;
			public readonly SCITER_SELECT_PARENT_W SciterSelectParentW;
			public readonly SCITER_SET_ELEMENT_HTML SciterSetElementHtml;
			public readonly SCITER_GET_ELEMENT_UID SciterGetElementUID;
			public readonly SCITER_GET_ELEMENT_BY_UID SciterGetElementByUID;
			public readonly SCITER_SHOW_POPUP SciterShowPopup;
			public readonly SCITER_SHOW_POPUP_AT SciterShowPopupAt;
			public readonly SCITER_HIDE_POPUP SciterHidePopup;
			public readonly SCITER_GET_ELEMENT_STATE SciterGetElementState;
			public readonly SCITER_SET_ELEMENT_STATE SciterSetElementState;
			public readonly SCITER_CREATE_ELEMENT SciterCreateElement;
			public readonly SCITER_CLONE_ELEMENT SciterCloneElement;
			public readonly SCITER_INSERT_ELEMENT SciterInsertElement;
			public readonly SCITER_DETACH_ELEMENT SciterDetachElement;
			public readonly SCITER_DELETE_ELEMENT SciterDeleteElement;
			public readonly SCITER_SET_TIMER SciterSetTimer;
			public readonly SCITER_DETACH_EVENT_HANDLER SciterDetachEventHandler;
			public readonly SCITER_ATTACH_EVENT_HANDLER SciterAttachEventHandler;
			public readonly SCITER_WINDOW_ATTACH_EVENT_HANDLER SciterWindowAttachEventHandler;
			public readonly SCITER_WINDOW_DETACH_EVENT_HANDLER SciterWindowDetachEventHandler;
			public readonly SCITER_SEND_EVENT SciterSendEvent;
			public readonly SCITER_POST_EVENT SciterPostEvent;
			public readonly SCITER_CALL_BEHAVIOR_METHOD SciterCallBehaviorMethod;
			public readonly SCITER_REQUEST_ELEMENT_DATA SciterRequestElementData;
			public readonly SCITER_HTTP_REQUEST SciterHttpRequest;
			public readonly SCITER_GET_SCROLL_INFO SciterGetScrollInfo;
			public readonly SCITER_SET_SCROLL_POS SciterSetScrollPos;
			public readonly SCITER_GET_ELEMENT_INTRINSIC_WIDTHS SciterGetElementIntrinsicWidths;
			public readonly SCITER_GET_ELEMENT_INTRINSIC_HEIGHT SciterGetElementIntrinsicHeight;
			public readonly SCITER_IS_ELEMENT_VISIBLE SciterIsElementVisible;
			public readonly SCITER_IS_ELEMENT_ENABLED SciterIsElementEnabled;
			public readonly SCITER_SORT_ELEMENTS SciterSortElements;
			public readonly SCITER_SWAP_ELEMENTS SciterSwapElements;
			public readonly SCITER_TRAVERSE_UI_EVENT SciterTraverseUIEvent;
			public readonly SCITER_CALL_SCRIPTING_METHOD SciterCallScriptingMethod;
			public readonly SCITER_CALL_SCRIPTING_FUNCTION SciterCallScriptingFunction;
			public readonly SCITER_EVAL_ELEMENT_SCRIPT SciterEvalElementScript;
			public readonly SCITER_ATTACH_HWND_TO_ELEMENT SciterAttachHwndToElement;
			public readonly SCITER_CONTROL_GET_TYPE SciterControlGetType;
			public readonly SCITER_GET_VALUE SciterGetValue;
			public readonly SCITER_SET_VALUE SciterSetValue;
			public readonly SCITER_GET_EXPANDO SciterGetExpando;
			public readonly SCITER_GET_OBJECT SciterGetObject;
			public readonly SCITER_GET_ELEMENT_NAMESPACE SciterGetElementNamespace;
			public readonly SCITER_GET_HIGHLIGHTED_ELEMENT SciterGetHighlightedElement;
			public readonly SCITER_SET_HIGHLIGHTED_ELEMENT SciterSetHighlightedElement;

			// DOM Node API 
			public readonly SCITER_NODE_ADD_REF SciterNodeAddRef;
			public readonly SCITER_NODE_RELEASE SciterNodeRelease;
			public readonly SCITER_NODE_CAST_FROM_ELEMENT SciterNodeCastFromElement;
			public readonly SCITER_NODE_CAST_TO_ELEMENT SciterNodeCastToElement;
			public readonly SCITER_NODE_FIRST_CHILD SciterNodeFirstChild;
			public readonly SCITER_NODE_LAST_CHILD SciterNodeLastChild;
			public readonly SCITER_NODE_NEXT_SIBLING SciterNodeNextSibling;
			public readonly SCITER_NODE_PREV_SIBLING SciterNodePrevSibling;
			public readonly SCITER_NODE_PARENT SciterNodeParent;
			public readonly SCITER_NODE_NTH_CHILD SciterNodeNthChild;
			public readonly SCITER_NODE_CHILDREN_COUNT SciterNodeChildrenCount;
			public readonly SCITER_NODE_TYPE SciterNodeType;
			public readonly SCITER_NODE_GET_TEXT SciterNodeGetText;
			public readonly SCITER_NODE_SET_TEXT SciterNodeSetText;
			public readonly SCITER_NODE_INSERT SciterNodeInsert;
			public readonly SCITER_NODE_REMOVE SciterNodeRemove;
			public readonly SCITER_CREATE_TEXT_NODE SciterCreateTextNode;
			public readonly SCITER_CREATE_COMMENT_NODE SciterCreateCommentNode;

			// Value API 
			public readonly VALUE_INIT ValueInit;
			public readonly VALUE_CLEAR ValueClear;
			public readonly VALUE_COMPARE ValueCompare;
			public readonly VALUE_COPY ValueCopy;
			public readonly VALUE_ISOLATE ValueIsolate;
			public readonly VALUE_TYPE ValueType;
			public readonly VALUE_STRING_DATA ValueStringData;
			public readonly VALUE_STRING_DATA_SET ValueStringDataSet;
			public readonly VALUE_INT_DATA ValueIntData;
			public readonly VALUE_INT_DATA_SET ValueIntDataSet;
			public readonly VALUE_INT_64DATA ValueInt64Data;
			public readonly VALUE_INT_64DATA_SET ValueInt64DataSet;
			public readonly VALUE_FLOAT_DATA ValueFloatData;
			public readonly VALUE_FLOAT_DATA_SET ValueFloatDataSet;
			public readonly VALUE_BINARY_DATA ValueBinaryData;
			public readonly VALUE_BINARY_DATA_SET ValueBinaryDataSet;
			public readonly VALUE_ELEMENTS_COUNT ValueElementsCount;
			public readonly VALUE_NTH_ELEMENT_VALUE ValueNthElementValue;
			public readonly VALUE_NTH_ELEMENT_VALUE_SET ValueNthElementValueSet;
			public readonly VALUE_NTH_ELEMENT_KEY ValueNthElementKey;
			public readonly VALUE_ENUM_ELEMENTS ValueEnumElements;
			public readonly VALUE_SET_VALUE_TO_KEY ValueSetValueToKey;
			public readonly VALUE_GET_VALUE_OF_KEY ValueGetValueOfKey;
			public readonly VALUE_TO_STRING ValueToString;
			public readonly VALUE_FROM_STRING ValueFromString;
			public readonly VALUE_INVOKE ValueInvoke;
			public readonly VALUE_NATIVE_FUNCTOR_SET ValueNativeFunctorSet;
			public readonly VALUE_IS_NATIVE_FUNCTOR ValueIsNativeFunctor;

			// tiscript VM API
			public readonly TI_SCRIPT_API TIScriptAPI;
			public readonly SCITER_GET_VM SciterGetVM;

			public readonly SCITER_v2V Sciter_v2V;
			public readonly SCITER_V2v Sciter_V2v;
			
			public readonly SCITER_OPEN_ARCHIVE SciterOpenArchive;
			public readonly SCITER_GET_ARCHIVE_ITEM SciterGetArchiveItem;
			public readonly SCITER_CLOSE_ARCHIVE SciterCloseArchive;

			public readonly SCITER_FIRE_EVENT SciterFireEvent;

			public readonly SCITER_GET_CALLBACK_PARAM SciterGetCallbackParam;
			public readonly SCITER_POST_CALLBACK SciterPostCallback;
			public readonly GET_SCITER_GRAPHICS_API GetSciterGraphicsAPI;
			public readonly GET_SCITER_REQUEST_API GetSciterRequestAPI;

#if WINDOWS || NETCORE
			public readonly SCITER_CREATE_ON_DIRECT_X_WINDOW SciterCreateOnDirectXWindow;
			public readonly SCITER_RENDER_ON_DIRECT_X_WINDOW SciterRenderOnDirectXWindow;
			public readonly SCITER_RENDER_ON_DIRECT_X_TEXTURE SciterRenderOnDirectXTexture;
#endif

			public readonly SCITER_PROC_X SciterProcX;



			// JUST FOR NOTE, IF NECESSARY TO DECORATED THE CallingConvention OR CharSet OF THE FPTR's use:
			//[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]

			// LPCWSTR	function() SciterClassName;
            public delegate IntPtr SCITER_CLASS_NAME();// use Marshal.PtrToStringUni(returned IntPtr) to get the actual string
            
			// UINT	function(BOOL major) SciterVersion;
            public delegate uint SCITER_VERSION(bool major);
            
			// BOOL	function(HWINDOW hwnd, LPCWSTR uri, LPCBYTE data, UINT dataLength) SciterDataReady;
            public delegate bool SCITER_DATA_READY(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)]string uri, byte[] data, uint dataLength);

			// BOOL	function(HWINDOW hwnd, LPCWSTR uri, LPCBYTE data, UINT dataLength, LPVOID requestId) SciterDataReadyAsync;
			public delegate bool SCITER_DATA_READY_ASYNC(IntPtr hwnd, string uri, byte[] data, uint dataLength, IntPtr requestId);

#if WINDOWS || NETCORE
			// LRESULT	function(HWINDOW hwnd, UINT msg, WPARAM wParam, LPARAM lParam) SciterProc;
			public delegate IntPtr SCITER_PROC(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);
			// LRESULT	function(HWINDOW hwnd, UINT msg, WPARAM wParam, LPARAM lParam, BOOL* pbHandled) SciterProcND;
			public delegate IntPtr SCITER_PROC_ND(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, ref bool pbHandled);
#endif
			// BOOL	function(HWINDOW hWndSciter, LPCWSTR filename) SciterLoadFile;
			public delegate bool SCITER_LOAD_FILE(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string filename);
			// BOOL function(HWINDOW hWndSciter, LPCBYTE html, UINT htmlSize, LPCWSTR baseUrl) SciterLoadHtml;
			public delegate bool SCITER_LOAD_HTML(IntPtr hwnd, byte[] html, uint htmlSize, string baseUrl);
			// VOID	function(HWINDOW hWndSciter, LPSciterHostCallback cb, LPVOID cbParam) SciterSetCallback;
			public delegate void SCITER_SET_CALLBACK(IntPtr hwnd, IntPtr cb, IntPtr param);// TODO
			// BOOL	function(LPCBYTE utf8, UINT numBytes) SciterSetMasterCSS;
			public delegate bool SCITER_SET_MASTER_CSS(byte[] utf8, uint numBytes);
			// BOOL	function(LPCBYTE utf8, UINT numBytes) SciterAppendMasterCSS;
			public delegate bool SCITER_APPEND_MASTER_CSS(byte[] utf8, uint numBytes);
			// BOOL	function(HWINDOW hWndSciter, LPCBYTE utf8, UINT numBytes, LPCWSTR baseUrl, LPCWSTR mediaType) SciterSetCSS;
			public delegate bool SCITER_SET_CSS(IntPtr hwnd, byte[] utf8, uint numBytes, [MarshalAs(UnmanagedType.LPWStr)]string baseUrl, [MarshalAs(UnmanagedType.LPWStr)]string mediaType);
			// BOOL	function(HWINDOW hWndSciter, LPCWSTR mediaType) SciterSetMediaType;
			public delegate bool SCITER_SET_MEDIA_TYPE(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)]string mediaType);
			// BOOL	function(HWINDOW hWndSciter, const SCITER_VALUE *mediaVars) SciterSetMediaVars;
			public delegate bool SCITER_SET_MEDIA_VARS(IntPtr hwnd, ref SciterValue.VALUE mediaVars);
			// UINT	function(HWINDOW hWndSciter) SciterGetMinWidth;
			public delegate uint SCITER_GET_MIN_WIDTH(IntPtr hwnd);
			// UINT	function(HWINDOW hWndSciter, UINT width) SciterGetMinHeight;
			public delegate uint SCITER_GET_MIN_HEIGHT(IntPtr hwnd, uint width);
			//BOOL	function(HWINDOW hWnd, LPCSTR functionName, UINT argc, const SCITER_VALUE* argv, SCITER_VALUE* retval) SciterCall;
			public delegate bool SCITER_CALL(IntPtr hwnd, [MarshalAs(UnmanagedType.LPStr)]string functionName, uint argc, SciterValue.VALUE[] argv, out SciterValue.VALUE retval);
			// BOOL	function(HWINDOW hwnd, LPCWSTR script, UINT scriptLength, SCITER_VALUE* pretval) SciterEval;
			public delegate bool SCITER_EVAL(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)]string script, uint scriptLength, out SciterValue.VALUE pretval);
			// VOID	function(HWINDOW hwnd) SciterUpdateWindow;
			public delegate bool SCITER_UPDATE_WINDOW(IntPtr hwnd);
#if WINDOWS || NETCORE
			// BOOL	function(MSG* lpMsg) SciterTranslateMessage;
			public delegate bool SCITER_TRANSLATE_MESSAGE(IntPtr lpMsg);// TODO: MSG
#endif
			// BOOL	function(HWINDOW hWnd, UINT option, UINT_PTR value ) SciterSetOption;
			public delegate bool SCITER_SET_OPTION(IntPtr hwnd, SciterXDef.SCITER_RT_OPTIONS option, IntPtr value);
			// VOID	function(HWINDOW hWndSciter, UINT* px, UINT* py) SciterGetPPI;
			public delegate void SCITER_GET_PPI(IntPtr hwnd, ref uint px, ref uint py);
			// BOOL	function(HWINDOW hwnd, VALUE* pval ) SciterGetViewExpando;
			public delegate bool SCITER_GET_VIEW_EXPANDO(IntPtr hwnd, out SciterValue.VALUE pval);
#if WINDOWS || NETCORE
			// BOOL	function(HWINDOW hWndSciter, ID2D1RenderTarget* prt) SciterRenderD2D;
			public delegate bool SCITER_RENDER_D2D(IntPtr hwnd, IntPtr prt);// TODO
			// BOOL	function(ID2D1Factory ** ppf) SciterD2DFactory;
			public delegate bool SCITER_D2D_FACTORY(IntPtr ppf);// TODO
			// BOOL	function(IDWriteFactory ** ppf) SciterDWFactory;
			public delegate bool SCITER_DW_FACTORY(IntPtr ppf);// TODO
#endif
			// BOOL	function(LPUINT pcaps) SciterGraphicsCaps;
			public delegate bool SCITER_GRAPHICS_CAPS(ref uint pcaps);
			// BOOL	function(HWINDOW hWndSciter, LPCWSTR baseUrl) SciterSetHomeURL;
			public delegate bool SCITER_SET_HOME_URL(IntPtr hwnd, string baseUrl);
#if OSX
			// HWINDOW function( LPRECT frame ) SciterCreateNSView;// returns NSView*
			public delegate IntPtr SCITER_CREATE_NS_VIEW(ref PInvokeUtils.RECT frame);
#endif
#if GTKMONO
			// HWINDOW SCFN( SciterCreateWidget )( LPRECT frame ); // returns GtkWidget
			public delegate IntPtr SCITER_CREATE_WIDGET(ref PInvokeUtils.RECT frame);
#endif
			// HWINDOW	function(UINT creationFlags, LPRECT frame, SciterWindowDelegate* delegt, LPVOID delegateParam, HWINDOW parent) SciterCreateWindow;
			public delegate IntPtr SCITER_CREATE_WINDOW(SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags, ref PInvokeUtils.RECT frame, SciterXDef.SCITER_WINDOW_DELEGATE delegt, IntPtr delegateParam, IntPtr parent);
			//VOID	function(
			//  HWINDOW               hwndOrNull,// HWINDOW or null if this is global output handler
			//  LPVOID                param,     // param to be passed "as is" to the pfOutput
			//  DEBUG_OUTPUT_PROC     pfOutput   // output function, output stream alike thing.
			//  ) SciterSetupDebugOutput;
			public delegate void SCITER_SETUP_DEBUG_OUTPUT(IntPtr hwndOrNull, IntPtr param, SciterXDef.DEBUG_OUTPUT_PROC pfOutput);

			//|
			//| DOM Element API
			//|

			// SCDOM_RESULT function(HELEMENT he) Sciter_UseElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_USE_ELEMENT(IntPtr he);
			// SCDOM_RESULT function(HELEMENT he) Sciter_UnuseElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_UNUSE_ELEMENT(IntPtr he);
			//SCDOM_RESULT function(HWINDOW hwnd, HELEMENT *phe) SciterGetRootElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ROOT_ELEMENT(IntPtr hwnd, out IntPtr phe);
			//SCDOM_RESULT function(HWINDOW hwnd, HELEMENT *phe) SciterGetFocusElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_FOCUS_ELEMENT(IntPtr hwnd, out IntPtr phe);
			//SCDOM_RESULT function(HWINDOW hwnd, POINT pt, HELEMENT* phe) SciterFindElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_FIND_ELEMENT(IntPtr hwnd, PInvokeUtils.POINT pt, out IntPtr phe);
			//SCDOM_RESULT function(HELEMENT he, UINT* count) SciterGetChildrenCount;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_CHILDREN_COUNT(IntPtr he, out uint count);
			//SCDOM_RESULT function(HELEMENT he, UINT n, HELEMENT* phe) SciterGetNthChild;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_NTH_CHILD(IntPtr he, uint n, out IntPtr phe);
			//SCDOM_RESULT function(HELEMENT he, HELEMENT* p_parent_he) SciterGetParentElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_PARENT_ELEMENT(IntPtr he, out IntPtr pParentHe);
			//SCDOM_RESULT function(HELEMENT he, BOOL outer, LPCBYTE_RECEIVER rcv, LPVOID rcv_param) SciterGetElementHtmlCB;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_HTML_CB(IntPtr he, bool outer, SciterXDom.LPCBYTE_RECEIVER rcv, IntPtr rcvParam);
			//SCDOM_RESULT function(HELEMENT he, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetElementTextCB;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_TEXT_CB(IntPtr he, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);
			//SCDOM_RESULT function(HELEMENT he, LPCWSTR utf16, UINT length) SciterSetElementText;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_ELEMENT_TEXT(IntPtr he, [MarshalAs(UnmanagedType.LPWStr)]string utf16, uint length);
			//SCDOM_RESULT function(HELEMENT he, LPUINT p_count) SciterGetAttributeCount;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ATTRIBUTE_COUNT(IntPtr he, out uint pCount);
			//SCDOM_RESULT function(HELEMENT he, UINT n, LPCSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetNthAttributeNameCB;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_NTH_ATTRIBUTE_NAME_CB(IntPtr he, uint n, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);
			//SCDOM_RESULT function(HELEMENT he, UINT n, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetNthAttributeValueCB;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_NTH_ATTRIBUTE_VALUE_CB(IntPtr he, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);
			//SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetAttributeByNameCB;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ATTRIBUTE_BY_NAME_CB(IntPtr he, [MarshalAs(UnmanagedType.LPStr)]string name, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);
			//SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR value) SciterSetAttributeByName;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_ATTRIBUTE_BY_NAME(IntPtr he, [MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPWStr)]string value);
			//SCDOM_RESULT function(HELEMENT he) SciterClearAttributes;
			public delegate SciterXDom.SCDOM_RESULT SCITER_CLEAR_ATTRIBUTES(IntPtr he);
			//SCDOM_RESULT function(HELEMENT he, LPUINT p_index) SciterGetElementIndex;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_INDEX(IntPtr he, out uint pIndex);
			//SCDOM_RESULT function(HELEMENT he, LPCSTR* p_type) SciterGetElementType;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_TYPE(IntPtr he, out IntPtr pType);
			//SCDOM_RESULT function(HELEMENT he, LPCSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetElementTypeCB;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_TYPE_CB(IntPtr he, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);
			//SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterGetStyleAttributeCB;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_STYLE_ATTRIBUTE_CB(IntPtr he, [MarshalAs(UnmanagedType.LPStr)]string name, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);
			//SCDOM_RESULT function(HELEMENT he, LPCSTR name, LPCWSTR value) SciterSetStyleAttribute;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_STYLE_ATTRIBUTE(IntPtr he, [MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPWStr)]string value);
			//SCDOM_RESULT function(HELEMENT he, LPRECT p_location, UINT areas /*ELEMENT_AREAS*/) SciterGetElementLocation;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_LOCATION(IntPtr he, out PInvokeUtils.RECT pLocation, SciterXDom.ELEMENT_AREAS areas);
			//SCDOM_RESULT function(HELEMENT he, UINT SciterScrollFlags) SciterScrollToView;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SCROLL_TO_VIEW(IntPtr he, uint sciterScrollFlags);
			//SCDOM_RESULT function(HELEMENT he, BOOL andForceRender) SciterUpdateElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_UPDATE_ELEMENT(IntPtr he, bool andForceRender);
			//SCDOM_RESULT function(HELEMENT he, RECT rc) SciterRefreshElementArea;
			public delegate SciterXDom.SCDOM_RESULT SCITER_REFRESH_ELEMENT_AREA(IntPtr he, PInvokeUtils.RECT rc);
			//SCDOM_RESULT function(HELEMENT he) SciterSetCapture;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_CAPTURE(IntPtr he);
			//SCDOM_RESULT function(HELEMENT he) SciterReleaseCapture;
			public delegate SciterXDom.SCDOM_RESULT SCITER_RELEASE_CAPTURE(IntPtr he);
			//SCDOM_RESULT function(HELEMENT he, HWINDOW* p_hwnd, BOOL rootWindow) SciterGetElementHwnd;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_HWND(IntPtr he, out IntPtr pHwnd, bool rootWindow);
			//SCDOM_RESULT function(HELEMENT he, LPWSTR szUrlBuffer, UINT UrlBufferSize) SciterCombineURL;
			public delegate SciterXDom.SCDOM_RESULT SCITER_COMBINE_URL(IntPtr he, /*[MarshalAs(UnmanagedType.LPWStr)]*/IntPtr szUrlBuffer, uint urlBufferSize);
			//SCDOM_RESULT function(HELEMENT  he, LPCSTR    CSS_selectors, SciterElementCallback callback, LPVOID param) SciterSelectElements;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SELECT_ELEMENTS(IntPtr he, [MarshalAs(UnmanagedType.LPStr)]string cssSelectors, SciterXDom.SCITER_ELEMENT_CALLBACK callback, IntPtr param);
			//SCDOM_RESULT function(HELEMENT  he, LPCWSTR   CSS_selectors, SciterElementCallback callback, LPVOID param) SciterSelectElementsW;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SELECT_ELEMENTS_W(IntPtr he, [MarshalAs(UnmanagedType.LPWStr)]string cssSelectors, SciterXDom.SCITER_ELEMENT_CALLBACK callback, IntPtr param);
			//SCDOM_RESULT function(HELEMENT  he, LPCSTR    selector, UINT      depth, HELEMENT* heFound) SciterSelectParent;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SELECT_PARENT(IntPtr he, [MarshalAs(UnmanagedType.LPStr)]string selector, uint depth, out IntPtr heFound);
			//SCDOM_RESULT function(HELEMENT  he, LPCWSTR   selector, UINT      depth, HELEMENT* heFound) SciterSelectParentW;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SELECT_PARENT_W(IntPtr he, [MarshalAs(UnmanagedType.LPWStr)]string selector, uint depth, out IntPtr heFound);
			//SCDOM_RESULT function(HELEMENT he, const BYTE* html, UINT htmlLength, UINT where) SciterSetElementHtml;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_ELEMENT_HTML(IntPtr he, byte[] html, uint htmlLength, SciterXDom.SET_ELEMENT_HTML where);
			//SCDOM_RESULT function(HELEMENT he, UINT* puid) SciterGetElementUID;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_UID(IntPtr he, out uint puid);
			//SCDOM_RESULT function(HWINDOW hwnd, UINT uid, HELEMENT* phe) SciterGetElementByUID;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_BY_UID(IntPtr hwnd, uint uid, out IntPtr phe);
			//SCDOM_RESULT function(HELEMENT hePopup, HELEMENT heAnchor, UINT placement) SciterShowPopup;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SHOW_POPUP(IntPtr he, IntPtr heAnchor, uint placement);
			//SCDOM_RESULT function(HELEMENT hePopup, POINT pos, UINT placement) SciterShowPopupAt;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SHOW_POPUP_AT(IntPtr he, PInvokeUtils.POINT pos, uint placement);
			//SCDOM_RESULT function(HELEMENT he) SciterHidePopup;
			public delegate SciterXDom.SCDOM_RESULT SCITER_HIDE_POPUP(IntPtr he);
			//SCDOM_RESULT function( HELEMENT he, UINT* pstateBits) SciterGetElementState;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_STATE(IntPtr he, out uint pstateBits);
			//SCDOM_RESULT function( HELEMENT he, UINT stateBitsToSet, UINT stateBitsToClear, BOOL updateView) SciterSetElementState;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_ELEMENT_STATE(IntPtr he, uint stateBitsToSet, uint stateBitsToClear, bool updateView);
			//SCDOM_RESULT function( LPCSTR tagname, LPCWSTR textOrNull, /*out*/ HELEMENT *phe ) SciterCreateElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_CREATE_ELEMENT([MarshalAs(UnmanagedType.LPStr)]string tagname, [MarshalAs(UnmanagedType.LPWStr)]string textOrNull, out IntPtr phe);
			//SCDOM_RESULT function( HELEMENT he, /*out*/ HELEMENT *phe ) SciterCloneElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_CLONE_ELEMENT(IntPtr he, out IntPtr phe);
			//SCDOM_RESULT function( HELEMENT he, HELEMENT hparent, UINT index ) SciterInsertElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_INSERT_ELEMENT(IntPtr he, IntPtr hparent, uint index);
			//SCDOM_RESULT function( HELEMENT he ) SciterDetachElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_DETACH_ELEMENT(IntPtr he);
			//SCDOM_RESULT function(HELEMENT he) SciterDeleteElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_DELETE_ELEMENT(IntPtr he);
			//SCDOM_RESULT function( HELEMENT he, UINT milliseconds, UINT_PTR timer_id ) SciterSetTimer;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_TIMER(IntPtr he, uint milliseconds, IntPtr timerId);
			//SCDOM_RESULT function( HELEMENT he, LPELEMENT_EVENT_PROC pep, LPVOID tag ) SciterDetachEventHandler;
			public delegate SciterXDom.SCDOM_RESULT SCITER_DETACH_EVENT_HANDLER(IntPtr he, SciterBehaviors.ELEMENT_EVENT_PROC pep, IntPtr tag);
			//SCDOM_RESULT function( HELEMENT he, LPELEMENT_EVENT_PROC pep, LPVOID tag ) SciterAttachEventHandler;
			public delegate SciterXDom.SCDOM_RESULT SCITER_ATTACH_EVENT_HANDLER(IntPtr he, SciterBehaviors.ELEMENT_EVENT_PROC pep, IntPtr tag);
			//SCDOM_RESULT function( HWINDOW hwndLayout, LPELEMENT_EVENT_PROC pep, LPVOID tag, UINT subscription ) SciterWindowAttachEventHandler;
			public delegate SciterXDom.SCDOM_RESULT SCITER_WINDOW_ATTACH_EVENT_HANDLER(IntPtr hwndLayout, SciterBehaviors.ELEMENT_EVENT_PROC pep, IntPtr tag, uint subscription);
			//SCDOM_RESULT function( HWINDOW hwndLayout, LPELEMENT_EVENT_PROC pep, LPVOID tag ) SciterWindowDetachEventHandler;
			public delegate SciterXDom.SCDOM_RESULT SCITER_WINDOW_DETACH_EVENT_HANDLER(IntPtr hwndLayout, SciterBehaviors.ELEMENT_EVENT_PROC pep, IntPtr tag);
			//SCDOM_RESULT function( HELEMENT he, UINT appEventCode, HELEMENT heSource, UINT_PTR reason, /*out*/ BOOL* handled) SciterSendEvent;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SEND_EVENT(IntPtr he, uint appEventCode, IntPtr heSource, IntPtr reason, out bool handled);
			//SCDOM_RESULT function( HELEMENT he, UINT appEventCode, HELEMENT heSource, UINT_PTR reason) SciterPostEvent;
			public delegate SciterXDom.SCDOM_RESULT SCITER_POST_EVENT(IntPtr he, uint appEventCode, IntPtr heSource, IntPtr reason);
			//SCDOM_RESULT function(HELEMENT he, METHOD_PARAMS* params) SciterCallBehaviorMethod;
			public delegate SciterXDom.SCDOM_RESULT SCITER_CALL_BEHAVIOR_METHOD(IntPtr he, ref SciterXDom.METHOD_PARAMS param);
			//SCDOM_RESULT function( HELEMENT he, LPCWSTR url, UINT dataType, HELEMENT initiator ) SciterRequestElementData;
			public delegate SciterXDom.SCDOM_RESULT SCITER_REQUEST_ELEMENT_DATA(IntPtr he, [MarshalAs(UnmanagedType.LPWStr)]string url, uint dataType, IntPtr initiator);
			//SCDOM_RESULT function( HELEMENT he,						// element to deliver data 
			//							LPCWSTR         url,			// url 
			//							UINT            dataType,		// data type, see SciterResourceType.
			//							UINT            requestType,	// one of REQUEST_TYPE values
			//							REQUEST_PARAM*  requestParams,	// parameters
			//							UINT            nParams			// number of parameters 
			//							) SciterHttpRequest;
			public delegate SciterXDom.SCDOM_RESULT SCITER_HTTP_REQUEST(IntPtr he, [MarshalAs(UnmanagedType.LPWStr)]string url, uint dataType, uint requestType, ref SciterXDom.REQUEST_PARAM requestParams, uint nParams);
			//SCDOM_RESULT function( HELEMENT he, LPPOINT scrollPos, LPRECT viewRect, LPSIZE contentSize ) SciterGetScrollInfo;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_SCROLL_INFO(IntPtr he, out PInvokeUtils.POINT scrollPos, out PInvokeUtils.RECT viewRect, out PInvokeUtils.SIZE contentSize);
			//SCDOM_RESULT function( HELEMENT he, POINT scrollPos, BOOL smooth ) SciterSetScrollPos;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_SCROLL_POS(IntPtr he, PInvokeUtils.POINT scrollPos, bool smooth);
			//SCDOM_RESULT function( HELEMENT he, INT* pMinWidth, INT* pMaxWidth ) SciterGetElementIntrinsicWidths;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_INTRINSIC_WIDTHS(IntPtr he, out int pMinWidth, out int pMaxWidth);
			//SCDOM_RESULT function( HELEMENT he, INT forWidth, INT* pHeight ) SciterGetElementIntrinsicHeight;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_INTRINSIC_HEIGHT(IntPtr he, int forWidth, out int pHeight);
			//SCDOM_RESULT function( HELEMENT he, BOOL* pVisible) SciterIsElementVisible;
			public delegate SciterXDom.SCDOM_RESULT SCITER_IS_ELEMENT_VISIBLE(IntPtr he, out bool pVisible);
			//SCDOM_RESULT function( HELEMENT he, BOOL* pEnabled ) SciterIsElementEnabled;
			public delegate SciterXDom.SCDOM_RESULT SCITER_IS_ELEMENT_ENABLED(IntPtr he, out bool pEnabled);
			//SCDOM_RESULT function( HELEMENT he, UINT firstIndex, UINT lastIndex, ELEMENT_COMPARATOR* cmpFunc, LPVOID cmpFuncParam ) SciterSortElements;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SORT_ELEMENTS(IntPtr he, uint firstIndex, uint lastIndex, SciterXDom.ELEMENT_COMPARATOR cmpFunc, IntPtr cmpFuncParam);
			//SCDOM_RESULT function( HELEMENT he1, HELEMENT he2 ) SciterSwapElements;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SWAP_ELEMENTS(IntPtr he, IntPtr he2);
			//SCDOM_RESULT function( UINT evt, LPVOID eventCtlStruct, BOOL* bOutProcessed ) SciterTraverseUIEvent;
			public delegate SciterXDom.SCDOM_RESULT SCITER_TRAVERSE_UI_EVENT(IntPtr he, IntPtr eventCtlStruct, out bool bOutProcessed);
			//SCDOM_RESULT function( HELEMENT he, LPCSTR name, const VALUE* argv, UINT argc, VALUE* retval ) SciterCallScriptingMethod;
			public delegate SciterXDom.SCDOM_RESULT SCITER_CALL_SCRIPTING_METHOD(IntPtr he, [MarshalAs(UnmanagedType.LPStr)]string name, SciterValue.VALUE[] argv, uint argc, out SciterValue.VALUE retval);
			//SCDOM_RESULT function( HELEMENT he, LPCSTR name, const VALUE* argv, UINT argc, VALUE* retval ) SciterCallScriptingFunction;
			public delegate SciterXDom.SCDOM_RESULT SCITER_CALL_SCRIPTING_FUNCTION(IntPtr he, [MarshalAs(UnmanagedType.LPStr)]string name, SciterValue.VALUE[] argv, uint argc, out SciterValue.VALUE retval);
			//SCDOM_RESULT function( HELEMENT he, LPCWSTR script, UINT scriptLength, VALUE* retval ) SciterEvalElementScript;
			public delegate SciterXDom.SCDOM_RESULT SCITER_EVAL_ELEMENT_SCRIPT(IntPtr he, [MarshalAs(UnmanagedType.LPWStr)]string script, uint scriptLength, out SciterValue.VALUE retval);
			//SCDOM_RESULT function( HELEMENT he, HWINDOW hwnd) SciterAttachHwndToElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_ATTACH_HWND_TO_ELEMENT(IntPtr he, IntPtr hwnd);
			//SCDOM_RESULT function( HELEMENT he, /*CTL_TYPE*/ UINT *pType ) SciterControlGetType;
			public delegate SciterXDom.SCDOM_RESULT SCITER_CONTROL_GET_TYPE(IntPtr he, out uint pType);
			//SCDOM_RESULT function( HELEMENT he, VALUE* pval ) SciterGetValue;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_VALUE(IntPtr he, out SciterValue.VALUE pval);
			//SCDOM_RESULT function( HELEMENT he, const VALUE* pval ) SciterSetValue;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_VALUE(IntPtr he, ref SciterValue.VALUE pval);
			//SCDOM_RESULT function( HELEMENT he, VALUE* pval, BOOL forceCreation ) SciterGetExpando;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_EXPANDO(IntPtr he, out SciterValue.VALUE pval, bool forceCreation);
			//SCDOM_RESULT function( HELEMENT he, tiscript_value* pval, BOOL forceCreation ) SciterGetObject;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_OBJECT(IntPtr he, out TIScript.tiscript_value pval, bool forceCreation);
			//SCDOM_RESULT function( HELEMENT he, tiscript_value* pval) SciterGetElementNamespace;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_ELEMENT_NAMESPACE(IntPtr he, out TIScript.tiscript_value pval);
			//SCDOM_RESULT function( HWINDOW hwnd, HELEMENT* phe) SciterGetHighlightedElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_GET_HIGHLIGHTED_ELEMENT(IntPtr hwnd, out IntPtr phe);
			//SCDOM_RESULT function( HWINDOW hwnd, HELEMENT he) SciterSetHighlightedElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_SET_HIGHLIGHTED_ELEMENT(IntPtr hwnd, IntPtr he);

			//|
			//| DOM Node API
			//|

			//SCDOM_RESULT function(HNODE hn) SciterNodeAddRef;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_ADD_REF(IntPtr hn);
			//SCDOM_RESULT function(HNODE hn) SciterNodeRelease;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_RELEASE(IntPtr hn);
			//SCDOM_RESULT function(HELEMENT he, HNODE* phn) SciterNodeCastFromElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_CAST_FROM_ELEMENT(IntPtr he, out IntPtr phn);
			//SCDOM_RESULT function(HNODE hn, HELEMENT* he) SciterNodeCastToElement;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_CAST_TO_ELEMENT(IntPtr hn, out IntPtr he);
			//SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodeFirstChild;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_FIRST_CHILD(IntPtr hn, out IntPtr phn);
			//SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodeLastChild;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_LAST_CHILD(IntPtr hn, out IntPtr phn);
			//SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodeNextSibling;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_NEXT_SIBLING(IntPtr hn, out IntPtr phn);
			//SCDOM_RESULT function(HNODE hn, HNODE* phn) SciterNodePrevSibling;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_PREV_SIBLING(IntPtr hn, out IntPtr phn);
			//SCDOM_RESULT function(HNODE hnode, HELEMENT* pheParent) SciterNodeParent;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_PARENT(IntPtr hn, out IntPtr pheParent);
			//SCDOM_RESULT function(HNODE hnode, UINT n, HNODE* phn) SciterNodeNthChild;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_NTH_CHILD(IntPtr hn, uint n, out IntPtr phn);
			//SCDOM_RESULT function(HNODE hnode, UINT* pn) SciterNodeChildrenCount;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_CHILDREN_COUNT(IntPtr hn, out uint pn);
			//SCDOM_RESULT function(HNODE hnode, UINT* pNodeType /*NODE_TYPE*/) SciterNodeType;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_TYPE(IntPtr hn, out SciterXDom.NODE_TYPE pn);
			//SCDOM_RESULT function(HNODE hnode, LPCWSTR_RECEIVER rcv, LPVOID rcv_param) SciterNodeGetText;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_GET_TEXT(IntPtr hn, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);
			//SCDOM_RESULT function(HNODE hnode, LPCWSTR text, UINT textLength) SciterNodeSetText;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_SET_TEXT(IntPtr hn, [MarshalAs(UnmanagedType.LPWStr)]string text, uint textLength);
			//SCDOM_RESULT function(HNODE hnode, UINT where /*NODE_INS_TARGET*/, HNODE what) SciterNodeInsert;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_INSERT(IntPtr hn, uint where, IntPtr what);
			//SCDOM_RESULT function(HNODE hnode, BOOL finalize) SciterNodeRemove;
			public delegate SciterXDom.SCDOM_RESULT SCITER_NODE_REMOVE(IntPtr hn, bool finalize);
			//SCDOM_RESULT function(LPCWSTR text, UINT textLength, HNODE* phnode) SciterCreateTextNode;
			public delegate SciterXDom.SCDOM_RESULT SCITER_CREATE_TEXT_NODE([MarshalAs(UnmanagedType.LPWStr)]string text, uint textLength, out IntPtr phnode);
			//SCDOM_RESULT function(LPCWSTR text, UINT textLength, HNODE* phnode) SciterCreateCommentNode;
			public delegate SciterXDom.SCDOM_RESULT SCITER_CREATE_COMMENT_NODE([MarshalAs(UnmanagedType.LPWStr)]string text, uint textLength, out IntPtr phnode);

			//|
			//| Value API
			//|
			// UINT function( VALUE* pval ) ValueInit;
			public delegate SciterValue.VALUE_RESULT VALUE_INIT(out SciterValue.VALUE pval);
			// UINT function( VALUE* pval ) ValueClear;
			public delegate SciterValue.VALUE_RESULT VALUE_CLEAR(out SciterValue.VALUE pval);
			// UINT function( const VALUE* pval1, const VALUE* pval2 ) ValueCompare;
			public delegate SciterValue.VALUE_RESULT VALUE_COMPARE(ref SciterValue.VALUE pval, ref IntPtr pval2);
			// UINT function( VALUE* pdst, const VALUE* psrc ) ValueCopy;
			public delegate SciterValue.VALUE_RESULT VALUE_COPY(out SciterValue.VALUE pdst, ref SciterValue.VALUE psrc);
			// UINT function( VALUE* pdst ) ValueIsolate;
			public delegate SciterValue.VALUE_RESULT VALUE_ISOLATE(ref SciterValue.VALUE pdst);
			// UINT function( const VALUE* pval, UINT* pType, UINT* pUnits ) ValueType;
			public delegate SciterValue.VALUE_RESULT VALUE_TYPE(ref SciterValue.VALUE pval, out uint pType, out uint pUnits);
			// UINT function( const VALUE* pval, LPCWSTR* pChars, UINT* pNumChars ) ValueStringData;
			public delegate SciterValue.VALUE_RESULT VALUE_STRING_DATA(ref SciterValue.VALUE pval, out IntPtr pChars, out uint pNumChars);
			// UINT function( VALUE* pval, LPCWSTR chars, UINT numChars, UINT units ) ValueStringDataSet;
			public delegate SciterValue.VALUE_RESULT VALUE_STRING_DATA_SET(ref SciterValue.VALUE pval, [MarshalAs(UnmanagedType.LPWStr)]string chars, uint numChars, uint units);
			// UINT function( const VALUE* pval, INT* pData ) ValueIntData;
			public delegate SciterValue.VALUE_RESULT VALUE_INT_DATA(ref SciterValue.VALUE pval, out int pData);
			// UINT function( VALUE* pval, INT data, UINT type, UINT units ) ValueIntDataSet;
			public delegate SciterValue.VALUE_RESULT VALUE_INT_DATA_SET(ref SciterValue.VALUE pval, int data, uint type, uint units);
			// UINT function( const VALUE* pval, INT64* pData ) ValueInt64Data;
			public delegate SciterValue.VALUE_RESULT VALUE_INT_64DATA(ref SciterValue.VALUE pval, out long pData);
			// UINT function( VALUE* pval, INT64 data, UINT type, UINT units ) ValueInt64DataSet;
			public delegate SciterValue.VALUE_RESULT VALUE_INT_64DATA_SET(ref SciterValue.VALUE pval, long data, uint type, uint units);
			// UINT function( const VALUE* pval, FLOAT_VALUE* pData ) ValueFloatData;
			public delegate SciterValue.VALUE_RESULT VALUE_FLOAT_DATA(ref SciterValue.VALUE pval, out double pData);
			// UINT function( VALUE* pval, FLOAT_VALUE data, UINT type, UINT units ) ValueFloatDataSet;
			public delegate SciterValue.VALUE_RESULT VALUE_FLOAT_DATA_SET(ref SciterValue.VALUE pval, double data, uint type, uint units);
			// UINT function( const VALUE* pval, LPCBYTE* pBytes, UINT* pnBytes ) ValueBinaryData;
			public delegate SciterValue.VALUE_RESULT VALUE_BINARY_DATA(ref SciterValue.VALUE pval, out IntPtr pBytes, out uint pnBytes);
			// UINT function( VALUE* pval, LPCBYTE pBytes, UINT nBytes, UINT type, UINT units ) ValueBinaryDataSet;
			public delegate SciterValue.VALUE_RESULT VALUE_BINARY_DATA_SET(ref SciterValue.VALUE pval, [MarshalAs(UnmanagedType.LPArray)]byte[] pBytes, uint nBytes, uint type, uint units);
			// UINT function( const VALUE* pval, INT* pn) ValueElementsCount;
			public delegate SciterValue.VALUE_RESULT VALUE_ELEMENTS_COUNT(ref SciterValue.VALUE pval, out int pn);
			// UINT function( const VALUE* pval, INT n, VALUE* pretval) ValueNthElementValue;
			public delegate SciterValue.VALUE_RESULT VALUE_NTH_ELEMENT_VALUE(ref SciterValue.VALUE pval, int n, out SciterValue.VALUE pretval);
			// UINT function( VALUE* pval, INT n, const VALUE* pval_to_set) ValueNthElementValueSet;
			public delegate SciterValue.VALUE_RESULT VALUE_NTH_ELEMENT_VALUE_SET(ref SciterValue.VALUE pval, int n, ref SciterValue.VALUE pvalToSet);
			// UINT function( const VALUE* pval, INT n, VALUE* pretval) ValueNthElementKey;
			public delegate SciterValue.VALUE_RESULT VALUE_NTH_ELEMENT_KEY(ref SciterValue.VALUE pval, int n, out SciterValue.VALUE pretval);
			// UINT function( VALUE* pval, KeyValueCallback* penum, LPVOID param) ValueEnumElements;
			public delegate SciterValue.VALUE_RESULT VALUE_ENUM_ELEMENTS(ref SciterValue.VALUE pval, SciterValue.KEY_VALUE_CALLBACK penum, IntPtr param);
			// UINT function( VALUE* pval, const VALUE* pkey, const VALUE* pval_to_set) ValueSetValueToKey;
			public delegate SciterValue.VALUE_RESULT VALUE_SET_VALUE_TO_KEY(ref SciterValue.VALUE pval, ref SciterValue.VALUE pkey, ref SciterValue.VALUE pvalToSet);
			// UINT function( const VALUE* pval, const VALUE* pkey, VALUE* pretval) ValueGetValueOfKey;
			public delegate SciterValue.VALUE_RESULT VALUE_GET_VALUE_OF_KEY(ref SciterValue.VALUE pval, ref SciterValue.VALUE pkey, out SciterValue.VALUE pretval);
			// UINT function( VALUE* pval, /*VALUE_STRING_CVT_TYPE*/ UINT how ) ValueToString;
			public delegate SciterValue.VALUE_RESULT VALUE_TO_STRING(ref SciterValue.VALUE pval, SciterValue.VALUE_STRING_CVT_TYPE how);
			// UINT function( VALUE* pval, LPCWSTR str, UINT strLength, /*VALUE_STRING_CVT_TYPE*/ UINT how ) ValueFromString;
			public delegate SciterValue.VALUE_RESULT VALUE_FROM_STRING(ref SciterValue.VALUE pval, [MarshalAs(UnmanagedType.LPWStr)]string str, uint strLength, uint how);
			// UINT function( VALUE* pval, VALUE* pthis, UINT argc, const VALUE* argv, VALUE* pretval, LPCWSTR url) ValueInvoke;
			public delegate SciterValue.VALUE_RESULT VALUE_INVOKE(ref SciterValue.VALUE pval, ref SciterValue.VALUE pthis, uint argc, SciterValue.VALUE[] argv, out SciterValue.VALUE pretval, [MarshalAs(UnmanagedType.LPWStr)]string url);
			// UINT function( VALUE* pval, NATIVE_FUNCTOR_INVOKE*  pinvoke, NATIVE_FUNCTOR_RELEASE* prelease, VOID* tag) ValueNativeFunctorSet;
			public delegate SciterValue.VALUE_RESULT VALUE_NATIVE_FUNCTOR_SET(ref SciterValue.VALUE pval, SciterValue.NATIVE_FUNCTOR_INVOKE pinvoke, SciterValue.NATIVE_FUNCTOR_RELEASE prelease, IntPtr tag);
			// BOOL function( const VALUE* pval) ValueIsNativeFunctor;
			public delegate SciterValue.VALUE_RESULT VALUE_IS_NATIVE_FUNCTOR(ref SciterValue.VALUE pval);

			// tiscript VM API
			// tiscript_native_interface* function() TIScriptAPI;
			public delegate IntPtr TI_SCRIPT_API();
			// HVM function(HWINDOW hwnd) SciterGetVM;
			public delegate IntPtr SCITER_GET_VM(IntPtr hwnd);

			// BOOL function(HVM vm, tiscript_value script_value, VALUE* value, BOOL isolate) Sciter_v2V;
			public delegate bool SCITER_v2V(IntPtr vm, TIScript.tiscript_value scriptValue, ref SciterValue.VALUE value, bool isolate);
			// BOOL function(HVM vm, const VALUE* valuev, tiscript_value* script_value) Sciter_V2v;
			public delegate bool SCITER_V2v(IntPtr vm, ref SciterValue.VALUE value, ref TIScript.tiscript_value scriptValue);

			// HSARCHIVE function(LPCBYTE archiveData, UINT archiveDataLength) SciterOpenArchive;
			public delegate IntPtr SCITER_OPEN_ARCHIVE(IntPtr archiveData, uint archiveDataLength);// archiveData must point to a pinned byte[] array!
			// BOOL function(HSARCHIVE harc, LPCWSTR path, LPCBYTE* pdata, UINT* pdataLength) SciterGetArchiveItem;
			public delegate bool SCITER_GET_ARCHIVE_ITEM(IntPtr harc, [MarshalAs(UnmanagedType.LPWStr)]string path, out IntPtr pdata, out uint pdataLength);
			// BOOL function(HSARCHIVE harc) SciterCloseArchive;
			public delegate bool SCITER_CLOSE_ARCHIVE(IntPtr harc);

			// SCDOM_RESULT function( const BEHAVIOR_EVENT_PARAMS* evt, BOOL post, BOOL *handled ) SciterFireEvent;
			public delegate int SCITER_FIRE_EVENT(ref SciterBehaviors.BEHAVIOR_EVENT_PARAMS evt, bool post, out bool handled);

			// LPVOID function(HWINDOW hwnd) SciterGetCallbackParam;
			public delegate IntPtr SCITER_GET_CALLBACK_PARAM(IntPtr hwnd);
			// UINT_PTR function(HWINDOW hwnd, UINT_PTR wparam, UINT_PTR lparam, UINT timeoutms) SciterPostCallback;// if timeoutms>0 then it is a send, not a post
			public delegate IntPtr SCITER_POST_CALLBACK(IntPtr hwnd, IntPtr wparam, IntPtr lparam, uint timeoutms);

			// LPSciterGraphicsAPI function() GetSciterGraphicsAPI;
			public delegate IntPtr GET_SCITER_GRAPHICS_API();

			// LPSciterRequestAPI SCFN(GetSciterRequestAPI )();
			public delegate IntPtr GET_SCITER_REQUEST_API();

#if WINDOWS || NETCORE
			// BOOL SCFN(SciterCreateOnDirectXWindow ) (HWINDOW hwnd, IDXGISwapChain* pSwapChain);
			public delegate bool SCITER_CREATE_ON_DIRECT_X_WINDOW(IntPtr hwnd, IntPtr pSwapChain);
			// BOOL SCFN(SciterRenderOnDirectXWindow ) (HWINDOW hwnd, HELEMENT elementToRenderOrNull, BOOL frontLayer);
			public delegate bool SCITER_RENDER_ON_DIRECT_X_WINDOW(IntPtr hwnd, IntPtr elementToRenderOrNull, bool frontLayer);
			// BOOL SCFN(SciterRenderOnDirectXTexture ) (HWINDOW hwnd, HELEMENT elementToRenderOrNull, IDXGISurface* surface);
			public delegate bool SCITER_RENDER_ON_DIRECT_X_TEXTURE(IntPtr hwnd, IntPtr elementToRenderOrNull, IntPtr surface);
#endif

			// BOOL SCFN(SciterProcX)(HWINDOW hwnd, SCITER_X_MSG* pMsg ); // returns TRUE if handled
			public delegate bool SCITER_PROC_X(IntPtr hwnd, IntPtr pMsg);
		}
	}
}