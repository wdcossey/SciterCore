// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Reflection.Emit;
using SciterCore.Attributes;
using SciterCore.Interop;
using SciterTest.CoreForms.Extensions;

// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore.Interop
{
	public static partial class Sciter
	{
		private static readonly object SciterApiLock = new object();
		private static readonly object SciterGraphicsApiLock = new object();
		private static readonly object SciterRequestApiLock = new object();
		private static readonly object SciterScriptApiLock = new object();
		
		// TODO: Rename to SciterApi
		public static ISciterApi Api => GetSciterApi();

		public static ISciterGraphicsApi GraphicsApi => GetGraphicsApi();

		public static ISciterRequestApi RequestApi => GetRequestApi();
		
		[Obsolete("Removed in Sciter v4.4.3.24", false)]
        public static ISciterScriptApi ScriptApi => GetScriptApi();
		
        private static ISciterApi _sciterApi = null;
		private static ISciterGraphicsApi _sciterGraphicsApi = null;
		private static ISciterRequestApi _sciterRequestApiInstance = null;
		private static ISciterScriptApi _sciterScriptApi = null;
	
		// ReSharper disable InconsistentNaming
		private const string SciterWindowsLibrary = "sciter.dll";
		private const string SciterUnixLibrary = "libsciter-gtk.so";
		private const string SciterMacOSLibrary = "sciter-osx-64.dylib";
		// ReSharper enable InconsistentNaming
		
#if NETCOREAPP3_1
		
		/// <summary>
		/// Name is purely to avoid collision
		/// </summary>
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
			lock (SciterApiLock)
			{
				if (_sciterApi != null)
					return _sciterApi;

				//var apiStructSize = Marshal.SizeOf(typeof(SciterApi));

#if NETCOREAPP3_1
				NativeLibrary.SetDllImportResolver(typeof(Sciter).Assembly, ImportResolver);
#elif WINDOWS || NETCORE

				var codeBasePath = new Uri(typeof(SciterApiDelegates).Assembly.CodeBase).LocalPath;
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

				_sciterApi = UnsafeNativeMethods.GetApiInterface();

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
		}

		private static ISciterGraphicsApi GetGraphicsApi()
		{
			lock (SciterGraphicsApiLock)
			{
				if (_sciterGraphicsApi != null)
					return _sciterGraphicsApi;
			
				var major = Api.SciterVersion(true);
				var minor = Api.SciterVersion(false);
				Debug.Assert(major >= 0x00040000);

				var apiStructSize = Marshal.SizeOf(t: typeof(SciterGraphics.SciterGraphicsApi));
			
				if(IntPtr.Size == 8)
					Debug.Assert(apiStructSize == 276 * 2);
				else
					Debug.Assert(apiStructSize == 276);

				_sciterGraphicsApi = SciterGraphics.UnsafeNativeMethods.GetApiInterface(Api);

				if (_sciterGraphicsApi == null)
					throw new NullReferenceException($"{nameof(_sciterGraphicsApi)} cannot be null");
			
				return _sciterGraphicsApi;
			}
		}

		private static ISciterRequestApi GetRequestApi()
		{
			lock (SciterRequestApiLock)
			{
				if (_sciterRequestApiInstance != null)
					return _sciterRequestApiInstance;
				
				var apiStructSize = Marshal.SizeOf(t: typeof(SciterRequest.SciterRequestApi));
				
				if (IntPtr.Size == 8)
					Debug.Assert(apiStructSize == 112 * 2);
				else
					Debug.Assert(apiStructSize == 112);
				
				_sciterRequestApiInstance = SciterRequest.UnsafeNativeMethods.GetApiInterface(Api);
				
				if (_sciterRequestApiInstance == null)
					throw new NullReferenceException($"{nameof(_sciterRequestApiInstance)} cannot be null");
				
				return _sciterRequestApiInstance;
			}
		}
		
		private static ISciterScriptApi GetScriptApi()
        {
	        lock (SciterRequestApiLock)
	        {
		        if (_sciterScriptApi != null)
			        return _sciterScriptApi;
				
		        var apiStructSize = Marshal.SizeOf(t: typeof(SciterScript.SciterScriptApi));
				
		        if (IntPtr.Size == 8)
			        Debug.Assert(apiStructSize == 616);
		        else
			        Debug.Assert(apiStructSize == 308);
				
		        _sciterScriptApi = SciterScript.UnsafeNativeMethods.GetApiInterface(Api);
				
		        if (_sciterScriptApi == null)
			        throw new NullReferenceException($"{nameof(_sciterScriptApi)} cannot be null");
				
		        return _sciterScriptApi;
	        }
        }

		internal static class UnsafeNativeMethods
		{
			public static ISciterApi GetApiInterface()
			{
				var sciterApiPtr = SciterAPI();
				var dynamicType = DynamicApiHelper.GetDynamicSciterApi(sciterApiPtr);
				return new NativeSciterApiWrapper(dynamicType, sciterApiPtr);
			}
			
			private static class DynamicApiHelper
			{
				private static IEnumerable
					<(OSPlatform Platform, SciterOSPlatform RuntimePlatform)?> EnumeratePlatforms()
				{
					yield return (OSPlatform.Windows, SciterOSPlatform.Windows);
					yield return (OSPlatform.OSX, SciterOSPlatform.MacOS);
					yield return (OSPlatform.Linux, SciterOSPlatform.Linux);
				}

				private static SciterOSPlatform GetSciterPlatform()
				{
					return EnumeratePlatforms().FirstOrDefault(p
						=> p != null && RuntimeInformation.IsOSPlatform(p.Value.Platform))?.RuntimePlatform ?? default;
				}

				public static TypeInfo GetDynamicSciterApi(IntPtr sciterApiPtr)
				{
					var sciterOS = GetSciterPlatform();
					
					var versionApi = Marshal.PtrToStructure<SciterVersionApi>(sciterApiPtr);
					var major = versionApi.SciterVersion(true);
					var minor = versionApi.SciterVersion(false);

					var version = new Version(
						(int) ((major >> 16) & 0xffff),
						(int) (major & 0xffff),
						(int) ((minor >> 16) & 0xffff),
						(int) (minor & 0xffff));
					
					var assemblyName = new AssemblyName(Guid.NewGuid().ToString());
					var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
					var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.ToString());
					
					var typeBuilder = moduleBuilder.DefineType("DynamicSciterApi",
						TypeAttributes.NotPublic |
						TypeAttributes.Sealed |
						TypeAttributes.SequentialLayout |
						TypeAttributes.Serializable |
						TypeAttributes.AnsiClass,
						typeof(ValueType));
				
					var fieldInfoDictionary = typeof(DynamicSciterApi)
						.GetFields(BindingFlags.Instance | BindingFlags.Public)
						.AsQueryable();

					fieldInfoDictionary = fieldInfoDictionary
						.Where(w => 
							w.GetCustomAttribute<SciterApiOSPlatformAttribute>() == null || 
							w.GetCustomAttribute<SciterApiOSPlatformAttribute>().Platform == sciterOS);

					fieldInfoDictionary = fieldInfoDictionary
						.Where(w => 
							w.GetCustomAttribute<SciterApiMinVersionAttribute>() == null || 
							(w.GetCustomAttribute<SciterApiMinVersionAttribute>().Version.CompareTo(version) <= 0));

					fieldInfoDictionary = fieldInfoDictionary
						.Where(w => 
							w.GetCustomAttribute<SciterApiMaxVersionAttribute>() == null || 
							(w.GetCustomAttribute<SciterApiMaxVersionAttribute>().Version.CompareTo(version) >= 0));
				
					foreach (var fieldInfo in fieldInfoDictionary)
						typeBuilder.DefineField(fieldInfo.Name, fieldInfo.FieldType, FieldAttributes.Public);

					return typeBuilder.CreateTypeInfo();
				}
			}

			private sealed class NativeSciterApiWrapper : ISciterApi
			{
				private IntPtr _apiPtr;
				
#pragma warning disable 649
				private readonly int _version;
				private readonly SciterApiDelegates.SciterClassName _sciterClassName = null;
				private readonly SciterApiDelegates.SciterVersion _sciterVersion = null;
				private readonly SciterApiDelegates.SciterDataReady _sciterDataReady = null;
				private readonly SciterApiDelegates.SciterDataReadyAsync _sciterDataReadyAsync = null;
				private readonly SciterApiDelegates.SciterProc _sciterProc = null;
				private readonly SciterApiDelegates.SciterProcNd _sciterProcND = null;
				private readonly SciterApiDelegates.SciterLoadFile _sciterLoadFile = null;
				private readonly SciterApiDelegates.SciterLoadHtml _sciterLoadHtml = null;
				private readonly SciterApiDelegates.SciterSetCallback _sciterSetCallback = null;
				private readonly SciterApiDelegates.SciterSetMasterCss _sciterSetMasterCSS = null;
				private readonly SciterApiDelegates.SciterAppendMasterCss _sciterAppendMasterCSS = null;
				private readonly SciterApiDelegates.SciterSetCss _sciterSetCSS = null;
				private readonly SciterApiDelegates.SciterSetMediaType _sciterSetMediaType = null;
				private readonly SciterApiDelegates.SciterSetMediaVars _sciterSetMediaVars = null;
				private readonly SciterApiDelegates.SciterGetMinWidth _sciterGetMinWidth = null;
				private readonly SciterApiDelegates.SciterGetMinHeight _sciterGetMinHeight = null;
				private readonly SciterApiDelegates.SciterCall _sciterCall = null;
				private readonly SciterApiDelegates.SciterEval _sciterEval = null;
				private readonly SciterApiDelegates.SciterUpdateWindow _sciterUpdateWindow = null;
				
				private readonly SciterApiDelegates.SciterTranslateMessage _sciterTranslateMessage = null;
				private readonly SciterApiDelegates.SciterSetOption _sciterSetOption = null;
				private readonly SciterApiDelegates.SciterGetPpi _sciterGetPPI = null;
				private readonly SciterApiDelegates.SciterGetViewExpando _sciterGetViewExpando = null;
				private readonly SciterApiDelegates.SciterRenderD2D _sciterRenderD2D = null;
				private readonly SciterApiDelegates.SciterD2DFactory _sciterD2DFactory = null;
				private readonly SciterApiDelegates.SciterDwFactory _sciterDWFactory = null;
				private readonly SciterApiDelegates.SciterGraphicsCaps _sciterGraphicsCaps = null;
				private readonly SciterApiDelegates.SciterSetHomeUrl _sciterSetHomeURL = null;
				private readonly SciterApiDelegates.SciterCreateNsView _sciterCreateNSView = null;
				private readonly SciterApiDelegates.SciterCreateWidget _sciterCreateWidget = null;

				private readonly SciterApiDelegates.SciterCreateWindow _sciterCreateWindow = null;
				private readonly SciterApiDelegates.SciterSetupDebugOutput _sciterSetupDebugOutput = null;
				
				#region DOM Element API
				
				private readonly SciterApiDelegates.SciterUseElement _sciter_UseElement = null;
				private readonly SciterApiDelegates.SciterUnuseElement _sciter_UnuseElement = null;
				private readonly SciterApiDelegates.SciterGetRootElement _sciterGetRootElement = null;
				private readonly SciterApiDelegates.SciterGetFocusElement _sciterGetFocusElement = null;
				private readonly SciterApiDelegates.SciterFindElement _sciterFindElement = null;
				private readonly SciterApiDelegates.SciterGetChildrenCount _sciterGetChildrenCount = null;
				private readonly SciterApiDelegates.SciterGetNthChild _sciterGetNthChild = null;
				private readonly SciterApiDelegates.SciterGetParentElement _sciterGetParentElement = null;
				private readonly SciterApiDelegates.SciterGetElementHtmlCb _sciterGetElementHtmlCB = null;
				private readonly SciterApiDelegates.SciterGetElementTextCb _sciterGetElementTextCB = null;
				private readonly SciterApiDelegates.SciterSetElementText _sciterSetElementText = null;
				private readonly SciterApiDelegates.SciterGetAttributeCount _sciterGetAttributeCount = null;
				private readonly SciterApiDelegates.SciterGetNthAttributeNameCb _sciterGetNthAttributeNameCB = null;
				private readonly SciterApiDelegates.SciterGetNthAttributeValueCb _sciterGetNthAttributeValueCB = null;
				private readonly SciterApiDelegates.SciterGetAttributeByNameCb _sciterGetAttributeByNameCB = null;
				private readonly SciterApiDelegates.SciterSetAttributeByName _sciterSetAttributeByName = null;
				private readonly SciterApiDelegates.SciterClearAttributes _sciterClearAttributes = null;
				private readonly SciterApiDelegates.SciterGetElementIndex _sciterGetElementIndex = null;
				private readonly SciterApiDelegates.SciterGetElementType _sciterGetElementType = null;
				private readonly SciterApiDelegates.SciterGetElementTypeCb _sciterGetElementTypeCB = null;
				private readonly SciterApiDelegates.SciterGetStyleAttributeCb _sciterGetStyleAttributeCB = null;
				private readonly SciterApiDelegates.SciterSetStyleAttribute _sciterSetStyleAttribute = null;
				private readonly SciterApiDelegates.SciterGetElementLocation _sciterGetElementLocation = null;
				private readonly SciterApiDelegates.SciterScrollToView _sciterScrollToView = null;
				private readonly SciterApiDelegates.SciterUpdateElement _sciterUpdateElement = null;
				private readonly SciterApiDelegates.SciterRefreshElementArea _sciterRefreshElementArea = null;
				private readonly SciterApiDelegates.SciterSetCapture _sciterSetCapture = null;
				private readonly SciterApiDelegates.SciterReleaseCapture _sciterReleaseCapture = null;
				private readonly SciterApiDelegates.SciterGetElementHwnd _sciterGetElementHwnd = null;
				private readonly SciterApiDelegates.SciterCombineUrl _sciterCombineURL = null;
				private readonly SciterApiDelegates.SciterSelectElements _sciterSelectElements = null;
				private readonly SciterApiDelegates.SciterSelectElementsW _sciterSelectElementsW = null;
				private readonly SciterApiDelegates.SciterSelectParent _sciterSelectParent = null;
				private readonly SciterApiDelegates.SciterSelectParentW _sciterSelectParentW = null;
				private readonly SciterApiDelegates.SciterSetElementHtml _sciterSetElementHtml = null;
				private readonly SciterApiDelegates.SciterGetElementUid _sciterGetElementUID = null;
				private readonly SciterApiDelegates.SciterGetElementByUid _sciterGetElementByUID = null;
				private readonly SciterApiDelegates.SciterShowPopup _sciterShowPopup = null;
				private readonly SciterApiDelegates.SciterShowPopupAt _sciterShowPopupAt = null;
				private readonly SciterApiDelegates.SciterHidePopup _sciterHidePopup = null;
				private readonly SciterApiDelegates.SciterGetElementState _sciterGetElementState = null;
				private readonly SciterApiDelegates.SciterSetElementState _sciterSetElementState = null;
				private readonly SciterApiDelegates.SciterCreateElement _sciterCreateElement = null;
				private readonly SciterApiDelegates.SciterCloneElement _sciterCloneElement = null;
				private readonly SciterApiDelegates.SciterInsertElement _sciterInsertElement = null;
				private readonly SciterApiDelegates.SciterDetachElement _sciterDetachElement = null;
				private readonly SciterApiDelegates.SciterDeleteElement _sciterDeleteElement = null;
				private readonly SciterApiDelegates.SciterSetTimer _sciterSetTimer = null;
				private readonly SciterApiDelegates.SciterDetachEventHandler _sciterDetachEventHandler = null;
				private readonly SciterApiDelegates.SciterAttachEventHandler _sciterAttachEventHandler = null;
				private readonly SciterApiDelegates.SciterWindowAttachEventHandler _sciterWindowAttachEventHandler = null;
				private readonly SciterApiDelegates.SciterWindowDetachEventHandler _sciterWindowDetachEventHandler = null;
				private readonly SciterApiDelegates.SciterSendEvent _sciterSendEvent = null;
				private readonly SciterApiDelegates.SciterPostEvent _sciterPostEvent = null;
				private readonly SciterApiDelegates.SciterCallBehaviorMethod _sciterCallBehaviorMethod = null;
				private readonly SciterApiDelegates.SciterRequestElementData _sciterRequestElementData = null;
				private readonly SciterApiDelegates.SciterHttpRequest _sciterHttpRequest = null;
				private readonly SciterApiDelegates.SciterGetScrollInfo _sciterGetScrollInfo = null;
				private readonly SciterApiDelegates.SciterSetScrollPos _sciterSetScrollPos = null;
				private readonly SciterApiDelegates.SciterGetElementIntrinsicWidths _sciterGetElementIntrinsicWidths = null;
				private readonly SciterApiDelegates.SciterGetElementIntrinsicHeight _sciterGetElementIntrinsicHeight = null;
				private readonly SciterApiDelegates.SciterIsElementVisible _sciterIsElementVisible = null;
				private readonly SciterApiDelegates.SciterIsElementEnabled _sciterIsElementEnabled = null;
				private readonly SciterApiDelegates.SciterSortElements _sciterSortElements = null;
				private readonly SciterApiDelegates.SciterSwapElements _sciterSwapElements = null;
				private readonly SciterApiDelegates.SciterTraverseUiEvent _sciterTraverseUIEvent = null;
				private readonly SciterApiDelegates.SciterCallScriptingMethod _sciterCallScriptingMethod = null;
				private readonly SciterApiDelegates.SciterCallScriptingFunction _sciterCallScriptingFunction = null;
				private readonly SciterApiDelegates.SciterEvalElementScript _sciterEvalElementScript = null;
				private readonly SciterApiDelegates.SciterAttachHwndToElement _sciterAttachHwndToElement = null;
				private readonly SciterApiDelegates.SciterControlGetType _sciterControlGetType = null;
				private readonly SciterApiDelegates.SciterGetValue _sciterGetValue = null;
				private readonly SciterApiDelegates.SciterSetValue _sciterSetValue = null;
				private readonly SciterApiDelegates.SciterGetExpando _sciterGetExpando = null;
				private readonly SciterApiDelegates.SciterGetObject _sciterGetObject = null;
				private readonly SciterApiDelegates.SciterGetElementNamespace _sciterGetElementNamespace = null;
				private readonly SciterApiDelegates.SciterGetHighlightedElement _sciterGetHighlightedElement = null;
				private readonly SciterApiDelegates.SciterSetHighlightedElement _sciterSetHighlightedElement = null;
				
				#endregion
			
				#region DOM Node API 
				
				private readonly SciterApiDelegates.SciterNodeAddRef _sciterNodeAddRef = null;
				private readonly SciterApiDelegates.SciterNodeRelease _sciterNodeRelease = null;
				private readonly SciterApiDelegates.SciterNodeCastFromElement _sciterNodeCastFromElement = null;
				private readonly SciterApiDelegates.SciterNodeCastToElement _sciterNodeCastToElement = null;
				private readonly SciterApiDelegates.SciterNodeFirstChild _sciterNodeFirstChild = null;
				private readonly SciterApiDelegates.SciterNodeLastChild _sciterNodeLastChild = null;
				private readonly SciterApiDelegates.SciterNodeNextSibling _sciterNodeNextSibling = null;
				private readonly SciterApiDelegates.SciterNodePrevSibling _sciterNodePrevSibling = null;
				private readonly SciterApiDelegates.SciterNodeParent _sciterNodeParent = null;
				private readonly SciterApiDelegates.SciterNodeNthChild _sciterNodeNthChild = null;
				private readonly SciterApiDelegates.SciterNodeChildrenCount _sciterNodeChildrenCount = null;
				private readonly SciterApiDelegates.SciterNodeType _sciterNodeType = null;
				private readonly SciterApiDelegates.SciterNodeGetText _sciterNodeGetText = null;
				private readonly SciterApiDelegates.SciterNodeSetText _sciterNodeSetText = null;
				private readonly SciterApiDelegates.SciterNodeInsert _sciterNodeInsert = null;
				private readonly SciterApiDelegates.SciterNodeRemove _sciterNodeRemove = null;
				private readonly SciterApiDelegates.SciterCreateTextNode _sciterCreateTextNode = null;
				private readonly SciterApiDelegates.SciterCreateCommentNode _sciterCreateCommentNode = null;
				
				#endregion
	
				#region Value API 
				
				private readonly SciterApiDelegates.ValueInit _valueInit = null;
				private readonly SciterApiDelegates.ValueClear _valueClear = null;
				private readonly SciterApiDelegates.ValueCompare _valueCompare = null;
				private readonly SciterApiDelegates.ValueCopy _valueCopy = null;
				private readonly SciterApiDelegates.ValueIsolate _valueIsolate = null;
				private readonly SciterApiDelegates.ValueType _valueType = null;
				private readonly SciterApiDelegates.ValueStringData _valueStringData = null;
				private readonly SciterApiDelegates.ValueStringDataSet _valueStringDataSet = null;
				private readonly SciterApiDelegates.ValueIntData _valueIntData = null;
				private readonly SciterApiDelegates.ValueIntDataSet _valueIntDataSet = null;
				private readonly SciterApiDelegates.ValueInt64Data _valueInt64Data = null;
				private readonly SciterApiDelegates.ValueInt64DataSet _valueInt64DataSet = null;
				private readonly SciterApiDelegates.ValueFloatData _valueFloatData = null;
				private readonly SciterApiDelegates.ValueFloatDataSet _valueFloatDataSet = null;
				private readonly SciterApiDelegates.ValueBinaryData _valueBinaryData = null;
				private readonly SciterApiDelegates.ValueBinaryDataSet _valueBinaryDataSet = null;
				private readonly SciterApiDelegates.ValueElementsCount _valueElementsCount = null;
				private readonly SciterApiDelegates.ValueNthElementValue _valueNthElementValue = null;
				private readonly SciterApiDelegates.ValueNthElementValueSet _valueNthElementValueSet = null;
				private readonly SciterApiDelegates.ValueNthElementKey _valueNthElementKey = null;
				private readonly SciterApiDelegates.ValueEnumElements _valueEnumElements = null;
				private readonly SciterApiDelegates.ValueSetValueToKey _valueSetValueToKey = null;
				private readonly SciterApiDelegates.ValueGetValueOfKey _valueGetValueOfKey = null;
				private readonly SciterApiDelegates.ValueToString _valueToString = null;
				private readonly SciterApiDelegates.ValueFromString _valueFromString = null;
				private readonly SciterApiDelegates.ValueInvoke _valueInvoke = null;
				private readonly SciterApiDelegates.ValueNativeFunctorSet _valueNativeFunctorSet = null;
				private readonly SciterApiDelegates.ValueIsNativeFunctor _valueIsNativeFunctor = null;
				
				#endregion
				
				#region pre v4.4.3.24

				private readonly SciterApiDelegates.GetTIScriptApi _getTIScriptApi = null;
				private readonly SciterApiDelegates.SciterGetVM _sciterGetVM = null;
				private readonly SciterApiDelegates.Sciter_v2V _sciter_v2V = null;
				private readonly SciterApiDelegates.Sciter_V2v _sciter_V2v = null;

				#endregion
				
				#region post v4.4.3.24

				private readonly SciterApiDelegates.Reserved1 _reserved1 = null;
				private readonly SciterApiDelegates.Reserved2 _reserved2 = null;
				private readonly SciterApiDelegates.Reserved3 _reserved3 = null;
				private readonly SciterApiDelegates.Reserved4 _reserved4 = null;

				#endregion
			
				#region Archive
				
				private readonly SciterApiDelegates.SciterOpenArchive _sciterOpenArchive = null;
				private readonly SciterApiDelegates.SciterGetArchiveItem _sciterGetArchiveItem = null;
				private readonly SciterApiDelegates.SciterCloseArchive _sciterCloseArchive = null;
				
				#endregion

				private readonly SciterApiDelegates.SciterFireEvent _sciterFireEvent = null;

				private readonly SciterApiDelegates.SciterGetCallbackParam _sciterGetCallbackParam = null;
				private readonly SciterApiDelegates.SciterPostCallback _sciterPostCallback = null;
				private readonly SciterApiDelegates.GetSciterGraphicsApi _getSciterGraphicsAPI = null;
				private readonly SciterApiDelegates.GetSciterRequestApi _getSciterRequestAPI = null;

				#region DirectX
				
				private readonly SciterApiDelegates.SciterCreateOnDirectXWindow _sciterCreateOnDirectXWindow = null;
				private readonly SciterApiDelegates.SciterRenderOnDirectXWindow _sciterRenderOnDirectXWindow = null;
				private readonly SciterApiDelegates.SciterRenderOnDirectXTexture _sciterRenderOnDirectXTexture = null;
				
				#endregion

				private readonly SciterApiDelegates.SciterProcX _sciterProcX = null; 
				
				#region Sciter 4.4.3.14
				
				private readonly SciterApiDelegates.SciterAtomValue _sciterAtomValue = null;
				private readonly SciterApiDelegates.SciterAtomNameCB _sciterAtomNameCB = null;
				
				#endregion
				
				#region Sciter 4.4.3.16
				
				private readonly SciterApiDelegates.SciterSetGlobalAsset _sciterSetGlobalAsset = null; 
				
				#endregion
				
				#region Sciter 4.4.4.6
				
				private readonly SciterApiDelegates.SciterSetVariable4446 _sciterSetVariable4446 = null;
				private readonly SciterApiDelegates.SciterGetVariable4446 _sciterGetVariable4446 = null;
				
				#endregion
				
				#region Sciter 4.4.4.7

				private readonly SciterApiDelegates.SciterGetElementAsset _sciterGetElementAsset = null;
				
				#endregion

				#region Sciter 4.4.5.4

				private readonly SciterApiDelegates.SciterSetVariable _sciterSetVariable = null;
				private readonly SciterApiDelegates.SciterGetVariable _sciterGetVariable = null;
				
				private readonly SciterApiDelegates.SciterElementUnwrap _sciterElementUnwrap = null;
				private readonly SciterApiDelegates.SciterElementWrap _sciterElementWrap = null;

				private readonly SciterApiDelegates.SciterNodeUnwrap _sciterNodeUnwrap = null;
				private readonly SciterApiDelegates.SciterNodeWrap _sciterNodeWrap = null;

				#endregion
				
#pragma warning restore 649

				internal NativeSciterApiWrapper(Type type, IntPtr apiPtr)
				{
					_apiPtr = apiPtr;
					
					var @struct = Marshal.PtrToStructure(apiPtr, type);

					var fieldInfoDictionary = GetType()
						.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
						.Where(w => w.FieldType.GetCustomAttribute<SciterStructMapAttribute>() != null)
						.ToDictionary(key => key.FieldType.GetCustomAttribute<SciterStructMapAttribute>()?.Name,
							value => value);

					var fieldInfos = @struct.GetType().GetFields();
					foreach (var fieldInfo in fieldInfos)
					{
						if (!fieldInfoDictionary.ContainsKey(fieldInfo.Name))
							continue;
						fieldInfoDictionary[fieldInfo.Name].SetValue(this, fieldInfo.GetValue(@struct));
					}
				}

				public string SciterClassName() =>
					Marshal.PtrToStringUni(_sciterClassName());

				// ReSharper disable once ConvertToAutoProperty
				public int Version => _version;

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

				public IntPtr SciterCreateNSView(ref PInvokeUtils.RECT frame)
                {
					if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
						throw new PlatformNotSupportedException($"{nameof(SciterCreateNSView)} is reserved for use on {nameof(OSPlatform.OSX)}");

                    return _sciterCreateNSView(frame: ref frame);
				}

                public IntPtr SciterCreateWidget(ref PInvokeUtils.RECT frame)
                {
                    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        throw new PlatformNotSupportedException($"{nameof(SciterCreateWidget)} is reserved for use on {nameof(OSPlatform.Linux)}");

					return _sciterCreateWidget(frame: ref frame);
                }

                public IntPtr SciterCreateWindow(SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags,
                    ref PInvokeUtils.RECT frame, MulticastDelegate delegt,
                    IntPtr delegateParam, IntPtr parent)
                {
                    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        throw new PlatformNotSupportedException($"{nameof(SciterCreateWidget)} is for use on {nameof(OSPlatform.Windows)}");

					return _sciterCreateWindow(creationFlags: creationFlags, frame: ref frame, delegt: delegt, delegateParam: delegateParam, parent: parent);
				}

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

				public SciterXDom.SCDOM_RESULT SciterGetObject(IntPtr he, out IntPtr pval, bool forceCreation) => 
					_sciterGetObject(he, out pval, forceCreation);

				public SciterXDom.SCDOM_RESULT SciterGetElementNamespace(IntPtr he, out IntPtr pval) => 
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

				public SciterXDom.SCDOM_RESULT SciterNodeInsert(IntPtr hn, uint where, IntPtr what) => 
					_sciterNodeInsert(hn, where, what);

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


                #region pre v4.4.3.24

				public IntPtr GetTIScriptApi() => 
					_getTIScriptApi();

				public IntPtr SciterGetVM(IntPtr hwnd) =>
					_sciterGetVM(hwnd);

				public bool Sciter_v2V(IntPtr vm, SciterScript.ScriptValue scriptValue, ref SciterValue.VALUE value,
					bool isolate) =>
					_sciter_v2V(vm, scriptValue, ref value, isolate);

				public bool Sciter_V2v(IntPtr vm, ref SciterValue.VALUE value,
					ref SciterScript.ScriptValue scriptValue) =>
					_sciter_V2v(vm, ref value, ref scriptValue);

				#endregion

                #region post v4.4.3.24
				public void Reserved1() => 
					_reserved1();

				public void Reserved2() => 
					_reserved2();

				public void Reserved3() =>
					_reserved3();

				public void Reserved4() =>
					_reserved4();

                #endregion

				public IntPtr SciterOpenArchive(IntPtr archiveData, uint archiveDataLength) => 
					_sciterOpenArchive(archiveData, archiveDataLength);

				public bool SciterGetArchiveItem(IntPtr harc, string path, out IntPtr pdata, out uint pdataLength) => 
					_sciterGetArchiveItem(harc, path, out pdata, out pdataLength);

				public bool SciterCloseArchive(IntPtr harc) => 
					_sciterCloseArchive(harc);

				public SciterXDom.SCDOM_RESULT SciterFireEvent(SciterBehaviorArgs evt, bool post, out bool handled)
				{
					var @event = evt.FromEventArgs();
					return _sciterFireEvent(ref @event, post, out handled);
				}
				
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

				public ulong SciterAtomValue(string name) => 
					_sciterAtomValue(name);

				public bool SciterAtomNameCB(ulong atomv, IntPtr rxc, IntPtr rcvParam) => 
					_sciterAtomNameCB(atomv, rxc, rcvParam);

				public bool SciterSetGlobalAsset(IntPtr pass) => 
					_sciterSetGlobalAsset(pass);

				public SciterXDom.SCDOM_RESULT SciterGetElementAsset(IntPtr el, ulong nameAtom, out IntPtr ppass) =>
					_sciterGetElementAsset(el, nameAtom, out ppass);

				//public bool SciterSetVariable(IntPtr hwndOrNull, string path, ref SciterValue.VALUE pvalToSet) =>
				//	_sciterSetVariable4446(hwndOrNull, path, ref pvalToSet);

				public uint SciterSetVariable(IntPtr hwndOrNull, string path, ref SciterValue.VALUE pvalToSet)
				{
					return _sciterSetVariable?.Invoke(hwndOrNull, path, ref pvalToSet) ??
					       (_sciterSetVariable4446 != null
						       ? System.Convert.ToUInt32(_sciterSetVariable4446(hwndOrNull, path, ref pvalToSet))
						       : 0u);
				}

				//public bool SciterGetVariable(IntPtr hwndOrNull, string path, ref SciterValue.VALUE pvalToGet) =>
				//	_sciterGetVariable4446(hwndOrNull, path, ref pvalToGet);

				public uint SciterGetVariable(IntPtr hwndOrNull, string path, out SciterValue.VALUE pvalToGet)
				{
					pvalToGet = new SciterValue.VALUE();
					
					return _sciterGetVariable?.Invoke(hwndOrNull, path, out pvalToGet) ??
					       (_sciterGetVariable4446 != null
						       ? System.Convert.ToUInt32(_sciterGetVariable4446(hwndOrNull, path, out pvalToGet))
						       : 0u);
				}

				public uint SciterElementUnwrap(ref SciterValue.VALUE pval, out IntPtr ppElement) =>
					_sciterElementUnwrap(ref pval, out ppElement);

				public uint SciterElementWrap(ref SciterValue.VALUE pval, IntPtr ppElement) =>
					_sciterElementWrap(ref pval, ppElement);

				public uint SciterNodeUnwrap(ref SciterValue.VALUE pval, out IntPtr ppNode) =>
					_sciterNodeUnwrap(ref pval, out ppNode);

				public uint SciterNodeWrap(ref SciterValue.VALUE pval, IntPtr ppNode) =>
					_sciterNodeWrap(ref pval, ppNode);

				#endregion
			}
		}
	}
}