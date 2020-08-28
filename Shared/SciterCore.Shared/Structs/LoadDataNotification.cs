using System;

namespace SciterCore
{
    
    public struct LoadData
    {
        /// <summary>
        /// one of the codes above.
        /// </summary>
        public uint Code { get; internal set; } // UINT - [in] 

        /// <summary>
        /// HWINDOW of the window this callback was attached to.
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

        public IntPtr Principal { get; internal set; }		// HELEMENT
        
        public IntPtr Initiator { get; internal set; }		// HELEMENT
    }
    
    public struct DataLoaded
    {
        /// <summary>
        /// [in] one of the codes above.
        /// </summary>
        public uint Code;	// UINT - 
        
        /// <summary>
        /// HWINDOW of the window this callback was attached to.
        /// </summary>
        public IntPtr Handle;	// HWINDOW - [in] 
        
        /// <summary>
        /// Zero terminated string, fully qualified uri, for example "http://server/folder/file.ext".
        /// </summary>
        public string Uri;  // LPCWSTR - [in] 

        /// <summary>
        /// pointer to loaded data.
        /// </summary>
        public IntPtr Data;									// LPCBYTE - [in] 
        
        /// <summary>
        /// loaded data size (in bytes).
        /// </summary>
        public uint DataSize;								// UINT - [in] 
        
        /// <summary>
        /// SciterResourceType
        /// </summary>
        public SciterResourceType DataType;	// UINT - [in] 
        
        /// <summary>
        /// <para>Status = 0 (<see cref="DataSize"/> == 0) - Unknown error. </para>
        /// <para>Status = 100...505 - Http response status, Note: 200 - OK! </para>
        /// <para>Status > 12000 - wininet error code, see ERROR_INTERNET_*** in wininet.h</para>
        /// </summary>
        public uint Status;									// UINT - [in] 

    }
}