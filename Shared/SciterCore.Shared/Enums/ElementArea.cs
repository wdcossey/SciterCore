using System;

namespace SciterCore
{
    [Flags]
    public enum ElementArea : int
    {
        /// <summary>
        /// `or` this flag if you want to get HTMLayout window relative coordinates,
        /// otherwise it will use nearest windowed container e.g. popup window.
        /// </summary>
        RootRelative = 0x01,

        /// <summary>
        /// `or` this flag if you want to get coordinates relative to the origin
        /// of element itself.
        /// </summary>
        SelfRelative = 0x02,
			
        /// <summary>
        /// Position inside immediate container.
        /// </summary>
        ContainerRelative = 0x03,
			
        /// <summary>
        /// Position relative to view - HTMLayout window
        /// </summary>
        ViewRelative = 0x04,

        /// <summary>
        /// Content (inner) box
        /// </summary>
        ContentBox = 0x00,
			
        /// <summary>
        /// Content and Padding
        /// </summary>
        PaddingBox = 0x10,
			
        /// <summary>
        /// Content, Padding and Border
        /// </summary>
        BorderBox = 0x20,
			
        /// <summary>
        /// Content, Padding, Border and Margins
        /// </summary>
        MarginBox = 0x30,

        /// <summary>
        /// Relative to content origin - location of background image (if it set no-repeat)
        /// </summary>
        BackImageArea = 0x40,
			
        /// <summary>
        /// Relative to content origin - location of foreground image (if it set no-repeat)
        /// </summary>
        ForeImageArea = 0x50,

        /// <summary>
        /// Scroll_area - scrollable area in content box
        /// </summary>
        ScrollableArea = 0x60,
    }
}