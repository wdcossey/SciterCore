using System;

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
                DrawEvent =  (DrawEvent)(int)@params.cmd,
                Handle = @params.gfx,
                Reserved = @params.reserved,
            };
        }
    }
}