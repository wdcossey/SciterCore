namespace SciterCore
{
    public enum DragAndDropMode : int
    {
        /// <summary>
        /// DROPEFFECT_NONE	( 0 )
        /// </summary>
        None = 0,
        
        /// <summary>
        /// DROPEFFECT_COPY	( 1 )
        /// </summary>
        Copy = 1,
        
        /// <summary>
        /// DROPEFFECT_MOVE	( 2 )
        /// </summary>
        Move = 2,
        
        /// <summary>
        /// DROPEFFECT_COPY	( 1 ) | DROPEFFECT_MOVE	( 2 )
        /// </summary>
        CopyOrMove = 3,
        
        /// <summary>
        /// DROPEFFECT_LINK	( 4 )
        /// </summary>
        Link = 4,
    }
}