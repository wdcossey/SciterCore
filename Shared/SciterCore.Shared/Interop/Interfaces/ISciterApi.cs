using System;
using System.Runtime.InteropServices;
// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace SciterCore.Interop
{
	public interface ISciterApi
	{
		string SciterClassName();

		int Version { get; }

		uint SciterVersion(bool major);

		Version SciterVersion();

		bool SciterDataReady(IntPtr hwnd, string uri, byte[] data, uint dataLength);

		bool SciterDataReadyAsync(IntPtr hwnd, string uri, byte[] data, uint dataLength, IntPtr requestId);

		IntPtr SciterProc(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

		IntPtr SciterProcND(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, ref bool pbHandled);

		bool SciterLoadFile(IntPtr hwnd, string filename);

		bool SciterLoadHtml(IntPtr hwnd, byte[] html, uint htmlSize, string baseUrl);

		void SciterSetCallback(IntPtr hwnd, MulticastDelegate cb, IntPtr param);
		bool SciterSetMasterCSS(byte[] utf8, uint numBytes);
		bool SciterAppendMasterCSS(byte[] utf8, uint numBytes);
		bool SciterSetCSS(IntPtr hwnd, byte[] utf8, uint numBytes, string baseUrl, string mediaType);
		bool SciterSetMediaType(IntPtr hwnd, string mediaType);
		bool SciterSetMediaVars(IntPtr hwnd, ref SciterValue.VALUE mediaVars);
		uint SciterGetMinWidth(IntPtr hwnd);
		uint SciterGetMinHeight(IntPtr hwnd, uint width);

		bool SciterCall(IntPtr hwnd, string functionName, uint argc, SciterValue.VALUE[] argv,
			out SciterValue.VALUE retval);

		bool SciterEval(IntPtr hwnd, string script, uint scriptLength, out SciterValue.VALUE pretval);
		bool SciterUpdateWindow(IntPtr hwnd);

		bool SciterTranslateMessage(IntPtr lpMsg);
		bool SciterSetOption(IntPtr hwnd, SciterXDef.SCITER_RT_OPTIONS option, IntPtr value);
		void SciterGetPPI(IntPtr hwnd, ref uint px, ref uint py);
		bool SciterGetViewExpando(IntPtr hwnd, out SciterValue.VALUE pval);
		bool SciterRenderD2D(IntPtr hwnd, IntPtr prt);
		bool SciterD2DFactory(IntPtr ppf);
		bool SciterDWFactory(IntPtr ppf);
		bool SciterGraphicsCaps(ref uint pcaps);
		bool SciterSetHomeURL(IntPtr hwnd, string baseUrl);
		IntPtr SciterCreateNSView(SciterRectangle frame);
		IntPtr SciterCreateWidget(SciterRectangle frame);
		IntPtr SciterCreateWindow(CreateWindowFlags creationFlags, SciterRectangle frame,
			MulticastDelegate delegt, IntPtr delegateParam, IntPtr parent);

		void SciterSetupDebugOutput(IntPtr hwndOrNull, IntPtr param, SciterXDef.DEBUG_OUTPUT_PROC pfOutput);

		#region DOM Element API

		SciterXDom.SCDOM_RESULT Sciter_UseElement(IntPtr he);
		SciterXDom.SCDOM_RESULT Sciter_UnuseElement(IntPtr he);
		SciterXDom.SCDOM_RESULT SciterGetRootElement(IntPtr hwnd, out IntPtr phe);
		SciterXDom.SCDOM_RESULT SciterGetFocusElement(IntPtr hwnd, out IntPtr phe);
		SciterXDom.SCDOM_RESULT SciterFindElement(IntPtr hwnd, SciterPoint pt, out IntPtr phe);
		SciterXDom.SCDOM_RESULT SciterGetChildrenCount(IntPtr he, out uint count);
		SciterXDom.SCDOM_RESULT SciterGetNthChild(IntPtr he, uint n, out IntPtr phe);
		SciterXDom.SCDOM_RESULT SciterGetParentElement(IntPtr he, out IntPtr pParentHe);

		SciterXDom.SCDOM_RESULT SciterGetElementHtmlCB(IntPtr he, bool outer, SciterXDom.LPCBYTE_RECEIVER rcv,
			IntPtr rcvParam);

		SciterXDom.SCDOM_RESULT SciterGetElementTextCB(IntPtr he, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);
		SciterXDom.SCDOM_RESULT SciterSetElementText(IntPtr he, string utf16, uint length);
		SciterXDom.SCDOM_RESULT SciterGetAttributeCount(IntPtr he, out uint pCount);

		SciterXDom.SCDOM_RESULT SciterGetNthAttributeNameCB(IntPtr he, uint n, SciterXDom.LPCSTR_RECEIVER rcv,
			IntPtr rcvParam);

		SciterXDom.SCDOM_RESULT SciterGetNthAttributeValueCB(IntPtr he, uint n, SciterXDom.LPCWSTR_RECEIVER rcv,
			IntPtr rcvParam);

		SciterXDom.SCDOM_RESULT SciterGetAttributeByNameCB(IntPtr he, string name, SciterXDom.LPCWSTR_RECEIVER rcv,
			IntPtr rcvParam);

		SciterXDom.SCDOM_RESULT SciterSetAttributeByName(IntPtr he, string name, string value);
		SciterXDom.SCDOM_RESULT SciterClearAttributes(IntPtr he);
		SciterXDom.SCDOM_RESULT SciterGetElementIndex(IntPtr he, out uint pIndex);
		SciterXDom.SCDOM_RESULT SciterGetElementType(IntPtr he, out IntPtr pType);
		SciterXDom.SCDOM_RESULT SciterGetElementTypeCB(IntPtr he, SciterXDom.LPCSTR_RECEIVER rcv, IntPtr rcvParam);

		SciterXDom.SCDOM_RESULT SciterGetStyleAttributeCB(IntPtr he, string name, SciterXDom.LPCWSTR_RECEIVER rcv,
			IntPtr rcvParam);

		SciterXDom.SCDOM_RESULT SciterSetStyleAttribute(IntPtr he, string name, string value);

		SciterXDom.SCDOM_RESULT SciterGetElementLocation(IntPtr he, out SciterRectangle pLocation,
			SciterXDom.ELEMENT_AREAS areas);

		SciterXDom.SCDOM_RESULT SciterScrollToView(IntPtr he, uint sciterScrollFlags);
		SciterXDom.SCDOM_RESULT SciterUpdateElement(IntPtr he, bool andForceRender);
		SciterXDom.SCDOM_RESULT SciterRefreshElementArea(IntPtr he, SciterRectangle rc);
		SciterXDom.SCDOM_RESULT SciterSetCapture(IntPtr he);
		SciterXDom.SCDOM_RESULT SciterReleaseCapture(IntPtr he);
		SciterXDom.SCDOM_RESULT SciterGetElementHwnd(IntPtr he, out IntPtr pHwnd, bool rootWindow);
		SciterXDom.SCDOM_RESULT SciterCombineURL(IntPtr he, IntPtr szUrlBuffer, uint urlBufferSize);

		SciterXDom.SCDOM_RESULT SciterSelectElements(IntPtr he, string cssSelectors,
			SciterXDom.SCITER_ELEMENT_CALLBACK callback, IntPtr param);

		SciterXDom.SCDOM_RESULT SciterSelectElementsW(IntPtr he, string cssSelectors,
			SciterXDom.SCITER_ELEMENT_CALLBACK callback, IntPtr param);

		SciterXDom.SCDOM_RESULT SciterSelectParent(IntPtr he, string selector, uint depth, out IntPtr heFound);
		SciterXDom.SCDOM_RESULT SciterSelectParentW(IntPtr he, string selector, uint depth, out IntPtr heFound);

		SciterXDom.SCDOM_RESULT SciterSetElementHtml(IntPtr he, byte[] html, uint htmlLength,
			SetElementHtml where);

		SciterXDom.SCDOM_RESULT SciterGetElementUID(IntPtr he, out uint puid);
		SciterXDom.SCDOM_RESULT SciterGetElementByUID(IntPtr hwnd, uint uid, out IntPtr phe);
		SciterXDom.SCDOM_RESULT SciterShowPopup(IntPtr he, IntPtr heAnchor, uint placement);
		SciterXDom.SCDOM_RESULT SciterShowPopupAt(IntPtr he, SciterPoint pos, uint placement);
		SciterXDom.SCDOM_RESULT SciterHidePopup(IntPtr he);
		SciterXDom.SCDOM_RESULT SciterGetElementState(IntPtr he, out uint pstateBits);

		SciterXDom.SCDOM_RESULT SciterSetElementState(IntPtr he, uint stateBitsToSet, uint stateBitsToClear,
			bool updateView);

		SciterXDom.SCDOM_RESULT SciterCreateElement(string tagname, string textOrNull, out IntPtr phe);
		SciterXDom.SCDOM_RESULT SciterCloneElement(IntPtr he, out IntPtr phe);
		SciterXDom.SCDOM_RESULT SciterInsertElement(IntPtr he, IntPtr hparent, uint index);
		SciterXDom.SCDOM_RESULT SciterDetachElement(IntPtr he);
		SciterXDom.SCDOM_RESULT SciterDeleteElement(IntPtr he);
		SciterXDom.SCDOM_RESULT SciterSetTimer(IntPtr he, uint milliseconds, IntPtr timerId);
		SciterXDom.SCDOM_RESULT SciterDetachEventHandler(IntPtr he, MulticastDelegate pep, IntPtr tag);
		SciterXDom.SCDOM_RESULT SciterAttachEventHandler(IntPtr he, MulticastDelegate pep, IntPtr tag);

		SciterXDom.SCDOM_RESULT SciterWindowAttachEventHandler(IntPtr hwndLayout, MulticastDelegate pep, IntPtr tag,
			uint subscription);

		SciterXDom.SCDOM_RESULT SciterWindowDetachEventHandler(IntPtr hwndLayout, MulticastDelegate pep, IntPtr tag);

		SciterXDom.SCDOM_RESULT SciterSendEvent(IntPtr he, uint appEventCode, IntPtr heSource, IntPtr reason,
			out bool handled);

		SciterXDom.SCDOM_RESULT SciterPostEvent(IntPtr he, uint appEventCode, IntPtr heSource, IntPtr reason);
		SciterXDom.SCDOM_RESULT SciterCallBehaviorMethod(IntPtr he, ref SciterXDom.METHOD_PARAMS param);
		SciterXDom.SCDOM_RESULT SciterRequestElementData(IntPtr he, string url, uint dataType, IntPtr initiator);

		SciterXDom.SCDOM_RESULT SciterHttpRequest(IntPtr he, string url, uint dataType, uint requestType,
			ref SciterXDom.REQUEST_PARAM requestParams, uint nParams);

		SciterXDom.SCDOM_RESULT SciterGetScrollInfo(IntPtr he, out SciterPoint scrollPos,
			out SciterRectangle viewRect, out SciterSize contentSize);

		SciterXDom.SCDOM_RESULT SciterSetScrollPos(IntPtr he, SciterPoint scrollPos, bool smooth);
		SciterXDom.SCDOM_RESULT SciterGetElementIntrinsicWidths(IntPtr he, out int pMinWidth, out int pMaxWidth);
		SciterXDom.SCDOM_RESULT SciterGetElementIntrinsicHeight(IntPtr he, int forWidth, out int pHeight);
		SciterXDom.SCDOM_RESULT SciterIsElementVisible(IntPtr he, out bool pVisible);
		SciterXDom.SCDOM_RESULT SciterIsElementEnabled(IntPtr he, out bool pEnabled);

		SciterXDom.SCDOM_RESULT SciterSortElements(IntPtr he, uint firstIndex, uint lastIndex,
			SciterXDom.ELEMENT_COMPARATOR cmpFunc, IntPtr cmpFuncParam);

		SciterXDom.SCDOM_RESULT SciterSwapElements(IntPtr he, IntPtr he2);
		SciterXDom.SCDOM_RESULT SciterTraverseUIEvent(IntPtr he, IntPtr eventCtlStruct, out bool bOutProcessed);

		SciterXDom.SCDOM_RESULT SciterCallScriptingMethod(IntPtr he, string name, SciterValue.VALUE[] argv, uint argc,
			out SciterValue.VALUE retval);

		SciterXDom.SCDOM_RESULT SciterCallScriptingFunction(IntPtr he, string name, SciterValue.VALUE[] argv, uint argc,
			out SciterValue.VALUE retval);

		SciterXDom.SCDOM_RESULT SciterEvalElementScript(IntPtr he, string script, uint scriptLength,
			out SciterValue.VALUE retval);

		SciterXDom.SCDOM_RESULT SciterAttachHwndToElement(IntPtr he, IntPtr hwnd);
		SciterXDom.SCDOM_RESULT SciterControlGetType(IntPtr he, out uint pType);
		SciterXDom.SCDOM_RESULT SciterGetValue(IntPtr he, out SciterValue.VALUE pval);
		SciterXDom.SCDOM_RESULT SciterSetValue(IntPtr he, ref SciterValue.VALUE pval);
		SciterXDom.SCDOM_RESULT SciterGetExpando(IntPtr he, out SciterValue.VALUE pval, bool forceCreation);

		/// <summary>
		/// <para>SciterGetObject - Get 'expando' object of the element. 'expando' is a scripting object (of class Element) <br/>
		/// that is assigned to the DOM element. 'expando' could be null as they are created on demand by script.</para>
		/// <para>ATTN!: If you plan to store the reference or use it inside code that calls script VM functions <br/>
		///  then you should use tiscript::pinned holder for the value.</para>
		/// </summary>
		/// <param name="he">Element which expando will be retrieved.</param>
		/// <param name="pval">Pointer to tiscript::value that will get reference to the scripting object associated with the element or null.</param>
		/// <param name="forceCreation">If there is no expando then when forceCreation==TRUE the function will create it.</param>
		/// <returns></returns>
		SciterXDom.SCDOM_RESULT SciterGetObject(IntPtr he, out IntPtr pval, bool forceCreation);

		/// <summary>
		/// <para>SciterGetElementNamespace - Get namespace of document of the DOM element.</para>
		/// <para>ATTN!: If you plan to store the reference or use it inside code that calls script VM functions</para>
		/// then you should use tiscript::pinned holder for the value.</summary>
		/// <param name="he">Element which expando will be retrieved.</param>
		/// <param name="pval">Pointer to tiscript::value that will get reference to the namespace scripting object. <br/>
		/// </param>
		/// <returns></returns>
		SciterXDom.SCDOM_RESULT SciterGetElementNamespace(IntPtr he, out IntPtr pval);
		SciterXDom.SCDOM_RESULT SciterGetHighlightedElement(IntPtr hwnd, out IntPtr phe);
		SciterXDom.SCDOM_RESULT SciterSetHighlightedElement(IntPtr hwnd, IntPtr he);

		#endregion

		#region DOM Node API

		SciterXDom.SCDOM_RESULT SciterNodeAddRef(IntPtr hn);
		SciterXDom.SCDOM_RESULT SciterNodeRelease(IntPtr hn);
		SciterXDom.SCDOM_RESULT SciterNodeCastFromElement(IntPtr he, out IntPtr phn);
		SciterXDom.SCDOM_RESULT SciterNodeCastToElement(IntPtr hn, out IntPtr he);
		SciterXDom.SCDOM_RESULT SciterNodeFirstChild(IntPtr hn, out IntPtr phn);
		SciterXDom.SCDOM_RESULT SciterNodeLastChild(IntPtr hn, out IntPtr phn);
		SciterXDom.SCDOM_RESULT SciterNodeNextSibling(IntPtr hn, out IntPtr phn);
		SciterXDom.SCDOM_RESULT SciterNodePrevSibling(IntPtr hn, out IntPtr phn);
		SciterXDom.SCDOM_RESULT SciterNodeParent(IntPtr hn, out IntPtr pheParent);
		SciterXDom.SCDOM_RESULT SciterNodeNthChild(IntPtr hn, uint n, out IntPtr phn);
		SciterXDom.SCDOM_RESULT SciterNodeChildrenCount(IntPtr hn, out uint pn);
		SciterXDom.SCDOM_RESULT SciterNodeType(IntPtr hn, out SciterXDom.NODE_TYPE pn);
		SciterXDom.SCDOM_RESULT SciterNodeGetText(IntPtr hn, SciterXDom.LPCWSTR_RECEIVER rcv, IntPtr rcvParam);

		SciterXDom.SCDOM_RESULT SciterNodeSetText(IntPtr hn, [MarshalAs(UnmanagedType.LPWStr)] string text,
			uint textLength);

		SciterXDom.SCDOM_RESULT SciterNodeInsert(IntPtr hn, uint where, IntPtr what);
		SciterXDom.SCDOM_RESULT SciterNodeRemove(IntPtr hn, bool finalize);
		SciterXDom.SCDOM_RESULT SciterCreateTextNode(string text, uint textLength, out IntPtr phnode);
		SciterXDom.SCDOM_RESULT SciterCreateCommentNode(string text, uint textLength, out IntPtr phnode);

		#endregion

		#region Value API

		SciterValue.VALUE_RESULT ValueInit(out SciterValue.VALUE pval);
		SciterValue.VALUE_RESULT ValueClear(out SciterValue.VALUE pval);
		SciterValue.VALUE_RESULT ValueCompare(ref SciterValue.VALUE pval, ref IntPtr pval2);
		SciterValue.VALUE_RESULT ValueCopy(out SciterValue.VALUE pdst, ref SciterValue.VALUE psrc);
		SciterValue.VALUE_RESULT ValueIsolate(ref SciterValue.VALUE pdst);
		SciterValue.VALUE_RESULT ValueType(ref SciterValue.VALUE pval, out uint pType, out uint pUnits);
		SciterValue.VALUE_RESULT ValueStringData(ref SciterValue.VALUE pval, out IntPtr pChars, out uint pNumChars);

		SciterValue.VALUE_RESULT
			ValueStringDataSet(ref SciterValue.VALUE pval, string chars, uint numChars, uint units);

		SciterValue.VALUE_RESULT ValueIntData(ref SciterValue.VALUE pval, out int pData);
		SciterValue.VALUE_RESULT ValueIntDataSet(ref SciterValue.VALUE pval, int data, uint type, uint units);
		SciterValue.VALUE_RESULT ValueInt64Data(ref SciterValue.VALUE pval, out long pData);
		SciterValue.VALUE_RESULT ValueInt64DataSet(ref SciterValue.VALUE pval, long data, uint type, uint units);
		SciterValue.VALUE_RESULT ValueFloatData(ref SciterValue.VALUE pval, out double pData);
		SciterValue.VALUE_RESULT ValueFloatDataSet(ref SciterValue.VALUE pval, double data, uint type, uint units);
		SciterValue.VALUE_RESULT ValueBinaryData(ref SciterValue.VALUE pval, out IntPtr pBytes, out uint pnBytes);

		SciterValue.VALUE_RESULT ValueBinaryDataSet(ref SciterValue.VALUE pval, byte[] pBytes, uint nBytes, uint type,
			uint units);

		SciterValue.VALUE_RESULT ValueElementsCount(ref SciterValue.VALUE pval, out int pn);
		SciterValue.VALUE_RESULT ValueNthElementValue(ref SciterValue.VALUE pval, int n, out SciterValue.VALUE pretval);

		SciterValue.VALUE_RESULT ValueNthElementValueSet(ref SciterValue.VALUE pval, int n,
			ref SciterValue.VALUE pvalToSet);

		SciterValue.VALUE_RESULT ValueNthElementKey(ref SciterValue.VALUE pval, int n, out SciterValue.VALUE pretval);

		SciterValue.VALUE_RESULT ValueEnumElements(ref SciterValue.VALUE pval, SciterValue.KEY_VALUE_CALLBACK penum,
			IntPtr param);

		SciterValue.VALUE_RESULT ValueSetValueToKey(ref SciterValue.VALUE pval, ref SciterValue.VALUE pkey,
			ref SciterValue.VALUE pvalToSet);

		SciterValue.VALUE_RESULT ValueGetValueOfKey(ref SciterValue.VALUE pval, ref SciterValue.VALUE pkey,
			out SciterValue.VALUE pretval);

		SciterValue.VALUE_RESULT ValueToString(ref SciterValue.VALUE pval, SciterValue.VALUE_STRING_CVT_TYPE how);
		SciterValue.VALUE_RESULT ValueFromString(ref SciterValue.VALUE pval, string str, uint strLength, uint how);

		SciterValue.VALUE_RESULT ValueInvoke(ref SciterValue.VALUE pval, ref SciterValue.VALUE pthis, uint argc,
			SciterValue.VALUE[] argv, out SciterValue.VALUE pretval, string url);

		SciterValue.VALUE_RESULT ValueNativeFunctorSet(ref SciterValue.VALUE pval,
			SciterValue.NATIVE_FUNCTOR_INVOKE pinvoke, SciterValue.NATIVE_FUNCTOR_RELEASE prelease, IntPtr tag);

		SciterValue.VALUE_RESULT ValueIsNativeFunctor(ref SciterValue.VALUE pval);

		#endregion

		#region Used to be TIScript VM API (Deprecated in v4.4.3.24)

		[Obsolete("Removed in Sciter v4.4.3.24", false)]
		IntPtr GetTIScriptApi();

		[Obsolete("Removed in Sciter v4.4.3.24", false)]
		IntPtr SciterGetVM(IntPtr hwnd);

		[Obsolete("Removed in Sciter v4.4.3.24", false)]
		bool Sciter_v2V(IntPtr vm, SciterScript.ScriptValue scriptValue, ref SciterValue.VALUE value, bool isolate);

		[Obsolete("Removed in Sciter v4.4.3.24", false)]
		bool Sciter_V2v(IntPtr vm, ref SciterValue.VALUE value, ref SciterScript.ScriptValue scriptValue);

		[Obsolete("Reserved in Sciter v4.4.3.24", true)]
		void Reserved1();

		[Obsolete("Reserved in Sciter v4.4.3.24", true)]
		void Reserved2();

		[Obsolete("Reserved in Sciter v4.4.3.24", true)]
		void Reserved3();

		[Obsolete("Reserved in Sciter v4.4.3.24", true)]
		void Reserved4();

		#endregion

		#region Archive

		IntPtr SciterOpenArchive(IntPtr archiveData, uint archiveDataLength);

		bool SciterGetArchiveItem(IntPtr harc, [MarshalAs(UnmanagedType.LPWStr)] string path, out IntPtr pdata,
			out uint pdataLength);

		bool SciterCloseArchive(IntPtr harc);

		#endregion

		SciterXDom.SCDOM_RESULT SciterFireEvent(SciterBehaviorArgs evt, bool post,
			out bool handled);

		IntPtr SciterGetCallbackParam(IntPtr hwnd);
		IntPtr SciterPostCallback(IntPtr hwnd, IntPtr wparam, IntPtr lparam, uint timeoutms);
		IntPtr GetSciterGraphicsAPI();
		IntPtr GetSciterRequestAPI();

		#region DirectX

		bool SciterCreateOnDirectXWindow(IntPtr hwnd, IntPtr pSwapChain);
		bool SciterRenderOnDirectXWindow(IntPtr hwnd, IntPtr elementToRenderOrNull, bool frontLayer);
		bool SciterRenderOnDirectXTexture(IntPtr hwnd, IntPtr elementToRenderOrNull, IntPtr surface);

		#endregion

		bool SciterProcX(IntPtr hwnd, IntPtr pMsg);

		#region Sciter 4.4.3.24

		ulong SciterAtomValue(string name);

		bool SciterAtomNameCB(ulong atomv, IntPtr rxc, IntPtr rcvParam);

		bool SciterSetGlobalAsset(IntPtr pass);

		#endregion

		#region Sciter 4.4.4.7

		SciterXDom.SCDOM_RESULT SciterGetElementAsset(IntPtr el, ulong nameAtom, out IntPtr ppass);
		//bool SciterGetVariable(IntPtr hwndOrNull, string path, ref SciterValue.VALUE pvalToGet);
		//bool SciterSetVariable(IntPtr hwndOrNull, string path, ref SciterValue.VALUE pvalToSet);

		#endregion

		#region Sciter 4.4.5.4
		uint SciterSetVariable(IntPtr hwndOrNull, string path, ref SciterValue.VALUE pvalToSet);
		uint SciterGetVariable(IntPtr hwndOrNull, string path, out SciterValue.VALUE pvalToGet);
		uint SciterElementUnwrap(ref SciterValue.VALUE pval, out IntPtr ppElement);
		uint SciterElementWrap(ref SciterValue.VALUE pval, IntPtr ppElement);
		uint SciterNodeUnwrap(ref SciterValue.VALUE pval, out IntPtr ppNode);
		uint SciterNodeWrap(ref SciterValue.VALUE pval, IntPtr ppNode);

		#endregion
	}
}