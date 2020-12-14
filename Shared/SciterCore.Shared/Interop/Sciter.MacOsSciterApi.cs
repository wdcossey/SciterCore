using System;
using System.Runtime.InteropServices;

// ReSharper disable MemberCanBePrivate.Global

namespace SciterCore.Interop
{
    public static partial class Sciter
    {
        [StructLayout(LayoutKind.Sequential)]
        internal readonly struct MacOsSciterApi
        {
#pragma warning disable 649
            public readonly int version;
            public readonly SciterApiDelegates.SciterClassName SciterClassName;
            public readonly SciterApiDelegates.SciterVersion SciterVersion;
            public readonly SciterApiDelegates.SciterDataReady SciterDataReady;
            public readonly SciterApiDelegates.SciterDataReadyAsync SciterDataReadyAsync;
            public readonly SciterApiDelegates.SciterLoadFile SciterLoadFile;
            public readonly SciterApiDelegates.SciterLoadHtml SciterLoadHtml;
            public readonly SciterApiDelegates.SciterSetCallback SciterSetCallback;
            public readonly SciterApiDelegates.SciterSetMasterCss SciterSetMasterCSS;
            public readonly SciterApiDelegates.SciterAppendMasterCss SciterAppendMasterCSS;
            public readonly SciterApiDelegates.SciterSetCss SciterSetCSS;
            public readonly SciterApiDelegates.SciterSetMediaType SciterSetMediaType;
            public readonly SciterApiDelegates.SciterSetMediaVars SciterSetMediaVars;
            public readonly SciterApiDelegates.SciterGetMinWidth SciterGetMinWidth;
            public readonly SciterApiDelegates.SciterGetMinHeight SciterGetMinHeight;
            public readonly SciterApiDelegates.SciterCall SciterCall;
            public readonly SciterApiDelegates.SciterEval SciterEval;
            public readonly SciterApiDelegates.SciterUpdateWindow SciterUpdateWindow;
            public readonly SciterApiDelegates.SciterSetOption SciterSetOption;
            public readonly SciterApiDelegates.SciterGetPpi SciterGetPPI;
            public readonly SciterApiDelegates.SciterGetViewExpando SciterGetViewExpando;
            public readonly SciterApiDelegates.SciterGraphicsCaps SciterGraphicsCaps;
            public readonly SciterApiDelegates.SciterSetHomeUrl SciterSetHomeURL;
            public readonly SciterApiDelegates.SciterCreateNsView SciterCreateNSView;
            public readonly SciterApiDelegates.SciterCreateWindow SciterCreateWindow;
            public readonly SciterApiDelegates.SciterSetupDebugOutput SciterSetupDebugOutput;

            // DOM Element API 
            public readonly SciterApiDelegates.SciterUseElement Sciter_UseElement;
            public readonly SciterApiDelegates.SciterUnuseElement Sciter_UnuseElement;
            public readonly SciterApiDelegates.SciterGetRootElement SciterGetRootElement;
            public readonly SciterApiDelegates.SciterGetFocusElement SciterGetFocusElement;
            public readonly SciterApiDelegates.SciterFindElement SciterFindElement;
            public readonly SciterApiDelegates.SciterGetChildrenCount SciterGetChildrenCount;
            public readonly SciterApiDelegates.SciterGetNthChild SciterGetNthChild;
            public readonly SciterApiDelegates.SciterGetParentElement SciterGetParentElement;
            public readonly SciterApiDelegates.SciterGetElementHtmlCb SciterGetElementHtmlCB;
            public readonly SciterApiDelegates.SciterGetElementTextCb SciterGetElementTextCB;
            public readonly SciterApiDelegates.SciterSetElementText SciterSetElementText;
            public readonly SciterApiDelegates.SciterGetAttributeCount SciterGetAttributeCount;
            public readonly SciterApiDelegates.SciterGetNthAttributeNameCb SciterGetNthAttributeNameCB;
            public readonly SciterApiDelegates.SciterGetNthAttributeValueCb SciterGetNthAttributeValueCB;
            public readonly SciterApiDelegates.SciterGetAttributeByNameCb SciterGetAttributeByNameCB;
            public readonly SciterApiDelegates.SciterSetAttributeByName SciterSetAttributeByName;
            public readonly SciterApiDelegates.SciterClearAttributes SciterClearAttributes;
            public readonly SciterApiDelegates.SciterGetElementIndex SciterGetElementIndex;
            public readonly SciterApiDelegates.SciterGetElementType SciterGetElementType;
            public readonly SciterApiDelegates.SciterGetElementTypeCb SciterGetElementTypeCB;
            public readonly SciterApiDelegates.SciterGetStyleAttributeCb SciterGetStyleAttributeCB;
            public readonly SciterApiDelegates.SciterSetStyleAttribute SciterSetStyleAttribute;
            public readonly SciterApiDelegates.SciterGetElementLocation SciterGetElementLocation;
            public readonly SciterApiDelegates.SciterScrollToView SciterScrollToView;
            public readonly SciterApiDelegates.SciterUpdateElement SciterUpdateElement;
            public readonly SciterApiDelegates.SciterRefreshElementArea SciterRefreshElementArea;
            public readonly SciterApiDelegates.SciterSetCapture SciterSetCapture;
            public readonly SciterApiDelegates.SciterReleaseCapture SciterReleaseCapture;
            public readonly SciterApiDelegates.SciterGetElementHwnd SciterGetElementHwnd;
            public readonly SciterApiDelegates.SciterCombineUrl SciterCombineURL;
            public readonly SciterApiDelegates.SciterSelectElements SciterSelectElements;
            public readonly SciterApiDelegates.SciterSelectElementsW SciterSelectElementsW;
            public readonly SciterApiDelegates.SciterSelectParent SciterSelectParent;
            public readonly SciterApiDelegates.SciterSelectParentW SciterSelectParentW;
            public readonly SciterApiDelegates.SciterSetElementHtml SciterSetElementHtml;
            public readonly SciterApiDelegates.SciterGetElementUid SciterGetElementUID;
            public readonly SciterApiDelegates.SciterGetElementByUid SciterGetElementByUID;
            public readonly SciterApiDelegates.SciterShowPopup SciterShowPopup;
            public readonly SciterApiDelegates.SciterShowPopupAt SciterShowPopupAt;
            public readonly SciterApiDelegates.SciterHidePopup SciterHidePopup;
            public readonly SciterApiDelegates.SciterGetElementState SciterGetElementState;
            public readonly SciterApiDelegates.SciterSetElementState SciterSetElementState;
            public readonly SciterApiDelegates.SciterCreateElement SciterCreateElement;
            public readonly SciterApiDelegates.SciterCloneElement SciterCloneElement;
            public readonly SciterApiDelegates.SciterInsertElement SciterInsertElement;
            public readonly SciterApiDelegates.SciterDetachElement SciterDetachElement;
            public readonly SciterApiDelegates.SciterDeleteElement SciterDeleteElement;
            public readonly SciterApiDelegates.SciterSetTimer SciterSetTimer;
            public readonly SciterApiDelegates.SciterDetachEventHandler SciterDetachEventHandler;
            public readonly SciterApiDelegates.SciterAttachEventHandler SciterAttachEventHandler;
            public readonly SciterApiDelegates.SciterWindowAttachEventHandler SciterWindowAttachEventHandler;
            public readonly SciterApiDelegates.SciterWindowDetachEventHandler SciterWindowDetachEventHandler;
            public readonly SciterApiDelegates.SciterSendEvent SciterSendEvent;
            public readonly SciterApiDelegates.SciterPostEvent SciterPostEvent;
            public readonly SciterApiDelegates.SciterCallBehaviorMethod SciterCallBehaviorMethod;
            public readonly SciterApiDelegates.SciterRequestElementData SciterRequestElementData;
            public readonly SciterApiDelegates.SciterHttpRequest SciterHttpRequest;
            public readonly SciterApiDelegates.SciterGetScrollInfo SciterGetScrollInfo;
            public readonly SciterApiDelegates.SciterSetScrollPos SciterSetScrollPos;
            public readonly SciterApiDelegates.SciterGetElementIntrinsicWidths SciterGetElementIntrinsicWidths;
            public readonly SciterApiDelegates.SciterGetElementIntrinsicHeight SciterGetElementIntrinsicHeight;
            public readonly SciterApiDelegates.SciterIsElementVisible SciterIsElementVisible;
            public readonly SciterApiDelegates.SciterIsElementEnabled SciterIsElementEnabled;
            public readonly SciterApiDelegates.SciterSortElements SciterSortElements;
            public readonly SciterApiDelegates.SciterSwapElements SciterSwapElements;
            public readonly SciterApiDelegates.SciterTraverseUiEvent SciterTraverseUIEvent;
            public readonly SciterApiDelegates.SciterCallScriptingMethod SciterCallScriptingMethod;
            public readonly SciterApiDelegates.SciterCallScriptingFunction SciterCallScriptingFunction;
            public readonly SciterApiDelegates.SciterEvalElementScript SciterEvalElementScript;
            public readonly SciterApiDelegates.SciterAttachHwndToElement SciterAttachHwndToElement;
            public readonly SciterApiDelegates.SciterControlGetType SciterControlGetType;
            public readonly SciterApiDelegates.SciterGetValue SciterGetValue;
            public readonly SciterApiDelegates.SciterSetValue SciterSetValue;
            public readonly SciterApiDelegates.SciterGetExpando SciterGetExpando;
            public readonly SciterApiDelegates.SciterGetObject SciterGetObject;
            public readonly SciterApiDelegates.SciterGetElementNamespace SciterGetElementNamespace;
            public readonly SciterApiDelegates.SciterGetHighlightedElement SciterGetHighlightedElement;
            public readonly SciterApiDelegates.SciterSetHighlightedElement SciterSetHighlightedElement;

            // DOM Node API 
            public readonly SciterApiDelegates.SciterNodeAddRef SciterNodeAddRef;
            public readonly SciterApiDelegates.SciterNodeRelease SciterNodeRelease;
            public readonly SciterApiDelegates.SciterNodeCastFromElement SciterNodeCastFromElement;
            public readonly SciterApiDelegates.SciterNodeCastToElement SciterNodeCastToElement;
            public readonly SciterApiDelegates.SciterNodeFirstChild SciterNodeFirstChild;
            public readonly SciterApiDelegates.SciterNodeLastChild SciterNodeLastChild;
            public readonly SciterApiDelegates.SciterNodeNextSibling SciterNodeNextSibling;
            public readonly SciterApiDelegates.SciterNodePrevSibling SciterNodePrevSibling;
            public readonly SciterApiDelegates.SciterNodeParent SciterNodeParent;
            public readonly SciterApiDelegates.SciterNodeNthChild SciterNodeNthChild;
            public readonly SciterApiDelegates.SciterNodeChildrenCount SciterNodeChildrenCount;
            public readonly SciterApiDelegates.SciterNodeType SciterNodeType;
            public readonly SciterApiDelegates.SciterNodeGetText SciterNodeGetText;
            public readonly SciterApiDelegates.SciterNodeSetText SciterNodeSetText;
            public readonly SciterApiDelegates.SciterNodeInsert SciterNodeInsert;
            public readonly SciterApiDelegates.SciterNodeRemove SciterNodeRemove;
            public readonly SciterApiDelegates.SciterCreateTextNode SciterCreateTextNode;
            public readonly SciterApiDelegates.SciterCreateCommentNode SciterCreateCommentNode;

            // Value API 
            public readonly SciterApiDelegates.ValueInit ValueInit;
            public readonly SciterApiDelegates.ValueClear ValueClear;
            public readonly SciterApiDelegates.ValueCompare ValueCompare;
            public readonly SciterApiDelegates.ValueCopy ValueCopy;
            public readonly SciterApiDelegates.ValueIsolate ValueIsolate;
            public readonly SciterApiDelegates.ValueType ValueType;
            public readonly SciterApiDelegates.ValueStringData ValueStringData;
            public readonly SciterApiDelegates.ValueStringDataSet ValueStringDataSet;
            public readonly SciterApiDelegates.ValueIntData ValueIntData;
            public readonly SciterApiDelegates.ValueIntDataSet ValueIntDataSet;
            public readonly SciterApiDelegates.ValueInt64Data ValueInt64Data;
            public readonly SciterApiDelegates.ValueInt64DataSet ValueInt64DataSet;
            public readonly SciterApiDelegates.ValueFloatData ValueFloatData;
            public readonly SciterApiDelegates.ValueFloatDataSet ValueFloatDataSet;
            public readonly SciterApiDelegates.ValueBinaryData ValueBinaryData;
            public readonly SciterApiDelegates.ValueBinaryDataSet ValueBinaryDataSet;
            public readonly SciterApiDelegates.ValueElementsCount ValueElementsCount;
            public readonly SciterApiDelegates.ValueNthElementValue ValueNthElementValue;
            public readonly SciterApiDelegates.ValueNthElementValueSet ValueNthElementValueSet;
            public readonly SciterApiDelegates.ValueNthElementKey ValueNthElementKey;
            public readonly SciterApiDelegates.ValueEnumElements ValueEnumElements;
            public readonly SciterApiDelegates.ValueSetValueToKey ValueSetValueToKey;
            public readonly SciterApiDelegates.ValueGetValueOfKey ValueGetValueOfKey;
            public readonly SciterApiDelegates.ValueToString ValueToString;
            public readonly SciterApiDelegates.ValueFromString ValueFromString;
            public readonly SciterApiDelegates.ValueInvoke ValueInvoke;
            public readonly SciterApiDelegates.ValueNativeFunctorSet ValueNativeFunctorSet;
            public readonly SciterApiDelegates.ValueIsNativeFunctor ValueIsNativeFunctor;

			#region Used to be TIScript VM API (Deprecated in v4.4.3.24)
			
			[Obsolete("Reserved in v4.4.3.24", false)]
			public readonly SciterApiDelegates.VoidReserved reserved1;

			[Obsolete("Reserved in v4.4.3.24", false)]
			public readonly SciterApiDelegates.VoidReserved reserved2;

			[Obsolete("Reserved in v4.4.3.24", false)]
			public readonly SciterApiDelegates.VoidReserved reserved3;
			
			[Obsolete("Reserved in v4.4.3.24", false)]
			public readonly SciterApiDelegates.VoidReserved reserved4;

			#endregion

            public readonly SciterApiDelegates.SciterOpenArchive SciterOpenArchive;
            public readonly SciterApiDelegates.SciterGetArchiveItem SciterGetArchiveItem;
            public readonly SciterApiDelegates.SciterCloseArchive SciterCloseArchive;

            public readonly SciterApiDelegates.SciterFireEvent SciterFireEvent;

            public readonly SciterApiDelegates.SciterGetCallbackParam SciterGetCallbackParam;
            public readonly SciterApiDelegates.SciterPostCallback SciterPostCallback;
            public readonly SciterApiDelegates.GetSciterGraphicsApi GetSciterGraphicsAPI;
            public readonly SciterApiDelegates.GetSciterRequestApi GetSciterRequestAPI;

            public readonly SciterApiDelegates.SciterProcX SciterProcX;
            
            // Sciter 4.4.3.24
			
            public readonly SciterApiDelegates.SciterAtomValue SciterAtomValue;
            public readonly SciterApiDelegates.SciterAtomNameCB SciterAtomNameCB;
            public readonly SciterApiDelegates.SciterSetGlobalAsset SciterSetGlobalAsset;
            
            // Sciter 4.4.4.7
            public readonly SciterApiDelegates.SciterGetElementAsset SciterGetElementAsset;
            public readonly SciterApiDelegates.SciterSetVariable SciterSetVariable;
            public readonly SciterApiDelegates.SciterGetVariable SciterGetVariable;
            
            // Sciter 4.4.5.4
            //public readonly SciterApiDelegates.SciterElementUnwrap	SciterElementUnwrap;
            //public readonly SciterApiDelegates.SciterElementWrap SciterElementWrap;
            //public readonly SciterApiDelegates.SciterNodeUnwrap SciterNodeUnwrap;
            //public readonly SciterApiDelegates.SciterNodeWrap SciterNodeWrap;

#pragma warning restore 649
        }
    }
}