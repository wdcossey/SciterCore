using System;
using SciterCore.Interop;

namespace SciterCore.PlatformWrappers
{
    internal interface ISciterWindowWrapper
    {
        //SciterWindow(IntPtr hwnd, bool weakReference = false)

        IntPtr GetWindowHandle(IntPtr handle);

        void CreateWindow(PInvokeUtils.RECT frame = new PInvokeUtils.RECT(),
            SciterXDef.SCITER_CREATE_WINDOW_FLAGS creationFlags = SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_MAIN |
                                                                  SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_TITLEBAR |
                                                                  SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE |
                                                                  SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CONTROLS, IntPtr parent = new IntPtr());
        
        //SciterWindow CreateChildWindow(IntPtr hwndParent, SciterXDef.SCITER_CREATE_WINDOW_FLAGS flags = SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_CHILD)
        
        void Destroy(IntPtr handle);
        
        //bool ModifyStyle(PInvokeWindows.WindowStyles dwRemove, PInvokeWindows.WindowStyles dwAdd);
        
        void CenterWindow(IntPtr handle);
        
        SciterSize GetPrimaryScreenSize();

        SciterSize GetScreenSize(IntPtr handle);

        SciterSize Size(IntPtr handle);

        SciterPoint GetPosition(IntPtr handle);

        void SetPosition(IntPtr handle, SciterPoint point);
        
        void Show(IntPtr handle, bool show = true);
        
        void ShowModal(IntPtr handle);
        
        void Close(IntPtr handle);
        
        bool GetIsVisible(IntPtr handle);

        //ISciterWindow SetIcon(Icon icon);
        
        void SetTitle(IntPtr handle, string title);
        
        string GetTitle(IntPtr handle);
        
        //private IntPtr InternalProcessSciterWindowMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, IntPtr pParam, ref bool handled)
    }
}