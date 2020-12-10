// ReSharper disable RedundantUsingDirective
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
#if NETCOREAPP3_1
using System.Reflection;
#endif
// ReSharper restore RedundantUsingDirective

// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore.Interop
{
	public static partial class Sciter
	{
		public static ISciterApi Api => GetSciterApi();

		public static SciterGraphics.SciterGraphicsApi GraphicsApi => GetGraphicsApi();

		public static SciterRequest.SciterRequestApi RequestApi => GetRequestApi();

		public static TIScript.SCITER_TI_SCRIPT_API ScriptApi => GetScriptApi();

        private static ISciterApi _sciterApi = null;
		private static SciterGraphics.SciterGraphicsApi? _sciterGraphicsApi = null;
		private static SciterRequest.SciterRequestApi? _sciterRequestApiInstance = null;
		private static TIScript.SCITER_TI_SCRIPT_API? _sciterScriptApi = null;

		// ReSharper disable InconsistentNaming
		private const string SciterWindowsLibrary = "sciter.dll";
		private const string SciterUnixLibrary = "libsciter-gtk.so";
		private const string SciterMacOSLibrary = "sciter-osx-64.dylib";
		// ReSharper enable InconsistentNaming
		
#if NETCOREAPP3_1
		//Name is purely to avoid collision
		private const string SciterPlatformLibrary = "sciter_1913ebe4-1c89-43ee-a659-949ef4e9a108_import";

		private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
		{
			var libHandle = IntPtr.Zero;
			if (libraryName != SciterPlatformLibrary) 
				return libHandle;

			string libName;
				
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				libName = SciterWindowsLibrary;
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				libName = SciterMacOSLibrary;
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
				libName = SciterUnixLibrary;
			else
				throw new PlatformNotSupportedException();

			NativeLibrary.TryLoad(libName, assembly, DllImportSearchPath.System32, out libHandle);

			return libHandle;
		}
#endif
			
#if NETCOREAPP3_1
		[DllImport(SciterPlatformLibrary, EntryPoint = "SciterAPI")]
#elif WINDOWS || NETCORE
		[DllImport(SciterWindowsLibrary, EntryPoint = "SciterAPI")]
#elif GTKMONO
		[DllImport(SciterUnixLibrary)]
#elif OSX
		[DllImport(SciterMacOSLibrary, EntryPoint = "SciterAPI")]
#endif
		private static extern IntPtr SciterAPI();

		private static ISciterApi GetSciterApi()
		{
			if (_sciterApi != null)
				return _sciterApi;
			
			//var apiStructSize = Marshal.SizeOf(typeof(SciterApi));
			
#if NETCOREAPP3_1 
			NativeLibrary.SetDllImportResolver(typeof(Sciter).Assembly, ImportResolver);
#elif WINDOWS || NETCORE

			var codeBasePath = new Uri(typeof(SciterApi).Assembly.CodeBase).LocalPath;
			var codeBaseDirectory = Path.GetDirectoryName(codeBasePath);
			var is64 = Environment.Is64BitProcess;
			var bitDirectory = is64 ? "x64" : "x86";
			var libName = Path.Combine(codeBaseDirectory, bitDirectory, SciterWindowsLibrary);
                
			PInvokeWindows.LoadLibrary(libName);
#elif GTKMONO
			if(IntPtr.Size != 8)
				throw new Exception("SciterSharp GTK/Mono only supports 64bits builds");

			//Debug.Assert(apiStructSize == 1304);
#elif OSX
			Debug.Assert(IntPtr.Size == 8);
			//Debug.Assert(apiStructSize == 648 * 2);
#endif
			var apiPtr = SciterAPI();

			_sciterApi = UnsafeNativeMethods.GetSciterApiInterface(); 

			if (_sciterApi == null)
				throw new NullReferenceException($"{nameof(ISciterApi)} cannot be null");
				
			// from time to time, Sciter changes its API
			// here we test the minimum Sciter version this library is compatible with
			var major = _sciterApi.SciterVersion(true);
			var minor = _sciterApi.SciterVersion(false);
			Debug.Assert(major >= 0x00040000);
			Debug.Assert(_sciterApi.Version >= 0);
			
			return _sciterApi;
		}

		private static SciterGraphics.SciterGraphicsApi GetGraphicsApi()
		{
			if (_sciterGraphicsApi != null)
				return _sciterGraphicsApi.Value;
			
			var major = Api.SciterVersion(true);
			var minor = Api.SciterVersion(false);
			Debug.Assert(major >= 0x00040000);

			var apiStructSize = Marshal.SizeOf(t: typeof(SciterGraphics.SciterGraphicsApi));
			
			if(IntPtr.Size == 8)
				Debug.Assert(apiStructSize == 276 * 2);
			else
				Debug.Assert(apiStructSize == 276);

			var apiPtr = Api.GetSciterGraphicsAPI();
			_sciterGraphicsApi = Marshal.PtrToStructure<SciterGraphics.SciterGraphicsApi>(ptr: apiPtr);
			
			if (_sciterGraphicsApi == null)
				throw new NullReferenceException($"{nameof(_sciterGraphicsApi)} cannot be null");
			
			return _sciterGraphicsApi.Value;
		}

		private static SciterRequest.SciterRequestApi GetRequestApi()
		{
			if (_sciterRequestApiInstance != null)
				return _sciterRequestApiInstance.Value;

			var apiStructSize = Marshal.SizeOf(t: typeof(SciterRequest.SciterRequestApi));

			if (IntPtr.Size == 8)
				Debug.Assert(apiStructSize == 104 * 2);
			else
				Debug.Assert(apiStructSize == 104);

			var apiPtr = Api.GetSciterRequestAPI();
			_sciterRequestApiInstance = Marshal.PtrToStructure<SciterRequest.SciterRequestApi>(ptr: apiPtr);

			if (_sciterRequestApiInstance == null)
				throw new NullReferenceException($"{nameof(_sciterRequestApiInstance)} cannot be null");

			return _sciterRequestApiInstance.Value;
		}

		private static TIScript.SCITER_TI_SCRIPT_API GetScriptApi()
		{
			if (_sciterScriptApi != null)
				return _sciterScriptApi.Value;

			var apiStructSize = Marshal.SizeOf(typeof(TIScript.SCITER_TI_SCRIPT_API));
			if (IntPtr.Size == 8)
				Debug.Assert(apiStructSize == 616);
			else
				Debug.Assert(apiStructSize == 308);

			var apiPtr = Api.TIScriptAPI();
			_sciterScriptApi = Marshal.PtrToStructure<TIScript.SCITER_TI_SCRIPT_API>(ptr: apiPtr);

			if (_sciterScriptApi == null)
				throw new NullReferenceException($"{nameof(_sciterScriptApi)} cannot be null");

			return _sciterScriptApi.Value;
		}
		
		internal static class UnsafeNativeMethods
		{
			public static ISciterApi GetSciterApiInterface()
			{
				var sciterApi = SciterAPI();
				
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					return new NativeSciterApiWrapper(sciterApi, Marshal.PtrToStructure<WindowsSciterApi>(sciterApi));

				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
					return new NativeSciterApiWrapper(sciterApi, Marshal.PtrToStructure<MacOsSciterApi>(sciterApi));

				if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
					return new NativeSciterApiWrapper(sciterApi, Marshal.PtrToStructure<LinuxSciterApi>(sciterApi));
				
				throw new PlatformNotSupportedException();
			}
			

			private sealed class NativeSciterApiWrapper : ISciterApi
			{
				private IntPtr _sciterApiPtr;
				
#pragma warning disable 649
				private readonly int _version;
				private readonly SciterApi.SCITER_CLASS_NAME _sciterClassName = null;
				private readonly SciterApi.SCITER_VERSION _sciterVersion = null;
				private readonly SciterApi.SCITER_DATA_READY _sciterDataReady = null;
				private readonly SciterApi.SCITER_DATA_READY_ASYNC _sciterDataReadyAsync = null;
				private readonly SciterApi.SCITER_PROC _sciterProc = null;
				private readonly SciterApi.SCITER_PROC_ND _sciterProcND = null;
				private readonly SciterApi.SCITER_LOAD_FILE _sciterLoadFile = null;
				private readonly SciterApi.SCITER_LOAD_HTML _sciterLoadHtml = null;
				private readonly SciterApi.SCITER_SET_CALLBACK _sciterSetCallback = null;
				private readonly SciterApi.SCITER_SET_MASTER_CSS _sciterSetMasterCSS = null;
				private readonly SciterApi.SCITER_APPEND_MASTER_CSS _sciterAppendMasterCSS = null;
				private readonly SciterApi.SCITER_SET_CSS _sciterSetCSS = null;
				private readonly SciterApi.SCITER_SET_MEDIA_TYPE _sciterSetMediaType = null;
				private readonly SciterApi.SCITER_SET_MEDIA_VARS _sciterSetMediaVars = null;
				private readonly SciterApi.SCITER_GET_MIN_WIDTH _sciterGetMinWidth = null;
				private readonly SciterApi.SCITER_GET_MIN_HEIGHT _sciterGetMinHeight = null;
				private readonly SciterApi.SCITER_CALL _sciterCall = null;
				private readonly SciterApi.SCITER_EVAL _sciterEval = null;
				private readonly SciterApi.SCITER_UPDATE_WINDOW _sciterUpdateWindow = null;
				
				private readonly SciterApi.SCITER_TRANSLATE_MESSAGE _sciterTranslateMessage = null;
				private readonly SciterApi.SCITER_SET_OPTION _sciterSetOption = null;
				private readonly SciterApi.SCITER_GET_PPI _sciterGetPPI = null;
				private readonly SciterApi.SCITER_GET_VIEW_EXPANDO _sciterGetViewExpando = null;
				private readonly SciterApi.SCITER_RENDER_D2D _sciterRenderD2D = null;
				private readonly SciterApi.SCITER_D2D_FACTORY _sciterD2DFactory = null;
				private readonly SciterApi.SCITER_DW_FACTORY _sciterDWFactory = null;
				private readonly SciterApi.SCITER_GRAPHICS_CAPS _sciterGraphicsCaps = null;
				private readonly SciterApi.SCITER_SET_HOME_URL _sciterSetHomeURL = null;
				private readonly SciterApi.SCITER_CREATE_NS_VIEW _sciterCreateNSView = null;
				private readonly SciterApi.SCITER_CREATE_WIDGET _sciterCreateWidget = null;

				private readonly SciterApi.SCITER_CREATE_WINDOW _sciterCreateWindow = null;
				private readonly SciterApi.SCITER_SETUP_DEBUG_OUTPUT _sciterSetupDebugOutput = null;
				
				#region DOM Element API
				
				private readonly SciterApi.SCITER_USE_ELEMENT _sciter_UseElement = null;
				private readonly SciterApi.SCITER_UNUSE_ELEMENT _sciter_UnuseElement = null;
				private readonly SciterApi.SCITER_GET_ROOT_ELEMENT _sciterGetRootElement = null;
				private readonly SciterApi.SCITER_GET_FOCUS_ELEMENT _sciterGetFocusElement = null;
				private readonly SciterApi.SCITER_FIND_ELEMENT _sciterFindElement = null;
				private readonly SciterApi.SCITER_GET_CHILDREN_COUNT _sciterGetChildrenCount = null;
				private readonly SciterApi.SCITER_GET_NTH_CHILD _sciterGetNthChild = null;
				private readonly SciterApi.SCITER_GET_PARENT_ELEMENT _sciterGetParentElement = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_HTML_CB _sciterGetElementHtmlCB = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_TEXT_CB _sciterGetElementTextCB = null;
				private readonly SciterApi.SCITER_SET_ELEMENT_TEXT _sciterSetElementText = null;
				private readonly SciterApi.SCITER_GET_ATTRIBUTE_COUNT _sciterGetAttributeCount = null;
				private readonly SciterApi.SCITER_GET_NTH_ATTRIBUTE_NAME_CB _sciterGetNthAttributeNameCB = null;
				private readonly SciterApi.SCITER_GET_NTH_ATTRIBUTE_VALUE_CB _sciterGetNthAttributeValueCB = null;
				private readonly SciterApi.SCITER_GET_ATTRIBUTE_BY_NAME_CB _sciterGetAttributeByNameCB = null;
				private readonly SciterApi.SCITER_SET_ATTRIBUTE_BY_NAME _sciterSetAttributeByName = null;
				private readonly SciterApi.SCITER_CLEAR_ATTRIBUTES _sciterClearAttributes = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_INDEX _sciterGetElementIndex = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_TYPE _sciterGetElementType = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_TYPE_CB _sciterGetElementTypeCB = null;
				private readonly SciterApi.SCITER_GET_STYLE_ATTRIBUTE_CB _sciterGetStyleAttributeCB = null;
				private readonly SciterApi.SCITER_SET_STYLE_ATTRIBUTE _sciterSetStyleAttribute = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_LOCATION _sciterGetElementLocation = null;
				private readonly SciterApi.SCITER_SCROLL_TO_VIEW _sciterScrollToView = null;
				private readonly SciterApi.SCITER_UPDATE_ELEMENT _sciterUpdateElement = null;
				private readonly SciterApi.SCITER_REFRESH_ELEMENT_AREA _sciterRefreshElementArea = null;
				private readonly SciterApi.SCITER_SET_CAPTURE _sciterSetCapture = null;
				private readonly SciterApi.SCITER_RELEASE_CAPTURE _sciterReleaseCapture = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_HWND _sciterGetElementHwnd = null;
				private readonly SciterApi.SCITER_COMBINE_URL _sciterCombineURL = null;
				private readonly SciterApi.SCITER_SELECT_ELEMENTS _sciterSelectElements = null;
				private readonly SciterApi.SCITER_SELECT_ELEMENTS_W _sciterSelectElementsW = null;
				private readonly SciterApi.SCITER_SELECT_PARENT _sciterSelectParent = null;
				private readonly SciterApi.SCITER_SELECT_PARENT_W _sciterSelectParentW = null;
				private readonly SciterApi.SCITER_SET_ELEMENT_HTML _sciterSetElementHtml = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_UID _sciterGetElementUID = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_BY_UID _sciterGetElementByUID = null;
				private readonly SciterApi.SCITER_SHOW_POPUP _sciterShowPopup = null;
				private readonly SciterApi.SCITER_SHOW_POPUP_AT _sciterShowPopupAt = null;
				private readonly SciterApi.SCITER_HIDE_POPUP _sciterHidePopup = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_STATE _sciterGetElementState = null;
				private readonly SciterApi.SCITER_SET_ELEMENT_STATE _sciterSetElementState = null;
				private readonly SciterApi.SCITER_CREATE_ELEMENT _sciterCreateElement = null;
				private readonly SciterApi.SCITER_CLONE_ELEMENT _sciterCloneElement = null;
				private readonly SciterApi.SCITER_INSERT_ELEMENT _sciterInsertElement = null;
				private readonly SciterApi.SCITER_DETACH_ELEMENT _sciterDetachElement = null;
				private readonly SciterApi.SCITER_DELETE_ELEMENT _sciterDeleteElement = null;
				private readonly SciterApi.SCITER_SET_TIMER _sciterSetTimer = null;
				private readonly SciterApi.SCITER_DETACH_EVENT_HANDLER _sciterDetachEventHandler = null;
				private readonly SciterApi.SCITER_ATTACH_EVENT_HANDLER _sciterAttachEventHandler = null;
				private readonly SciterApi.SCITER_WINDOW_ATTACH_EVENT_HANDLER _sciterWindowAttachEventHandler = null;
				private readonly SciterApi.SCITER_WINDOW_DETACH_EVENT_HANDLER _sciterWindowDetachEventHandler = null;
				private readonly SciterApi.SCITER_SEND_EVENT _sciterSendEvent = null;
				private readonly SciterApi.SCITER_POST_EVENT _sciterPostEvent = null;
				private readonly SciterApi.SCITER_CALL_BEHAVIOR_METHOD _sciterCallBehaviorMethod = null;
				private readonly SciterApi.SCITER_REQUEST_ELEMENT_DATA _sciterRequestElementData = null;
				private readonly SciterApi.SCITER_HTTP_REQUEST _sciterHttpRequest = null;
				private readonly SciterApi.SCITER_GET_SCROLL_INFO _sciterGetScrollInfo = null;
				private readonly SciterApi.SCITER_SET_SCROLL_POS _sciterSetScrollPos = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_INTRINSIC_WIDTHS _sciterGetElementIntrinsicWidths = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_INTRINSIC_HEIGHT _sciterGetElementIntrinsicHeight = null;
				private readonly SciterApi.SCITER_IS_ELEMENT_VISIBLE _sciterIsElementVisible = null;
				private readonly SciterApi.SCITER_IS_ELEMENT_ENABLED _sciterIsElementEnabled = null;
				private readonly SciterApi.SCITER_SORT_ELEMENTS _sciterSortElements = null;
				private readonly SciterApi.SCITER_SWAP_ELEMENTS _sciterSwapElements = null;
				private readonly SciterApi.SCITER_TRAVERSE_UI_EVENT _sciterTraverseUIEvent = null;
				private readonly SciterApi.SCITER_CALL_SCRIPTING_METHOD _sciterCallScriptingMethod = null;
				private readonly SciterApi.SCITER_CALL_SCRIPTING_FUNCTION _sciterCallScriptingFunction = null;
				private readonly SciterApi.SCITER_EVAL_ELEMENT_SCRIPT _sciterEvalElementScript = null;
				private readonly SciterApi.SCITER_ATTACH_HWND_TO_ELEMENT _sciterAttachHwndToElement = null;
				private readonly SciterApi.SCITER_CONTROL_GET_TYPE _sciterControlGetType = null;
				private readonly SciterApi.SCITER_GET_VALUE _sciterGetValue = null;
				private readonly SciterApi.SCITER_SET_VALUE _sciterSetValue = null;
				private readonly SciterApi.SCITER_GET_EXPANDO _sciterGetExpando = null;
				private readonly SciterApi.SCITER_GET_OBJECT _sciterGetObject = null;
				private readonly SciterApi.SCITER_GET_ELEMENT_NAMESPACE _sciterGetElementNamespace = null;
				private readonly SciterApi.SCITER_GET_HIGHLIGHTED_ELEMENT _sciterGetHighlightedElement = null;
				private readonly SciterApi.SCITER_SET_HIGHLIGHTED_ELEMENT _sciterSetHighlightedElement = null;
				
				#endregion
			
				#region DOM Node API 
				
				private readonly SciterApi.SCITER_NODE_ADD_REF _sciterNodeAddRef = null;
				private readonly SciterApi.SCITER_NODE_RELEASE _sciterNodeRelease = null;
				private readonly SciterApi.SCITER_NODE_CAST_FROM_ELEMENT _sciterNodeCastFromElement = null;
				private readonly SciterApi.SCITER_NODE_CAST_TO_ELEMENT _sciterNodeCastToElement = null;
				private readonly SciterApi.SCITER_NODE_FIRST_CHILD _sciterNodeFirstChild = null;
				private readonly SciterApi.SCITER_NODE_LAST_CHILD _sciterNodeLastChild = null;
				private readonly SciterApi.SCITER_NODE_NEXT_SIBLING _sciterNodeNextSibling = null;
				private readonly SciterApi.SCITER_NODE_PREV_SIBLING _sciterNodePrevSibling = null;
				private readonly SciterApi.SCITER_NODE_PARENT _sciterNodeParent = null;
				private readonly SciterApi.SCITER_NODE_NTH_CHILD _sciterNodeNthChild = null;
				private readonly SciterApi.SCITER_NODE_CHILDREN_COUNT _sciterNodeChildrenCount = null;
				private readonly SciterApi.SCITER_NODE_TYPE _sciterNodeType = null;
				private readonly SciterApi.SCITER_NODE_GET_TEXT _sciterNodeGetText = null;
				private readonly SciterApi.SCITER_NODE_SET_TEXT _sciterNodeSetText = null;
				private readonly SciterApi.SCITER_NODE_INSERT _sciterNodeInsert = null;
				private readonly SciterApi.SCITER_NODE_REMOVE _sciterNodeRemove = null;
				private readonly SciterApi.SCITER_CREATE_TEXT_NODE _sciterCreateTextNode = null;
				private readonly SciterApi.SCITER_CREATE_COMMENT_NODE _sciterCreateCommentNode = null;
				
				#endregion
	
				#region Value API 
				
				private readonly SciterApi.VALUE_INIT _valueInit = null;
				private readonly SciterApi.VALUE_CLEAR _valueClear = null;
				private readonly SciterApi.VALUE_COMPARE _valueCompare = null;
				private readonly SciterApi.VALUE_COPY _valueCopy = null;
				private readonly SciterApi.VALUE_ISOLATE _valueIsolate = null;
				private readonly SciterApi.VALUE_TYPE _valueType = null;
				private readonly SciterApi.VALUE_STRING_DATA _valueStringData = null;
				private readonly SciterApi.VALUE_STRING_DATA_SET _valueStringDataSet = null;
				private readonly SciterApi.VALUE_INT_DATA _valueIntData = null;
				private readonly SciterApi.VALUE_INT_DATA_SET _valueIntDataSet = null;
				private readonly SciterApi.VALUE_INT_64DATA _valueInt64Data = null;
				private readonly SciterApi.VALUE_INT_64DATA_SET _valueInt64DataSet = null;
				private readonly SciterApi.VALUE_FLOAT_DATA _valueFloatData = null;
				private readonly SciterApi.VALUE_FLOAT_DATA_SET _valueFloatDataSet = null;
				private readonly SciterApi.VALUE_BINARY_DATA _valueBinaryData = null;
				private readonly SciterApi.VALUE_BINARY_DATA_SET _valueBinaryDataSet = null;
				private readonly SciterApi.VALUE_ELEMENTS_COUNT _valueElementsCount = null;
				private readonly SciterApi.VALUE_NTH_ELEMENT_VALUE _valueNthElementValue = null;
				private readonly SciterApi.VALUE_NTH_ELEMENT_VALUE_SET _valueNthElementValueSet = null;
				private readonly SciterApi.VALUE_NTH_ELEMENT_KEY _valueNthElementKey = null;
				private readonly SciterApi.VALUE_ENUM_ELEMENTS _valueEnumElements = null;
				private readonly SciterApi.VALUE_SET_VALUE_TO_KEY _valueSetValueToKey = null;
				private readonly SciterApi.VALUE_GET_VALUE_OF_KEY _valueGetValueOfKey = null;
				private readonly SciterApi.VALUE_TO_STRING _valueToString = null;
				private readonly SciterApi.VALUE_FROM_STRING _valueFromString = null;
				private readonly SciterApi.VALUE_INVOKE _valueInvoke = null;
				private readonly SciterApi.VALUE_NATIVE_FUNCTOR_SET _valueNativeFunctorSet = null;
				private readonly SciterApi.VALUE_IS_NATIVE_FUNCTOR _valueIsNativeFunctor = null;
				
				#endregion
				
				#region TIScript VM API
				
				private readonly SciterApi.TI_SCRIPT_API _tIScriptAPI = null;
				
				private readonly SciterApi.SCITER_GET_VM _sciterGetVM = null;

				private readonly SciterApi.SCITER_v2V _sciter_v2V = null;
				private readonly SciterApi.SCITER_V2v _sciter_V2v = null;
				
				#endregion
			
				#region Archive
				
				private readonly SciterApi.SCITER_OPEN_ARCHIVE _sciterOpenArchive = null;
				private readonly SciterApi.SCITER_GET_ARCHIVE_ITEM _sciterGetArchiveItem = null;
				private readonly SciterApi.SCITER_CLOSE_ARCHIVE _sciterCloseArchive = null;
				
				#endregion

				private readonly SciterApi.SCITER_FIRE_EVENT _sciterFireEvent = null;

				private readonly SciterApi.SCITER_GET_CALLBACK_PARAM _sciterGetCallbackParam = null;
				private readonly SciterApi.SCITER_POST_CALLBACK _sciterPostCallback = null;
				private readonly SciterApi.GET_SCITER_GRAPHICS_API _getSciterGraphicsAPI = null;
				private readonly SciterApi.GET_SCITER_REQUEST_API _getSciterRequestAPI = null;

				#region DirectX
				
				private readonly SciterApi.SCITER_CREATE_ON_DIRECT_X_WINDOW _sciterCreateOnDirectXWindow = null;
				private readonly SciterApi.SCITER_RENDER_ON_DIRECT_X_WINDOW _sciterRenderOnDirectXWindow = null;
				private readonly SciterApi.SCITER_RENDER_ON_DIRECT_X_TEXTURE _sciterRenderOnDirectXTexture = null;
				
				#endregion

				private readonly SciterApi.SCITER_PROC_X _sciterProcX = null; 
				
#pragma warning restore 649

				private NativeSciterApiWrapper(IntPtr sciterApiPtr, object @struct)
				{
					_sciterApiPtr = sciterApiPtr;
					
					var myFieldInfos = this.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
					
					var fieldInfos = @struct.GetType().GetFields();
					foreach (var fieldInfo in fieldInfos)
					{
						
						var updateFieldInfo = myFieldInfos.FirstOrDefault(fd => fd.Name.Equals($"_{char.ToLower(fieldInfo.Name[0])}{fieldInfo.Name.Substring(1, fieldInfo.Name.Length-1)}", StringComparison.Ordinal));
						if (updateFieldInfo != null)
							updateFieldInfo.SetValue(this, fieldInfo.GetValue(@struct));
					}
				}

				internal NativeSciterApiWrapper(IntPtr sciterApiPtr, WindowsSciterApi windowsSciterApi)
					: this(sciterApiPtr, (object)windowsSciterApi) { }

				internal NativeSciterApiWrapper(IntPtr sciterApiPtr, MacOsSciterApi macOsSciterApi)
					: this(sciterApiPtr, (object)macOsSciterApi) { }

				internal NativeSciterApiWrapper(IntPtr sciterApiPtr, LinuxSciterApi linuxSciterApi)
					: this(sciterApiPtr, (object)linuxSciterApi) { }

				public string SciterClassName() =>
					Marshal.PtrToStringUni(_sciterClassName());

				int ISciterApi.Version => _version;

				public uint SciterVersion(bool major) =>
					_sciterVersion(major);

				public Version SciterVersion()
				{
					var major = _sciterVersion(true);
					var minor = _sciterVersion(false);

					return new Version(
						(int) ((major >> 16) & 0xffff),
						(int) (major & 0xffff),
						(int) ((minor >> 16) & 0xffff),
						(int) (minor & 0xffff)
					);
				}
				
				public bool SciterDataReady(IntPtr hwnd, string uri, byte[] data, uint dataLength) =>
					_sciterDataReady(hwnd: hwnd, uri: uri, data: data, dataLength: dataLength);

				public bool SciterDataReadyAsync(IntPtr hwnd, string uri, byte[] data, uint dataLength,
					IntPtr requestId) =>
					_sciterDataReadyAsync(hwnd: hwnd, uri: uri, data: data, dataLength: dataLength,
						requestId: requestId);

				public IntPtr SciterProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam) =>
					_sciterProc(hwnd: hwnd, msg: msg, wParam: wParam, lParam: lParam);

				public IntPtr SciterProcND(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, ref bool pbHandled) =>
					_sciterProcND(hwnd: hwnd, msg: msg, wParam: wParam, lParam: lParam, pbHandled: ref pbHandled);

				public bool SciterLoadFile(IntPtr hwnd, string filename) =>
					_sciterLoadFile(hwnd: hwnd, filename: filename);

				public bool SciterLoadHtml(IntPtr hwnd, byte[] html, uint htmlSize, string baseUrl) =>
					_sciterLoadHtml(hwnd: hwnd, html: html, htmlSize: htmlSize, baseUrl: baseUrl);

				public void SciterSetCallback(IntPtr hwnd, MulticastDelegate cb, IntPtr param) =>
					_sciterSetCallback(hwnd: hwnd, cb: cb, param: param);

				public bool SciterSetMasterCSS(byte[] utf8, uint numBytes) =>
					_sciterSetMasterCSS(utf8, numBytes);

				public bool SciterAppendMasterCSS(byte[] utf8, uint numBytes) =>
					_sciterAppendMasterCSS(utf8, numBytes);

				public bool SciterSetCSS(IntPtr hwnd, byte[] utf8, uint numBytes, string baseUrl, string mediaType) =>
					_sciterSetCSS(hwnd, utf8, numBytes, baseUrl, mediaType);

				public bool SciterSetMediaType(IntPtr hwnd, string mediaType) =>
					_sciterSetMediaType(hwnd: hwnd, mediaType: mediaType);

				public bool SciterSetMediaVars(IntPtr hwnd, ref SciterValue.VALUE mediaVars) =>
					_sciterSetMediaVars(hwnd: hwnd, mediaVars: ref mediaVars);

				public uint SciterGetMinWidth(IntPtr hwnd) =>
					_sciterGetMinWidth(hwnd: hwnd);

				public uint SciterGetMinHeight(IntPtr hwnd, uint width) =>
					_sciterGetMinHeight(hwnd: hwnd, width: width);

				public bool SciterCall(IntPtr hwnd, string functionName, uint argc, SciterValue.VALUE[] argv, out SciterValue.VALUE retval) =>
					_sciterCall(hwnd: hwnd, functionName: functionName, argc: argc, argv: argv, retval: out retval);

				public bool SciterEval(IntPtr hwnd, string script, uint scriptLength, out SciterValue.VALUE pretval) =>
					_sciterEval(hwnd: hwnd, script: script, scriptLength: scriptLength, pretval: out pretval);

				public bool SciterUpdateWindow(IntPtr hwnd) =>
					_sciterUpdateWindow(hwnd: hwnd);

				public bool SciterTranslateMessage(IntPtr lpMsg) =>
					_sciterTranslateMessage(lpMsg: lpMsg);

				public bool SciterSetOption(IntPtr hwnd, SciterXDef.SCITER_RT_OPTIONS option, IntPtr value) =>
					_sciterSetOption(hwnd: hwnd, option: option, value: value);

				public void SciterGetPPI(IntPtr hwnd, ref uint px, ref uint py) =>
					_sciterGetPPI(hwnd: hwnd, px: ref px, py: ref py);

				public bool SciterGetViewExpando(IntPtr hwnd, out SciterValue.VALUE pval) =>
					_sciterGetViewExpando(hwnd: hwnd, pval: out pval);

				public bool SciterRenderD2D(IntPtr hwnd, IntPtr prt) =>
					_sciterRenderD2D(hwnd: hwnd, prt: prt);

				public bool SciterD2DFactory(IntPtr ppf) =>
					_sciterD2DFactory(ppf: ppf);

				public bool SciterDWFactory(IntPtr ppf) =>
					_sciterDWFactory(ppf: ppf);

				public bool SciterGraphicsCaps(ref uint pcaps) =>
					_sciterGraphicsCaps(pcaps: ref pcaps);

				public bool SciterSetHomeURL(IntPtr hwnd, string baseUrl) =>
					_sciterSetHomeURL(hwnd: hwnd, baseUrl: baseUrl);

				public IntPtr SciterCreateNSView(ref PInvokeUtils.RECT frame) =>
					_sciterCreateNSView(frame: ref frame);

				public IntPtr SciterCreateWidget(ref PInvokeUtils.RECT frame) =>
					_sciterCreateWidget(frame: ref frame);

				public IntPtr SciterCreateWindow(SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags, ref PInvokeUtils.RECT frame, MulticastDelegate delegt,
					IntPtr delegateParam, IntPtr parent) =>
					_sciterCreateWindow(creationFlags: creationFlags, frame: ref frame, delegt: delegt, delegateParam: delegateParam, parent: parent);
				
				public void SciterSetupDebugOutput(IntPtr hwndOrNull, IntPtr param, SciterXDef.DEBUG_OUTPUT_PROC pfOutput) =>
					_sciterSetupDebugOutput(hwndOrNull: hwndOrNull, param:  param, pfOutput: pfOutput);

				#region DOM Element API

				public SciterXDom.SCDOM_RESULT Sciter_UseElement(IntPtr he) => 
					_sciter_UseElement(he);

				public SciterXDom.SCDOM_RESULT Sciter_UnuseElement(IntPtr he) => 
					_sciter_UnuseElement(he);

				public SciterXDom.SCDOM_RESULT SciterGetRootElement(IntPtr hwnd, out IntPtr phe) => 
					_sciterGetRootElement(hwnd, out phe);

				public SciterXDom.SCDOM_RESULT SciterGetFocusElement(IntPtr hwnd, out IntPtr phe) => 
					_sciterGetFocusElement(hwnd, out phe);

				public SciterXDom.SCDOM_RESULT SciterFindElement(IntPtr hwnd, PInvokeUtils.POINT pt, out IntPtr phe) => 
					_sciterFindElement(hwnd, pt, out phe);

				public SciterXDom.SCDOM_RESULT SciterGetChildrenCount(IntPtr he, out uint count) => 
					_sciterGetChildrenCount(he, out count);

				public SciterXDom.SCDOM_RESULT SciterGetNthChild(IntPtr he, uint n, out IntPtr phe) => 
					_sciterGetNthChild(he, n, out phe);

				public SciterXDom.SCDOM_RESULT SciterGetParentElement(IntPtr he, out IntPtr pParentHe) => 
					_sciterGetParentElement(he, out pParentHe);

				public SciterXDom.SCDOM_RESULT SciterGetElementHtmlCB(IntPtr he, bool outer, SciterXDom.LPCBYTE_RECEIVER rcv, IntPtr rcvParam) => 
					_sciterGetElementHtmlCB(he, outer, rcv, rcvParam);

				public SciterXDom.SCDOM_RESULT SciterGetElementTextCB(IntPtr he, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_sciterGetElementTextCB(he, rcv, rcvParam);

				public SciterXDom.SCDOM_RESULT SciterSetElementText(IntPtr he, string utf16, uint length) => 
					_sciterSetElementText(he, utf16, length);

				public SciterXDom.SCDOM_RESULT SciterGetAttributeCount(IntPtr he, out uint pCount) => 
					_sciterGetAttributeCount(he, out pCount);

				public SciterXDom.SCDOM_RESULT SciterGetNthAttributeNameCB(IntPtr he, uint n, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_sciterGetNthAttributeNameCB(he, n, rcv, rcvParam);

				public SciterXDom.SCDOM_RESULT SciterGetNthAttributeValueCB(IntPtr he, uint n, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_sciterGetNthAttributeValueCB(he, n, rcv, rcvParam);

				public SciterXDom.SCDOM_RESULT SciterGetAttributeByNameCB(IntPtr he, string name, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_sciterGetAttributeByNameCB(he, name, rcv, rcvParam);

				public SciterXDom.SCDOM_RESULT SciterSetAttributeByName(IntPtr he, string name, string value) => 
					_sciterSetAttributeByName(he, name, value);

				public SciterXDom.SCDOM_RESULT SciterClearAttributes(IntPtr he) => 
					_sciterClearAttributes(he);

				public SciterXDom.SCDOM_RESULT SciterGetElementIndex(IntPtr he, out uint pIndex) => 
					_sciterGetElementIndex(he, out pIndex);

				public SciterXDom.SCDOM_RESULT SciterGetElementType(IntPtr he, out IntPtr pType) => 
					_sciterGetElementType(he, out pType);

				public SciterXDom.SCDOM_RESULT SciterGetElementTypeCB(IntPtr he, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_sciterGetElementTypeCB(he, rcv, rcvParam);

				public SciterXDom.SCDOM_RESULT SciterGetStyleAttributeCB(IntPtr he, string name, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_sciterGetStyleAttributeCB(he, name, rcv, rcvParam);

				public SciterXDom.SCDOM_RESULT SciterSetStyleAttribute(IntPtr he, string name, string value) => 
					_sciterSetStyleAttribute(he, name, value);

				public SciterXDom.SCDOM_RESULT SciterGetElementLocation(IntPtr he, out PInvokeUtils.RECT pLocation, SciterXDom.ELEMENT_AREAS areas) => 
					_sciterGetElementLocation(he, out pLocation, areas);

				public SciterXDom.SCDOM_RESULT SciterScrollToView(IntPtr he, uint sciterScrollFlags) => 
					_sciterScrollToView(he, sciterScrollFlags);

				public SciterXDom.SCDOM_RESULT SciterUpdateElement(IntPtr he, bool andForceRender) => 
					_sciterUpdateElement(he, andForceRender);

				public SciterXDom.SCDOM_RESULT SciterRefreshElementArea(IntPtr he, PInvokeUtils.RECT rc) => 
					_sciterRefreshElementArea(he, rc);

				public SciterXDom.SCDOM_RESULT SciterSetCapture(IntPtr he) => 
					_sciterSetCapture(he);

				public SciterXDom.SCDOM_RESULT SciterReleaseCapture(IntPtr he) => 
					_sciterReleaseCapture(he);

				public SciterXDom.SCDOM_RESULT SciterGetElementHwnd(IntPtr he, out IntPtr pHwnd, bool rootWindow) => 
					_sciterGetElementHwnd(he, out pHwnd, rootWindow);

				public SciterXDom.SCDOM_RESULT SciterCombineURL(IntPtr he, IntPtr szUrlBuffer, uint urlBufferSize) => 
					_sciterCombineURL(he, szUrlBuffer, urlBufferSize);

				public SciterXDom.SCDOM_RESULT SciterSelectElements(IntPtr he, string cssSelectors, SciterXDom.SCITER_ELEMENT_CALLBACK callback, IntPtr param) => 
					_sciterSelectElements(he, cssSelectors, callback, param);

				public SciterXDom.SCDOM_RESULT SciterSelectElementsW(IntPtr he, string cssSelectors, SciterXDom.SCITER_ELEMENT_CALLBACK callback, IntPtr param) => 
					_sciterSelectElementsW(he, cssSelectors, callback, param);

				public SciterXDom.SCDOM_RESULT SciterSelectParent(IntPtr he, string selector, uint depth, out IntPtr heFound) => 
					_sciterSelectParent(he, selector, depth, out heFound);

				public SciterXDom.SCDOM_RESULT SciterSelectParentW(IntPtr he, string selector, uint depth, out IntPtr heFound) => 
					_sciterSelectParentW(he, selector, depth, out heFound);

				public SciterXDom.SCDOM_RESULT SciterSetElementHtml(IntPtr he, byte[] html, uint htmlLength, SciterXDom.SET_ELEMENT_HTML where) => 
					_sciterSetElementHtml(he, html, htmlLength, where);

				public SciterXDom.SCDOM_RESULT SciterGetElementUID(IntPtr he, out uint puid) => 
					_sciterGetElementUID(he, out puid);

				public SciterXDom.SCDOM_RESULT SciterGetElementByUID(IntPtr hwnd, uint uid, out IntPtr phe) => 
					_sciterGetElementByUID(hwnd, uid, out phe);

				public SciterXDom.SCDOM_RESULT SciterShowPopup(IntPtr he, IntPtr heAnchor, uint placement) => 
					_sciterShowPopup(he, heAnchor, placement);

				public SciterXDom.SCDOM_RESULT SciterShowPopupAt(IntPtr he, PInvokeUtils.POINT pos, uint placement) => 
					_sciterShowPopupAt(he, pos, placement);

				public SciterXDom.SCDOM_RESULT SciterHidePopup(IntPtr he) => 
					_sciterHidePopup(he);

				public SciterXDom.SCDOM_RESULT SciterGetElementState(IntPtr he, out uint pstateBits) => 
					_sciterGetElementState(he, out pstateBits);

				public SciterXDom.SCDOM_RESULT SciterSetElementState(IntPtr he, uint stateBitsToSet, uint stateBitsToClear, bool updateView) => 
					_sciterSetElementState(he, stateBitsToSet, stateBitsToClear, updateView);

				public SciterXDom.SCDOM_RESULT SciterCreateElement(string tagname, string textOrNull, out IntPtr phe) => 
					_sciterCreateElement(tagname, textOrNull, out phe);

				public SciterXDom.SCDOM_RESULT SciterCloneElement(IntPtr he, out IntPtr phe) => 
					_sciterCloneElement(he, out phe);

				public SciterXDom.SCDOM_RESULT SciterInsertElement(IntPtr he, IntPtr hparent, uint index) => 
					_sciterInsertElement(he, hparent, index);

				public SciterXDom.SCDOM_RESULT SciterDetachElement(IntPtr he) => 
					_sciterDetachElement(he);

				public SciterXDom.SCDOM_RESULT SciterDeleteElement(IntPtr he) => 
					_sciterDeleteElement(he);

				public SciterXDom.SCDOM_RESULT SciterSetTimer(IntPtr he, uint milliseconds, IntPtr timerId) => 
					_sciterSetTimer(he, milliseconds, timerId);

				public SciterXDom.SCDOM_RESULT SciterDetachEventHandler(IntPtr he, MulticastDelegate pep, IntPtr tag) => 
					_sciterDetachEventHandler(he, pep, tag);

				public SciterXDom.SCDOM_RESULT SciterAttachEventHandler(IntPtr he, MulticastDelegate pep, IntPtr tag) => 
					_sciterAttachEventHandler(he, pep, tag);

				public SciterXDom.SCDOM_RESULT SciterWindowAttachEventHandler(IntPtr hwndLayout, MulticastDelegate pep, IntPtr tag, uint subscription) => 
					_sciterWindowAttachEventHandler(hwndLayout, pep, tag, subscription);

				public SciterXDom.SCDOM_RESULT SciterWindowDetachEventHandler(IntPtr hwndLayout, MulticastDelegate pep, IntPtr tag) => 
					_sciterWindowDetachEventHandler(hwndLayout, pep, tag);

				public SciterXDom.SCDOM_RESULT SciterSendEvent(IntPtr he, uint appEventCode, IntPtr heSource, IntPtr reason, out bool handled) => 
					_sciterSendEvent(he, appEventCode, heSource, reason, out handled);

				public SciterXDom.SCDOM_RESULT SciterPostEvent(IntPtr he, uint appEventCode, IntPtr heSource, IntPtr reason) => 
					_sciterPostEvent(he, appEventCode, heSource, reason);

				public SciterXDom.SCDOM_RESULT SciterCallBehaviorMethod(IntPtr he, ref SciterXDom.METHOD_PARAMS param) => 
					_sciterCallBehaviorMethod(he, ref param);

				public SciterXDom.SCDOM_RESULT SciterRequestElementData(IntPtr he, string url, uint dataType, IntPtr initiator) => 
					_sciterRequestElementData(he, url, dataType, initiator);

				public SciterXDom.SCDOM_RESULT SciterHttpRequest(IntPtr he, string url, uint dataType, uint requestType, ref SciterXDom.REQUEST_PARAM requestParams,
					uint nParams) => 
					_sciterHttpRequest(he, url, dataType, requestType, ref requestParams, nParams);

				public SciterXDom.SCDOM_RESULT SciterGetScrollInfo(IntPtr he, out PInvokeUtils.POINT scrollPos, out PInvokeUtils.RECT viewRect, out PInvokeUtils.SIZE contentSize) => 
					_sciterGetScrollInfo(he, out scrollPos, out viewRect, out contentSize);

				public SciterXDom.SCDOM_RESULT SciterSetScrollPos(IntPtr he, PInvokeUtils.POINT scrollPos, bool smooth) => 
					_sciterSetScrollPos(he, scrollPos, smooth);

				public SciterXDom.SCDOM_RESULT SciterGetElementIntrinsicWidths(IntPtr he, out int pMinWidth, out int pMaxWidth) => 
					_sciterGetElementIntrinsicWidths(he, out pMinWidth, out pMaxWidth);

				public SciterXDom.SCDOM_RESULT SciterGetElementIntrinsicHeight(IntPtr he, int forWidth, out int pHeight) => 
					_sciterGetElementIntrinsicHeight(he, forWidth, out pHeight);

				public SciterXDom.SCDOM_RESULT SciterIsElementVisible(IntPtr he, out bool pVisible) => 
					_sciterIsElementVisible(he, out pVisible);

				public SciterXDom.SCDOM_RESULT SciterIsElementEnabled(IntPtr he, out bool pEnabled) => 
					_sciterIsElementEnabled(he, out pEnabled);

				public SciterXDom.SCDOM_RESULT SciterSortElements(IntPtr he, uint firstIndex, uint lastIndex, SciterXDom.ELEMENT_COMPARATOR cmpFunc,
					IntPtr cmpFuncParam) => 
					_sciterSortElements(he, firstIndex, lastIndex, cmpFunc, cmpFuncParam);

				public SciterXDom.SCDOM_RESULT SciterSwapElements(IntPtr he, IntPtr he2) => 
					_sciterSwapElements(he, he2);

				public SciterXDom.SCDOM_RESULT SciterTraverseUIEvent(IntPtr he, IntPtr eventCtlStruct, out bool bOutProcessed) => 
					_sciterTraverseUIEvent(he, eventCtlStruct, out bOutProcessed);

				public SciterXDom.SCDOM_RESULT SciterCallScriptingMethod(IntPtr he, string name, SciterValue.VALUE[] argv, uint argc, out SciterValue.VALUE retval) => 
					_sciterCallScriptingMethod(he, name, argv, argc, out retval);

				public SciterXDom.SCDOM_RESULT SciterCallScriptingFunction(IntPtr he, string name, SciterValue.VALUE[] argv, uint argc, out SciterValue.VALUE retval) => 
					_sciterCallScriptingFunction(he, name, argv, argc, out retval);

				public SciterXDom.SCDOM_RESULT SciterEvalElementScript(IntPtr he, string script, uint scriptLength, out SciterValue.VALUE retval) => 
					_sciterEvalElementScript(he, script, scriptLength, out retval);

				public SciterXDom.SCDOM_RESULT SciterAttachHwndToElement(IntPtr he, IntPtr hwnd) => 
					_sciterAttachHwndToElement(he, hwnd);

				public SciterXDom.SCDOM_RESULT SciterControlGetType(IntPtr he, out uint pType) => 
					_sciterControlGetType(he, out pType);

				public SciterXDom.SCDOM_RESULT SciterGetValue(IntPtr he, out SciterValue.VALUE pval) => 
					_sciterGetValue(he, out pval);

				public SciterXDom.SCDOM_RESULT SciterSetValue(IntPtr he, ref SciterValue.VALUE pval) => 
					_sciterSetValue(he, ref pval);

				public SciterXDom.SCDOM_RESULT SciterGetExpando(IntPtr he, out SciterValue.VALUE pval, bool forceCreation) => 
					_sciterGetExpando(he, out pval, forceCreation);

				public SciterXDom.SCDOM_RESULT SciterGetObject(IntPtr he, out TIScript.tiscript_value pval, bool forceCreation) => 
					_sciterGetObject(he, out pval, forceCreation);

				public SciterXDom.SCDOM_RESULT SciterGetElementNamespace(IntPtr he, out TIScript.tiscript_value pval) => 
					_sciterGetElementNamespace(he, out pval);

				public SciterXDom.SCDOM_RESULT SciterGetHighlightedElement(IntPtr hwnd, out IntPtr phe) => 
					_sciterGetHighlightedElement(hwnd, out phe);

				public SciterXDom.SCDOM_RESULT SciterSetHighlightedElement(IntPtr hwnd, IntPtr he) => 
					_sciterSetHighlightedElement(hwnd, he);

				public SciterXDom.SCDOM_RESULT SciterNodeAddRef(IntPtr hn) => 
					_sciterNodeAddRef(hn);

				public SciterXDom.SCDOM_RESULT SciterNodeRelease(IntPtr hn) => 
					_sciterNodeRelease(hn);

				public SciterXDom.SCDOM_RESULT SciterNodeCastFromElement(IntPtr he, out IntPtr phn) => 
					_sciterNodeCastFromElement(he, out phn);

				public SciterXDom.SCDOM_RESULT SciterNodeCastToElement(IntPtr hn, out IntPtr he) => 
					_sciterNodeCastToElement(hn, out he);

				public SciterXDom.SCDOM_RESULT SciterNodeFirstChild(IntPtr hn, out IntPtr phn) => 
					_sciterNodeFirstChild(hn, out phn);

				public SciterXDom.SCDOM_RESULT SciterNodeLastChild(IntPtr hn, out IntPtr phn) => 
					_sciterNodeLastChild(hn, out phn);

				public SciterXDom.SCDOM_RESULT SciterNodeNextSibling(IntPtr hn, out IntPtr phn) => 
					_sciterNodeNextSibling(hn, out phn);

				public SciterXDom.SCDOM_RESULT SciterNodePrevSibling(IntPtr hn, out IntPtr phn) => 
					_sciterNodePrevSibling(hn, out phn);

				public SciterXDom.SCDOM_RESULT SciterNodeParent(IntPtr hn, out IntPtr pheParent) => 
					_sciterNodeParent(hn, out pheParent);

				public SciterXDom.SCDOM_RESULT SciterNodeNthChild(IntPtr hn, uint n, out IntPtr phn) => 
					_sciterNodeNthChild(hn, n, out phn);

				public SciterXDom.SCDOM_RESULT SciterNodeChildrenCount(IntPtr hn, out uint pn) => 
					_sciterNodeChildrenCount(hn, out pn);

				public SciterXDom.SCDOM_RESULT SciterNodeType(IntPtr hn, out SciterXDom.NODE_TYPE pn) => 
					_sciterNodeType(hn, out pn);

				public SciterXDom.SCDOM_RESULT SciterNodeGetText(IntPtr hn, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam) => 
					_sciterNodeGetText(hn, rcv, rcvParam);

				public SciterXDom.SCDOM_RESULT SciterNodeSetText(IntPtr hn, string text, uint textLength) => 
					_sciterNodeSetText(hn, text, textLength);

				public SciterXDom.SCDOM_RESULT SciterNodeInsert(IntPtr hn, uint @where, IntPtr what) => 
					_sciterNodeInsert(hn, @where, what);

				public SciterXDom.SCDOM_RESULT SciterNodeRemove(IntPtr hn, bool finalize) => 
					_sciterNodeRemove(hn, finalize);

				public SciterXDom.SCDOM_RESULT SciterCreateTextNode(string text, uint textLength, out IntPtr phnode) => 
					_sciterCreateTextNode(text, textLength, out phnode);

				public SciterXDom.SCDOM_RESULT SciterCreateCommentNode(string text, uint textLength, out IntPtr phnode) => 
					_sciterCreateCommentNode(text, textLength, out phnode);

				public SciterValue.VALUE_RESULT ValueInit(out SciterValue.VALUE pval) => 
					_valueInit(out pval);

				public SciterValue.VALUE_RESULT ValueClear(out SciterValue.VALUE pval) => 
					_valueClear(out pval);

				public SciterValue.VALUE_RESULT ValueCompare(ref SciterValue.VALUE pval, ref IntPtr pval2) => 
					_valueCompare(ref pval, ref pval2);

				public SciterValue.VALUE_RESULT ValueCopy(out SciterValue.VALUE pdst, ref SciterValue.VALUE psrc) => 
					_valueCopy(out pdst, ref psrc);

				public SciterValue.VALUE_RESULT ValueIsolate(ref SciterValue.VALUE pdst) => 
					_valueIsolate(ref pdst);

				public SciterValue.VALUE_RESULT ValueType(ref SciterValue.VALUE pval, out uint pType, out uint pUnits) => 
					_valueType(ref pval, out pType, out pUnits);

				public SciterValue.VALUE_RESULT ValueStringData(ref SciterValue.VALUE pval, out IntPtr pChars, out uint pNumChars) => 
					_valueStringData(ref pval, out pChars, out pNumChars);

				public SciterValue.VALUE_RESULT ValueStringDataSet(ref SciterValue.VALUE pval, string chars, uint numChars, uint units) => 
					_valueStringDataSet(ref pval, chars, numChars, units);

				public SciterValue.VALUE_RESULT ValueIntData(ref SciterValue.VALUE pval, out int pData) => 
					_valueIntData(ref pval, out pData);

				public SciterValue.VALUE_RESULT ValueIntDataSet(ref SciterValue.VALUE pval, int data, uint type, uint units) => 
					_valueIntDataSet(ref pval, data, type, units);

				public SciterValue.VALUE_RESULT ValueInt64Data(ref SciterValue.VALUE pval, out long pData) => 
					_valueInt64Data(ref pval, out pData);

				public SciterValue.VALUE_RESULT ValueInt64DataSet(ref SciterValue.VALUE pval, long data, uint type, uint units) => 
					_valueInt64DataSet(ref pval, data, type, units);

				public SciterValue.VALUE_RESULT ValueFloatData(ref SciterValue.VALUE pval, out double pData) => 
					_valueFloatData(ref pval, out pData);

				public SciterValue.VALUE_RESULT ValueFloatDataSet(ref SciterValue.VALUE pval, double data, uint type, uint units) => 
					_valueFloatDataSet(ref pval, data, type, units);

				public SciterValue.VALUE_RESULT ValueBinaryData(ref SciterValue.VALUE pval, out IntPtr pBytes, out uint pnBytes) => 
					_valueBinaryData(ref pval, out pBytes, out pnBytes);

				public SciterValue.VALUE_RESULT ValueBinaryDataSet(ref SciterValue.VALUE pval, byte[] pBytes, uint nBytes, uint type, uint units) => 
					_valueBinaryDataSet(ref pval, pBytes, nBytes, type, units);

				public SciterValue.VALUE_RESULT ValueElementsCount(ref SciterValue.VALUE pval, out int pn) => 
					_valueElementsCount(ref pval, out pn);

				public SciterValue.VALUE_RESULT ValueNthElementValue(ref SciterValue.VALUE pval, int n, out SciterValue.VALUE pretval) => 
					_valueNthElementValue(ref pval, n, out pretval);

				public SciterValue.VALUE_RESULT ValueNthElementValueSet(ref SciterValue.VALUE pval, int n, ref SciterValue.VALUE pvalToSet) => 
					_valueNthElementValueSet(ref pval, n, ref pvalToSet);

				public SciterValue.VALUE_RESULT ValueNthElementKey(ref SciterValue.VALUE pval, int n, out SciterValue.VALUE pretval) => 
					_valueNthElementKey(ref pval, n, out pretval);

				public SciterValue.VALUE_RESULT ValueEnumElements(ref SciterValue.VALUE pval, SciterValue.KEY_VALUE_CALLBACK penum, IntPtr param) => 
					_valueEnumElements(ref pval, penum, param);

				public SciterValue.VALUE_RESULT ValueSetValueToKey(ref SciterValue.VALUE pval, ref SciterValue.VALUE pkey, ref SciterValue.VALUE pvalToSet) => 
					_valueSetValueToKey(ref pval, ref pkey, ref pvalToSet);

				public SciterValue.VALUE_RESULT ValueGetValueOfKey(ref SciterValue.VALUE pval, ref SciterValue.VALUE pkey, out SciterValue.VALUE pretval) => 
					_valueGetValueOfKey(ref pval, ref pkey, out pretval);

				public SciterValue.VALUE_RESULT ValueToString(ref SciterValue.VALUE pval, SciterValue.VALUE_STRING_CVT_TYPE how) => 
					_valueToString(ref pval, how);

				public SciterValue.VALUE_RESULT ValueFromString(ref SciterValue.VALUE pval, string str, uint strLength, uint how) => 
					_valueFromString(ref pval, str, strLength, how);

				public SciterValue.VALUE_RESULT ValueInvoke(ref SciterValue.VALUE pval, ref SciterValue.VALUE pthis, uint argc, SciterValue.VALUE[] argv, out SciterValue.VALUE pretval, string url) => 
					_valueInvoke(ref pval, ref pthis, argc, argv, out pretval, url);

				public SciterValue.VALUE_RESULT ValueNativeFunctorSet(ref SciterValue.VALUE pval,
					SciterValue.NATIVE_FUNCTOR_INVOKE pinvoke, SciterValue.NATIVE_FUNCTOR_RELEASE prelease,
					IntPtr tag) =>
					_valueNativeFunctorSet(ref pval, pinvoke, prelease, tag);

				public SciterValue.VALUE_RESULT ValueIsNativeFunctor(ref SciterValue.VALUE pval) => 
					_valueIsNativeFunctor(ref pval);

				public IntPtr TIScriptAPI() => 
					_tIScriptAPI();

				[Obsolete("Deprecated in v4.4.3.24", true)]
				public IntPtr SciterGetVM(IntPtr hwnd) => 
					_sciterGetVM(hwnd);

				public bool Sciter_v2V(IntPtr vm, TIScript.tiscript_value scriptValue, ref SciterValue.VALUE value,
					bool isolate) =>
					_sciter_v2V(vm, scriptValue, ref value, isolate);

				public bool Sciter_V2v(IntPtr vm, ref SciterValue.VALUE value, ref TIScript.tiscript_value scriptValue) =>
					_sciter_V2v(vm, ref value, ref scriptValue);
				
				public IntPtr SciterOpenArchive(IntPtr archiveData, uint archiveDataLength) => 
					_sciterOpenArchive(archiveData, archiveDataLength);

				public bool SciterGetArchiveItem(IntPtr harc, string path, out IntPtr pdata, out uint pdataLength) => 
					_sciterGetArchiveItem(harc, path, out pdata, out pdataLength);

				public bool SciterCloseArchive(IntPtr harc) => 
					_sciterCloseArchive(harc);

				public SciterXDom.SCDOM_RESULT SciterFireEvent(ref SciterBehaviors.BEHAVIOR_EVENT_PARAMS evt, bool post, out bool handled) => 
					_sciterFireEvent(ref evt, post, out handled);

				public IntPtr SciterGetCallbackParam(IntPtr hwnd) => 
					_sciterGetCallbackParam(hwnd);

				public IntPtr SciterPostCallback(IntPtr hwnd, IntPtr wparam, IntPtr lparam, uint timeoutms) => 
					_sciterPostCallback(hwnd, wparam, lparam, timeoutms);

				public IntPtr GetSciterGraphicsAPI() => 
					_getSciterGraphicsAPI();

				public IntPtr GetSciterRequestAPI() => 
					_getSciterRequestAPI();

				public bool SciterCreateOnDirectXWindow(IntPtr hwnd, IntPtr pSwapChain) => 
					_sciterCreateOnDirectXWindow(hwnd, pSwapChain);

				public bool SciterRenderOnDirectXWindow(IntPtr hwnd, IntPtr elementToRenderOrNull, bool frontLayer) => 
					_sciterRenderOnDirectXWindow(hwnd, elementToRenderOrNull, frontLayer);

				public bool SciterRenderOnDirectXTexture(IntPtr hwnd, IntPtr elementToRenderOrNull, IntPtr surface) => 
					_sciterRenderOnDirectXTexture(hwnd, elementToRenderOrNull, surface);

				public bool SciterProcX(IntPtr hwnd, IntPtr pMsg) => 
					_sciterProcX(hwnd, pMsg);

				#endregion
			}
		}
	}
}