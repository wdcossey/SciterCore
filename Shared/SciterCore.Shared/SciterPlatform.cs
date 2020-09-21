using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using SciterCore.Interop;

namespace SciterCore
{
    public static class SciterPlatform
    {
        /// <summary>
        /// <para>Applications that use the following functionality must call OleInitialize before calling any other function in the COM library:</para>
        /// <para>Clipboard, Drag and Drop, Object linking and embedding (OLE), In-place activation</para>
        /// <para>https://docs.microsoft.com/en-us/windows/win32/api/ole2/nf-ole2-oleinitialize</para>
        /// </summary>
#if WINDOWS && !NETCORE
        public static void EnableDragAndDrop()
        {
            var oleThread = new Thread(() =>
            {
                var oleResult = PInvokeWindows.OleInitialize(IntPtr.Zero);
                Debug.Assert(oleResult == 0);
            });
            oleThread.SetApartmentState(ApartmentState.STA);
            oleThread.Start();
            oleThread.Join();
        }
#elif NETCORE
        public static void EnableDragAndDrop()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
                return;
            
            var oleThread = new Thread(() =>
            {
                var oleResult = PInvokeWindows.OleInitialize(IntPtr.Zero);
                Debug.Assert(oleResult == 0);
            });
            oleThread.SetApartmentState(ApartmentState.STA);
            oleThread.Start();
            oleThread.Join();
        }
#else
        public static void EnableDragAndDrop()
        {
            //Not supported
        }
#endif 
    }
}