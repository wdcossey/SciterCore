// ReSharper disable InconsistentNaming
namespace SciterCore
{
	public enum BehaviorEvents
	{
		/// <summary>
		/// Click on button
		/// </summary>
		ButtonClick = 0,

		/// <summary>
		/// Mouse/Key down in button
		/// </summary>
		ButtonPress = 1,

		/// <summary>
		/// Checkbox/Radio/Slider changed its state/value
		/// </summary>
		ButtonStateChanged = 2,

		/// <summary>
		/// Before text change
		/// </summary>
		EditValueChanging = 3,

		/// <summary>
		/// After text change
		/// </summary>
		EditValueChanged = 4,

		/// <summary>
		/// Selection in &lt;select&gt; changed
		/// </summary>
		SelectSelectionChanged = 5,

		/// <summary>
		/// Node in select expanded/collapsed, heTarget is the node
		/// </summary>
		SelectStateChanged = 6,

		/// <summary>
		/// Request to show popup just received,
		/// here DOM of popup element can be modified.
		/// </summary>
		PopupRequest = 7,

		/// <summary>
		/// Popup element has been measured and ready to be shown on screen,
		/// here you can use functions like ScrollToView.
		/// </summary>
		PopupReady = 8,

		/// <summary>
		/// Popup element is closed,
		/// here DOM of popup element can be modified again - e.g. some items can be removed
		/// to free memory.
		/// </summary>
		PopupDismissed = 9,

		/// <summary>
		/// Menu item activated by mouse hover or by keyboard
		/// </summary>
		MenuItemActive = 0xA,

		/// <summary>
		/// Menu item click,
		/// BEHAVIOR_EVENT_PARAMS structure layout
		/// BEHAVIOR_EVENT_PARAMS.cmd - MENU_ITEM_CLICK/MENU_ITEM_ACTIVE <br/>
		/// BEHAVIOR_EVENT_PARAMS.heTarget - owner(anchor) of the menu <br/>
		/// BEHAVIOR_EVENT_PARAMS.he - the menu item, presumably &lt;li&gt; element <br/>
		/// BEHAVIOR_EVENT_PARAMS.reason - BY_MOUSE_CLICK | BY_KEY_CLICK
		/// </summary>
		MenuItemClick = 0xB,

		/// <summary>
		/// "Right-Click", BEHAVIOR_EVENT_PARAMS::he is current popup menu HELEMENT being processed or `null`. <br/>
		/// Application can provide its own HELEMENT here (if it is `null`) or modify current menu element.
		/// </summary>
		ContextMenuRequest = 0x10,

		/// <summary>
		/// Broadcast notification, sent to all elements of some container being shown or hidden
		/// </summary>
		VisualStatusChanged = 0x11,

		/// <summary>
		/// Broadcast notification, sent to all elements of some container that got new value of :disabled state
		/// </summary>
		DisabledStatusChanged = 0x12,

		/// <summary>
		/// Popup is about to be closed
		/// </summary>
		PopupDismissing = 0x13,

		/// <summary>
		/// Content has been changed, is posted to the element that gets content changed,  reason is combination of CONTENT_CHANGE_BITS. <br/>
		/// Target == `null` means the window got new document and this event is dispatched only to the window.
		/// </summary>
		ContentChanged = 0x15,

		/// <summary>
		/// Generic click
		/// </summary>
		Click = 0x16,

		/// <summary>
		/// Generic change
		/// </summary>
		Change = 0x17,

		// "grey" event codes  - notfications from behaviors from this SDK

		/// <summary>
		/// Hyperlink click
		/// </summary>
		HyperlinkClick = 0x80,

		///// <summary>
		///// Click on some cell in table header,
		///// target = the cell,
		///// reason = index of the cell (column number, 0..n)
		///// </summary>
		//TableHeaderClick,

		///// <summary>
		///// Click on data row in the table, target is the row
		///// target = the row,
		///// reason = index of the row (fixed_rows..n)
		///// </summary>
		//TableRowClick,
		
		///// <summary>
		///// Mouse dbl click on data row in the table, target is the row
		///// target = the row,
		///// reason = index of the row (fixed_rows..n)
		///// </summary>
		//TableRowDoubleClick,

		/// <summary>
		/// Element was collapsed, so far only behavior:tabs is sending these two to the panels
		/// </summary>
		ElementCollapsed = 0x90, // 

		/// <summary>
		/// Element was expanded,
		/// </summary>
		ElementExpanded = 0x91, // 

		/// <summary>
		/// Activate (select) child,
		/// used for example by accesskeys behaviors to send activation request, e.g. tab on behavior:tabs.
		/// </summary>
		ActivateChild = 0x92,

		///// <summary>
		///// command to switch tab programmatically, handled by behavior:tabs
		///// use it as HTMLayoutPostEvent(tabsElementOrItsChild, DO_SWITCH_TAB, tabElementToShow, 0);
		///// </summary>
		//DoSwitchTab = ActivateChild,
		
		///// <summary>
		///// Request to virtual grid to initialize its view
		///// </summary>
		//InitDataView,


