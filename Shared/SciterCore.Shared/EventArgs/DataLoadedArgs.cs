using System;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SciterCore
{
    /// <summary>
    /// SCN_DATA_LOADED
    /// </summary>
    public struct DataLoadedArgs
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
        public Uri Uri { get; internal set; }

        /// <summary>
        /// <para>Pointer to loaded data.</para>
        /// <para>You can use <see cref="System.Runtime.InteropServices.Marshal.Copy(byte[],int,System.IntPtr,int)"/> to get the raw <see cref="byte"/>[]</para>
        /// <para>example:<br/>byte[] managedArray = new byte[unchecked((int)<see cref="DataSize"/>)];<br/>
        /// Marshal.Copy(loadData.data, managedArray, 0, managedArray.Length);</para>
        /// </summary>
        public IntPtr Data { get; internal set; }
        
        /// <summary>
        /// loaded data size (in bytes).
        /// </summary>
        public int DataSize { get; internal set; }
        
        /// <summary>
        /// SciterResourceType
        /// </summary>
        public SciterResourceType DataType { get; internal set; }
        
        /// <summary>
        /// <para>Status = 0 (<see cref="DataSize"/> == 0) - Unknown error.</para>
        /// <para>Status = 100...505 - Http response status, Note: 200 - OK!</para>
        /// <para>Status > 12000 - wininet error code, see ERROR_INTERNET_*** in wininet.h</para>
        /// </summary>
        public int Status { get; internal set; }

        public override string ToString()
        {
            return
                $"{nameof(WindowHandle)}: {WindowHandle.ToInt64()}; {nameof(Uri)}: {Uri}; {nameof(Code)}: {Code}; {nameof(DataType)}: {DataType}; {nameof(DataSize)}: {DataSize}; {nameof(Status)}: {Status}";
        }
    }
}