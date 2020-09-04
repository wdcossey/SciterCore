using System;

namespace SciterCore
{
    public class DataLoadedEventArgs
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