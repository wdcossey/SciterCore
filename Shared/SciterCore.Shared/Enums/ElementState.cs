using System;

namespace SciterCore
{
	[Flags]
    public enum ElementState : long
    {
	    None		     = 0,
	    
        Link             = 0x00000001,
		Hover            = 0x00000002,
		Active           = 0x00000004,
		Focus            = 0x00000008,
		Visited          = 0x00000010,
		
		/// <summary>
		/// Current (Hot) item 
		/// </summary>
		Current          = 0x00000020,
		
		/// <summary>
		/// Element is Checked (or Selected)
		/// </summary>
		Checked          = 0x00000040,
		
		/// <summary>
		/// Element is Disabled
		/// </summary>
		Disabled         = 0x00000080,
		
		/// <summary>
		/// Readonly Input Element 
		/// </summary>
		Readonly         = 0x00000100,
		
		/// <summary>
		/// Expanded State - Nodes in TreeView 
		/// </summary>
		Expanded         = 0x00000200,
		
		/// <summary>
		/// Collapsed State - Nodes in TreeView - Mutually exclusive with
		/// </summary>
		Collapsed        = 0x00000400,
		
		/// <summary>
		/// One of Bore/Back images requested but not delivered
		/// </summary>
		Incomplete       = 0x00000800,
		
		/// <summary>
		/// Is animating currently
		/// </summary>
		Animating        = 0x00001000,
		
		/// <summary>
		/// Will accept focus
		/// </summary>
		Focusable        = 0x00002000,
		
		/// <summary>
		/// Anchor in selection (used with current in selects)
		/// </summary>
		Anchor           = 0x00004000,
		
		/// <summary>
		/// This is a synthetic element - don't emit it's head/tail
		/// </summary>
		Synthetic        = 0x00008000,
		
		/// <summary>
		/// This is a synthetic element - don't emit it's head/tail
		/// </summary>
		OwnsPopup       = 0x00010000,
		
		/// <summary>
		/// Focus gained by Tab traversal
		/// </summary>
		TabFocus         = 0x00020000,
		
		/// <summary>
		/// Element is empty (text.size() == 0 &amp; subs.size() == 0) 
		/// If element has behavior attached then the behavior is responsible for the value of this flag.
		/// </summary>
		Empty            = 0x00040000, 
										  
		/// <summary>
		/// Busy and/or Loading
		/// </summary>
		Busy             = 0x00080000,

		/// <summary>
		/// Drag over the block that can accept it (so is current drop target).
		/// Flag is set for the drop target block
		/// </summary>
		DragOver        = 0x00100000,
		
		/// <summary>
		/// Active drop target.
		/// </summary>
		DropTarget      = 0x00200000,  // 
		
		/// <summary>
		/// Dragging/Moving - the flag is set for the moving block.
		/// </summary>
		Moving           = 0x00400000,
		
		/// <summary>
		/// Dragging/Copying - the flag is set for the copying block.
		/// </summary>
		Copying          = 0x00800000,
		
		/// <summary>
		/// Element that is a drag source.
		/// </summary>
		DragSource      = 0x01000000,
		
		/// <summary>
		/// Element is drop marker
		/// </summary>
		DropMarker      = 0x02000000,

		/// <summary>
		/// Close to active but has wider life span - e.g. in MOUSE_UP it
		/// is still on; so behavior can check it in MOUSE_UP to discover CLICK condition.
		/// </summary>
		Pressed          = 0x04000000,

		/// <summary>
		/// This element is out of flow - Popup 
		/// </summary>
	    PopUp            = 0x08000000,

		/// <summary>
		/// The Element or one of its containers has dir=ltr declared
		/// </summary>
		IsLeftToRight           = 0x10000000,
		
		/// <summary>
		/// The Element or one of its containers has dir=rtl declared
		/// </summary>
		IsRightToLeft           = 0x20000000,
    }
}