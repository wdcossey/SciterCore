using System;

namespace SciterCore.Interop
{
    internal static class InteropExtensions
    {
        internal static LoadDataArgs ToEventArgs(this SciterXDef.SCN_LOAD_DATA loadData)
        {
            return new LoadDataArgs
            {
                Code = unchecked((int)loadData.code),
                Data = loadData.outData,
                Window = new SciterWindow(loadData.hwnd),
                Initiator = loadData.initiator,
                Principal = loadData.principal,
                Uri = new Uri(loadData.uri, UriKind.RelativeOrAbsolute),
                DataSize = unchecked((int)loadData.outDataSize),
                DataType = (SciterResourceType)unchecked((int)loadData.dataType),
                RequestId = loadData.requestId
            };
        }
        
        internal static DataLoadedArgs ToEventArgs(this SciterXDef.SCN_DATA_LOADED loadData)
        {
            return new DataLoadedArgs
            {
                Code = unchecked((int)loadData.code),
                Data = loadData.data,
                Window = new SciterWindow(loadData.hwnd),
                Uri = new Uri(loadData.uri, UriKind.RelativeOrAbsolute),
                DataSize = unchecked((int)loadData.dataSize),
                DataType = (SciterResourceType)unchecked((int)loadData.dataType),
                Status = unchecked((int)loadData.status),
            };
        }
        
        //internal static SciterXDef.SCN_DATA_LOADED FromEventArgs(this DataLoadedEventArgs loadData)
        //{
        //    return new SciterXDef.SCN_DATA_LOADED
        //    {
        //        code = loadData.Code,
        //        data = loadData.Data,
        //        hwnd = loadData.Handle,
        //        uri = loadData.Uri,
        //        dataSize = loadData.DataSize,
        //        dataType = (SciterRequest.SciterResourceType)(uint)loadData.DataType,
        //        status = loadData.Status,
        //    };
        //}
        
        internal static DrawArgs ToEventArgs(this SciterBehaviors.DRAW_PARAMS @params)
        {
            return new DrawArgs
            {
                Area = new SciterRectangle(@params.area.Left, @params.area.Top, @params.area.Right, @params.area.Bottom),
                DrawEvent =  (DrawEvent)unchecked((int)@params.cmd),
                // Graphics = new SciterCore.SciterGraphics(@params.gfx),
                Handle = @params.gfx,
                Reserved = unchecked((int)@params.reserved),
            };
        }
        
        internal static KeyArgs ToEventArgs(this SciterBehaviors.KEY_PARAMS @params)
        {
            return new KeyArgs
            {
                Event = (KeyEvent)unchecked((int)@params.cmd),
                KeyboardState = (KeyboardStates)unchecked((int)@params.alt_state),
                KeyCode = unchecked((int)@params.key_code),
                TargetElement = @params.target.Equals(IntPtr.Zero) ? null : new SciterElement(@params.target)
            };
        }
        
        internal static MouseArgs ToEventArgs(this SciterBehaviors.MOUSE_PARAMS @params)
        {
            return new MouseArgs
            {
                Event = (MouseEvents)unchecked((int)@params.cmd),
                Cursor = (CursorType)unchecked((int)@params.cursor_type),
                ButtonState = (MouseButton)unchecked((int)@params.button_state),
                DragMode = (DraggingMode)unchecked((int)@params.dragging_mode),
                DragTarget = @params.dragging.Equals(IntPtr.Zero) ? null : new SciterElement(@params.dragging),
                ElementPosition = new SciterPoint(@params.pos.X, @params.pos.Y),
                ViewPosition = new SciterPoint(@params.pos_view.X, @params.pos_view.Y),
                KeyboardState = (KeyboardStates)unchecked((int)@params.alt_state),
                TargetElement = @params.target.Equals(IntPtr.Zero) ? null : new SciterElement(@params.target),
                IsOverIcon = @params.is_on_icon
            };
        }
        
        internal static ExchangeArgs ToEventArgs(this SciterBehaviors.EXCHANGE_PARAMS @params)
        {
            return new ExchangeArgs
            {
                Event = (ExchangeEvent)unchecked((int)@params.cmd),
                ElementPosition = new SciterPoint(@params.pos.X, @params.pos.Y),
                ViewPosition = new SciterPoint(@params.pos_view.X, @params.pos_view.Y),
                TargetElement = @params.target.Equals(IntPtr.Zero) ? null : new SciterElement(@params.target),
                SourceElement = @params.source.Equals(IntPtr.Zero) ? null : new SciterElement(@params.target),
                Mode = (DragAndDropMode)unchecked((int)@params.mode),
                Value = new SciterCore.SciterValue(@params.data)
            };
        }
        
        internal static ScrollEventArgs ToEventArgs(this SciterBehaviors.SCROLL_PARAMS @params)
        {
            return new ScrollEventArgs
            {
                Event = (ScrollEvents)unchecked((int)@params.cmd),
                TargetElement = @params.target.Equals(IntPtr.Zero) ? null : new SciterElement(@params.target),
                Position = @params.pos,
                Source = (ScrollSource)unchecked((int)@params.source),
                IsVertical = @params.vertical,
                Reason = unchecked((int)@params.reason)
            };
        }
        
        internal static FocusArgs ToEventArgs(this SciterBehaviors.FOCUS_PARAMS @params)
        {
            return new FocusArgs
            {
                Event = (FocusEvents)unchecked((int)@params.cmd),
                TargetElement = @params.target.Equals(IntPtr.Zero) ? null : new SciterElement(@params.target),
                Cancel = @params.cancel,
                IsMouseClick = @params.by_mouse_click
            };
        }
        
        internal static GestureArgs ToEventArgs(this SciterBehaviors.GESTURE_PARAMS @params)
        {
            return new GestureArgs
            {
                Event = (GestureEvent)unchecked((int)@params.cmd),
                TargetElement = @params.target.Equals(IntPtr.Zero) ? null : new SciterElement(@params.target),
                Flags = unchecked((int)@params.flags),
                DeltaTime = unchecked((int)@params.delta_time),
                DeltaV = @params.delta_v,
                ElementPosition = new SciterPoint(@params.pos.X, @params.pos.Y),
                ViewPosition = new SciterPoint(@params.pos_view.X, @params.pos_view.Y),
                DeltaXY = new SciterSize(@params.delta_xy.cx, @params.delta_xy.cy),
            };
        }
    }
}