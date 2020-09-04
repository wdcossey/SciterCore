using System;

namespace SciterCore
{
    public class LoadDataEventArgs
    {
        /// <summary>
        /// one of the codes above.
        /// </summary>
        public uint Code { get; internal set; } // UINT - [in] 

        /// <summary>
        /// Handle of the <see cref="SciterWindow"/> this callback was attached to.
        /// </summary>
        public IntPtr Handle { get; internal set; }				// HWINDOW - [in] 
        
        /// <summary>
        /// Zero terminated string, fully qualified uri, for example "http://server/folder/file.ext".
        /// </summary>
        public Uri Uri { get; internal set; }           /// [in] 

        /// <summary>
        /// Pointer to loaded data to return. if data exists in the cache then this field contain pointer to it
        /// </summary>
        public IntPtr Data { get; internal set; }								// LPCBYTE - [in,out] 
        
        /// <summary>
        /// Loaded data size to return.
        /// </summary>
        public uint DataSize { get; internal set; }							// UINT - [in,out] 
        
        /// <summary>
        /// SciterResourceType
        /// </summary>
        public SciterResourceType DataType { get; internal set; }	// UINT - [in] 

        /// <summary>
        /// Request handle that can be used with sciter-x-request API
        /// </summary>
        public IntPtr RequestId { get; internal set; }      // HREQUEST - [in] 

        /// <summary>
        /// <see cref="SciterElement"/> Handle
        /// </summary>
        public IntPtr Principal { get; internal set; }
        
        /// <summary>
        /// <see cref="SciterElement"/> Handle
        /// </summary>
        public IntPtr Initiator { get; internal set; }	
    }
}