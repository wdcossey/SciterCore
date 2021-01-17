using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SciterCore
{
    /// <summary>
    /// SCN_LOAD_DATA
    /// </summary>
    public struct LoadDataArgs
    {
        /// <summary>
        /// One of the codes above.
        /// </summary>
        public int Code { get; internal set; }

        ///// <summary>
        ///// The <see cref="SciterWindow"/> this callback was attached to.
        ///// </summary>
        //public SciterWindow Window { get; internal set; }

        /// <summary>
        /// The <see cref="SciterWindow"/> this callback was attached to.
        /// </summary>
        public IntPtr WindowHandle { get; internal set; }
        
        /// <summary>
        /// Zero terminated string, fully qualified uri, for example "http://server/folder/file.ext".
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Pointer to loaded data to return. If data exists in the cache then this field contain pointer to it
        /// </summary>
        public IntPtr Data { get; set; }
        
        /// <summary>
        /// Loaded data size to return.
        /// </summary>
        public int DataSize { get; set; }
        
        /// <summary>
        /// SciterResourceType
        /// </summary>
        public SciterResourceType DataType { get; internal set; }

        /// <summary>
        /// Request handle that can be used with sciter-x-request API
        /// </summary>
        public IntPtr RequestId { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public IntPtr Principal { get; internal set; }
        
        /// <summary>
        /// 
        /// </summary>
        public IntPtr Initiator { get; internal set; }

        public override string ToString()
        {
            return
                $"{nameof(WindowHandle)}: {WindowHandle.ToInt64()}; {nameof(Uri)}: {Uri}; {nameof(Code)}: {Code}; {nameof(DataType)}: {DataType}; {nameof(DataSize)}: {DataSize}; {nameof(RequestId)}: {RequestId.ToInt64()}; {nameof(Principal)}: {Principal.ToInt64()}; {nameof(Initiator)}: {Initiator.ToInt64()}";
        }
    }
}