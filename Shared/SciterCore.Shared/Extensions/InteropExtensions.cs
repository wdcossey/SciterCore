﻿using System;

namespace SciterCore.Interop
{
    internal static class InteropExtensions
    {
        internal static LoadDataEventArgs Convert(this SciterXDef.SCN_LOAD_DATA loadData)
        {
            return new LoadDataEventArgs
            {
                Code = loadData.code,
                Data = loadData.outData,
                Handle = loadData.hwnd,
                Initiator = loadData.initiator,
                Principal = loadData.principal,
                Uri = new Uri(loadData.uri, UriKind.RelativeOrAbsolute),
                DataSize = loadData.outDataSize,
                DataType = (SciterResourceType)unchecked((int)loadData.dataType),
                RequestId = loadData.requestId
            };
        }
        
        internal static DataLoadedEventArgs Convert(this SciterXDef.SCN_DATA_LOADED loadData)
        {
            return new DataLoadedEventArgs
            {
                Code = loadData.code,
                Data = loadData.data,
                Handle = loadData.hwnd,
                Uri = loadData.uri,
                DataSize = loadData.dataSize,
                DataType = (SciterResourceType)unchecked((int)loadData.dataType),
                Status = loadData.status,
            };
        }
        
        internal static SciterXDef.SCN_DATA_LOADED Convert(this DataLoadedEventArgs loadData)
        {
            return new SciterXDef.SCN_DATA_LOADED
            {
                code = loadData.Code,
                data = loadData.Data,
                hwnd = loadData.Handle,
                uri = loadData.Uri,
                dataSize = loadData.DataSize,
                dataType = (SciterRequest.SciterResourceType)(uint)loadData.DataType,
                status = loadData.Status,
            };
        }
        
        internal static DrawEventArgs Convert(this SciterBehaviors.DRAW_PARAMS @params)
        {
            return new DrawEventArgs
            {
                Area = new SciterRectangle(@params.area.Left, @params.area.Top, @params.area.Right, @params.area.Bottom),
                DrawEvent =  (DrawEvent)unchecked((int)@params.cmd),
                Handle = @params.gfx,
                Reserved = @params.reserved,
            };
        }
        
        internal static KeyEventArgs Convert(this SciterBehaviors.KEY_PARAMS @params)
        {
            return new KeyEventArgs
            {
                Event = (KeyEvent)unchecked((int)@params.cmd),
                KeyboardState = (KeyboardStates)unchecked((int)@params.alt_state),
                KeyCode = unchecked((int)@params.key_code),
                TargetElement = @params.target.Equals(IntPtr.Zero) ? null : new SciterElement(@params.target)
            };
        }
        
        internal static MouseEventArgs Convert(this SciterBehaviors.MOUSE_PARAMS @params)
        {
            return new MouseEventArgs
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
        
        internal static ExchangeEventArgs Convert(this SciterBehaviors.EXCHANGE_PARAMS @params)
        {
            return new ExchangeEventArgs
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
    }
}