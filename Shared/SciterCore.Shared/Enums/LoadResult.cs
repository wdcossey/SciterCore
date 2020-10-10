namespace SciterCore
{
    public enum LoadResult : int
    {
        /// <summary>
        /// Do default loading if data not set
        /// </summary>
        Ok = 0,

        /// <summary>
        /// Discard request completely
        /// </summary>
        Discard = 1,

        /// <summary>
        /// <para>Data will be delivered later by the host</para>
        /// <para>Host application must call SciterDataReadyAsync(,,, requestId) on each LOAD_DELAYED request to avoid memory leaks.</para>
        /// </summary>
        Delayed = 2,

        /// <summary>
        /// <para>You return LOAD_MYSELF result to indicate that your (the host) application took or will take care about HREQUEST in your code completely.</para>
        /// <para>Use sciter-x-request.h[pp] API functions with SCN_LOAD_DATA::requestId handle.</para>
        /// </summary>
        Myself = 3,
    }
}