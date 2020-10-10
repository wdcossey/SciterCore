namespace SciterCore
{
    public enum FocusEvents : int
    {
        /// <summary>
        /// <para>Container got focus on a <see cref="SciterElement"/> inside it.</para>
        /// Target is the <see cref="SciterElement"/> that got focus.
        /// </summary>
        Out = 0,
        
        /// <summary>
        /// <para>Container lost focus from any <see cref="SciterElement"/> inside it.</para>
        /// Target is the <see cref="SciterElement"/> that lost focus.
        /// </summary>
        In = 1,
        
        /// <summary>
        /// Target <see cref="SciterElement"/> got focus.
        /// </summary>
        Got = 2,
        
        /// <summary>
        /// Target <see cref="SciterElement"/> lost focus.
        /// </summary>
        Lost = 3,
        
        /// <summary>
        /// Bubbling event/request, gets sent on child-parent chain to accept/reject focus to be set on the child (target)
        /// </summary>
        Request = 4,
        
        /// <summary>
        /// Bubbling event/request, gets sent on child-parent chain to advance focus 
        /// </summary>
        AdvanceRequest = 5,
    }
}