		///// <summary>
		///// request from virtual grid to data source behavior to fill data in the table
		///// parameters passed through DATA_ROWS_PARAMS structure.
		///// </summary>
		//RowsDataRequest,

		/// <summary>
		/// UI state changed, observers shall update their visual states.
		/// is sent for example by behavior:richtext when caret position/selection has changed.
		/// </summary>
		UIStateChanged = 0x95,

		/// <summary>
		/// behavior:form detected submission event. BEHAVIOR_EVENT_PARAMS::data field contains data to be posted. <br/>
		/// BEHAVIOR_EVENT_PARAMS::data is of type T_MAP in this case key/value pairs of data that is about
		/// to be submitted. You can modify the data or discard submission by returning true from the handler.
		/// </summary>
		FormSubmit = 0x96,

		/// <summary>
		/// Behavior:form detected reset event (from button type=reset). BEHAVIOR_EVENT_PARAMS::data field contains data to be reset. <br/>
		/// BEHAVIOR_EVENT_PARAMS::data is of type T_MAP in this case key/value pairs of data that is about
		/// to be rest. You can modify the data or discard reset by returning true from the handler.
		/// </summary>
		FormReset = 0x97,

		/// <summary>
		/// Document in behavior:frame or root document is complete.
		/// </summary>
		DocumentComplete = 0x98,

		/// <summary>
		/// Requests to behavior:history (commands)
		/// </summary>
		HistoryPush = 0x99,

		/// <summary>
		/// 
		/// </summary>
		HistoryDrop = 0x9A,

		/// <summary>
		/// 
		/// </summary>
		HistoryPrior = 0x9B,

		/// <summary>
		/// 
		/// </summary>
		HistoryNext = 0x9C,

		/// <summary>
		/// Behavior:history notification - history stack has changed
		/// </summary>
		HistoryStateChanged = 0x9D, // 

		/// <summary>
		/// Close popup request,
		/// </summary>
		ClosePopup = 0x9E,

		/// <summary>
		/// Request tooltip, evt.source &lt;- is the tooltip element.
		/// </summary>
		RequestTooltip = 0x9F,

		/// <summary>
		/// Animation started (reason=1) or ended(reason=0) on the element.
		/// </summary>
		Animation = 0xA0,

		/// <summary>
		/// Document created, script namespace initialized. target -> the document
		/// </summary>
		DocumentCreated = 0xC0,

		/// <summary>
		/// Document is about to be closed, to cancel closing do: evt.data = sciter::value("cancel");
		/// </summary>
		DocumentCloseRequest = 0xC1,

		/// <summary>
		/// Last notification before document removal from the DOM
		/// </summary>
		DocumentClose = 0xC2,

		/// <summary>
		/// Document has got DOM structure, styles and behaviors of DOM elements. Script loading run is complete at this moment.
		/// </summary>
		DocumentReady = 0xC3,

		/// <summary>
		/// Document just finished parsing - has got DOM structure. This event is generated before DOCUMENT_READY
		/// </summary>
		DocumentParsed = 0xC4,


		/// <summary>
		/// &lt;video&gt; "ready" notification   
		/// </summary>
		VideoInitialized = 0xD1,

		/// <summary>
		/// &lt;video&gt; playback started notification   
		/// </summary>
		VideoStarted = 0xD2,

		/// <summary>
		/// &lt;video&gt; playback stopped/paused notification   
		/// </summary>
		VideoStopped = 0xD3,

		/// <summary>
		/// <para>&lt;video&gt; request for frame source binding,
		/// If you want to provide your own video frames source for the given target &lt;video&gt; element do the following:</para>
		/// <para>
		/// 1. Handle and consume this <see cref="VideoBindRq"/> request <br/>
		/// 2. You will receive second <see cref="VideoBindRq"/> request/event for the same &lt;video&gt; element <br/>
		/// but this time with the 'reason' field set to an instance of sciter::video_destination interface. <br/>
		/// 3. Add_ref() it and store it for example in worker thread producing video frames. <br/>
		/// 4. call sciter::video_destination::start_streaming(...) providing needed parameters <br/>
		/// call sciter::video_destination::render_frame(...) as soon as they are available <br/>
		/// call sciter::video_destination::stop_streaming() to stop the rendering (a.k.a. end of movie reached) <br/>
		/// </para>
		/// </summary>
		VideoBindRq = 0xD4,

		/// <summary>
		/// Behavior: Pager starts pagination
		/// </summary>
		PaginationStarts = 0xE0,

		/// <summary>
		/// Behavior: Pager paginated page no, reason -> page no
		/// </summary>
		PaginationPage = 0xE1, // 

		/// <summary>
		/// Behavior: Pager end pagination, reason -> total pages
		/// </summary>
		PaginationEnds = 0xE2, // 

		/// <summary>
		/// Event with custom name
		/// </summary>
		Custom = 0xF0,

		FirstApplicationEventCode = 0x100
		// all custom event codes shall be greater
		// than this number. All codes below this will be used
		// solely by application - HTMLayout will not interpret it
		// and will do just dispatching.
		// To send event notifications with these codes use
		// HTMLayoutSend/PostEvent API.
	}
}
// ReSharper enable InconsistentNaming