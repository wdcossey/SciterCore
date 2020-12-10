using System;
using System.Runtime.InteropServices;

// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore.Interop
{
	public static partial class Sciter
	{
		[StructLayout(LayoutKind.Sequential)]
		internal struct WindowsSciterApi
		{
#pragma warning disable 649
			public readonly int version;
			public readonly SciterApi.SCITER_CLASS_NAME SciterClassName;
			public readonly SciterApi.SCITER_VERSION SciterVersion;
			public readonly SciterApi.SCITER_DATA_READY SciterDataReady;
			public readonly SciterApi.SCITER_DATA_READY_ASYNC SciterDataReadyAsync;
			public readonly SciterApi.SCITER_PROC SciterProc;
			public readonly SciterApi.SCITER_PROC_ND SciterProcND;
			public readonly SciterApi.SCITER_LOAD_FILE SciterLoadFile;
			public readonly SciterApi.SCITER_LOAD_HTML SciterLoadHtml;
			public readonly SciterApi.SCITER_SET_CALLBACK SciterSetCallback;
			public readonly SciterApi.SCITER_SET_MASTER_CSS SciterSetMasterCSS;
			public readonly SciterApi.SCITER_APPEND_MASTER_CSS SciterAppendMasterCSS;
			public readonly SciterApi.SCITER_SET_CSS SciterSetCSS;
			public readonly SciterApi.SCITER_SET_MEDIA_TYPE SciterSetMediaType;
			public readonly SciterApi.SCITER_SET_MEDIA_VARS SciterSetMediaVars;
			public readonly SciterApi.SCITER_GET_MIN_WIDTH SciterGetMinWidth;
			public readonly SciterApi.SCITER_GET_MIN_HEIGHT SciterGetMinHeight;
			public readonly SciterApi.SCITER_CALL SciterCall;
			public readonly SciterApi.SCITER_EVAL SciterEval;
			public readonly SciterApi.SCITER_UPDATE_WINDOW SciterUpdateWindow;
			public readonly SciterApi.SCITER_TRANSLATE_MESSAGE SciterTranslateMessage;
			public readonly SciterApi.SCITER_SET_OPTION SciterSetOption;
			public readonly SciterApi.SCITER_GET_PPI SciterGetPPI;
			public readonly SciterApi.SCITER_GET_VIEW_EXPANDO SciterGetViewExpando;
			public readonly SciterApi.SCITER_RENDER_D2D SciterRenderD2D;
			public readonly SciterApi.SCITER_D2D_FACTORY SciterD2DFactory;
			public readonly SciterApi.SCITER_DW_FACTORY SciterDWFactory;
			public readonly SciterApi.SCITER_GRAPHICS_CAPS SciterGraphicsCaps;
			public readonly SciterApi.SCITER_SET_HOME_URL SciterSetHomeURL;
			public readonly SciterApi.SCITER_CREATE_WINDOW SciterCreateWindow;
			public readonly SciterApi.SCITER_SETUP_DEBUG_OUTPUT SciterSetupDebugOutput;

			#region DOM Element API

			public readonly SciterApi.SCITER_USE_ELEMENT Sciter_UseElement;
			public readonly SciterApi.SCITER_UNUSE_ELEMENT Sciter_UnuseElement;
			public readonly SciterApi.SCITER_GET_ROOT_ELEMENT SciterGetRootElement;
			public readonly SciterApi.SCITER_GET_FOCUS_ELEMENT SciterGetFocusElement;
			public readonly SciterApi.SCITER_FIND_ELEMENT SciterFindElement;
			public readonly SciterApi.SCITER_GET_CHILDREN_COUNT SciterGetChildrenCount;
			public readonly SciterApi.SCITER_GET_NTH_CHILD SciterGetNthChild;
			public readonly SciterApi.SCITER_GET_PARENT_ELEMENT SciterGetParentElement;
			public readonly SciterApi.SCITER_GET_ELEMENT_HTML_CB SciterGetElementHtmlCB;
			public readonly SciterApi.SCITER_GET_ELEMENT_TEXT_CB SciterGetElementTextCB;
			public readonly SciterApi.SCITER_SET_ELEMENT_TEXT SciterSetElementText;
			public readonly SciterApi.SCITER_GET_ATTRIBUTE_COUNT SciterGetAttributeCount;
			public readonly SciterApi.SCITER_GET_NTH_ATTRIBUTE_NAME_CB SciterGetNthAttributeNameCB;
			public readonly SciterApi.SCITER_GET_NTH_ATTRIBUTE_VALUE_CB SciterGetNthAttributeValueCB;
			public readonly SciterApi.SCITER_GET_ATTRIBUTE_BY_NAME_CB SciterGetAttributeByNameCB;
			public readonly SciterApi.SCITER_SET_ATTRIBUTE_BY_NAME SciterSetAttributeByName;
			public readonly SciterApi.SCITER_CLEAR_ATTRIBUTES SciterClearAttributes;
			public readonly SciterApi.SCITER_GET_ELEMENT_INDEX SciterGetElementIndex;
			public readonly SciterApi.SCITER_GET_ELEMENT_TYPE SciterGetElementType;
			public readonly SciterApi.SCITER_GET_ELEMENT_TYPE_CB SciterGetElementTypeCB;
			public readonly SciterApi.SCITER_GET_STYLE_ATTRIBUTE_CB SciterGetStyleAttributeCB;
			public readonly SciterApi.SCITER_SET_STYLE_ATTRIBUTE SciterSetStyleAttribute;
			public readonly SciterApi.SCITER_GET_ELEMENT_LOCATION SciterGetElementLocation;
			public readonly SciterApi.SCITER_SCROLL_TO_VIEW SciterScrollToView;
			public readonly SciterApi.SCITER_UPDATE_ELEMENT SciterUpdateElement;
			public readonly SciterApi.SCITER_REFRESH_ELEMENT_AREA SciterRefreshElementArea;
			public readonly SciterApi.SCITER_SET_CAPTURE SciterSetCapture;
			public readonly SciterApi.SCITER_RELEASE_CAPTURE SciterReleaseCapture;
			public readonly SciterApi.SCITER_GET_ELEMENT_HWND SciterGetElementHwnd;
			public readonly SciterApi.SCITER_COMBINE_URL SciterCombineURL;
			public readonly SciterApi.SCITER_SELECT_ELEMENTS SciterSelectElements;
			public readonly SciterApi.SCITER_SELECT_ELEMENTS_W SciterSelectElementsW;
			public readonly SciterApi.SCITER_SELECT_PARENT SciterSelectParent;
			public readonly SciterApi.SCITER_SELECT_PARENT_W SciterSelectParentW;
			public readonly SciterApi.SCITER_SET_ELEMENT_HTML SciterSetElementHtml;
			public readonly SciterApi.SCITER_GET_ELEMENT_UID SciterGetElementUID;
			public readonly SciterApi.SCITER_GET_ELEMENT_BY_UID SciterGetElementByUID;
			public readonly SciterApi.SCITER_SHOW_POPUP SciterShowPopup;
			public readonly SciterApi.SCITER_SHOW_POPUP_AT SciterShowPopupAt;
			public readonly SciterApi.SCITER_HIDE_POPUP SciterHidePopup;
			public readonly SciterApi.SCITER_GET_ELEMENT_STATE SciterGetElementState;
			public readonly SciterApi.SCITER_SET_ELEMENT_STATE SciterSetElementState;
			public readonly SciterApi.SCITER_CREATE_ELEMENT SciterCreateElement;
			public readonly SciterApi.SCITER_CLONE_ELEMENT SciterCloneElement;
			public readonly SciterApi.SCITER_INSERT_ELEMENT SciterInsertElement;
			public readonly SciterApi.SCITER_DETACH_ELEMENT SciterDetachElement;
			public readonly SciterApi.SCITER_DELETE_ELEMENT SciterDeleteElement;
			public readonly SciterApi.SCITER_SET_TIMER SciterSetTimer;
			public readonly SciterApi.SCITER_DETACH_EVENT_HANDLER SciterDetachEventHandler;
			public readonly SciterApi.SCITER_ATTACH_EVENT_HANDLER SciterAttachEventHandler;
			public readonly SciterApi.SCITER_WINDOW_ATTACH_EVENT_HANDLER SciterWindowAttachEventHandler;
			public readonly SciterApi.SCITER_WINDOW_DETACH_EVENT_HANDLER SciterWindowDetachEventHandler;
			public readonly SciterApi.SCITER_SEND_EVENT SciterSendEvent;
			public readonly SciterApi.SCITER_POST_EVENT SciterPostEvent;
			public readonly SciterApi.SCITER_CALL_BEHAVIOR_METHOD SciterCallBehaviorMethod;
			public readonly SciterApi.SCITER_REQUEST_ELEMENT_DATA SciterRequestElementData;
			public readonly SciterApi.SCITER_HTTP_REQUEST SciterHttpRequest;
			public readonly SciterApi.SCITER_GET_SCROLL_INFO SciterGetScrollInfo;
			public readonly SciterApi.SCITER_SET_SCROLL_POS SciterSetScrollPos;
			public readonly SciterApi.SCITER_GET_ELEMENT_INTRINSIC_WIDTHS SciterGetElementIntrinsicWidths;
			public readonly SciterApi.SCITER_GET_ELEMENT_INTRINSIC_HEIGHT SciterGetElementIntrinsicHeight;
			public readonly SciterApi.SCITER_IS_ELEMENT_VISIBLE SciterIsElementVisible;
			public readonly SciterApi.SCITER_IS_ELEMENT_ENABLED SciterIsElementEnabled;
			public readonly SciterApi.SCITER_SORT_ELEMENTS SciterSortElements;
			public readonly SciterApi.SCITER_SWAP_ELEMENTS SciterSwapElements;
			public readonly SciterApi.SCITER_TRAVERSE_UI_EVENT SciterTraverseUIEvent;
			public readonly SciterApi.SCITER_CALL_SCRIPTING_METHOD SciterCallScriptingMethod;
			public readonly SciterApi.SCITER_CALL_SCRIPTING_FUNCTION SciterCallScriptingFunction;
			public readonly SciterApi.SCITER_EVAL_ELEMENT_SCRIPT SciterEvalElementScript;
			public readonly SciterApi.SCITER_ATTACH_HWND_TO_ELEMENT SciterAttachHwndToElement;
			public readonly SciterApi.SCITER_CONTROL_GET_TYPE SciterControlGetType;
			public readonly SciterApi.SCITER_GET_VALUE SciterGetValue;
			public readonly SciterApi.SCITER_SET_VALUE SciterSetValue;
			public readonly SciterApi.SCITER_GET_EXPANDO SciterGetExpando;
			public readonly SciterApi.SCITER_GET_OBJECT SciterGetObject;
			public readonly SciterApi.SCITER_GET_ELEMENT_NAMESPACE SciterGetElementNamespace;
			public readonly SciterApi.SCITER_GET_HIGHLIGHTED_ELEMENT SciterGetHighlightedElement;
			public readonly SciterApi.SCITER_SET_HIGHLIGHTED_ELEMENT SciterSetHighlightedElement;

			#endregion

			#region DOM Node API

			public readonly SciterApi.SCITER_NODE_ADD_REF SciterNodeAddRef;
			public readonly SciterApi.SCITER_NODE_RELEASE SciterNodeRelease;
			public readonly SciterApi.SCITER_NODE_CAST_FROM_ELEMENT SciterNodeCastFromElement;
			public readonly SciterApi.SCITER_NODE_CAST_TO_ELEMENT SciterNodeCastToElement;
			public readonly SciterApi.SCITER_NODE_FIRST_CHILD SciterNodeFirstChild;
			public readonly SciterApi.SCITER_NODE_LAST_CHILD SciterNodeLastChild;
			public readonly SciterApi.SCITER_NODE_NEXT_SIBLING SciterNodeNextSibling;
			public readonly SciterApi.SCITER_NODE_PREV_SIBLING SciterNodePrevSibling;
			public readonly SciterApi.SCITER_NODE_PARENT SciterNodeParent;
			public readonly SciterApi.SCITER_NODE_NTH_CHILD SciterNodeNthChild;
			public readonly SciterApi.SCITER_NODE_CHILDREN_COUNT SciterNodeChildrenCount;
			public readonly SciterApi.SCITER_NODE_TYPE SciterNodeType;
			public readonly SciterApi.SCITER_NODE_GET_TEXT SciterNodeGetText;
			public readonly SciterApi.SCITER_NODE_SET_TEXT SciterNodeSetText;
			public readonly SciterApi.SCITER_NODE_INSERT SciterNodeInsert;
			public readonly SciterApi.SCITER_NODE_REMOVE SciterNodeRemove;
			public readonly SciterApi.SCITER_CREATE_TEXT_NODE SciterCreateTextNode;
			public readonly SciterApi.SCITER_CREATE_COMMENT_NODE SciterCreateCommentNode;

			#endregion

			#region Value API

			public readonly SciterApi.VALUE_INIT ValueInit;
			public readonly SciterApi.VALUE_CLEAR ValueClear;
			public readonly SciterApi.VALUE_COMPARE ValueCompare;
			public readonly SciterApi.VALUE_COPY ValueCopy;
			public readonly SciterApi.VALUE_ISOLATE ValueIsolate;
			public readonly SciterApi.VALUE_TYPE ValueType;
			public readonly SciterApi.VALUE_STRING_DATA ValueStringData;
			public readonly SciterApi.VALUE_STRING_DATA_SET ValueStringDataSet;
			public readonly SciterApi.VALUE_INT_DATA ValueIntData;
			public readonly SciterApi.VALUE_INT_DATA_SET ValueIntDataSet;
			public readonly SciterApi.VALUE_INT_64DATA ValueInt64Data;
			public readonly SciterApi.VALUE_INT_64DATA_SET ValueInt64DataSet;
			public readonly SciterApi.VALUE_FLOAT_DATA ValueFloatData;
			public readonly SciterApi.VALUE_FLOAT_DATA_SET ValueFloatDataSet;
			public readonly SciterApi.VALUE_BINARY_DATA ValueBinaryData;
			public readonly SciterApi.VALUE_BINARY_DATA_SET ValueBinaryDataSet;
			public readonly SciterApi.VALUE_ELEMENTS_COUNT ValueElementsCount;
			public readonly SciterApi.VALUE_NTH_ELEMENT_VALUE ValueNthElementValue;
			public readonly SciterApi.VALUE_NTH_ELEMENT_VALUE_SET ValueNthElementValueSet;
			public readonly SciterApi.VALUE_NTH_ELEMENT_KEY ValueNthElementKey;
			public readonly SciterApi.VALUE_ENUM_ELEMENTS ValueEnumElements;
			public readonly SciterApi.VALUE_SET_VALUE_TO_KEY ValueSetValueToKey;
			public readonly SciterApi.VALUE_GET_VALUE_OF_KEY ValueGetValueOfKey;
			public readonly SciterApi.VALUE_TO_STRING ValueToString;
			public readonly SciterApi.VALUE_FROM_STRING ValueFromString;
			public readonly SciterApi.VALUE_INVOKE ValueInvoke;
			public readonly SciterApi.VALUE_NATIVE_FUNCTOR_SET ValueNativeFunctorSet;
			public readonly SciterApi.VALUE_IS_NATIVE_FUNCTOR ValueIsNativeFunctor;

			#endregion

			#region TIScript VM API

			public readonly SciterApi.TI_SCRIPT_API TIScriptAPI;

			[Obsolete("Deprecated in v4.4.3.24", true)]
			public readonly SciterApi.SCITER_GET_VM SciterGetVM;

			public readonly SciterApi.SCITER_v2V Sciter_v2V;
			public readonly SciterApi.SCITER_V2v Sciter_V2v;

			#endregion

			#region Archive

			public readonly SciterApi.SCITER_OPEN_ARCHIVE SciterOpenArchive;
			public readonly SciterApi.SCITER_GET_ARCHIVE_ITEM SciterGetArchiveItem;
			public readonly SciterApi.SCITER_CLOSE_ARCHIVE SciterCloseArchive;

			#endregion

			public readonly SciterApi.SCITER_FIRE_EVENT SciterFireEvent;

			public readonly SciterApi.SCITER_GET_CALLBACK_PARAM SciterGetCallbackParam;
			public readonly SciterApi.SCITER_POST_CALLBACK SciterPostCallback;
			public readonly SciterApi.GET_SCITER_GRAPHICS_API GetSciterGraphicsAPI;
			public readonly SciterApi.GET_SCITER_REQUEST_API GetSciterRequestAPI;

			#region DirectX

			public readonly SciterApi.SCITER_CREATE_ON_DIRECT_X_WINDOW SciterCreateOnDirectXWindow;
			public readonly SciterApi.SCITER_RENDER_ON_DIRECT_X_WINDOW SciterRenderOnDirectXWindow;
			public readonly SciterApi.SCITER_RENDER_ON_DIRECT_X_TEXTURE SciterRenderOnDirectXTexture;

			#endregion

			public readonly SciterApi.SCITER_PROC_X SciterProcX;

#pragma warning restore 649
		}
	}
}